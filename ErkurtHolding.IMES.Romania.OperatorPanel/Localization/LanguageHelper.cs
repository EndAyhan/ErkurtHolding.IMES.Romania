using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    public static class LanguageHelper
    {
        internal static void InitializeLanguage(XtraForm frm)
        {
            GeneralControlVisitor visitor = new GeneralControlVisitor(frm.Tag);
            visitor.VisitControl(frm);
        }
        internal static void InitializeLanguage(WaitForm frm)
        {
            GeneralControlVisitor visitor = new GeneralControlVisitor(frm.Tag);
            visitor.VisitControl(frm);
        }
        internal static void InitializeLanguage(UserControl frm)
        {
            GeneralControlVisitor visitor = new GeneralControlVisitor(frm.Tag);
            visitor.VisitControl(frm);
        }
    }
}
