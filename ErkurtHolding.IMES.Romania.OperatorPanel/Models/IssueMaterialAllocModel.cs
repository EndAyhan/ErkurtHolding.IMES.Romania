using System;
using System.Collections.Generic;
using ErkurtHolding.IMES.Entity.Views;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents a material allocation row and its real-time usage during issuing/weighing.
    /// </summary>
    [Serializable]
    public class IssueMaterialAllocModel
    {
        public IssueMaterialAllocModel()
        {
            vw_MaterialAllocs = new List<vw_MaterialAlloc>();
        }

        public Guid ShopOrderID { get; set; }
        public Guid MainProductID { get; set; }
        public Guid MaterialAllocID { get; set; }
        public Guid MaterialAllocProductID { get; set; }

        public string ShopOrderNo { get; set; }
        public double ShopOrderOperationNo { get; set; }

        public string MainProductCode { get; set; }
        public string MainProductDescription { get; set; }

        public string MaterialAllocProductCode { get; set; }
        public string MaterialAllocProductDescription { get; set; }

        /// <summary>Quantity per assembly (BOM requirement).</summary>
        public double qtyPerAssembly { get; set; }

        /// <summary>Total required quantity for this allocation line.</summary>
        public double qtyRequired { get; set; }

        /// <summary>Final weighing value captured from scale (if any).</summary>
        public double FinalWeighingValue { get; set; } = 0;

        /// <summary>Total issued quantity so far.</summary>
        public double qtyIssued { get; set; } = 0;

        /// <summary>IFS integration sent flag.</summary>
        public bool IfsSend { get; set; } = false;

        /// <summary>Underlying stock lots/allocs for this material line.</summary>
        public List<vw_MaterialAlloc> vw_MaterialAllocs { get; set; }

        /// <summary>Computed missing quantity (based on <see cref="qtyRequired"/> and <see cref="qtyIssued"/> or <see cref="usagePercentage"/> setter).</summary>
        public double qtyMissing { get; set; }

        /// <summary>
        /// Ratio of issued / required (0..1). Writing this property adjusts <see cref="qtyMissing"/> accordingly.
        /// </summary>
        public double usagePercentage
        {
            get
            {
                if (qtyRequired == 0) return 0;
                return Math.Round(qtyIssued / qtyRequired, 4);
            }
            set
            {
                // Interpret setter as: target usage ratio provided; recompute missing vs. issued.
                if (value <= 0)
                {
                    qtyMissing = qtyIssued; // nothing should be used -> everything issued is "over", preserve original behavior
                }
                else if (qtyRequired > 0 && Math.Round(qtyIssued / qtyRequired, 4) != value)
                {
                    qtyMissing = qtyRequired * value - qtyIssued;
                }
                else
                {
                    qtyMissing = 0;
                }
            }
        }

        /// <summary>Optional % field for UI charts; not used in core logic.</summary>
        public double qtyPercentage { get; set; }
    }
}
