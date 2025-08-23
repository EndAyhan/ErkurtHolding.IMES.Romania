using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Detailed production state identifiers.</summary>
    public enum ProductionStateId
    {
        OK = 1,
        Scrap = 2,     // original description said "ŞÜPHELİ" (Suspect); name kept for compatibility
        NotOk = 3,     // original description said "HURDA" (Scrap)
        QUALITYOK = 4  // "Parça OK"
    }

    public static class ProductionStateIdTextKey
    {
        public static string Key(ProductionStateId s)
        {
            switch (s)
            {
                case ProductionStateId.OK: return "enums.production_state_id.ok";
                case ProductionStateId.Scrap: return "enums.production_state_id.suspect";
                case ProductionStateId.NotOk: return "enums.production_state_id.scrap";
                case ProductionStateId.QUALITYOK: return "enums.production_state_id.quality_ok";
                default: return "enums.production_state_id.unknown";
            }
        }
    }

    public static class ProductionStateIdTextExtensions
    {
        public static string ToText(this ProductionStateId s)
        {
            return StaticValues.T[ProductionStateIdTextKey.Key(s)];
        }
    }
}
