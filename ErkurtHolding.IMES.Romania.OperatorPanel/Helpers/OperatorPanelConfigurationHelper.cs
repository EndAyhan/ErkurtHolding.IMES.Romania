using System;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Centralized helpers to interpret operator-panel configuration flags
    /// carried in IFS/ERP fields (e.g., alan5, alan6, alan7, alan9, alan13).
    /// </summary>
    public static class OperatorPanelConfigurationHelper
    {
        /// <summary>
        /// Returns <c>true</c> if automatic production notification is enabled for the given operation.
        /// </summary>
        /// <param name="shopOrderOperation">IFS shop order operation entity.</param>
        /// <returns>True when automatic notification is desired; otherwise false.</returns>
        public static bool DoAutomaticProductionNotification(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan9);
        }

        /// <summary>
        /// Returns <c>true</c> if product labels should be printed for the given operation.
        /// </summary>
        public static bool PrintProductLabel(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan5);
        }

        /// <summary>
        /// Returns <c>true</c> if box labels should be printed for the given operation.
        /// </summary>
        public static bool PrintBoxLabel(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan6);
        }

        /// <summary>
        /// Returns <c>true</c> if process labels should be printed for the given operation.
        /// </summary>
        public static bool PrintProsesLabel(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan7);
        }

        /// <summary>
        /// Returns <c>true</c> if product labels should be printed for the given order view.
        /// </summary>
        public static bool PrintProductLabel(vw_ShopOrderGridModel shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan5);
        }

        /// <summary>
        /// Returns <c>true</c> if box labels should be printed for the given order view.
        /// </summary>
        public static bool PrintBoxLabel(vw_ShopOrderGridModel shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan6);
        }

        /// <summary>
        /// Returns <c>true</c> if process labels should be printed for the given order view.
        /// </summary>
        public static bool PrintProsesLabel(vw_ShopOrderGridModel shopOrderOperation)
        {
            return shopOrderOperation != null && IsTrue(shopOrderOperation.alan7);
        }

        /// <summary>
        /// Returns <c>true</c> if manual creation of production details is allowed in this order.
        /// </summary>
        /// <remarks>
        /// Original implementation treated <c>null</c> as <c>false</c>; this preserves that behavior.
        /// </remarks>
        public static bool CanManuallyCreateShopOrderProductionDetail(vw_ShopOrderGridModel shopOrderGridModel)
        {
            return shopOrderGridModel != null && IsTrue(shopOrderGridModel.alan13);
        }

        // ---------- helpers ----------

        /// <summary>
        /// Interprets common “truthy” string values from ERP/IFS fields.
        /// Accepts: "TRUE", "1", "Y", "YES" (case-insensitive, trimmed). Everything else is false.
        /// </summary>
        private static bool IsTrue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            var s = value.Trim();
            // Fast checks first
            if (s.Length == 4 && s.Equals("TRUE", StringComparison.OrdinalIgnoreCase)) return true;
            if (s.Length == 1)
            {
                var c = s[0];
                return c == '1' || c == 'Y' || c == 'y';
            }

            return s.Equals("YES", StringComparison.OrdinalIgnoreCase);
        }
    }
}
