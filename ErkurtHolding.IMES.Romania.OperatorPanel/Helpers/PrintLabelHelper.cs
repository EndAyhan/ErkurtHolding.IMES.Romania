using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Helper methods for selecting printers and label designs for product/process/box labels.
    /// </summary>
    public static class PrintLabelHelper
    {
        /// <summary>
        /// Builds a <see cref="PrintLabelModel"/> for the given product/machine/resource and label type.
        /// Throws a localized exception when no printer or design is configured
        /// (unless <paramref name="flag"/> is false, in which case <c>null</c> is returned).
        /// </summary>
        public static PrintLabelModel GetLabelModel(
            Product product,
            Machine machine,
            Machine resource,
            ProductionLabelType productionLabelType,
            bool flag = true)
        {
            try
            {
                if (product == null) throw new ArgumentNullException(nameof(product));
                if (machine == null) throw new ArgumentNullException(nameof(machine));
                if (resource == null) throw new ArgumentNullException(nameof(resource));

                var model = new PrintLabelModel
                {
                    ProductId = product.Id,
                    printerName = GetProductPrinterName(product, machine, resource, productionLabelType),
                    productionLabelType = productionLabelType
                };

                if (string.IsNullOrEmpty(model.printerName))
                {
                    var prm = product.PartNo.CreateParameters("@PartNo");
                    prm.Add("@MachineCode", machine.Code);
                    prm.Add("@ResourceName", resource.resourceName);
                    prm.Add("@PrinterType", productionLabelType.ToText());

                    var msg = StaticValues.T["printlabel.error.no_printer_for_order"];
                    throw new Exception(ToolsMessageBox.ReplaceParameters(msg, prm));
                }

                var design = GetDesignFilePath(product, productionLabelType);
                model.LabelDesingFilePath = design.LabelDesignPath;
                model.PrintCopyCount = (short)design.CopyCount;

                return model;
            }
            catch (Exception ex)
            {
                if (flag) throw new Exception(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Resolves the printer name for a product/resource and label type.
        /// Returns <c>""</c> when no matching printer is found (caller decides how to handle).
        /// </summary>
        private static string GetProductPrinterName(
            Product product,
            Machine machine,
            Machine resource,
            ProductionLabelType productionLabelType)
        {
            if (product == null || machine == null || resource == null) return string.Empty;

            SpecialCode labelType = ResolveLabelType(productionLabelType);

            // 1) Explicit mapping for (Product, Resource, LabelType)
            var stockLabelPrint = vw_ProductPrinterManager.Current
                .GetStockLabelPrinterById(product.Id, resource.Id, productionLabelType.ToText());

            if (stockLabelPrint != null && !string.IsNullOrEmpty(stockLabelPrint.PrinterName))
                return stockLabelPrint.PrinterName;

            // 2) Default printer configured at (Machine, Resource, LabelType)
            var defaultPrinter = StaticValues.printerMachines
                .Where(p => p.LabelTypeID == labelType.Id
                         && p.ResourceID == resource.Id
                         && p.MachineID == machine.Id
                         && p.DefaultPrinter)
                .OrderByDescending(x => x.CreatedAt)
                .Select(p => p.PrinterID)
                .FirstOrDefault();

            if (defaultPrinter == Guid.Empty)
                return string.Empty;

            var printer = SpecialCodeManager.Current.GetSpecialCodeById(defaultPrinter);
            var printerName = printer != null ? printer.Name : string.Empty;

            if (!string.IsNullOrEmpty(printerName))
                return printerName;

            var prm = machine.Code.CreateParameters("@MachineCode");
            prm.Add("@ResourceName", resource.resourceName);

            var msg = StaticValues.T["printlabel.error.no_printer_defined"];
            throw new Exception(ToolsMessageBox.ReplaceParameters(msg, prm));
        }

        /// <summary>
        /// Selects the label design for the given product and label type.
        /// Throws a localized exception when no design is found.
        /// </summary>
        public static LabelDesign GetDesignFilePath(
            Product product,
            ProductionLabelType productionLabelType)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            SpecialCode labelType = ResolveLabelType(productionLabelType);

            var labelDesign = LabelDesignManager.Current
                .GetLabelDesing(StaticValues.panel.BranchId, labelType.Id, product.alan4);

            if (labelDesign != null)
                return labelDesign;

            var branch = BranchManager.Current.GetBranch(StaticValues.panel.BranchId);

            var prm = product.PartNo.CreateParameters("@PartNo");
            prm.Add("@BranchName", branch == null ? StaticValues.T["printlabel.value_none"] : branch.Name);
            prm.Add("@CustomerShortCode", product.alan4 ?? string.Empty);
            prm.Add("@PrinterType", productionLabelType.ToText());

            var msg = StaticValues.T["printlabel.error.no_label_design_for_stock"];
            throw new Exception(ToolsMessageBox.ReplaceParameters(msg, prm));
        }

        // ----------------- helpers -----------------

        private static SpecialCode ResolveLabelType(ProductionLabelType productionLabelType)
        {
            switch (productionLabelType)
            {
                case ProductionLabelType.Product:
                    return StaticValues.specialCodeProductTypeProduct;
                case ProductionLabelType.Process:
                    return StaticValues.specialCodeProductTypeProcess;
                case ProductionLabelType.Box:
                    return StaticValues.specialCodeProductTypeBox;
                case ProductionLabelType.InkjetProcess:
                    return StaticValues.specialCodeProductTypeInkjetProcess;
                default:
                    return StaticValues.specialCodeProductTypeProduct;
            }
        }
    }
}
