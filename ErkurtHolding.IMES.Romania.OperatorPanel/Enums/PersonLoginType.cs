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
    /// Provides stable localization keys for <see cref="PersonLoginType"/>.
    /// </summary>
    public static class PersonLoginTypeTextKey
    {
        public static string Key(PersonLoginType type)
        {
            switch (type)
            {
                case PersonLoginType.ShopOrderSuperVisorLogin: return "enums.person_login_type.shop_order_supervisor_login";
                case PersonLoginType.ShopOrderPersonelLogin: return "enums.person_login_type.shop_order_personel_login";
                case PersonLoginType.ShopOrderPersonelLogout: return "enums.person_login_type.shop_order_personel_logout";
                default: return "enums.person_login_type.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="PersonLoginType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class PersonLoginTypeTextExtensions
    {
        public static string ToText(this PersonLoginType type)
        {
            return StaticValues.T[PersonLoginTypeTextKey.Key(type)];
        }
    }
}
