using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class OperatorPanelConfigurationHelper
    {
        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if automatic production notification is wanted or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>Return true if an automatic production notification should be made, else false</returns>
        public static bool DoAutomaticProductionNotification(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation.alan9 == "TRUE";
        }

        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if Product Labels should be printed or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>true if labels for products can be printed for this order, else false</returns>
        public static bool PrintProductLabel(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation.alan5 == "TRUE";
        }

        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if Box Labels should be printed or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>true if labels for boxes can be printed for this order, else false</returns>
        public static bool PrintBoxLabel(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation.alan6 == "TRUE";
        }

        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if Proses Labels should be printed or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>true if labels for proses can be printed for this order, else false</returns>
        public static bool PrintProsesLabel(ShopOrderOperation shopOrderOperation)
        {
            return shopOrderOperation.alan7 == "TRUE";
        }

        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if Product Labels should be printed or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>true if labels for products can be printed for this order, else false</returns>
        public static bool PrintProductLabel(vw_ShopOrderGridModel shopOrderOperation)
        {
            return shopOrderOperation.alan5 == "TRUE";
        }

        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if Box Labels should be printed or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>true if labels for boxes can be printed for this order, else false</returns>
        public static bool PrintBoxLabel(vw_ShopOrderGridModel shopOrderOperation)
        {
            return shopOrderOperation.alan6 == "TRUE";
        }

        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if Proses Labels should be printed or not.
        /// </summary>
        /// <param name="shopOrderOperation"></param>
        /// <returns>true if labels for proses can be printed for this order, else false</returns>
        public static bool PrintProsesLabel(vw_ShopOrderGridModel shopOrderOperation)
        {
            return shopOrderOperation.alan7 == "TRUE";
        }


        /// <summary>
        /// Check IFS Parameter of ShopOrderOperation if it is allowed to manuelly create production details in this shop order.
        /// </summary>
        /// <param name="shopOrderGridModel"></param>
        /// <returns>true if production can be manuelly added for this order, else false</returns>
        public static bool CanManuallyCreateShopOrderProductionDetail(vw_ShopOrderGridModel shopOrderGridModel)
        {
            return (shopOrderGridModel.alan13 == null ? false : shopOrderGridModel.alan13 == "TRUE");
        }
    }
}
