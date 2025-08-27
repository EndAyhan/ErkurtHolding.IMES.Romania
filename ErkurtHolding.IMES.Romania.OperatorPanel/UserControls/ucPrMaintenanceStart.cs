using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.QueryModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucPrMaintenanceStart : DevExpress.XtraEditors.XtraUserControl
    {
        UserModel userModels = ToolsMdiManager.frmOperatorActive.machinePeriyodicStartUser;
        List<UserModel> userModelList = new List<UserModel>();
        List<MaintenanceMain> maintenanceMains = ToolsMdiManager.frmOperatorActive.PrMaintenance;
        MaintenanceDetail maintenanceDetail = ToolsMdiManager.frmOperatorActive.PrMaintenanceActive;

        public ucPrMaintenanceStart()
        {
            InitializeComponent();

            // Designer/localization for static labels & captions
            LanguageHelper.InitializeLanguage(this);

            // Data-prep
            var matchingItems = maintenanceMains.Where(m => m.alan3 == maintenanceDetail.alan3).ToList();
            var startMaintanance = maintenanceMains.FirstOrDefault(m => m.alan3 == maintenanceDetail.alan3)?.StartMaintanance;
            grpMain.Text = $"{maintenanceDetail.alan4} : {startMaintanance}";
            userModelList.Add(userModels);
            gridControl1.DataSource = userModelList;
            timer1.Start();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(UserLoginAuthorization.maintenanceUserAuthorization);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (userModelList.Any(x => x.CompanyPersonId == frm.userModel.CompanyPersonId))
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "978", "Operatör listede mevcut. Başka bir kullanıcı ile giriş yapabilirsiniz", "Message"));
                    return;
                }
                userModelList.Add(frm.userModel);

                gridControl1.DataSource = userModelList;
                gridControl1.RefreshDataSource();
                gridView1.RefreshData();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (userModelList.Count == 1)
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "979", "Son bakımcı arızayı bitiremeden çıkamaz", "Message"));
                    return;
                }

                FrmUserLogin frm = new FrmUserLogin(UserLoginAuthorization.maintenanceUserAuthorization);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (userModelList.Any(x => x.CompanyPersonId == frm.userModel.CompanyPersonId))
                    {
                        userModelList.RemoveAll(x => x.CompanyPersonId == frm.userModel.CompanyPersonId);

                        gridControl1.DataSource = userModelList;
                        gridControl1.RefreshDataSource();
                        gridView1.RefreshData();
                    }
                    else
                    {
                        ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "980", "Çıkış yapabilmek için öncelikle giriş yapmalısınız", "Message"));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void btnMachineLockFalse_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (userModelList.Any(x => x.IfsEmplooyeId == frm.userModel.IfsEmplooyeId))
                {
                    StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, false);
                }
                else
                {
                    var prm = $"{StaticValues.branch.ERPConnectionCode}BAKIM".CreateParameters("@LaborClass");
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "981", "Makine kilit sadece ilgili bakım ekibi tarafından kaldırılabilir. Bu personel @LaborClass üyesi değil yada arıza için girişi yapılmamış", "Message"), prm);
                }

            }
        }

        private void btnMachineLockActive_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (userModelList.Any(x => x.IfsEmplooyeId == frm.userModel.IfsEmplooyeId))
                {
                    StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, true);
                }
                else
                {
                    var prm = $"{StaticValues.branch.ERPConnectionCode}BAKIM".CreateParameters("@LaborClass");
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "982", "Makine kilit sadece ilgili bakım ekibi tarafından aktif edilebilir. Bu personel @LaborClass üyesi değil yada arıza için girişi yapılmamış", "Message"), prm);
                }

            }
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "gcUnboundColumn" && e.IsGetData)
                {
                    object date = gridView1.GetListSourceRowCellValue(e.ListSourceRowIndex, "StartDate");
                    if (date != null)
                    {
                        e.Value = Convert.ToDouble(Math.Round((DateTime.Now - Convert.ToDateTime(date)).TotalMinutes, 0));
                    }
                }
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var startMaintanance = maintenanceMains.FirstOrDefault(m => m.WorkOrderNo == maintenanceDetail.woNo)?.StartMaintanance;
            lblDuration.Text = (startMaintanance - DateTime.Now)?.ToString(@"dd\.hh\:mm\:ss");
            gridView1.RefreshData();
        }
    }
}
