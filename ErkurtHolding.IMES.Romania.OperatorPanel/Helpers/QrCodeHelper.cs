using DevExpress.XtraEditors;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using QRCoder;
using System.Drawing;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public class QrCodeHelper
    {
        public static void ShowQrCodeForm(string qrText)
        {
            // QR kodunu oluştur
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    XtraForm qrForm = new XtraForm
                    {
                        Text = ToolsMdiManager.frmOperatorActive.machine.Definition,
                        Size = new Size(400, 400),
                        MinimumSize = new Size(400, 400),
                        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
                    };
                    PictureEdit pictureEdit = new PictureEdit
                    {
                        Dock = System.Windows.Forms.DockStyle.Fill,
                        Image = qrCodeImage,
                        Properties =
                    {
                        SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom,
                        AllowZoomOnMouseWheel = DevExpress.Utils.DefaultBoolean.True
                    }
                    };
                    qrForm.Controls.Add(pictureEdit);
                    qrForm.ShowDialog();
                }
            }
        }
    }
}
