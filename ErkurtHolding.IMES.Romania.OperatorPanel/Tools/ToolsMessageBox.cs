using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Tools
{
    public static class ToolsMessageBox
    {
        #region Information
        public static void Information(IWin32Window Frm, string Message, Dictionary<string, object> prm = null)
        {
            XtraMessageBox.Show(Frm, ReplaceParameters(Message, prm), "Bilgilendirme..!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Warning
        public static void Warning(IWin32Window Frm, string Message, Dictionary<string, object> prm = null)
        {
            XtraMessageBox.Show(Frm, ReplaceParameters(Message, prm), "Dikkat..!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void Warning(IWin32Window Frm)
        {
            XtraMessageBox.Show(Frm, "Beklenmedik hata ile karşılaşıldı", "Dikkat..!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region Warning
        public static void Error(IWin32Window Frm, string Message, Dictionary<string, object> prm = null)
        {
            XtraMessageBox.Show(Frm, ReplaceParameters(Message, prm), "Hata..!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Error(IWin32Window Frm)
        {
            XtraMessageBox.Show(Frm, "Beklenmedik hata ile karşılaşıldı", "Hata..!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Error(IWin32Window Frm, Exception ex)
        {
            XtraMessageBox.Show(Frm, $"{"Beklenmedik hata ile karşılaşıldı"}\r\nAdmin Info : {ex.Message}", "Hata..!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Error(IWin32Window Frm, string Message, Exception ex)
        {
            XtraMessageBox.Show(Frm, $"{Message}\r\nAdmin Info : {ex.Message}", "Hata..!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Success
        public static void SuccessMessage(IWin32Window Frm)
        {
            XtraMessageBox.Show(Frm, "İşlem Başarılı", "Bilgilendirme..!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void InsertSuccessMessage(IWin32Window Frm)
        {
            XtraMessageBox.Show(Frm, "Kayıt işlemi başarılı", "Bilgilendirme..!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Question
        public static bool Question(IWin32Window Frm, string Message, Dictionary<string, object> prm = null)
        {
            return XtraMessageBox.Show(Frm, ReplaceParameters(Message, prm), "Soru..!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
        #endregion

        #region Delete
        public static bool DeleteQuestion(IWin32Window Frm)
        {
            return XtraMessageBox.Show(Frm, "Kaydı silmek istediğinize emin misiniz?", "Soru..!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
        #endregion

        #region Cancle
        internal static bool CancelMessage(IWin32Window Frm)
        {
            if (XtraMessageBox.Show((IWin32Window)Frm, "Devam etmek istemediğinize eminmisiniz", "Dikkat..!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

                return true;
            else
                return false;

        }
        #endregion

        public static void BigWarning(IWin32Window Frm, string Message)
        {
            XtraMessageBoxArgs args = new XtraMessageBoxArgs();
            args.Caption = "Dikkat..!";
            args.Text = Message;
            args.Buttons = new DialogResult[] { DialogResult.OK };
            args.Showing += Args_Showing;
            args.Owner = Frm;
            XtraMessageBox.Show(args).ToString();
        }

        internal static void AutoCloseMessage(IWin32Window Frm, string message, string Caption)
        {
            XtraMessageBoxArgs args = new XtraMessageBoxArgs();
            args.Caption = Caption;
            args.Text = message;
            args.Buttons = new DialogResult[] { DialogResult.OK };
            args.Owner = Frm;
            args.AutoCloseOptions.Delay = 3000;
            args.Showing += Args_Showing;

            XtraMessageBox.Show(args).ToString();

        }

        private static void Args_Showing(object sender, XtraMessageShowingArgs e)
        {
            // Bold message caption
            e.Form.Appearance.FontStyleDelta = FontStyle.Bold;
            e.Form.Appearance.FontSizeDelta = 25;
            e.Form.Appearance.ForeColor = Color.Black;
            e.Form.Appearance.BackColor = Color.DeepSkyBlue;
            // Increased button height and font size
            MessageButtonCollection buttons = e.Buttons as MessageButtonCollection;
            SimpleButton btn = buttons[System.Windows.Forms.DialogResult.OK] as SimpleButton;
            if (btn != null)
            {
                btn.Appearance.FontSizeDelta = 20;
                btn.Height += 25;
            }
        }

        public static string ReplaceParameters(string msg, Dictionary<string, object> prm)
        {
            if (prm != null)
            {
                foreach (var p in prm)
                {
                    msg = msg.Replace(p.Key, p.Value.ToString());
                }
            }

            return msg;
        }
    }

}
