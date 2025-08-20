using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.KafkaManager;
using ErkurtHolding.IMES.KafkaFlow;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class NotificationHelper
    {
        public static void SendKafkaAndEmailNotification(
            string topicName,
            string messageDescription,
            string messageMainCause,
            string messageCauseDetail,
            object senderMachine,
            object senderResource,
            Guid workCenterId,
            TopicDefinitions topicDefinition)
        {
            try
            {
                var topic = TopicManager.Current.GetTopicByBranchNameAndCauseGroupName(
                    StaticValues.branch.ERPConnectionCode,
                    topicName.Replace(' ', '_'),
                    topicDefinition);

                if (topic == null)
                    return;

                var topicUsers = TopicUserManager.Current.getByTopicId(topic.Id);
                if (topicUsers == null || topicUsers.Count == 0)
                    return;

                KafkaService kafkaService = StaticValues.kafkaService;

                foreach (var user in topicUsers)
                {
                    var companyPerson = CompanyPersonManager.Current.GetCompanyPersonById(user.UserId);

                    var message = new InterruptionCauseMessageModel
                    {
                        TopicName = topic.TopicName,
                        InterruptionCauseID = workCenterId,
                        ResourceName = ((dynamic)senderResource).resourceName,
                        WorkCenterName = ((dynamic)senderMachine).Definition,
                        CreatedAt = DateTime.Now,
                        Description = messageDescription,
                        MainCause = messageMainCause,
                        CauseDetail = messageCauseDetail,
                        CauseID = ((dynamic)senderMachine).Id,
                        BranchName = StaticValues.branch.ERPConnectionCode,
                        BranchID = StaticValues.branch.Id,
                        GroupName = StaticValues.groups.Single(x => x.Id == ((dynamic)senderMachine).GroupId).Name,
                        GroupID = ((dynamic)senderMachine).GroupId,
                        ProductionDepartmentName = StaticValues.productionDepartments.Single(x => x.Id == ((dynamic)senderMachine).ProductionDepartmentId).Name,
                        CreatedBy = StaticValues.groups.Single(x => x.Id == ((dynamic)senderMachine).GroupId).Name,
                        UserId = user.UserId,
                        SmsSend = user.SmsSend,
                        MailSend = user.MailSend,
                        MobileSend = user.MobileSend,
                        Email = user.Email,
                        FcmToken = companyPerson?.FcmToken
                    };

                    string messageJson = JsonConvert.SerializeObject(message);

                    if (user.MobileSend)
                    {
                        kafkaService.ProduceMessageAsync(messageJson, "interruption-downtime-cause")
                            .ContinueWith(task =>
                            {
                                if (task.IsFaulted)
                                {
                                    // logla: task.Exception?.Message
                                }
                            });
                    }

                    if (user.MailSend && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Length > 5)
                    {
                        string emailTemplate = !string.IsNullOrWhiteSpace(user.Body) && user.Body.Length > 30
                            ? user.Body
                            : EmailTemplateHelper.FaultCauseMail();

                        emailTemplate = emailTemplate
                            .Replace("{Type}", messageMainCause)
                            .Replace("{BranchName}", StaticValues.branch.Name)
                            .Replace("{Yetkili}", companyPerson?.name)
                            .Replace("{CreatedBy}", message.CreatedBy)
                            .Replace("{WorkCenterName}", message.WorkCenterName)
                            .Replace("{MainCause}", message.MainCause)
                            .Replace("{CauseDetail}", message.CauseDetail)
                            .Replace("{ResourceName}", message.ResourceName)
                            .Replace("{Date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                        var mailModel = new EMailModel
                        {
                            UserEmails = user.Email,
                            subject = $"{messageMainCause} Bildirimi : {message.WorkCenterName}",
                            Body = emailTemplate,
                            OriginalBody = emailTemplate,
                            SendRefreshTime = DateTime.Now,
                            SendTime = DateTime.Now,
                            FaultOrInterruptionId = ((dynamic)senderMachine).Id,
                            EmailSendDelayMinutes = user.EmailSendDelayMinutes
                        };

                        EmailQueeService.quee.Enqueue(mailModel);
                        EmailQueeService.start = true;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
