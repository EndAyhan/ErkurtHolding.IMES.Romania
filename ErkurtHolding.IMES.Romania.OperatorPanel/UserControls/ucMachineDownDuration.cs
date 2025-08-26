using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucMachineDownDuration : DevExpress.XtraEditors.XtraUserControl
    {
        public Fault fault { get; set; }
        public ucMachineDownDuration(Fault _fault)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            fault = _fault;

            var prm = ToolsMdiManager.frmOperatorActive.machindeDownStartUser.Name.CreateParameters("@UserName");
            lblWorkTypeID.Text = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("000", "865", "Arızayı Bildiren: @UserName", "Message"), prm);
            lblErrDescription.Text = fault.ErrDescription.Split('-')[0];
            lblStartDate.Text = fault.RegisterDate.ToString("dd/MM/yyyy HH:mm:ss");

            lblDuration.ForeColor = System.Drawing.Color.Red;

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

            lblDuration.Text = (fault.RegisterDate - DateTime.Now).ToString(@"dd\.hh\:mm\:ss");
        }
    }
}
