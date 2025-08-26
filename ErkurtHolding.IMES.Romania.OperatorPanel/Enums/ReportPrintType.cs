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

    public static class ReportPrintTypeTextExtensions
    {
        public static string ToText(this ReportPrintType t)
        {
            switch (t)
            {
                case ReportPrintType.View: return MessageTextHelper.GetMessageText("ENUM", "222", "View", "Enum");
                case ReportPrintType.Designer: return MessageTextHelper.GetMessageText("ENUM", "223", "Designer", "Enum");
                case ReportPrintType.print: return MessageTextHelper.GetMessageText("ENUM", "224", "Print", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "225", "Unknown Mode", "Enum");
            }
        }
    }
}
