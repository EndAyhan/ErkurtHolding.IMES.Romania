using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

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
    /// Helper to render <see cref="CounterReadType"/> using an <c>IText</c> provider.
    /// </summary>
    public static class CounterReadTypeTextExtensions
    {
        public static string ToText(this CounterReadType type)
        {
            switch (type)
            {
                case CounterReadType.PLC: return MessageTextHelper.GetMessageText("ENUM", "138", "PLC", "Enum");
                case CounterReadType.BARCODEPLC: return MessageTextHelper.GetMessageText("ENUM", "139", "Barcode First, Then PLC Counter", "Enum");
                case CounterReadType.MANUEL: return MessageTextHelper.GetMessageText("ENUM", "140", "Manual Quantity Entry", "Enum");
                case CounterReadType.BUTTONANDREADBARCODE: return MessageTextHelper.GetMessageText("ENUM", "141", "Button and Barcode Read", "Enum");
                case CounterReadType.PLCBARCODE: return MessageTextHelper.GetMessageText("ENUM", "142", "PLC Counter First, Then Barcode", "Enum");
                case CounterReadType.SUPPLIERPARK: return MessageTextHelper.GetMessageText("ENUM", "143", "Supplier Park", "Enum");
                default: return MessageTextHelper.GetMessageText("ENUM", "144", "Unknown Counter Read Type", "Enum");
            }
        }
    }
}
