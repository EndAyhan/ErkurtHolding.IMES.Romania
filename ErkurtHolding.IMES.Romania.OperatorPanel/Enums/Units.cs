using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Measurement units (IFS shorthand).</summary>
    public enum Units
    {
        ad,
        m2,
        kg
    }

    public static class UnitsTextExtensions
    {
        public static string ToText(this Units u)
        {
            switch (u)
            {
                case Units.ad: return MessageTextHelper.GetMessageText("ENUM", "267", "pcs", "Enum");
                case Units.m2: return MessageTextHelper.GetMessageText("ENUM", "268", "m²", "Enum");
                case Units.kg: return MessageTextHelper.GetMessageText("ENUM", "269", "kg", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "270", "Unknown Unit", "Enum");
            }
        }
    }
}
