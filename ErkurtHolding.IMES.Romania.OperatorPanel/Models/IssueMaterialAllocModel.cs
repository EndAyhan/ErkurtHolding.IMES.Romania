using ErkurtHolding.IMES.Entity.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
            VwMaterialAllocs = new List<vw_MaterialAlloc>();
        }

        // --- Identity & relations ---

        public Guid ShopOrderID { get; set; }
        public Guid MainProductID { get; set; }
        public Guid MaterialAllocID { get; set; }
        public Guid MaterialAllocProductID { get; set; }

        // --- Shop order ---

        public string ShopOrderNo { get; set; }

        /// <summary>IFS/ERP operation number (often 10, 20, ...).</summary>
        public double ShopOrderOperationNo { get; set; }

        // --- Main product (parent) ---

        public string MainProductCode { get; set; }
        public string MainProductDescription { get; set; }

        // --- Allocated material (child) ---

        public string MaterialAllocProductCode { get; set; }
        public string MaterialAllocProductDescription { get; set; }

        // --- Quantities (backward-compatible JSON names preserved) ---

        /// <summary>Quantity per assembly (BOM requirement).</summary>
        [JsonProperty("qtyPerAssembly")]
        public double QtyPerAssembly { get; set; }

        /// <summary>Total required quantity for this allocation line.</summary>
        [JsonProperty("qtyRequired")]
        public double QtyRequired { get; set; }

        /// <summary>Final weighing value captured from scale (if any).</summary>
        public double FinalWeighingValue { get; set; } = 0;

        /// <summary>Total issued quantity so far.</summary>
        [JsonProperty("qtyIssued")]
        public double QtyIssued { get; set; } = 0;

        /// <summary>Computed missing quantity (based on <see cref="QtyRequired"/> and <see cref="UsagePercentage"/> setter).</summary>
        [JsonProperty("qtyMissing")]
        public double QtyMissing { get; set; }

        /// <summary>Optional % field for UI charts; not used in core logic.</summary>
        [JsonProperty("qtyPercentage")]
        public double QtyPercentage { get; set; }

        /// <summary>IFS integration sent flag.</summary>
        public bool IfsSend { get; set; } = false;

        /// <summary>Underlying stock lots/allocs for this material line.</summary>
        public List<vw_MaterialAlloc> VwMaterialAllocs { get; set; }

        // --- Calculated / helper properties ---

        /// <summary>
        /// Ratio of issued / required (0..1). 
        /// <para>Getter: returns <c>Math.Round(QtyIssued / QtyRequired, 4)</c> (0 if required is 0).</para>
        /// <para>Setter: interprets value as target usage; adjusts <see cref="QtyMissing"/> accordingly.</para>
        /// </summary>
        [JsonProperty("usagePercentage")]
        public double UsagePercentage
        {
            get
            {
                if (QtyRequired == 0) return 0;
                return Math.Round(QtyIssued / QtyRequired, 4);
            }
            set
            {
                // Interpret setter as: user provided a target usage ratio; compute missing quantity to meet it.
                if (value <= 0)
                {
                    // Nothing should be used -> treat everything issued as "over"; preserve original behavior.
                    QtyMissing = QtyIssued;
                    return;
                }

                if (QtyRequired > 0)
                {
                    var current = Math.Round(QtyIssued / QtyRequired, 4);
                    if (!current.Equals(value))
                        QtyMissing = (QtyRequired * value) - QtyIssued;
                    else
                        QtyMissing = 0;
                }
                else
                {
                    QtyMissing = 0;
                }
            }
        }

        // --- Convenience methods ---

        /// <summary>
        /// Applies a new weighing value and optionally bumps <see cref="QtyIssued"/>; 
        /// then recomputes <see cref="UsagePercentage"/> (getter reflects new ratio).
        /// </summary>
        /// <param name="weighed">Weighed amount to apply.</param>
        /// <param name="addToIssued">If true, adds <paramref name="weighed"/> to <see cref="QtyIssued"/>.</param>
        public void ApplyWeighing(double weighed, bool addToIssued = true)
        {
            if (weighed < 0) weighed = 0;
            FinalWeighingValue = weighed;

            if (addToIssued)
            {
                QtyIssued += weighed;
                // keep QtyMissing coherent relative to current target usage (if any was set previously)
                // If not set previously, you can choose to recalc against 100% target by uncommenting:
                // UsagePercentage = 1.0;
            }
        }

        /// <summary>
        /// Resets issued and missing quantities (does not touch requirements).
        /// </summary>
        public void ResetUsage()
        {
            QtyIssued = 0;
            QtyMissing = 0;
            FinalWeighingValue = 0;
            QtyPercentage = 0;
        }
    }
}
