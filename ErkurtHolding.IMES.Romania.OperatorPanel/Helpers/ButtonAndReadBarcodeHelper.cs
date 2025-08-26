using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;
using System.Threading;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Handles "Button + Read Barcode" flow:
    /// - Validates the scanned serial
    /// - Finds the corresponding production detail on any open operator form
    /// - Sets the production detail's barcode if needed
    /// - Triggers counter increments and handshake (when required)
    /// - Updates UI counters via an <see cref="OpcOtherReadModel"/>
    /// - Enforces panel/business rules and throws localized errors when something is off
    /// </summary>
    public static class ButtonAndReadBarcodeHelper
    {
        /// <summary>
        /// Main entry point for "button + read barcode" logic.
        /// </summary>
        /// <param name="Serial">Scanned serial or product barcode text.</param>
        /// <param name="factorCounter">Current factor counter value from PLC/process.</param>
        /// <param name="shopOrderOperationFactor">Required factor threshold for handshake.</param>
        /// <exception cref="Exception">Throws localized errors for all failure scenarios.</exception>
        public static void ButtonAndReadBarcodeControl(string Serial, int factorCounter, int shopOrderOperationFactor)
        {
            // Basic input validations
            if (string.IsNullOrWhiteSpace(Serial))
                throw new Exception(MessageTextHelper.GetMessageText("000", "617", "Okuttuğunuz barkod ürün barkoduna ait değildir.", "Message"));

            // Serial must be numeric (backward-compatible with your logic)
            if (!long.TryParse(Serial, out _))
                throw new Exception(MessageTextHelper.GetMessageText("000", "617", "Okuttuğunuz barkod ürün barkoduna ait değildir.", "Message"));

            // Walk all open operator forms; stop at the first one that matches the configured read type and holds this serial
            bool processed = false;

            foreach (var frmOperator in ToolsMdiManager.frmOperators)
            {
                if (frmOperator?.panelDetail == null ||
                    frmOperator.panelDetail.DataReadParameterID != StaticValues.specialCodeCounterReadTypeButtonAndReadBarcode.Id)
                    continue;

                // Need existing production details
                if (!frmOperator.productionDetails.HasEntries())
                    continue;

                var selectedProductionDetail = frmOperator.productionDetails.FirstOrDefault(x => x.serial.ToString() == Serial);
                if (selectedProductionDetail == null)
                    continue;

                // Found a matching form + detail; handle it here
                processed = true;

                // Already matched serial used as barcode?
                if (selectedProductionDetail.serial.ToString() == selectedProductionDetail.Barcode)
                    throw new Exception(MessageTextHelper.GetMessageText("000", "615", "Okuttuğunuz ürünü tekrar okutamazsınız", "Message"));

                // Save barcode and persist
                selectedProductionDetail.Barcode = Serial;
                ShopOrderProductionDetailManager.Current.UpdateBarcode(selectedProductionDetail);

                // Rebuild UI data/state for this form
                frmOperator.CreateShopOrderProductionDetails();

                // When factor threshold is met, do the handshake loop (unchanged behavior, with small safety guards)
                if (factorCounter == shopOrderOperationFactor)
                    PerformHandshakeIfConfigured(frmOperator);

                // Business/UI flows after successful barcode handling
                var selectedProduct = frmOperator.products.First(x => x.Id == selectedProductionDetail.ProductID);

                // Update the "read count" UI (lblValue9) via OPC model
                frmOperator.SetLabelOpcOtherreadValue(CreateOpcReadModel());

                // Proceed with quota checks & next steps
                frmOperator.PartHandlingUnitMaxQuotaCheckButtonAndReadBarcode(selectedProduct, selectedProductionDetail);
            }

            if (!processed)
            {
                // Either not found on any operator form, or form not configured for this read type
                throw new Exception(MessageTextHelper.GetMessageText("000", "618", "Okuttuğunuz barkod ilgili üretime ait değildir.", "Message"));
            }
        }

        /// <summary>
        /// Attempts to perform the HandShake loop when configured on the active form.
        /// Keeps the original timing (200 * 30ms = ~6s) and throws an error if it can’t complete.
        /// </summary>
        private static void PerformHandshakeIfConfigured(FrmOperator frmOperator)
        {
            var handShakeNode = frmOperator?.opcOtherReadModels?.FirstOrDefault(x => x.NodeId != null && x.NodeId.Contains("HandShake"))?.NodeId;
            if (string.IsNullOrEmpty(handShakeNode))
                return; // No handshake configured; nothing to do.

            // Original semantics preserved: add a counter, poll the handshake node until it becomes "1" or we time out.
            const int maxIterations = 200;   // ~6 seconds
            const int sleepMs = 30;

            for (int i = 0; i < maxIterations; i++)
            {
                Thread.Sleep(sleepMs);

                // Increment the counter on each loop just like your original code
                StaticValues.opcClient.AddCounter(frmOperator.panelDetail.OPCNodeIdCounterIncrement, 1);

                var value = StaticValues.opcClient.ReadNode(handShakeNode);
                if (value == "1")
                    return; // Handshake acknowledged
            }

            // Timed out
            throw new Exception(MessageTextHelper.GetMessageText("000", "616", "Network probleminden dolayı devam edilemiyor. Lütfen sistem yöneticinize başvurun..!", "Message"));
        }

        /// <summary>
        /// Builds the OPC model that updates the "read product count" UI indicator.
        /// </summary>
        private static OpcOtherReadModel CreateOpcReadModel()
        {
            var localizedDescription = MessageTextHelper.GetMessageText("000", "100", "Number of Products Read", "Message");

            return new OpcOtherReadModel
            {
                SpecialCodeId = StaticValues.specialCodeCounterReadTypeButtonAndReadBarcode.Id,
                Description = localizedDescription, // shown in UI
                Location = "lblValue9",             // UI target (kept as-is)
                readValue = 1
            };
        }
    }
}
