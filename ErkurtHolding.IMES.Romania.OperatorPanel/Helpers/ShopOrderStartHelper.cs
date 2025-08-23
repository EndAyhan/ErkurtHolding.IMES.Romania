using System;
using System.Linq;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Helpers used when starting shop orders: pre-start checks, printer/label setup, OPC run mode, and preloading history.
    /// </summary>
    public static class ShopOrderStartHelper
    {
        /// <summary>
        /// Checks whether a shop order can start, based on material allocations and optional wait-time logic.
        /// If an allocation has a numeric wait-time (alan1, in hours) &gt; 0, only material that has been
        /// available longer than that time window is considered toward the required quantity.
        /// </summary>
        /// <param name="product">The product (not used directly by current check; reserved for future).</param>
        /// <param name="shopOrder">The shop order row to validate.</param>
        /// <returns><c>true</c> if start is allowed; otherwise <c>false</c>.</returns>
        public static bool ShopOrderStartControl(Product product, vw_ShopOrderGridModel shopOrder)
        {
            if (shopOrder == null)
                return true; // no order to validate => allow

            var resultMaterialAlloc = ShopMaterialAllocManager.Current.GetlistByOrderID(shopOrder.Id);
            if (resultMaterialAlloc == null || resultMaterialAlloc.Count == 0)
                return true;

            bool allowStart = true;

            foreach (var materialAlloc in resultMaterialAlloc)
            {
                // alan1: wait-time (hours)
                int waitHours;
                if (!int.TryParse(materialAlloc.alan1, out waitHours) || waitHours <= 0)
                    continue;

                double totalEligibleQty = 0d;

                var stocks = InventoryStockManager.Current.GetInventoryStock(materialAlloc.PartID, StaticValues.branch.Id);
                if (stocks.HasEntries())
                {
                    foreach (var inv in stocks)
                    {
                        // Try to resolve handling unit by lot/batch
                        var hu = HandlingUnitManager.Current.GetHandlingUnitByBarcodeOrSerial(inv.lotBatchNo);

                        if (hu != null)
                        {
                            // Count only HU quantity older than the configured wait window
                            var hoursSinceCreated = (DateTime.Now - hu.CreatedAt).TotalHours;
                            if (hoursSinceCreated > waitHours)
                                totalEligibleQty += (double)hu.Quantity;
                        }
                        else
                        {
                            // If no HU found, fall back to available stock quantity
                            totalEligibleQty += inv.availableQty;
                        }
                    }

                    // If eligible qty does not cover required, we must block start
                    if (totalEligibleQty <= materialAlloc.qtyRequired)
                    {
                        allowStart = false;
                        break;
                    }
                }
            }

            return allowStart;
        }

        /// <summary>
        /// Prepares the label print models for the current panel settings (product/process/box),
        /// resolving printers and layouts via <see cref="PrintLabelHelper"/>.
        /// </summary>
        /// <param name="frmOperator">Active operator form.</param>
        /// <param name="product">Current product.</param>
        /// <param name="panelDetail">Panel configuration.</param>
        public static void PrintAndLabelSettings(FrmOperator frmOperator, Product product, PanelDetail panelDetail)
        {
            if (frmOperator == null || product == null || panelDetail == null)
                return;

            // Product label
            if (panelDetail.PrintProductBarcode)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Product));

            // Process label
            if (panelDetail.ProcessBarcode)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Process));

            // Box label (original logic preserved):
            // - when BoxFillsUp == false and Scales == false -> add box label
            // - when BoxFillsUp == true -> add box label
            if (!panelDetail.BoxFillsUp && !frmOperator.panelDetail.Scales)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Box));
            if (panelDetail.BoxFillsUp)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Box));
        }

        /// <summary>
        /// Sets the PLC run mode via OPC according to the operation number (10 =&gt; 40, otherwise 50).
        /// Sends a pre-command 45, waits 1 second, then sends the final code, closing the session on the last write.
        /// </summary>
        /// <param name="frmOperator">Active operator form.</param>
        /// <param name="flag">If <c>true</c>, the method attempts to set run mode and returns <c>false</c> when done.</param>
        /// <param name="panelDetail">Panel configuration (not used here; reserved).</param>
        /// <param name="shopOrder">Order row to decide final run mode (opNo 10=&gt;40; else 50).</param>
        /// <returns><c>false</c> if run mode was set; the original <paramref name="flag"/> otherwise.</returns>
        public static bool SetMachineRunMode(FrmOperator frmOperator, bool flag, PanelDetail panelDetail, vw_ShopOrderGridModel shopOrder)
        {
            if (!flag || frmOperator == null || shopOrder == null)
                return flag;

            if (frmOperator.opcOtherReadModels != null &&
                frmOperator.opcOtherReadModels.Any(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePlcRunModeParameter.Id))
            {
                var runModeModel = frmOperator.opcOtherReadModels
                    .First(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePlcRunModeParameter.Id);

                if (!string.IsNullOrWhiteSpace(runModeModel.NodeId))
                {
                    // Session “pre” code
                    StaticValues.opcClient.WriteRunMode(45, runModeModel.NodeId);

                    System.Threading.Thread.Sleep(1000);

                    // Final mode by opNo
                    if (shopOrder.operationNo == 10)
                        StaticValues.opcClient.WriteRunMode(40, runModeModel.NodeId, true);
                    else
                        StaticValues.opcClient.WriteRunMode(50, runModeModel.NodeId, true);

                    flag = false;
                }
            }

            return flag;
        }

        /// <summary>
        /// Prepares in-memory collections for the selected order:
        /// - Adds the order operation to the form
        /// - Marks the order as selected/running (work center &amp; resource)
        /// - Loads existing production details (OK and By-Product) and handling units for the order
        /// </summary>
        /// <param name="frmOperator">Active operator form.</param>
        /// <param name="shopOrder">Order row to attach.</param>
        /// <param name="selected">Whether to mark the order as selected/running.</param>
        public static void ShopOrderProductionDetailSettings(FrmOperator frmOperator, vw_ShopOrderGridModel shopOrder, bool selected = true)
        {
            if (frmOperator == null || shopOrder == null)
                return;

            // Attach full operation entity
            var selectedShopOrder = ShopOrderOperationManager.Current.GetShopOrderOperationById(shopOrder.Id);
            if (selectedShopOrder != null)
                frmOperator.shopOrderOperations.Add(selectedShopOrder);

            // Mark as running here
            ShopOrderOperationManager.Current.SelectedUpdate(
                shopOrder.Id,
                Selected: selected,
                workCenterRun: frmOperator.machine != null ? frmOperator.machine.Id : Guid.Empty,
                resourceIdRun: frmOperator.resource != null ? frmOperator.resource.Id : Guid.Empty);

            // If previously produced, preload details & handling units into the form
            var shopOrderProductions = ShopOrderProductionManager.Current.GetShopOrderProductionByShopOrderID(shopOrder.Id);

            if (shopOrderProductions.HasEntries())
            {
                var resultDetails = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetails(shopOrder.Id);
                if (resultDetails != null && resultDetails.Count > 0)
                {
                    frmOperator.productionDetails.AddRange(resultDetails.Where(x => x.ByProduct == false));
                    frmOperator.productionDetailsByProducts.AddRange(resultDetails.Where(x => x.ByProduct == true));
                }

                var resultHandlingUnits = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(shopOrder.Id);
                if (resultHandlingUnits != null && resultHandlingUnits.Count > 0)
                    frmOperator.handlingUnits.AddRange(resultHandlingUnits);
            }
        }

        /// <summary>
        /// Creates and inserts a new <see cref="ShopOrderProduction"/> header for the given work center/resource.
        /// </summary>
        /// <param name="workCenterID">Work center (machine) id.</param>
        /// <param name="resourceID">Resource id.</param>
        /// <param name="userModel">Current user.</param>
        /// <param name="process">Whether this is a process production.</param>
        /// <returns>The inserted <see cref="ShopOrderProduction"/>.</returns>
        public static ShopOrderProduction InsertShopOrderProduction(Guid workCenterID, Guid resourceID, UserModel userModel, bool process = false)
        {
            try
            {
                var shopOrderProduction = new ShopOrderProduction
                {
                    ShopOrderID = Guid.Empty,
                    OrderStartDate = DateTime.Now,
                    SetupStartDate = DateTime.Now,
                    OrderFinishDate = DateTime.Now,
                    SetupFinishDate = DateTime.Now,
                    QuantityScrap = 0,
                    QuantityOk = 0,
                    QuantityNok = 0,
                    WorkCenterID = workCenterID,
                    ResourceID = resourceID,
                    StartCompanyPersonID = userModel != null ? userModel.CompanyPersonId : Guid.Empty,
                    Process = process,
                    TotalEnergyConsumed = 0
                };

                var result = ShopOrderProductionManager.Current.Insert(shopOrderProduction);
                return result.ListData[0];
            }
            catch
            {
                throw;
            }
        }
    }
}
