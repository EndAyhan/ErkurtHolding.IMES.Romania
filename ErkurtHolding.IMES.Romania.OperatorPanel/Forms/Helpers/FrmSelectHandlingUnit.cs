using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System;
using System.Collections.Generic;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmSelectHandlingUnit : DevExpress.XtraEditors.XtraForm
    {
        public ProcessHandlingUnitModel selectedHandlingUnit;
        public FrmSelectHandlingUnit(List<ProcessHandlingUnitModel> handlingUnitModels, string ressource, string order)
        {
            InitializeComponent();

            var t = new JsonText();
            FormLocalizer.Localize(this, t);

            txtRessource.Text = ressource;
            txtOrder.Text = order;

            gcHandlingUnits.DataSource = handlingUnitModels;
        }

        private void gcHandlinbgUnits_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}