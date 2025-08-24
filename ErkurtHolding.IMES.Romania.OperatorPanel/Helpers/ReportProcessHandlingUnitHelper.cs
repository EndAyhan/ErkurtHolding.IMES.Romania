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
    public class ReportProcessHandlingUnitHelper
    {
        public ReportProcessHandlingUnitHelper()
        {
            shopOrderOperations = new List<ShopOrderOperation>();
            vwShopOrderGridModels = new List<vw_ShopOrderGridModel>();
            shopOrderProductionDetails = new List<ShopOrderProductionDetail>();
            handlingUnits = new List<HandlingUnit>();
        }

        public PrintLabelModel printLabelModel { get; set; }
        public Product product { get; set; }
        public List<ShopOrderOperation> shopOrderOperations { get; set; }
        public List<vw_ShopOrderGridModel> vwShopOrderGridModels { get; set; }
        public Machine machine { get; set; }
        public Machine resource { get; set; }
        public ShopOrderProduction shopOrderProduction { get; set; }
        public List<ShopOrderProductionDetail> shopOrderProductionDetails { get; set; }
        public List<HandlingUnit> handlingUnits { get; set; }
        public UserModel user { get; set; }

        private DataSet _dataSet;
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

        #region METHODS

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

        public void PrintBarcodeDesigner(string myfilePath)
        {
            try
            {
                if (!RequireAdmin()) return;
                if (!EnsureFileExists(myfilePath)) return; // FIX: check the path we will load

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

        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            try
            {
                if (handlingUnits != null && handlingUnits.Count > 0)
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeProsesHandlingUnit.Id, handlingUnits[0].Id, machine.Id, resource.Id, printLabelModel.printerName);
                else
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeProsesHandlingUnit.Id, Guid.Empty, machine.Id, resource.Id, printLabelModel.printerName);

                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;

                using (var frm = new FrmPrinting())
                {
                    frm.ShowDialog();
                }
            }
            catch
            {
                // ignore logging UI errors
            }
        }

        #endregion

        #region helpers

        private static bool EnsureFileExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                return true;

            var msg = MessageTextHelper.GetMessageText("RPRT", "105", "The label file path cannot be accessed.\r\nPlease contact your system administrator.", "Report");

            ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
            return false;
        }

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
