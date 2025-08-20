using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class UserLoginHelper
    {
        public static void StartShopOrderOperationUserLogin(Machine activeWorkCenter, UserModel userModel)
        {
            DateTime dt = DateTime.Now;
            foreach (var frmOperator in ToolsMdiManager.frmOperators)
            {
                if (frmOperator.machine.Id == activeWorkCenter.Id)
                {
                    if (frmOperator.shopOrderOperations.HasEntries())
                    {
                        if (!frmOperator.Users.Any(x => x.CompanyPersonId == userModel.CompanyPersonId))
                        {
                            frmOperator.Users.Add(userModel);
                            UserProduction userProduction = new UserProduction()
                            {
                                CompanyPersonId = userModel.CompanyPersonId,
                                ResourceID = frmOperator.resource.Id,
                                WorkCenterID = frmOperator.machine.Id,
                                ShopOrderProductionID = frmOperator.shopOrderProduction.Id,
                                StartDate = dt,
                                ShiftId = StaticValues.shift.Id,
                                EndDate = DateTime.MaxValue
                            };
                            UserProductionManager.Current.Insert(userProduction);
                            userModel.UserProductionId = userProduction.Id;
                        }
                    }
                }
            }
        }

        public static void StartShopOrderOperationFinishLogin(Machine activeWorkCenter, UserModel userModel)
        {
            foreach (var frmOperator in ToolsMdiManager.frmOperators)
            {
                if (frmOperator.machine.Id == activeWorkCenter.Id)
                {
                    if (frmOperator.shopOrderOperations.HasEntries())
                    {
                        if (frmOperator.Users.Any(x => x.CompanyPersonId == userModel.CompanyPersonId))
                        {
                            frmOperator.Users.Remove(userModel);
                            UserProductionManager.Current.UpdateFinishDate(userModel.UserProductionId);
                        }
                    }
                }
            }
        }
    }
}
