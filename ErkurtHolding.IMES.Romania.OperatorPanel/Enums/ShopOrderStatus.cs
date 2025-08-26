using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Shop order workflow status.</summary>
    public enum ShopOrderStatus
    {
        Start = 0,
        End = 1,
        StartProduction = 2
    }

    public static class ShopOrderStatusTextExtensions
    {
        public static string ToText(this ShopOrderStatus s)
        {
            switch (s)
            {
                case ShopOrderStatus.Start: return MessageTextHelper.GetMessageText("ENUM", "228", "Start Shop Order", "Enum");
                case ShopOrderStatus.End: return MessageTextHelper.GetMessageText("ENUM", "229", "End Shop Order", "Enum");
                case ShopOrderStatus.StartProduction: return MessageTextHelper.GetMessageText("ENUM", "230", "Start Production", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "231", "Unknown Status", "Enum");
            }
        }
    }
}
