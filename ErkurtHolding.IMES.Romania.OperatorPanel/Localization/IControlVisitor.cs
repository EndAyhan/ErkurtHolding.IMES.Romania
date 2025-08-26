using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Defines a visitor interface for traversing and processing
    /// Windows Forms <see cref="Control"/> elements in a control tree.
    /// </summary>
    public interface IControlVisitor
    {
        /// <summary>
        /// Applies visitor logic to the specified <see cref="Control"/>.
        /// Implementations can perform localization, styling, or any
        /// other transformation on the control and its children.
        /// </summary>
        /// <param name="control">
        /// The <see cref="Control"/> instance to visit. 
        /// Must not be <c>null</c>.
        /// </param>
        void VisitControl(Control control);
    }
}
