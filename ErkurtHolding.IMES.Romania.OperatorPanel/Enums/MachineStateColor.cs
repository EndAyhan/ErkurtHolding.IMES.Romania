using System;
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
    /// Provides stable localization keys for <see cref="MachineStateColor"/>.
    /// </summary>
    public static class MachineStateColorTextKey
    {
        public static string Key(MachineStateColor state)
        {
            switch (state)
            {
                case MachineStateColor.Run: return "enums.machine_state_color.run";
                case MachineStateColor.MachineDown: return "enums.machine_state_color.machine_down";
                case MachineStateColor.Fault: return "enums.machine_state_color.fault";
                case MachineStateColor.Setup: return "enums.machine_state_color.setup";
                case MachineStateColor.ShopOrderWaiting: return "enums.machine_state_color.shop_order_waiting";
                default: return "enums.machine_state_color.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="MachineStateColor"/> using an <c>IText</c> provider.
    /// </summary>
    public static class MachineStateColorTextExtensions
    {
        public static string ToText(this MachineStateColor state)
        {
            return StaticValues.T[MachineStateColorTextKey.Key(state)];
        }
    }
}
