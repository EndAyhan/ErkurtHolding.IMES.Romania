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
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucShiftBook : DevExpress.XtraEditors.XtraUserControl
    {
        private UserModel userModel;
        private Shift selectedShift = null;
        private DateTime selectedDate = DateTime.MinValue;
        public ucShiftBook(UserModel userModel)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            this.userModel = userModel;

            deWorkingDate.DateTime = DateTime.Now;
            selectedDate = new DateTime(deWorkingDate.DateTime.Year, deWorkingDate.DateTime.Month, deWorkingDate.DateTime.Day, 0, 0, 0);

            var shifts = ShiftManager.Current.GetShifts(StaticValues.branch.Id).OrderBy(x => x.Description).ToList();
            glueShift.Properties.DataSource = shifts;
            glueShift.Properties.ValueMember = "Id";
            glueShift.Properties.DisplayMember = "Description";

            DateTime dt = DateTime.Now;
            TimeSpan tsNow = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            for (int i = 0; i < shifts.Count; i++)
            {
                TimeSpan tsStart = new TimeSpan(shifts[i].StartDate.Hour, shifts[i].StartDate.Minute, shifts[i].StartDate.Second);
                TimeSpan tsEnd = new TimeSpan(shifts[i].EndDate.Hour, shifts[i].EndDate.Minute, shifts[i].EndDate.Second);

                if (tsStart <= tsNow && tsNow <= tsEnd)
                {
                    glueShift.EditValue = shifts[i].Id;
                    selectedShift = shifts[i];
                    break;
                }
            }

            this.repositoryItemButtonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(ButtonClick);
        }

        private void ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            var title = MessageTextHelper.GetMessageText("000", "871", "Üretim Miktarı", "Message");
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

        #region BUTTON CLICK
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var view = (ColumnView)gcShiftBook.FocusedView;
            view.CloseEditor();
            view.UpdateCurrentRow();
            for (int i = 0; i < gvShiftBook.RowCount; i++)
            {
                var row = (ShiftBook)gvShiftBook.GetRow(i);
                row.UpdatedAt = DateTime.Now;
                row.OvermanPersonID = userModel.CompanyPersonId;
                ShiftBookManager.Current.Update(row);
            }
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }
        #endregion

        #region EVENTS
        private void deWorkingDate_SelectionChanged(object sender, EventArgs e)
        {
            DateTime dt = new DateTime(deWorkingDate.DateTime.Year, deWorkingDate.DateTime.Month, deWorkingDate.DateTime.Day, 0, 0, 0);
            if (dt.ToString() != selectedDate.ToString())
            {
                selectedDate = dt;
                refreshDataGrid();
            }
        }

        private void glueShift_EditValueChanged(object sender, EventArgs e)
        {
            if (selectedShift == null || selectedShift.Id != ((Shift)glueShift.GetSelectedDataRow()).Id)
            {
                selectedShift = (Shift)glueShift.GetSelectedDataRow();
                refreshDataGrid();
            }
        }
        #endregion

        private void refreshDataGrid()
        {
            if (selectedDate == DateTime.MinValue || selectedShift == null)
                return;
            Guid[] guid = { ToolsMdiManager.frmOperatorActive.resource.Id };
            DateTime start = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedShift.StartDate.Hour, selectedShift.StartDate.Minute, selectedShift.StartDate.Second);
            DateTime end = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedShift.EndDate.Hour, selectedShift.EndDate.Minute, selectedShift.EndDate.Second);
            var productionDetails = ProductionReportQueryManager.Current.GetProductions(true, false, false, start, end, guid);
            var entries = ShiftBookManager.Current.GetShiftBooks(ToolsMdiManager.frmOperatorActive.resource.Id, start, end);
            if (productionDetails == null)
                productionDetails = new List<Entity.QueryModel.ProductionReport>();
            if (entries == null)
                entries = new List<ShiftBook>();
            foreach (var entry in entries)
            {
                entry.TotalAmount = 0;
                entry.PLCCounter = 0;
                ShiftBookManager.Current.Update(entry);
            }

            foreach (var details in productionDetails)
            {
                if (entries.Any(x => x.PartID == details.ProductID))
                {
                    foreach (var entry in entries.Where(x => x.PartID == details.ProductID))
                    {
                        entry.TotalAmount = details.total_quantity;
                        entry.PLCCounter = details.plc_counter;
                        entry.UpdatedAt = DateTime.Now;
                        ShiftBookManager.Current.Update(entry);
                    }
                }
                else
                {
                    var entry = new ShiftBook();
                    entry.ResourceID = details.ResourceID;
                    entry.StartDate = start;
                    entry.EndDate = end;
                    entry.ShiftID = selectedShift.Id;
                    entry.PartID = details.ProductID;
                    entry.PartNo = details.part_no;
                    entry.PartDescription = details.part_description;
                    entry.TotalAmount = details.total_quantity;
                    entry.PLCCounter = details.plc_counter;
                    entry.OvermanPersonID = userModel.CompanyPersonId;
                    var r = ShiftBookManager.Current.Insert(entry).ListData[0];
                    entries.Add(r);
                }
            }

            gcShiftBook.DataSource = entries;
            gvShiftBook.BestFitColumns();
        }
    }

}
