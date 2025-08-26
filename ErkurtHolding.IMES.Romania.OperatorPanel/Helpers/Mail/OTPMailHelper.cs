using System;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers.Mail
{
    public static class OTPMailHelper
    {
        private const string SmtpServer = "smtp.office365.com";
        private const int SmtpPort = 587;

        private static readonly Lazy<string> _senderEmail = new Lazy<string>(() =>
            ErkurtHolding.IMES.DataAccess.ToolsAES.Decrypt(ConfigurationManager.AppSettings["senderEmail"]));

        private static readonly Lazy<string> _senderPassword = new Lazy<string>(() =>
            ErkurtHolding.IMES.DataAccess.ToolsAES.Decrypt(ConfigurationManager.AppSettings["senderPassword"]));

        public static readonly TimeSpan DefaultOtpValidity = TimeSpan.FromMinutes(5);

        public static bool SendOtpEmail(string recipientEmail, string otpCode, string machineDefinition, TimeSpan? validFor = null)
        {
            return SendOtpEmailAsync(recipientEmail, otpCode, machineDefinition, validFor).GetAwaiter().GetResult();
        }

        public static async Task<bool> SendOtpEmailAsync(string recipientEmail, string otpCode, string machineDefinition, TimeSpan? validFor = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipientEmail))
                    throw new ArgumentException(MessageTextHelper.GetMessageText("OTP", "100", "Recipient email address cannot be empty.", "EMail"));
                if (string.IsNullOrWhiteSpace(otpCode))
                    throw new ArgumentException(MessageTextHelper.GetMessageText("OTP", "101", "OTP code cannot be empty.", "EMail"));
                if (string.IsNullOrWhiteSpace(machineDefinition))
                    throw new ArgumentException(MessageTextHelper.GetMessageText("OTP", "102", "Machine name cannot be empty.", "EMail"));

                var senderEmail = _senderEmail.Value;
                var senderPassword = _senderPassword.Value;

                if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderPassword))
                    throw new InvalidOperationException(MessageTextHelper.GetMessageText("OTP", "103", "SMTP credentials are not configured.", "EMail"));

                var validity = validFor ?? DefaultOtpValidity;
                var prm = machineDefinition.CreateParameters("@Machine");
                var subject = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("OTP", "104", "Your One-Time Password for the @Machine workstation", "EMail"), prm);

                var body = CreateHtmlBody(machineDefinition, otpCode, validity);
                await SendEmailAsync(senderEmail, senderPassword, recipientEmail, subject, body);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string CreateHtmlBody(string machineDefinition, string otpCode, TimeSpan validity)
        {
            var now = DateTime.Now;
            var until = now.Add(validity);

            var prm = machineDefinition.CreateParameters("@Machine");
            var title = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("OTP", "105", "{Machine} Workstation", "EMail"), prm);
            var subHeader = MessageTextHelper.GetMessageText("OTP", "106", "Your One-Time Password to Get a Manual Label", "EMail");

            var colFeature = MessageTextHelper.GetMessageText("OTP", "107", "Property", "EMail");
            var colValue = MessageTextHelper.GetMessageText("OTP", "108", "Value", "EMail");

            var lblOtp = MessageTextHelper.GetMessageText("OTP", "109", "Verification Code", "EMail");
            var lblStart = MessageTextHelper.GetMessageText("OTP", "110", "Code Start Time", "EMail");
            var lblEnd = MessageTextHelper.GetMessageText("OTP", "111", "Code End Time", "EMail");
            var lblValidity = MessageTextHelper.GetMessageText("OTP", "112", "Code Validity Period", "EMail");

            var footer = MessageTextHelper.GetMessageText("OTP", "113", "This email was generated automatically, please do not reply.", "EMail");

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"UTF-8\" />");
            sb.AppendLine("<style>body{font-family:'Segoe UI';}</style>");
            sb.AppendLine("</head><body>");
            sb.AppendLine($"<h1>{title}</h1>");
            sb.AppendLine($"<h2>{subHeader}</h2>");
            sb.AppendLine("<table border='1' cellspacing='0' cellpadding='4'>");
            sb.AppendLine($"<tr><th>{colFeature}</th><th>{colValue}</th></tr>");
            sb.AppendLine($"<tr><td>{lblOtp}</td><td>{otpCode}</td></tr>");
            sb.AppendLine($"<tr><td>{lblStart}</td><td>{now}</td></tr>");
            sb.AppendLine($"<tr><td>{lblEnd}</td><td>{until}</td></tr>");
            sb.AppendLine($"<tr><td>{lblValidity}</td><td>{(int)validity.TotalMinutes} dakika</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine($"<p>{footer}</p>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        private static async Task SendEmailAsync(string senderEmail, string senderPassword, string recipientEmail, string subject, string htmlBody)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(senderEmail);
                message.To.Add(recipientEmail.Trim());
                message.Subject = subject;
                message.Body = htmlBody;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient(SmtpServer, SmtpPort))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message).ConfigureAwait(false);
                }
            }
        }

        public static string ShowInputDialog(string title, string promptText, DateTime expirationTime)
        {
            // localized labels
            var lblDelete = MessageTextHelper.GetMessageText("OTP", "114", "Delete", "EMail");
            var lblCancel = MessageTextHelper.GetMessageText("OTP", "115", "Cancel", "EMail");
            var lblOk = MessageTextHelper.GetMessageText("OTP", "116", "OK", "EMail");
            var lblRemaining = MessageTextHelper.GetMessageText("OTP", "117", "Remaining Time", "EMail");

            using (var inputBox = new Form { Width = 350, Height = 480, FormBorderStyle = FormBorderStyle.FixedDialog })
            using (var textBox = new TextBox { Left = 10, Top = 74, Width = 200 })
            using (var deleteButton = new Button { Text = lblDelete, Left = 220, Top = 74 })
            using (var timerLabel = new Label { Left = 10, Top = 114, AutoSize = true })
            using (var numPadPanel = new Panel { Left = 30, Top = 156, Width = 280, Height = 250 })
            using (var timer = new Timer { Interval = 1000 })
            {
                deleteButton.Click += (_, __) =>
                {
                    if (textBox.Text.Length > 0)
                        textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
                };

                // Cancel
                var cancelButton = new Button { Text = lblCancel, Left = 0, Top = 3 * 65, Width = 80, Height = 60 };
                cancelButton.Click += (_, __) => { inputBox.DialogResult = DialogResult.Cancel; inputBox.Close(); };

                // OK
                var okButton = new Button { Text = lblOk, Left = 170, Top = 3 * 65, Width = 80, Height = 60, DialogResult = DialogResult.OK };
                okButton.Click += (_, __) => inputBox.Close();

                numPadPanel.Controls.Add(cancelButton);
                numPadPanel.Controls.Add(okButton);

                timer.Tick += (_, __) =>
                {
                    var remaining = expirationTime - DateTime.Now;
                    if (remaining.TotalSeconds <= 0)
                    {
                        timer.Stop();
                        inputBox.DialogResult = DialogResult.Cancel;
                        inputBox.Close();
                    }
                    else
                    {
                        timerLabel.Text = $"{lblRemaining}: {remaining.Minutes}:{remaining.Seconds:D2}";
                    }
                };

                inputBox.Controls.Add(textBox);
                inputBox.Controls.Add(deleteButton);
                inputBox.Controls.Add(timerLabel);
                inputBox.Controls.Add(numPadPanel);

                timer.Start();
                return inputBox.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }
    }
}
