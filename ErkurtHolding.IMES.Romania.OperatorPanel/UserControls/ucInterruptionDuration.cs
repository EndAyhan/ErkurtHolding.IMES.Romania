using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucInterruptionDuration : DevExpress.XtraEditors.XtraUserControl
    {
        public InterruptionCause interruptionCause { get; set; }

        public ucInterruptionDuration(InterruptionCause _couse)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            interruptionCause = _couse;
            var _opCause = OpInterruptionCauseManager.Current.GetOpInterruptionCauseById(_couse.CouseID);
            groupControl1.Text = _opCause.alan4;
            lblAlan2.Text = _opCause.alan2;
            lblDescription.Text = _opCause.description;
            lblStartDate.Text = _couse.InterruptionStartDate.ToString("dd/MM/yyyy HH:mm:ss");

            lblDuration.ForeColor = System.Drawing.Color.Red;

            if (_opCause.alan2 == "TEMİZLİK / GÜNLÜK OTONOM BAKIM")
            {
                btnLockFalse.Visible = true;
                btnLockTrue.Visible = true;
                btnLockFalse.Appearance.BackColor = Color.Gray;
                btnLockTrue.Appearance.BackColor = Color.Green;
            }
            else
            {
                btnLockFalse.Visible = false;
                btnLockTrue.Visible = false;
            }

            timer1.Start();
        }
        bool darkred = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (darkred)
            {

                lblDuration.ForeColor = panel1.BackColor = System.Drawing.Color.Red;
                darkred = false;
            }
            else
            {
                panel1.BackColor = System.Drawing.SystemColors.HotTrack;
                darkred = true;
            }

            lblDuration.Text = (interruptionCause.InterruptionStartDate - DateTime.Now).ToString(@"dd\.hh\:mm\:ss");

        }

        private void btnLockFalse_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(false);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, false);
                if (ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != null && ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != "")
                    StaticValues.opcClient.WriteNode(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption, false);
                btnLockFalse.Appearance.BackColor = Color.Green;
                btnLockTrue.Appearance.BackColor = Color.Gray;
            }
        }

        private void btnLockTrue_Click(object sender, EventArgs e)
        {
            FrmUserLogin frm = new FrmUserLogin(false);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, true);
                if (ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != null && ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != "")
                    StaticValues.opcClient.WriteNode(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption, true);
                btnLockFalse.Appearance.BackColor = Color.Gray;
                btnLockTrue.Appearance.BackColor = Color.Green;
            }
        }
    }
}
