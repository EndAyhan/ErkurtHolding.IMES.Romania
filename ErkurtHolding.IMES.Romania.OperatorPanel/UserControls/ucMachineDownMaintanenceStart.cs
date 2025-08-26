using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity.ImesDataModel;
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
    public partial class ucMachineDownMaintanenceStart : DevExpress.XtraEditors.XtraUserControl
    {
        public Fault fault { get; set; }
        List<UserModel> userModels = new List<UserModel>();
        public ucMachineDownMaintanenceStart(Fault _fault, UserModel userModel)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            fault = _fault;
            grpMain.Text = $"{fault.ErrDescription} : {fault.RegisterDate}";

            userModels.Add(userModel);
            gridControl1.DataSource = userModels;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDuration.Text = (fault.RegisterDate - DateTime.Now).ToString(@"dd\.hh\:mm\:ss");
            gridView1.RefreshData();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(UserLoginAuthorization.maintenanceUserAuthorization);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (userModels.Any(x => x.CompanyPersonId == frm.userModel.CompanyPersonId))
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "978", "Operatör listede mevcut. Başka bir kullanıcı ile giriş yapabilirsiniz", "Message"));
                    return;
                }
                userModels.Add(frm.userModel);

                var result = ActiveSeparateManager.Current.GetActiveSeparateBYIMES_ID(ToolsMdiManager.frmOperatorActive.faults.Last().Imes_ID);

                gridControl1.DataSource = userModels;
                gridControl1.RefreshDataSource();
                gridView1.RefreshData();
                //}
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (userModels.Count == 1)
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "979", "Son bakımcı arızayı bitiremeden çıkamaz", "Message"));
                    return;
                }

                FrmUserLogin frm = new FrmUserLogin(true);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (userModels.Any(x => x.CompanyPersonId == frm.userModel.CompanyPersonId))
                    {
                        var result = ActiveSeparateManager.Current.GetActiveSeparateBYIMES_ID(ToolsMdiManager.frmOperatorActive.faults.Last().Imes_ID);

                        userModels.RemoveAll(x => x.CompanyPersonId == frm.userModel.CompanyPersonId);

                        gridControl1.DataSource = userModels;
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


        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "gcUnboundColumn" && e.IsGetData)
                {

                    Object date = gridView1.GetListSourceRowCellValue(e.ListSourceRowIndex, "StartDate");
                    if (date != null)
                    {
                        e.Value = Convert.ToDouble(Math.Round((DateTime.Now - Convert.ToDateTime(date)).TotalMinutes, 0));
                    }

                }
            }
            catch { }
        }

        private void btnMachineLockFalse_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //var laborClases = LaborClassManager.Current.GetLaborClasses(StaticValues.panel.BranchId, frm.userModel.CompanyPersonId);
                //var maintananceClass = laborClases.Where(x => x.laborClassNo == StaticValues.branch.ERPConnectionCode + "BAKIM").ToList();
                //var maintananceClass = userModels.Where(x => x.IfsEmplooyeId == frm.userModel.IfsEmplooyeId).ToList();
                if (userModels.Any(x => x.IfsEmplooyeId == frm.userModel.IfsEmplooyeId))
                {
                    StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, false);
                    if (ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != null && ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != "")
                        StaticValues.opcClient.WriteNode(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption, false);

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
                //var laborClases = LaborClassManager.Current.GetLaborClasses(StaticValues.panel.BranchId, frm.userModel.CompanyPersonId);
                //var maintananceClass = laborClases.Where(x => x.laborClassNo == StaticValues.branch.ERPConnectionCode + "BAKIM").ToList();
                //var maintananceClass = userModels.Where(x => x.IfsEmplooyeId == frm.userModel.IfsEmplooyeId).ToList();
                if (userModels.Any(x => x.IfsEmplooyeId == frm.userModel.IfsEmplooyeId))
                {
                    StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, true); if (ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != null && ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != "")
                        StaticValues.opcClient.WriteNode(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption, true);
                }
                else
                {
                    var prm = $"{StaticValues.branch.ERPConnectionCode}BAKIM".CreateParameters("@LaborClass");
                    ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "982", "Makine kilit sadece ilgili bakım ekibi tarafından aktif edilebilir. Bu personel @LaborClass üyesi değil yada arıza için girişi yapılmamış", "Message"), prm);
                }

            }
        }
    }

}
