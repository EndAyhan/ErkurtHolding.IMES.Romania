using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Authorization checks for user login flows.</summary>
    public enum UserLoginAuthorization
    {
        noAuthorization,
        personelLevelAuthorization,
        startWorkOrderAuthorization,
        maintenanceUserAuthorization,
        qualityUserAuthorization,
        manuelLabelAuthorization,
        inventoryTransferUserAuthorization
    }

    public static class UserLoginAuthorizationTextKey
    {
        public static string Key(UserLoginAuthorization a)
        {
            switch (a)
            {
                case UserLoginAuthorization.noAuthorization: return "enums.user_login_authorization.no_authorization";
                case UserLoginAuthorization.personelLevelAuthorization: return "enums.user_login_authorization.personnel_level";
                case UserLoginAuthorization.startWorkOrderAuthorization: return "enums.user_login_authorization.start_work_order";
                case UserLoginAuthorization.maintenanceUserAuthorization: return "enums.user_login_authorization.maintenance_user";
                case UserLoginAuthorization.qualityUserAuthorization: return "enums.user_login_authorization.quality_user";
                case UserLoginAuthorization.manuelLabelAuthorization: return "enums.user_login_authorization.manual_label";
                case UserLoginAuthorization.inventoryTransferUserAuthorization: return "enums.user_login_authorization.inventory_transfer_user";
                default: return "enums.user_login_authorization.unknown";
            }
        }
    }

    public static class UserLoginAuthorizationTextExtensions
    {
        public static string ToText(this UserLoginAuthorization a, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[UserLoginAuthorizationTextKey.Key(a)];
        }
    }
}
