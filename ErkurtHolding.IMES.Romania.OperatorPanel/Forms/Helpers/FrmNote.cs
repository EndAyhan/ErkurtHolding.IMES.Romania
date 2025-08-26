using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmNote : DevExpress.XtraEditors.XtraForm
    {
        public FrmNote()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);
        }

        public string GetNote()
        {
            return txtNote.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}