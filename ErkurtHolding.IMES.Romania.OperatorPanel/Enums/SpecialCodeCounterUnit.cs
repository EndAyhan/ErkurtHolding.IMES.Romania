using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    public enum SpecialCodeCounterUnit
    {
        squareMeter
    }

    public static class SpecialCodeCounterUnitTextExtensions
    {
        public static string ToText(this SpecialCodeCounterUnit u)
        {
            switch (u)
            {
                case SpecialCodeCounterUnit.squareMeter: return MessageTextHelper.GetMessageText("ENUM", "234", "Square Meter", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "235", "Unknown Unit", "Enum");
            }
        }
    }
}
