using System;
using System.Net.Mail;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    public class EMailModel
    {
        public string UserEmails { get; set; }
        public string subject { get; set; }
        public string Body { get; set; }
        public string OriginalBody { get; set; }
        public MailPriority Priority { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime SendRefreshTime { get; set; }
        public int EmailSendDelayMinutes { get; set; }
        public Guid FaultOrInterruptionId { get; set; }
    }
}
