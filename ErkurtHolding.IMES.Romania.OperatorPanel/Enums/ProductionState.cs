using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Basic production state indicator.</summary>
    public enum ProductionState
    {
        Ok = 1,
        NOk = 2,
        Questionable = 3
    }

    public static class ProductionStateTextExtensions
    {
        public static string ToText(this ProductionState s)
        {
            switch (s)
            {
                case ProductionState.Ok: return MessageTextHelper.GetMessageText("ENUM", "209", "OK", "Enum");
                case ProductionState.NOk: return MessageTextHelper.GetMessageText("ENUM", "210", "Not OK", "Enum");
                case ProductionState.Questionable: return MessageTextHelper.GetMessageText("ENUM", "211", "Questionable", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "212", "Unknown State", "Enum");
            }
        }
    }
}
