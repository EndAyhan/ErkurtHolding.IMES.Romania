using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Handles reading of process barcodes (moving WIP from operation N to operation N+10).
    /// Includes optional "recipe" (reçete) control path and PLC counter integration.
    /// </summary>
    public static class ProcessReadBarcodeHelper
    {
        /// <summary>
        /// Legacy/simple read path for process barcode.
        /// Validates that the scanned detail belongs to an order at the current form, at next operation (op+10),
        /// checks duplicates, assigns barcode into the correct production detail, and optionally increments PLC counter.
        /// </summary>
        /// <exception cref="Exception">Throws with localized messages on all validation failures.</exception>
        public static void ReadProcessBarcode(string barcode)
        {
            var resultProductionDetail = ShopOrderProductionDetailManager.Current
                .GetShopOrderProductionDetailByBarcodeOrSerial(barcode);

            if (resultProductionDetail == null)
            {
                var prm = barcode.CreateParameters("@barcode");
                throw new Exception(MessageTextHelper.ReplaceParameters(
                    MessageTextHelper.GetMessageText("000", "649", "@barcode nolu ürün bulunamadı.", "Message"), prm));
            }

            foreach (var frmOperator in ToolsMdiManager.frmOperators)
            {
                // Must have an order for "next operation" of the scanned detail's order/op
                if (!frmOperator.shopOrderOperations.HasEntries() ||
                    !frmOperator.shopOrderOperations.Any(x =>
                        x.orderNo == resultProductionDetail.OrderNo &&
                        x.operationNo == resultProductionDetail.OperationNo + 10))
                {
                    continue;
                }

                ActivateOperatorTab(frmOperator);

                // Duplicate checks (for both serial-only and barcode cases)
                if (string.IsNullOrEmpty(resultProductionDetail.Barcode) &&
                    frmOperator.productionDetails.Any(p => p.serial == resultProductionDetail.serial))
                {
                    throw new Exception(MessageTextHelper.GetMessageText("000", "895", "Ürün daha önceden okutuldu..!", "Message"));
                }

                if (!string.IsNullOrEmpty(resultProductionDetail.Barcode) &&
                    frmOperator.productionDetails.Any(p =>
                        p.Barcode == resultProductionDetail.Barcode ||
                        p.Barcode == resultProductionDetail.serial.ToString()))
                {
                    throw new Exception(MessageTextHelper.GetMessageText("000", "895", "Ürün daha önceden okutuldu..!", "Message"));
                }

                // Sanity: ensure the current form really has the target (order, op+10)
                if (!frmOperator.vw_ShopOrderGridModels.Any(s =>
                        s.orderNo == resultProductionDetail.OrderNo &&
                        s.operationNo == resultProductionDetail.OperationNo + 10))
                {
                    var prm = resultProductionDetail.OrderNo.CreateParameters("@OrderNo");
                    prm.Add("@OperationNo", resultProductionDetail.OperationNo);
                    throw new Exception(MessageTextHelper.ReplaceParameters(
                        MessageTextHelper.GetMessageText("000", "896",
                            "Okutulan barkod IFS rota yönetimine göre bir önceki operasyona ait değildir.\r\n Okutulan barkodun sipariş No @OrderNo operasyon no :@OperationNo..!",
                            "Message"), prm));
                }

                // Assign barcode to the matching "next operation" production detail
                var nextDetail = FindNextOperationDetail(frmOperator, resultProductionDetail);
                if (nextDetail == null)
                {
                    // No target detail row exists yet at op+10 (UI state out of sync)
                    var prm = resultProductionDetail.OrderNo.CreateParameters("@OrderNo");
                    prm.Add("@OperationNo", resultProductionDetail.OperationNo + 10);
                    throw new Exception(MessageTextHelper.ReplaceParameters(
                        MessageTextHelper.GetMessageText("000", "896",
                            "Okutulan barkod IFS rota yönetimine göre bir önceki operasyona ait değildir.\r\n Okutulan barkodun sipariş No @OrderNo operasyon no :@OperationNo..!",
                            "Message"), prm));
                }

                nextDetail.Barcode = string.IsNullOrEmpty(resultProductionDetail.Barcode)
                    ? resultProductionDetail.serial.ToString()
                    : resultProductionDetail.Barcode;

                // Optional PLC “count by process label”
                if (frmOperator.panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id)
                {
                    StaticValues.opcClient.AddCounter(frmOperator.panelDetail.OPCNodeIdCounterIncrement, 1);
                    frmOperator.Label9SetValue();
                    frmOperator.container.Tag = ContainerSelectUserControl.GeneralReadResult;
                    frmOperator.container.Visible = true;
                }

                break;
            }
        }

        /// <summary>
        /// New read path with optional recipe (reçete) control and automatic resource find option.
        /// </summary>
        /// <exception cref="Exception">Throws with localized messages on all validation failures.</exception>
        public static void ReadProcessBarcodeNew(string barcode)
        {
            bool checkSuccessful = false;

            var resultProductionDetail = ShopOrderProductionDetailManager.Current
                .GetShopOrderProductionDetailByBarcodeOrSerial(barcode);

            if (resultProductionDetail == null)
            {
                var prm = barcode.CreateParameters("@barcode");
                throw new Exception(MessageTextHelper.ReplaceParameters(
                    MessageTextHelper.GetMessageText("000", "649", "@barcode nolu ürün bulunamadı.", "Message"), prm));
            }

            // Reçete control consistency across current form's orders:
            // if some orders require it and some don't, error out with a descriptive message.
            int receteControlCount = ToolsMdiManager.frmOperatorActive.shopOrderOperations
                .Where(x => x.alan10 == "TRUE")
                .Count();

            if (receteControlCount > 0 &&
                receteControlCount < ToolsMdiManager.frmOperatorActive.shopOrderOperations.Count)
            {
                string msg = "İş emirlerinin bir bölümünde recete kontrolü yapılması isteniyor.";
                foreach (var order in ToolsMdiManager.frmOperatorActive.shopOrderOperations)
                    msg += $"\r\nİş emri no: {order.orderNo} recete kontrol: {order.alan10}";
                throw new Exception(msg);
            }

            // Auto-find across all operator forms?
            if (StaticValues.panel.ProcessAutomaticFindResource && receteControlCount == 0)
            {
                foreach (var frmOperator in ToolsMdiManager.frmOperators)
                {
                    if (ProcessBarcode(resultProductionDetail, frmOperator, receteControl: false))
                    {
                        checkSuccessful = true;
                        break;
                    }
                }
            }
            else
            {
                checkSuccessful = ProcessBarcode(resultProductionDetail, ToolsMdiManager.frmOperatorActive, receteControl: receteControlCount != 0);
            }

            // Build a nicer error when we know the last form the product belonged to
            var lastReadProductionDetail =
                ToolsMdiManager.frmOperatorActive.shopOrderOperations.FirstOrDefault(x => x.orderNo == resultProductionDetail.OrderNo);

            if (!checkSuccessful && lastReadProductionDetail != null)
            {
                var prm = resultProductionDetail.OrderNo.CreateParameters("@OrderNo");
                prm.Add("@OperationNo", resultProductionDetail.OperationNo);
                prm.Add("@lastOperationNo", lastReadProductionDetail.operationNo);
                throw new Exception(MessageTextHelper.ReplaceParameters(
                    MessageTextHelper.GetMessageText("000", "899",
                        "Ürünün son okutulduğu İş Emri No: @OrderNo, Operasyon No: @OperationNo +\r\n Okutulmaya çalışılan Operasyon No: @lastOperationNo ",
                        "Message"), prm));
            }
            else if (!checkSuccessful && lastReadProductionDetail == null)
            {
                throw new Exception(MessageTextHelper.GetMessageText("000", "620", "Ürüne ait iş emiri bulunamadı.", "Message"));
            }
        }

        /// <summary>
        /// Core processing for a scanned barcode in the context of a specific operator form.
        /// Handles both "recipe-controlled" and normal flows.
        /// </summary>
        private static bool ProcessBarcode(Entity.ShopOrderProductionDetail resultProductionDetail, FrmOperator frmOperator, bool receteControl = false)
        {
            if (frmOperator == null) return false;

            if (receteControl)
            {
                try
                {
                    // Ensure the scanned product exists in the BOM (recipe) of each order at this form,
                    // and that it hasn't exceeded its allowed prescription read count.
                    foreach (var order in frmOperator.shopOrderOperations)
                    {
                        if (frmOperator.shopOrderProduction == null)
                        {
                            SetBuzzer(true);
                            throw new Exception(MessageTextHelper.GetMessageText("000", "621", "Bir hata oluştu!", "Message"));
                        }

                        var product = ProductManager.Current.GetProductById(resultProductionDetail.ProductID);
                        var allocs = ShopMaterialAllocManager.Current.GetlistByOrderID(order.Id);

                        if (!allocs.HasEntries() || !allocs.Any(x => x.description == product.Description))
                        {
                            SetBuzzer(true);
                            var prm = order.orderNo.CreateParameters("@orderNo");
                            throw new Exception(MessageTextHelper.ReplaceParameters(
                                MessageTextHelper.GetMessageText("000", "897",
                                    "Okutuğunuz ürün reçetesinde bulunamadı. İş Emri No: @orderNo", "Message"), prm));
                        }

                        if (resultProductionDetail.PrescriptionControlCount > 0)
                        {
                            SetBuzzer(true);
                            var prm = resultProductionDetail.serial.CreateParameters("@serial");
                            prm.Add("@prescriptionControlCount", resultProductionDetail.PrescriptionControlCount);
                            throw new Exception(MessageTextHelper.ReplaceParameters(
                                MessageTextHelper.GetMessageText("000", "898",
                                    "@serial Barkod numaralı ürün @prescriptionControlCount defa okutulmuştur. Tekrar kullanamazsınız.", "Message"), prm));
                        }
                    }

                    // OK — count & UI feedback
                    StaticValues.opcClient.AddCounter(frmOperator.panelDetail.OPCNodeIdCounterIncrement, 1);
                    frmOperator.ShopOrderProductionDetailPrescriptionControlCount = resultProductionDetail;
                    frmOperator.Label9SetValue();

                    frmOperator.container.Tag = ContainerSelectUserControl.GeneralReadResult;
                    frmOperator.container.Visible = true;

                    SetBuzzer(false);
                    return true;
                }
                catch (Exception ex)
                {
                    // bubble up the localized error
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                // Must have next op (op+10) for the scanned product on this form
                bool hasNextOp =
                    frmOperator.vw_ShopOrderGridModels.HasEntries() &&
                    frmOperator.vw_ShopOrderGridModels.Any(x =>
                        x.ProductID == resultProductionDetail.ProductID &&
                        x.operationNo == resultProductionDetail.OperationNo + 10);

                if (!hasNextOp) return false;

                ActivateOperatorTab(frmOperator);

                // Duplicate checks
                if (string.IsNullOrEmpty(resultProductionDetail.Barcode) &&
                    frmOperator.productionDetails.Any(p => p.serial == resultProductionDetail.serial))
                {
                    throw new Exception(MessageTextHelper.GetMessageText("000", "619", "Ürün daha önceden okutuldu..!", "Message"));
                }

                if (!string.IsNullOrEmpty(resultProductionDetail.Barcode) &&
                    frmOperator.productionDetails.Any(p =>
                        p.Barcode == resultProductionDetail.Barcode ||
                        p.Barcode == resultProductionDetail.serial.ToString()))
                {
                    throw new Exception(MessageTextHelper.GetMessageText("000", "619", "Ürün daha önceden okutuldu..!", "Message"));
                }

                // Sanity that the exact (order, op+10) exists here
                if (!frmOperator.vw_ShopOrderGridModels.Any(s =>
                        s.orderNo == resultProductionDetail.OrderNo &&
                        s.operationNo == (resultProductionDetail.OperationNo + 10)))
                {
                    var prm = resultProductionDetail.OrderNo.CreateParameters("@OrderNo");
                    prm.Add("@OperationNo", resultProductionDetail.OperationNo);
                    throw new Exception(MessageTextHelper.ReplaceParameters(
                        MessageTextHelper.GetMessageText("000", "896",
                            "Okutulan barkod IFS rota yönetimine göre bir önceki operasyona ait değildir.\r\n Okutulan barkodun sipariş No @OrderNo operasyon no :@OperationNo..!",
                            "Message"), prm));
                }

                // Assign barcode to the correct next-operation detail (not .First() arbitrary)
                var nextDetail = FindNextOperationDetail(frmOperator, resultProductionDetail);
                if (nextDetail == null) return false;

                nextDetail.OrderNo = resultProductionDetail.OrderNo; // keep order consistent
                nextDetail.Barcode = string.IsNullOrEmpty(resultProductionDetail.Barcode)
                    ? resultProductionDetail.serial.ToString()
                    : resultProductionDetail.Barcode;

                // Optional PLC count
                if (frmOperator.panelDetail.DataReadParameterID == StaticValues.specialCodeCounterReadTypeBarocodePlc.Id)
                {
                    StaticValues.opcClient.AddCounter(frmOperator.panelDetail.OPCNodeIdCounterIncrement, 1);
                    frmOperator.Label9SetValue();
                    frmOperator.container.Tag = ContainerSelectUserControl.GeneralReadResult;
                    frmOperator.container.Visible = true;
                }

                return true;
            }
        }

        // ---------- helpers ----------

        /// <summary>
        /// Finds the production detail on the form that corresponds to the scanned item’s next operation (op+10).
        /// </summary>
        private static Entity.ShopOrderProductionDetail FindNextOperationDetail(FrmOperator frmOperator, Entity.ShopOrderProductionDetail scanned)
        {
            if (frmOperator == null || scanned == null) return null;

            var nextOpNo = scanned.OperationNo + 10;

            // Choose the first active production detail row that is for the same order AND next operation
            // (If your UI model stores details per order/op, adjust as needed.)
            var detail = frmOperator.shopOrderProductionDetails
                .FirstOrDefault(s => s.OrderNo == scanned.OrderNo && s.OperationNo == nextOpNo);

            if (detail == null)
            {
                // As a fallback, try any detail that belongs to the same next-op shop order at this form
                var nextOpShopOrder =
                    frmOperator.vw_ShopOrderGridModels.FirstOrDefault(x =>
                        x.orderNo == scanned.OrderNo && x.operationNo == nextOpNo);
                if (nextOpShopOrder != null)
                {
                    detail = frmOperator.shopOrderProductionDetails
                        .FirstOrDefault(s => s.ShopOrderOperationID == nextOpShopOrder.Id);
                }
            }

            return detail;
        }

        /// <summary>
        /// Brings the operator tab to front (no throws if tab not found).
        /// </summary>
        private static void ActivateOperatorTab(FrmOperator frmOperator)
        {
            try
            {
                var mdi = ToolsMdiManager.mdiManager;
                var page = mdi?.Pages?.FirstOrDefault(x => x.Text == frmOperator.Text);
                if (page != null) mdi.SelectedPage = page;
            }
            catch
            {
                // ignore UI activation failures
            }
        }

        /// <summary>
        /// Toggles the buzzer alarm line if configured.
        /// </summary>
        private static void SetBuzzer(bool on)
        {
            var node = ToolsMdiManager.frmOperatorActive?.panelDetail?.OPCNodeIdBuzzerAlarm;
            if (!string.IsNullOrEmpty(node))
            {
                try { StaticValues.opcClient.WriteNode(node, on); } catch { /* ignore */ }
            }
        }
    }
}
