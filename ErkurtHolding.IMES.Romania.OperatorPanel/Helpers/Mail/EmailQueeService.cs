using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail
{
    public static class EmailQueeService
    {
        internal static ConcurrentQueue<EMailModel> quee = new ConcurrentQueue<EMailModel>();
        public static readonly object lockObjectQuee = new object();
        private static bool _start;

        private static readonly string email = ErkurtHolding.IMES.DataAccess.ToolsAES.Decrypt(ConfigurationManager.AppSettings["senderEmail"]);
        private static readonly string password = ErkurtHolding.IMES.DataAccess.ToolsAES.Decrypt(ConfigurationManager.AppSettings["senderPassword"]);
        private static System.Threading.Timer _timer;

        private static SmtpClient _smtpClient;

        private static SmtpClient smtpClient
        {
            get
            {
                if (_smtpClient == null)
                {
                    _smtpClient = new SmtpClient();
                    _smtpClient.Host = "smtp.office365.com";
                    _smtpClient.Port = 587;
                    _smtpClient.EnableSsl = true;
                    _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    _smtpClient.UseDefaultCredentials = false;
                    _smtpClient.Credentials = new NetworkCredential(email, password);

                }
                return _smtpClient;
            }

        }

        private static MailMessage _mailMessage;
        private static MailMessage mailMessage
        {
            get
            {
                if (_mailMessage == null)
                {
                    _mailMessage = new MailMessage();
                    _mailMessage.IsBodyHtml = true;
                    _mailMessage.From = new MailAddress(email, "IMES - INFORMATION");
                }
                return _mailMessage;
            }
        }

        public static bool start
        {
            get { return _start; }
            set
            {
                if (value == true)
                {
                    if (_start == true)
                        return;
                    _start = value;
                    TimerCallback timerCallback = new TimerCallback(SendMail);
                    _timer = new System.Threading.Timer(timerCallback, null, 0, 10000);
                }
                _start = value;
            }
        }

        private static int _running;

        private static void SendMail(object state)
        {
            if (Interlocked.Exchange(ref _running, 1) == 1)
                return;

            try
            {
                int count = quee.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!quee.TryDequeue(out EMailModel model))
                        continue;

                    if (model.EmailSendDelayMinutes > 0)
                    {
                        DateTime hedef = model.SendRefreshTime.AddMinutes(model.EmailSendDelayMinutes);
                        if (DateTime.Now < hedef)
                        {
                            quee.Enqueue(model);
                            continue;
                        }

                        if (!IsStillRelevant(model))
                            continue;
                    }

                    try
                    {
                        // Body şablonu her seferinde en baştaki haliyle alınmalı
                        string bodyTemplate = model.OriginalBody ?? model.Body;

                        int totalDuration = model.SendTime != default ? (int)(DateTime.Now - model.SendTime).TotalMinutes : 0;

                        model.Body = bodyTemplate.Replace("{ElapsedDuration}", totalDuration.ToString());

                        mailMessage.To.Clear();
                        mailMessage.To.Add(model.UserEmails);
                        mailMessage.Subject = model.subject;
                        mailMessage.Body = model.Body;
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = true;
                        smtpClient.Credentials = new NetworkCredential(email, password);
                        smtpClient.Send(mailMessage);

                        if (model.EmailSendDelayMinutes > 0)
                        {
                            model.SendRefreshTime = DateTime.Now;
                            quee.Enqueue(model);
                        }
                    }
                    catch { }
                }
            }
            finally
            {
                _running = 0;
            }
        }

        private static bool IsStillRelevant(EMailModel model)
        {
            string subj = model.subject?.ToLowerInvariant() ?? "";

            if (subj.Contains("arıza"))
                return FaultManager.Current.GetEmailFaultControl(model.FaultOrInterruptionId) != null;

            if (subj.Contains("duruş"))
                return InterruptionCauseManager.Current.GetEmailInterruptionCauseControl(model.FaultOrInterruptionId) != null;

            return true;
        }
    }
}
