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
    /// Provides stable localization keys for <see cref="MachineOPCDataType"/>.
    /// </summary>
    public static class MachineOPCDataTypeTextKey
    {
        public static string Key(MachineOPCDataType type)
        {
            switch (type)
            {
                case MachineOPCDataType.SquareMetersCounter: return "enums.machine_opc_data_type.square_meters_counter";
                case MachineOPCDataType.Handshake: return "enums.machine_opc_data_type.handshake";
                case MachineOPCDataType.PlcRunModeParameter: return "enums.machine_opc_data_type.plc_run_mode_parameter";
                case MachineOPCDataType.PokaYoke: return "enums.machine_opc_data_type.poka_yoke";
                case MachineOPCDataType.MachineProgramNodeId: return "enums.machine_opc_data_type.machine_program_node_id";
                default: return "enums.machine_opc_data_type.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="MachineOPCDataType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class MachineOPCDataTypeTextExtensions
    {
        public static string ToText(this MachineOPCDataType type, IText t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            return t[MachineOPCDataTypeTextKey.Key(type)];
        }
    }
}
