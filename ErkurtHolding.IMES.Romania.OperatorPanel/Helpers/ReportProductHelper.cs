using DevExpress.XtraReports.UI;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class ReportProductHelper
    {
        //public static string printerName = "TSC TTP-244CE";
        ////public static string filePath = @"D:\Erkurt\ErkurttestProductLabel.repx";
        //public static string filePath = ConfigurationManager.AppSettings["BarcodeFilePath"];

        private DataSet _dataSet;
        public PrintLabelModel printLabelModel { get; set; }
        public vw_ShopOrderGridModel shopOrderOperation { get; set; }
        public ShopOrderProduction shopOrderProduction { get; set; }

        public ShopOrderProductionDetail shopOrderProductionDetail { get; set; }

        public Machine machine { get; set; }

        public Machine resource { get; set; }
        public Product product { get; set; }
        public Branch branch
        {
            get { return StaticValues.branch; }
        }

        public UserModel userModel { get; set; }
        public DataSet dataSet
        {
            get
            {
                _dataSet = new DataSet();

                _dataSet.Tables.Add(machine.CreateDataTable("WorkCenter"));
                _dataSet.Tables.Add(shopOrderOperation.CreateDataTable());
                _dataSet.Tables.Add(shopOrderProduction.CreateDataTable());
                _dataSet.Tables.Add(shopOrderProductionDetail.CreateDataTable());
                _dataSet.Tables.Add(resource.CreateDataTable("Resource"));
                _dataSet.Tables.Add(branch.CreateDataTable("Branch"));
                _dataSet.Tables.Add(product.CreateDataTable());
                _dataSet.Tables.Add(userModel.CreateDataTable());
                return _dataSet;
            }

        }
        public void PrintBarcodeDesigner()
        {
            FrmAdminLogin frm = new FrmAdminLogin();
            if (frm.ShowDialog() != DialogResult.OK)
                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Admin şifresi yanlış");

            try
            {
                XtraReport xtraReport = new XtraReport();
                xtraReport.DataSource = dataSet;
                xtraReport.LoadLayout(printLabelModel.LabelDesingFilePath);
                xtraReport.ShowPrintStatusDialog = false;
                xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                xtraReport.ShowDesigner();
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
                {
                    ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, "Admin şifresi yanlış");
                    return;
                }

                XtraReport xtraReport = new XtraReport();
                xtraReport.DataSource = dataSet;
                xtraReport.LoadLayout(myfilePath);
                xtraReport.ShowPrintStatusDialog = false;
                xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                xtraReport.ShowDesigner();
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }
        public void PrintBarcodeView(bool view)
        {
            try
            {
                XtraReport xtraReport = new XtraReport();
                xtraReport.DataSource = dataSet;
                xtraReport.LoadLayout(printLabelModel.LabelDesingFilePath);
                xtraReport.ShowPrintStatusDialog = false;
                xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                xtraReport.ShowPreview();
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        private bool isScrap;
        public async void PrintLabel(bool scrap = false)
        {
            if (StaticValues.inkjetPrintQueue == "TRUE" || StaticValues.inkjetPrintQueue != null)
            {
                var inkjetErrors = await PrintInkJetLabel();
                if (inkjetErrors.Count == 0)
                {
                    return;
                }
                foreach (var error in inkjetErrors)
                {
                    ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, error);
                }
            }

            try
            {
                isScrap = scrap;
                XtraReport xtraReport = new XtraReport();
                xtraReport.DataSource = dataSet;
                xtraReport.LoadLayout(printLabelModel.LabelDesingFilePath);
                xtraReport.PrinterName = printLabelModel.printerName;
                xtraReport.ShowPrintStatusDialog = false;
                xtraReport.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                if (
                    (printLabelModel.productionLabelType == ProductionLabelType.Process && shopOrderOperation.alan7 == "TRUE") ||
                    (printLabelModel.productionLabelType == ProductionLabelType.Product && shopOrderOperation.alan5 == "TRUE") || scrap
                    )
                    xtraReport.Print();
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

        private async Task<List<string>> PrintInkJetLabel()
        {
            List<string> errors = new List<string>();
            string inkjetName = string.Empty;

            try
            {
                var inkjetPrinterId = PrinterMachineManager.Current.GetPrinterMachine(resource.Id, StaticValues.specialCodeProductTypeInkjetProcess.Id);
                if (inkjetPrinterId == null)
                    errors.Add("Inkjet printer tanımı bulunamadı.");
                else
                {
                    var specialCode = SpecialCodeManager.Current.GetSpecialCodeById(inkjetPrinterId.PrinterID);
                    if (specialCode == null)
                        errors.Add("Inkjet printer için SpecialCode bulunamadı.");
                    else
                        inkjetName = specialCode.Name;
                }
            }
            catch (Exception ex)
            {
                errors.Add($"MQTT yazıcı ismi alınamadı: {ex.Message}");
            }

            if (string.IsNullOrEmpty(inkjetName))
                return errors;

            string jsonPayload = string.Empty;
            try
            {
                var denemeModel = InkJetPrintModel();
                jsonPayload = JsonConvert.SerializeObject(denemeModel);
            }
            catch (Exception ex)
            {
                errors.Add($"Etiket verisi serialize edilemedi: {ex.Message}");
                return errors;
            }

            try
            {
                bool success = await MqttHelper.PublishAsync(inkjetName, jsonPayload);
                if (!success)
                {
                    errors.Add("MQTT gönderimi başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                errors.Add($"MQTT gönderimi sırasında hata oluştu: {ex.Message}");
            }

            return errors;
        }


        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            try
            {
                if (isScrap)
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeScrap.Id, shopOrderProductionDetail.Id, machine.Id, resource.Id, printLabelModel.printerName);
                else
                    PrintLogManager.Current.AddLog(StaticValues.specialCodePrintLogTypeProductiongDetail.Id, shopOrderProductionDetail.Id, machine.Id, resource.Id, printLabelModel.printerName);
                e.PrintDocument.PrinterSettings.Copies = printLabelModel.PrintCopyCount;
                FrmPrinting frm = new FrmPrinting();
                frm.ShowDialog();
            }
            catch
            {

            }
        }

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


    }

}
