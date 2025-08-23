using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents the status of machine downtime action buttons.
    /// </summary>
    public enum MachineDownTimeButtonStatus
    {
        Start = 0,
        InterventionStart = 1,
        Waiting = 2,
        InterventionStop = 3,
        BarBtnQualtyApproved = 4,
        End = 5,
        Cancel = 6
    }

    /// <summary>
    /// Provides stable localization keys for <see cref="MachineDownTimeButtonStatus"/>.
    /// </summary>
    public static class MachineDownTimeButtonStatusTextKey
    {
        public static string Key(MachineDownTimeButtonStatus status)
        {
            switch (status)
            {
                case MachineDownTimeButtonStatus.Start: return "enums.machine_down_time_button_status.start";
                case MachineDownTimeButtonStatus.InterventionStart: return "enums.machine_down_time_button_status.intervention_start";
                case MachineDownTimeButtonStatus.Waiting: return "enums.machine_down_time_button_status.waiting";
                case MachineDownTimeButtonStatus.InterventionStop: return "enums.machine_down_time_button_status.intervention_stop";
                case MachineDownTimeButtonStatus.BarBtnQualtyApproved: return "enums.machine_down_time_button_status.quality_approved";
                case MachineDownTimeButtonStatus.End: return "enums.machine_down_time_button_status.end";
                case MachineDownTimeButtonStatus.Cancel: return "enums.machine_down_time_button_status.cancel";
                default: return "enums.machine_down_time_button_status.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="MachineDownTimeButtonStatus"/> using an <c>IText</c> provider.
    /// </summary>
    public static class MachineDownTimeButtonStatusTextExtensions
    {
        public static string ToText(this MachineDownTimeButtonStatus status)
        {
            return StaticValues.T[MachineDownTimeButtonStatusTextKey.Key(status)];
        }
    }
}
