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
    /// Helper to compose and persist Handling Units (HUs) for "adet" (piece-count) scenarios.
    /// Ensures details are assigned to a persisted HU only after insert (so a valid Id is used).
    /// </summary>
    public static class ReportHandlingUnitAdetHelper
    {
        /// <summary>
        /// Creates (or completes) a Handling Unit for a shop order by piece count.
        /// - If <paramref name="manuel"/> is true, draws missing details from unboxed details and, if needed,
        ///   creates additional production details to reach <paramref name="manualInput"/>.
        /// - If <paramref name="halfHandlingUnit"/> is supplied, completes that HU to the target quantity instead of creating a new one.
        /// </summary>
        public static ReportHandlingUnitHelper CreateReportHandlingUnit(
            vw_ShopOrderGridModel shopOrder,
            decimal manualInput,
            UserModel userModel,
            PartHandlingUnit partHandlingUnit,
            HandlingUnit halfHandlingUnit,
            bool manuel = true,
            string PartiNoAdet = "")
        {
            if (shopOrder == null || userModel == null || partHandlingUnit == null || ToolsMdiManager.frmOperatorActive == null)
                return null;

            // Current HU quantity already made (continuation case)
            decimal qtyFound = halfHandlingUnit != null ? halfHandlingUnit.Quantity : 0m;

            var helper = CreateReportHandlingUnitHelper(shopOrder, userModel);

            // Determine which production details will go into the HU
            if (manuel)
            {
                var unboxedForOp = ToolsMdiManager.frmOperatorActive.productionDetails
                    .Where(d => d.ShopOrderOperationID == helper.shopOrderOperation.Id && d.BoxID == Guid.Empty)
                    .ToList();

                var sumQty = (double)unboxedForOp.Sum(x => x.Quantity);
                var targetRemaining = Convert.ToDouble(manualInput - qtyFound);

                if (sumQty <= targetRemaining)
                {
                    // Need to create additional details to reach target
                    var difference = targetRemaining - sumQty;
                    for (int i = 0; i < difference; i++)
                    {
                        var d = ShopOrderProductionDetailHelper.CreateAndInsertProductionDetail(
                            shopOrder,
                            helper.product,
                            printed: false,
                            userModel,
                            crewSize: ToolsMdiManager.frmOperatorActive.Users.Count);

                        ToolsMdiManager.frmOperatorActive.productionDetails.Add(d);
                    }

                    // Re-pull now that new details exist
                    helper.shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails
                        .Where(d => d.ShopOrderOperationID == helper.shopOrderOperation.Id && d.BoxID == Guid.Empty)
                        .ToList();
                }
                else
                {
                    // Take only what's needed, oldest first
                    helper.shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails
                        .Where(d => d.ShopOrderOperationID == helper.shopOrderOperation.Id && d.BoxID == Guid.Empty)
                        .OrderBy(d => d.CreatedAt)
                        .Take(Convert.ToInt32(targetRemaining))
                        .ToList();
                }
            }
            else
            {
                helper.shopOrderProductionDetails = GetShopOrderProductionDetails(
                    shopOrder,
                    helper.product,
                    manualInput - qtyFound,
                    userModel);
            }

            // Insert or update the HU
            if (halfHandlingUnit == null)
            {
                var hu = new HandlingUnit
                {
                    // If not m2, store manual input into ManuelInput field (as per your original logic)
                    ManuelInput = string.Equals(shopOrder.unitMeas, Units.m2.ToText(), StringComparison.Ordinal)
                        ? 0
                        : manualInput,
                    Description = PartiNoAdet,
                    PartHandlingUnitID = partHandlingUnit.Id,
                    ShopOrderID = helper.shopOrderOperation.Id,
                    ShopOrderProductionID = helper.shopOrderProduction.Id,
                    Quantity = manualInput,
                    CompanyPersonId = (ToolsMdiManager.frmOperatorActive.manuelLabelUser != null)
                        ? ToolsMdiManager.frmOperatorActive.manuelLabelUser.CompanyPersonId
                        : userModel.CompanyPersonId
                };

                // Persist first to get a real Id & Serial
                hu = HandlingUnitManager.Current.Insert(hu).ListData[0];

                // Generate customer barcode after Serial is known
                hu.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(hu.Serial, helper);
                HandlingUnitManager.Current.UpdateBoxBarcode(hu);

                helper.handlingUnit = hu;
            }
            else
            {
                // Complete an existing half HU to the new total quantity
                helper.handlingUnit = halfHandlingUnit;
                helper.handlingUnit.Quantity = manualInput;
                helper.handlingUnit.ManuelInput = 0; // you had this reset; preserved
                HandlingUnitManager.Current.Update(helper.handlingUnit);
            }

            // Assign picked details to the HU (now that we have a persisted Id)
            foreach (var detail in helper.shopOrderProductionDetails)
            {
                // reflect in the in-memory list too
                var inMem = ToolsMdiManager.frmOperatorActive.productionDetails.FirstOrDefault(x => x.Id == detail.Id);
                if (inMem != null) inMem.BoxID = helper.handlingUnit.Id;

                detail.BoxID = helper.handlingUnit.Id;
                ShopOrderProductionDetailManager.Current.UpdateBoxID(detail);
            }

            // Track HU in operator context if not already present
            if (!ToolsMdiManager.frmOperatorActive.handlingUnits.Any(x => x.Id == helper.handlingUnit.Id))
                ToolsMdiManager.frmOperatorActive.handlingUnits.Add(helper.handlingUnit);

            return helper;
        }

        /// <summary>
        /// Creates a Handling Unit with a specific set of production details (already chosen by caller).
        /// </summary>
        public static ReportHandlingUnitHelper CreateReportHandlingUnit(
            vw_ShopOrderGridModel shopOrder,
            UserModel userModel,
            List<ShopOrderProductionDetail> productionDetails)
        {
            if (shopOrder == null || userModel == null || productionDetails == null || ToolsMdiManager.frmOperatorActive == null)
                return null;

            var helper = CreateReportHandlingUnitHelper(shopOrder, userModel);

            // Use provided details as-is
            helper.shopOrderProductionDetails = productionDetails;

            // Create HU FIRST to get Id; DO NOT assign BoxID before insert (bug in original code)
            var hu = new HandlingUnit
            {
                PartHandlingUnitID = shopOrder.PartHandlingUnitID,
                ShopOrderID = helper.shopOrderOperation.Id,
                ShopOrderProductionID = helper.shopOrderProduction.Id,
                Quantity = productionDetails.Sum(x => x.Quantity)
            };

            hu = HandlingUnitManager.Current.Insert(hu).ListData[0];

            hu.Barcode = BoxBarcodeHelper.GetCustomerBoxBarcode(hu.Serial, helper);
            HandlingUnitManager.Current.UpdateBoxBarcode(hu);

            // Now assign each detail to the real HU Id
            foreach (var d in helper.shopOrderProductionDetails)
            {
                d.BoxID = hu.Id;
                ShopOrderProductionDetailManager.Current.UpdateBoxID(d);
            }

            helper.handlingUnit = hu;
            ToolsMdiManager.frmOperatorActive.handlingUnits.Add(hu);
            return helper;
        }

        /// <summary>
        /// Rehydrates a helper for an existing Handling Unit (and localizes the barcode by replacing Turkish chars).
        /// </summary>
        public static ReportHandlingUnitHelper CreatePartHandlingUnit(
            vw_ShopOrderGridModel vwshopOrderGridModel,
            UserModel userModel,
            HandlingUnit handlingUnit,
            bool flag = true)
        {
            if (vwshopOrderGridModel == null || userModel == null || handlingUnit == null || ToolsMdiManager.frmOperatorActive == null)
                return null;

            var helper = GetReportHandling(vwshopOrderGridModel, userModel, flag);
            helper.handlingUnit = handlingUnit;

            // Normalize barcode (as in your original code)
            helper.handlingUnit.Barcode = helper.handlingUnit.Barcode
                .Replace('ç', 'c').Replace('ş', 's').Replace('ğ', 'g').Replace('ö', 'o').Replace('ü', 'u')
                .Replace('Ç', 'C').Replace('S', 'S').Replace('Ğ', 'G').Replace('Ö', 'O').Replace('Ü', 'U').Replace('İ', 'I');

            helper.shopOrderProductionDetails = ToolsMdiManager.frmOperatorActive.productionDetails
                .Where(x => x.BoxID == handlingUnit.Id)
                .ToList();

            return helper;
        }

        /// <summary>
        /// Internal: builds a populated <see cref="ReportHandlingUnitHelper"/> from active operator context.
        /// </summary>
        private static ReportHandlingUnitHelper GetReportHandling(vw_ShopOrderGridModel shopOrder, UserModel userModel, bool flag = true)
        {
            var helper = new ReportHandlingUnitHelper();
            try
            {
                helper.vwShopOrderGridModel = shopOrder;
                helper.product = ToolsMdiManager.frmOperatorActive.products.First(p => p.Id == shopOrder.ProductID);
                helper.shopOrderOperation = ToolsMdiManager.frmOperatorActive.shopOrderOperations.Single(s => s.Id == shopOrder.Id);
                helper.shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction;
                helper.machine = ToolsMdiManager.frmOperatorActive.machine;
                helper.resource = ToolsMdiManager.frmOperatorActive.resource;
                helper.user = userModel;

                if (flag)
                {
                    helper.printLabelModel = ToolsMdiManager.frmOperatorActive.printLabelModels.First(p =>
                        p.ProductId == helper.product.Id &&
                        p.productionLabelType == ProductionLabelType.Box);
                }

                return helper;
            }
            catch
            {
                var prm = helper.product?.Description.CreateParameters("@ProductDescription") ?? "@ProductDescription".CreateParameters("@ProductDescription");
                throw new Exception(
                    MessageTextHelper.ReplaceParameters(
                        MessageTextHelper.GetMessageText("000", "651",
                            "@ProductDescription ürünü için etiket dizaynı veya printer tanımlı değildir.",
                            "Message"),
                        prm));
            }
        }

        /// <summary>
        /// Internal: common builder used by the main creation path (always loads Box label model).
        /// </summary>
        private static ReportHandlingUnitHelper CreateReportHandlingUnitHelper(vw_ShopOrderGridModel shopOrder, UserModel userModel)
        {
            var helper = new ReportHandlingUnitHelper();
            try
            {
                helper.vwShopOrderGridModel = shopOrder;
                helper.user = userModel;
                helper.product = ToolsMdiManager.frmOperatorActive.products.First(p => p.Id == shopOrder.ProductID);
                helper.shopOrderOperation = ToolsMdiManager.frmOperatorActive.shopOrderOperations.Single(s => s.Id == shopOrder.Id);
                helper.shopOrderProduction = ToolsMdiManager.frmOperatorActive.shopOrderProduction;
                helper.machine = ToolsMdiManager.frmOperatorActive.machine;
                helper.resource = ToolsMdiManager.frmOperatorActive.resource;
                helper.printLabelModel = ToolsMdiManager.frmOperatorActive.printLabelModels.First(p =>
                    p.ProductId == helper.product.Id &&
                    p.productionLabelType == ProductionLabelType.Box);

                return helper;
            }
            catch
            {
                var prm = helper.product?.Description.CreateParameters("@ProductDescription") ?? "@ProductDescription".CreateParameters("@ProductDescription");
                throw new Exception(
                    MessageTextHelper.ReplaceParameters(
                        MessageTextHelper.GetMessageText("000", "651",
                            "@ProductDescription ürünü için etiket dizaynı veya printer tanımlı değildir.",
                            "Message"),
                        prm));
            }
        }

        /// <summary>
        /// Internal: Computes which details (and how much of each) are required to reach <paramref name="maxQuantityCapacity"/>.
        /// Updates <see cref="ShopOrderProductionDetail.HandlingUnitQuantity"/> accordingly and creates an extra detail if needed.
        /// </summary>
        private static List<ShopOrderProductionDetail> GetShopOrderProductionDetails(
            vw_ShopOrderGridModel shopOrder,
            Product selectedProduct,
            decimal maxQuantityCapacity,
            UserModel userModel)
        {
            decimal currentQuantity = 0;
            var result = new List<ShopOrderProductionDetail>();

            foreach (var p in ToolsMdiManager.frmOperatorActive.productionDetails)
            {
                if (p.ProductID != selectedProduct.Id)
                    continue;

                // Single-quantity items (full or unassigned)
                if (p.Quantity == 1 && (p.HandlingUnitQuantity == 1 || p.HandlingUnitQuantity == 0))
                {
                    if (p.BoxID == Guid.Empty)
                    {
                        result.Add(p);
                        currentQuantity += p.Quantity;
                        p.HandlingUnitQuantity = p.Quantity;
                        ShopOrderProductionDetailManager.Current.Update(p);

                        if (currentQuantity == maxQuantityCapacity)
                            break;
                    }
                }
                // Partial case: some of the quantity may have been assigned already
                else if (p.Quantity > p.HandlingUnitQuantity)
                {
                    result.Add(p);

                    var remaining = p.Quantity - p.HandlingUnitQuantity;
                    if (remaining + currentQuantity >= maxQuantityCapacity)
                    {
                        // take only what we need from this detail
                        p.HandlingUnitQuantity += (maxQuantityCapacity - currentQuantity);
                        currentQuantity = maxQuantityCapacity;
                        ShopOrderProductionDetailManager.Current.Update(p);
                        break;
                    }
                    else
                    {
                        // take all remaining from this detail
                        currentQuantity += remaining;
                        p.HandlingUnitQuantity = p.Quantity;
                        ShopOrderProductionDetailManager.Current.Update(p);
                    }
                }
            }

            // If still short, create one extra detail to fill the gap
            if (maxQuantityCapacity > currentQuantity)
            {
                var difference = maxQuantityCapacity - currentQuantity;
                var d = ShopOrderProductionDetailHelper.CreateAndInsertProductionDetail(
                    shopOrder,
                    selectedProduct,
                    printed: false,
                    userModel,
                    crewSize: ToolsMdiManager.frmOperatorActive.Users.Count,
                    quantity: difference,
                    handlingUnitQuantity: difference);

                ToolsMdiManager.frmOperatorActive.productionDetails.Add(d);
                result.Add(d);
            }

            return result;
        }
    }
}
