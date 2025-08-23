using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents a queued email to be sent by the background mail service.
    /// </summary>
    /// <remarks>
    /// Typical flow:
    /// <list type="number">
    /// <item><description>Fill <see cref="UserEmails"/>, <see cref="subject"/>, <see cref="Body"/> (and optionally <see cref="OriginalBody"/>).</description></item>
    /// <item><description>Set <see cref="SendTime"/> (usually <c>DateTime.Now</c>), and optionally <see cref="SendRefreshTime"/> if using delayed/repeat sends.</description></item>
    /// <item><description>Enqueue into EmailQueeService (which updates {ElapsedDuration} using <see cref="SendTime"/> before sending).</description></item>
    /// </list>
    /// <para>
    /// Placeholders: The queue service expects <c>{ElapsedDuration}</c> in the HTML body and will replace it with the minutes elapsed since <see cref="SendTime"/>.
    /// </para>
    /// </remarks>
    [Serializable]
    public class EMailModel
    {
        /// <summary>
        /// Recipient email addresses. Can be a single address or multiple addresses separated by
        /// comma/semicolon/space. The sender should ensure addresses are valid.
        /// </summary>
        public string UserEmails { get; set; }

        /// <summary>
        /// Email subject (kept as <c>subject</c> for backward compatibility).
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// Email body (prefer HTML). May contain the placeholder <c>{ElapsedDuration}</c>.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// A preserved copy of the original body before queue mutations (e.g., so the queue can
        /// re-inject an updated <c>{ElapsedDuration}</c> on repeated sends).
        /// </summary>
        public string OriginalBody { get; set; }

        /// <summary>
        /// Mail priority (defaults to <see cref="MailPriority.Normal"/>).
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// The moment the notification intent was created. The queue uses this to compute
        /// <c>{ElapsedDuration}</c> in minutes.
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// The last time the queued mail was (re)scheduled or sent. Used with
        /// <see cref="EmailSendDelayMinutes"/> to throttle repeated notifications.
        /// </summary>
        public DateTime SendRefreshTime { get; set; }

        /// <summary>
        /// Minutes to wait between repeated sends. If &gt; 0, the queue will re-enqueue and
        /// resend the mail at that cadence (so long as it remains relevant).
        /// </summary>
        public int EmailSendDelayMinutes { get; set; }

        /// <summary>
        /// Correlation id for the related fault/interrupt entity. Used by relevance checks to avoid
        /// sending stale emails when the underlying issue is resolved.
        /// </summary>
        public Guid FaultOrInterruptionId { get; set; }

        /// <summary>
        /// Creates a model with safe defaults.
        /// </summary>
        public EMailModel()
        {
            Priority = MailPriority.Normal;
            SendTime = DateTime.Now;
            SendRefreshTime = DateTime.Now;
        }

        /// <summary>
        /// Splits <see cref="UserEmails"/> into individual addresses using comma, semicolon,
        /// or whitespace separators. Empty results are skipped.
        /// </summary>
        public IEnumerable<string> EnumerateRecipients()
        {
            if (string.IsNullOrWhiteSpace(UserEmails))
                yield break;

            var parts = UserEmails.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                var trimmed = p.Trim();
                if (trimmed.Length > 0)
                    yield return trimmed;
            }
        }
    }
}
