using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Factory and utilities for creating and inserting <see cref="ShopOrderProductionDetail"/> records,
    /// including OK and Scrap flows, and printing scrap labels.
    /// </summary>
    public static class ShopOrderProductionDetailHelper
    {
        /// <summary>
        /// Creates and inserts a production detail using the active <see cref="FrmOperator"/> context
        /// (machine, resource, current production, and parent handling unit are taken from the active form).
        /// </summary>
        /// <param name="shopOrder">
        /// Optional shop order row. If <c>null</c>, the method tries to resolve it from the active form by <paramref name="product"/> ID.
        /// </param>
        /// <param name="product">The product for which the production detail is created.</param>
        /// <param name="printed">Whether a label was printed for this detail.</param>
        /// <param name="userModel">The user who performed the operation.</param>
        /// <param name="crewSize">Crew size.</param>
        /// <param name="quantity">Reported quantity (default 1).</param>
        /// <param name="handlingUnitQuantity">Quantity per handling unit (optional; default 0).</param>
        /// <returns>The inserted <see cref="ShopOrderProductionDetail"/>; returns <c>null</c> if prerequisites are missing.</returns>
        public static ShopOrderProductionDetail CreateAndInsertProductionDetail(
            vw_ShopOrderGridModel shopOrder,
            Product product,
            bool printed,
            UserModel userModel,
            int crewSize,
            decimal quantity = 1,
            decimal handlingUnitQuantity = 0)
        {
            var frm = ToolsMdiManager.frmOperatorActive;
            if (frm == null || product == null || userModel == null)
                return null;

            // Resolve shop order row when not provided
            if (shopOrder == null && frm.vw_ShopOrderGridModels != null)
                shopOrder = frm.vw_ShopOrderGridModels.FirstOrDefault(x => x.ProductID == product.Id);

            if (shopOrder == null || frm.shopOrderProduction == null || frm.machine == null || frm.resource == null)
                return null;

            // Resolve parent HU safely
            var parentHu = frm.partHandlingUnits != null
                ? frm.partHandlingUnits.FirstOrDefault(x => x.PartNo == product.PartNo)
                : null;

            var parentHuId = parentHu != null ? parentHu.Id : Guid.Empty;

            return CreateAndInsertProductionDetail(
                frm.machine.Id,
                frm.resource.Id,
                frm.shopOrderProduction.Id,
                parentHuId,
                shopOrder,
                product.Id,
                printed,
                userModel.CompanyPersonId,
                crewSize,
                quantity,
                handlingUnitQuantity);
        }

        /// <summary>
        /// Creates and inserts a production detail using explicit IDs (machine, resource, production, parent HU).
        /// </summary>
        /// <param name="machineId">Work center (machine) ID.</param>
        /// <param name="resourceId">Resource ID.</param>
        /// <param name="shopOrderProductionId">Current shop order production ID.</param>
        /// <param name="parHandlingUnitID">Parent handling unit ID (or <see cref="Guid.Empty"/> if none).</param>
        /// <param name="shopOrder">Shop order/operation row (required).</param>
        /// <param name="productId">Product ID.</param>
        /// <param name="printed">Whether a label was printed for this detail.</param>
        /// <param name="userId">Operator person ID.</param>
        /// <param name="crewSize">Crew size.</param>
        /// <param name="quantity">Reported quantity (default 1).</param>
        /// <param name="handlingUnitQuantity">Quantity per handling unit (optional; default 0).</param>
        /// <returns>The inserted <see cref="ShopOrderProductionDetail"/>; returns <c>null</c> if input is insufficient.</returns>
        public static ShopOrderProductionDetail CreateAndInsertProductionDetail(
            Guid machineId,
            Guid resourceId,
            Guid shopOrderProductionId,
            Guid parHandlingUnitID,
            vw_ShopOrderGridModel shopOrder,
            Guid productId,
            bool printed,
            Guid userId,
            int crewSize,
            decimal quantity = 1,
            decimal handlingUnitQuantity = 0)
        {
            if (shopOrderProductionId == Guid.Empty || machineId == Guid.Empty || resourceId == Guid.Empty || shopOrder == null)
                return null;

            var detail = new ShopOrderProductionDetail
            {
                ShopOrderOperationID = shopOrder.Id,
                ShopOrderProductionID = shopOrderProductionId,
                WorkCenterID = machineId,
                ResourceID = resourceId,
                ShiftId = StaticValues.shift != null ? StaticValues.shift.Id : Guid.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Unit = shopOrder.unitMeas,
                Quantity = quantity,
                BoxID = Guid.Empty,
                ProductionStateID = StaticValues.specialCodeOk.Id,
                Factor = 1,
                Divisor = 1,
                Printed = printed,
                ProductID = productId,
                ManualInput = quantity,
                Active = true,
                ParHandlingUnitID = parHandlingUnitID,
                CompanyPersonId = ResolveCompanyPersonId(userId),
                HandlingUnitQuantity = handlingUnitQuantity,
                OrderNo = shopOrder.orderNo,
                OperationNo = shopOrder.operationNo,
                CrewSize = crewSize
            };

            // Insert and then set the barcode to the generated serial
            var inserted = ShopOrderProductionDetailManager.Current.Insert(detail).ListData[0];
            inserted.Barcode = inserted.serial.ToString();
            ShopOrderProductionDetailManager.Current.UpdateBarcode(inserted);

            return inserted;
        }

        /// <summary>
        /// Creates and inserts a production detail in <b>SCRAP</b> state for the provided operator form context,
        /// including Ifs scrap type, and prints scrap label via <see cref="ReportProductHelper"/> if configured.
        /// </summary>
        /// <param name="frmOperator">Active operator form.</param>
        /// <param name="shopOrder">Optional shop order row; resolved from form if <c>null</c>.</param>
        /// <param name="product">Product to scrap.</param>
        /// <param name="printed">Whether a label was printed for this detail.</param>
        /// <param name="userModel">The user who performed the operation.</param>
        /// <param name="crewSize">Crew size.</param>
        /// <param name="quantity">Scrap quantity (default 1).</param>
        /// <param name="handlingUnitQuantity">Quantity per handling unit (optional; default 0).</param>
        /// <param name="btnManuelScrap">Reserved flag (kept for backward compatibility).</param>
        /// <returns>The inserted scrap <see cref="ShopOrderProductionDetail"/>; <c>null</c> if prerequisites are missing.</returns>
        public static ShopOrderProductionDetail CreateAndInsertScrapProductionDetail(
            FrmOperator frmOperator,
            vw_ShopOrderGridModel shopOrder,
            Product product,
            bool printed,
            UserModel userModel,
            int crewSize,
            decimal quantity = 1,
            decimal handlingUnitQuantity = 0,
            bool btnManuelScrap = false)
        {
            if (frmOperator == null || product == null || userModel == null || frmOperator.shopOrderProduction == null || frmOperator.machine == null || frmOperator.resource == null)
                return null;

            if (shopOrder == null && frmOperator.vw_ShopOrderGridModels != null)
                shopOrder = frmOperator.vw_ShopOrderGridModels.FirstOrDefault(x => x.ProductID == product.Id);

            if (shopOrder == null)
                return null;

            // Resolve parent HU safely
            var parentHu = frmOperator.partHandlingUnits != null
                ? frmOperator.partHandlingUnits.FirstOrDefault(x => x.PartNo == product.PartNo)
                : null;

            var detail = new ShopOrderProductionDetail
            {
                ShopOrderOperationID = shopOrder.Id,
                ShopOrderProductionID = frmOperator.shopOrderProduction.Id,
                WorkCenterID = frmOperator.machine.Id,
                ResourceID = frmOperator.resource.Id,
                ShiftId = StaticValues.shift != null ? StaticValues.shift.Id : Guid.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Unit = shopOrder.unitMeas,
                Quantity = quantity,
                BoxID = Guid.Empty,
                CustomerBoxID = Guid.Empty,
                ProductionStateID = StaticValues.specialCodeScrap.Id,
                IfsScrapTypeId = frmOperator.scrapReason != null ? frmOperator.scrapReason.Id : Guid.Empty,
                Factor = 1,
                Divisor = 1,
                Printed = printed,
                ProductID = product.Id,
                ManualInput = quantity,
                Active = true,
                ParHandlingUnitID = parentHu != null ? parentHu.Id : Guid.Empty,
                CompanyPersonId = userModel.CompanyPersonId,
                HandlingUnitQuantity = handlingUnitQuantity,
                OrderNo = shopOrder.orderNo,
                OperationNo = shopOrder.operationNo,
                CrewSize = crewSize
            };

            var inserted = ShopOrderProductionDetailManager.Current.Insert(detail).ListData[0];

            // If IFS scrap integration is re-enabled later, place it here (kept from your original structure).

            inserted.Barcode = inserted.serial.ToString();
            ShopOrderProductionDetailManager.Current.UpdateBarcode(inserted);

            return inserted;
        }

        /// <summary>
        /// Prints a scrap label using <see cref="ReportProductHelper"/> if <see cref="StaticValues.ScrapPrinterName"/> is configured.
        /// </summary>
        /// <param name="product">Product for label content.</param>
        /// <param name="shopOrderProduction">Parent production.</param>
        /// <param name="shopOrderGridModel">Shop order row.</param>
        /// <param name="shopOrderProductionDetail">Scrap production detail to log/print.</param>
        /// <param name="machine">Work center (machine).</param>
        /// <param name="resource">Resource.</param>
        /// <param name="userModel">User context.</param>
        public static void PrintScrapProduction(
            Product product,
            ShopOrderProduction shopOrderProduction,
            vw_ShopOrderGridModel shopOrderGridModel,
            ShopOrderProductionDetail shopOrderProductionDetail,
            Machine machine,
            Machine resource,
            UserModel userModel)
        {
            if (product == null || shopOrderProduction == null || shopOrderProductionDetail == null || machine == null || resource == null || userModel == null)
                return;

            if (string.IsNullOrEmpty(StaticValues.ScrapPrinterName))
                return;

            var report = new ReportProductHelper
            {
                product = product,
                shopOrderProduction = shopOrderProduction,
                shopOrderProductionDetail = shopOrderProductionDetail,
                machine = machine,
                resource = resource,
                userModel = userModel,
                shopOrderOperation = shopOrderGridModel,
                printLabelModel = new PrintLabelModel
                {
                    LabelDesingFilePath = StaticValues.ScrapProductDesignPath,
                    PrintCopyCount = 1,
                    printerName = StaticValues.ScrapPrinterName,
                    productionLabelType = ProductionLabelType.Product
                }
            };

            report.PrintLabel(true);
        }

        // ------------- helpers -------------

        private static Guid ResolveCompanyPersonId(Guid fallbackUserId)
        {
            var frm = ToolsMdiManager.frmOperatorActive;
            if (frm != null && frm.manuelLabelUser != null && frm.manuelLabelUser.CompanyPersonId != Guid.Empty)
                return frm.manuelLabelUser.CompanyPersonId;

            return fallbackUserId;
        }
    }
}
