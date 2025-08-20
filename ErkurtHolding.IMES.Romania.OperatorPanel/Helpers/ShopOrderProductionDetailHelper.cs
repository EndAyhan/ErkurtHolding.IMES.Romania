using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class ShopOrderProductionDetailHelper
    {
        public static ShopOrderProductionDetail CreateAndInsertProductionDetail(vw_ShopOrderGridModel shopOrder, Product product, bool printed, UserModel userModel, int crewSize, decimal quantity = 1, decimal handlingUnitQuantity = 0)
        {
            if (shopOrder == null)
                shopOrder = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels.Single(x => x.ProductID == product.Id);

            return CreateAndInsertProductionDetail(
                ToolsMdiManager.frmOperatorActive.machine.Id,
                ToolsMdiManager.frmOperatorActive.resource.Id,
                ToolsMdiManager.frmOperatorActive.shopOrderProduction.Id,
                ToolsMdiManager.frmOperatorActive.partHandlingUnits.First(x => x.PartNo == product.PartNo).Id,
                shopOrder,
                product.Id,
                printed,
                userModel.CompanyPersonId,
                crewSize,
                quantity,
                handlingUnitQuantity);
        }

        public static ShopOrderProductionDetail CreateAndInsertProductionDetail(Guid machineId, Guid resourceId, Guid shopOrderProductionId, Guid parHandlingUnitID, vw_ShopOrderGridModel shopOrder, Guid productId, bool printed, Guid userId, int crewSize, decimal quantity = 1, decimal handlingUnitQuantity = 0)
        {
            ShopOrderProductionDetail productionDetail = new ShopOrderProductionDetail();
            productionDetail.ShopOrderOperationID = shopOrder.Id;
            productionDetail.ShopOrderProductionID = shopOrderProductionId;
            productionDetail.WorkCenterID = machineId;
            productionDetail.ResourceID = resourceId;
            productionDetail.ShiftId = StaticValues.shift.Id;
            productionDetail.StartDate = productionDetail.EndDate = DateTime.Now;
            productionDetail.Unit = shopOrder.unitMeas;
            productionDetail.Quantity = quantity;
            productionDetail.BoxID = Guid.Empty;
            productionDetail.ProductionStateID = StaticValues.specialCodeOk.Id;
            productionDetail.Factor = 1;
            productionDetail.Divisor = 1;
            productionDetail.Printed = printed;
            productionDetail.ProductID = productId;
            productionDetail.ManualInput = quantity;
            productionDetail.Active = true;
            productionDetail.ParHandlingUnitID = parHandlingUnitID;
            if (ToolsMdiManager.frmOperatorActive.manuelLabelUser != null)
            {
                productionDetail.CompanyPersonId = ToolsMdiManager.frmOperatorActive.manuelLabelUser.CompanyPersonId;
            }
            else
            {
                productionDetail.CompanyPersonId = userId;
            }

            productionDetail.HandlingUnitQuantity = handlingUnitQuantity;
            productionDetail.OrderNo = shopOrder.orderNo;
            productionDetail.OperationNo = shopOrder.operationNo;
            productionDetail.CrewSize = crewSize;
            productionDetail = ShopOrderProductionDetailManager.Current.Insert(productionDetail).ListData[0];
            productionDetail.Barcode = productionDetail.serial.ToString();
            ShopOrderProductionDetailManager.Current.UpdateBarcode(productionDetail);

            return productionDetail;
        }

        public static ShopOrderProductionDetail CreateAndInsertScrapProductionDetail(FrmOperator frmOperator, vw_ShopOrderGridModel shopOrder, Product product, bool printed, UserModel userModel, int crewSize, decimal quantity = 1, decimal handlingUnitQuantity = 0, bool btnManuelScrap = false)
        {
            if (shopOrder == null)
                shopOrder = frmOperator.vw_ShopOrderGridModels.Single(x => x.ProductID == product.Id);
            ShopOrderProductionDetail productionDetail = new ShopOrderProductionDetail();

            productionDetail.ShopOrderOperationID = shopOrder.Id;
            productionDetail.ShopOrderProductionID = frmOperator.shopOrderProduction.Id;
            productionDetail.WorkCenterID = frmOperator.machine.Id;
            productionDetail.ResourceID = frmOperator.resource.Id;
            productionDetail.ShiftId = StaticValues.shift.Id;
            productionDetail.StartDate = productionDetail.EndDate = DateTime.Now;
            productionDetail.Unit = shopOrder.unitMeas;
            productionDetail.Quantity = quantity;
            productionDetail.BoxID = Guid.Empty;
            productionDetail.CustomerBoxID = Guid.Empty;
            productionDetail.ProductionStateID = StaticValues.specialCodeScrap.Id;
            productionDetail.IfsScrapTypeId = frmOperator.scrapReason.Id;
            productionDetail.Factor = 1;
            productionDetail.Divisor = 1;
            productionDetail.Printed = printed;
            productionDetail.ProductID = product.Id;
            productionDetail.ManualInput = quantity;
            productionDetail.Active = true;
            productionDetail.ParHandlingUnitID = frmOperator.partHandlingUnits.First(x => x.PartNo == product.PartNo).Id;
            productionDetail.CompanyPersonId = userModel.CompanyPersonId;
            productionDetail.HandlingUnitQuantity = handlingUnitQuantity;
            productionDetail.OrderNo = shopOrder.orderNo;
            productionDetail.OperationNo = shopOrder.operationNo;
            productionDetail.CrewSize = crewSize;

            productionDetail = ShopOrderProductionDetailManager.Current.Insert(productionDetail).ListData[0];

            //try
            //{
            //    if (btnManuelScrap == false)
            //    {
            //        OperationalScrapReport operationalScrapReport = new OperationalScrapReport();
            //        operationalScrapReport.CONTRACT = StaticValues.ifsContract;
            //        operationalScrapReport.CURRENT_EMPLOYEE_ID = userModel.IfsEmplooyeId;
            //        operationalScrapReport.OPERATION_ID = shopOrder.opId;
            //        operationalScrapReport.ORDER_NO = shopOrder.orderNo;
            //        operationalScrapReport.RELEASE_NO = shopOrder.releaseNo;
            //        operationalScrapReport.SEQUENCE_NO = shopOrder.sequenceNo;
            //        operationalScrapReport.OPERATION_NO = shopOrder.operationNo;
            //        operationalScrapReport.SCRAP_REASON = frmOperator.scrapReason.rejectReason;
            //        operationalScrapReport.URUN_ETIKET = productionDetail.serial.ToString();
            //        operationalScrapReport.QTY = (double)quantity;
            //        operationalScrapReport.KASA_BARKOD = "";
            //        operationalScrapReport.LOT_BATCH_NO = "";
            //        IfsSendReport.IFSSendOperationalScrapReport(operationalScrapReport);
            //    }
            //}
            //catch (Exception)
            //{
            //}

            productionDetail.Barcode = productionDetail.serial.ToString();
            ShopOrderProductionDetailManager.Current.UpdateBarcode(productionDetail);

            return productionDetail;
        }


        public static void PrintScrapProduction(Product product, ShopOrderProduction shopOrderProduction, vw_ShopOrderGridModel shopOrderGridModel, ShopOrderProductionDetail shopOrderProductionDetail, Machine machine, Machine resource, UserModel userModel)
        {
            if (StaticValues.ScrapPrinterName == null || StaticValues.ScrapPrinterName.Length == 0)
                return;
            ReportProductHelper report = new ReportProductHelper();
            report.product = product;
            report.shopOrderProduction = shopOrderProduction;
            report.shopOrderProductionDetail = shopOrderProductionDetail;
            report.machine = machine;
            report.resource = resource;
            report.userModel = userModel;
            report.shopOrderOperation = shopOrderGridModel;
            report.printLabelModel = new PrintLabelModel()
            {
                LabelDesingFilePath = StaticValues.ScrapProductDesignPath,
                PrintCopyCount = 1,
                printerName = StaticValues.ScrapPrinterName,
                productionLabelType = ProductionLabelType.Product
            };

            report.PrintLabel(true);
        }
    }
}
