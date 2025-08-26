using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System.Collections.Generic;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucChooseWorkOrder : DevExpress.XtraEditors.XtraUserControl
    {
        List<int> workOrderSelectedRows = new List<int>();
        UserModel userModel { get; set; }

        public ucChooseWorkOrder(UserModel model)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            userModel = model;

            InitData();
        }

        #region INIT DATA
        private void InitData()
        {
            gcWorkOrders.DataSource = ToolsMdiManager.frmOperatorActive.vw_ShopOrderGridModels;
        }

        #endregion

        private void batBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void barBtnChoose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (workOrderSelectedRows.Count == 0)
            {
                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "966", "İş Emrini seçmeden ilerleyemezsiniz", "Message"));
                return;
            }
            ucBoxLabel ucBoxLabel = new ucBoxLabel(userModel);
            var shopOrder = (vw_ShopOrderGridModel)gvWorkOrders.GetRow(workOrderSelectedRows[0]);
            ucBoxLabel.SelectWorkOrder(shopOrder.Id);
            ucBoxLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            ToolsMdiManager.frmOperatorActive.container.Controls.Clear();
            ToolsMdiManager.frmOperatorActive.container.Controls.Add(ucBoxLabel);
        }

        private void gvWorkOrders_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            workOrderSelectedRows = gvWorkOrders.GetSelectedRows().ToList();
        }
    }

}
