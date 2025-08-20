using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents selectable container UI controls in the operator panel.
    /// </summary>
    public enum ContainerSelectUserControl
    {
        MachineDown,
        MachineDownDuration,
        MachineDownStop,
        MachineDownMaintanenceStart,
        PrMaintenance,
        PrMaintanenceDuration,
        PrMaintenanceStart,
        PrMaintenanceFinish,
        InterruptionCause,
        InterruptionCauseDuration,
        SetupCheckList,
        UserLogin,
        UserLogOut,
        BoxBarcode,
        QuestionableProduct,
        ProcessBoxBarcode,
        GeneralReadResult,
        ShiftBook,
        MachineAutoMaintanenceCheckList,
        YellowCard
    }

    /// <summary>
    /// Provides stable localization keys for <see cref="ContainerSelectUserControl"/>.
    /// </summary>
    public static class ContainerSelectUserControlTextKey
    {
        public static string Key(ContainerSelectUserControl control)
        {
            switch (control)
            {
                case ContainerSelectUserControl.MachineDown: return "enums.container_control.machine_down";
                case ContainerSelectUserControl.MachineDownDuration: return "enums.container_control.machine_down_duration";
                case ContainerSelectUserControl.MachineDownStop: return "enums.container_control.machine_down_stop";
                case ContainerSelectUserControl.MachineDownMaintanenceStart: return "enums.container_control.machine_down_maintenance_start";
                case ContainerSelectUserControl.PrMaintenance: return "enums.container_control.pr_maintenance";
                case ContainerSelectUserControl.PrMaintanenceDuration: return "enums.container_control.pr_maintenance_duration";
                case ContainerSelectUserControl.PrMaintenanceStart: return "enums.container_control.pr_maintenance_start";
                case ContainerSelectUserControl.PrMaintenanceFinish: return "enums.container_control.pr_maintenance_finish";
                case ContainerSelectUserControl.InterruptionCause: return "enums.container_control.interruption_cause";
                case ContainerSelectUserControl.InterruptionCauseDuration: return "enums.container_control.interruption_cause_duration";
                case ContainerSelectUserControl.SetupCheckList: return "enums.container_control.setup_checklist";
                case ContainerSelectUserControl.UserLogin: return "enums.container_control.user_login";
                case ContainerSelectUserControl.UserLogOut: return "enums.container_control.user_logout";
                case ContainerSelectUserControl.BoxBarcode: return "enums.container_control.box_barcode";
                case ContainerSelectUserControl.QuestionableProduct: return "enums.container_control.questionable_product";
                case ContainerSelectUserControl.ProcessBoxBarcode: return "enums.container_control.process_box_barcode";
                case ContainerSelectUserControl.GeneralReadResult: return "enums.container_control.general_read_result";
                case ContainerSelectUserControl.ShiftBook: return "enums.container_control.shift_book";
                case ContainerSelectUserControl.MachineAutoMaintanenceCheckList: return "enums.container_control.machine_auto_maintenance_checklist";
                case ContainerSelectUserControl.YellowCard: return "enums.container_control.yellow_card";
                default: return "enums.container_control.unknown";
            }
        }
    }

    /// <summary>
    /// Helpers to render <see cref="ContainerSelectUserControl"/> using an <c>IText</c> provider.
    /// </summary>
    public static class ContainerSelectUserControlTextExtensions
    {
        public static string ToText(this ContainerSelectUserControl control, IText t)
        {
            if (t == null) throw new ArgumentNullException("t");
            return t[ContainerSelectUserControlTextKey.Key(control)];
        }
    }
}
