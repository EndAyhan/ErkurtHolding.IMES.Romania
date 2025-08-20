using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class PrintLabelHelper
    {
        public static PrintLabelModel GetLabelModel(Product product, Machine machine, Machine resource, ProductionLabelType productionLabelType, bool flag = true)
        {
            try
            {
                PrintLabelModel model = new PrintLabelModel();
                model.ProductId = product.Id;
                model.printerName = GetProductPrinterName(product, machine, resource, productionLabelType);
                if (model.printerName == "")
                {
                    var t = new JsonText();
                    var prm = product.PartNo.CreateParameters("@PartNo");
                    prm.Add("@MachineCode", machine.Code);
                    prm.Add("@ResourceName", resource.resourceName);
                    prm.Add("@PrinterType", productionLabelType.ToText(t));
                    throw new Exception(ToolsMessageBox.ReplaceParameters("Tanımlanmış yazıcı bulunamadığı için iş emri başlatılamıyor.\r\n\r\nReferans no: @PartNo\r\nİş merkezi: @MachineCode\r\nKaynak: @ResourceName\r\nYazıcı tipi: @PrinterType", prm));
                }

                var design = GetDesignFilePath(product, productionLabelType);
                model.LabelDesingFilePath = design.LabelDesignPath;
                model.PrintCopyCount = (short)design.CopyCount;
                model.productionLabelType = productionLabelType;
                return model;
            }
            catch (Exception ex)
            {
                if (flag)
                {
                    throw new Exception(ex.Message);
                }
                else
                    return null;
            }
        }

        private static string GetProductPrinterName(Product product, Machine machine, Machine resource, ProductionLabelType productionLabelType)
        {
            string printerName = "";
            SpecialCode labelType = new SpecialCode(); //label tipi için ihtiyaç duyulan Id yi bul
            switch (productionLabelType)
            {
                case ProductionLabelType.Product:
                    labelType = StaticValues.specialCodeProductTypeProduct;
                    break;
                case ProductionLabelType.Process:
                    labelType = StaticValues.specialCodeProductTypeProcess;
                    break;
                case ProductionLabelType.Box:
                    labelType = StaticValues.specialCodeProductTypeBox;
                    break;
                case ProductionLabelType.InkjetProcess:
                    labelType = StaticValues.specialCodeProductTypeInkjetProcess;
                    break;
            }

            var t = new JsonText();
            var stockLabelPrint = vw_ProductPrinterManager.Current.GetStockLabelPrinterById(product.Id, resource.Id, productionLabelType.ToText(t)); //iligili stok kodu için resource tanımlı printer var mı

            if (stockLabelPrint != null)
                return stockLabelPrint.PrinterName;

            Guid specialCodePrinterID;

            if (!StaticValues.printerMachines.Any(p => p.LabelTypeID == labelType.Id && p.ResourceID == resource.Id && p.DefaultPrinter == true))
                return "";

            specialCodePrinterID = StaticValues.printerMachines.OrderByDescending(x => x.CreatedAt).First(p => p.LabelTypeID == labelType.Id && p.MachineID == machine.Id && p.ResourceID == resource.Id && p.DefaultPrinter == true).PrinterID;

            printerName = SpecialCodeManager.Current.GetSpecialCodeById(specialCodePrinterID).Name;

            if (printerName != "")
                return printerName;
            else
            {
                var prm = machine.Code.CreateParameters("@MachineCode");
                prm.Add("@ResourceName", resource.resourceName);
                throw new Exception(ToolsMessageBox.ReplaceParameters("Tanımlanmış yazıcı bulunamadı.\r\nLütfen sistem yöneticinize başvurunuz.\r\n\r\nİş merkezi: @MachineCode\r\nKaynak: @ResourceName", prm));
            }
        }

        /// <summary>
        /// kullanılacak designı seçmek için çalıştırılır
        /// </summary>
        /// <param name="product">Product entity sinde ki alan4 </param>
        /// <param name="productionLabelType">Kasa etiketi,product etiketi vb etiket tip bilgisi</param>
        /// <returns></returns>
        public static LabelDesign GetDesignFilePath(Product product, ProductionLabelType productionLabelType)
        {

            SpecialCode labelType = new SpecialCode(); //label tipi için ihtiyaç duyulan Id yi bul
            switch (productionLabelType)
            {
                case ProductionLabelType.Product:
                    labelType = StaticValues.specialCodeProductTypeProduct;
                    break;
                case ProductionLabelType.Process:
                    labelType = StaticValues.specialCodeProductTypeProcess;
                    break;
                case ProductionLabelType.Box:
                    labelType = StaticValues.specialCodeProductTypeBox;
                    break;
            }

            var labelDesign = LabelDesignManager.Current.GetLabelDesing(StaticValues.panel.BranchId, labelType.Id, product.alan4);

            if (labelDesign != null)
                return labelDesign;
            else
            {
            var t = new JsonText();
                var branch = BranchManager.Current.GetBranch(StaticValues.panel.BranchId);
                var prm = product.PartNo.CreateParameters("@PartNo");
                prm.Add("@BranchName", branch == null ? "Yok" : branch.Name);
                prm.Add("@CustomerShortCode", product.alan4);
                prm.Add("@PrinterType", productionLabelType.ToText(t));
                throw new Exception(ToolsMessageBox.ReplaceParameters("İş Emrine ait stok kartı bilgisine ulaşılamadı\r\n Lütfen sistem yönetimicinize başvurunuz. Stok kartına ait tanımlı etiket dizaynı bulunamadı.\r\n\r\nReferans no: @PartNo\r\nŞube: @BranchName\r\nMüşteri Kısa Kodu: @CustomerShortCode\r\nYazıcı tipi: @PrinterType", prm));
            }
        }
    }
}
