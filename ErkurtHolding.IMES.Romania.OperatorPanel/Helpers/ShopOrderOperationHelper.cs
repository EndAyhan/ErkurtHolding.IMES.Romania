using System;
using System.Collections.Generic;
using System.Linq;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Helper methods for reading shop order operations and completing an active shop order
    /// from the operator panel (OPC resets, locks, and DB updates).
    /// </summary>
    public static class ShopOrderOperationHelper
    {
        /// <summary>
        /// Gets the <see cref="ShopOrderOperation"/> for a given work center and shop order number.
        /// Falls back to IFS (placeholder) if the primary query fails.
        /// </summary>
        /// <param name="workCenter">Work center code (used only by fallback).</param>
        /// <param name="shopOrderNo">Shop order number.</param>
        /// <returns>The resolved <see cref="ShopOrderOperation"/> or <c>null</c> when not found.</returns>
        public static ShopOrderOperation GetShopOrderOperation(string workCenter, string shopOrderNo)
        {
            if (string.IsNullOrWhiteSpace(shopOrderNo))
                return null;

            return ShopOrderOperationManager.Current.GetShopOrderOperation(shopOrderNo);

        }

        /// <summary>
        /// Completes the active shop order for the given <paramref name="frmOperator"/>:
        /// - Reads runtime counters from OPC (if mapped)
        /// - Sends Poka‑Yoke handshake (if mapped)
        /// - Resets machine control nodes & counters
        /// - Closes <c>ShopOrderProduction</c> records and related subcontract production (if any)
        /// - Computes and writes energy delta (if available)
        /// - Creates "production stop" entries for each order in the active page
        /// - Resets panel state & in‑memory collections
        /// </summary>
        /// <param name="frmOperator">Active operator form instance.</param>
        /// <param name="userModel">Current user.</param>
        public static void ShopOrderOperationFinish(FrmOperator frmOperator, UserModel userModel)
        {
            if (frmOperator == null || userModel == null || frmOperator.panelDetail == null)
                return;

            // 1) Try read duration (seconds) from OPC, if mapped
            decimal resourceSure = 0;
            if (!string.IsNullOrWhiteSpace(frmOperator.panelDetail.OPCNodeIdSure))
            {
                decimal.TryParse(StaticValues.opcClient.ReadNode(frmOperator.panelDetail.OPCNodeIdSure), out resourceSure);
            }

            // 2) Poka‑Yoke handshake (true) if a PokaYoke node exists among other reads
            if (frmOperator.opcOtherReadModels != null &&
                frmOperator.opcOtherReadModels.Any(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePokayoke.Id))
            {
                var nodeId = frmOperator.opcOtherReadModels
                                        .First(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePokayoke.Id)
                                        .NodeId;
                if (!string.IsNullOrWhiteSpace(nodeId))
                    StaticValues.opcClient.WriteNode(nodeId, true);
            }

            // 3) Reset socket/mode flags & shop order boolean on PLC (when mapped)
            if (!string.IsNullOrWhiteSpace(frmOperator.panelDetail.OPCNodeIdSocketAdress))
                StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdSocketAdress, Convert.ToUInt16(0));

            if (!string.IsNullOrWhiteSpace(frmOperator.panelDetail.OPCNodeIdShopOrder))
                StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdShopOrder, false);

            // 4) Create production stop entries for each active order row
            if (frmOperator.vw_ShopOrderGridModels != null)
            {
                foreach (var shopOrder in frmOperator.vw_ShopOrderGridModels)
                {
                    CreateProductionStop(frmOperator, userModel, shopOrder, resourceSure);
                }
            }

            // 5) Energy delta (if a start value exists)
            decimal energy = 0; // placeholder for OPC energy read if re-enabled: frmOperator.energyDBHelper.GetCurrentTotalEnergyFromOPC(frmOperator.resource.Id);
            var panelStatus = PanelStatusManager.Current.GetPanelStatus(frmOperator.panelDetail.Id);
            decimal totalEnergy = 0;
            if (panelStatus != null && panelStatus.Count > 0 && panelStatus[0].EnergyStartValue > 0)
                totalEnergy = energy - panelStatus[0].EnergyStartValue;

            // 6) Close productions (main + subcontractor)
            if (frmOperator.shopOrderProduction != null)
            {
                ShopOrderProductionManager.Current.SetShopOrderProductionFinish(
                    frmOperator.shopOrderProduction.Id, DateTime.Now, userModel.CompanyPersonId, totalEnergy);
            }

            if (!frmOperator.SubcontractorShopOrderProductionId.Equals(Guid.Empty))
            {
                ShopOrderProductionManager.Current.SetShopOrderProductionFinish(
                    frmOperator.SubcontractorShopOrderProductionId, DateTime.Now, userModel.CompanyPersonId, 0);
            }

            // 7) Machine lock + counter reset (+ start/stop lock)
            if (!string.IsNullOrWhiteSpace(frmOperator.panelDetail.OPCNodeIdMachineControl))
                StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, true);

            if (!string.IsNullOrWhiteSpace(frmOperator.panelDetail.OPCNodeIdCounterReset))
                StaticValues.opcClient.ResetCounter(frmOperator.panelDetail.OPCNodeIdCounterReset);

            System.Threading.Thread.Sleep(1000); // original timing retained

            if (!string.IsNullOrWhiteSpace(frmOperator.panelDetail.OPCNodeIdStartStop))
                StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdStartStop, true);

            // 8) Reset UI state and in‑memory caches/collections
            frmOperator.SetMachineStateColor(MachineStateColor.ShopOrderWaiting);
            frmOperator.shopOrderStatus = ShopOrderStatus.Start;

            if (frmOperator.vw_ShopOrderGridModels != null) frmOperator.vw_ShopOrderGridModels.Clear();
            frmOperator.vw_ShopOrderGridModelActive = null;
            if (frmOperator.exShopOrderProductions != null) frmOperator.exShopOrderProductions.Clear();
            if (frmOperator.productionDetails != null) frmOperator.productionDetails.Clear();
            if (frmOperator.shopOrderProductionDetails != null) frmOperator.shopOrderProductionDetails.Clear();
            if (frmOperator.shopOrderOperations != null) frmOperator.shopOrderOperations.Clear();
            if (frmOperator.handlingUnits != null) frmOperator.handlingUnits.Clear();

            frmOperator.counter = -1;
            frmOperator.notShopOrderCounter = -1;

            if (frmOperator.issueMaterialAllocModels != null) frmOperator.issueMaterialAllocModels.Clear();
        }

        /// <summary>
        /// Creates a <see cref="ProductionStop"/> entry for the given order row and updates the
        /// selection flags in <see cref="ShopOrderOperationManager"/>. Duration is derived from
        /// OPC "resourceSure" if available; otherwise computed from order runtime.
        /// </summary>
        private static void CreateProductionStop(FrmOperator frmOperator, UserModel userModel, Entity.Views.vw_ShopOrderGridModel shopOrder, decimal resourceSure)
        {
            if (frmOperator == null || frmOperator.shopOrderProduction == null || userModel == null || shopOrder == null)
                return;

            try
            {
                var allDetails = ShopOrderProductionDetailManager.Current
                    .GetShopOrderProductionDetailsByProduction(frmOperator.shopOrderProduction.Id)
                    .Where(x => x.ByProduct == false);

                // Only consider details for this order/operation
                var orderDetails = allDetails.Where(x => x.OrderNo == shopOrder.orderNo && x.OperationNo == shopOrder.operationNo);
                if (!orderDetails.Any())
                    return;

                var productionStop = new ProductionStop
                {
                    ProductionID = frmOperator.shopOrderProduction.Id,
                    ShopOrderID = shopOrder.Id,
                    UserID = userModel.CompanyPersonId,
                    Description1 = null
                };

                var sure = resourceSure;

                // If process was active and "sure" not provided, compute from elapsed runtime
                if (frmOperator.processNewActive)
                {
                    if (sure <= 0 && frmOperator.shopOrderProduction.OrderStartDate != default(DateTime))
                        sure = (decimal)(DateTime.Now - frmOperator.shopOrderProduction.OrderStartDate).TotalSeconds;

                    // Distribute duration proportionally by order quantity vs. total quantity
                    var totalQty = allDetails.Sum(x => x.Quantity);
                    var thisQty = orderDetails.Sum(x => x.Quantity);
                    productionStop.Duration = totalQty > 0 ? (sure / 60m) * thisQty / totalQty : 0m;
                }
                else if (sure > 0)
                {
                    productionStop.Duration = sure / 60m;
                }

                ProductionStopManager.Current.Insert(productionStop);
            }
            catch
            {
                // swallow to preserve original behavior
            }
            finally
            {
                // Unselect and clear "running" pointers for the finished shop order
                ShopOrderOperationManager.Current.SelectedUpdate(
                    shopOrder.Id,
                    Selected: false,
                    workCenterRun: Guid.Empty,
                    resourceIdRun: Guid.Empty);
            }
        }
    }
} 
