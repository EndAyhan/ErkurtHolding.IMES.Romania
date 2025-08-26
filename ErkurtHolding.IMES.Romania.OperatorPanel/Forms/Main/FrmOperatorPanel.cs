using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Main
{
    public partial class FrmOperatorPanel : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FrmOperatorPanel()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);
        }
    }
}