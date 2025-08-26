using DevExpress.XtraEditors;
using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmNumericKeyboard : DevExpress.XtraEditors.XtraForm
    {
        public decimal value { get; set; } = 0;
        public decimal MaxValue = 0;
        vw_ShopOrderGridModel shopOrder = null;
        List<PartHandlingUnit> partHandlingUnits;
        public PartHandlingUnit selectedPartHandlingUnit { get; set; }
        //private Product selectedProduct;
        public FrmNumericKeyboard(string header)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            lblHandlingUnitType.Visible = false;
            gluePartHandlingUnit.Visible = false;
            gcNumpad.Text = header;
        }
        public FrmNumericKeyboard(vw_ShopOrderGridModel _shopOrder)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            shopOrder = _shopOrder;

            try
            {
                var selectedProduct = ToolsMdiManager.frmOperatorActive.products.Single(x => x.Id == shopOrder.ProductID);

                partHandlingUnits = PartHandlingUnitManager.Current.GetPartHandlingUnit(selectedProduct.PartNo);
                gluePartHandlingUnit.Properties.DataSource = partHandlingUnits;
                selectedPartHandlingUnit = ToolsMdiManager.frmOperatorActive.partHandlingUnits.First(x => x.PartNo == selectedProduct.PartNo);
                gluePartHandlingUnit.EditValue = selectedPartHandlingUnit.Id;
                MaxValue = (decimal)selectedPartHandlingUnit.MaxQuantityCapacity;
                var prm = MaxValue.CreateParameters("@MaxValue");
                gcNumpad.Text = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("000", "845", "Kasa içi adet @MaxValue 'den büyük olamaz", "Message"), prm);

            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }
        private void btnNumber_Click(object sender, EventArgs e)
        {
            txtCode.Text += (sender as SimpleButton).Text;
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            if (txtCode.Text.Length > 0)
                txtCode.Text = txtCode.Text.Remove(txtCode.Text.Length - 1, 1);
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            //if(txtCode.Text=="")
            //{
            //    return;
            //}
            //else if(txtCode.Text.Contains('.'))
            //{
            //    return;
            //}
            //else
            //{
            //    txtCode.Text += ".0";
            //}
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                value = Convert.ToDecimal(txtCode.Text);
                if (shopOrder != null && value > MaxValue)
                {
                    var prm = MaxValue.CreateParameters("@MaxValue");
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "910", "Kasa içi adet @MaxValue 'den büyük olamaz", "Message"), prm);
                    return;
                }
                else if (shopOrder != null && value == 0)
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "911", "Kasa içi Miktar sıfırdan fazla olmak zorunda", "Message"));
                    return;
                }
                else
                    this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "912", "Sayısal ifade girmelisiniz", "Message"));
            }
        }

        private void FrmNumericKeyboard_Load(object sender, EventArgs e)
        {

        }

        private void gluePartHandlingUnit_EditValueChanged(object sender, EventArgs e)
        {
            if (shopOrder != null)
            {
                selectedPartHandlingUnit = partHandlingUnits.First(x => x.Id == (Guid)gluePartHandlingUnit.EditValue);
                var prm = selectedPartHandlingUnit.MaxQuantityCapacity.CreateParameters("@MaxValue");
                gcNumpad.Text = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("000", "845", "Kasa içi adet @MaxValue 'den büyük olamaz", "Message"), prm);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                return;
            }
            else if (txtCode.Text.Contains('.'))
            {
                return;
            }
            else
            {
                txtCode.Text += ".";
            }
        }
    }

}