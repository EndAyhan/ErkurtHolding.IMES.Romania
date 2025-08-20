using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    public class ProcessHandlingUnitModel
    {
        public string BoxBarcode { get; set; }
        public Guid shopOrderProductionId { get; set; }
        public decimal Quantity { get; set; }
        public int LotCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
