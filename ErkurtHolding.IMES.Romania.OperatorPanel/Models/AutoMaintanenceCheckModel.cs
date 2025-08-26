using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents a record used in autonomous (auto) maintenance checks.
    /// </summary>
    [Serializable]
    public class AutoMaintanenceCheckModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for this maintenance check item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the maintenance task or checkpoint.
        /// </summary>
        public string MaintenanceDescription { get; set; }
    }
}
