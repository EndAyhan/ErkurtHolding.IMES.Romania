using DevExpress.XtraEditors;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.KafkaManager;
using ErkurtHolding.IMES.Business.ReportManagers;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.KafkaFlow;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
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

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucInterruptionCause : DevExpress.XtraEditors.XtraUserControl
    {
        private int Level;
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

        private Machine machine { get; set; }
        public OpInterruptionCause opInterruptionCause { get; set; }

        private string alan2 = "";
        private List<OpInterruptionCause> interruptionCauses = new List<OpInterruptionCause>();
        private MemoEdit memoEdit = new MemoEdit();
        private LabelControl label = new LabelControl();
        private LabelControl lbl;

        public ucInterruptionCause()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            interruptionCauses = OpInterruptionCauseManager.Current.GetOpInterruptionCause(StaticValues.panel.BranchId, ToolsMdiManager.frmOperatorActive.machine.Id);

            level = 0;
        }


        private void ucInterruptionCause_Load(object sender, EventArgs e)
        {
            deStartDate.EditValue = DateTime.Now;
            machine = ToolsMdiManager.frmOperatorActive.machine;

            foreach (var item in ToolsMdiManager.frmOperators)
            {
                if (ToolsMdiManager.frmOperatorActive.shopOrderStatus != ShopOrderStatus.Start)
                {
                    if (item.shopOrderStatus == ShopOrderStatus.Start ||
                        item.interruptionCause != null)
                        continue;
                    if (!item.Users.Any(x => x.IfsEmplooyeId == ToolsMdiManager.frmOperatorActive.interrupstionCouseStartUser.IfsEmplooyeId))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!(item.shopOrderStatus == ShopOrderStatus.Start ||
                        item.interruptionCause != null))
                        continue;
                }
                string eq;
                if (item.machine.Definition == item.resource.resourceName)
                    eq = item.machine.Definition;
                else
                    eq = $"{item.machine.Definition} | {item.resource.resourceName}";
                if (!cmbResources.Properties.Items.Contains(eq))
                    cmbResources.Properties.Items.Add(eq);
                var prm = ToolsMdiManager.frmOperatorActive.interrupstionCouseStartUser.IfsEmplooyeId.CreateParameters("@Employee");
                lblEquipment.Text = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("000", "863", "@Employee'Nolu Sicile Sahip \r\nOperatör Tarafından \r\nSeçilebilecek Ekipmanlar", "Message"), prm);
            }
        }


        private void InitControls()
        {
            flpContainer.Controls.Clear();
            int index = 0;
            switch (level)
            {
                case 0:
                    List<string> groups;
                    if (ToolsMdiManager.frmOperatorActive.shopOrderStatus == ShopOrderStatus.Start)
                        groups = interruptionCauses.Where(x => x.alan2 == "ÇEVRİMDIŞI").GroupBy(x => x.alan2).Select(group => group.Key).ToList();
                    else
                        groups = interruptionCauses.Where(x => x.alan2 != "ÇEVRİMDIŞI").GroupBy(x => x.alan2, (key) => new { category = key }).Select(s => s.Key).ToList();
                    flpDescriptions.Controls.Clear();

                    foreach (var group in groups)
                    {
                        var btn = CreateButton($"btnGroup{index}", group.ToString(), group);
                        btn.Click += BtnGroup_Click;
                        flpContainer.Controls.Add(btn);
                    }
                    btnSave.Enabled = false;
                    break;
                case 1:
                    var result = interruptionCauses.Where(x => x.alan2 == alan2);
                    foreach (var item in result)
                    {
                        var btn = CreateButton($"btnList{index}", item.description, item);
                        btn.Click += BtnListItem_Click;
                        flpContainer.Controls.Add(btn);
                    }
                    btnSave.Enabled = false;
                    break;
                case 2:
                    CreateMemoEdit();
                    flpContainer.Controls.Add(memoEdit);
                    btnSave.Enabled = true;
                    break;
            }
        }

        #region Button Click Event   
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ToolsMdiManager.frmOperatorActive.container.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (level == 0)
                return;

            if (cmbResources.Text.Length == 0)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "968", "Kaydetmeden önce Ekipman seçmelisiniz", "Message"));
                return;
            }

            ToolsMdiManager.frmOperatorActive.container.Visible = false;

            var resourceDefinitions = cmbResources.Text.Split(',');
            KafkaService kafkaService = null;
            foreach (var resourceDefinition in resourceDefinitions)
            {
                var split = resourceDefinition.Split('|');
                string machineDefinition = split[0].Trim();
                string resourceName = machineDefinition;
                if (split.Length > 1)
                    resourceName = split[1].Trim();

                var frmOperator = ToolsMdiManager.frmOperators.Single(x => x.machine.Definition == machineDefinition && x.resource.resourceName == resourceName);

                InterruptionCause interruption = new InterruptionCause()
                {
                    ShiftID = StaticValues.shift.Id,
                    CompanyPersonID = ToolsMdiManager.frmOperatorActive.interrupstionCouseStartUser.CompanyPersonId,
                    WorkCenterID = frmOperator.machine.Id,
                    CouseID = opInterruptionCause.Id,
                    Description = memoEdit.Text,
                    InterruptionStartDate = Convert.ToDateTime(deStartDate.EditValue),
                    State = true,
                    OnlineState = ToolsMdiManager.frmOperatorActive.shopOrderStatus != ShopOrderStatus.Start
                };

                try
                {
                    InterruptionCauseManager.Current.Insert(interruption);

                    InterruptionCauseHelper.CreateInterruption(frmOperator, interruption, opInterruptionCause);


                    //KAFKA
                    #region Kafka
                    //string topicName = $"{StaticValues.branch.ERPConnectionCode}_{opInterruptionCause.alan2}_INTERRUPTION_TOPIC";
                    var topic = TopicManager.Current.GetTopicByBranchNameAndCauseGroupName(StaticValues.branch.ERPConnectionCode, opInterruptionCause.alan2.Replace(' ', '_'), TopicDefinitions.interruption);
                    //var topic = TopicManager.Current.GetByTopicName(topicName);
                    if (topic != null)
                    {
                        var topcUsers = TopicUserManager.Current.getByTopicId(topic.Id);

                        if (topcUsers != null && topcUsers.Count > 0)
                        {
                            InterruptionCauseMessageModel message = new InterruptionCauseMessageModel();
                            message.TopicName = topic.TopicName;
                            message.InterruptionCauseID = interruption.Id;
                            message.ResourceName = resourceName;
                            message.WorkCenterName = machineDefinition;
                            message.CreatedAt = DateTime.Now;
                            message.Description = memoEdit.Text;
                            message.MainCause = opInterruptionCause.alan2;
                            message.CauseDetail = opInterruptionCause.description;
                            message.CauseID = opInterruptionCause.Id;
                            message.BranchName = StaticValues.branch.ERPConnectionCode;
                            message.BranchID = StaticValues.branch.Id;
                            message.GroupName = StaticValues.groups.Single(x => x.Id == frmOperator.machine.GroupId).Name;
                            message.GroupID = frmOperator.machine.GroupId;
                            message.ProductionDepartmentName = StaticValues.productionDepartments.Single(x => x.Id == frmOperator.machine.ProductionDepartmentId).Name;
                            message.CreatedBy = ToolsMdiManager.frmOperatorActive.interrupstionCouseStartUser.Name;

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
                                    emailTemplate = emailTemplate.Replace("{Type}", "Üretim Duruş")
                                    .Replace("{BranchName}", StaticValues.branch.Name)
                                    .Replace("{Yetkili}", companyPerson.name)
                                    .Replace("{CreatedBy}", message.CreatedBy)
                                    .Replace("{WorkCenterName}", message.WorkCenterName)
                                    .Replace("{MainCause}", message.MainCause)
                                    .Replace("{CauseDetail}", message.CauseDetail)
                                    .Replace("{ResourceName}", message.ResourceName)
                                    .Replace("{Date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    EMailModel mailModel = new EMailModel();
                                    mailModel.UserEmails = user.Email;
                                    mailModel.subject = $"Üretim Duruşu Bildirimi : {message.WorkCenterName} ";
                                    mailModel.Body = emailTemplate;
                                    mailModel.OriginalBody = mailModel.Body;
                                    mailModel.SendRefreshTime = DateTime.Now;
                                    mailModel.SendTime = DateTime.Now;
                                    mailModel.FaultOrInterruptionId = interruption.Id;
                                    mailModel.EmailSendDelayMinutes = user.EmailSendDelayMinutes;
                                    EmailQueeService.quee.Enqueue(mailModel);
                                    EmailQueeService.start = true;
                                }
                            }
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    InterruptionCauseManager.Current.UpdateInterruptionCauseEndDate(interruption.Id);
                    frmOperator.interruptionCauseOptions = InterruptionCauseOptions.End;
                    ResourceStatusManager.Current.StopResourceStatus(frmOperator.resource.Id, frmOperator.shopOrderStatus == ShopOrderStatus.Start ? Guid.Empty : frmOperator.shopOrderProduction.Id, (int)ResourceWorkingStatus.Interruption);
                    throw ex;
                }

                frmOperator.RefreshInterruptionCauseGrid();
                frmOperator.interruptionCauseOptions = InterruptionCauseOptions.Waiting;

                List<vw_ShopOrderGridModel> shopOrderGridModels;
                shopOrderGridModels = frmOperator.vw_ShopOrderGridModels;

                if (frmOperator.shopOrderStatus != ShopOrderStatus.Start && opInterruptionCause.countPLC == "FALSE")
                {
                    StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, true);
                    if (frmOperator.panelDetail.OPCNodeIdInterruption != null && frmOperator.panelDetail.OPCNodeIdInterruption != "")
                        StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdInterruption, true);//jet resourcelerde duruş bilgisi gönderme
                }
                if (frmOperator.shopOrderStatus == ShopOrderStatus.Start && opInterruptionCause.countPLC == "TRUE")
                    StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, false);
            }

            ToolsMdiManager.frmOperatorActive.FocusAndSelectTxtBarcode();
        }
        #endregion

        #region Interruption Button Events
        private void BtnGroup_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            alan2 = btn.Text;
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
        }

        private void LabelGroup_Click(object sender, EventArgs e)
        {
            // lbl.Text = "";
            level = 0;
        }

        /// <summary>
        /// Groups altında listelenen OpInterruptionCauses'lardan biri seçildiğinde tetikleniyor..
        /// </summary>
        private void BtnListItem_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            opInterruptionCause = btn.Tag as OpInterruptionCause;
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
            lbl.Text = opInterruptionCause.description + "\\";
            lbl.Click += LabelList_Click;
            flpDescriptions.Controls.Add(lbl);
        }

        private void LabelList_Click(object sender, EventArgs e)
        {
            lbl.Text = "";
            level = 1;
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
            btn.Size = new System.Drawing.Size(245, 60);
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

        private void cmbResources_EditValueChanged(object sender, EventArgs e)
        {
            lblEquipment.Text = "";
            var resourceNames = cmbResources.Text.Split(',');
            foreach (var resourceName in resourceNames)
            {
                lblEquipment.Text += $"{resourceName.Trim()}\r\n";
            }
        }

        private void deStartDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }

}
