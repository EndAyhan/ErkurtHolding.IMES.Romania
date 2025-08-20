using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using Opc.UaFx.Client;
using System.Collections.Generic;
using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class OPCClientHelper
    {
        public OPCSetting _opcSetting;
        private readonly object _lock = new object();
        public OpcClient client;

        private bool _clientConnect;
        public bool clientConnect
        {
            get
            {
                lock (_lock)
                {
                    return _clientConnect;
                }
            }
            set
            {
                lock (_lock)
                {
                    if (value)
                        EnsureClientConnected();
                    else
                        DisposeClient();

                    _clientConnect = value;
                }
            }
        }

        public OPCClientHelper(OPCSetting opcSetting)
        {
            _opcSetting = opcSetting;
        }

        private void EnsureClientConnected()
        {
            lock (_lock)
            {
                if (client == null || client.State != OpcClientState.Connected)
                {
                    try
                    {
                        DisposeClient();
                        CreateClient();
                        client.Connect();
                    }
                    catch { }
                }
            }
        }

        private void DisposeClient()
        {
            try
            {
                client?.Disconnect();
                client?.Dispose();
                client = null;
            }
            catch { }
        }

        private void CreateClient()
        {
            try
            {
                client = new OpcClient($"{_opcSetting.EndPoint}:{_opcSetting.PortNumber}")
                {
                    Security = { UserIdentity = new OpcClientIdentity(_opcSetting.UserName, _opcSetting.Password) },
                    UseDynamic = false,
                    SessionTimeout = 2000,
                    ReconnectTimeout = 60000
                };

                client.Connected += Client_Connected;
                client.StateChanged += Client_StateChanged;
                client.SubscriptionsChanged += Client_SubscriptionsChanged;
            }
            catch { }
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            try
            {
                var opcSubscribes = new List<OpcSubscribeDataChange>();

                foreach (var opwc in ToolsMdiManager.frmOperators)
                {
                    if (StaticValues.Restart)
                    {
                        WriteNode(opwc.panelDetail.OPCNodeIdCounterReset, true);
                        bool machineControl = !opwc.vw_ShopOrderGridModels.HasEntries();
                        WriteNode(opwc.panelDetail.OPCNodeIdMachineControl, machineControl);
                    }

                    if (opwc.panelDetail.UnitTypeID != StaticValues.specialCodeUnitSquareMeter.Id)
                    {
                        opcSubscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNodeIdQuantity, HandleDataChanged));
                        if (opwc.panelDetail.OPCNotShopOrderCounter != null)
                            opcSubscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNotShopOrderCounter, HandleDataChanged));
                    }



                    opcSubscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNodeIdStartStop, HandleDataChanged));

                    if (!string.IsNullOrWhiteSpace(opwc.panelDetail.OPCNodeIdScruptCount))
                        opcSubscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNodeIdScruptCount, HandleDataChanged));
                    if (!string.IsNullOrWhiteSpace(opwc.panelDetail.OPCCycleNoProductionSignal))
                        opcSubscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCCycleNoProductionSignal, HandleDataChanged));

                    foreach (var setting in opwc.opcOtherReadModels)
                        opcSubscribes.Add(new OpcSubscribeDataChange(setting.NodeId, HandleDataChanged));
                }

                StaticValues.Restart = false;
                client.SubscribeNodes(opcSubscribes.ToArray());
            }
            catch { }
        }

        private void Client_StateChanged(object sender, OpcClientStateChangedEventArgs e) { }

        private void Client_SubscriptionsChanged(object sender, EventArgs e) { }

        private void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
        {
            try
            {
                if (client.State != OpcClientState.Connected)
                {
                    EnsureClientConnected();
                    return;
                }

                var monitoredItem = (OpcMonitoredItem)sender;
                var nodeId = monitoredItem.NodeId.ToString();
                var value = e.Item.Value.ConvertDecimalExtension();

                if (value == null) return;

                foreach (var frm in ToolsMdiManager.frmOperators)
                {
                    if (frm.panelDetail.OPCNodeIdQuantity == nodeId)
                        frm.counter = (decimal)value;
                    else if (frm.panelDetail.OPCNodeIdScruptCount == nodeId)
                        frm.scrapCounter = (decimal)value;
                    else if (frm.panelDetail.OPCNotShopOrderCounter == nodeId)
                        frm.notShopOrderCounter = (decimal)value;
                    else if (frm.panelDetail.OPCCycleNoProductionSignal == nodeId)
                        frm.cycleNoProductionSignal = (decimal)value;
                    else
                    {
                        var opcSetting = frm.opcOtherReadModels.FirstOrDefault(s => s.NodeId == nodeId);
                        if (opcSetting != null)
                            opcSetting.readValue = (decimal)value;
                    }
                }
            }
            catch { }
        }

        public void ResetCounter(string nodeId) => WriteNode(nodeId, true);

        public void MachineLock(string nodeId, bool value) => WriteNode(nodeId, value);

        public void AddCounter(string nodeId, float value)
        {
            try
            {
                EnsureClientConnected();
                for (int i = 0; i < 3; i++)
                {
                    var status = client.WriteNode(nodeId, value);
                    if (status.IsGood) break;
                }
            }
            catch { }
        }

        public string ReadNode(string NodeId)
        {
            try
            {
                EnsureClientConnected();
                return client?.ReadNode(NodeId).Value?.ToString() ?? string.Empty;
            }
            catch { return string.Empty; }
        }

        public void WriteNode(string NodeId, bool value)
        {
            try
            {
                EnsureClientConnected();
                client?.WriteNode(NodeId, value);
            }
            catch { }
        }

        public void WriteNode(string NodeId, UInt16 value)
        {
            try
            {
                EnsureClientConnected();
                client?.WriteNode(NodeId, value);
            }
            catch { }
        }

        public void WriteNode(string NodeId, UInt32 value)
        {
            try
            {
                EnsureClientConnected();
                client?.WriteNode(NodeId, value);
            }
            catch { }
        }

        private static OpcClient _runModeClient;
        public static OpcClient runModeClient
        {
            get
            {
                try
                {
                    if (_runModeClient == null || _runModeClient.State != OpcClientState.Connected)
                    {
                        _runModeClient = new OpcClient($"{StaticValues.opcSetting.EndPoint}:{StaticValues.opcSetting.PortNumber}")
                        {
                            Security = { UserIdentity = new OpcClientIdentity(StaticValues.opcSetting.UserName, StaticValues.opcSetting.Password) },
                            UseDynamic = false
                        };
                        _runModeClient.Connect();
                    }
                }
                catch { }
                return _runModeClient;
            }
        }

        public void WriteRunMode(UInt16 mode, string NodeID, bool closeFlag = false)
        {
            try
            {
                runModeClient?.WriteNode(NodeID, mode);
            }
            catch { }
            finally
            {
                if (closeFlag && _runModeClient != null)
                {
                    try
                    {
                        _runModeClient.Disconnect();
                        _runModeClient.Dispose();
                        _runModeClient = null;
                    }
                    catch { }
                }
            }
        }
    }

}
