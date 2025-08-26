using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Message payload used for interruption / downtime notifications,
    /// distributed through Kafka, email, or push channels.
    /// </summary>
    [Serializable]
    public class InterruptionCauseMessageModel
    {
        /// <summary>
        /// Gets or sets the timestamp when the message was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the name of the user or system that created the message.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the main cause of the interruption or downtime.
        /// </summary>
        public string MainCause { get; set; }

        /// <summary>
        /// Gets or sets additional descriptive details of the cause.
        /// </summary>
        public string CauseDetail { get; set; }

        /// <summary>
        /// Gets or sets a free-text description of the interruption event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the work center name where the interruption occurred.
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// Gets or sets the resource name associated with the interruption.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the branch (factory/site) name.
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Gets or sets the production department name.
        /// </summary>
        public string ProductionDepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the group name related to this interruption.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the specific cause.
        /// </summary>
        public Guid CauseID { get; set; }

        /// <summary>
        /// Gets or sets the branch identifier.
        /// </summary>
        public Guid BranchID { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the interruption cause record.
        /// </summary>
        public Guid InterruptionCauseID { get; set; }

        /// <summary>
        /// Gets or sets the Kafka topic name where this message will be published.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the user identifier for the recipient.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an SMS notification should be sent.
        /// </summary>
        public bool SmsSend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an email notification should be sent.
        /// </summary>
        public bool MailSend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a mobile push notification should be sent.
        /// </summary>
        public bool MobileSend { get; set; }

        /// <summary>
        /// Gets or sets the recipient's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Firebase Cloud Messaging (FCM) token for push notifications.
        /// </summary>
        public string FcmToken { get; set; }
    }
}
