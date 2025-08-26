using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using Opc.UaFx.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Lightweight helper around <see cref="OpcClient"/> for connecting, subscribing and reading/writing nodes.
    /// </summary>
    public class OPCClientHelper
    {
        private readonly object _lock = new object();
        private OpcClient _client;

        /// <summary>
        /// Backing setting for target server (host/port and credentials).
        /// </summary>
        public OPCSetting _opcSetting;

        private bool _clientConnect;

        /// <summary>
        /// Controls the connection state. Setting to <c>true</c> ensures a connected client;
        /// setting to <c>false</c> disposes the client and clears subscriptions.
        /// </summary>
        public bool clientConnect
        {
            get
            {
                lock (_lock) { return _clientConnect; }
            }
            set
            {
                lock (_lock)
                {
                    if (value)
                        EnsureClientConnected_NoThrow();
                    else
                        DisposeClient_NoThrow();

                    _clientConnect = value;
                }
            }
        }

        /// <summary>
        /// Exposes the underlying OPC UA client instance (read-only).
        /// </summary>
        public OpcClient client
        {
            get { lock (_lock) { return _client; } }
            private set { lock (_lock) { _client = value; } }
        }

        /// <summary>
        /// Creates a new helper for the given OPC settings.
        /// </summary>
        public OPCClientHelper(OPCSetting opcSetting)
        {
            _opcSetting = opcSetting;
        }

        /// <summary>
        /// Ensures there is a connected client (creates and connects if missing).
        /// </summary>
        private void EnsureClientConnected_NoThrow()
        {
            lock (_lock)
            {
                if (_client != null && _client.State == OpcClientState.Connected)
                    return;

                try
                {
                    DisposeClient_NoThrow();
                    CreateClient_NoThrow();
                    _client.Connect();
                }
                catch
                {
                    // keep silent to not crash callers; next attempt will try again
                }
            }
        }

        /// <summary>
        /// Disposes the client and unsubscribes handlers.
        /// </summary>
        private void DisposeClient_NoThrow()
        {
            try
            {
                if (_client != null)
                {
                    try
                    {
                        _client.Connected -= Client_Connected;
                        _client.StateChanged -= Client_StateChanged;
                        _client.SubscriptionsChanged -= Client_SubscriptionsChanged;
                    }
                    catch { /* ignore */ }

                    try { if (_client.State == OpcClientState.Connected) _client.Disconnect(); } catch { /* ignore */ }
                    try { _client.Dispose(); } catch { /* ignore */ }
                }
            }
            catch { /* ignore */ }
            finally
            {
                _client = null;
            }
        }

        /// <summary>
        /// Creates an <see cref="OpcClient"/> and wires events.
        /// </summary>
        private void CreateClient_NoThrow()
        {
            try
            {
                // Common pattern in UA FX is an endpoint URL like "opc.tcp://host:port"
                // Your settings appear to store "host" and "port"; concatenate as you did.
                var endpoint = string.Concat(_opcSetting.EndPoint, ":", _opcSetting.PortNumber);

                var c = new OpcClient(endpoint)
                {
                    UseDynamic = false,
                    SessionTimeout = 2000,   // ms
                    ReconnectTimeout = 60000 // ms
                };

                // Credentials (if required by the server)
                if (!string.IsNullOrEmpty(_opcSetting.UserName))
                {
                    c.Security.UserIdentity = new OpcClientIdentity(_opcSetting.UserName, _opcSetting.Password);
                }

                c.Connected += Client_Connected;
                c.StateChanged += Client_StateChanged;
                c.SubscriptionsChanged += Client_SubscriptionsChanged;

                _client = c;
            }
            catch
            {
                _client = null;
            }
        }

        // ------------------- client event handlers -------------------

        private void Client_Connected(object sender, EventArgs e)
        {
            try
            {
                // Build all subscriptions based on currently opened operator forms.
                var subscribes = new List<OpcSubscribeDataChange>();

                // Snapshot the forms list to avoid concurrent modification while iterating.
                var forms = ToolsMdiManager.frmOperators != null
                    ? ToolsMdiManager.frmOperators.ToList()
                    : new List<FrmOperator>();

                foreach (var opwc in forms)
                {
                    if (StaticValues.Restart)
                    {
                        // On restart push counter reset and machine control
                        WriteNode(opwc.panelDetail.OPCNodeIdCounterReset, true);
                        var noShopOrders = !opwc.vw_ShopOrderGridModels.HasEntries();
                        WriteNode(opwc.panelDetail.OPCNodeIdMachineControl, noShopOrders);
                    }

                    // Quantity & non-shop-order counters (except square meter)
                    if (opwc.panelDetail.UnitTypeID != StaticValues.specialCodeUnitSquareMeter.Id)
                    {
                        if (!string.IsNullOrEmpty(opwc.panelDetail.OPCNodeIdQuantity))
                            subscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNodeIdQuantity, HandleDataChanged));

                        if (!string.IsNullOrEmpty(opwc.panelDetail.OPCNotShopOrderCounter))
                            subscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNotShopOrderCounter, HandleDataChanged));
                    }

                    // Start/Stop
                    if (!string.IsNullOrEmpty(opwc.panelDetail.OPCNodeIdStartStop))
                        subscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNodeIdStartStop, HandleDataChanged));

                    // Scrap count
                    if (!string.IsNullOrWhiteSpace(opwc.panelDetail.OPCNodeIdScruptCount))
                        subscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCNodeIdScruptCount, HandleDataChanged));

                    // Cycle no-production
                    if (!string.IsNullOrWhiteSpace(opwc.panelDetail.OPCCycleNoProductionSignal))
                        subscribes.Add(new OpcSubscribeDataChange(opwc.panelDetail.OPCCycleNoProductionSignal, HandleDataChanged));

                    // Additional custom reads
                    foreach (var setting in opwc.opcOtherReadModels)
                    {
                        if (!string.IsNullOrEmpty(setting.NodeId))
                            subscribes.Add(new OpcSubscribeDataChange(setting.NodeId, HandleDataChanged));
                    }
                }

                StaticValues.Restart = false;

                // Subscribe all at once
                var c = client; // property (already locked inside getter)
                if (c != null && c.State == OpcClientState.Connected && subscribes.Count > 0)
                {
                    c.SubscribeNodes(subscribes.ToArray());
                }
            }
            catch
            {
                // Swallow to avoid bringing down UI; re-subscribe will be tried on next connect
            }
        }

        private void Client_StateChanged(object sender, OpcClientStateChangedEventArgs e)
        {
            // You can log or inspect transitions here if you want.
            // e.OldState, e.NewState
        }

        private void Client_SubscriptionsChanged(object sender, EventArgs e)
        {
            // Hook if you need to refresh bindings upon subscription changes
        }

        // ------------------- subscription callback -------------------

        private void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
        {
            try
            {
                var c = client;
                if (c == null || c.State != OpcClientState.Connected)
                {
                    EnsureClientConnected_NoThrow();
                    return;
                }

                var monitoredItem = sender as OpcMonitoredItem;
                if (monitoredItem == null) return;

                var nodeId = monitoredItem.NodeId != null ? monitoredItem.NodeId.ToString() : null;
                if (string.IsNullOrEmpty(nodeId)) return;

                var value = e.Item.Value.ConvertDecimalExtension();
                if (value == null) return;

                // Snapshot forms to avoid concurrent modification
                var forms = ToolsMdiManager.frmOperators != null
                    ? ToolsMdiManager.frmOperators.ToList()
                    : new List<FrmOperator>();

                foreach (var frm in forms)
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
            catch
            {
                // Intentionally ignored; the next change will try again
            }
        }

        // ------------------- public operations -------------------

        /// <summary>
        /// Writes <c>true</c> to the specified node (e.g., to reset a counter).
        /// </summary>
        public void ResetCounter(string nodeId) => WriteNode(nodeId, true);

        /// <summary>
        /// Writes a machine lock/unlock boolean to the specified node.
        /// </summary>
        public void MachineLock(string nodeId, bool value) => WriteNode(nodeId, value);

        /// <summary>
        /// Adds to a counter node by writing a <see cref="float"/> value (with small retry).
        /// </summary>
        public void AddCounter(string nodeId, float value)
        {
            WriteNodeCore(nodeId, value, retries: 3);
        }

        /// <summary>
        /// Reads a node’s value as string (empty string on failure).
        /// </summary>
        public string ReadNode(string NodeId)
        {
            try
            {
                if (string.IsNullOrEmpty(NodeId)) return string.Empty;
                EnsureClientConnected_NoThrow();
                var c = client;
                if (c == null) return string.Empty;

                var dv = c.ReadNode(NodeId);
                return dv != null && dv.Value != null ? dv.Value.ToString() : string.Empty;
            }
            catch { return string.Empty; }
        }

        /// <summary>Writes a boolean value to a node.</summary>
        public void WriteNode(string NodeId, bool value) => WriteNodeCore(NodeId, value);

        /// <summary>Writes a UInt16 value to a node.</summary>
        public void WriteNode(string NodeId, UInt16 value) => WriteNodeCore(NodeId, value);

        /// <summary>Writes a UInt32 value to a node.</summary>
        public void WriteNode(string NodeId, UInt32 value) => WriteNodeCore(NodeId, value);

        // ------------------- run mode client (static) -------------------

        private static readonly object _runModeLock = new object();
        private static OpcClient _runModeClient;

        /// <summary>
        /// Lazily provides a connected client for RunMode writes (separate session).
        /// </summary>
        public static OpcClient runModeClient
        {
            get
            {
                lock (_runModeLock)
                {
                    try
                    {
                        if (_runModeClient == null || _runModeClient.State != OpcClientState.Connected)
                        {
                            var endpoint = string.Concat(StaticValues.opcSetting.EndPoint, ":", StaticValues.opcSetting.PortNumber);
                            var c = new OpcClient(endpoint)
                            {
                                UseDynamic = false
                            };
                            if (!string.IsNullOrEmpty(StaticValues.opcSetting.UserName))
                            {
                                c.Security.UserIdentity = new OpcClientIdentity(StaticValues.opcSetting.UserName, StaticValues.opcSetting.Password);
                            }
                            c.Connect();
                            _runModeClient = c;
                        }
                    }
                    catch { /* keep null if connect fails */ }

                    return _runModeClient;
                }
            }
        }

        /// <summary>
        /// Writes a RunMode code to the provided node. Optionally closes the separate client after write.
        /// </summary>
        public void WriteRunMode(UInt16 mode, string NodeID, bool closeFlag = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(NodeID))
                    runModeClient?.WriteNode(NodeID, mode);
            }
            catch { }
            finally
            {
                if (closeFlag)
                {
                    lock (_runModeLock)
                    {
                        if (_runModeClient != null)
                        {
                            try { _runModeClient.Disconnect(); } catch { }
                            try { _runModeClient.Dispose(); } catch { }
                            _runModeClient = null;
                        }
                    }
                }
            }
        }

        // ------------------- internals -------------------

        /// <summary>
        /// Core writer with small retry and on-demand connect.
        /// </summary>
        private void WriteNodeCore(string nodeId, object value, int retries = 2)
        {
            if (string.IsNullOrEmpty(nodeId)) return;

            try
            {
                EnsureClientConnected_NoThrow();
                var c = client;
                if (c == null) return;

                for (int i = 0; i < Math.Max(1, retries); i++)
                {
                    var status = c.WriteNode(nodeId, value);
                    if (status != null && status.IsGood) break;
                }
            }
            catch
            {
                // ignore; next call can retry
            }
        }
    }
}
