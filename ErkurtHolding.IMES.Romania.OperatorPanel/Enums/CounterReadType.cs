using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Enums
{
    /// <summary>
    /// Defines how counters are read/confirmed in the operator panel.
    /// </summary>
    public enum CounterReadType
    {
        PLC,
        BARCODEPLC,
        MANUEL,
        BUTTONANDREADBARCODE,
        PLCBARCODE,
        SUPPLIERPARK
    }

    /// <summary>
    /// Provides stable localization keys for <see cref="CounterReadType"/>.
    /// </summary>
    public static class CounterReadTypeTextKey
    {
        /// <summary>Returns the localization key for the given <see cref="CounterReadType"/>.</summary>
        public static string Key(CounterReadType type)
        {
            switch (type)
            {
                case CounterReadType.PLC: return "enums.counter_read.plc";
                case CounterReadType.BARCODEPLC: return "enums.counter_read.barcode_plc";
                case CounterReadType.MANUEL: return "enums.counter_read.manual";
                case CounterReadType.BUTTONANDREADBARCODE: return "enums.counter_read.button_and_barcode";
                case CounterReadType.PLCBARCODE: return "enums.counter_read.plc_barcode";
                case CounterReadType.SUPPLIERPARK: return "enums.counter_read.supplier_park";
                default: return "enums.counter_read.unknown";
            }
        }
    }

    /// <summary>
    /// Helper to render <see cref="CounterReadType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class CounterReadTypeTextExtensions
    {
        public static string ToText(this CounterReadType type)
        {
            return StaticValues.T[CounterReadTypeTextKey.Key(type)];
        }
    }
}
