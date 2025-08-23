using System;
using Opc.Ua;
using Opc.UaFx;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    /// <summary>
    /// Extensions to convert OPC UA typed values and 16-bit register arrays to <see cref="decimal"/>.
    /// </summary>
    public static class ConvertExtension
    {
        private const int WordBase = 65536; // 2^16

        /// <summary>
        /// Converts an array of 16-bit unsigned integers (registers) into a single decimal value,
        /// interpreting the array as a base-65536 number with <paramref name="wordsBigEndian"/> ordering.
        /// </summary>
        /// <param name="uintArray">The array of 16-bit words (e.g., Modbus/OPC registers).</param>
        /// <param name="wordsBigEndian">
        /// If true, the first element is the most-significant word (MSW). If false (default), the first element is the least-significant word (LSW).
        /// </param>
        /// <param name="scale">
        /// Optional scale to apply at the end (e.g., 0.01 for 2 decimal places coming from the PLC). Default is 1 (no scaling).
        /// </param>
        /// <returns>The computed <see cref="decimal"/> value. Returns 0 for null/empty inputs.</returns>
        public static decimal ConvertDecimalExtension(this UInt16[] uintArray, bool wordsBigEndian = false, decimal scale = 1m)
        {
            if (uintArray == null || uintArray.Length == 0)
                return 0m;

            // Reorder if caller uses big-endian word order
            int len = uintArray.Length;
            decimal result = 0m;

            if (wordsBigEndian)
            {
                // word[0] = MSW, so multiply by base^(len-1-i)
                for (int i = 0; i < len; i++)
                {
                    int power = (len - 1) - i;
                    result += (decimal)uintArray[i] * Pow65536(power);
                }
            }
            else
            {
                // word[0] = LSW, so multiply by base^i
                for (int i = 0; i < len; i++)
                {
                    result += (decimal)uintArray[i] * Pow65536(i);
                }
            }

            if (scale != 1m)
                result *= scale;

            return result;
        }

        /// <summary>
        /// Attempts to convert any common OPC UA value container (e.g. <see cref="DataValue"/>,
        /// <see cref="Variant"/>, <see cref="OpcValue"/>, or the underlying raw value) to <see cref="decimal"/>.
        /// </summary>
        /// <remarks>
        /// Handles:
        /// <list type="bullet">
        /// <item><description><c>UInt16[]</c> (register arrays) → combined base-65536 number.</description></item>
        /// <item><description><c>float</c>, <c>double</c>, <c>decimal</c> → converted to <c>decimal</c>.</description></item>
        /// <item><description>Signed/unsigned integrals → converted to <c>decimal</c>.</description></item>
        /// <item><description><c>string</c> → parsed if numeric.</description></item>
        /// </list>
        /// If the effective value is <c>null</c>, returns <c>null</c>.
        /// </remarks>
        /// <param name="obj">An OPC value or wrapper (e.g., <see cref="DataValue"/>, <see cref="Variant"/>, <see cref="OpcValue"/>).</param>
        /// <param name="wordsBigEndian">Word order for <c>UInt16[]</c> values. Default is LSW-first (<c>false</c>).</param>
        /// <param name="scale">Optional scale applied to register arrays. Default is 1.</param>
        /// <param name="roundTo">Optional decimal places to round to (e.g., 4). Pass <c>null</c> to skip rounding.</param>
        /// <returns>A <see cref="decimal"/> value if conversion succeeds; otherwise <c>null</c>.</returns>
        public static decimal? ConvertDecimalExtension(this object obj, bool wordsBigEndian = false, decimal scale = 1m, int? roundTo = null)
        {
            // 1) Unwrap common OPC UA containers
            object value = UnwrapOpcValue(obj);
            if (value == null)
                return null;

            decimal result;

            // 2) Special case: array of 16-bit registers
            var asU16Array = value as UInt16[];
            if (asU16Array != null)
            {
                result = asU16Array.ConvertDecimalExtension(wordsBigEndian, scale);
                return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
            }

            // 3) Numeric primitives
            // double/float/decimal first
            if (value is decimal decVal)
            {
                result = decVal;
                return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
            }
            if (value is double dblVal)
            {
                result = Convert.ToDecimal(dblVal);
                return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
            }
            if (value is float fltVal)
            {
                result = Convert.ToDecimal(fltVal);
                return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
            }

            // signed/unsigned integrals
            if (value is sbyte || value is byte ||
                value is short || value is ushort ||
                value is int || value is uint ||
                value is long || value is ulong)
            {
                result = Convert.ToDecimal(value);
                return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
            }

            // 4) Strings: try parse
            if (value is string s)
            {
                decimal parsed;
                if (decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsed))
                {
                    result = parsed;
                    return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
                }
                return null;
            }

            // 5) Fallback: try System.Convert
            try
            {
                result = Convert.ToDecimal(value);
                return roundTo.HasValue ? Math.Round(result, roundTo.Value) : result;
            }
            catch
            {
                return null;
            }
        }

        // ---------- helpers ----------

        /// <summary>
        /// Fast integer power for 65536^exp as <see cref="decimal"/>.
        /// </summary>
        private static decimal Pow65536(int exp)
        {
            // 65536^0 = 1, 65536^1 = 65536, etc. All exactly representable as decimal for reasonable exp (< 11 produces huge numbers already).
            decimal acc = 1m;
            for (int i = 0; i < exp; i++)
                acc *= WordBase;
            return acc;
        }

        /// <summary>
        /// Extracts the underlying value from common OPC UA containers.
        /// </summary>
        private static object UnwrapOpcValue(object obj)
        {
            if (obj == null) return null;

            // DataValue (OPC Foundation)
            var dv = obj as DataValue;
            if (dv != null)
                return dv.Value;

            // Variant (OPC Foundation)
            if (obj is Variant variant)
                return variant.Value;

            // OpcValue (OPC UA FX)
            var ov = obj as OpcValue;
            if (ov != null)
                return ov.Value;

            // Already a raw value
            return obj;
        }
    }
}
