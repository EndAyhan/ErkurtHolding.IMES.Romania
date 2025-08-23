using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using Newtonsoft.Json;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class ReportProductHelper
    {
        private DataSet _dataSet;

        public PrintLabelModel printLabelModel { get; set; }
        public vw_ShopOrderGridModel shopOrderOperation { get; set; }
        public ShopOrderProduction shopOrderProduction { get; set; }
        public ShopOrderProductionDetail shopOrderProductionDetail { get; set; }
        public Machine machine { get; set; }
        public Machine resource { get; set; }
        public Product product { get; set; }

        public UserModel userModel { get; set; }

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
                    // dataset will contain whatever succeeded
                }
                return _dataSet;
            }
        }

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

        public void PrintBarcodeView(bool view)
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

        private bool isScrap;

        public async void PrintLabel(bool scrap = false)
        {
            // Inkjet queue (only when explicitly TRUE)
            if (string.Equals(StaticValues.inkjetPrintQueue, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                var inkjetErrors = await PrintInkJetLabel();
                if (inkjetErrors.Count == 0)
                {
                    // printed via MQTT; skip local print
                    return;
                }
                foreach (var error in inkjetErrors)
                    ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, error);
            }

            try
            {
                if (!EnsureFileExists(printLabelModel?.LabelDesingFilePath)) return;

                isScrap = scrap;
                using (var xr = new XtraReport())
                {
                    xr.DataSource = dataSet;
                    xr.LoadLayout(printLabelModel.LabelDesingFilePath);
                    xr.PrinterName = printLabelModel.printerName;
                    xr.ShowPrintStatusDialog = false;
                    xr.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                    if ((printLabelModel.productionLabelType == ProductionLabelType.Process && shopOrderOperation.alan7 == "TRUE") ||
                        (printLabelModel.productionLabelType == ProductionLabelType.Product && shopOrderOperation.alan5 == "TRUE") ||
                        scrap)
                    {
                        xr.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(ToolsMdiManager.frmOperatorActive, ex);
            }
        }

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
                    errors.Add(StaticValues.T["inkjet.no_printer_map"] ?? "Inkjet printer tanımı bulunamadı.");
                }
                else
                {
                    var specialCode = SpecialCodeManager.Current.GetSpecialCodeById(inkjetPrinter.PrinterID);
                    if (specialCode == null)
                        errors.Add(StaticValues.T["inkjet.no_specialcode"] ?? "Inkjet printer için SpecialCode bulunamadı.");
                    else
                        inkjetName = specialCode.Name;
                }
            }
            catch (Exception ex)
            {
                var msg = (StaticValues.T["inkjet.error_printer_name"] ?? "MQTT yazıcı ismi alınamadı: {Message}")
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
                var msg = (StaticValues.T["inkjet.serialize_failed"] ?? "Etiket verisi serialize edilemedi: {Message}")
                    .Replace("{Message}", ex.Message);
                errors.Add(msg);
                return errors;
            }

            try
            {
                bool success = await MqttHelper.PublishAsync(inkjetName, jsonPayload);
                if (!success)
                    errors.Add(StaticValues.T["inkjet.mqtt_failed"] ?? "MQTT gönderimi başarısız oldu.");
            }
            catch (Exception ex)
            {
                var msg = (StaticValues.T["inkjet.mqtt_exception"] ?? "MQTT gönderimi sırasında hata oluştu: {Message}")
                    .Replace("{Message}", ex.Message);
                errors.Add(msg);
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

                using (var frm = new FrmPrinting())
                    frm.ShowDialog();
            }
            catch
            {
                // ignore logging/printing UI errors
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

        // -------- helpers --------

        private static bool EnsureFileExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                return true;

            var msg = StaticValues.T["report.file_not_found"];
            if (string.IsNullOrEmpty(msg))
                msg = "Etiket dosya yoluna ulaşılamıyor.\r\nLütfen sistem yöneticinize başvurunuz";
            ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
            return false;
        }

        private static bool RequireAdmin()
        {
            using (var frm = new FrmAdminLogin())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    return true;

                var msg = StaticValues.T["report.admin_wrong_password"];
                if (string.IsNullOrEmpty(msg))
                    msg = "Admin şifresi yanlış";
                ToolsMessageBox.Information(ToolsMdiManager.frmOperatorActive, msg);
                return false;
            }
        }
    }
}
