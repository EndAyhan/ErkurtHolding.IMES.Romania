using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraTabbedMdi;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Main;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Tools
{
    /// <summary>
    /// Central MDI utilities:
    /// - Tracks open operator forms and the active one
    /// - Wires DevExpress <see cref="XtraTabbedMdiManager"/> events
    /// - Exposes helpers to open/activate forms and toggle tab images (interruption icon)
    /// </summary>
    public static class ToolsMdiManager
    {
        // ---------------- State ----------------

        /// <summary>All open operator forms (PANEL work centers).</summary>
        public static List<FrmOperator> frmOperators { get; set; } = new List<FrmOperator>();

        /// <summary>The currently active operator form (when WorkCenterType == PANEL).</summary>
        public static FrmOperator frmOperatorActive { get; set; }

        /// <summary>The currently active "Fill Box" form (when WorkCenterType == FILLBOX).</summary>
        public static FrmFillHandlingUnit frmFillBoxActive { get; set; }

        private static XtraTabbedMdiManager _mdiManager;

        /// <summary>
        /// The shared DevExpress MDI manager. When set, event handlers are (re)attached safely.
        /// </summary>
        public static XtraTabbedMdiManager mdiManager
        {
            get { return _mdiManager; }
            set
            {
                // Detach old handlers (avoid double subscription)
                if (_mdiManager != null)
                {
                    _mdiManager.PageAdded -= _mdiManager_PageAdded;
                    _mdiManager.PageRemoved -= _mdiManager_PageRemoved;
                    _mdiManager.SelectedPageChanged -= _mdiManager_SelectedPageChanged;
                    _mdiManager.SetNextMdiChild -= _mdiManager_SetNextMdiChild;
                }

                _mdiManager = value;

                // Attach new handlers
                if (_mdiManager != null)
                {
                    _mdiManager.PageAdded += _mdiManager_PageAdded;
                    _mdiManager.PageRemoved += _mdiManager_PageRemoved;
                    _mdiManager.SelectedPageChanged += _mdiManager_SelectedPageChanged;
                    _mdiManager.SetNextMdiChild += _mdiManager_SetNextMdiChild;
                }
            }
        }

        private static RibbonControl _ribbonControl;

        /// <summary>The shared ribbon control used to show/hide groups/items per active form state.</summary>
        public static RibbonControl ribbonControl
        {
            get { return _ribbonControl; }
            set { _ribbonControl = value; }
        }

        // ---------------- Constants ----------------

        private const string WorkCenter_Panel = "PANEL";
        private const string WorkCenter_FillBox = "FILLBOX";

        // ---------------- Event handlers ----------------

        /// <summary>
        /// Optional DevExpress hook to control next/previous tab navigation. Not implemented by default.
        /// Throwing is intentional to discover accidental wiring in production; replace with logic as needed.
        /// </summary>
        private static void _mdiManager_SetNextMdiChild(object sender, SetNextMdiChildEventArgs e)
        {
            throw new NotImplementedException("Implement custom next/previous child navigation if needed.");
        }

        /// <summary>
        /// Tracks active page changes and updates:
        /// - Active operator/fillbox reference
        /// - Ribbon button visibility (via <see cref="ToolsRibbonManager"/>)
        /// - Brings forward related form when working on the same machine (not in Start state)
        /// </summary>
        private static void _mdiManager_SelectedPageChanged(object sender, EventArgs e)
        {
            var mgr = _mdiManager;
            if (mgr == null || mgr.MdiParent == null) return;

            var activeChild = mgr.MdiParent.ActiveMdiChild;
            var workCenter = StaticValues.WorkCenterType ?? string.Empty;

            switch (workCenter)
            {
                default:
                case WorkCenter_Panel:
                    {
                        var op = activeChild as FrmOperator;
                        if (op == null) { frmOperatorActive = null; return; }

                        var previous = frmOperatorActive;
                        frmOperatorActive = op;

                        // Toggle QR section on ribbon (TRUE check, case-insensitive)
                        var qrEnabled = string.Equals(StaticValues.QrCodeGenerator, "TRUE", StringComparison.OrdinalIgnoreCase);
                        ToolsRibbonManager.RibbonButtonQrCode(qrEnabled);

                        // Update ribbon buttons for the active operator form
                        ToolsRibbonManager.RibbonButtonStatus(
                            op.shopOrderStatus,
                            op.interruptionCauseOptions,
                            op.machineDownTimeButtonStatus,
                            op.prMaintenanceButtonStatus);

                        // If another form for the same machine is open and not in Start, bring it forward
                        foreach (var item in frmOperators)
                        {
                            if (item == null || item.machine == null || op.machine == null || item.panelDetail == null || op.panelDetail == null)
                                continue;

                            if (op.panelDetail.Id != item.panelDetail.Id &&
                                op.machine.Id == item.machine.Id &&
                                item.machineDownTimeButtonStatus != MachineDownTimeButtonStatus.Start)
                            {
                                ActivatedForm(item);
                                break;
                            }
                        }

                        break;
                    }

                case WorkCenter_FillBox:
                    {
                        var fb = activeChild as FrmFillHandlingUnit;
                        if (fb != null) frmFillBoxActive = fb;
                        break;
                    }
            }
        }

        /// <summary>
        /// Keeps <see cref="frmOperators"/> in sync when MDI pages are closed.
        /// </summary>
        private static void _mdiManager_PageRemoved(object sender, MdiTabPageEventArgs e)
        {
            if (e == null || e.Page == null) return;

            var workCenter = StaticValues.WorkCenterType ?? string.Empty;

            switch (workCenter)
            {
                default:
                case WorkCenter_Panel:
                    var op = e.Page.MdiChild as FrmOperator;
                    if (op != null) frmOperators.Remove(op);
                    break;

                case WorkCenter_FillBox:
                    // No-op for now
                    break;
            }
        }

        /// <summary>
        /// Keeps <see cref="frmOperators"/> in sync when new MDI pages are opened.
        /// </summary>
        private static void _mdiManager_PageAdded(object sender, MdiTabPageEventArgs e)
        {
            if (e == null || e.Page == null) return;

            var workCenter = StaticValues.WorkCenterType ?? string.Empty;

            switch (workCenter)
            {
                default:
                case WorkCenter_Panel:
                    var op = e.Page.MdiChild as FrmOperator;
                    if (op != null) frmOperators.Add(op);
                    break;

                case WorkCenter_FillBox:
                    // No-op for now
                    break;
            }
        }

        // ---------------- Form helpers ----------------

        /// <summary>
        /// Opens a form inside the current MDI parent, maximized, without control box.
        /// Safe no-op if MDI context is not ready.
        /// </summary>
        public static void FormOpen(Form frm)
        {
            var mgr = _mdiManager;
            if (frm == null || mgr == null || mgr.MdiParent == null) return;

            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.MdiParent = mgr.MdiParent;
            frm.Show();
        }

        /// <summary>
        /// Activates the tab that has the same <see cref="Form.Tag"/> as <paramref name="frm"/>.
        /// Useful when multiple panels for the same machine exist and you want to focus the peer tab.
        /// </summary>
        public static void ActivatedForm(Form frm)
        {
            var mgr = _mdiManager;
            if (frm == null || mgr == null || mgr.MdiParent == null) return;

            var children = mgr.MdiParent.MdiChildren;
            if (children == null || children.Length == 0) return;

            var targetTag = frm.Tag;
            if (targetTag == null) return;

            foreach (var child in children)
            {
                if (child == null || child.Tag == null) continue;

                if (Equals(targetTag.ToString(), child.Tag.ToString()))
                {
                    var page = mgr.Pages[child];
                    if (page != null && page.MdiChild != null)
                        page.MdiChild.Activate();
                }
            }
        }

        /// <summary>
        /// Sets/clears the "interruption" image on the tab that matches <paramref name="frm"/>'s tag.
        /// </summary>
        /// <param name="frm">Reference form whose <see cref="Form.Tag"/> is used to find the tab.</param>
        /// <param name="visibility">If <c>true</c> shows the stop icon; otherwise clears it.</param>
        public static void InterrutionImage(Form frm, bool visibility)
        {
            var mgr = _mdiManager;
            if (frm == null || mgr == null || mgr.MdiParent == null) return;

            var children = mgr.MdiParent.MdiChildren;
            if (children == null || children.Length == 0) return;

            var targetTag = frm.Tag;
            if (targetTag == null) return;

            foreach (var child in children)
            {
                if (child == null || child.Tag == null) continue;

                if (Equals(targetTag.ToString(), child.Tag.ToString()))
                {
                    var page = mgr.Pages[child];
                    if (page != null)
                    {
                        page.ImageOptions.Image = visibility
                            ? ErkurtHolding.IMES.Romania.OperatorPanel.Properties.Resources.stop
                            : null;
                    }
                }
            }
        }
    }
}
