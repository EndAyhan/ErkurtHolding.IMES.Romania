using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmSelectHandlingUnit : DevExpress.XtraEditors.XtraForm
    {
        public ProcessHandlingUnitModel selectedHandlingUnit;
        public FrmSelectHandlingUnit(List<ProcessHandlingUnitModel> handlingUnitModels, string ressource, string order)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            txtRessource.Text = ressource;
            txtOrder.Text = order;

            gcHandlingUnits.DataSource = handlingUnitModels;
        }

        private void gcHandlinbgUnits_DoubleClick(object sender, EventArgs e)
        {
            selectedHandlingUnit = (ProcessHandlingUnitModel)gvHandlingUnits.GetRow(gvHandlingUnits.FocusedRowHandle);

            this.DialogResult = DialogResult.OK;
        }
    }
}