using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Centralized authorization helper for creating <see cref="UserModel"/> logins under different policies.
    /// - Operator vs. generic user flows
    /// - Optional personel-level enforcement (> 2)
    /// - Group-guarded logins (e.g., Maintenance/Quality/Manual/Transfer)
    /// 
    /// All thrown exceptions are localized via <see cref="MessageTextHelper"/>.
    /// </summary>
    public static class AuthorizeHelper
    {
        /// <summary>Authorize maintenance login (group-bound).</summary>
        public static UserModel IsAuthorizedMaintenanceLogin(string employeeId) =>
            UserGroupLogin(employeeId, checkPersonelLevel: false, groupName: "BAKIM");

        /// <summary>Authorize quality login (group-bound).</summary>
        public static UserModel IsAuthorizedQualityLogin(string employeeId) =>
            UserGroupLogin(employeeId, checkPersonelLevel: false, groupName: "KALITE");

        /// <summary>Authorize manual-checked login (group-bound).</summary>
        public static UserModel IsAuthorizedManuelCheckedLogin(string employeeId) =>
            UserGroupLogin(employeeId, checkPersonelLevel: false, groupName: "MANUEL");

        /// <summary>Authorize inventory transfer login (group-bound).</summary>
        public static UserModel IsAuthorizedInventoryTransferCheckedLogin(string employeeId) =>
            UserGroupLogin(employeeId, checkPersonelLevel: false, groupName: "TRANSFER");

        /// <summary>
        /// Generic user login (no personel-level enforcement).
        /// </summary>
        public static UserModel IsAuthorizedLogin(string employeeId) =>
            UserLogin(employeeId, checkPersonelLevel: false);

        /// <summary>
        /// Generic user login (requires personel level &gt; 2).
        /// </summary>
        public static UserModel IsAuthorizedSupervisorLogin(string employeeId) =>
            UserLogin(employeeId, checkPersonelLevel: true);

        /// <summary>
        /// Operator login (no personel-level enforcement).
        /// Work-center authorization is checked.
        /// </summary>
        public static UserModel IsAuthorizedOperatorLogin(string employeeId) =>
            OperatorLogin(employeeId, checkPersonelLevel: false);

        /// <summary>
        /// Operator login (requires personel level &gt; 2).
        /// Work-center authorization is checked.
        /// </summary>
        public static UserModel IsAuthorizedOperatorSupervisorLogin(string employeeId) =>
            OperatorLogin(employeeId, checkPersonelLevel: true);

        // -------------------- Implementation --------------------

        /// <summary>
        /// Operator flow: checks work-center authorization (labor class match) and optionally personel level &gt; 2.
        /// Returns null when not authorized (mirroring your original flow).
        /// Throws localized exceptions for lookup errors etc.
        /// </summary>
        private static UserModel OperatorLogin(string employeeId, bool checkPersonelLevel)
        {
            try
            {
                var person = GetCompanyPersonByLoginMode(employeeId);
                if (person == null)
                    throw new Exception(MessageTextHelper.GetMessageText("000", "645", "Sicil numarası yanlış.", "Message"));

                // Labor classes of person at branch
                var laborClasses = LaborClassManager.Current.GetLaborClasses(StaticValues.branch.Id, person.Id);
                if (laborClasses == null || laborClasses.Count == 0)
                    return null;

                // Work-center authorization for active machine
                var activeMachineId = ToolsMdiManager.frmOperatorActive?.machine?.Id ?? Guid.Empty;
                var workcenterAuthorize = WorkCenterAuthorizeManager.Current.GetWorkCenterAuthorize(StaticValues.branch.Id, activeMachineId);
                if (workcenterAuthorize == null || workcenterAuthorize.Count == 0)
                    return null;

                // Eligible labor classes per personel level rule
                var eligible = checkPersonelLevel
                    ? laborClasses.Where(x => x.laborClassPersonelLevel > 2).ToList()
                    : laborClasses;

                if (eligible.Count == 0)
                    return null;

                // Find first intersection between user's eligible classes and work-center required classes
                var allowedLaborClassNos = new HashSet<string>(workcenterAuthorize.Select(a => a.LaborClassNo));
                var match = eligible.FirstOrDefault(x => allowedLaborClassNos.Contains(x.laborClassNo));
                if (match == null)
                    return null;

                // Build user model
                var model = BuildUserModel(person, employeeId, match);
                model.StartDate = DateTime.Now;
                model.FinishDate = DateTime.MinValue; // preserve your original
                return model;
            }
            catch (Exception ex)
            {
                // Localize & wrap
                var prm = ex.Message.CreateParameters("@ErrorMessage");
                var msg = MessageTextHelper.ReplaceParameters(
                    MessageTextHelper.GetMessageText("000", "644", "Giriş Başarısız. \r\nAdminInfo: @ErrorMessage", "Message"), prm);
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// Generic user flow: no work-center authorization; optional personel level &gt; 2.
        /// Returns null when not authorized, throws localized exceptions on lookup errors.
        /// </summary>
        private static UserModel UserLogin(string employeeId, bool checkPersonelLevel)
        {
            try
            {
                var person = GetCompanyPersonByLoginMode(employeeId);
                if (person == null)
                    throw new Exception(MessageTextHelper.GetMessageText("000", "645", "Sicil numarası yanlış.", "Message"));

                var laborClasses = LaborClassManager.Current.GetLaborClasses(StaticValues.branch.Id, person.Id);
                if (laborClasses == null || laborClasses.Count == 0)
                    return null;

                var match = checkPersonelLevel
                    ? laborClasses.FirstOrDefault(x => x.laborClassPersonelLevel > 2)
                    : laborClasses.FirstOrDefault();

                if (match == null)
                    return null;

                var model = BuildUserModel(person, employeeId, match);
                model.StartDate = DateTime.Now;
                return model;
            }
            catch (Exception ex)
            {
                var prm = ex.Message.CreateParameters("@ErrorMessage");
                var msg = MessageTextHelper.ReplaceParameters(
                    MessageTextHelper.GetMessageText("000", "644", "Giriş Başarısız. \r\nAdminInfo: @ErrorMessage", "Message"), prm);
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// Group-guarded login: user must have a labor class matching "{ERPConnectionCode}{groupName}".
        /// Optional personel level &gt; 2.
        /// </summary>
        private static UserModel UserGroupLogin(string employeeId, bool checkPersonelLevel, string groupName)
        {
            try
            {
                var person = GetCompanyPersonByLoginMode(employeeId);
                if (person == null)
                    throw new Exception(MessageTextHelper.GetMessageText("000", "645", "Sicil numarası yanlış.", "Message"));

                var laborClasses = LaborClassManager.Current.GetLaborClasses(StaticValues.branch.Id, person.Id);
                if (laborClasses == null || laborClasses.Count == 0)
                    return null;

                var requiredClass = (StaticValues.branch?.ERPConnectionCode ?? string.Empty) + groupName;
                var match = laborClasses.FirstOrDefault(x =>
                    x.laborClassNo == requiredClass &&
                    (!checkPersonelLevel || x.laborClassPersonelLevel > 2));

                if (match == null)
                    return null;

                var model = BuildUserModel(person, employeeId, match);
                model.StartDate = DateTime.Now;
                return model;
            }
            catch (Exception ex)
            {
                var prm = ex.Message.CreateParameters("@ErrorMessage");
                var msg = MessageTextHelper.ReplaceParameters(
                    MessageTextHelper.GetMessageText("000", "644", "Giriş Başarısız. \r\nAdminInfo: @ErrorMessage", "Message"), prm);
                throw new Exception(msg);
            }
        }

        // -------------------- Small helpers --------------------

        /// <summary>
        /// Resolves the <see cref="CompanyPerson"/> either by RFID or by employeeId, 
        /// based on <see cref="StaticValues.OperatorRfIdLogin"/> ("TRUE" is the only enabling value).
        /// </summary>
        private static CompanyPerson GetCompanyPersonByLoginMode(string employeeId)
        {
            var useRfid = string.Equals(StaticValues.OperatorRfIdLogin, "TRUE", StringComparison.OrdinalIgnoreCase);

            if (useRfid)
                return CompanyPersonManager.Current.GetCompanyPersonRfIdNo(StaticValues.company.Id, employeeId);

            return CompanyPersonManager.Current.GetCompanyPerson(StaticValues.company.Id, employeeId);
        }

        /// <summary>
        /// Builds a <see cref="UserModel"/> from a person and a matched labor class.
        /// Keeps existing behavior: when RFID mode is active, <see cref="UserModel.IfsEmplooyeId"/>
        /// is overwritten by <see cref="CompanyPerson.employeeId"/>.
        /// </summary>
        private static UserModel BuildUserModel(CompanyPerson person, string inputEmployeeId, dynamic laborClass)
        {
            var model = new UserModel
            {
                CompanyPersonId = person.Id,
                IfsEmplooyeId = inputEmployeeId,
                LaborClass = laborClass.laborClassNo,
                Name = person.name,
                Role = laborClass.laborClassPersonelLevel,
                rfIdNo = person.rfIdNo,
                Email = person.Email,
                TwoFactorActive = person.TwoFactorActive
            };

            // Preserve original nuance: in RFID mode, override IfsEmplooyeId with the person.employeeId
            var useRfid = string.Equals(StaticValues.OperatorRfIdLogin, "TRUE", StringComparison.OrdinalIgnoreCase);
            if (useRfid && !string.IsNullOrEmpty(person.employeeId))
            {
                model.IfsEmplooyeId = person.employeeId;
            }

            return model;
        }
    }
}
