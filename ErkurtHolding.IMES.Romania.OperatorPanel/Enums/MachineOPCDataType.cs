using System;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Represents the type of OPC data for machine integration.
    /// </summary>
    public enum MachineOPCDataType
    {
        SquareMetersCounter,
        Handshake,
        PlcRunModeParameter,
        PokaYoke,
        MachineProgramNodeId
    }

    /// <summary>
    /// Helper to render <see cref="MachineOPCDataType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class MachineOPCDataTypeTextExtensions
    {
        public static string ToText(this MachineOPCDataType type)
        {
            switch (type)
            {
                case MachineOPCDataType.SquareMetersCounter: return MessageTextHelper.GetMessageText("ENUM", "165", "Square Meters Counter", "Enum");
                case MachineOPCDataType.Handshake: return MessageTextHelper.GetMessageText("ENUM", "166", "Handshake", "Enum");
                case MachineOPCDataType.PlcRunModeParameter: return MessageTextHelper.GetMessageText("ENUM", "167", "PLC Run Mode Parameter", "Enum");
                case MachineOPCDataType.PokaYoke: return MessageTextHelper.GetMessageText("ENUM", "168", "Poka-Yoke", "Enum");
                case MachineOPCDataType.MachineProgramNodeId: return MessageTextHelper.GetMessageText("ENUM", "169", "Recipe Number", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "170", "Unknown OPC Data Type", "Enum");
            }
        }
    }
}
