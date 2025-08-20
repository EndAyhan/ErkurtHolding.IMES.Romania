using Opc.Ua;
using Opc.UaFx;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    public static class ConvertExtension
    {
        public static decimal ConvertDecimalExtension(this UInt16[] uintArray)
        {
            double sumValue = 0;

            for (int i = 0; i < uintArray.Length; i++)
            {
                if (i + 1 == uintArray.Length)
                {
                    sumValue += Math.Abs(uintArray[i]);
                }
                else
                {
                    sumValue += Math.Abs(Convert.ToUInt32(uintArray[i]) * 65535);
                }
            }
            return Convert.ToDecimal(sumValue);
        }

        public static decimal? ConvertDecimalExtension(this object obj)
        {
            decimal returnValue = 0;
            OpcValue opcValue = new OpcValue(obj);
            var dataValue = ((DataValue)opcValue);
            var value = dataValue.GetValue(dataValue).Value;
            if (value == null)
                return 0;
            Type myType = value.GetType();
            //Todo : Double tipi için önlem almak gerekiyor
            if (myType.Equals(typeof(UInt16[])))
            {
                returnValue = Math.Abs((value as UInt16[]).ConvertDecimalExtension());
            }
            else if (myType.Equals(typeof(double)))
            {
                returnValue = Math.Abs(Math.Round(Convert.ToDecimal(value), 4));
            }
            else if (myType.Equals(typeof(UInt16)) || myType.Equals(typeof(float)) || myType.Equals(typeof(double)))
            {
                returnValue = Math.Abs(Convert.ToDecimal(value));
            }
            return returnValue;
        }
    }
}
