using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Represents the type of print log records.</summary>
    public enum PrintLogType
    {
        HandlingUnit,
        ProductiongDetail,
        Scrap,
        ProsesHandlingUnit
    }

    public static class PrintLogTypeTextExtensions
    {
        public static string ToText(this PrintLogType type)
        {
            switch (type)
            {
                case PrintLogType.HandlingUnit: return MessageTextHelper.GetMessageText("ENUM", "187", "Handling Unit", "Enum");
                case PrintLogType.ProductiongDetail: return MessageTextHelper.GetMessageText("ENUM", "188", "Production Detail", "Enum");
                case PrintLogType.Scrap: return MessageTextHelper.GetMessageText("ENUM", "189", "Scrap", "Enum");
                case PrintLogType.ProsesHandlingUnit: return MessageTextHelper.GetMessageText("ENUM", "190", "Process Handling Unit", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "191", "Unknown Print Log Type", "Enum");
            }
        }
    }
}
