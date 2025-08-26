using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents machine states mapped to colors for the operator panel.
    /// </summary>
    public enum MachineStateColor
    {
        Run = 0,
        MachineDown = 1,
        Fault = 2,
        Setup = 3,
        ShopOrderWaiting = 4
    }

    /// <summary>
    /// Helper to render <see cref="MachineStateColor"/> using an <c>IText</c> provider.
    /// </summary>
    public static class MachineStateColorTextExtensions
    {
        public static string ToText(this MachineStateColor state)
        {
            switch (state)
            {
                case MachineStateColor.Run: return MessageTextHelper.GetMessageText("ENUM", "173", "Running", "Enum");
                case MachineStateColor.MachineDown: return MessageTextHelper.GetMessageText("ENUM", "174", "Machine Down", "Enum");
                case MachineStateColor.Fault: return MessageTextHelper.GetMessageText("ENUM", "175", "Fault", "Enum");
                case MachineStateColor.Setup: return MessageTextHelper.GetMessageText("ENUM", "176", "Setup", "Enum");
                case MachineStateColor.ShopOrderWaiting: return MessageTextHelper.GetMessageText("ENUM", "177", "Shop Order Waiting", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "178", "Unknown Machine State", "Enum");
            }
        }
    }
}
