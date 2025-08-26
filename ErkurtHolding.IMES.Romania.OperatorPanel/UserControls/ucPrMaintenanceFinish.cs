using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucPrMaintenanceFinish : DevExpress.XtraEditors.XtraUserControl
    {
        List<vw_InventoryStock> selectedStocks = new List<vw_InventoryStock>();
        vw_InventoryStock selectedStock;

        PrMaintenanceProductModel selectedPrMaintenanceProduct;
        List<PrMaintenanceProductModel> prMaintenanceProductList = new List<PrMaintenanceProductModel>();

        public ucPrMaintenanceFinish()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            var startMaintanance = ToolsMdiManager.frmOperatorActive.PrMaintenance.FirstOrDefault(m => m.alan3 == ToolsMdiManager.frmOperatorActive.PrMaintenanceActive.alan3)?.StartMaintanance;
            grpMain.Text = $"{ToolsMdiManager.frmOperatorActive.PrMaintenanceActive.alan4} : {startMaintanance}";
            gridControl1.DataSource = prMaintenanceProductList;
        }

        private void barBtnFinish_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var maintenanceList = ToolsMdiManager.frmOperatorActive.PrMaintenance;
            var maintenanceDetailList = ToolsMdiManager.frmOperatorActive.PrMaintenanceActive;
            var detailListChange = maintenanceList.Single(x => x.WorkOrderNo == maintenanceDetailList.woNo).details.Single(x => x.alan3 == maintenanceDetailList.alan3);
            detailListChange.alan11 = "REPORTED";

            if (ToolsMdiManager.frmOperatorActive.PrMaintenance.All(maintenance => maintenance.details.All(detail => detail.alan11 == "REPORTED")))
            {
                ToolsMdiManager.frmOperatorActive.prMaintenanceButtonStatus = PrMaintenanceButtonStatus.FinishMaintenance;
            }
            else
            {
                ToolsMdiManager.frmOperatorActive.prMaintenanceButtonStatus = PrMaintenanceButtonStatus.InterventionStart;
            }
        }

        private void barBtnInsert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (selectedStock == null)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "972", "Öncelikle stok seçmelisiniz", "Message"));
                return;
            }

            if (spnQuantity.Value == 0)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "973", "Adet bilgisi girmelisiniz", "Message"));
                return;
            }

            try
            {
                selectedPrMaintenanceProduct = new PrMaintenanceProductModel();
                selectedPrMaintenanceProduct.ProductId = selectedStock.Id;
                selectedPrMaintenanceProduct.ProductDescription = selectedStock.description;
                selectedPrMaintenanceProduct.PartNo = selectedStock.PartNo;
                selectedPrMaintenanceProduct.Quantity = (double)spnQuantity.Value;

                selectedStocks.Add(selectedStock);
                prMaintenanceProductList.Add(selectedPrMaintenanceProduct);

                btnEditSelectedInventoryStock.Text = "";
                spnQuantity.Value = 0;
                gridControl1.BeginInit();
                gridControl1.DataSource = prMaintenanceProductList;
                gridControl1.EndInit();
                gridControl1.Refresh();
                gridView1.RefreshData();
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void barBtnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "976", "Öncelikle çıkartmak istediğiniz malzemeyi seçmelisiniz", "Message"));
                return;
            }
            List<PrMaintenanceProductModel> rowsList = new List<PrMaintenanceProductModel>();
            List<vw_InventoryStock> rowsStock = new List<vw_InventoryStock>();
            foreach (var selectedRow in selectedRows)
            {
                rowsList.Add(prMaintenanceProductList[selectedRow]);
                rowsStock.Add(selectedStocks[selectedRow]);
            }
            foreach (var selectedRow in rowsList)
            {
                prMaintenanceProductList.Remove(selectedRow);
            }
            foreach (var selectedRow in rowsStock)
            {
                selectedStocks.Remove(selectedRow);
            }
            gridControl1.Refresh();
            gridView1.RefreshData();
            gridView1.ClearSelection();
        }

        private void btnEditSelectedInventoryStock_Click(object sender, EventArgs e)
        {
            FrmInventoryStock frmStock = new FrmInventoryStock();
            if (frmStock.ShowDialog() == DialogResult.OK)
            {
                if (prMaintenanceProductList.Any(x => x.ProductId == frmStock.selectedStock.Id))
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "971", "Mevcut lot numarası veya barkod numarası daha önceden tamamlanmıştır", "Message"));
                    return;
                }
                selectedStock = frmStock.selectedStock;
                btnEditSelectedInventoryStock.Text = frmStock.selectedStock.description;
            }
        }
    }

}
