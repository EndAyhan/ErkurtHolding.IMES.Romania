using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business.KafkaManager;
using ErkurtHolding.IMES.KafkaFlow;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Sends interruption/fault notifications via Kafka and email for topic subscribers.
    /// </summary>
    public static class NotificationHelper
    {
        /// <summary>
        /// Builds the interruption message for each topic subscriber and dispatches it to Kafka / Email
        /// according to the user’s notification preferences.
        /// </summary>
        /// <param name="topicName">Logical cause group or topic display name (spaces will be replaced by underscores).</param>
        /// <param name="messageDescription">Human readable description.</param>
        /// <param name="messageMainCause">Main cause text (also used in email subject/title).</param>
        /// <param name="messageCauseDetail">Detailed cause text.</param>
        /// <param name="senderMachine">Machine object (dynamic is tolerated; must expose Id, Definition, GroupId, ProductionDepartmentId).</param>
        /// <param name="senderResource">Resource object (dynamic is tolerated; must expose resourceName).</param>
        /// <param name="workCenterId">The related work center identifier.</param>
        /// <param name="topicDefinition">Topic definition selector for the query.</param>
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
                if (StaticValues.branch == null || string.IsNullOrEmpty(StaticValues.branch.ERPConnectionCode))
                    return;

                // Resolve topic by ERP branch + normalized topic name
                var normalizedTopicName = string.IsNullOrEmpty(topicName) ? string.Empty : topicName.Replace(' ', '_');
                var topic = TopicManager.Current.GetTopicByBranchNameAndCauseGroupName(
                    StaticValues.branch.ERPConnectionCode,
                    normalizedTopicName,
                    topicDefinition);

                if (topic == null)
                    return;

                var topicUsers = TopicUserManager.Current.getByTopicId(topic.Id);
                if (topicUsers == null || topicUsers.Count == 0)
                    return;

                // Cache frequently used lookups (avoid repeating Single(...) for each user)
                dynamic machine = senderMachine;
                dynamic resource = senderResource;

                var machineDefinition = SafeStr(machine, "Definition");
                var machineId = SafeGuidOrLong(machine, "Id");
                var machineGroupId = SafeGuidOrLong(machine, "GroupId");
                var machineProdDeptId = SafeGuidOrLong(machine, "ProductionDepartmentId");

                var group = SafeFindById(StaticValues.groups, machineGroupId);
                var prodDept = SafeFindById(StaticValues.productionDepartments, machineProdDeptId);

                var groupName = group != null ? group.Name : string.Empty;
                var workCenterName = machineDefinition;
                var resourceName = SafeStr(resource, "resourceName");

                // Kafka service (assumed initialized in StaticValues)
                KafkaService kafkaService = StaticValues.kafkaService;

                foreach (var user in topicUsers)
                {
                    var companyPerson = CompanyPersonManager.Current.GetCompanyPersonById(user.UserId);

                    var message = new InterruptionCauseMessageModel
                    {
                        TopicName = topic.TopicName,
                        InterruptionCauseID = workCenterId,
                        ResourceName = resourceName,
                        WorkCenterName = workCenterName,
                        CreatedAt = DateTime.Now,
                        Description = messageDescription ?? string.Empty,
                        MainCause = messageMainCause ?? string.Empty,
                        CauseDetail = messageCauseDetail ?? string.Empty,
                        CauseID = machineId, // dynamic can be Guid/long; JSON will serialize accordingly
                        BranchName = StaticValues.branch.ERPConnectionCode,
                        BranchID = StaticValues.branch.Id,
                        GroupName = groupName,
                        GroupID = machineGroupId,
                        ProductionDepartmentName = prodDept != null ? prodDept.Name : string.Empty,
                        CreatedBy = groupName, // preserved from original
                        UserId = user.UserId,
                        SmsSend = user.SmsSend,
                        MailSend = user.MailSend,
                        MobileSend = user.MobileSend,
                        Email = user.Email,
                        FcmToken = companyPerson != null ? companyPerson.FcmToken : null
                    };

                    var messageJson = JsonConvert.SerializeObject(message);

                    // ---- Mobile (Kafka) ----
                    if (user.MobileSend && kafkaService != null)
                    {
                        // Fire-and-forget; log errors inside continuation (optional)
                        kafkaService.ProduceMessageAsync(messageJson, "interruption-downtime-cause")
                            .ContinueWith(task =>
                            {
                                // TODO: add your logger here if desired
                                // if (task.IsFaulted) Logger.Warn(task.Exception, "Kafka produce failed");
                            });
                    }

                    // ---- Email ----
                    if (user.MailSend && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Length > 5)
                    {
                        // choose template: custom body if long enough, otherwise our localized default
                        var template = (!string.IsNullOrWhiteSpace(user.Body) && user.Body.Length > 30)
                            ? user.Body
                            : EmailTemplateHelper.FaultCauseMail();

                        // tokens for {{...}} placeholders – values are HTML‑encoded by FillTemplate
                        var tokens = new Dictionary<string, string>
                        {
                            { "Type", messageMainCause ?? string.Empty },
                            { "BranchName", StaticValues.branch != null ? (StaticValues.branch.Name ?? string.Empty) : string.Empty },
                            { "Yetkili", companyPerson != null ? (companyPerson.name ?? string.Empty) : string.Empty },
                            { "CreatedBy", message.CreatedBy ?? string.Empty },
                            { "WorkCenterName", workCenterName ?? string.Empty },
                            { "MainCause", message.MainCause ?? string.Empty },
                            { "CauseDetail", message.CauseDetail ?? string.Empty },
                            { "ResourceName", resourceName ?? string.Empty },
                            { "Date", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture) }
                            // {ElapsedDuration} intentionally left for EmailQueeService to update at send time
                        };

                        // fill {{Tokens}}; leave {ElapsedDuration} intact for queue service
                        var emailHtml = EmailTemplateHelper.FillTemplate(template, tokens);

                        // Backward-compat: also replace single-brace tokens if template contains them
                        emailHtml = emailHtml
                            .Replace("{Type}", tokens["Type"])
                            .Replace("{BranchName}", tokens["BranchName"])
                            .Replace("{Yetkili}", tokens["Yetkili"])
                            .Replace("{CreatedBy}", tokens["CreatedBy"])
                            .Replace("{WorkCenterName}", tokens["WorkCenterName"])
                            .Replace("{MainCause}", tokens["MainCause"])
                            .Replace("{CauseDetail}", tokens["CauseDetail"])
                            .Replace("{ResourceName}", tokens["ResourceName"])
                            .Replace("{Date}", tokens["Date"]);

                        var localizedNotificationWord = MessageTextHelper.GetMessageText("EMAIL", "114", "Notification", "EMail");

                        var mailModel = new EMailModel
                        {
                            UserEmails = user.Email,
                            subject = string.Format(
                                CultureInfo.InvariantCulture,
                                "{0} {1} : {2}",
                                messageMainCause,
                                localizedNotificationWord,
                                workCenterName),
                            Body = emailHtml,
                            OriginalBody = emailHtml,
                            SendRefreshTime = DateTime.Now,
                            SendTime = DateTime.Now,
                            FaultOrInterruptionId = machineId,
                            EmailSendDelayMinutes = user.EmailSendDelayMinutes
                        };


                        EmailQueeService.quee.Enqueue(mailModel);
                        EmailQueeService.start = true;
                    }
                }
            }
            catch
            {
                // Intentionally swallow to prevent notification pipeline from crashing caller.
                // Optionally plug your logger here.
            }
        }

        // --------- helpers (safe dynamic reads) ---------

        private static string SafeStr(object obj, string memberName)
        {
            if (obj == null || string.IsNullOrEmpty(memberName)) return string.Empty;
            try
            {
                var t = obj.GetType();
                var p = t.GetProperty(memberName);
                if (p != null)
                {
                    var v = p.GetValue(obj, null);
                    return v != null ? v.ToString() : string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }

        private static Guid SafeGuidOrGuid(object obj, string memberName)
        {
            var s = SafeStr(obj, memberName);
            Guid g;
            return Guid.TryParse(s, out g) ? g : Guid.Empty;
        }

        /// <summary>
        /// Returns either a <see cref="Guid"/> (if the value parses) or the original numeric id (boxed), 
        /// preserving your original dynamic behavior for <c>CauseID</c>, <c>GroupID</c>, etc.
        /// </summary>
        private static dynamic SafeGuidOrLong(object obj, string memberName)
        {
            var s = SafeStr(obj, memberName);
            if (string.IsNullOrEmpty(s)) return 0L;

            // try Guid
            Guid g;
            if (Guid.TryParse(s, out g)) return g;

            // try long / int
            long l;
            if (long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out l)) return l;

            int i;
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out i)) return (long)i;

            return s; // last resort: return as string
        }

        private static dynamic SafeFindById<T>(IEnumerable<T> list, object idLike)
        {
            if (list == null || idLike == null) return null;

            // Try GUID match first
            var idGuid = idLike as Guid?;
            if (idGuid.HasValue && idGuid.Value != Guid.Empty)
            {
                return list.Cast<dynamic>().FirstOrDefault(x => (Guid)x.Id == idGuid.Value);
            }

            // Try long/int
            long asLong;
            if (idLike is long) asLong = (long)idLike;
            else if (!long.TryParse(idLike.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out asLong))
                return null;

            return list.Cast<dynamic>().FirstOrDefault(x => (long)x.Id == asLong);
        }
    }
}
