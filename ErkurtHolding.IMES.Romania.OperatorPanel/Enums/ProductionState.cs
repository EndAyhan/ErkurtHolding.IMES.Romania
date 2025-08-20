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

    public static class ProductionStateTextKey
    {
        public static string Key(ProductionState s)
        {
            switch (s)
            {
                case ProductionState.Ok: return "enums.production_state.ok";
                case ProductionState.NOk: return "enums.production_state.not_ok";
                case ProductionState.Questionable: return "enums.production_state.questionable";
                default: return "enums.production_state.unknown";
            }
        }
    }

    public static class ProductionStateTextExtensions
    {
        public static string ToText(this ProductionState s, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[ProductionStateTextKey.Key(s)];
        }
    }
}
