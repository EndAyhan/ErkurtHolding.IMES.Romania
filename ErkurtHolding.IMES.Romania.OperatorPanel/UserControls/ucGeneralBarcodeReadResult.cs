using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System.Drawing;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucGeneralBarcodeReadResult : DevExpress.XtraEditors.XtraUserControl
    {
        public ucGeneralBarcodeReadResult(string workCenter, string amount, bool ok)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            lblWorkCenter.Text = workCenter;
            lblAmount.Text = amount;
            if (ok)
            {
                lblStatus.Text = "OK";
                background.BackColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "NOK";
                background.BackColor = Color.Red;
            }
        }
    }
}
