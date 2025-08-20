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

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
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
                }

                return _dataSet;
            }
        }

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
                else
                {
                    XtraReport xtraReport = new XtraReport();
                    xtraReport.DataSource = dataSet;
                    xtraReport.LoadLayout(myfilePath);
                    xtraReport.ShowPrintStatusDialog = false;
                    xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    xtraReport.ShowDesigner();
                }

                if (System.IO.File.Exists(myfilePath))
                {
                    return;
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
                if (shopOrderOperation.alan6 == "TRUE")
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

                    if (shopOrderOperation.alan6 == "TRUE")
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
                PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeHandlingUnit.Id, handlingUnit.Id, machine.Id, resource.Id, printLabelModel.printerName);
                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;
                FrmPrinting frm = new FrmPrinting();
                frm.ShowDialog();
            }
            catch
            {
            }
        }
    }
}
