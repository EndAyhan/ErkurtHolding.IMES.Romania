using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents a product record associated with a machine downtime event.
    /// </summary>
    [Serializable]
    public class MachineDownProductModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the human-readable description of the product.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets the part number (material/stock code) of the product.
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// Gets or sets the quantity of this product linked to the downtime event.
        /// </summary>
        public double Quantity { get; set; }
    }
}
