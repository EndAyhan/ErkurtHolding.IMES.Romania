using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail
{
    /// <summary>
    /// Background email dispatch service backed by a concurrent queue.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Enqueue <see cref="EMailModel"/> items to <see cref="quee"/> and set <see cref="start"/> to <c>true</c>
    /// to begin background processing. The service awakens every 10 seconds and attempts to send queued emails.
    /// </para>
    /// <para>
    /// For recurring/reminder emails, set <see cref="EMailModel.EmailSendDelayMinutes"/> to a value &gt; 0.
    /// Such items will be re-enqueued after a successful send and evaluated again after the specified delay.
    /// </para>
    /// <para>
    /// This implementation uses a single, reused <see cref="SmtpClient"/> (lazy-initialized) and creates a fresh
    /// <see cref="MailMessage"/> per send to avoid shared-state issues.
    /// </para>
    /// </remarks>
    public static class EmailQueeService
    {
        /// <summary>
        /// The email queue to be processed by the background sender.
        /// </summary>
        /// <remarks>Internal for unit tests and for producers in the same assembly.</remarks>
        internal static readonly ConcurrentQueue<EMailModel> quee = new ConcurrentQueue<EMailModel>();

        /// <summary>
        /// A public lock object preserved for backward compat with existing code
        /// that may synchronize external producers/consumers.
        /// </summary>
        public static readonly object lockObjectQuee = new object();

        // Credentials (decrypted once at type init)
        private static readonly string _email =
            ErkurtHolding.IMES.DataAccess.ToolsAES.Decrypt(ConfigurationManager.AppSettings["senderEmail"]);

        private static readonly string _password =
            ErkurtHolding.IMES.DataAccess.ToolsAES.Decrypt(ConfigurationManager.AppSettings["senderPassword"]);

        // Timer and run-state
        private static Timer _timer;
        private static volatile bool _start;
        private static int _running; // 0 = idle, 1 = running

        // Lazy, shared SMTP client
        private static SmtpClient _smtpClient;

        /// <summary>
        /// Gets the reusable <see cref="SmtpClient"/> configured for Office 365.
        /// </summary>
        private static SmtpClient SmtpClient
        {
            get
            {
                var client = _smtpClient;
                if (client != null) return client;

                client = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_email, _password),
                    Timeout = 10000 // 10s
                };

                // Race-safe publish
                Interlocked.CompareExchange(ref _smtpClient, client, null);
                return _smtpClient;
            }
        }

        /// <summary>
        /// Starts or stops the background sender loop.
        /// </summary>
        /// <remarks>
        /// Setting this to <c>true</c> creates a timer that invokes the sender every 10 seconds.
        /// Setting to <c>false</c> disposes the timer and stops new iterations (in-flight send will finish).
        /// </remarks>
        public static bool start
        {
            get { return _start; }
            set
            {
                if (value == _start) return;
                _start = value;

                if (value)
                {
                    // Start immediately, then every 10 seconds
                    _timer = new Timer(SendMail, null, dueTime: 0, period: 10_000);
                }
                else
                {
                    // Stop and dispose timer
                    var t = Interlocked.Exchange(ref _timer, null);
                    if (t != null)
                    {
                        t.Change(Timeout.Infinite, Timeout.Infinite);
                        t.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Timer callback that processes queued emails with a reentrancy guard.
        /// </summary>
        /// <param name="state">Unused state object.</param>
        private static void SendMail(object state)
        {
            // Ensure only one execution at a time
            if (Interlocked.Exchange(ref _running, 1) == 1)
                return;

            try
            {
                if (quee.IsEmpty) return;

                // Take a snapshot of current queue length to bound the iteration,
                // avoiding starvation when we re-enqueue delayed items.
                var budget = quee.Count;

                for (int i = 0; i < budget; i++)
                {
                    if (!quee.TryDequeue(out var model) || model == null)
                        continue;

                    // Delay-based scheduling (UTC to avoid DST issues)
                    if (model.EmailSendDelayMinutes > 0)
                    {
                        var reference =
                            (model.SendRefreshTime == default(DateTime) ? model.SendTime : model.SendRefreshTime);
                        if (reference == default(DateTime)) reference = DateTime.UtcNow;

                        var next = reference.ToUniversalTime().AddMinutes(model.EmailSendDelayMinutes);
                        if (DateTime.UtcNow < next)
                        {
                            // Not yet time → put it back at tail
                            quee.Enqueue(model);
                            continue;
                        }

                        // If this email no longer applies (fault/interruption resolved), drop it
                        if (!IsStillRelevant(model))
                            continue;
                    }

                    try
                    {
                        using (var msg = BuildMailMessage(model))
                        {
                            SmtpClient.Send(msg);
                        }

                        // Reschedule if it's a repeating notification
                        if (model.EmailSendDelayMinutes > 0)
                        {
                            model.SendRefreshTime = DateTime.UtcNow;
                            quee.Enqueue(model);
                        }
                    }
                    catch (SmtpException smtpEx)
                    {
                        // TODO: integrate your logger here
                        // Logger.Warn(smtpEx, "SMTP send failed for subject {0}", model?.subject);
                        // Optional: backoff or requeue based on smtpEx.StatusCode
                    }
                    catch (Exception ex)
                    {
                        // TODO: integrate your logger here
                        // Logger.Error(ex, "SendMail failed for subject {0}", model?.subject);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref _running, 0);
            }
        }

        /// <summary>
        /// Creates a fresh <see cref="MailMessage"/> for the given email model.
        /// Handles placeholder expansion and recipient parsing.
        /// </summary>
        /// <param name="model">Email data to send.</param>
        /// <returns>A configured <see cref="MailMessage"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
        private static MailMessage BuildMailMessage(EMailModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var from = new MailAddress(_email, "IMES - INFORMATION");

            var msg = new MailMessage
            {
                From = from,
                Subject = model.subject ?? string.Empty,
                IsBodyHtml = true
            };

            // Resolve body with runtime placeholders
            var template = model.OriginalBody ?? model.Body ?? string.Empty;

            // Elapsed minutes based on UTC
            var elapsedMinutes = model.SendTime != default(DateTime)
                ? (int)(DateTime.UtcNow - model.SendTime.ToUniversalTime()).TotalMinutes
                : 0;

            var body = template.Replace("{ElapsedDuration}", elapsedMinutes.ToString());
            msg.Body = body;

            // Add recipients (comma/semicolon-delimited)
            AddRecipients(msg.To, model.UserEmails);

            return msg;
        }

        /// <summary>
        /// Adds one or more recipients to the provided <see cref="MailAddressCollection"/>.
        /// Supports comma and semicolon delimiters; invalid addresses are ignored.
        /// </summary>
        /// <param name="target">The target collection (e.g., <see cref="MailMessage.To"/>).</param>
        /// <param name="recipients">One or more recipients separated by ',' or ';'.</param>
        private static void AddRecipients(MailAddressCollection target, string recipients)
        {
            if (target == null || string.IsNullOrWhiteSpace(recipients)) return;

            var parts = recipients
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0);

            foreach (var addr in parts)
            {
                try
                {
                    target.Add(addr);
                }
                catch
                {
                    // Ignore invalid addresses to avoid dropping the whole send
                }
            }
        }

        /// <summary>
        /// Determines whether a queued email is still relevant (i.e., the condition that triggered it still exists).
        /// </summary>
        /// <param name="model">The email model.</param>
        /// <returns><c>true</c> if the email should still be sent or repeated; otherwise <c>false</c>.</returns>
        /// <remarks>
        /// This uses a subject keyword heuristic for "arıza" (fault) and "duruş" (interruption) to query
        /// the respective managers. Adjust if you add more categories or switch to a typed model flag.
        /// </remarks>
        private static bool IsStillRelevant(EMailModel model)
        {
            var subj = model.subject == null ? string.Empty : model.subject.ToLowerInvariant();

            if (subj.Contains("arıza")) // fault
                return FaultManager.Current.GetEmailFaultControl(model.FaultOrInterruptionId) != null;

            if (subj.Contains("duruş")) // interruption
                return InterruptionCauseManager.Current.GetEmailInterruptionCauseControl(model.FaultOrInterruptionId) != null;

            return true;
        }

        /// <summary>
        /// Convenience wrapper for starting the background sender.
        /// Equivalent to setting <see cref="start"/> to <c>true</c>.
        /// </summary>
        public static void Start() => start = true;

        /// <summary>
        /// Convenience wrapper for stopping the background sender.
        /// Equivalent to setting <see cref="start"/> to <c>false</c>.
        /// </summary>
        public static void Stop() => start = false;
    }
}
