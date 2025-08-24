using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    public partial class FrmAdminLogin : DevExpress.XtraEditors.XtraForm
    {
        public FrmAdminLogin()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                if (txtPassword.Text == "gizli.şifre")
                    this.DialogResult = DialogResult.OK;
                else
                    this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}