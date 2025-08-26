using ErkurtHolding.IMES.Business.Views;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    public partial class FrmInventoryStock : DevExpress.XtraEditors.XtraForm
    {
        public vw_InventoryStock selectedStock = new vw_InventoryStock();
        public FrmInventoryStock()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            InitData();
        }

        private void InitData()
        {

            try
            {
                var data = vw_InventoryStockManager.Current.GetAllActiveStockByBranchID(StaticValues.panel.BranchId);
                if (data == null)
                {
                    throw new Exception("");
                }
                gridControl1.DataSource = data;
            }
            catch (Exception)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "908", "Stok listesine ulaşılamadı.\r\nLütfen sistem yöneticiniz ile temasa geçiniz.", "Message"));
            }
        }

        private void barLargeButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "909", "Öncelikle seçim yapmalısınız", "Message"));
                return;
            }


            selectedStock = (vw_InventoryStock)gridView1.GetRow(gridView1.FocusedRowHandle);

            this.DialogResult = DialogResult.OK;
        }

        private void barBtnFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmVirtualKeyboard frm = new FrmVirtualKeyboard(gridView1.FindFilterText);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                gridView1.ApplyFindFilter(frm.InputText);
            }
        }
    }

}