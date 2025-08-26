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

    public static class UserLoginAuthorizationTextExtensions
    {
        public static string ToText(this UserLoginAuthorization a)
        {
            switch (a)
            {
                case UserLoginAuthorization.noAuthorization: return MessageTextHelper.GetMessageText("ENUM", "273", "No authorization check", "Enum");
                case UserLoginAuthorization.personelLevelAuthorization: return MessageTextHelper.GetMessageText("ENUM", "274", "Personnel level > 2", "Enum");
                case UserLoginAuthorization.startWorkOrderAuthorization: return MessageTextHelper.GetMessageText("ENUM", "275", "Start Work Order", "Enum");
                case UserLoginAuthorization.maintenanceUserAuthorization: return MessageTextHelper.GetMessageText("ENUM", "276", "Maintenance User", "Enum");
                case UserLoginAuthorization.qualityUserAuthorization: return MessageTextHelper.GetMessageText("ENUM", "277", "Quality User", "Enum");
                case UserLoginAuthorization.manuelLabelAuthorization: return MessageTextHelper.GetMessageText("ENUM", "278", "Manual Label User", "Enum");
                case UserLoginAuthorization.inventoryTransferUserAuthorization: return MessageTextHelper.GetMessageText("ENUM", "279", "Inventory Transfer User", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "280", "Unknown Authorization", "Enum");
            }
        }
    }
}
