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

    public static class SpecialCodeTypeTextKey
    {
        public static string Key(SpecialCodeType t)
        {
            switch (t)
            {
                case SpecialCodeType.MachineType: return "enums.special_code_type.machine_type";
                case SpecialCodeType.Category: return "enums.special_code_type.category";
                case SpecialCodeType.Brand: return "enums.special_code_type.brand";
                case SpecialCodeType.Model: return "enums.special_code_type.model";
                case SpecialCodeType.Type: return "enums.special_code_type.type";
                case SpecialCodeType.GroupCode: return "enums.special_code_type.group_code";
                case SpecialCodeType.Unit: return "enums.special_code_type.unit";
                case SpecialCodeType.State: return "enums.special_code_type.state";
                case SpecialCodeType.FileType: return "enums.special_code_type.file_type";
                case SpecialCodeType.CounterUnit: return "enums.special_code_type.counter_unit";
                case SpecialCodeType.CounterType: return "enums.special_code_type.counter_type";
                case SpecialCodeType.Action: return "enums.special_code_type.action";
                case SpecialCodeType.MachineCategory: return "enums.special_code_type.machine_category";
                case SpecialCodeType.OperatorType: return "enums.special_code_type.operator_type";
                case SpecialCodeType.MachineState: return "enums.special_code_type.machine_state";
                case SpecialCodeType.DataType: return "enums.special_code_type.data_type";
                case SpecialCodeType.OPCDataReadType: return "enums.special_code_type.opc_data_read_type";
                case SpecialCodeType.MachineOPCDataType: return "enums.special_code_type.machine_opc_data_type";
                case SpecialCodeType.ProductionStateType: return "enums.special_code_type.production_state_type";
                case SpecialCodeType.LabelType: return "enums.special_code_type.label_type";
                case SpecialCodeType.Printer: return "enums.special_code_type.printer";
                case SpecialCodeType.CallType: return "enums.special_code_type.call_type";
                case SpecialCodeType.DataReadParameter: return "enums.special_code_type.data_read_parameter";
                case SpecialCodeType.Automaticlabeltype: return "enums.special_code_type.automatic_label_type";
                case SpecialCodeType.AutonomousMaintenance: return "enums.special_code_type.autonomous_maintenance";
                case SpecialCodeType.Language: return "enums.special_code_type.language";
                default: return "enums.special_code_type.unknown";
            }
        }
    }

    public static class SpecialCodeTypeTextExtensions
    {
        public static string ToText(this SpecialCodeType t, IText text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            return text[SpecialCodeTypeTextKey.Key(t)];
        }
    }
}
