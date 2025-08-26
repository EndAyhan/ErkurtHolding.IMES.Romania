using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    public partial class FrmPdfViewer : DevExpress.XtraEditors.XtraForm
    {
        public FrmPdfViewer(string documentFilePath)
        {
            try
            {
                InitializeComponent();

                LanguageHelper.InitializeLanguage(this);

                pdfViewer1.LoadDocument(documentFilePath);
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}