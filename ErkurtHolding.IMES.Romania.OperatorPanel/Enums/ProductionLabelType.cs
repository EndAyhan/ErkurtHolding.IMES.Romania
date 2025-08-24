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

    public static class ProductionLabelTypeTextExtensions
    {
        public static string ToText(this ProductionLabelType type)
        {
            switch (type)
            {
                case ProductionLabelType.Product: return MessageTextHelper.GetMessageText("ENUM", "202", "Product Label", "Enum");
                case ProductionLabelType.Process: return MessageTextHelper.GetMessageText("ENUM", "203", "Process Label", "Enum");
                case ProductionLabelType.Box: return MessageTextHelper.GetMessageText("ENUM", "204", "Box Label", "Enum");
                case ProductionLabelType.InkjetProcess: return MessageTextHelper.GetMessageText("ENUM", "205", "Inkjet Process", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "206", "Unknown Label Type", "Enum");
            }
        }
    }
}
