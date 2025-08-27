using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.QueryManagers;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    /// <summary>
    /// Shift book user control for tracking production amounts per shift and resource.
    /// Provides a grid with editable amounts, date and shift selection.
    /// </summary>
    public partial class ucShiftBook : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly UserModel _userModel;
        private Shift _selectedShift;
        private DateTime _selectedDate;

        /// <summary>
        /// Initializes a new instance of <see cref="ucShiftBook"/>.
        /// </summary>
        /// <param name="userModel">The current logged-in user.</param>
        public ucShiftBook(UserModel userModel)
        {
            InitializeComponent();
            LanguageHelper.InitializeLanguage(this);

            _userModel = userModel;
            _selectedDate = DateTime.Today;

            InitializeDateControl();
            InitializeShifts();

            repositoryItemButtonEdit1.ButtonClick += ButtonClick;
        }

        #region Initialization
        /// <summary>
        /// Sets default working date to today.
        /// </summary>
        private void InitializeDateControl()
        {
            deWorkingDate.DateTime = DateTime.Now;
        }

        /// <summary>
        /// Loads shifts for the branch and pre-selects the current shift based on system time.
        /// </summary>
        private void InitializeShifts()
        {
            var shifts = ShiftManager.Current
                .GetShifts(StaticValues.branch.Id)
                .OrderBy(x => x.Description)
                .ToList();

            glueShift.Properties.DataSource = shifts;
            glueShift.Properties.ValueMember = "Id";
            glueShift.Properties.DisplayMember = "Description";

            TimeSpan now = DateTime.Now.TimeOfDay;
            foreach (var shift in shifts)
            {
                if (shift.StartDate.TimeOfDay <= now && now <= shift.EndDate.TimeOfDay)
                {
                    glueShift.EditValue = shift.Id;
                    _selectedShift = shift;
                    break;
                }
            }
        }
        #endregion

        #region Button Clicks
        /// <summary>
        /// Opens numeric keyboard dialog for entering production amount.
        /// </summary>
        private void ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            string title = MessageTextHelper.GetMessageText("000", "871", "Üretim Miktarı", "Message");
            FrmNumericKeyboard frm = new FrmNumericKeyboard(title);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                var row = (ShiftBook)gvShiftBook.GetRow(gvShiftBook.FocusedRowHandle);
                row.OvermanAmount = (double)frm.value;

                var view = (ColumnView)gcShiftBook.FocusedView;
                view.CloseEditor();
                view.UpdateCurrentRow();
            }
        }

        /// <summary>
        /// Closes the shift book panel.
        /// </summary>
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        /// <summary>
        /// Saves current shift book entries.
        /// </summary>
        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var view = (ColumnView)gcShiftBook.FocusedView;
            view.CloseEditor();
            view.UpdateCurrentRow();

            for (int i = 0; i < gvShiftBook.RowCount; i++)
            {
                var row = (ShiftBook)gvShiftBook.GetRow(i);
                row.UpdatedAt = DateTime.Now;
                row.OvermanPersonID = _userModel.CompanyPersonId;
                ShiftBookManager.Current.Update(row);
            }

            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }
        #endregion

        #region Events
        /// <summary>
        /// Triggered when working date is changed.
        /// </summary>
        private void deWorkingDate_SelectionChanged(object sender, EventArgs e)
        {
            DateTime dt = deWorkingDate.DateTime.Date;
            if (dt != _selectedDate)
            {
                _selectedDate = dt;
                RefreshDataGrid();
            }
        }

        /// <summary>
        /// Triggered when shift selection changes.
        /// </summary>
        private void glueShift_EditValueChanged(object sender, EventArgs e)
        {
            Shift newShift = glueShift.GetSelectedDataRow() as Shift;
            if (newShift != null && (_selectedShift == null || _selectedShift.Id != newShift.Id))
            {
                _selectedShift = newShift;
                RefreshDataGrid();
            }
        }
        #endregion

        #region Data Grid
        /// <summary>
        /// Refreshes the data grid with shift book entries and production details.
        /// </summary>
        private void RefreshDataGrid()
        {
            if (_selectedDate == DateTime.MinValue || _selectedShift == null)
                return;

            Guid resourceId = ToolsMdiManager.frmOperatorActive.resource.Id;
            DateTime start = _selectedDate.Date + _selectedShift.StartDate.TimeOfDay;
            DateTime end = _selectedDate.Date + _selectedShift.EndDate.TimeOfDay;

            var productionDetails = ProductionReportQueryManager.Current
                .GetProductions(true, false, false, start, end, new[] { resourceId })
                ?? new List<Entity.QueryModel.ProductionReport>();

            var entries = ShiftBookManager.Current
                .GetShiftBooks(resourceId, start, end)
                ?? new List<ShiftBook>();

            // Reset amounts in existing entries
            foreach (var entry in entries)
            {
                entry.TotalAmount = 0;
                entry.PLCCounter = 0;
                ShiftBookManager.Current.Update(entry);
            }

            // Merge production details
            foreach (var details in productionDetails)
            {
                var matching = entries.Where(x => x.PartID == details.ProductID).ToList();
                if (matching.Any())
                {
                    foreach (var entry in matching)
                    {
                        entry.TotalAmount = details.total_quantity;
                        entry.PLCCounter = details.plc_counter;
                        entry.UpdatedAt = DateTime.Now;
                        ShiftBookManager.Current.Update(entry);
                    }
                }
                else
                {
                    var newEntry = new ShiftBook
                    {
                        ResourceID = details.ResourceID,
                        StartDate = start,
                        EndDate = end,
                        ShiftID = _selectedShift.Id,
                        PartID = details.ProductID,
                        PartNo = details.part_no,
                        PartDescription = details.part_description,
                        TotalAmount = details.total_quantity,
                        PLCCounter = details.plc_counter,
                        OvermanPersonID = _userModel.CompanyPersonId
                    };

                    var inserted = ShiftBookManager.Current.Insert(newEntry).ListData[0];
                    entries.Add(inserted);
                }
            }

            gcShiftBook.DataSource = entries;
            gvShiftBook.BestFitColumns();
        }
        #endregion
    }
}
