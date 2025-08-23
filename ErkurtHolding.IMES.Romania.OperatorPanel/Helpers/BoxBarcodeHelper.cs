using System;
using System.Globalization;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Helpers to build customer box barcodes for various customers/specs.
    /// </summary>
    public static class BoxBarcodeHelper
    {
        /// <summary>
        /// Builds the default “customer box barcode” using the current shop order context.
        /// </summary>
        /// <param name="serial">Box serial number.</param>
        /// <param name="reportHandlingUnitHelper">Active report/handling unit context.</param>
        /// <returns>Formatted customer barcode string.</returns>
        /// <remarks>
        /// This uses <paramref name="reportHandlingUnitHelper"/> to resolve quantity (MaxQuantityCapacity)
        /// and product fields (<c>alan5</c> or <c>dimQuality</c>) and then delegates to
        /// <see cref="GetCustomerBoxBarcode(long, decimal, string, string, string)"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reportHandlingUnitHelper"/> is null.</exception>
        public static string GetCustomerBoxBarcode(long serial, ReportHandlingUnitHelper reportHandlingUnitHelper)
        {
            if (reportHandlingUnitHelper == null) throw new ArgumentNullException(nameof(reportHandlingUnitHelper));

            var qty = (decimal)reportHandlingUnitHelper.vwShopOrderGridModel.MaxQuantityCapacity;
            var alan5 = reportHandlingUnitHelper.product.alan5;
            var dim = reportHandlingUnitHelper.product.dimQuality;
            var productCode = reportHandlingUnitHelper.product.dimQuality; // kept as in original code

            return GetCustomerBoxBarcode(serial, qty, productCode, alan5, dim);
        }

        /// <summary>
        /// Builds a Hyundai‑style customer box barcode using the current shop order context.
        /// </summary>
        /// <param name="serial">Box serial number.</param>
        /// <param name="reportHandlingUnitHelper">Active report/handling unit context.</param>
        /// <returns>Formatted Hyundai barcode string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reportHandlingUnitHelper"/> is null.</exception>
        public static string GetCustomerBoxBarcodeHyundai(long serial, ReportHandlingUnitHelper reportHandlingUnitHelper)
        {
            if (reportHandlingUnitHelper == null) throw new ArgumentNullException(nameof(reportHandlingUnitHelper));

            var qty = (decimal)reportHandlingUnitHelper.vwShopOrderGridModel.MaxQuantityCapacity;
            var alan5 = reportHandlingUnitHelper.product.alan5;
            var dim = reportHandlingUnitHelper.product.dimQuality;

            return GetCustomerBoxBarcodeHyundai(serial, qty, alan5, dim);
        }

        /// <summary>
        /// Builds (and persists) a Hyundai‑style box barcode for a given serial and product fields.
        /// </summary>
        /// <param name="serial">Box serial number.</param>
        /// <param name="MaxQuantityCapacity">Max quantity capacity (used for the 5‑digit padded quantity).</param>
        /// <param name="productAlan5">Product field “alan5” (preferred for part name if present).</param>
        /// <param name="productDimQuality">Product field “dimQuality” (fallback for part name).</param>
        /// <returns>Formatted Hyundai barcode string.</returns>
        /// <remarks>
        /// - If no existing record is found, a new <see cref="CustomerBarcodeHyundai"/> is created with default
        ///   <c>CustomerCode = "BTV5"</c>, <c>PartName</c>, <c>Date</c> (<c>yMMdd</c>), <c>BoxSerial</c>, and <c>Quantity</c>.
        /// - The entity is updated via <see cref="CustomerBarcodeHyundaiManager"/> and the serial number is coerced to 4 digits.
        /// </remarks>
        public static string GetCustomerBoxBarcodeHyundai(long serial, decimal MaxQuantityCapacity, string productAlan5, string productDimQuality)
        {
            var mgr = CustomerBarcodeHyundaiManager.Current;
            var customerBarcode = mgr.GetCustomerBarcodeHyundaiByBoxSerial(serial);

            if (customerBarcode == null)
            {
                customerBarcode = new CustomerBarcodeHyundai
                {
                    CustomerCode = "BTV5",
                    PartName = !string.IsNullOrEmpty(productAlan5) ? productAlan5 : (productDimQuality ?? string.Empty),
                    Date = DateTime.Now.ToString("yMMdd", CultureInfo.InvariantCulture),
                    BoxSerial = serial,
                    Quantity = ((long)Math.Abs(MaxQuantityCapacity)).ToString(CultureInfo.InvariantCulture)
                };

                // Persist and retrieve assigned SerialNo
                var inserted = mgr.Insert(customerBarcode);
                if (inserted != null && inserted.ListData != null && inserted.ListData.Count > 0)
                    customerBarcode.SerialNo = inserted.ListData[0].SerialNo;
            }

            return GetCustomerBoxBarcodeHyundai(customerBarcode);
        }

        /// <summary>
        /// Low‑level builder for Hyundai‑style barcode based on an existing <see cref="CustomerBarcodeHyundai"/> entity.
        /// </summary>
        /// <param name="customerBarcode">Entity carrying part name, quantity, date, serial, and code.</param>
        /// <returns>Formatted Hyundai barcode string.</returns>
        /// <remarks>
        /// The format is: <c>CustomerCode + PartName(13, padded right with spaces) + Quantity(5, left‑zero‑padded) + Date(yMMdd) + SerialNo(4 digits)</c>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="customerBarcode"/> is null.</exception>
        public static string GetCustomerBoxBarcodeHyundai(CustomerBarcodeHyundai customerBarcode)
        {
            if (customerBarcode == null) throw new ArgumentNullException(nameof(customerBarcode));

            var code = customerBarcode.CustomerCode ?? string.Empty;
            var partName = (customerBarcode.PartName ?? string.Empty).PadRight(13, ' ');
            var quantity = (customerBarcode.Quantity ?? "0").PadLeft(5, '0');

            var date = customerBarcode.Date;
            if (string.IsNullOrEmpty(date))
                date = DateTime.Now.ToString("yMMdd", CultureInfo.InvariantCulture);

            // Coerce serial number to 4 digits by modulo 10000 as per original logic
            var serial = customerBarcode.SerialNo;
            while (serial > 9999) serial -= 10000;
            var serialStr = serial.ToString("D4", CultureInfo.InvariantCulture);

            // Persist potential normalization (keeps original behavior that called Update)
            CustomerBarcodeHyundaiManager.Current.Update(customerBarcode);

            return code + partName + quantity + date + serialStr;
        }

        /// <summary>
        /// Builds the default customer box barcode string (non‑Hyundai).
        /// </summary>
        /// <param name="serial">Box serial number.</param>
        /// <param name="quantity">Quantity in the box (used as 7‑digit left‑zero‑padded integer).</param>
        /// <param name="productCode">Fallback product code (used when <paramref name="alan5"/> and <paramref name="productDimQuality"/> are empty).</param>
        /// <param name="alan5">Preferred product field.</param>
        /// <param name="productDimQuality">Secondary product field.</param>
        /// <returns>Formatted customer barcode string.</returns>
        /// <remarks>
        /// Format: <c>yyWW + serial + quantity(7, left‑zero‑padded) + "00" + (alan5 | dimQuality | productCode)</c>.
        /// <br/>
        /// Week string is derived from <see cref="GetWeekAndYear(DateTime)"/> and then trimmed to <c>yyWW</c>.
        /// </remarks>
        public static string GetCustomerBoxBarcode(long serial, decimal quantity, string productCode, string alan5, string productDimQuality)
        {
            // "YYYYWW" string then take "YYWW" (Substring(2,4))
            var yearAndWeek = GetWeekAndYear(DateTime.Now);
            var weekOfYearYYWW = (yearAndWeek != null && yearAndWeek.Length >= 6)
                ? yearAndWeek.Substring(2, 4)
                : DateTime.Now.ToString("yy", CultureInfo.InvariantCulture) + "00";

            // 7-digit zero-padded integer quantity (truncate decimals)
            var qtyDigits = Math.Abs((long)quantity).ToString("D7", CultureInfo.InvariantCulture);

            // Choose product field in precedence order
            var tail = !string.IsNullOrEmpty(alan5)
                ? alan5
                : (!string.IsNullOrEmpty(productDimQuality) ? productDimQuality : (productCode ?? string.Empty));

            return string.Concat(weekOfYearYYWW, serial.ToString(CultureInfo.InvariantCulture), qtyDigits, "00", tail);
        }

        /// <summary>
        /// Returns a <c>YYYYWW</c> string for the given date using the current culture’s
        /// week settings (rule: <see cref="CalendarWeekRule.FirstDay"/>, first day Monday).
        /// </summary>
        /// <param name="dateTime">The date to evaluate.</param>
        /// <returns>A 6‑character string composed of year (4) + week (2).</returns>
        public static string GetWeekAndYear(DateTime dateTime)
        {
            var cul = CultureInfo.CurrentCulture;

            int weekNum = cul.Calendar.GetWeekOfYear(
                dateTime,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);

            // Pad week to 2 digits
            var weekStr = weekNum.ToString("D2", CultureInfo.InvariantCulture);
            return dateTime.Year.ToString("D4", CultureInfo.InvariantCulture) + weekStr;
        }
    }
}
