using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

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
    /// Helpers to render <see cref="ContainerSelectUserControl"/> using an <c>IText</c> provider.
    /// </summary>
    public static class ContainerSelectUserControlTextExtensions
    {
        public static string ToText(this ContainerSelectUserControl control)
        {
            switch (control)
            {
                case ContainerSelectUserControl.MachineDown: return MessageTextHelper.GetMessageText("ENUM", "115", "Machine Down", "Enum");
                case ContainerSelectUserControl.MachineDownDuration: return MessageTextHelper.GetMessageText("ENUM", "116", "Machine Down Duration", "Enum");
                case ContainerSelectUserControl.MachineDownStop: return MessageTextHelper.GetMessageText("ENUM", "117", "Machine Down Stop", "Enum");
                case ContainerSelectUserControl.MachineDownMaintanenceStart: return MessageTextHelper.GetMessageText("ENUM", "118", "Machine Down Maintenance Start", "Enum");
                case ContainerSelectUserControl.PrMaintenance: return MessageTextHelper.GetMessageText("ENUM", "119", "Preventive Maintenance", "Enum");
                case ContainerSelectUserControl.PrMaintanenceDuration: return MessageTextHelper.GetMessageText("ENUM", "120", "Preventive Maintenance Duration", "Enum");
                case ContainerSelectUserControl.PrMaintenanceStart: return MessageTextHelper.GetMessageText("ENUM", "121", "Preventive Maintenance Start", "Enum");
                case ContainerSelectUserControl.PrMaintenanceFinish: return MessageTextHelper.GetMessageText("ENUM", "122", "Preventive Maintenance Finish", "Enum");
                case ContainerSelectUserControl.InterruptionCause: return MessageTextHelper.GetMessageText("ENUM", "123", "Interruption Cause", "Enum");
                case ContainerSelectUserControl.InterruptionCauseDuration: return MessageTextHelper.GetMessageText("ENUM", "124", "Interruption Cause Duration", "Enum");
                case ContainerSelectUserControl.SetupCheckList: return MessageTextHelper.GetMessageText("ENUM", "125", "Setup Checklist", "Enum");
                case ContainerSelectUserControl.UserLogin: return MessageTextHelper.GetMessageText("ENUM", "126", "User Login", "Enum");
                case ContainerSelectUserControl.UserLogOut: return MessageTextHelper.GetMessageText("ENUM", "127", "User Logout", "Enum");
                case ContainerSelectUserControl.BoxBarcode: return MessageTextHelper.GetMessageText("ENUM", "128", "Box Barcode", "Enum");
                case ContainerSelectUserControl.QuestionableProduct: return MessageTextHelper.GetMessageText("ENUM", "129", "Questionable Product", "Enum");
                case ContainerSelectUserControl.ProcessBoxBarcode: return MessageTextHelper.GetMessageText("ENUM", "130", "Process Box Barcode", "Enum");
                case ContainerSelectUserControl.GeneralReadResult: return MessageTextHelper.GetMessageText("ENUM", "131", "General Read Result", "Enum");
                case ContainerSelectUserControl.ShiftBook: return MessageTextHelper.GetMessageText("ENUM", "132", "Shift Book", "Enum");
                case ContainerSelectUserControl.MachineAutoMaintanenceCheckList: return MessageTextHelper.GetMessageText("ENUM", "133", "Machine Auto Maintenance Checklist", "Enum");
                case ContainerSelectUserControl.YellowCard: return MessageTextHelper.GetMessageText("ENUM", "134", "Yellow Card", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "135", "Unknown Control", "Enum");
            }
        }
    }
}
