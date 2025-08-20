using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    public class PrintLabelModel
    {
        public Guid ProductId { get; set; }
        public ProductionLabelType productionLabelType { get; set; }
        public string printerName { get; set; }
        public string LabelDesingFilePath { get; set; }
        public short PrintCopyCount { get; set; } = 1;
    }
}
