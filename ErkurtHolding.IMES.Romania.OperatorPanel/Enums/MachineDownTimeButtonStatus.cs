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
    /// Helper to render <see cref="MachineDownTimeButtonStatus"/> using an <c>IText</c> provider.
    /// </summary>
    public static class MachineDownTimeButtonStatusTextExtensions
    {
        public static string ToText(this MachineDownTimeButtonStatus status)
        {
            switch (status)
            {
                case MachineDownTimeButtonStatus.Start: return MessageTextHelper.GetMessageText("ENUM", "155", "Start Fault", "Enum");
                case MachineDownTimeButtonStatus.InterventionStart: return MessageTextHelper.GetMessageText("ENUM", "156", "Start Intervention", "Enum");
                case MachineDownTimeButtonStatus.Waiting: return MessageTextHelper.GetMessageText("ENUM", "157", "Waiting", "Enum");
                case MachineDownTimeButtonStatus.InterventionStop: return MessageTextHelper.GetMessageText("ENUM", "158", "Stop Intervention", "Enum");
                case MachineDownTimeButtonStatus.BarBtnQualtyApproved: return MessageTextHelper.GetMessageText("ENUM", "159", "Quality Approval", "Enum");
                case MachineDownTimeButtonStatus.End: return MessageTextHelper.GetMessageText("ENUM", "160", "End Fault", "Enum");
                case MachineDownTimeButtonStatus.Cancel: return MessageTextHelper.GetMessageText("ENUM", "161", "Cancel", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "162", "Unknown Status", "Enum");
            }
        }
    }
}
