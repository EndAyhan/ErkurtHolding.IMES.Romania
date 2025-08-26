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

    public static class ProductionStateIdTextExtensions
    {
        public static string ToText(this ProductionStateId s)
        {
            switch (s)
            {
                case ProductionStateId.OK: return MessageTextHelper.GetMessageText("ENUM", "215", "OK", "Enum");
                case ProductionStateId.Scrap: return MessageTextHelper.GetMessageText("ENUM", "216", "Suspect", "Enum");
                case ProductionStateId.NotOk: return MessageTextHelper.GetMessageText("ENUM", "217", "Scrap", "Enum");
                case ProductionStateId.QUALITYOK: return MessageTextHelper.GetMessageText("ENUM", "218", "Quality OK", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "219", "Unknown State", "Enum");
            }
        }
    }
}
