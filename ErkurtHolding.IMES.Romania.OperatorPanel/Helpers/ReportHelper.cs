using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// High-level helper for composing report/label data models (product & handling unit)
    /// and persisting related entities (HandlingUnit, ProductionDetail updates).
    /// 
    /// Notes:
    /// - Thread safety: critical write sections guarded with a static lock.
    /// - Localization: keep existing MessageTextHelper usage; enum .ToText() uses StaticValues.T internally.
    /// - Important fix: BoxID is assigned to details only AFTER the handling unit has been inserted, 
    ///   so we use the actual generated ID (not Guid.Empty).
    /// </summary>
    public static class ReportHelper
    {
        // single in-process gate for critical sections that create/update shared entities
        private static readonly object _gate = new object();

        /// <summary>
        /// Builds a <see cref="ReportHandlingUnitHelper"/> for the given order row and creates a new <see cref="HandlingUnit"/>.
        /// It also assigns all unboxed production details (for this operation) to the new handling unit and updates barcodes.
        /// </summary>
        public static void GetReportHandlingUnit(vw_ShopOrderGridModel vwshopOrderGridModel)
        {
            if (vwshopOrderGridModel == null) return;
            if (ToolsMdiManager.frmOperatorActive == null) return;

            lock (_gate)
            {
                var helper = GetReportHandling(vwshopOrderGridModel);

                // take details for this operation which are not in a box yet
                helper.shopOrderProductionDetails =
                    ToolsMdiManager.frmOperatorActive.productionDetails
                        .Where(d => d.ShopOrderOperationID == helper.shopOrderOperation.Id &&
                                    d.BoxID == Guid.Empty)
                        .ToList();

                // create HU
                var handlingUnit = new HandlingUnit
                {
                    PartHandlingUnitID = vwshopOrderGridModel.PartHandlingUnitID,
                    ShopOrderID = helper.shopOrderOperation.Id,
                    ShopOrderProductionID = helper.shopOrderProduction.Id
                };

                handlingUnit = HandlingUnitManager.Current.Insert(handlingUnit).ListData[0];

                // barcode after serial is assigned
                handlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(handlingUnit.Serial, helper);
                HandlingUnitManager.Current.UpdateBoxBarcode(handlingUnit);

                // now we have a real HU Id -> assign it to details, then persist
                foreach (var sopd in helper.shopOrderProductionDetails)
                {
                    sopd.BoxID = handlingUnit.Id;
                    ShopOrderProductionDetailManager.Current.UpdateBoxID(sopd);
                }

                helper.handlingUnit = handlingUnit;
            }
        }

        /// <summary>
        /// Manually create a box (<see cref="HandlingUnit"/>) for the given order line with a target quantity.
        /// Picks unboxed production details up to the requested amount (by CreatedAt ascending),
        /// assigns them to the newly created HU, updates barcode, and returns the composed helper.
        /// </summary>
        public static ReportHandlingUnitHelper CreatePartHandlingUnitAd(vw_ShopOrderGridModel vwshopOrderGridModel, decimal quantity)
        {
            if (vwshopOrderGridModel == null) return null;
            if (ToolsMdiManager.frmOperatorActive == null) return null;

            lock (_gate)
            {
                var helper = GetReportHandling(vwshopOrderGridModel);

                var unboxed = ToolsMdiManager.frmOperatorActive.productionDetails
                    .Where(d => d.ShopOrderOperationID == helper.shopOrderOperation.Id && d.BoxID == Guid.Empty)
                    .OrderBy(d => d.CreatedAt)
                    .ToList();

                var sumQty = unboxed.Sum(x => x.Quantity);
                helper.shopOrderProductionDetails = (sumQty <= quantity)
                    ? unboxed
                    : unboxed.Take((int)quantity).ToList();

                // create HU first to get its Id
                var handlingUnit = new HandlingUnit
                {
                    PartHandlingUnitID = vwshopOrderGridModel.PartHandlingUnitID,
                    ShopOrderID = helper.shopOrderOperation.Id,
                    ShopOrderProductionID = helper.shopOrderProduction.Id,
                    Quantity = quantity
                };

                handlingUnit = HandlingUnitManager.Current.Insert(handlingUnit).ListData[0];

                // barcode after serial
                handlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(handlingUnit.Serial, helper);
                HandlingUnitManager.Current.UpdateBoxBarcode(handlingUnit);

                // now update details with the new HU Id
                foreach (var sopd in helper.shopOrderProductionDetails)
                {
                    sopd.BoxID = handlingUnit.Id;
                    ShopOrderProductionDetailManager.Current.UpdateBoxID(sopd);
                }

                helper.handlingUnit = handlingUnit;
                ToolsMdiManager.frmOperatorActive.handlingUnits.Add(handlingUnit);

                return helper;
            }
        }

        /// <summary>
        /// Manual m² scenario: create a HU with a single, most-recent detail of the operation and override its ManualInput.
        /// </summary>
        public static ReportHandlingUnitHelper CreatePartHandlingUnitm2(vw_ShopOrderGridModel vwshopOrderGridModel, decimal manuelInput, string description = "")
        {
            if (vwshopOrderGridModel == null) return null;
            if (ToolsMdiManager.frmOperatorActive == null) return null;

            lock (_gate)
            {
                var helper = GetReportHandling(vwshopOrderGridModel);

                // pick the last produced detail for this operation
                var detail = ToolsMdiManager.frmOperatorActive.productionDetails
                    .Where(x => x.ShopOrderOperationID == vwshopOrderGridModel.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

                if (detail == null) return null;

                // override manual input (domain-specific)
                detail.ManualInput = manuelInput;

                helper.shopOrderProductionDetails = new List<ShopOrderProductionDetail> { detail };

                // create HU first to get Id
                var handlingUnit = new HandlingUnit
                {
                    PartHandlingUnitID = vwshopOrderGridModel.PartHandlingUnitID,
                    ShopOrderID = helper.shopOrderOperation.Id,
                    ShopOrderProductionID = helper.shopOrderProduction.Id,
                    Quantity = manuelInput,
                    Description = description
                };

                handlingUnit = HandlingUnitManager.Current.Insert(handlingUnit).ListData[0];

                // barcode
                handlingUnit.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(handlingUnit.Serial, helper);
                HandlingUnitManager.Current.UpdateBoxBarcode(handlingUnit);

                // now assign detail to HU and persist both box id + manual input update
                detail.BoxID = handlingUnit.Id;
                ShopOrderProductionDetailManager.Current.UpdateBoxIDAndManualInput(detail);

                helper.handlingUnit = handlingUnit;
                ToolsMdiManager.frmOperatorActive.handlingUnits.Add(handlingUnit);

                return helper;
            }
        }

        /// <summary>
        /// Builds helper for an existing handling unit (re-hydrates HU+details context).
        /// </summary>
        public static ReportHandlingUnitHelper CreatePartHandlingUnit(vw_ShopOrderGridModel vwshopOrderGridModel, HandlingUnit handlingUnit)
        {
            if (vwshopOrderGridModel == null || handlingUnit == null) return null;
            if (ToolsMdiManager.frmOperatorActive == null) return null;

            var helper = GetReportHandling(vwshopOrderGridModel);
            helper.handlingUnit = handlingUnit;

            helper.shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails
                .Where(x => x.BoxID == handlingUnit.Id)
                .ToList();

            return helper;
        }

        /// <summary>
        /// Compose a <see cref="ReportProductHelper"/> for a newly created production detail and, if requested,
        /// auto-select the appropriate printer/label model based on panel configuration.
        /// </summary>
        /// <param name="flag">
        /// If true, tries to pick a <see cref="PrintLabelModel"/> (Product vs Process) according to
        /// the active panel's <c>AutomaticlabeltypeID</c>. If not found, throws a localized exception.
        /// </param>
        public static ReportProductHelper CreateProductionDetail(
            UserModel userModel,
            vw_ShopOrderGridModel vwshopOrderGridModel,
            ShopOrderProduction shopOrderProduction,
            ShopOrderProductionDetail productionDetail,
            bool flag = true)
        {
            if (vwshopOrderGridModel == null || shopOrderProduction == null || productionDetail == null)
                return null;
            if (ToolsMdiManager.frmOperatorActive == null) return null;

            lock (_gate)
            {
                var helper = new ReportProductHelper
                {
                    shopOrderOperation = vwshopOrderGridModel,
                    shopOrderProduction = shopOrderProduction,
                    shopOrderProductionDetail = productionDetail,
                    userModel = userModel,
                    machine = ToolsMdiManager.frmOperatorActive.machine,
                    resource = ToolsMdiManager.frmOperatorActive.resource,
                    product = ToolsMdiManager.frmOperatorActive.products.First(p => p.Id == productionDetail.ProductID)
                };

                if (!flag)
                    return helper;

                // find which operator form currently contains this shop order op
                foreach (var frmOperator in ToolsMdiManager.frmOperators)
                {
                    var sogm = frmOperator?.vw_ShopOrderGridModels;
                    if (sogm == null || !sogm.Any(x => x.Id == vwshopOrderGridModel.Id))
                        continue;

                    // Select by panel setting: Product or Process
                    var isProduct = frmOperator.panelDetail.AutomaticlabeltypeID == StaticValues.specialCodeProductTypeProduct.Id;
                    var desiredType = isProduct ? ProductionLabelType.Product : ProductionLabelType.Process;

                    try
                    {
                        helper.printLabelModel = ToolsMdiManager.frmOperatorActive.printLabelModels
                            .First(x => x.ProductId == helper.product.Id && x.productionLabelType == desiredType);
                    }
                    catch
                    {
                        // Localized exception with parameters
                        var prm = helper.product.PartNo.CreateParameters("@PartNo");
                        prm.Add("@PrinterType", desiredType.ToText());
                        var msg = MessageTextHelper.ReplaceParameters(
                            MessageTextHelper.GetMessageText("000", "650",
                                "Tanımlı yazıcı bulunamadı.\r\n\r\nReferans no: @PartNo\r\nYazıcı tipi: @PrinterType",
                                "Message"),
                            prm);
                        throw new Exception(msg);
                    }
                    break;
                }

                return helper;
            }
        }

        /// <summary>
        /// Compose a <see cref="ReportProductHelper"/> with fully provided context (no auto printer lookup).
        /// </summary>
        public static ReportProductHelper CreateProductionDetail(
            UserModel userModel,
            vw_ShopOrderGridModel vwshopOrderGridModel,
            ShopOrderProduction shopOrderProduction,
            ShopOrderProductionDetail productionDetail,
            Machine machine,
            Machine resource,
            Product product)
        {
            if (vwshopOrderGridModel == null || shopOrderProduction == null || productionDetail == null ||
                machine == null || resource == null || product == null)
                return null;

            return new ReportProductHelper
            {
                shopOrderOperation = vwshopOrderGridModel,
                shopOrderProduction = shopOrderProduction,
                shopOrderProductionDetail = productionDetail,
                userModel = userModel,
                machine = machine,
                resource = resource,
                product = product
            };
        }

        // ---------------------- internals ----------------------

        /// <summary>
        /// Populate a <see cref="ReportHandlingUnitHelper"/> snapshot from the active operator context.
        /// </summary>
        private static ReportHandlingUnitHelper GetReportHandling(vw_ShopOrderGridModel vwshopOrderGridModel)
        {
            var op = ToolsMdiManager.frmOperatorActive;

            var helper = new ReportHandlingUnitHelper
            {
                vwShopOrderGridModel = vwshopOrderGridModel,
                user = op.Users.SingleOrDefault(),
                product = op.products.First(p => p.Id == vwshopOrderGridModel.ProductID),
                printLabelModel = op.printLabelModels.First(p =>
                    p.ProductId == vwshopOrderGridModel.ProductID &&
                    p.productionLabelType == ProductionLabelType.Box),
                shopOrderOperation = op.shopOrderOperations.Single(s => s.Id == vwshopOrderGridModel.Id),
                shopOrderProduction = op.shopOrderProduction,
                machine = op.machine,
                resource = op.resource
            };

            return helper;
        }
    }
}
