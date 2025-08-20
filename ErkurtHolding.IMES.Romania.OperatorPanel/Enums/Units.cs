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

    public static class UnitsTextKey
    {
        public static string Key(Units u)
        {
            switch (u)
            {
                case Units.ad: return "enums.units.ad";
                case Units.m2: return "enums.units.m2";
                case Units.kg: return "enums.units.kg";
                default: return "enums.units.unknown";
            }
        }
    }

    public static class UnitsTextExtensions
    {
        public static string ToText(this Units u, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[UnitsTextKey.Key(u)];
        }
    }
}
