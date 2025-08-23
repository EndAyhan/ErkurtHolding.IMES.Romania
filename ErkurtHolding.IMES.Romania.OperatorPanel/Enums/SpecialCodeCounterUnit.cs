using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    public enum SpecialCodeCounterUnit
    {
        squareMeter
    }

    public static class SpecialCodeCounterUnitTextKey
    {
        public static string Key(SpecialCodeCounterUnit u)
        {
            switch (u)
            {
                case SpecialCodeCounterUnit.squareMeter: return "enums.special_code_counter_unit.square_meter";
                default: return "enums.special_code_counter_unit.unknown";
            }
        }
    }

    public static class SpecialCodeCounterUnitTextExtensions
    {
        public static string ToText(this SpecialCodeCounterUnit u)
        {
            return StaticValues.T[SpecialCodeCounterUnitTextKey.Key(u)];
        }
    }
}
