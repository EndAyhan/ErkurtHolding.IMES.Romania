using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents a product involved in a Preventive Maintenance (PrMaintenance) operation.
    /// </summary>
    [Serializable]
    public class PrMaintenanceProductModel
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
        /// Gets or sets the part number (material number / stock code) of the product.
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// Gets or sets the quantity of this product required/used during preventive maintenance.
        /// </summary>
        public double Quantity { get; set; }
    }
}
