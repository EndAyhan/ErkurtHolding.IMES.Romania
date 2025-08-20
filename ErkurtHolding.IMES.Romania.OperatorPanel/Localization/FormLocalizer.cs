using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    public static class FormLocalizer
    {
        /// <summary>
        /// Localizes a control tree (Form, XtraUserControl, etc.). 
        /// Scope examples: "forms.frmoperator", "views.uc_dashboard".
        /// If scope is null, uses the root control name (e.g., "forms.{name}").
        /// </summary>
        public static void Localize(Control root, IText t, string scope = null)
        {
            if (root == null || t == null) return;

            // default scope: "forms.{rootname}" for Forms, otherwise "views.{rootname}"
            if (string.IsNullOrWhiteSpace(scope))
            {
                var prefix = (root is Form) ? "forms." : "views.";
                scope = prefix + (root.Name ?? "root");
            }

            var baseKey = scope.ToLowerInvariant();

            // Root text/title (Forms have a visible title; user controls usually don't, but keeping it for consistency)
            if (root is Form f)
                f.Text = t[baseKey + ".title"];

            // Walk all children
            foreach (Control c in root.Controls)
                LocalizeControlRecursive(c, t, baseKey);

            // Also pick up RibbonControls directly on the root (if not in Controls tree)
            var ribbons = root.Controls.OfType<RibbonControl>().ToArray();
            foreach (var ribbon in ribbons)
                LocalizeRibbon(ribbon, t, baseKey);
        }

        // ------------ recursion over child controls ------------
        private static void LocalizeControlRecursive(Control ctrl, IText t, string baseKey)
        {
            if (ctrl == null) return;

            // 1) Explicit Tag key wins if Tag is string
            var explicitKey = ctrl.Tag as string;
            if (!string.IsNullOrWhiteSpace(explicitKey))
                SetControlText(ctrl, t[explicitKey]);

            // 2) Conventional key by control name (if no explicit tag)
            if (string.IsNullOrWhiteSpace(explicitKey) && !string.IsNullOrEmpty(ctrl.Name))
            {
                var key = baseKey + "." + ctrl.Name.ToLowerInvariant();
                SetControlText(ctrl, t[key]);
                // Optional tooltip by ".tooltip"
                SetControlToolTip(ctrl, t[key + ".tooltip"]);
            }

            // Type‑specific handling
            if (ctrl is RibbonControl ribbon)
                LocalizeRibbon(ribbon, t, baseKey);

            if (ctrl is XtraTabControl tabs)
                LocalizeTabs(tabs, t, baseKey);

            if (ctrl is GridControl grid)
                LocalizeGrid(grid, t, baseKey);

            if (ctrl is LayoutControl layout)
                LocalizeLayout(layout, t, baseKey);

            // Recurse
            foreach (Control child in ctrl.Controls)
                LocalizeControlRecursive(child, t, baseKey);
        }

        // ------------ DevExpress: Ribbon ------------
        private static void LocalizeRibbon(RibbonControl ribbon, IText t, string baseKey)
        {
            if (ribbon == null) return;

            foreach (RibbonPage page in ribbon.Pages)
            {
                var pageKey = ResolveKey(page.Tag, baseKey + ".ribbon." + (page.Name ?? "page").ToLowerInvariant());
                page.Text = t[pageKey];

                foreach (RibbonPageGroup group in page.Groups)
                {
                    var groupKey = ResolveKey(group.Tag, baseKey + ".ribbon." + (group.Name ?? "group").ToLowerInvariant());
                    group.Text = t[groupKey];

                    foreach (BarItemLink link in group.ItemLinks)
                    {
                        var item = link?.Item;
                        if (item == null) continue;

                        var itemKey = ResolveKey(item.Tag, baseKey + ".ribbon." + (item.Name ?? "item").ToLowerInvariant());
                        item.Caption = t[itemKey];
                        item.Description = t[itemKey + ".description"];
                        item.Hint = t[itemKey + ".hint"];
                    }
                }
            }
        }

        // ------------ DevExpress: Tabs ------------
        private static void LocalizeTabs(XtraTabControl tabs, IText t, string baseKey)
        {
            foreach (XtraTabPage tab in tabs.TabPages)
            {
                var key = ResolveKey(tab.Tag, baseKey + ".tabs." + (tab.Name ?? "tab").ToLowerInvariant());
                tab.Text = t[key];
                // If you use SuperToolTip, handle it via explicit Tag-based keys
            }
        }

        // ------------ DevExpress: Grid / GridView ------------
        private static void LocalizeGrid(GridControl grid, IText t, string baseKey)
        {
            var gv = grid.MainView as GridView;
            if (gv != null) LocalizeGridView(gv, t, baseKey);

            foreach (BaseView view in grid.ViewCollection)
            {
                var gridView = view as GridView;
                if (gridView != null) LocalizeGridView(gridView, t, baseKey);
            }
        }

        private static void LocalizeGridView(GridView view, IText t, string baseKey)
        {
            var viewName = (view.Name ?? "gridview").ToLowerInvariant();

            foreach (var colObj in view.Columns)
            {
                var col = colObj as DevExpress.XtraGrid.Columns.GridColumn;
                if (col == null) continue;

                var explicitKey = col.Tag as string;
                if (!string.IsNullOrWhiteSpace(explicitKey))
                {
                    col.Caption = t[explicitKey];
                    continue;
                }

                var id = !string.IsNullOrEmpty(col.FieldName) ? col.FieldName : col.Name;
                if (string.IsNullOrEmpty(id)) continue;

                var key = baseKey + ".grid." + viewName + "." + id.ToLowerInvariant();
                col.Caption = t[key];
                col.ToolTip = t[key + ".tooltip"];
            }
        }

        // ------------ DevExpress: LayoutControl ------------
        private static void LocalizeLayout(LayoutControl layout, IText t, string baseKey)
        {
            foreach (BaseLayoutItem item in layout.Items)
                LocalizeLayoutItem(item, t, baseKey);
        }

        private static void LocalizeLayoutItem(BaseLayoutItem item, IText t, string baseKey)
        {
            if (item == null) return;

            // Groups
            var group = item as LayoutControlGroup;
            if (group != null)
            {
                var key = ResolveKey(group.Tag, baseKey + ".layout." + (group.Name ?? "group").ToLowerInvariant());
                group.Text = t[key];

                foreach (BaseLayoutItem child in group.Items)
                    LocalizeLayoutItem(child, t, baseKey);
                return;
            }

            // Items (labels for editors)
            var lcItem = item as LayoutControlItem;
            if (lcItem != null)
            {
                var key = ResolveKey(lcItem.Tag, baseKey + ".layout." + (lcItem.Name ?? "item").ToLowerInvariant());
                lcItem.Text = t[key];

                // The embedded control itself can also be localized recursively
                if (lcItem.Control != null)
                    LocalizeControlRecursive(lcItem.Control, t, baseKey);
                return;
            }

            // Other item types (EmptySpaceItem, Separator, etc.) usually have no text
        }

        // ------------ helpers ------------
        private static string ResolveKey(object tag, string fallback)
        {
            var s = tag as string;
            return !string.IsNullOrWhiteSpace(s) ? s : fallback;
        }

        private static void SetControlText(Control ctrl, string value)
        {
            if (ctrl == null || string.IsNullOrEmpty(value)) return;

            // DevExpress editors
            var xLabel = ctrl as LabelControl;
            if (xLabel != null) { xLabel.Text = value; return; }

            var xButton = ctrl as SimpleButton;
            if (xButton != null) { xButton.Text = value; return; }

            var xCheck = ctrl as CheckEdit;
            if (xCheck != null) { xCheck.Text = value; return; }

            var xGroup = ctrl as GroupControl;
            if (xGroup != null) { xGroup.Text = value; return; }

            // WinForms
            var wLabel = ctrl as Label;
            if (wLabel != null) { wLabel.Text = value; return; }

            var wButton = ctrl as Button;
            if (wButton != null) { wButton.Text = value; return; }

            var wGroup = ctrl as GroupBox;
            if (wGroup != null) { wGroup.Text = value; return; }

            // Fallback
            ctrl.Text = value;
        }

        private static void SetControlToolTip(Control ctrl, string value)
        {
            if (ctrl == null || string.IsNullOrEmpty(value)) return;

            var type = ctrl.GetType();
            var prop = type.GetProperty("ToolTip");
            if (prop != null && prop.CanWrite)
                prop.SetValue(ctrl, value, null);
        }
    }
}
