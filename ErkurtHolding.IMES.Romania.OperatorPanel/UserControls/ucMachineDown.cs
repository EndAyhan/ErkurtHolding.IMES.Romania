using DevExpress.XtraEditors;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.KafkaManager;
using ErkurtHolding.IMES.Business.ReportManagers;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Reports;
using ErkurtHolding.IMES.KafkaFlow;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucMachineDown : DevExpress.XtraEditors.XtraUserControl
    {

        List<MtchngSptmDscvrs> mtchngSptmDscvrses = new List<MtchngSptmDscvrs>();
        List<Machine> machines = new List<Machine>();
        MtchngSptmDscvrs mtchngSptmDscvrs;
        private Machine machine { get; set; }
        public DialogResult dialogResult;
        MemoEdit memoEdit = new MemoEdit();
        LabelControl label = new LabelControl();
        LabelControl lbl;
        private int Level;
        string selectedMachineDownDescription = "";

        public int level
        {
            get
            {

                return Level;
            }

            set
            {
                Level = value;
                InitControls();

            }
        }
        public ucMachineDown()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            mtchngSptmDscvrses = MtchngSptmDscvrsManager.Current.GetMtchngSptmDscvrsesByBranchID(StaticValues.branch.Id);
            if (mtchngSptmDscvrses == null)
            {
                mtchngSptmDscvrses = new List<MtchngSptmDscvrs>();
            }
            level = 0;
        }

        private void ucMachineDown_Load(object sender, EventArgs e)
        {
            deStartDate.EditValue = DateTime.Now;
            machine = ToolsMdiManager.frmOperatorActive.machine;

            foreach (var item in ToolsMdiManager.frmOperators)
            {
                if (item.machine.Id == machine.Id)
                    cmbResources.Properties.Items.Add(item.resource.resourceName);
            }
        }

        private void InitControls()
        {
            flpContainer.Controls.Clear();
            int index = 0;
            switch (level)
            {
                case 0:
                    //var groups = machineDowntimeCauses.GroupBy(x => x.workTypeDescription, (key) => new { category = key }).Select(s => s.Key).ToList();
                    var groups = from p in mtchngSptmDscvrses
                                 group p by new { p.workTypeDesc, p.workTypeId }
                                 into g
                                 select new { workTypeDesc = g.Key.workTypeDesc, workTypeId = g.Key.workTypeDesc };

                    flpDescriptions.Controls.Clear();

                    foreach (var group in groups)
                    {
                        var btn = CreateButton($"btnGroup{index}", group.workTypeDesc, group.workTypeId);
                        btn.Click += BtnGroup_Click;
                        flpContainer.Controls.Add(btn);
                        index++;
                    }
                    break;
                case 1:
                    CreateMemoEdit();
                    flpContainer.Controls.Add(memoEdit);
                    break;
            }
        }

        #region Button Click Event   

        private void BtnGroup_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            selectedMachineDownDescription = btn.Tag.ToString();
            level = 1;
            label.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            label.Appearance.Options.UseFont = true;
            label.Appearance.Options.UseForeColor = true;
            label.Cursor = System.Windows.Forms.Cursors.Hand;
            label.Location = new System.Drawing.Point(11, 11);
            label.Name = "labelControl17";
            label.Size = new System.Drawing.Size(113, 21);
            label.TabIndex = 0;
            label.Text = btn.Text + "\\";
            label.Click += LabelGroup_Click;
            flpDescriptions.Controls.Add(label);
            //flpContainer.Controls.Clear();
            //flpContainer.Controls.Add(CreateMemoEdit());
            lblLine.Location = new Point(563, 70);
            mtchngSptmDscvrs = mtchngSptmDscvrses.First(x => x.workTypeDesc == btn.Text);
        }

        private void LabelGroup_Click(object sender, EventArgs e)
        {
            // lbl.Text = "";
            level = 0;
        }

        private void BtnListItem_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            var workTypeId = btn.Tag as MachineDowntimeCause;
            level = 2;
            lbl = new LabelControl();
            lbl.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            lbl.Appearance.Options.UseFont = true;
            lbl.Appearance.Options.UseForeColor = true;
            lbl.Cursor = System.Windows.Forms.Cursors.Hand;
            lbl.Location = new System.Drawing.Point(11, 11);
            lbl.Name = "labelControl18";
            lbl.Size = new System.Drawing.Size(113, 21);
            lbl.TabIndex = 0;
            lbl.Text = mtchngSptmDscvrs.workTypeDesc + "\\";
            lbl.Click += LabelList_Click;
            flpDescriptions.Controls.Add(lbl);

        }

        private void LabelList_Click(object sender, EventArgs e)
        {
            lbl.Text = "";
            level = 1;
        }

        //private void InitControlsIfsIntegration()
        //{
        //    mtchngSptmDscvrses.Clear();
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.machineDownTimeButtonStatus = MachineDownTimeButtonStatus.Start;
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }
        private void btnVirtualKey_Click(object sender, EventArgs e)
        {
            FrmVirtualKeyboard frmVirtualKeyboard = new FrmVirtualKeyboard(memoEdit.Text);
            if (frmVirtualKeyboard.ShowDialog() == DialogResult.OK)
            {
                memoEdit.Text = frmVirtualKeyboard.InputText;
            }

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (level == 0)
                return;
            if (memoEdit.Text.Length <= 5)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "500", "Lütfen tespit edilen arızanın açıklamasını detaylı olarak giriniz!", "Message"));
                return;
            }

            Fault faultActive = new Fault();
            faultActive.CompanyID = StaticValues.company.Id;
            faultActive.BranchID = StaticValues.branch.Id;
            faultActive.ReportedByCompanyPersonID = ToolsMdiManager.frmOperatorActive.machindeDownStartUser.CompanyPersonId;
            faultActive.ErrDescription = $"{selectedMachineDownDescription} - {memoEdit.Text}";
            faultActive.WorkTypeID = mtchngSptmDscvrs.workTypeId;
            faultActive.WorkCenterID = ToolsMdiManager.frmOperatorActive.machine.Id;
            faultActive.ResourceID = ToolsMdiManager.frmOperatorActive.resource.Id;
            faultActive.RegisterDate = DateTime.Now;
            faultActive.ShopOrderProductionId = ToolsMdiManager.frmOperatorActive.shopOrderProduction.Id;
            faultActive = FaultManager.Current.Insert(faultActive).ListData[0];
            ToolsMdiManager.frmOperatorActive.faults.Add(faultActive);


            //KAFKA
            #region Kafka
            var topic = TopicManager.Current.GetTopicByBranchNameAndCauseGroupName(StaticValues.branch.ERPConnectionCode, selectedMachineDownDescription.Replace(' ', '_'), TopicDefinitions.fault);
            if (topic != null)
            {
                var topcUsers = TopicUserManager.Current.getByTopicId(topic.Id);

                if (topcUsers != null && topcUsers.Count > 0)
                {
                    InterruptionCauseMessageModel message = new InterruptionCauseMessageModel();
                    message.TopicName = topic.TopicName;
                    message.InterruptionCauseID = faultActive.Id;
                    message.ResourceName = ToolsMdiManager.frmOperatorActive.resource.resourceName;
                    message.WorkCenterName = ToolsMdiManager.frmOperatorActive.machine.Definition;
                    message.CreatedAt = DateTime.Now;
                    message.Description = memoEdit.Text;
                    message.MainCause = selectedMachineDownDescription;
                    message.CauseDetail = memoEdit.Text;
                    message.CauseID = faultActive.Id;
                    message.BranchName = StaticValues.branch.ERPConnectionCode;
                    message.BranchID = StaticValues.branch.Id;
                    message.GroupName = StaticValues.groups.Single(x => x.Id == ToolsMdiManager.frmOperatorActive.machine.GroupId).Name;
                    message.GroupID = ToolsMdiManager.frmOperatorActive.machine.GroupId;
                    message.ProductionDepartmentName = StaticValues.productionDepartments.Single(x => x.Id == ToolsMdiManager.frmOperatorActive.machine.ProductionDepartmentId).Name;
                    message.CreatedBy = ToolsMdiManager.frmOperatorActive.machindeDownStartUser.Name;

                    KafkaService kafkaService = null;
                    foreach (var user in topcUsers)
                    {
                        var companyPerson = CompanyPersonManager.Current.GetCompanyPersonById(user.UserId);
                        message.UserId = user.UserId;
                        message.SmsSend = user.SmsSend;
                        message.MailSend = user.MailSend;
                        message.MobileSend = user.MobileSend;
                        message.Email = user.Email;
                        message.FcmToken = companyPerson.FcmToken;

                        //var topicName = TopicManager.Current.GetTopicByBranchNameAndCauseGroupName(message.BranchName, message.MainCause).TopicName;
                        var messageJson = JsonConvert.SerializeObject(message);
                        if (user.MobileSend)
                        {
                            if (kafkaService == null)
                                kafkaService = StaticValues.kafkaService;
                            kafkaService.ProduceMessageAsync(messageJson, "interruption-downtime-cause").ContinueWith(task =>
                            {
                                if (task.IsFaulted)
                                {
                                    // Hata yönetimi

                                    // Hata ile ilgili işlemler burada yapılabilir
                                }
                                else
                                {
                                    // Başarı durumu
                                    // İşlem tamamlandığında yapılacaklar
                                }
                            });
                        }
                        //SendCallNotification(interruption.Id);
                        if (user.MailSend && user.Email != null && user.Email.Length > 5)
                        {
                            string emailTemplate = $"";
                            if (!string.IsNullOrWhiteSpace(user.Body) && user.Body.Length > 30)
                            {
                                emailTemplate = $@"{user.Body}";
                            }
                            else
                            {
                                emailTemplate = EmailTemplateHelper.FaultCauseMail();
                            }
                            emailTemplate = emailTemplate.Replace("{Type}", "Arıza")
                            .Replace("{BranchName}", StaticValues.branch.Name)
                            .Replace("{Yetkili}", companyPerson.name)
                            .Replace("{CreatedBy}", message.CreatedBy)
                            .Replace("{WorkCenterName}", message.WorkCenterName)
                            .Replace("{MainCause}", message.MainCause)
                            .Replace("{CauseDetail}", message.Description)
                            .Replace("{ResourceName}", message.ResourceName)
                            .Replace("{Date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                            EMailModel mailModel = new EMailModel();
                            mailModel.UserEmails = user.Email;
                            mailModel.subject = $"Arıza Bildirimi : {message.WorkCenterName} ";
                            mailModel.Body = emailTemplate;
                            mailModel.OriginalBody = mailModel.Body;
                            mailModel.SendRefreshTime = DateTime.Now;
                            mailModel.SendTime = DateTime.Now;
                            mailModel.FaultOrInterruptionId = faultActive.Id;
                            mailModel.EmailSendDelayMinutes = user.EmailSendDelayMinutes;
                            EmailQueeService.quee.Enqueue(mailModel);
                            EmailQueeService.start = true;
                        }
                    }
                }
            }
            #endregion

            ToolsMdiManager.frmOperatorActive.container.Visible = false;
            ToolsMdiManager.frmOperatorActive.machineDownTimeButtonStatus = MachineDownTimeButtonStatus.Waiting;
            StaticValues.opcClient.MachineLock(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdMachineControl, true);
            if (ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != null && ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption != "")
                StaticValues.opcClient.WriteNode(ToolsMdiManager.frmOperatorActive.panelDetail.OPCNodeIdInterruption, true);

            try
            {
                var resourceStatus = new ResourceStatus()
                {
                    MachineId = ToolsMdiManager.frmOperatorActive.resource.Id,
                    ShopOrderProductionId = ToolsMdiManager.frmOperatorActive.shopOrderProduction.Id,
                    Status = (int)ResourceWorkingStatus.Fault
                };
                var response = ResourceStatusManager.Current.Insert(resourceStatus);
            }
            catch { }
        }
        #endregion

        #region LABEL CLICK EVENT
        private void lblGroup_Click(object sender, EventArgs e)
        {
            level = 0;


        }
        #endregion

        #region Create Simple Button flag is true than IFS else IMES fault detail
        private SimpleButton CreateButton(string Name, string Text, object Tag)
        {
            SimpleButton btn = new SimpleButton();
            btn.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btn.Appearance.Options.UseFont = true;
            btn.Location = new System.Drawing.Point(269, 109);
            btn.LookAndFeel.SkinName = "The Asphalt World";
            btn.LookAndFeel.UseDefaultLookAndFeel = false;
            btn.Name = Name;
            btn.Padding = new System.Windows.Forms.Padding(20);
            btn.Size = new System.Drawing.Size(250, 60);
            btn.TabIndex = 4;
            btn.Text = Text;
            btn.Name = Name;
            btn.Tag = Tag;

            return btn;
        }
        #endregion

        #region Create Memo Edit
        private MemoEdit CreateMemoEdit()
        {
            memoEdit.EditValue = "";
            memoEdit.Location = new System.Drawing.Point(193, 276);
            memoEdit.Name = "memoEdit1";
            memoEdit.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            memoEdit.Properties.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            memoEdit.Properties.Appearance.Options.UseFont = true;
            memoEdit.Properties.Appearance.Options.UseForeColor = true;
            memoEdit.Properties.HideSelection = false;
            memoEdit.Properties.NullValuePrompt = "Açıklama...!";
            memoEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            memoEdit.Size = new System.Drawing.Size(567, 240);
            memoEdit.TabIndex = 3;
            return memoEdit;
        }

        #endregion

        private void deStartDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
