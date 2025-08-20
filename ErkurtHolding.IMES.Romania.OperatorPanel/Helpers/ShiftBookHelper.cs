using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.QueryManagers;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class ShiftBookHelper
    {
        public static Shift GetCurrentShift()
        {
            var shifts = ShiftManager.Current.GetShifts(StaticValues.branch.Id)
                                             .OrderBy(x => x.Description)
                                             .ToList();

            DateTime now = DateTime.Now;
            TimeSpan tsNow = new TimeSpan(now.Hour, now.Minute, now.Second);

            foreach (var shift in shifts)
            {
                TimeSpan tsStart = new TimeSpan(shift.StartDate.Hour, shift.StartDate.Minute, shift.StartDate.Second);
                TimeSpan tsEnd = new TimeSpan(shift.EndDate.Hour, shift.EndDate.Minute, shift.EndDate.Second);

                if (tsStart <= tsNow && tsNow <= tsEnd)
                    return shift;
            }

            return null;
        }

        public static void ShiftBookData(UserModel userModel, Guid resourceId, Shift selectedShift)
        {
            DateTime now = DateTime.Now;
            DateTime selectedDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            if (selectedShift == null)
                return;

            Guid[] guid = { resourceId };

            DateTime start = new DateTime(
                selectedDate.Year, selectedDate.Month, selectedDate.Day,
                selectedShift.StartDate.Hour, selectedShift.StartDate.Minute, selectedShift.StartDate.Second);

            DateTime end = new DateTime(
                selectedDate.Year, selectedDate.Month, selectedDate.Day,
                selectedShift.EndDate.Hour, selectedShift.EndDate.Minute, selectedShift.EndDate.Second);

            var productionDetails = ProductionReportQueryManager.Current.GetProductions(true, false, false, start, end, guid);
            var entries = ShiftBookManager.Current.GetShiftBooks(resourceId, start, end);

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
                var match = entries.FirstOrDefault(x => x.PartID == details.ProductID);
                if (match != null)
                {
                    match.TotalAmount = details.total_quantity;
                    match.PLCCounter = details.plc_counter;
                    match.UpdatedAt = DateTime.Now;
                    ShiftBookManager.Current.Update(match);
                }
                else
                {
                    var entry = new ShiftBook
                    {
                        ResourceID = details.ResourceID,
                        StartDate = start,
                        EndDate = end,
                        ShiftID = selectedShift.Id,
                        PartID = details.ProductID,
                        PartNo = details.part_no,
                        PartDescription = details.part_description,
                        TotalAmount = details.total_quantity,
                        PLCCounter = details.plc_counter,
                        OvermanPersonID = userModel.CompanyPersonId,
                    };

                    ShiftBookManager.Current.Insert(entry);
                }
            }
        }
    }
}
