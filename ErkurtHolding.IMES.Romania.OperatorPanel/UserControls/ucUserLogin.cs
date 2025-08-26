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
    public partial class ucUserLogin : DevExpress.XtraEditors.XtraUserControl
    {
        public CompanyPerson companyPerson { get; set; }
        public UserModel userModel { get; set; }
        PersonLoginType personLoginType;


        public ucUserLogin(PersonLoginType _personLoginType)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            personLoginType = _personLoginType;

            switch (personLoginType)
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
            if (ToolsMdiManager.frmOperatorActive.shopOrderProduction != null && ToolsMdiManager.frmOperatorActive.Users.Count > 0)
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
            if ((ContainerSelectUserControl)ToolsMdiManager.frmOperatorActive.container.Tag == ContainerSelectUserControl.UserLogin || (ContainerSelectUserControl)ToolsMdiManager.frmOperatorActive.container.Tag == ContainerSelectUserControl.UserLogOut)
            {
                try
                {
                    var personID = txtCode.Text;
                    if (StaticValues.branch.ERPConnectionCode.StartsWith("R"))
                        personID = "R" + personID;
                    CompanyPerson companyPerson;
                    UserModel userModel = null;
                    if (StaticValues.OperatorRfIdLogin == "TRUE")
                    {
                        companyPerson = CompanyPersonManager.Current.GetCompanyPersonRfIdNo(StaticValues.company.Id, personID);
                    }
                    else
                        companyPerson = CompanyPersonManager.Current.GetCompanyPerson(StaticValues.company.Id, personID);
                    if (companyPerson == null)
                    {
                        HandleError(MessageTextHelper.GetMessageText("000", "997", "Kullanıcı Id Hatalı", "Message"));
                        return;
                    }
                    else
                    {
                        bool personLogin = ToolsMdiManager.frmOperatorActive.Users.Any(x => x.IfsEmplooyeId == companyPerson.employeeId);
                        bool flag = false;
                        var laborClases = LaborClassManager.Current.GetLaborClasses(StaticValues.branch.Id, companyPerson.Id);

                        if (laborClases == null || laborClases.Count == 0)
                        {
                            HandleError(MessageTextHelper.GetMessageText("000", "998", "Personele ait herhangi bir yetki bulunamadı.\r\nLütfen sistem yöneticinize başvurun", "Message"));
                            return;
                        }
                        else if (ToolsMdiManager.frmOperatorActive.Users.Count > 0 && ToolsMdiManager.frmOperatorActive.Users.Max(u => u.Role) > 2)
                        {
                            flag = true;
                        }
                        else
                        {
                            foreach (var labor in laborClases)
                            {
                                if (labor.laborClassPersonelLevel > 2)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }

                        userModel = new UserModel();
                        userModel.CompanyPersonId = companyPerson.Id;
                        userModel.IfsEmplooyeId = companyPerson.employeeId;
                        userModel.rfIdNo = companyPerson.rfIdNo;
                        userModel.Name = companyPerson.name;
                        userModel.TwoFactorActive = companyPerson.TwoFactorActive;
                        userModel.Email = companyPerson.Email;
                        userModel.LaborClass = laborClases.Max(x => x.laborClassNo);
                        userModel.Role = laborClases.Max(x => x.laborClassPersonelLevel);
                        userModel.StartDate = DateTime.Now;
                        userModel.FinishDate = DateTime.MinValue;

                        if (flag)
                        {
                            switch (personLoginType)
                            {
                                case PersonLoginType.ShopOrderPersonelLogin:
                                    if (personLogin)
                                    {
                                        HandleError(MessageTextHelper.GetMessageText("000", "999", "Kullanıcı zaten giriş yapmış durumda", "Message"));
                                        return;
                                    }

                                    UserLoginHelper.StartShopOrderOperationUserLogin(ToolsMdiManager.frmOperatorActive.machine, userModel);
                                    ResourceLoginPanelVisibleControl();

                                    break;
                                case PersonLoginType.ShopOrderPersonelLogout:

                                    if (!personLogin)
                                    {
                                        HandleError(MessageTextHelper.GetMessageText("000", "800", "Personel giriş yapmadığı için çıkartılamaz", "Message"));
                                        return;
                                    }
                                    if (ToolsMdiManager.frmOperatorActive.shopOrderProduction != null && ToolsMdiManager.frmOperatorActive.Users.Count == 0)
                                    {
                                        HandleError(MessageTextHelper.GetMessageText("000", "801", "Personel listesi boş", "Message"));
                                        return;
                                    }
                                    if (ToolsMdiManager.frmOperatorActive.shopOrderProduction != null && ToolsMdiManager.frmOperatorActive.Users.Count == 1)
                                    {
                                        HandleError(MessageTextHelper.GetMessageText("000", "802", "Son Personelin cıkışı yapılamaz", "Message"));
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
                                    if (ToolsMdiManager.frmOperatorActive.shopOrderProduction != null && ToolsMdiManager.frmOperatorActive.Users.Count > 0)
                                    {
                                        ToolsMdiManager.frmOperatorActive.container.Visible = false;
                                    }
                                    break;
                                case PersonLoginType.ShopOrderSuperVisorLogin:

                                    UserLoginHelper.StartShopOrderOperationUserLogin(ToolsMdiManager.frmOperatorActive.machine, userModel);
                                    ResourceLoginPanelVisibleControl();

                                    break;
                            }



                        }
                        else
                        {
                            HandleError(MessageTextHelper.GetMessageText("000", "803", "İş Emri başlatabilmek için gerekli olan yetkiye sahip değilsiniz", "Message"));
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    txtCode.Text = string.Empty;
                    ToolsMessageBox.Error(this, ex);
                }
            }
        }

        private void ResourceLoginPanelVisibleControl()
        {
            var machine = ToolsMdiManager.frmOperatorActive.machine;
            foreach (var item in ToolsMdiManager.frmOperators)
            {
                if (machine.Id == item.machine.Id)
                {
                    if (item.container.Tag == null || (ContainerSelectUserControl)item.container.Tag == (ContainerSelectUserControl)ToolsMdiManager.frmOperatorActive.container.Tag)
                        item.container.Visible = false;
                }
            }
        }

        private void ucUserLogin_Load(object sender, EventArgs e)
        {
            txtCode.Focus();
        }

        private void txtCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)  // Eğer Enter tuşuna basıldıysa
            {
                btnOk_Click(sender, e);  // Butonun tıklanma metodunu çağır
            }
        }
        private void HandleError(string message)
        {
            txtCode.Text = string.Empty;
            ToolsMessageBox.Information(this, message);
            txtCode.Focus();
        }
    }

}
