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

    public static class PrintLogTypeTextKey
    {
        public static string Key(PrintLogType type)
        {
            switch (type)
            {
                case PrintLogType.HandlingUnit: return "enums.print_log_type.handling_unit";
                case PrintLogType.ProductiongDetail: return "enums.print_log_type.production_detail";
                case PrintLogType.Scrap: return "enums.print_log_type.scrap";
                case PrintLogType.ProsesHandlingUnit: return "enums.print_log_type.process_handling_unit";
                default: return "enums.print_log_type.unknown";
            }
        }
    }

    public static class PrintLogTypeTextExtensions
    {
        public static string ToText(this PrintLogType type, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[PrintLogTypeTextKey.Key(type)];
        }
    }
}
