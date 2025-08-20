using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    public class InterruptionCauseMessageModel
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string MainCause { get; set; }
        public string CauseDetail { get; set; }
        public string Description { get; set; }
        public string WorkCenterName { get; set; }
        public string ResourceName { get; set; }
        public string BranchName { get; set; }
        public string ProductionDepartmentName { get; set; }
        public string GroupName { get; set; }
        public Guid CauseID { get; set; }
        public Guid BranchID { get; set; }
        public Guid GroupID { get; set; }
        public Guid InterruptionCauseID { get; set; }
        public string TopicName { get; set; }
        public Guid UserId { get; set; }
        public bool SmsSend { get; set; }
        public bool MailSend { get; set; }
        public bool MobileSend { get; set; }
        public string Email { get; set; }
        public string FcmToken { get; set; }
    }
}
