using DevExpress.XtraEditors;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    /// <summary>
    /// User login control that supports login, logout and supervisor login flows.
    /// Handles validation, user rights, and localized error messages.
    /// </summary>
    public partial class ucUserLogin : XtraUserControl
    {
        /// <summary>
        /// The company person entity of the currently processed user.
        /// </summary>
        public CompanyPerson CompanyPerson { get; set; }

        /// <summary>
        /// The logged-in user model.
        /// </summary>
        public UserModel UserModel { get; set; }

        private readonly PersonLoginType _personLoginType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ucUserLogin"/> class.
        /// </summary>
        /// <param name="personLoginType">Defines which login type (login, logout, supervisor login) this control will perform.</param>
        public ucUserLogin(PersonLoginType personLoginType)
        {
            InitializeComponent();
            LanguageHelper.InitializeLanguage(this);

            _personLoginType = personLoginType;
            InitializeLoginHeaderText();
            ApplyButtonStyles();
        }

        /// <summary>
        /// Initializes the header text of the login group based on login type.
        /// </summary>
        private void InitializeLoginHeaderText()
        {
            switch (_personLoginType)
            {
                case PersonLoginType.ShopOrderPersonelLogin:
                    groupControl1.Text = MessageTextHelper.GetMessageText("000", "872", "Giriş yapmak için personel kodunuzu girin", "Message");
                    break;
                case PersonLoginType.ShopOrderPersonelLogout:
                    groupControl1.Text = MessageTextHelper.GetMessageText("000", "873", "Çıkış yapmak için personel kodunuzu girin", "Message");
                    break;
                default:
                    groupControl1.Text = MessageTextHelper.GetMessageText("000", "874", "Personel kodunuzu girin", "Message");
                    break;
            }
        }

        /// <summary>
        /// Applies consistent style to all buttons in layout control.
        /// </summary>
        private void ApplyButtonStyles()
        {
            foreach (var ctrl in layoutControl1.Controls)
            {
                var btn = ctrl as SimpleButton;
                if (btn != null)
                {
                    btn.LookAndFeel.SkinName = "The Asphalt World";
                    btn.LookAndFeel.UseDefaultLookAndFeel = false;
                }
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            txtCode.Text += (sender as SimpleButton)?.Text;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text))
                txtCode.Text = txtCode.Text.Remove(txtCode.Text.Length - 1, 1);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ToolsMdiManager.frmOperatorActive.shopOrderProduction != null &&
                ToolsMdiManager.frmOperatorActive.Users.Count > 0)
            {
                ToolsMdiManager.frmOperatorActive.container.Visible = false;
            }
            else
            {
                ToolsMessageBox.Information(this, MessageTextHelper.GetMessageText("000", "996", "Mutlaka bir kullanıcının giriş yapması gerekmektedir", "Message"));
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!IsUserLoginContainer()) return;

            try
            {
                string personId = PreparePersonId(txtCode.Text);
                var companyPerson = LoadCompanyPerson(personId);
                if (companyPerson == null)
                {
                    HandleError(MessageTextHelper.GetMessageText("000", "997", "Kullanıcı Id Hatalı", "Message"));
                    return;
                }

                var laborClasses = LaborClassManager.Current.GetLaborClasses(StaticValues.branch.Id, companyPerson.Id);
                if (laborClasses == null || laborClasses.Count == 0)
                {
                    HandleError(MessageTextHelper.GetMessageText("000", "998", "Personele ait herhangi bir yetki bulunamadı.\r\nLütfen sistem yöneticinize başvurun", "Message"));
                    return;
                }

                var userModel = CreateUserModel(companyPerson, laborClasses);
                bool hasPermission = CheckUserPermission(laborClasses);

                if (!hasPermission)
                {
                    HandleError(MessageTextHelper.GetMessageText("000", "803", "İş Emri başlatabilmek için gerekli olan yetkiye sahip değilsiniz", "Message"));
                    return;
                }

                ProcessLoginFlow(companyPerson, userModel);
            }
            catch (Exception ex)
            {
                txtCode.Text = string.Empty;
                ToolsMessageBox.Error(this, ex);
            }
        }

        /// <summary>
        /// Returns true if container is user login/logout type.
        /// </summary>
        private bool IsUserLoginContainer()
        {
            var tag = ToolsMdiManager.frmOperatorActive.container.Tag;
            return (ContainerSelectUserControl)tag == ContainerSelectUserControl.UserLogin ||
                   (ContainerSelectUserControl)tag == ContainerSelectUserControl.UserLogOut;
        }

        private string PreparePersonId(string rawId)
        {
            if (StaticValues.branch.ERPConnectionCode.StartsWith("R"))
                return "R" + rawId;
            return rawId;
        }

        private CompanyPerson LoadCompanyPerson(string personId)
        {
            if (StaticValues.OperatorRfIdLogin == "TRUE")
                return CompanyPersonManager.Current.GetCompanyPersonRfIdNo(StaticValues.company.Id, personId);

            return CompanyPersonManager.Current.GetCompanyPerson(StaticValues.company.Id, personId);
        }

        private UserModel CreateUserModel(CompanyPerson companyPerson, System.Collections.Generic.List<LaborClass> laborClasses)
        {
            return new UserModel
            {
                CompanyPersonId = companyPerson.Id,
                IfsEmplooyeId = companyPerson.employeeId,
                rfIdNo = companyPerson.rfIdNo,
                Name = companyPerson.name,
                TwoFactorActive = companyPerson.TwoFactorActive,
                Email = companyPerson.Email,
                LaborClass = laborClasses.Max(x => x.laborClassNo),
                Role = laborClasses.Max(x => x.laborClassPersonelLevel),
                StartDate = DateTime.Now,
                FinishDate = DateTime.MinValue
            };
        }

        private bool CheckUserPermission(System.Collections.Generic.List<LaborClass> laborClasses)
        {
            if (ToolsMdiManager.frmOperatorActive.Users.Count > 0 &&
                ToolsMdiManager.frmOperatorActive.Users.Max(u => u.Role) > 2)
            {
                return true;
            }

            foreach (var labor in laborClasses)
            {
                if (labor.laborClassPersonelLevel > 2)
                    return true;
            }
            return false;
        }

        private void ProcessLoginFlow(CompanyPerson companyPerson, UserModel userModel)
        {
            bool personAlreadyLogged = ToolsMdiManager.frmOperatorActive.Users
                .Any(x => x.IfsEmplooyeId == companyPerson.employeeId);

            switch (_personLoginType)
            {
                case PersonLoginType.ShopOrderPersonelLogin:
                    if (personAlreadyLogged)
                    {
                        HandleError(MessageTextHelper.GetMessageText("000", "999", "Kullanıcı zaten giriş yapmış durumda", "Message"));
                        return;
                    }
                    UserLoginHelper.StartShopOrderOperationUserLogin(ToolsMdiManager.frmOperatorActive.machine, userModel);
                    ResourceLoginPanelVisibleControl();
                    break;

                case PersonLoginType.ShopOrderPersonelLogout:
                    HandleLogout(companyPerson);
                    break;

                case PersonLoginType.ShopOrderSuperVisorLogin:
                    UserLoginHelper.StartShopOrderOperationUserLogin(ToolsMdiManager.frmOperatorActive.machine, userModel);
                    ResourceLoginPanelVisibleControl();
                    break;
            }
        }

        private void HandleLogout(CompanyPerson companyPerson)
        {
            bool personLogged = ToolsMdiManager.frmOperatorActive.Users
                .Any(x => x.IfsEmplooyeId == companyPerson.employeeId);

            if (!personLogged)
            {
                HandleError(MessageTextHelper.GetMessageText("000", "800", "Personel giriş yapmadığı için çıkartılamaz", "Message"));
                return;
            }
            if (ToolsMdiManager.frmOperatorActive.Users.Count == 0)
            {
                HandleError(MessageTextHelper.GetMessageText("000", "801", "Personel listesi boş", "Message"));
                return;
            }
            if (ToolsMdiManager.frmOperatorActive.Users.Count == 1)
            {
                HandleError(MessageTextHelper.GetMessageText("000", "802", "Son Personelin çıkışı yapılamaz", "Message"));
                return;
            }

            foreach (var item in ToolsMdiManager.frmOperatorActive.Users)
            {
                if (item.IfsEmplooyeId == companyPerson.employeeId)
                {
                    UserLoginHelper.StartShopOrderOperationFinishLogin(ToolsMdiManager.frmOperatorActive.machine, item);
                    break;
                }
            }
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void ResourceLoginPanelVisibleControl()
        {
            var machine = ToolsMdiManager.frmOperatorActive.machine;
            foreach (var item in ToolsMdiManager.frmOperators)
            {
                if (machine.Id == item.machine.Id &&
                    (item.container.Tag == null ||
                     (ContainerSelectUserControl)item.container.Tag == (ContainerSelectUserControl)ToolsMdiManager.frmOperatorActive.container.Tag))
                {
                    item.container.Visible = false;
                }
            }
        }

        private void ucUserLogin_Load(object sender, EventArgs e)
        {
            txtCode.Focus();
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(sender, e);
        }

        /// <summary>
        /// Clears input and shows a localized error message.
        /// </summary>
        private void HandleError(string message)
        {
            txtCode.Text = string.Empty;
            ToolsMessageBox.Information(this, message);
            txtCode.Focus();
        }
    }
}
