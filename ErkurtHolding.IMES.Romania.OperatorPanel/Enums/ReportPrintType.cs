using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Report printing modes.</summary>
    public enum ReportPrintType
    {
        View,
        Designer,
        print
    }

    public static class ReportPrintTypeTextKey
    {
        public static string Key(ReportPrintType t)
        {
            switch (t)
            {
                case ReportPrintType.View: return "enums.report_print_type.view";
                case ReportPrintType.Designer: return "enums.report_print_type.designer";
                case ReportPrintType.print: return "enums.report_print_type.print";
                default: return "enums.report_print_type.unknown";
            }
        }
    }

    public static class ReportPrintTypeTextExtensions
    {
        public static string ToText(this ReportPrintType t)
        {
            return StaticValues.T[ReportPrintTypeTextKey.Key(t)];
        }
    }
}
