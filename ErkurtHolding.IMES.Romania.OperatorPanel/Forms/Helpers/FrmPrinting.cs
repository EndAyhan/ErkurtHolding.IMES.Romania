using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;
using System.ComponentModel;
using System.Threading;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmPrinting : DevExpress.XtraEditors.XtraForm
    {
        private BackgroundWorker BgWorker;

        public FrmPrinting()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            SetLookAndFeel();
        }

        private void SetLookAndFeel()
        {
            lblPrintingText.LookAndFeel.SkinName = "The Asphalt World";
            lblPrintingText.LookAndFeel.UseDefaultLookAndFeel = false;
        }

        private void FrmPrinting_Shown(object sender, EventArgs e)
        {
            BgWorker = new BackgroundWorker();
            BgWorker.DoWork += new DoWorkEventHandler(BgWorker_DoWork);
            BgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgWorker_RunWorkerCompleted);
            if (!BgWorker.IsBusy)
                BgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Sleep for 1 seconds
        /// </summary>
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }
        /// <summary>
        /// After the background worker is done close and dispose form
        /// </summary>
        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }

}