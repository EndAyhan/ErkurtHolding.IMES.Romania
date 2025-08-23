using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Tools
{
    /// <summary>
    /// Centralized message box helper for the Operator Panel.
    /// Uses <c>StaticValues.T</c> for localization. If a key is missing, falls back to the
    /// original Turkish text to preserve current behavior.
    /// </summary>
    public static class ToolsMessageBox
    {
        // -------- Localization helpers --------

        /// <summary>
        /// Gets a localized text from <c>StaticValues.T</c>, or returns <paramref name="fallback"/>
        /// if the key is missing/empty.
        /// </summary>
        private static string TT(string key, string fallback)
        {
            try
            {
                var s = StaticValues.T[key];
                return string.IsNullOrEmpty(s) ? fallback : s;
            }
            catch
            {
                return fallback;
            }
        }

        // Common titles (keys grouped under "ui.message.*")
        private static string TitleInfo => TT("ui.message.info.title", "Bilgilendirme..!");
        private static string TitleWarn => TT("ui.message.warn.title", "Dikkat..!");
        private static string TitleError => TT("ui.message.error.title", "Hata..!");
        private static string TitleQuestion => TT("ui.message.question.title", "Soru..!");

        // Common bodies
        private static string BodyUnexpectedError => TT("ui.message.error.unexpected", "Beklenmedik hata ile karşılaşıldı");
        private static string BodySuccess => TT("ui.message.success", "İşlem Başarılı");
        private static string BodyInsertSuccess => TT("ui.message.insert.success", "Kayıt işlemi başarılı");
        private static string BodyDeleteConfirm => TT("ui.message.delete.confirm", "Kaydı silmek istediğinize emin misiniz?");
        private static string BodyCancelConfirm => TT("ui.message.cancel.confirm", "Devam etmek istemediğinize emin misiniz");

        // -------- Information --------

        /// <summary>
        /// Shows an informational message box with OK button.
        /// </summary>
        public static void Information(IWin32Window owner, string message, Dictionary<string, object> prm = null)
        {
            XtraMessageBox.Show(owner, ReplaceParameters(message, prm), TitleInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // -------- Warning --------

        /// <summary>
        /// Shows a warning message box with OK button.
        /// </summary>
        public static void Warning(IWin32Window owner, string message, Dictionary<string, object> prm = null)
        {
            XtraMessageBox.Show(owner, ReplaceParameters(message, prm), TitleWarn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a generic warning for unexpected situations.
        /// </summary>
        public static void Warning(IWin32Window owner)
        {
            XtraMessageBox.Show(owner, BodyUnexpectedError, TitleWarn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // -------- Error --------

        /// <summary>
        /// Shows an error message box with OK button.
        /// </summary>
        public static void Error(IWin32Window owner, string message, Dictionary<string, object> prm = null)
        {
            XtraMessageBox.Show(owner, ReplaceParameters(message, prm), TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a generic error message box.
        /// </summary>
        public static void Error(IWin32Window owner)
        {
            XtraMessageBox.Show(owner, BodyUnexpectedError, TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows an error message box and appends the exception message for admin information.
        /// </summary>
        public static void Error(IWin32Window owner, Exception ex)
        {
            var adminInfo = TT("ui.message.error.adminInfoPrefix", "Admin Info");
            XtraMessageBox.Show(owner, $"{BodyUnexpectedError}\r\n{adminInfo} : {ex.Message}", TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows an error message box with a custom body and exception details for admin information.
        /// </summary>
        public static void Error(IWin32Window owner, string message, Exception ex)
        {
            var adminInfo = TT("ui.message.error.adminInfoPrefix", "Admin Info");
            XtraMessageBox.Show(owner, $"{message}\r\n{adminInfo} : {ex.Message}", TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // -------- Success --------

        /// <summary>
        /// Shows a generic success message.
        /// </summary>
        public static void SuccessMessage(IWin32Window owner)
        {
            XtraMessageBox.Show(owner, BodySuccess, TitleInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows a success message for insert operations.
        /// </summary>
        public static void InsertSuccessMessage(IWin32Window owner)
        {
            XtraMessageBox.Show(owner, BodyInsertSuccess, TitleInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // -------- Question / Confirmations --------

        /// <summary>
        /// Shows a Yes/No question box. Returns <c>true</c> for Yes.
        /// </summary>
        public static bool Question(IWin32Window owner, string message, Dictionary<string, object> prm = null)
        {
            return XtraMessageBox.Show(owner, ReplaceParameters(message, prm), TitleQuestion, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Asks for delete confirmation (Yes/No). Returns <c>true</c> if user confirms.
        /// </summary>
        public static bool DeleteQuestion(IWin32Window owner)
        {
            return XtraMessageBox.Show(owner, BodyDeleteConfirm, TitleQuestion, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Asks for cancel confirmation (Yes/No). Returns <c>true</c> if user confirms cancel.
        /// </summary>
        internal static bool CancelMessage(IWin32Window owner)
        {
            return XtraMessageBox.Show(owner, BodyCancelConfirm, TitleWarn, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // -------- Styled / Auto-close --------

        /// <summary>
        /// Shows a large, styled warning box with a single OK button.
        /// </summary>
        public static void BigWarning(IWin32Window owner, string message)
        {
            var args = new XtraMessageBoxArgs
            {
                Caption = TitleWarn,
                Text = message,
                Buttons = new[] { DialogResult.OK },
                Owner = owner
            };
            args.Showing += Args_Showing;
            XtraMessageBox.Show(args);
        }

        /// <summary>
        /// Shows an auto-closing message box (3 seconds) with a single OK button.
        /// </summary>
        internal static void AutoCloseMessage(IWin32Window owner, string message, string caption)
        {
            var args = new XtraMessageBoxArgs
            {
                Caption = caption,
                Text = message,
                Buttons = new[] { DialogResult.OK },
                Owner = owner
            };
            args.AutoCloseOptions.Delay = 3000;
            args.Showing += Args_Showing;
            XtraMessageBox.Show(args);
        }

        // Style the DevExpress message form (caption font/size/color, button size)
        private static void Args_Showing(object sender, XtraMessageShowingArgs e)
        {
            e.Form.Appearance.FontStyleDelta = FontStyle.Bold;
            e.Form.Appearance.FontSizeDelta = 25;
            e.Form.Appearance.ForeColor = Color.Black;
            e.Form.Appearance.BackColor = Color.DeepSkyBlue;

            var buttons = e.Buttons as MessageButtonCollection;
            var ok = buttons != null ? buttons[DialogResult.OK] as SimpleButton : null;
            if (ok != null)
            {
                ok.Appearance.FontSizeDelta = 20;
                ok.Height += 25;
            }
        }

        // -------- Parameter replacement --------

        /// <summary>
        /// Replaces placeholders in the form <c>@Key</c> with the corresponding values
        /// from <paramref name="prm"/>. Keys are matched as-is (case sensitive).
        /// If <paramref name="prm"/> is null or empty, returns <paramref name="msg"/> unchanged.
        /// </summary>
        public static string ReplaceParameters(string msg, Dictionary<string, object> prm)
        {
            if (string.IsNullOrEmpty(msg) || prm == null || prm.Count == 0)
                return msg;

            foreach (var p in prm)
            {
                var key = p.Key ?? string.Empty;
                var val = p.Value == null ? string.Empty : p.Value.ToString();
                if (key.Length == 0) continue;

                // Preserve your current convention (keys already include '@' in many places).
                msg = msg.Replace(key, val);
            }

            return msg;
        }
    }
}
