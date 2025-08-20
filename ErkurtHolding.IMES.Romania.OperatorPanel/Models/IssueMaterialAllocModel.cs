using ErkurtHolding.IMES.Entity.Views;
using System.Collections.Generic;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
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
        public double qtyPerAssembly { get; set; }
        public double qtyRequired { get; set; }
        public double FinalWeighingValue { get; set; } = 0;
        public double qtyIssued { get; set; } = 0;
        public bool IfsSend { get; set; } = false;
        public List<vw_MaterialAlloc> vw_MaterialAllocs { get; set; }
        public double qtyMissing { get; set; }
        public double usagePercentage
        {
            get
            {
                return Math.Round(qtyIssued / qtyRequired, 4);
            }
            set
            {
                if (value == 0)
                    qtyMissing = qtyIssued;
                else if (Math.Round(qtyIssued / qtyRequired, 4) != value)
                    qtyMissing = qtyRequired * value - qtyIssued;
                else
                    qtyMissing = 0;
            }
        }
        public double qtyPercentage { get; set; }
    }

}
