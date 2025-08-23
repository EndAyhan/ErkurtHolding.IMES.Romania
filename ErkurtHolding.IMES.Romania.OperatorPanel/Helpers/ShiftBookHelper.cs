using System;
using System.Collections.Generic;
using System.Linq;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.QueryManagers;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Utilities for resolving the current shift and synchronizing shift-book
    /// aggregates with production details for a resource/shift window.
    /// </summary>
    public static class ShiftBookHelper
    {
        /// <summary>
        /// Returns the <see cref="Shift"/> that is active at <c>DateTime.Now</c> for the current branch.
        /// Handles shifts that cross midnight (e.g., 22:00–06:00).
        /// </summary>
        /// <remarks>
        /// If no shifts match, returns <c>null</c>.
        /// Shifts are evaluated using only their time-of-day components.
        /// </remarks>
        public static Shift GetCurrentShift()
        {
            var branchId = StaticValues.branch != null ? StaticValues.branch.Id : Guid.Empty;

            // Prefer ordering by start time-of-day (not by Description)
            var shifts = ShiftManager.Current.GetShifts(branchId)
                                .OrderBy(s => s.StartDate.TimeOfDay)
                                .ToList();

            if (shifts == null || shifts.Count == 0)
                return null;

            var now = DateTime.Now;
            var nowTod = now.TimeOfDay;

            foreach (var s in shifts)
            {
                var start = s.StartDate.TimeOfDay;
                var end = s.EndDate.TimeOfDay;

                if (IsInShift(nowTod, start, end))
                    return s;
            }

            return null;
        }

        /// <summary>
        /// Synchronizes (creates/updates) shift-book rows for the given <paramref name="resourceId"/>
        /// and <paramref name="selectedShift"/> for the current day’s shift window.
        /// </summary>
        /// <param name="userModel">The operator user context (used for Overman/Person id on inserts).</param>
        /// <param name="resourceId">The resource (machine) id to aggregate for.</param>
        /// <param name="selectedShift">The shift to aggregate within; if <c>null</c>, the method returns immediately.</param>
        /// <remarks>
        /// - The method computes the shift’s start/end <see cref="DateTime"/> for “today”, correctly handling
        ///   overnight shifts (where End &lt; Start) by moving the end to the next day.
        /// - Existing entries in the window are reset to 0 before being filled from the production report.
        /// - If a product has no existing entry, it will be inserted.
        /// </remarks>
        public static void ShiftBookData(UserModel userModel, Guid resourceId, Shift selectedShift)
        {
            if (selectedShift == null)
                return;

            var now = DateTime.Now;
            // Build the DateTimes for the shift window based on local "today".
            DateTime start, end;
            GetShiftWindowForDate(selectedShift, now.Date, out start, out end);

            var guids = new[] { resourceId };

            // Fetch production details for the window
            var productionDetails = ProductionReportQueryManager.Current.GetProductions(true, false, false, start, end, guids);

            // Fetch existing shift-book entries
            var entries = ShiftBookManager.Current.GetShiftBooks(resourceId, start, end);

            if (productionDetails == null)
                productionDetails = new List<Entity.QueryModel.ProductionReport>();
            if (entries == null)
                entries = new List<ShiftBook>();

            // Reset existing entries to zero (keeps the rows but clears totals)
            foreach (var entry in entries)
            {
                entry.TotalAmount = 0;
                entry.PLCCounter = 0;
                entry.UpdatedAt = DateTime.Now;
                ShiftBookManager.Current.Update(entry);
            }

            // Upsert from production details
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
                        OvermanPersonID = userModel != null ? userModel.CompanyPersonId : Guid.Empty
                    };

                    ShiftBookManager.Current.Insert(entry);
                }
            }
        }

        // ----------------- helpers -----------------

        /// <summary>
        /// Returns true when the <paramref name="now"/> time-of-day is within a shift defined by
        /// <paramref name="start"/> and <paramref name="end"/> times. Handles overnight ranges.
        /// </summary>
        private static bool IsInShift(TimeSpan now, TimeSpan start, TimeSpan end)
        {
            if (start <= end)
                return now >= start && now <= end;

            // Overnight: e.g., 22:00–06:00 (end wraps past midnight)
            return now >= start || now <= end;
        }

        /// <summary>
        /// Computes concrete start/end datetimes for a shift on a given <paramref name="referenceDate"/>.
        /// Handles overnight shifts by rolling the end to the next day.
        /// </summary>
        private static void GetShiftWindowForDate(Shift shift, DateTime referenceDate, out DateTime windowStart, out DateTime windowEnd)
        {
            var sTod = shift.StartDate.TimeOfDay;
            var eTod = shift.EndDate.TimeOfDay;

            windowStart = referenceDate.Date + sTod;

            // If the end TOD is less than the start TOD, the shift crosses midnight → end is next day.
            if (eTod >= sTod)
                windowEnd = referenceDate.Date + eTod;
            else
                windowEnd = referenceDate.Date.AddDays(1) + eTod;
        }
    }
}
