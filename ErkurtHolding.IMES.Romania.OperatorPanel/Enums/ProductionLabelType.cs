using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Label types used in production.</summary>
    public enum ProductionLabelType
    {
        Product,
        Process,
        Box,
        InkjetProcess
    }

    public static class ProductionLabelTypeTextKey
    {
        public static string Key(ProductionLabelType type)
        {
            switch (type)
            {
                case ProductionLabelType.Product: return "enums.production_label_type.product";
                case ProductionLabelType.Process: return "enums.production_label_type.process";
                case ProductionLabelType.Box: return "enums.production_label_type.box";
                case ProductionLabelType.InkjetProcess: return "enums.production_label_type.inkjet_process";
                default: return "enums.production_label_type.unknown";
            }
        }
    }

    public static class ProductionLabelTypeTextExtensions
    {
        public static string ToText(this ProductionLabelType type, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[ProductionLabelTypeTextKey.Key(type)];
        }
    }
}
