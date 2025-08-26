using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Romania.OperatorPanel.GridModels;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmSplitProductDetail : DevExpress.XtraEditors.XtraForm
    {
        private QuestionableProductGridModel qpgm;

        public FrmSplitProductDetail(QuestionableProductGridModel qpgm)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            this.qpgm = qpgm;

            txtOrderNo.Text = qpgm.OrderNo;
            txtOperationNo.Text = qpgm.OperationNo.ToString();
            txtPartNo.Text = qpgm.PartNo;
            txtProductionDate.Text = qpgm.StartDate.ToString();
            txtSerial.Text = qpgm.Serial;
            txtBarcode.Text = qpgm.Barcode;
            txtQuantity.Text = qpgm.Quantity.ToString();
            txtUnit.Text = qpgm.Unit;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal scrapAmount = 0;
            string s = txtScrapQuantity.Text.Trim();
            if (s.Length == 0)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "950", "Geçerli bir değer girmelisiniz", "Message"));
                return;
            }
            if (!decimal.TryParse(s, out scrapAmount))
            {
                var prm = s.CreateParameters("@Value");
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "951", "@Value geçerli bir değer değil", "Message"), prm);
                return;
            }
            var orig = qpgm.ShopOrderProductionDetail;
            if (scrapAmount <= 0 || scrapAmount >= orig.Quantity)
            {
                var prm = scrapAmount.CreateParameters("@ScrapAmount");
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "952", "@ScrapAmount geçerli bir değer değil", "Message"), prm);
                return;
            }

            ShopOrderProductionDetail productionDetail = new ShopOrderProductionDetail();
            productionDetail.ShopOrderOperationID = orig.ShopOrderOperationID;
            productionDetail.ShopOrderProductionID = orig.ShopOrderProductionID;
            productionDetail.WorkCenterID = orig.WorkCenterID;
            productionDetail.ResourceID = orig.ResourceID;
            productionDetail.ShiftId = orig.ShiftId;
            productionDetail.StartDate = orig.StartDate;
            productionDetail.EndDate = orig.EndDate;
            productionDetail.Unit = orig.Unit;
            productionDetail.BoxID = orig.BoxID;
            productionDetail.ProductionStateID = orig.ProductionStateID;
            productionDetail.Factor = orig.Factor;
            productionDetail.Divisor = orig.Divisor;
            productionDetail.Printed = orig.Printed;
            productionDetail.TypeId = orig.TypeId;
            productionDetail.IfsReported = orig.IfsReported;
            productionDetail.ProductID = orig.ProductID;
            productionDetail.IfsScrapTypeId = orig.IfsScrapTypeId;
            productionDetail.ParHandlingUnitID = orig.ParHandlingUnitID;
            productionDetail.OrderNo = orig.OrderNo;
            productionDetail.OperationNo = orig.OperationNo;
            productionDetail.CustomerBoxID = orig.CustomerBoxID;
            productionDetail.HandlingUnitChange = orig.HandlingUnitChange;
            productionDetail.ByProduct = orig.ByProduct;
            productionDetail.OperatorNote = orig.OperatorNote;
            productionDetail.QualityOperatorNote = orig.QualityOperatorNote;
            productionDetail.CrewSize = orig.CrewSize;
            productionDetail.CreatedAt = orig.CreatedAt;
            productionDetail.UpdatedAt = orig.UpdatedAt;
            productionDetail.Active = orig.Active;

            productionDetail.Barcode = orig.Barcode;
            productionDetail.Quantity = scrapAmount;
            orig.Quantity -= scrapAmount;
            if (orig.ManualInput < orig.Quantity)
                productionDetail.ManualInput = 0;
            else
            {
                productionDetail.ManualInput = orig.ManualInput - orig.Quantity;
                orig.ManualInput = orig.Quantity;
            }
            if (orig.Quantity >= orig.HandlingUnitQuantity)
            {
                productionDetail.HandlingUnitQuantity = 0;
                productionDetail.IfsReported = false;
            }
            else
            {
                productionDetail.HandlingUnitQuantity = orig.HandlingUnitQuantity - orig.Quantity;
                orig.HandlingUnitQuantity = orig.Quantity;
            }
            ShopOrderProductionDetailManager.Current.Update(orig);
            var res = ShopOrderProductionDetailManager.Current.Insert(productionDetail).ListData[0];
            ToolsMdiManager.frmOperatorActive.productionDetails.Add(res);

            this.DialogResult = DialogResult.OK;
        }

        private void BtnNumPad_Click(object sender, EventArgs e)
        {
            var title = MessageTextHelper.GetMessageText("000", "860", "Miktar giriniz", "Message");
            FrmNumericKeyboard frm = new FrmNumericKeyboard(title);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtScrapQuantity.Text = frm.value.ToString();
            }
        }
    }

}