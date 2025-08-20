using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    public class UserModel
    {
        public Guid UserProductionId { get; set; }
        public Guid CompanyPersonId { get; set; }
        public string IfsEmplooyeId { get; set; }
        public string Name { get; set; }
        public string LaborClass { get; set; }
        public double Role { get; set; }
        public string rfIdNo { get; set; }
        public string Email { get; set; }
        public bool TwoFactorActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; } = DateTime.Now;
    }
}
