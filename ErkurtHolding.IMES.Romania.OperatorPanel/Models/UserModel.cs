using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Lightweight user context used on the operator panel and production logging.
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>Current open <c>UserProduction</c> row id (set when logged in).</summary>
        public Guid UserProductionId { get; set; }

        /// <summary>Company person id (foreign key).</summary>
        public Guid CompanyPersonId { get; set; }

        /// <summary>IFS employee identifier (if any).</summary>
        public string IfsEmplooyeId { get; set; }

        /// <summary>Display name.</summary>
        public string Name { get; set; }

        /// <summary>Labor class description or code.</summary>
        public string LaborClass { get; set; }

        /// <summary>Role weight/classification (numeric).</summary>
        public double Role { get; set; }

        /// <summary>RFID card number.</summary>
        public string rfIdNo { get; set; }

        /// <summary>Email (used for notifications).</summary>
        public string Email { get; set; }

        /// <summary>True if 2FA is enabled for this user.</summary>
        public bool TwoFactorActive { get; set; }

        /// <summary>Login/start time for the current session (if tracked).</summary>
        public DateTime StartDate { get; set; }

        /// <summary>Finish time for the current session (defaults to now).</summary>
        public DateTime FinishDate { get; set; } = DateTime.Now;
    }
}
