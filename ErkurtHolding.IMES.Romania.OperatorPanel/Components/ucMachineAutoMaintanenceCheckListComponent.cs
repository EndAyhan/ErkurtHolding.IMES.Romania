using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using System.Collections.Generic;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Components
{
    public partial class ucMachineAutoMaintanenceCheckListComponent : DevExpress.XtraEditors.XtraUserControl
    {
        public ucMachineAutoMaintanenceCheckListComponent()
        {
            InitializeComponent();
        }

        public AutoMaintanenceCheckModel selected
        {
            get
            {
                return (AutoMaintanenceCheckModel)glueComponents.GetSelectedDataRow();
            }
        }

        private List<AutoMaintanenceCheckModel> CheckMaintanenceList;
        public List<AutoMaintanenceCheckModel> checkMaintanenceList
        {
            get
            {
                if (CheckMaintanenceList == null)
                    CheckMaintanenceList = new List<AutoMaintanenceCheckModel>();
                return CheckMaintanenceList;
            }
            set
            {
                glueComponents.Properties.DataSource = value;
                CheckMaintanenceList = value;
            }
        }

        private AutoMaintanenceCheckModel SetAutoMaintanenceModel;

        public AutoMaintanenceCheckModel setAutoMaintanenceModel
        {
            get
            {
                return SetAutoMaintanenceModel;
            }
            set
            {
                if (value != null)
                    glueComponents.EditValue = value.Id;
                SetAutoMaintanenceModel = value;
            }
        }

        public bool check
        {
            get { return chkCheck.Checked; }
        }
        public string description { get; set; }
    }

}
