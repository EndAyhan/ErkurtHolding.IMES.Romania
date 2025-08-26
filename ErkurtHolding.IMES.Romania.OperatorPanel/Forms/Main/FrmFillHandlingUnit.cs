using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Main
{
    public partial class FrmFillHandlingUnit : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FrmFillHandlingUnit()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);
        }
    }
}