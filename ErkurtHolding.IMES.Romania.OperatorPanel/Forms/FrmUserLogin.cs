using DevExpress.XtraEditors;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    public partial class FrmUserLogin : DevExpress.XtraEditors.XtraForm
    {
        private CompanyPerson companyPerson { get; set; }
        public UserModel userModel { get; set; }

        private UserLoginAuthorization authorizationType;

        /// <summary>
        /// User login constructor
        /// </summary>
        /// <param name="roleFlag">roleFlag = false ise yetki 3 ve 4 dü kontrol eder. true ise 3-4 yetkisine sahip personel zaten vardır her operatörü kabul eder </param>
        public FrmUserLogin(bool roleFlag)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            SetLookAndFeel();

            if (roleFlag)
                authorizationType = UserLoginAuthorization.noAuthorization;
            else
                authorizationType = UserLoginAuthorization.personelLevelAuthorization;
        }

        public FrmUserLogin(UserLoginAuthorization authorizationType)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            SetLookAndFeel();

            this.authorizationType = authorizationType;
        }

        private void SetLookAndFeel()
        {
            foreach (var ctrl in layoutControl1.Controls)
            {
                if (ctrl is SimpleButton)
                {
                    (ctrl as SimpleButton).LookAndFeel.SkinName = "The Asphalt World";
                    (ctrl as SimpleButton).LookAndFeel.UseDefaultLookAndFeel = false;
                }
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            txtCode.Text += (sender as SimpleButton).Text;
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            if (txtCode.Text.Length > 0)
                txtCode.Text = txtCode.Text.Remove(txtCode.Text.Length - 1, 1);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                var personID = txtCode.Text;
                if (StaticValues.branch.ERPConnectionCode.StartsWith("R"))
                    personID = "R" + personID;
                switch (authorizationType)
                {
                    case UserLoginAuthorization.noAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedLogin(personID);
                        if (userModel == null)
                        {
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "953", "Yetki tanımlaması yapılmamış", "Message"));
                            return;
                        }
                        break;
                    case UserLoginAuthorization.personelLevelAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedSupervisorLogin(personID);
                        if (userModel == null)
                        {
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "954", "Gerekli olan yetkiye sahip değilsiniz", "Message"));
                            return;
                        }
                        break;
                    case UserLoginAuthorization.startWorkOrderAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedOperatorSupervisorLogin(personID);
                        if (userModel == null)
                        {
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "955", "İş emri başlatabilmek için yeterli yetkiye sahip değilsiniz", "Message"));
                            return;
                        }
                        break;
                    case UserLoginAuthorization.qualityUserAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedQualityLogin(personID);
                        if (userModel == null)
                        {
                            var prm = $"{StaticValues.branch.ERPConnectionCode}KALITE".CreateParameters("@LaborClass");
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "956", "Bu personel @LaborClass üyesi değil", "Message"), prm);
                            return;
                        }
                        break;
                    case UserLoginAuthorization.maintenanceUserAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedMaintenanceLogin(personID);
                        if (userModel == null)
                        {
                            var prm = $"{StaticValues.branch.ERPConnectionCode}BAKIM".CreateParameters("@LaborClass");
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "956", "Bu personel @LaborClass üyesi değil", "Message"), prm);
                            return;
                        }
                        break;
                    case UserLoginAuthorization.manuelLabelAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedManuelCheckedLogin(personID);
                        if (userModel == null)
                        {
                            var prm = $"{StaticValues.branch.ERPConnectionCode}MANUEL".CreateParameters("@LaborClass");
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "956", "Bu personel @LaborClass üyesi değil", "Message"), prm);
                            return;
                        }
                        break;
                    case UserLoginAuthorization.inventoryTransferUserAuthorization:
                        userModel = AuthorizeHelper.IsAuthorizedInventoryTransferCheckedLogin(personID);
                        if (userModel == null)
                        {
                            var prm = $"{StaticValues.branch.ERPConnectionCode}TRANSFER".CreateParameters("@LaborClass");
                            ShowWarningAndClearCode(this, MessageTextHelper.GetMessageText("000", "956", "Bu personel @LaborClass üyesi değil", "Message"), prm);
                            return;
                        }
                        break;
                }

                if (userModel == null)
                    throw new Exception("AdminInfo: userModel null");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                txtCode.Text = string.Empty;
                ToolsMessageBox.Error(this, ex);
            }
        }
        private void ShowWarningAndClearCode(XtraForm form, string message, Dictionary<string, object> parameters = null)
        {
            txtCode.Text = string.Empty;

            if (parameters != null)
                ToolsMessageBox.Warning(form, message, parameters);
            else
                ToolsMessageBox.Warning(form, message);
            txtCode.Focus();
        }
    }

}