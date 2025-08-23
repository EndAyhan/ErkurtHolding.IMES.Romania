using System;
using System.Linq;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Handles operator user sign-in/out against the currently running shop order/production
    /// for a given work center (machine) and updates <see cref="UserProduction"/> records.
    /// </summary>
    public static class UserLoginHelper
    {
        /// <summary>
        /// Signs the <paramref name="userModel"/> into the active shop order on the specified <paramref name="activeWorkCenter"/>.
        /// If the user is already signed in for that work center/session, this is a no-op.
        /// Creates a <see cref="UserProduction"/> row with an open-ended <c>EndDate</c>.
        /// </summary>
        /// <param name="activeWorkCenter">Target machine/work center.</param>
        /// <param name="userModel">User to sign in.</param>
        public static void StartShopOrderOperationUserLogin(Machine activeWorkCenter, UserModel userModel)
        {
            if (activeWorkCenter == null || userModel == null)
                return;

            var shiftId = StaticValues.shift != null ? StaticValues.shift.Id : Guid.Empty;
            var now = DateTime.Now;

            foreach (var frmOperator in ToolsMdiManager.frmOperators)
            {
                // Target only the requested work center
                if (frmOperator == null || frmOperator.machine == null || frmOperator.machine.Id != activeWorkCenter.Id)
                    continue;

                // Require an active operation & header (mirrors your original intentions)
                if (!frmOperator.shopOrderOperations.HasEntries() || frmOperator.shopOrderProduction == null || frmOperator.resource == null)
                    continue;

                // Already signed in for this form/work center?
                var already = frmOperator.Users != null &&
                              frmOperator.Users.Any(x => x.CompanyPersonId == userModel.CompanyPersonId);
                if (already)
                    continue;

                // Add to in-memory list
                frmOperator.Users.Add(userModel);

                // Persist a new UserProduction row (open-ended session)
                var up = new UserProduction
                {
                    CompanyPersonId = userModel.CompanyPersonId,
                    ResourceID = frmOperator.resource.Id,
                    WorkCenterID = frmOperator.machine.Id,
                    ShopOrderProductionID = frmOperator.shopOrderProduction.Id,
                    StartDate = now,
                    ShiftId = shiftId,
                    EndDate = DateTime.MaxValue
                };

                UserProductionManager.Current.Insert(up);

                // Cache the generated id back on the user for later finish
                userModel.UserProductionId = up.Id;
            }
        }

        /// <summary>
        /// Signs the <paramref name="userModel"/> out from the active shop order on the specified <paramref name="activeWorkCenter"/>.
        /// If the user is not currently signed in, this is a no-op.
        /// Closes the corresponding <see cref="UserProduction"/> by updating its finish date.
        /// </summary>
        /// <param name="activeWorkCenter">Target machine/work center.</param>
        /// <param name="userModel">User to sign out.</param>
        public static void StartShopOrderOperationFinishLogin(Machine activeWorkCenter, UserModel userModel)
        {
            if (activeWorkCenter == null || userModel == null)
                return;

            foreach (var frmOperator in ToolsMdiManager.frmOperators)
            {
                if (frmOperator == null || frmOperator.machine == null || frmOperator.machine.Id != activeWorkCenter.Id)
                    continue;

                if (!frmOperator.shopOrderOperations.HasEntries() || frmOperator.Users == null)
                    continue;

                // Find the user in the form's user list by CompanyPersonId (avoid reference mismatches)
                var existing = frmOperator.Users.FirstOrDefault(x => x.CompanyPersonId == userModel.CompanyPersonId);
                if (existing == null)
                    continue;

                // Remove from in-memory list
                frmOperator.Users.Remove(existing);

                // Close the persisted session if we know the row id
                if (userModel.UserProductionId != Guid.Empty)
                {
                    UserProductionManager.Current.UpdateFinishDate(userModel.UserProductionId);
                }
            }
        }
    }
}
