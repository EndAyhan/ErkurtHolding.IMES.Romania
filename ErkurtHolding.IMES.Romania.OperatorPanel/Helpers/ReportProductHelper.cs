using DevExpress.XtraReports.UI;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Helper responsible for preparing datasource and printing **product** labels (single-unit labels).
    /// Supports:
    /// <list type="bullet">
    /// <item>Designer preview &amp; editing</item>
    /// <item>Direct print (with copy count, printer selection)</item>
    /// <item>Inkjet publishing via MQTT (optional queue)</item>
    /// </list>
    /// This class is safe for .NET Framework 4.8 WinForms usage.
    /// </summary>
    public class ReportProductHelper
    {
        private DataSet _dataSet;

        /// <summary>Printing configuration such as design file path, printer name, and copies.</summary>
        public PrintLabelModel printLabelModel { get; set; }

        /// <summary>Active shop order operation (view) that drives enabled/disabled printing.</summary>
        public vw_ShopOrderGridModel shopOrderOperation { get; set; }

        /// <summary>Header of the production being printed.</summary>
        public ShopOrderProduction shopOrderProduction { get; set; }

        /// <summary>Detail line of the product being printed (serial, dates, etc.).</summary>
        public ShopOrderProductionDetail shopOrderProductionDetail { get; set; }

        /// <summary>Current work center (machine).</summary>
        public Machine machine { get; set; }

        /// <summary>Current resource (station).</summary>
        public Machine resource { get; set; }

        /// <summary>Product master (part information).</summary>
        public Product product { get; set; }

        /// <summary>Current user model (optional, included in the dataset if provided).</summary>
        public UserModel userModel { get; set; }

        /// <summary>
        /// Lazily composed report <see cref="DataSet"/>. Each related object contributes one
        /// <see cref="DataTable"/> via the <c>CreateDataTable</c> extension.
        /// </summary>
        public DataSet dataSet
        {
            get
            {
                _dataSet = new DataSet();
                try
                {
                    _dataSet.Tables.Add(machine.CreateDataTable("WorkCenter"));
                    _dataSet.Tables.Add(shopOrderOperation.CreateDataTable());
                    _dataSet.Tables.Add(shopOrderProduction.CreateDataTable());
                    _dataSet.Tables.Add(shopOrderProductionDetail.CreateDataTable());
                    _dataSet.Tables.Add(resource.CreateDataTable("Resource"));
                    _dataSet.Tables.Add(StaticValues.branch.CreateDataTable("Branch"));
                    _dataSet.Tables.Add(product.CreateDataTable());
                    if (userModel != null)
                        _dataSet.Tables.Add(userModel.CreateDataTable());
                }
                catch
                {
                    // Dataset will contain whatever tables were successfully added.
                }
                return _dataSet;
            }
        }

        #region Designer / Preview

        /// <summary>
        /// Opens the label layout in the DevExpress designer after an admin check.
        /// </summary>
        public void PrintBarcodeDesigner()
        {
            if (!RequireAdmin()) return;

            try
            {
                if (!EnsureFileExists(printLabelModel?.LabelDesingFilePath)) return;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xr.ShowDesigner();
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        /// <summary>
        /// Opens a specific label file in the DevExpress designer after an admin check.
        /// </summary>
        /// <param name="myfilePath">Full path of the label layout to open.</param>
        public void PrintBarcodeDesigner(string myfilePath)
        {
            try
            {
                if (!RequireAdmin()) return;
                if (!EnsureFileExists(myfilePath)) return;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(myfilePath);
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xr.ShowDesigner();
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        /// <summary>
        /// Shows the label preview window with current data.
        /// </summary>
        public void PrintBarcodeView()
        {
            try
            {
                if (!EnsureFileExists(printLabelModel?.LabelDesingFilePath)) return;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xr.ShowPreview();
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        #endregion

        #region Print

        private bool isScrap;

        /// <summary>
        /// Prints the label to the configured printer. If the inkjet queue is enabled (by app setting),
        /// a JSON payload is published to the MQTT topic instead and local printing is skipped when successful.
        /// </summary>
        /// <param name="scrap">If <c>true</c>, logs print under the scrap print category.</param>
        public void PrintLabel(bool scrap = false)
        {
            // 1) Optional: publish to inkjet queue via MQTT
            if (string.Equals(StaticValues.inkjetPrintQueue, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                // We are in a sync method for backward compatibility; block on the async send.
                List<string> inkjetErrors;
                try
                {
                    inkjetErrors = PrintInkJetLabel().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    inkjetErrors = new List<string>
                    {
                        (MessageTextHelper.GetMessageText("RPRT", "112", "An error occurred during MQTT publish: {Message}", "Report"))
                            .Replace("{Message}", ex.Message)
                    };
                }

                if (inkjetErrors.Count == 0)
                {
                    // Successfully published to inkjet; skip local printing
                    return;
                }

                foreach (var error in inkjetErrors)
                    ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, error);
            }

            // 2) Local print into the selected printer
            try
            {
                if (!EnsureFileExists(printLabelModel?.LabelDesingFilePath)) return;

                // Guard critical dependencies to prevent NREs
                if (shopOrderOperation == null || machine == null || resource == null || product == null || printLabelModel == null)
                {
                    ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive,
                        MessageTextHelper.GetMessageText("RPRT", "113", "Missing report context data.", "Report"));
                    return;
                }

                isScrap = scrap;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xr.PrinterName = printLabelModel.printerName;
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                    // Respect shop order flags; allow explicit scrap prints
                    bool canPrint =
                        (printLabelModel.productionLabelType == ProductionLabelType.Process && shopOrderOperation.alan7 == "TRUE") ||
                        (printLabelModel.productionLabelType == ProductionLabelType.Product && shopOrderOperation.alan5 == "TRUE") ||
                        scrap;

                    if (canPrint)
                        xr.Print();
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        /// <summary>
        /// DevExpress printing pipeline hook: logs a print record, sets copy count, and shows a small "printing" dialog.
        /// </summary>
        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            try
            {
                if (isScrap)
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeScrap.Id, shopOrderProductionDetail.Id, machine.Id, resource.Id, printLabelModel.printerName);
                else
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeProductiongDetail.Id, shopOrderProductionDetail.Id, machine.Id, resource.Id, printLabelModel.printerName);

                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;

                using (var frm = new FrmPrinting())
                    frm.ShowDialog();
            }
            catch
            {
                // Swallow logging/printing UI errors to avoid blocking print.
            }
        }

        #endregion

        #region Inkjet (MQTT)

        /// <summary>
        /// Publishes the inkjet print payload to the targeted MQTT topic.
        /// Returns a list of human-readable error messages if anything goes wrong; otherwise an empty list.
        /// </summary>
        private async Task<List<string>> PrintInkJetLabel()
        {
            var errors = new List<string>();
            string inkjetName = string.Empty;

            try
            {
                var inkjetPrinter = PrinterMachineManager.Current
                    .GetPrinterMachine(resource.Id, StaticValues.specialCodeProductTypeInkjetProcess.Id);

                if (inkjetPrinter == null)
                {
                    errors.Add(MessageTextHelper.GetMessageText("RPRT", "107", "No inkjet printer mapping was found.", "Report"));
                }
                else
                {
                    var specialCode = SpecialCodeManager.Current.GetSpecialCodeById(inkjetPrinter.PrinterID);
                    if (specialCode == null)
                        errors.Add(MessageTextHelper.GetMessageText("RPRT", "108", "SpecialCode for inkjet printer could not be found.", "Report"));
                    else
                        inkjetName = specialCode.Name;
                }
            }
            catch (Exception ex)
            {
                var msg = (MessageTextHelper.GetMessageText("RPRT", "109", "Failed to get MQTT printer name: {Message}", "Report"))
                    .Replace("{Message}", ex.Message);
                errors.Add(msg);
            }

            if (string.IsNullOrEmpty(inkjetName))
                return errors;

            string jsonPayload;
            try
            {
                var payloadModel = InkJetPrintModel();
                jsonPayload = JsonConvert.SerializeObject(payloadModel);
            }
            catch (Exception ex)
            {
                var msg = (MessageTextHelper.GetMessageText("RPRT", "110", "Could not serialize label payload: {Message}", "Report"))
                    .Replace("{Message}", ex.Message);
                errors.Add(msg);
                return errors;
            }

            try
            {
                bool success = await MqttHelper.PublishAsync(inkjetName, jsonPayload);
                if (!success)
                    errors.Add(MessageTextHelper.GetMessageText("RPRT", "111", "MQTT publish failed.", "Report"));
            }
            catch (Exception ex)
            {
                var msg = (MessageTextHelper.GetMessageText("RPRT", "112", "An error occurred during MQTT publish: {Message}", "Report"))
                    .Replace("{Message}", ex.Message);
                errors.Add(msg);
            }

            return errors;
        }

        /// <summary>
        /// Creates the inkjet payload model for MQTT publishing.
        /// </summary>
        private InkJetModel InkJetPrintModel()
        {
            return new InkJetModel
            {
                PartName = product.Description,
                PartNo = product.PartNo,
                OrderNo = shopOrderOperation.orderNo,
                SerialNo = shopOrderProductionDetail.serial.ToString(),
                WorkCenterName = machine.resourceName,
                WorkCenter = machine.Definition,
                Company = "FORMFLEKS A.Ş",
                ProductionCreatedDate = shopOrderProductionDetail.EndDate.ToString("dd-MM-yyyy HH:mm:ss"),
                SerialPrivateNo = $"{product.dimQuality}_{shopOrderProductionDetail.serial}"
            };
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Ensures the given layout file exists; shows a localized message if not.
        /// </summary>
        private static bool EnsureFileExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                return true;

            var msg = MessageTextHelper.GetMessageText(
                "RPRT",
                "105",
                "The label file path cannot be accessed.\r\nPlease contact your system administrator.",
                "Report");

            // The owner can be null; XtraMessageBox handles it.
            ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
            return false;
        }

        /// <summary>
        /// Opens an admin login dialog and returns true only if user passed authentication.
        /// Shows a localized message on failure.
        /// </summary>
        private static bool RequireAdmin()
        {
            using (var frm = new FrmAdminLogin())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    return true;

                var msg = MessageTextHelper.GetMessageText("RPRT", "106", "Admin password is incorrect.", "Report");
                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
                return false;
            }
        }

        #endregion
    }
}