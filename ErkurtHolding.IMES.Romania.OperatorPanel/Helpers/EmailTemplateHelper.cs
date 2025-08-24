using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web; // for HttpUtility.HtmlEncode (available in .NET Framework)

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Provides HTML email templates and helper methods for safely filling them with data.
    /// </summary>
    public static class EmailTemplateHelper
    {
        /// <summary>
        /// Returns the localized HTML template for a Fault/Interruption notification email.
        /// </summary>
        /// <remarks>
        /// The template preserves runtime placeholders (e.g., <c>{Type}</c>, <c>{BranchName}</c>, <c>{ElapsedDuration}</c>)
        /// so that upstream code (like <see cref="Helpers.Mail.EmailQueeService"/>) can replace some values later.
        /// </remarks>
        /// <param name="t">
        /// Optional text provider. If <c>null</c>, a new <see cref="JsonText"/> is created and
        /// the current <c>appSettings["Language"]</c> (default <c>en_EN</c>) is used.
        /// </param>
        /// <returns>HTML email template as a string.</returns>
        public static string FaultCauseMail()
        {
            // Localized snippets (you can customize in your JSON)
            string title = MessageTextHelper.GetMessageText("EMAIL", "101", "{Type} Notification", "EMail");
            string greetingPrefix = MessageTextHelper.GetMessageText("EMAIL", "102", "Hello", "EMail");
            string detailsHeader = MessageTextHelper.GetMessageText("EMAIL", "103", "Details:", "EMail");
            string footer = MessageTextHelper.GetMessageText("EMAIL", "105", "This email was generated automatically; please do not reply.", "EMail");
            string informed = MessageTextHelper.GetMessageText("EMAIL", "104", "For your information.", "EMail");

            // Labels (words following the dynamic {Type} placeholder)
            string branchLbl = MessageTextHelper.GetMessageText("EMAIL", "106", "Branch", "EMail");
            string mainCauseLbl = MessageTextHelper.GetMessageText("EMAIL", "107", "Main Cause", "EMail");
            string causeDetailLbl = MessageTextHelper.GetMessageText("EMAIL", "108", "Description", "EMail");
            string workCenterLbl = MessageTextHelper.GetMessageText("EMAIL", "109", "Work Center", "EMail");
            string resourceLbl = MessageTextHelper.GetMessageText("EMAIL", "110", "Resource", "EMail");
            string startDateLbl = MessageTextHelper.GetMessageText("EMAIL", "111", "Start Date", "EMail");
            string elapsedLbl = MessageTextHelper.GetMessageText("EMAIL", "112", "Total Elapsed Time", "EMail");
            string minutesSuffix = MessageTextHelper.GetMessageText("EMAIL", "113", "MINUTES", "EMail");

            // Build HTML (use StringBuilder for readability)
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>")
              .AppendLine("<html>")
              .AppendLine("<head>")
              .AppendLine("  <meta charset='UTF-8'>")
              .AppendFormat("  <title>{0}</title>\n", HtmlEncode(title))
              .AppendLine("  <style>")
              .AppendLine("    body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }")
              .AppendLine("    .container { max-width: 600px; background-color: #ffffff; padding: 20px; border-radius: 8px;")
              .AppendLine("                 box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); margin: auto; text-align: left; }")
              .AppendLine("    h2 { text-align: center; color: #333; font-size: 22px; }")
              .AppendLine("    p { font-size: 16px; color: #555; }")
              .AppendLine("    .highlight { font-weight: bold; color: #d32f2f; }")
              .AppendLine("    .bold { font-weight: bold; }")
              .AppendLine("    .details { margin-top: 15px; }")
              .AppendLine("    .details ul { list-style-type: none; padding: 0; }")
              .AppendLine("    .details li { margin: 5px 0; font-size: 16px; }")
              .AppendLine("    .footer { text-align: center; font-size: 12px; color: #777; margin-top: 20px; padding-top: 10px; border-top: 1px solid #ddd; }")
              .AppendLine("  </style>")
              .AppendLine("</head>")
              .AppendLine("<body>")
              .AppendLine("<div class='container'>")
              .AppendFormat("  <h2>{0}</h2>\n", HtmlEncode(title))
              .AppendFormat("  <p>{0} <span class='highlight'>{{Yetkili}}</span>,</p>\n", HtmlEncode(greetingPrefix))
              .AppendFormat("  <p><span class='highlight'>{{CreatedBy}}</span> tarafından <span class='highlight'>{{WorkCenterName}}</span> iş merkezinde bir {{Type}} bildirimi yapılmıştır.</p>\n")
              .AppendLine()
              .AppendLine("  <div class='details'>")
              .AppendFormat("    <p><strong>{0}</strong></p>\n", HtmlEncode(detailsHeader))
              .AppendLine("    <ul>")
              .AppendFormat("      <li><span class='bold'>• {{Type}} {0}:</span> {{BranchName}}</li>\n", HtmlEncode(branchLbl))
              .AppendFormat("      <li><span class='bold'>• {{Type}} {0}:</span> {{MainCause}}</li>\n", HtmlEncode(mainCauseLbl))
              .AppendFormat("      <li><span class='bold'>• {{Type}} {0}:</span> {{CauseDetail}}</li>\n", HtmlEncode(causeDetailLbl))
              .AppendFormat("      <li><span class='bold'>• {0}:</span> {{WorkCenterName}}</li>\n", HtmlEncode(workCenterLbl))
              .AppendFormat("      <li><span class='bold'>• {0}:</span> {{ResourceName}}</li>\n", HtmlEncode(resourceLbl))
              .AppendFormat("      <li><span class='bold'>• {{Type}} {0}:</span> {{Date}}</li>\n", HtmlEncode(startDateLbl))
              .AppendFormat("      <li><span class='bold'>• {{Type}} {0}:</span> {{ElapsedDuration}} {1}</li>\n", HtmlEncode(elapsedLbl), HtmlEncode(minutesSuffix))
              .AppendLine("    </ul>")
              .AppendLine("  </div>")
              .AppendLine()
              .AppendFormat("  <p>{0}</p>\n", HtmlEncode(informed))
              .AppendFormat("  <p class='footer'>{0}</p>\n", HtmlEncode(footer))
              .AppendLine("</div>")
              .AppendLine("</body>")
              .AppendLine("</html>");

            // NOTE: we used {{Token}} double braces to avoid accidental .NET string.Format collisions.
            // The FillTemplate method below replaces {{Token}} with HTML-encoded values.
            return sb.ToString();
        }

        /// <summary>
        /// Fills a template returned by <see cref="FaultCauseMail"/> by replacing tokens like
        /// <c>{{Type}}</c>, <c>{{BranchName}}</c>, <c>{{ElapsedDuration}}</c> with provided values.
        /// </summary>
        /// <remarks>
        /// All values are HTML-encoded via <see cref="HttpUtility.HtmlEncode(string)"/> to keep the email safe.
        /// Tokens not present in <paramref name="tokens"/> remain unchanged.
        /// </remarks>
        /// <param name="template">The HTML template containing <c>{{Token}}</c> placeholders.</param>
        /// <param name="tokens">Key/value pairs to insert. Keys should NOT include braces.</param>
        /// <returns>HTML with tokens replaced.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="template"/> is null.</exception>
        public static string FillTemplate(string template, IDictionary<string, string> tokens)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (tokens == null || tokens.Count == 0) return template;

            var result = new StringBuilder(template);
            foreach (var kvp in tokens)
            {
                var key = kvp.Key ?? string.Empty;
                var value = kvp.Value ?? string.Empty;
                // Replace {{Key}} with HTML-encoded value
                result.Replace("{{" + key + "}}", HtmlEncode(value));
            }
            return result.ToString();
        }

        /// <summary>
        /// Encodes a string for safe inclusion in HTML.
        /// </summary>
        private static string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s ?? string.Empty);
        }
    }
}
