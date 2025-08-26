using DevExpress.Utils.Win;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Automatically resizes the popup of a <see cref="GridLookUpEdit"/> to a “best fit”
    /// width/height based on visible columns and rows.
    /// - Falls back gracefully when there are no rows/columns.
    /// - Caps size to the current screen’s working area to avoid off-screen popups.
    /// - Restores the original popup size when the editor closes.
    /// </summary>
    public sealed class GridLookUpEditBestPopupFormSizeHelper : IDisposable
    {
        private readonly GridLookUpEdit _edit;
        private readonly Size _initialPopupSize;
        private Size _lastBestSize;
        private bool _attached;

        /// <summary>
        /// Binds the helper to the editor and sets an initial popup size.
        /// </summary>
        /// <param name="edit">Target <see cref="GridLookUpEdit"/>.</param>
        /// <param name="initialHeight">Initial popup height in pixels.</param>
        /// <param name="width">Initial popup width in pixels.</param>
        public GridLookUpEditBestPopupFormSizeHelper(GridLookUpEdit edit, int initialHeight, int width)
        {
            _edit = edit ?? throw new ArgumentNullException(nameof(edit));
            _initialPopupSize = new Size(Math.Max(100, width), Math.Max(50, initialHeight)); // sensible floor

            // Initial sizing + default behavior
            _edit.Properties.PopupFormSize = _initialPopupSize;
            _edit.Properties.View.OptionsView.ColumnAutoWidth = false;

            Attach();
        }

        /// <summary>Attach popup/close handlers. Safe to call once.</summary>
        private void Attach()
        {
            if (_attached) return;
            _edit.Popup += OnPopup;
            _edit.Closed += OnClosed;
            _attached = true;
        }

        /// <summary>Detach popup/close handlers. Call before disposing the editor.</summary>
        public void Detach()
        {
            if (!_attached) return;
            _edit.Popup -= OnPopup;
            _edit.Closed -= OnClosed;
            _attached = false;
        }

        /// <inheritdoc />
        public void Dispose() => Detach();

        private void OnClosed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            // Keep the best size for the next open if we have one; otherwise restore initial
            _edit.Properties.PopupFormSize = _lastBestSize.Width > 0 ? _lastBestSize : _initialPopupSize;
        }

        private void OnPopup(object sender, EventArgs e)
        {
            try
            {
                var view = _edit.Properties.View as GridView;
                if (view == null) return;

                // Fit columns to content (since ColumnAutoWidth=false)
                view.BestFitColumns();

                var vinfo = view.GetViewInfo() as GridViewInfo;
                if (vinfo == null) { ApplySize(_initialPopupSize); return; }

                // --- Compute best width ---
                // “Right” of the last column (guard: LastColumnInfo may be null)
                int bestWidth = vinfo.ColumnsInfo?.LastColumnInfo?.Bounds.Right ?? 0;
                if (bestWidth <= 0) bestWidth = _initialPopupSize.Width; // fallback

                // Tiny padding to avoid clipping
                bestWidth += 2;

                // --- Compute best height ---
                int lastVisibleRow = vinfo.RowsInfo?.GetLastVisibleRowIndex() ?? -1;
                int contentBottom = (lastVisibleRow >= 0)
                    ? vinfo.RowsInfo.GetInfoByHandle(lastVisibleRow).Bounds.Bottom
                    : 0;

                // popup host window (for chrome padding) 
                var popupWin = (sender as IPopupControl)?.PopupWindow as Control;
                int chromeHeight = 0;
                if (popupWin != null && view.GridControl != null)
                {
                    // height difference between host and grid is headers/footers/scrollbars
                    chromeHeight = Math.Max(0, popupWin.Height - view.GridControl.Height);
                }

                int bestHeight = contentBottom + chromeHeight;
                if (bestHeight <= 0) bestHeight = _initialPopupSize.Height; // fallback

                // --- Cap to screen working area ---
                var screen = Screen.FromControl(_edit);
                var work = screen?.WorkingArea ?? Screen.PrimaryScreen.WorkingArea;
                // Add a small margin so it doesn’t touch edges
                var maxWidth = Math.Max(200, work.Width - 16);
                var maxHeight = Math.Max(100, work.Height - 16);

                bestWidth = Math.Min(bestWidth, maxWidth);
                bestHeight = Math.Min(bestHeight, maxHeight);

                // Apply
                var size = new Size(bestWidth, bestHeight);
                _lastBestSize = size;
                ApplySize(size, popupWin);
            }
            catch
            {
                // If anything goes wrong, don’t block the popup; use initial size.
                ApplySize(_initialPopupSize);
            }
        }

        private void ApplySize(Size size, Control popupWindow = null)
        {
            if (popupWindow != null)
                popupWindow.Size = size;

            // Keep the editor’s property in sync for next open
            _edit.Properties.PopupFormSize = size;
        }
    }
}
