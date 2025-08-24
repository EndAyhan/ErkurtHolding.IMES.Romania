using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>Dictionary/lookup categories used across the app.</summary>
    public enum SpecialCodeType
    {
        MachineType = 1,
        Category = 2,
        Brand = 3,
        Model = 4,
        Type = 5,
        GroupCode = 6,
        Unit = 7,
        State = 8,
        FileType = 9,
        CounterUnit = 10,
        CounterType = 11,
        Action = 12,
        MachineCategory = 13,
        OperatorType = 14,
        MachineState = 15,
        DataType = 16,
        OPCDataReadType = 17,
        MachineOPCDataType = 18,
        ProductionStateType = 19,
        LabelType = 20,
        Printer = 21,
        CallType = 22,
        DataReadParameter = 23,
        Automaticlabeltype = 24,
        AutonomousMaintenance = 25,
        Language = 26
    }

    public static class SpecialCodeTypeTextExtensions
    {
        public static string ToText(this SpecialCodeType t)
        {
            switch (t)
            {
                case SpecialCodeType.MachineType: return MessageTextHelper.GetMessageText("ENUM", "238", "Machine Type", "Enum");
                case SpecialCodeType.Category: return MessageTextHelper.GetMessageText("ENUM", "239", "Category", "Enum");
                case SpecialCodeType.Brand: return MessageTextHelper.GetMessageText("ENUM", "240", "Brand", "Enum");
                case SpecialCodeType.Model: return MessageTextHelper.GetMessageText("ENUM", "241", "Model", "Enum");
                case SpecialCodeType.Type: return MessageTextHelper.GetMessageText("ENUM", "242", "Type", "Enum");
                case SpecialCodeType.GroupCode: return MessageTextHelper.GetMessageText("ENUM", "243", "Group Code", "Enum");
                case SpecialCodeType.Unit: return MessageTextHelper.GetMessageText("ENUM", "244", "Unit", "Enum");
                case SpecialCodeType.State: return MessageTextHelper.GetMessageText("ENUM", "245", "State", "Enum");
                case SpecialCodeType.FileType: return MessageTextHelper.GetMessageText("ENUM", "246", "File Type", "Enum");
                case SpecialCodeType.CounterUnit: return MessageTextHelper.GetMessageText("ENUM", "247", "Counter Unit", "Enum");
                case SpecialCodeType.CounterType: return MessageTextHelper.GetMessageText("ENUM", "248", "Counter Type", "Enum");
                case SpecialCodeType.Action: return MessageTextHelper.GetMessageText("ENUM", "249", "Action", "Enum");
                case SpecialCodeType.MachineCategory: return MessageTextHelper.GetMessageText("ENUM", "250", "Machine Category", "Enum");
                case SpecialCodeType.OperatorType: return MessageTextHelper.GetMessageText("ENUM", "251", "Operator Type", "Enum");
                case SpecialCodeType.MachineState: return MessageTextHelper.GetMessageText("ENUM", "252", "Machine State", "Enum");
                case SpecialCodeType.DataType: return MessageTextHelper.GetMessageText("ENUM", "253", "Data Type", "Enum");
                case SpecialCodeType.OPCDataReadType: return MessageTextHelper.GetMessageText("ENUM", "254", "OPC Data Read Type", "Enum");
                case SpecialCodeType.MachineOPCDataType: return MessageTextHelper.GetMessageText("ENUM", "255", "Machine OPC Data Type", "Enum");
                case SpecialCodeType.ProductionStateType: return MessageTextHelper.GetMessageText("ENUM", "256", "Production State Type", "Enum");
                case SpecialCodeType.LabelType: return MessageTextHelper.GetMessageText("ENUM", "257", "Label Type", "Enum");
                case SpecialCodeType.Printer: return MessageTextHelper.GetMessageText("ENUM", "258", "Printer", "Enum");
                case SpecialCodeType.CallType: return MessageTextHelper.GetMessageText("ENUM", "259", "Call Type", "Enum");
                case SpecialCodeType.DataReadParameter: return MessageTextHelper.GetMessageText("ENUM", "260", "Production Data Read Parameter", "Enum");
                case SpecialCodeType.Automaticlabeltype: return MessageTextHelper.GetMessageText("ENUM", "261", "Automatic Label Type", "Enum");
                case SpecialCodeType.AutonomousMaintenance: return MessageTextHelper.GetMessageText("ENUM", "262", "Autonomous Maintenance", "Enum");
                case SpecialCodeType.Language: return MessageTextHelper.GetMessageText("ENUM", "263", "Language", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "264", "Unknown Special Code Type", "Enum");
            }
        }
    }
}
