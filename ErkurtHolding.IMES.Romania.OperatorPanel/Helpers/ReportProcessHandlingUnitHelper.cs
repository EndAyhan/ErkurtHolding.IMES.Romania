using DevExpress.XtraReports.UI;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System.Collections.Generic;
using System.Data;
using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using System.Windows.Forms;
using System.Linq;

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
                }

                return _dataSet;
            }
        }

        #region METHODS
        public void PrintBarcodeView()
        {
            try
            {
                if (System.IO.File.Exists(printLabelModel.LabelDesingFilePath))
                {
                    XtraReport xtraReport = new XtraReport();
                    xtraReport.DataSource = dataSet;
                    xtraReport.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xtraReport.ShowPrintStatusDialog = false;
                    xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xtraReport.ShowPreview();
                }
                else
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Etiket dosya yoluna ulaşılamıyor.\r\nLütfen sistem yöneticinize başvurunuz");
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
                FrmAdminLogin frm = new FrmAdminLogin();
                if (frm.ShowDialog() != DialogResult.OK)
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Admin şifresi yanlış");

                if (System.IO.File.Exists(printLabelModel.LabelDesingFilePath))
                {
                    XtraReport xtraReport = new XtraReport();
                    xtraReport.DataSource = dataSet;
                    xtraReport.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xtraReport.ShowPrintStatusDialog = false;
                    xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xtraReport.ShowDesigner();
                }
                else
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Etiket dosya yoluna ulaşılamıyor.\r\nLütfen sistem yöneticinize başvurunuz");
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
                FrmAdminLogin frm = new FrmAdminLogin();
                if (frm.ShowDialog() != DialogResult.OK)
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Admin şifresi yanlış");

                if (System.IO.File.Exists(printLabelModel.LabelDesingFilePath))
                {
                    XtraReport xtraReport = new XtraReport();
                    xtraReport.DataSource = dataSet;
                    xtraReport.LoadLayout(myfilePath);
                    xtraReport.ShowPrintStatusDialog = false;
                    xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xtraReport.ShowDesigner();
                }
                else
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Etiket dosya yoluna ulaşılamıyor.\r\nLütfen sistem yöneticinize başvurunuz");
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
                FrmAdminLogin frm = new FrmAdminLogin();
                if (frm.ShowDialog() != DialogResult.OK)
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Admin şifresi yanlış");

                if (System.IO.File.Exists(myfilePath))
                {
                    XtraReport xtraReport = new XtraReport();
                    xtraReport.DataSource = dataSet;
                    xtraReport.LoadLayout(myfilePath);
                    xtraReport.ShowPrintStatusDialog = false;
                    xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xtraReport.ShowDesigner();
                }
                else
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Etiket dosya yoluna ulaşılamıyor.\r\nLütfen sistem yöneticinize başvurunuz");
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
                XtraReport xtraReport = new XtraReport();
                xtraReport.DataSource = dataSet;
                xtraReport.LoadLayout(myfilePath);
                xtraReport.ShowPrintStatusDialog = false;
                xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                if (shopOrderOperations.Any(x => x.alan6 == "TRUE"))
                    xtraReport.Print(myPrinterName);
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
                if (System.IO.File.Exists(printLabelModel.LabelDesingFilePath))
                {
                    XtraReport xtraReport = new XtraReport();
                    xtraReport.DataSource = dataSet;
                    xtraReport.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xtraReport.PrinterName = printLabelModel.printerName;
                    xtraReport.ShowPrintStatusDialog = false;
                    xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                    if (shopOrderOperations.Any(x => x.alan6 == "TRUE"))
                        xtraReport.Print();
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
                if (handlingUnits.Count > 0)
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeProsesHandlingUnit.Id, handlingUnits[0].Id, machine.Id, resource.Id, printLabelModel.printerName);
                else
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeProsesHandlingUnit.Id, Guid.Empty, machine.Id, resource.Id, printLabelModel.printerName);
                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;
                FrmPrinting frm = new FrmPrinting();
                frm.ShowDialog();
            }
            catch
            {
            }
        }
        #endregion
    }
}
