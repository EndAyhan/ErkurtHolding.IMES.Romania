using ErkurtHolding.IMES.Entity.QueryModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucPrMaintenance : DevExpress.XtraEditors.XtraUserControl
    {
        public ucPrMaintenance()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            LoadPrMaintenanceData();
            BindMchCodesToGridLookup();
            gcMaintenanceDetail.MouseUp += gcMaintenanceDetail_MouseUp;
        }
        private void gcMaintenanceDetail_MouseUp(object sender, MouseEventArgs e)
        {
            var gridView = gcMaintenanceDetail.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (gridView == null) return;

            if (gridView.IsValidRowHandle(0))
            {
                gridView.SelectRow(0);
                gridView.FocusedRowHandle = 0;
            }
        }

        private void WaitForInitData()
        {
            //splashScreenManager1.ShowWaitForm();
            //splashScreenManager1.SetWaitFormCaption(MessageTextHelper.GetMessageText("000", "867", "Lütfen bekleyiniz", "Message"));
            //Thread initDataThread = new Thread(() => ToolsMdiManager.frmOperatorActive.PrMaintenance = EquipmentSerialPrHelper.FrmPrMaintenanceInitData());
            //initDataThread.Start();
            //initDataThread.Join();
            //splashScreenManager1.CloseWaitForm();
        }

        private void LoadPrMaintenanceData()
        {
            if (ToolsMdiManager.frmOperatorActive.PrMaintenanceActive == null)
            {
                labelControl1.Visible = false;
                labelControl4.Visible = false;
                labelControl2.Visible = false;
                labelControl3.Visible = false;
                WaitForInitData();
                return;
            }

            var activeMaintenanceDetail = ToolsMdiManager.frmOperatorActive.PrMaintenanceActive;
            var activeMaintenance = ToolsMdiManager.frmOperatorActive.PrMaintenance;
            gridLookUpEdit1.Visible = false;
            gleMaintenanceMain.Visible = false;
            labelControl1.Text = $"{activeMaintenanceDetail.mchCode}-{activeMaintenanceDetail.mchCodeDescription}";
            labelControl4.Text = activeMaintenanceDetail.woNo.ToString();

            var item = activeMaintenance.Single(x => x.WorkOrderNo == activeMaintenanceDetail.woNo);
            activeMaintenance.Clear();
            activeMaintenance.Add(item);
            if (ToolsMdiManager.frmOperatorActive.PrMaintenance.All(maintenance => maintenance.details.All(detail => detail.alan11 == "REPORTED")))
            {
                labelControl2.Text = MessageTextHelper.GetMessageText("000", "868", "İş emrine ait görevleri tamamladınız. Periyodik Bakımı Sonlandırınız", "Message");
                barBtnClose.Enabled = false;
                barBtnStartWorkOrder.Enabled = false;
                labelControl1.Visible = false;
                labelControl3.Visible = false;
                labelControl4.Visible = false;


            }

        }

        private void BindMchCodesToGridLookup()
        {
            var mchCodes = ToolsMdiManager.frmOperatorActive.PrMaintenance
                .Select(x => new { x.mchCode, x.mchCodeDescription })
                .Distinct()
                .OrderBy(x => x.mchCode)
                .ToList();

            gridLookUpEdit1.Properties.DataSource = mchCodes;
            gridLookUpEdit1.EditValue = gridLookUpEdit1.Properties.GetKeyValue(0);
            gridLookUpEdit1.Properties.View.Columns.Clear();
            gridLookUpEdit1.Properties.View.Columns.AddVisible("mchCode", MessageTextHelper.GetMessageText("000", "869", "Nesne Kodu", "Message"));
            gridLookUpEdit1.Properties.View.Columns.AddVisible("mchCodeDescription", MessageTextHelper.GetMessageText("000", "870", "Nesne Açıklaması", "Message"));
            gridLookUpEdit1.Properties.PopupFormSize = new Size(gridLookUpEdit1.Width, gridLookUpEdit1.Properties.PopupFormSize.Height);
            gridLookUpEdit1.Properties.View.BestFitColumns();
        }

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            string selectedMchCode = gridLookUpEdit1.EditValue.ToString();
            gleMaintenanceMain.Properties.DataSource = ToolsMdiManager.frmOperatorActive.PrMaintenance.Where(x => x.mchCode == selectedMchCode).ToList();
            if (gleMaintenanceMain.Properties.DataSource is List<MaintenanceMain> dataSource && dataSource.Count > 0)
            {
                gleMaintenanceMain.EditValue = dataSource[0].WorkOrderNo;
            }

        }

        private void gleMaintenanceMain_EditValueChanged(object sender, EventArgs e)
        {
            if (gleMaintenanceMain.EditValue != null)
            {
                double selectedWorkOrderNo = Convert.ToDouble(gleMaintenanceMain.EditValue);
                var selectedMain = ToolsMdiManager.frmOperatorActive.PrMaintenance.FirstOrDefault(x => x.WorkOrderNo == selectedWorkOrderNo);
                if (selectedMain != null)
                {
                    gcMaintenanceDetail.DataSource = selectedMain.details
                    .Where(detail => detail.alan11 != "REPORTED")
                    .OrderBy(detail => detail.woNo);
                }
            }
        }


        private void barBtnStartWorkOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedRows = gvMaintenanceDetail.GetSelectedRows();
            ToolsMdiManager.frmOperatorActive.PrMaintenanceActive = (MaintenanceDetail)gvMaintenanceDetail.GetRow(selectedRows[0]);
            var loginUser = ToolsMdiManager.frmOperatorActive.machinePeriyodicStartUser;
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
            ToolsMdiManager.frmOperatorActive.prMaintenanceButtonStatus = PrMaintenanceButtonStatus.InterventionStop;
            StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, true);
        }

        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Tag = ContainerSelectUserControl.PrMaintenance;
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }
    }

}
