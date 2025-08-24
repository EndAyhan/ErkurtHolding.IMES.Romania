using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraWaitForm;
using System;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Localization
{
    /// <summary>
    /// Walks a Windows Forms / DevExpress control tree and localizes visible text
    /// by calling <c>MessageTextHelper.GetMessageText</c>.
    /// 
    /// Key format convention is up to your <c>MessageTextHelper</c>. This visitor only forwards:
    /// - <c>frmTag</c> (form scope)
    /// - <c>control.Tag</c> (explicit key if present)
    /// - current text (fallback)
    /// - <c>control.Name</c> (conventional key component)
    /// </summary>
    internal sealed class GeneralControlVisitor : IControlVisitor
    {
        private readonly object _frmTag;

        /// <summary>
        /// Creates a new visitor for a given form scope tag (e.g., "forms.frmoperator").
        /// </summary>
        public GeneralControlVisitor(object frmTag)
        {
            _frmTag = frmTag;
        }

        /// <summary>
        /// Entry: visit a control and its children (when appropriate).
        /// </summary>
        public void VisitControl(Control control)
        {
            if (control == null) return;

            switch (control)
            {
                // ---------- Top-level containers ----------
                case XtraForm frm when control is XtraForm: // 000
                    frm.Text = T(frm.Tag, frm.Text, frm.Name);
                    VisitChildren(frm);
                    break;

                case WaitForm waitForm when control is WaitForm: // 000
                    waitForm.Text = T(waitForm.Tag, waitForm.Text, waitForm.Name);
                    VisitChildren(waitForm);
                    break;

                case XtraUserControl uc when control is XtraUserControl: // 000
                    // If you really need to gate by Tag equality, do it safely:
                    if (!TagEquals(_frmTag, uc.Tag)) return;
                    uc.Text = T(uc.Tag, uc.Text, uc.Name);
                    VisitChildren(uc);
                    break;

                case PanelControl panel when control is PanelControl:
                    VisitChildren(panel);
                    break;

                case XtraTabControl xtraTabControl when control is XtraTabControl:
                    foreach (XtraTabPage page in xtraTabControl.TabPages)
                        VisitControl(page);
                    break;

                case XtraTabPage xtraTabPage when control is XtraTabPage: // 800
                    xtraTabPage.Text = T(xtraTabPage.Tag, xtraTabPage.Text, xtraTabPage.Name);
                    VisitChildren(xtraTabPage);
                    break;

                // ---------- Ribbon ----------
                case RibbonControl ribbon when control is RibbonControl: // 800
                    LocalizeRibbon(ribbon);
                    break;

                // ---------- DevExpress: Wait/Progress ----------
                case ProgressPanel progressPanel when control is ProgressPanel: // 800
                    progressPanel.Caption = T(progressPanel.Tag, progressPanel.Caption, progressPanel.Name);
                    break;

                // ---------- Groups / Common captions ----------
                case GroupControl group when control is GroupControl: // 100
                    group.Text = T(group.Tag, group.Text, group.Name);
                    VisitChildren(group);
                    break;

                case CheckEdit check when control is CheckEdit: // 100
                    check.Text = T(check.Tag, check.Text, check.Name);
                    break;

                case LabelControl label when control is LabelControl: // 200
                    label.Text = T(label.Tag, label.Text, label.Name);
                    break;

                case SimpleButton button when control is SimpleButton: // 400
                    button.Text = T(button.Tag, button.Text, button.Name);
                    break;

                // ---------- Charts ----------
                case ChartControl chart when control is ChartControl: // 100
                    if (chart.Series != null)
                    {
                        foreach (Series series in chart.Series)
                        {
                            if (series?.Points == null) continue;
                            foreach (SeriesPoint point in series.Points)
                                point.Argument = T(point.Tag, point.Argument, "ccPoint");
                        }
                    }
                    if (chart.Titles != null)
                    {
                        foreach (ChartTitle chartTitle in chart.Titles)
                            chartTitle.Text = T(chartTitle.Tag, chartTitle.Text, "chartTitle");
                    }
                    break;

                // ---------- Layout ----------
                case LayoutControl layout when control is LayoutControl: // 300
                    foreach (var item in layout.Items)
                    {
                        if (item is SimpleLabelItem simpleLabel)
                        {
                            simpleLabel.Text = T(simpleLabel.Tag, simpleLabel.Text, simpleLabel.Name);
                        }
                        else if (item is LayoutControlItem lci && lci.Control != null)
                        {
                            VisitControl(lci.Control);
                        }
                        // Other item types usually don’t carry display text
                    }
                    break;

                // ---------- Bars / Menus (via docking manager) ----------
                case BarDockControl barDock when control is BarDockControl: // 300
                    var mgr = barDock.Manager;
                    var menu = mgr?.MainMenu;
                    var links = menu?.LinksPersistInfo;
                    if (links != null)
                    {
                        foreach (LinkPersistInfo link in links)
                        {
                            LocalizeBarItem(link?.Item);
                        }
                    }
                    break;

                // ---------- Grid family ----------
                case GridControl grid when control is GridControl: // 500
                    var mainView = grid.MainView as GridView;
                    if (mainView != null) LocalizeGridView(mainView);

                    foreach (var view in grid.ViewCollection)
                        if (view is GridView gv) LocalizeGridView(gv);
                    break;

                case GridLookUpEdit glue when control is GridLookUpEdit: // 500
                    var pv = glue.Properties.PopupView as GridView;
                    if (pv != null) LocalizeGridView(pv);
                    break;

                case TreeListLookUpEdit tlue when control is TreeListLookUpEdit: // 500
                    var tree = tlue.Properties.TreeList as TreeList;
                    if (tree?.Columns != null)
                    {
                        foreach (TreeListColumn col in tree.Columns)
                            col.Caption = T(col.Tag, col.Caption, col.Name);
                    }
                    break;

                default:
                    // Recurse into children for any other composite controls
                    VisitChildren(control);
                    break;
            }
        }

        // ---------------- Helpers ----------------

        /// <summary>
        /// Localizes a full Ribbon (pages, groups, bar links).
        /// </summary>
        private void LocalizeRibbon(RibbonControl ribbonControl)
        {
            var pages = ribbonControl?.Pages;
            if (pages == null) return;

            foreach (RibbonPage ribbonPage in pages)
            {
                ribbonPage.Text = T(ribbonPage.Tag, ribbonPage.Text, ribbonPage.Name);

                var groups = ribbonPage.Groups;
                if (groups == null) continue;

                foreach (RibbonPageGroup group in groups)
                {
                    group.Text = T(group.Tag, group.Text, group.Name);

                    var links = group.ItemLinks;
                    if (links == null) continue;

                    foreach (var link in links)
                    {
                        // Common link types
                        if (link is BarButtonItemLink btnLink && btnLink.Item != null)
                        {
                            btnLink.Item.Caption = T(btnLink.Item.Tag, btnLink.Item.Caption, btnLink.Item.Name);
                        }
                        else if (link is BarSubItemLink subLink && subLink.Item is BarSubItem subItem)
                        {
                            LocalizeBarSubItem(subItem);
                        }
                        else if (link is BarEditItemLink editLink && editLink.Item != null)
                        {
                            editLink.Item.Caption = T(editLink.Item.Tag, editLink.Item.Caption, editLink.Item.Name);
                        }
                        else if (link is BarItemLink any && any.Item != null)
                        {
                            // Fallback for other bar items
                            any.Item.Caption = T(any.Item.Tag, any.Item.Caption, any.Item.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Localizes a BarSubItem hierarchy (caption + children recursively).
        /// </summary>
        private void LocalizeBarSubItem(BarSubItem subItem)
        {
            if (subItem == null) return;

            subItem.Caption = T(subItem.Tag, subItem.Caption, subItem.Name);

            if (subItem.LinksPersistInfo != null)
            {
                foreach (LinkPersistInfo link in subItem.LinksPersistInfo)
                {
                    if (link?.Item is BarSubItem nestedSub)
                        LocalizeBarSubItem(nestedSub); // recurse
                    else
                        LocalizeBarItem(link?.Item);
                }
            }
        }

        /// <summary>
        /// Localizes any BarItem (Caption only; extend if you use Hint/Description via MessageTextHelper).
        /// </summary>
        private void LocalizeBarItem(BarItem item)
        {
            if (item == null) return;
            item.Caption = T(item.Tag, item.Caption, item.Name);

            // If you also store hints/descriptions via your helper, do it here:
            // item.Hint = T(item.Tag, item.Hint, item.Name + ".hint");
            // item.Description = T(item.Tag, item.Description, item.Name + ".description");
        }

        /// <summary>
        /// Localizes a GridView (columns only; add more if needed).
        /// </summary>
        private void LocalizeGridView(GridView view)
        {
            if (view?.Columns == null) return;

            foreach (GridColumn column in view.Columns)
                column.Caption = T(column.Tag, column.Caption, column.Name);
        }

        /// <summary>
        /// Visits child controls of a composite control (safe for null lists).
        /// </summary>
        private void VisitChildren(Control control)
        {
            if (control?.Controls == null || control.Controls.Count == 0) return;

            foreach (Control child in control.Controls)
                VisitControl(child);
        }

        /// <summary>
        /// Compares two Tag objects safely as strings.
        /// </summary>
        private static bool TagEquals(object a, object b)
        {
            var sa = a == null ? string.Empty : a.ToString();
            var sb = b == null ? string.Empty : b.ToString();
            return string.Equals(sa, sb, StringComparison.Ordinal);
        }

        /// <summary>
        /// Wrapper around MessageTextHelper. Keeps call site tidy and guards against null keys.
        /// </summary>
        private string T(object controlTag, string currentText, string controlName)
        {
            return MessageTextHelper.GetMessageText(_frmTag, controlTag, currentText, controlName);
        }
    }
}
