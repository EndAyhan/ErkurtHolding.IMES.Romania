using DevExpress.XtraReports.UI;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Prepares data and handles DevExpress report interactions for
    /// **process / handling unit** labels (kasa / proses etiketleri).
    /// Provides:
    /// <list type="bullet">
    ///   <item>Preview (ShowPreview)</item>
    ///   <item>Designer (ShowDesigner) with optional Admin-gated access</item>
    ///   <item>Direct printing with copy count and print logging</item>
    /// </list>
    /// The helper is defensive against missing files and null references and
    /// displays localized user messages when necessary.
    /// </summary>
    public class ReportProcessHandlingUnitHelper
    {
        /// <summary>
        /// Initializes lists that back the report dataset (avoids null checks downstream).
        /// </summary>
        public ReportProcessHandlingUnitHelper()
        {
            shopOrderOperations = new List<ShopOrderOperation>();
            vwShopOrderGridModels = new List<vw_ShopOrderGridModel>();
            shopOrderProductionDetails = new List<ShopOrderProductionDetail>();
            handlingUnits = new List<HandlingUnit>();
        }

        /// <summary>Printing configuration (design path, printer name, copy count).</summary>
        public PrintLabelModel printLabelModel { get; set; }

        /// <summary>Product master (used by designs that bind product fields).</summary>
        public Product product { get; set; }

        /// <summary>Operations that participate in this process print run.</summary>
        public List<ShopOrderOperation> shopOrderOperations { get; set; }

        /// <summary>Shop order view rows for the current run.</summary>
        public List<vw_ShopOrderGridModel> vwShopOrderGridModels { get; set; }

        /// <summary>Work center (machine) context of the print.</summary>
        public Machine machine { get; set; }

        /// <summary>Resource (station) context of the print.</summary>
        public Machine resource { get; set; }

        /// <summary>Header of the production for which labels are printed.</summary>
        public ShopOrderProduction shopOrderProduction { get; set; }

        /// <summary>Detail rows (e.g., individual quantities/serials) included in the report datasource.</summary>
        public List<ShopOrderProductionDetail> shopOrderProductionDetails { get; set; }

        /// <summary>Handling units (kasa) to be printed/logged.</summary>
        public List<HandlingUnit> handlingUnits { get; set; }

        /// <summary>Optional user (included in dataset when present).</summary>
        public UserModel user { get; set; }

        private DataSet _dataSet;

        /// <summary>
        /// Lazily builds a <see cref="DataSet"/> for the report by converting the configured
        /// objects/lists into <see cref="DataTable"/>s via <c>CreateDataTable</c> extensions.
        /// Only successfully transformed tables are added; failures are ignored to keep the
        /// report operational with partial data when necessary.
        /// </summary>
        public DataSet dataSet
        {
            get
            {
                _dataSet = new DataSet();
                try
                {
                    _dataSet.Tables.Add(product.CreateDataTable("Product"));
                    _dataSet.Tables.Add(shopOrderOperations.CreateDataTable());
                    _dataSet.Tables.Add(vwShopOrderGridModels.CreateDataTable("ViewShopOrderGridModel"));
                    _dataSet.Tables.Add(machine.CreateDataTable("WorkCenter"));
                    _dataSet.Tables.Add(resource.CreateDataTable("Resource"));
                    _dataSet.Tables.Add(StaticValues.branch.CreateDataTable("Branch"));

                    _dataSet.Tables.Add(handlingUnits.CreateDataTable());
                    _dataSet.Tables.Add(shopOrderProductionDetails.CreateDataTable());
                    _dataSet.Tables.Add(shopOrderProduction.CreateDataTable());
                    if (user != null)
                        _dataSet.Tables.Add(user.CreateDataTable());
                }
                catch
                {
                    // dataset will contain what succeeded
                }

                return _dataSet;
            }
        }

        #region Public API

        /// <summary>
        /// Loads the configured layout and shows the report **preview** with current data.
        /// Displays a localized warning if the file path is not accessible.
        /// </summary>
        public void PrintBarcodeView()
        {
            try
            {
                if (printLabelModel == null) return;
                if (!EnsureFileExists(printLabelModel.LabelDesingFilePath)) return;

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
        /// Opens the configured layout in the DevExpress **Designer** after an Admin check.
        /// </summary>
        public void PrintBarcodeDesigner()
        {
            try
            {
                if (printLabelModel == null) return;
                if (!RequireAdmin()) return;
                if (!EnsureFileExists(printLabelModel.LabelDesingFilePath)) return;

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
        /// Opens a **specific** layout file in the Designer after an Admin check.
        /// </summary>
        /// <param name="myfilePath">Absolute path to the report layout file.</param>
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
        /// Opens a user-selected layout file directly in the Designer (Admin check included).
        /// </summary>
        /// <param name="myfilePath">Absolute path selected by the user.</param>
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
        /// Prints using a **specific** layout file and **explicit** printer name.
        /// Skips printing unless any operation allows label printing (<c>alan6 == "TRUE"</c>).
        /// </summary>
        /// <param name="myfilePath">Absolute path to the layout to load.</param>
        /// <param name="myPrinterName">Target printer name.</param>
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

                    if (shopOrderOperations != null && shopOrderOperations.Any(x => x.alan6 == "TRUE"))
                        xr.Print(myPrinterName);
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        /// <summary>
        /// Prints using the layout/printer settings in <see cref="printLabelModel"/>.
        /// Respects shop order flag (<c>alan6 == "TRUE"</c>) before issuing the print command.
        /// </summary>
        public void PrintLabel()
        {
            try
            {
                if (printLabelModel == null) return;
                if (!EnsureFileExists(printLabelModel.LabelDesingFilePath)) return;

                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xr.PrinterName = printLabelModel.printerName;
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                    if (shopOrderOperations != null && shopOrderOperations.Any(x => x.alan6 == "TRUE"))
                        xr.Print();
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        #endregion

        #region DevExpress Printing Hook

        /// <summary>
        /// DevExpress printing hook — sets copy count and writes a print log entry.
        /// </summary>
        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            try
            {
                if (handlingUnits != null && handlingUnits.Count > 0)
                    PrintLogManager.Current.AddLog(
                        StaticValues.specialCodePrintLogTypeProsesHandlingUnit.Id,
                        handlingUnits[0].Id, machine.Id, resource.Id, printLabelModel.printerName);
                else
                    PrintLogManager.Current.AddLog(
                        StaticValues.specialCodePrintLogTypeProsesHandlingUnit.Id,
                        Guid.Empty, machine.Id, resource.Id, printLabelModel.printerName);

                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;

                using (var frm = new FrmPrinting())
                {
                    frm.ShowDialog();
                }
            }
            catch
            {
                // Logging/UX errors must not block printing
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Ensures a file path exists and shows a localized information message if it does not.
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

            ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
            return false;
        }

        /// <summary>
        /// Shows an Admin login dialog and returns <c>true</c> only upon success.
        /// Displays a localized message on failure.
        /// </summary>
        private static bool RequireAdmin()
        {
            using (var frm = new FrmAdminLogin())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    return true;

                var msg = MessageTextHelper.GetMessageText(
                    "RPRT",
                    "106",
                    "Admin password is incorrect.",
                    "Report");

                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
                return false;
            }
        }

        #endregion
    }
}
