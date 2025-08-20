using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity;
using System;
using System.Globalization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class BoxBarcodeHelper
    {
        public static string GetCustomerBoxBarcode(long serial, ReportHandlingUnitHelper reportHandlingUnitHelper)
        {
            return GetCustomerBoxBarcode(serial, (decimal)reportHandlingUnitHelper.vwShopOrderGridModel.MaxQuantityCapacity, reportHandlingUnitHelper.product.dimQuality, reportHandlingUnitHelper.product.alan5, reportHandlingUnitHelper.product.dimQuality);
        }

        public static string GetCustomerBoxBarcodeHyundai(long serial, ReportHandlingUnitHelper reportHandlingUnitHelper)
        {
            return GetCustomerBoxBarcodeHyundai(serial, (decimal)reportHandlingUnitHelper.vwShopOrderGridModel.MaxQuantityCapacity, reportHandlingUnitHelper.product.alan5, reportHandlingUnitHelper.product.dimQuality);
        }

        public static string GetCustomerBoxBarcodeHyundai(long serial, decimal MaxQuantityCapacity, string productAlan5, string productDimQuality)
        {
            CustomerBarcodeHyundai customerBarcode = CustomerBarcodeHyundaiManager.Current.GetCustomerBarcodeHyundaiByBoxSerial(serial);
            if (customerBarcode == null)
            {
                customerBarcode = new CustomerBarcodeHyundai();
                customerBarcode.CustomerCode = "BTV5";
                if (productAlan5 != null && productAlan5 != "")
                {
                    customerBarcode.PartName = productAlan5;
                }
                else
                    customerBarcode.PartName = productDimQuality;

                customerBarcode.Date = DateTime.Now.ToString("yMMdd");
                customerBarcode.BoxSerial = serial;
                customerBarcode.Quantity = MaxQuantityCapacity.ToString();

                customerBarcode.SerialNo = CustomerBarcodeHyundaiManager.Current.Insert(customerBarcode).ListData[0].SerialNo;
            }

            return GetCustomerBoxBarcodeHyundai(customerBarcode);
        }
        public static string GetCustomerBoxBarcode(long serial, decimal quantity, string productCode, string alan5, string productDimQuality)
        {
            var yearAndWeek = GetWeekAndYear(DateTime.Now);
            var weekOfYear = yearAndWeek.ToString().Substring(2, 4);

            var zeroCount = 7 - quantity.ToString().Length;
            string zero = "";
            for (int i = 0; i < zeroCount; i++)
            {
                zero += "0";
            }

            if (alan5 != null && alan5 != "")
                return $"{weekOfYear}{serial}{zero}{quantity}00{alan5}";
            else if (productDimQuality != null && productDimQuality != "")
                return $"{weekOfYear}{serial}{zero}{quantity}00{productDimQuality}";
            else
                return $"{weekOfYear}{serial}{zero}{quantity}00{productCode}";
        }

        public static string GetCustomerBoxBarcodeHyundai(CustomerBarcodeHyundai customerBarcode)
        {
            string barcode = customerBarcode.CustomerCode;
            var partNameZeroCount = 13 - customerBarcode.PartName.Length;
            for (int i = 0; i < partNameZeroCount; i++)
            {
                customerBarcode.PartName += " ";
            }

            var quantityZeroCount = 5 - customerBarcode.Quantity.Length;
            for (int i = 0; i < quantityZeroCount; i++)
            {
                customerBarcode.Quantity = "0" + customerBarcode.Quantity;
            }

            CustomerBarcodeHyundaiManager.Current.Update(customerBarcode);
            while (customerBarcode.SerialNo > 9999)
                customerBarcode.SerialNo -= 10000;
            return barcode + customerBarcode.PartName + customerBarcode.Quantity + customerBarcode.Date + customerBarcode.SerialNo.ToString("D4");
        }

        public static string GetWeekAndYear(DateTime dateTime)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;

            int weekNum = cul.Calendar.GetWeekOfYear(
                dateTime,
                CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);

            if (weekNum.ToString().Length == 1)
                return $"{dateTime.Year}0{weekNum}";
            else
                return $"{dateTime.Year}{weekNum}";
        }
    }
}
