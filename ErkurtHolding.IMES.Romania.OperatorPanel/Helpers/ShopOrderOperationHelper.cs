using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class ShopOrderOperationHelper
    {
        public static ShopOrderOperation GetShopOrderOperation(string workCenter, string shopOrderNo)
        {
            ShopOrderOperation shopOrderOperation = new ShopOrderOperation();
            try
            {
                shopOrderOperation = ShopOrderOperationManager.Current.GetShopOrderOperation(shopOrderNo);
            }
            catch
            {
                shopOrderOperation = GetIfsShopOrderOperation(workCenter, shopOrderNo);
            }

            return shopOrderOperation;
        }

        private static ShopOrderOperation GetIfsShopOrderOperation(string workCenterNo, string shopOrderNo)
        {

            //var shopOrderOperations= ErkurtHolding.IMES.IFSIntegration.Managers.ServiceReadManager.GetShopOrderOperations(workCenterNo);
            //return shopOrderOperations.SingleOrDefault(x => x.orderNo == shopOrderNo);
            return null;
        }


        public static void ShopOrderOperationFinish(FrmOperator frmOperator, UserModel userModel)
        {
            decimal resourceSure = 0;
            if (frmOperator.panelDetail.OPCNodeIdSure != null && frmOperator.panelDetail.OPCNodeIdSure != "")
                decimal.TryParse(StaticValues.opcClient.ReadNode(frmOperator.panelDetail.OPCNodeIdSure), out resourceSure);

            if (frmOperator.opcOtherReadModels.Any(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePokayoke.Id))
            {
                var nodeId = frmOperator.opcOtherReadModels.First(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePokayoke.Id).NodeId;
                StaticValues.opcClient.WriteNode(nodeId, true);
            }

            if (frmOperator.panelDetail.OPCNodeIdSocketAdress != null && frmOperator.panelDetail.OPCNodeIdSocketAdress != "")
                StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdSocketAdress, Convert.ToUInt16(0));

            if (frmOperator.panelDetail.OPCNodeIdShopOrder != null && frmOperator.panelDetail.OPCNodeIdShopOrder != "")
                StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdShopOrder, false);

            foreach (var shopOrder in frmOperator.vw_ShopOrderGridModels)
                CreateProductionStop(frmOperator, userModel, shopOrder, resourceSure);

            decimal energy = 0; // frmOperator.energyDBHelper.GetCurrentTotalEnergyFromOPC(frmOperator.resource.Id);
            var panelStatus = PanelStatusManager.Current.GetPanelStatus(frmOperator.panelDetail.Id);
            decimal totalEnergy = 0;
            if (panelStatus != null && panelStatus.Count > 0 && panelStatus[0].EnergyStartValue > 0)
                totalEnergy = energy - panelStatus[0].EnergyStartValue;

            ShopOrderProductionManager.Current.SetShopOrderProductionFinish(frmOperator.shopOrderProduction.Id, DateTime.Now, userModel.CompanyPersonId, totalEnergy);
            if (!frmOperator.SubcontractorShopOrderProductionId.Equals(Guid.Empty))
                ShopOrderProductionManager.Current.SetShopOrderProductionFinish(frmOperator.SubcontractorShopOrderProductionId, DateTime.Now, userModel.CompanyPersonId, 0);

            frmOperator.SetMachineStateColor(MachineStateColor.ShopOrderWaiting);
            //eski çalışma şekli subcription testlerinden sonra değiştirildi sıfırlama handlechange içerisine atıldı 
            //StaticValues.opcClient.ResetCounter(frmOperator.panelDetail.OPCNodeIdCounterReset); 
            //StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, true);

            StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, true);
            StaticValues.opcClient.ResetCounter(frmOperator.panelDetail.OPCNodeIdCounterReset);
            System.Threading.Thread.Sleep(1000);
            StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdStartStop, true);


            //ToDo : Parça Üretim Süresi Gönderme metodu parça üretim süresine nasıl ulaşabiliriz
            //StaticValues.opcClient.WriteProductionTime(frmOperator.panelDetail.OPCNodeIdProductionTime, 10);

            frmOperator.shopOrderStatus = ShopOrderStatus.Start;
            frmOperator.vw_ShopOrderGridModels.Clear();
            frmOperator.vw_ShopOrderGridModelActive = null;
            frmOperator.exShopOrderProductions.Clear();
            frmOperator.productionDetails.Clear();
            frmOperator.shopOrderProductionDetails.Clear();
            frmOperator.shopOrderOperations.Clear();
            frmOperator.handlingUnits.Clear();
            frmOperator.counter = -1;
            frmOperator.notShopOrderCounter = -1;
            frmOperator.issueMaterialAllocModels.Clear();
        }

        private static void CreateProductionStop(FrmOperator frmOperator, UserModel userModel, Entity.Views.vw_ShopOrderGridModel shopOrder, decimal resourceSure)
        {
            try
            {
                var productionDetails = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetailsByProduction(frmOperator.shopOrderProduction.Id).Where(x => x.ByProduct == false);

                var orderProductionDetails = productionDetails.Where(x => x.OrderNo == shopOrder.orderNo && x.OperationNo == shopOrder.operationNo);
                if (orderProductionDetails.Count() == 0)
                    return;

                ProductionStop productionStop = new ProductionStop();

                productionStop.ProductionID = frmOperator.shopOrderProduction.Id;
                productionStop.ShopOrderID = shopOrder.Id;
                productionStop.UserID = userModel.CompanyPersonId;
                productionStop.Description1 = null;// frm.description;
                decimal sure = resourceSure;
                if (frmOperator.processNewActive)
                {
                    if (sure == 0)
                        sure = (decimal)(DateTime.Now - frmOperator.shopOrderProduction.OrderStartDate).TotalSeconds;
                    productionStop.Duration = (sure / 60) * orderProductionDetails.Sum(x => x.Quantity) / productionDetails.Sum(x => x.Quantity);
                }
                else if (sure > 0)
                    productionStop.Duration = sure / 60;
                ProductionStopManager.Current.Insert(productionStop);
            }
            catch
            {
            }
            finally
            {
                ShopOrderOperationManager.Current.SelectedUpdate(shopOrder.Id,
                    Selected: false,
                    workCenterRun: Guid.Empty,
                    resourceIdRun: Guid.Empty);
            }
        }
    }
}
