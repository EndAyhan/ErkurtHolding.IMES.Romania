using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using QRCoder;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Utilities for generating and displaying QR codes.
    /// </summary>
    public static class QrCodeHelper
    {
        /// <summary>
        /// Generates a QR code image from <paramref name="qrText"/> and shows it
        /// in a modal DevExpress <see cref="XtraForm"/>.
        /// </summary>
        /// <param name="qrText">The text to encode into the QR code.</param>
        /// <param name="t">
        /// Optional localizer (reads from appSettings Language). If provided,
        /// the form title will use <c>t["qrcode.title"]</c> with <c>{Machine}</c> token.
        /// </param>
        public static void ShowQrCodeForm(string qrText)
        {
            if (string.IsNullOrWhiteSpace(qrText))
                return;

            // Best-effort machine name for title fallback
            var machineName = SafeMachineName();
            var titleTemplate = MessageTextHelper.GetMessageText("QR", "100", "QR Code - {Machine}", "QR");
            if (string.IsNullOrEmpty(titleTemplate))
                titleTemplate = "QR Code - {Machine}";

            var formTitle = titleTemplate.Replace("{Machine}", machineName);

            // Create QR code data
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            {
                // Create a bitmap for the QR (20 = pixels per module)
                using (var bmp = qrCode.GetGraphic(20))
                {
                    // Clone the bitmap before disposing it so PictureEdit owns the clone
                    var displayImage = (Bitmap)bmp.Clone();

                    using (var qrForm = new XtraForm())
                    using (var pictureEdit = new PictureEdit())
                    {
                        qrForm.Text = formTitle;
                        qrForm.Size = new Size(400, 400);
                        qrForm.MinimumSize = new Size(400, 400);
                        qrForm.StartPosition = FormStartPosition.CenterScreen;

                        pictureEdit.Dock = DockStyle.Fill;
                        pictureEdit.Image = displayImage;
                        pictureEdit.Properties.SizeMode = PictureSizeMode.Zoom;
                        pictureEdit.Properties.AllowZoomOnMouseWheel = DefaultBoolean.True;
                        pictureEdit.Properties.ShowCameraMenuItem = CameraMenuItemVisibility.Auto;

                        qrForm.Controls.Add(pictureEdit);

                        try
                        {
                            qrForm.ShowDialog();
                        }
                        finally
                        {
                            // Ensure the image handle is released explicitly
                            if (pictureEdit.Image != null)
                            {
                                var img = pictureEdit.Image;
                                pictureEdit.Image = null;
                                img.Dispose();
                            }
                        }
                    }
                }
            }
        }

        // ---------- helpers ----------

        private static string SafeMachineName()
        {
            try
            {
                var op = ToolsMdiManager.frmOperatorActive;
                if (op != null && op.machine != null && !string.IsNullOrEmpty(op.machine.Definition))
                    return op.machine.Definition;
            }
            catch { /* ignored */ }

            return "Machine";
        }
    }
}
