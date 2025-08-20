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
    public static class ToolsMdiManager
    {
        // --- State ---
        public static List<FrmOperator> frmOperators { get; set; } = new List<FrmOperator>();
        public static FrmOperator frmOperatorActive { get; set; }
        public static FrmFillHandlingUnit frmFillBoxActive { get; set; }

        private static XtraTabbedMdiManager _mdiManager;
        public static XtraTabbedMdiManager mdiManager
        {
            get => _mdiManager;
            set
            {
                // detach old handlers to avoid double-subscribe
                if (_mdiManager != null)
                {
                    _mdiManager.PageAdded -= _mdiManager_PageAdded;
                    _mdiManager.PageRemoved -= _mdiManager_PageRemoved;
                    _mdiManager.SelectedPageChanged -= _mdiManager_SelectedPageChanged;
                    _mdiManager.SetNextMdiChild -= _mdiManager_SetNextMdiChild;
                }

                _mdiManager = value;

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
        public static RibbonControl ribbonControl
        {
            get => _ribbonControl;
            set => _ribbonControl = value;
        }

        // --- Constants (avoid magic strings) ---
        private const string WorkCenter_Panel = "PANEL";
        private const string WorkCenter_FillBox = "FILLBOX";

        // --- Event handlers ---
        private static void _mdiManager_SetNextMdiChild(object sender, SetNextMdiChildEventArgs e)
        {
            // Implement custom navigation if needed.
            throw new NotImplementedException();
        }

        private static void _mdiManager_SelectedPageChanged(object sender, EventArgs e)
        {
            var mgr = _mdiManager;
            if (mgr == null || mgr.MdiParent == null) return;

            var active = mgr.MdiParent.ActiveMdiChild;
            var workCenter = StaticValues.WorkCenterType ?? string.Empty;

            switch (workCenter)
            {
                default:
                case WorkCenter_Panel:
                    if (!(active is FrmOperator op)) { frmOperatorActive = null; return; }

                    var previousActive = frmOperatorActive;
                    frmOperatorActive = op;

                    // robust TRUE check (case-insensitive)
                    var qrEnabled = string.Equals(StaticValues.QrCodeGenerator, "TRUE", StringComparison.OrdinalIgnoreCase);
                    ToolsRibbonManager.RibbonButtonQrCode(qrEnabled);

                    ToolsRibbonManager.RibbonButtonStatus(op.shopOrderStatus, op.interruptionCauseOptions, op.machineDownTimeButtonStatus, op.prMaintenanceButtonStatus);

                    // ensure same machine, different panel, and not in "Start"
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

                case WorkCenter_FillBox:
                    if (active is FrmFillHandlingUnit fb)
                        frmFillBoxActive = fb;
                    break;
            }
        }

        private static void _mdiManager_PageRemoved(object sender, MdiTabPageEventArgs e)
        {
            if (e == null || e.Page == null) return;

            var workCenter = StaticValues.WorkCenterType ?? string.Empty;
            switch (workCenter)
            {
                default:
                case WorkCenter_Panel:
                    if (e.Page.MdiChild is FrmOperator op)
                        frmOperators.Remove(op);
                    break;

                case WorkCenter_FillBox:
                    // No-op currently
                    break;
            }
        }

        private static void _mdiManager_PageAdded(object sender, MdiTabPageEventArgs e)
        {
            if (e == null || e.Page == null) return;

            var workCenter = StaticValues.WorkCenterType ?? string.Empty;
            switch (workCenter)
            {
                default:
                case WorkCenter_Panel:
                    if (e.Page.MdiChild is FrmOperator op)
                        frmOperators.Add(op);
                    break;

                case WorkCenter_FillBox:
                    // No-op currently
                    break;
            }
        }

        // --- Form helpers ---
        public static void FormOpen(Form frm)
        {
            var mgr = _mdiManager;
            if (frm == null || mgr == null || mgr.MdiParent == null) return;

            frm.WindowState = FormWindowState.Maximized;
            frm.ControlBox = false;
            frm.MdiParent = mgr.MdiParent;
            frm.Show();
        }

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
                if (child?.Tag != null && Equals(targetTag.ToString(), child.Tag.ToString()))
                {
                    var page = mgr.Pages[child];
                    if (page?.MdiChild != null)
                        page.MdiChild.Activate();
                }
            }
        }

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
                if (child?.Tag != null && Equals(targetTag.ToString(), child.Tag.ToString()))
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
