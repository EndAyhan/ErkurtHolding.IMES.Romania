using System;
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

    public static class ShopOrderStatusTextKey
    {
        public static string Key(ShopOrderStatus s)
        {
            switch (s)
            {
                case ShopOrderStatus.Start: return "enums.shop_order_status.start";
                case ShopOrderStatus.End: return "enums.shop_order_status.end";
                case ShopOrderStatus.StartProduction: return "enums.shop_order_status.start_production";
                default: return "enums.shop_order_status.unknown";
            }
        }
    }

    public static class ShopOrderStatusTextExtensions
    {
        public static string ToText(this ShopOrderStatus s, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[ShopOrderStatusTextKey.Key(s)];
        }
    }
}
