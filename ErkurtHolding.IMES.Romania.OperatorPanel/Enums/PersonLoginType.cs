using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents different types of personnel login/logout actions for a shop order.
    /// </summary>
    public enum PersonLoginType
    {
        ShopOrderSuperVisorLogin,
        ShopOrderPersonelLogin,
        ShopOrderPersonelLogout
    }

    /// <summary>
    /// Helper to render <see cref="PersonLoginType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class PersonLoginTypeTextExtensions
    {
        public static string ToText(this PersonLoginType type)
        {
            switch (type)
            {
                case PersonLoginType.ShopOrderSuperVisorLogin: return MessageTextHelper.GetMessageText("ENUM", "181", "Shop Order Supervisor Login", "Enum");
                case PersonLoginType.ShopOrderPersonelLogin: return MessageTextHelper.GetMessageText("ENUM", "182", "Shop Order Personnel Login", "Enum");
                case PersonLoginType.ShopOrderPersonelLogout: return MessageTextHelper.GetMessageText("ENUM", "183", "Shop Order Personnel Logout", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "184", "Unknown Login Type", "Enum");
            }
        }
    }
}
