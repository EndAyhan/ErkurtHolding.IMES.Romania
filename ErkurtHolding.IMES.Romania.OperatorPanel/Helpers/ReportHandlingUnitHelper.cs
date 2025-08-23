using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Builds the dataset for label reports and provides helper methods to preview/design/print.
    /// </summary>
    public class ReportHandlingUnitHelper
    {
        public PrintLabelModel printLabelModel { get; set; }
        public Product product { get; set; }
        public ShopOrderOperation shopOrderOperation { get; set; }
        public vw_ShopOrderGridModel vwShopOrderGridModel { get; set; }
        public Machine machine { get; set; }
        public Machine resource { get; set; }
        public ShopOrderProduction shopOrderProduction { get; set; }
        public List<ShopOrderProductionDetail> shopOrderProductionDetails { get; set; }
        public HandlingUnit handlingUnit { get; set; }
        public UserModel user { get; set; }

        private DataSet _dataSet;

        /// <summary>
        /// Builds a new DataSet on each access with all related entities as tables.
        /// </summary>
        public DataSet dataSet
        {
            get
            {
                _dataSet = new DataSet();
                try
                {
                    _dataSet.Tables.Add(product.CreateDataTable("Product"));
                    _dataSet.Tables.Add(shopOrderOperation.CreateDataTable());
                    _dataSet.Tables.Add(vwShopOrderGridModel.CreateDataTable("ViewShopOrderGridModel"));
                    _dataSet.Tables.Add(machine.CreateDataTable("WorkCenter"));
                    _dataSet.Tables.Add(resource.CreateDataTable("Resource"));
                    _dataSet.Tables.Add(StaticValues.branch.CreateDataTable("Branch"));

                    _dataSet.Tables.Add(handlingUnit.CreateDataTable());
                    _dataSet.Tables.Add(shopOrderProductionDetails.CreateDataTable());
                    _dataSet.Tables.Add(shopOrderProduction.CreateDataTable());
                    if (user != null)
                        _dataSet.Tables.Add(user.CreateDataTable());
                }
                catch
                {
                    // swallow: dataset will contain what succeeded
                }

                return _dataSet;
            }
        }

        /// <summary>
        /// Loads the configured label layout and shows a preview window.
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

        /// <summary>
        /// Prompts for admin login, then opens the configured label layout in the designer.
        /// </summary>
        public void PrintBarcodeDesigner()
        {
            try
            {
                if (!RequireAdmin()) return;
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
        /// Prompts for admin login, then opens the given layout file in the designer.
        /// </summary>
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
        /// Prompts for admin login, then opens a user-selected layout file in the designer.
        /// </summary>
        public void PrintBarcodeSelectFileDesigner(string myfilePath)
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
        /// Loads the given layout and prints directly to the specified printer (if allowed by order flags).
        /// </summary>
        public void PrintBarcodeDesigner(string myfilePath, string myPrinterName)
        {
            try
            {
                if (!EnsureFileExists(myfilePath)) return;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(myfilePath);
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                    if (shopOrderOperation != null && shopOrderOperation.alan6 == "TRUE")
                        xr.Print(myPrinterName);
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }


        /// <summary>
        /// Prints using the configured layout and printer in <see cref="printLabelModel"/>.
        /// </summary>
        public void PrintLabel()
        {
            try
            {
                if (printLabelModel == null) return;

                // ensure the layout file exists before proceeding (shows a localized message if missing)
                if (!EnsureFileExists(printLabelModel.LabelDesingFilePath)) return;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xr.PrinterName = printLabelModel.printerName;
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                    if (shopOrderOperation != null && shopOrderOperation.alan6 == "TRUE")
                        xr.Print();
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        // -------------------- internals --------------------

        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            try
            {
                PrintLogManager.Current.AddLog(
                    StaticValues.specialCodePrintLogTypeHandlingUnit.Id,
                    handlingUnit.Id,
                    machine.Id,
                    resource.Id,
                    printLabelModel.printerName);

                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;

                using (var frm = new FrmPrinting())
                {
                    frm.ShowDialog();
                }
            }
            catch
            {
                // Ignore logging/printing UI errors to avoid breaking print
            }
        }

        /// <summary>
        /// Validates the file path and shows a localized message if it’s missing.
        /// </summary>
        private static bool EnsureFileExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                return true;

            // Localized info message
            var msg = StaticValues.T["report.file_not_found"];
            if (string.IsNullOrEmpty(msg))
            {
                // Turkish fallback (legacy)
                msg = "Etiket dosya yoluna ulaşılamıyor.\r\nLütfen sistem yöneticinize başvurunuz";
            }
            ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
            return false;
        }

        /// <summary>
        /// Prompts for admin login; shows a localized warning if login fails.
        /// </summary>
        private static bool RequireAdmin()
        {
            using (var frm = new FrmAdminLogin())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    return true;

                var msg = StaticValues.T["report.admin_wrong_password"];
                if (string.IsNullOrEmpty(msg))
                {
                    // Turkish fallback (legacy)
                    msg = "Admin şifresi yanlış";
                }
                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
                return false;
            }
        }
    }
}
