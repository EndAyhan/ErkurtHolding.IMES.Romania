using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// DTO sent to inkjet printers via MQTT.
    /// </summary>
    [Serializable]
    public class InkJetModel
    {
        /// <summary>Product description.</summary>
        public string PartName { get; set; }

        /// <summary>Product part number.</summary>
        public string PartNo { get; set; }

        /// <summary>Shop order number.</summary>
        public string OrderNo { get; set; }

        /// <summary>Human‑readable serial (e.g., production detail serial).</summary>
        public string SerialNo { get; set; }

        /// <summary>Display name of the work center.</summary>
        public string WorkCenterName { get; set; }

        /// <summary>Work center code/definition.</summary>
        public string WorkCenter { get; set; }

        /// <summary>Company display name printed on label.</summary>
        public string Company { get; set; }

        /// <summary>Production finish timestamp as formatted text (printer‑specific).</summary>
        public string ProductionCreatedDate { get; set; }

        /// <summary>Extra serial or lot info (e.g., quality + serial).</summary>
        public string SerialPrivateNo { get; set; }

        /// <summary>Reserved field 1.</summary>
        public string Alan1 { get; set; } = "bos";

        /// <summary>Reserved field 2.</summary>
        public string Alan2 { get; set; } = "bos";

        /// <summary>Reserved field 3.</summary>
        public string Alan3 { get; set; } = "bos";

        /// <summary>Reserved field 4.</summary>
        public string Alan4 { get; set; } = "bos";

        /// <summary>Reserved field 5.</summary>
        public string Alan5 { get; set; } = "bos";
    }
}
