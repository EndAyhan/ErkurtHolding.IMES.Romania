using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class ShopOrderStartHelper
    {
        public static bool ShopOrderStartControl(Product product, vw_ShopOrderGridModel shopOrder)
        {
            bool flag = true;

            var resultMaterialAlloc = ShopMaterialAllocManager.Current.GetlistByOrderID(shopOrder.Id);

            if (resultMaterialAlloc == null)
                return true;

            foreach (var materialAlloc in resultMaterialAlloc)
            {
                int waitTime = 0;

                if (int.TryParse(materialAlloc.alan1, out waitTime))
                {
                    if (waitTime > 0)
                    {
                        double totalRequired = 0;

                        var resultInventoryStock = InventoryStockManager.Current.GetInventoryStock(materialAlloc.PartID, StaticValues.branch.Id);
                        if (resultInventoryStock.HasEntries())
                        {
                            foreach (var inventoryStock in resultInventoryStock)
                            {
                                var handlingUnitResult = HandlingUnitManager.Current.GetHandlingUnitByBarcodeOrSerial(inventoryStock.lotBatchNo);

                                if (handlingUnitResult != null)
                                {
                                    if ((DateTime.Now - handlingUnitResult.CreatedAt).TotalHours > Convert.ToDouble(materialAlloc.alan1))
                                        totalRequired += (double)handlingUnitResult.Quantity;
                                }
                                else
                                {
                                    totalRequired += inventoryStock.availableQty;
                                }
                            }

                            flag = totalRequired > materialAlloc.qtyRequired;
                        }
                    }
                }
            }
            return flag = true;
        }

        public static void PrintAndLabelSettings(FrmOperator frmOperator, Product product, PanelDetail panelDetail)
        {
            //Etiket ve yazıcı yarları
            if (panelDetail.PrintProductBarcode)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Product));
            if (panelDetail.ProcessBarcode)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Process));
            if (!panelDetail.BoxFillsUp && !frmOperator.panelDetail.Scales)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Box));
            if (panelDetail.BoxFillsUp)
                frmOperator.printLabelModels.Add(PrintLabelHelper.GetLabelModel(product, frmOperator.machine, frmOperator.resource, ProductionLabelType.Box));
        }


        public static bool SetMachineRunMode(FrmOperator frmOperator, bool flag, PanelDetail panelDetail, vw_ShopOrderGridModel shopOrder)
        {
            if (flag)
            {
                if (frmOperator.opcOtherReadModels.Any(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePlcRunModeParameter.Id))
                {
                    var runModeModel = frmOperator.opcOtherReadModels.First(x => x.SpecialCodeId == StaticValues.specialCodeMachineOPCDataTypePlcRunModeParameter.Id);

                    //Session probleminden sonra değiştirildi
                    StaticValues.opcClient.WriteRunMode(45, runModeModel.NodeId);

                    System.Threading.Thread.Sleep(1000);

                    if (shopOrder.operationNo == 10)
                        StaticValues.opcClient.WriteRunMode(40, runModeModel.NodeId, true);
                    else
                        StaticValues.opcClient.WriteRunMode(50, runModeModel.NodeId, true);

                    flag = false;
                }
            }

            return flag;
        }

        public static void ShopOrderProductionDetailSettings(FrmOperator frmOperator, vw_ShopOrderGridModel shopOrder, bool selected = true)
        {
            //forma iş emrini ekleme için
            var selectedShopOrder = ShopOrderOperationManager.Current.GetShopOrderOperationById(shopOrder.Id);
            frmOperator.shopOrderOperations.Add(selectedShopOrder);


            //iş emrinin anlık olarak nerede çalışıldığını görmek için
            ShopOrderOperationManager.Current.SelectedUpdate(
                shopOrder.Id,
                Selected: selected,
                workCenterRun: frmOperator.machine.Id,
                resourceIdRun: frmOperator.resource.Id);

            //daha önce iş emri çalışıldımı kontrolleri başlıyor
            var shopOrderProductions = ShopOrderProductionManager.Current.GetShopOrderProductionByShopOrderID(shopOrder.Id);

            if (shopOrderProductions.HasEntries())
            //if(shopOrderProductions!=null || shopOrderProductions.Count>0)//iş emri daha önce üretildi ise
            {

                //Üretilen Eski ürün Listesi
                var resultDetails = ShopOrderProductionDetailManager.Current.GetShopOrderProductionDetails(shopOrder.Id);
                if (resultDetails != null && resultDetails.Count > 0)
                {
                    frmOperator.productionDetails.AddRange(resultDetails.Where(x => x.ByProduct == false));
                    frmOperator.productionDetailsByProducts.AddRange(resultDetails.Where(x => x.ByProduct == true));
                }

                //üretilen kasa listesi
                var resultHandlingUnits = HandlingUnitManager.Current.GetHandlingUnitByShopOrderOperationId(shopOrder.Id);
                if (resultHandlingUnits != null && resultHandlingUnits.Count > 0)
                    frmOperator.handlingUnits.AddRange(resultHandlingUnits);
            }
        }

        public static ShopOrderProduction InsertShopOrderProduction(Guid workCenterID, Guid resourceID, UserModel userModel, bool process = false)
        {
            try
            {
                ShopOrderProduction shopOrderProduction = new ShopOrderProduction();
                shopOrderProduction.ShopOrderID = Guid.Empty;
                shopOrderProduction.OrderStartDate = shopOrderProduction.SetupStartDate = DateTime.Now;
                shopOrderProduction.OrderFinishDate = shopOrderProduction.SetupFinishDate = DateTime.Now;
                shopOrderProduction.QuantityScrap = 0;
                shopOrderProduction.QuantityOk = 0;
                shopOrderProduction.QuantityNok = 0;
                shopOrderProduction.WorkCenterID = workCenterID;
                shopOrderProduction.ResourceID = resourceID;
                shopOrderProduction.StartCompanyPersonID = userModel.CompanyPersonId;
                shopOrderProduction.Process = process;
                shopOrderProduction.TotalEnergyConsumed = 0;
                var result = ShopOrderProductionManager.Current.Insert(shopOrderProduction);

                return result.ListData[0];
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
