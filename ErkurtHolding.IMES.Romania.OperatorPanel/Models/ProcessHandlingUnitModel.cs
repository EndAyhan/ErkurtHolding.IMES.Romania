using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents a handling unit used during process labeling/printing.
    /// </summary>
    [Serializable]
    public class ProcessHandlingUnitModel
    {
        /// <summary>Generated or scanned box barcode.</summary>
        public string BoxBarcode { get; set; }

        /// <summary>Related shop order production header.</summary>
        public Guid shopOrderProductionId { get; set; }

        /// <summary>Quantity in this handling unit.</summary>
        public decimal Quantity { get; set; }

        /// <summary>Lot/pack count inside the handling unit.</summary>
        public int LotCount { get; set; }

        /// <summary>Creation time (client side for UI ordering).</summary>
        public DateTime CreatedAt { get; set; }
    }
}
