using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    public interface IControlVisitor
    {
        void VisitControl(Control control);
    }
}
