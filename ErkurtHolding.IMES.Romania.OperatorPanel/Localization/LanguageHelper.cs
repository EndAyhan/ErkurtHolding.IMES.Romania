using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Language bootstrapper that applies localization to a Windows Forms control tree.
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// Localizes the given <paramref name="root"/> control and its children using <see cref="GeneralControlVisitor"/>.
        /// </summary>
        /// <param name="root">Root control (e.g., <see cref="XtraForm"/>, <see cref="WaitForm"/>, <see cref="UserControl"/>).</param>
        /// <param name="overrideScopeTag">
        /// Optional scope identifier to use instead of <c>root.Tag</c>. 
        /// Pass when you want to localize a form with a different key (e.g., shared controls).
        /// </param>
        public static void InitializeLanguage(Control root, object overrideScopeTag = null)
        {
            if (root == null) return;

            // Prefer explicit scope if provided; otherwise use the form/control's Tag.
            var scopeTag = overrideScopeTag ?? root.Tag ?? string.Empty;

            var visitor = new GeneralControlVisitor(scopeTag);
            visitor.VisitControl(root);
        }

        // ---- Convenience overloads (optional). Keep 'internal' if you want to hide from other assemblies. ----

        /// <summary>Localizes an <see cref="XtraForm"/>.</summary>
        internal static void InitializeLanguage(XtraForm form) => InitializeLanguage((Control)form);

        /// <summary>Localizes a <see cref="WaitForm"/>.</summary>
        internal static void InitializeLanguage(WaitForm form) => InitializeLanguage((Control)form);

        /// <summary>Localizes a <see cref="UserControl"/>.</summary>
        internal static void InitializeLanguage(UserControl control) => InitializeLanguage((Control)control);
    }
}
