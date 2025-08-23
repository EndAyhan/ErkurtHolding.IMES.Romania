using System;
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

    public static class PrMaintenanceButtonStatusTextKey
    {
        public static string Key(PrMaintenanceButtonStatus status)
        {
            switch (status)
            {
                case PrMaintenanceButtonStatus.Start: return "enums.pr_maintenance_button_status.start";
                case PrMaintenanceButtonStatus.InterventionStart: return "enums.pr_maintenance_button_status.intervention_start";
                case PrMaintenanceButtonStatus.InterventionStop: return "enums.pr_maintenance_button_status.intervention_stop";
                case PrMaintenanceButtonStatus.End: return "enums.pr_maintenance_button_status.end";
                case PrMaintenanceButtonStatus.FinishMaintenance: return "enums.pr_maintenance_button_status.finish_maintenance";
                default: return "enums.pr_maintenance_button_status.unknown";
            }
        }
    }

    public static class PrMaintenanceButtonStatusTextExtensions
    {
        public static string ToText(this PrMaintenanceButtonStatus status)
        {
            return StaticValues.T[PrMaintenanceButtonStatusTextKey.Key(status)];
        }
    }
}
