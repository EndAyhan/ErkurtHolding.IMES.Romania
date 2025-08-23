using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Printing configuration for a single label action (design, printer, copies).
    /// </summary>
    [Serializable]
    public class PrintLabelModel
    {
        /// <summary>Product id used by the label design.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Type of label (Product / Process / Box / InkjetProcess).</summary>
        public ProductionLabelType productionLabelType { get; set; }

        /// <summary>Target printer display name.</summary>
        public string printerName { get; set; }

        /// <summary>Absolute path to the .repx (DevExpress report) design file.</summary>
        public string LabelDesingFilePath { get; set; }

        /// <summary>Number of copies to print.</summary>
        public short PrintCopyCount { get; set; } = 1;
    }
}
