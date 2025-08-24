using DevExpress.XtraBars;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Main
{
    public partial class FrmFillHandlingUnit : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FrmFillHandlingUnit()
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);
        }
    }
}