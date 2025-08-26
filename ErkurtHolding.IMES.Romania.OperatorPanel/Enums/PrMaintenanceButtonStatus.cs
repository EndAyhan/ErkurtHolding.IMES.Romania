using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Status of preventive maintenance action buttons.</summary>
    public enum PrMaintenanceButtonStatus
    {
        Start = 0,
        InterventionStart = 1,
        InterventionStop = 2,
        End = 3,
        FinishMaintenance = 4
    }

    public static class PrMaintenanceButtonStatusTextExtensions
    {
        public static string ToText(this PrMaintenanceButtonStatus status)
        {
            switch (status)
            {
                case PrMaintenanceButtonStatus.Start: return MessageTextHelper.GetMessageText("ENUM", "194", "Start PM", "Enum");
                case PrMaintenanceButtonStatus.InterventionStart: return MessageTextHelper.GetMessageText("ENUM", "195", "Start Intervention", "Enum");
                case PrMaintenanceButtonStatus.InterventionStop: return MessageTextHelper.GetMessageText("ENUM", "196", "Stop Intervention", "Enum");
                case PrMaintenanceButtonStatus.End: return MessageTextHelper.GetMessageText("ENUM", "197", "End PM", "Enum");
                case PrMaintenanceButtonStatus.FinishMaintenance: return MessageTextHelper.GetMessageText("ENUM", "198", "Finish Maintenance", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "199", "Unknown Status", "Enum");
            }
        }
    }
}
