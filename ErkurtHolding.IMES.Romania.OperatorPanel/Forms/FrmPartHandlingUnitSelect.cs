using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    public partial class FrmPartHandlingUnitSelect : DevExpress.XtraEditors.XtraForm
    {
        List<PartHandlingUnit> partHandlingUnits;
        public PartHandlingUnit partHandlingUnit { get; set; }
        public FrmPartHandlingUnitSelect(List<PartHandlingUnit> _partHandlingUnits)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            var prm = _partHandlingUnits[0].PartNo.CreateParameters("@PartNo");
            this.Text = ToolsMessageBox.ReplaceParameters("@PartNo İÇİN TAŞIMA KASASI SEÇİM EKRANI", prm);
            partHandlingUnits = _partHandlingUnits;
            gcMain.DataSource = partHandlingUnits;
        }

        private void FrmPartHandlingUnitSelect_Load(object sender, EventArgs e)
        {

        }

        private void gvMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvMain.FocusedRowHandle < 0)
                return;
            partHandlingUnit = (PartHandlingUnit)gvMain.GetRow(gvMain.FocusedRowHandle);
        }

        private void barLargeButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (partHandlingUnit == null)
            {
                ToolsMessageBox.Warning(this, "Taşıma kasası seçmeden devam edemezsiniz");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}