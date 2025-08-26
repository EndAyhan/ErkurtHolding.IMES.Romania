using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Components;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Extensions;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Helpers;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucMachineAutoMaintanenceList : DevExpress.XtraEditors.XtraUserControl
    {
        FrmOperator frmOperator;
        List<MachineDocumentList> machineMaintenanceDocument;
        public List<MachineAutonomousList> machineAutonomousLists { get; set; }
        public ucMachineAutoMaintanenceList(FrmOperator frmOperator)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            try
            {
                this.frmOperator = frmOperator;
                FillMyEditors();
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, MessageTextHelper.GetMessageText("000", "969", "IFS servisine ulaşılamıyor. Lütfen sistem yöneticinize başvurun", "Message"), ex);
            }
        }

        public void FillMyEditors()
        {
            var machineAutonomousLists = MachineAutoMaintanenceManager.Current.GetMachineAutoMaintenenceList(frmOperator.machine.Id);

            if (machineAutonomousLists.HasEntries())
            {
                foreach (var item in machineAutonomousLists)
                {
                    List<AutoMaintanenceCheckModel> maintanenceCheckModels = new List<AutoMaintanenceCheckModel>();
                    AutoMaintanenceCheckModel checkModel = new AutoMaintanenceCheckModel();

                    checkModel.Id = item.Id;
                    checkModel.MaintenanceDescription = item.MaintenanceDescription;

                    maintanenceCheckModels.Add(checkModel);

                    ucMachineAutoMaintanenceCheckListComponent checkList = new ucMachineAutoMaintanenceCheckListComponent();
                    checkList.checkMaintanenceList = maintanenceCheckModels;
                    checkList.setAutoMaintanenceModel = maintanenceCheckModels.FirstOrDefault();
                    flowLayoutPanel2.Controls.Add(checkList);
                }
            }
        }

        private void AutoMaintanenceCheckList_Load(object sender, EventArgs e)
        {
            var opInterruptionCause = OpInterruptionCauseManager.Current.GetOpInterruptionCauseByCauseId(StaticValues.panel.BranchId, frmOperator.machine.Id, StaticValues.autoMaintenanceInterruptionCauseId);

            var unFinishedInterruptionCause = InterruptionCauseManager.Current.GetUnfinishedAutoMaintananceInterruptionCauseByMachineId(frmOperator.machine.Id, opInterruptionCause.Id);

            //document varlık kontrolü yapılacak!!
            var documentType = SpecialCodeManager.Current.GetSpecialCodeByName("Otonom Bakım", 25);
            machineMaintenanceDocument = MachineDocumentManager.Current.GetMachineDocumentList(frmOperator.machine.Id, documentType.Id);

            if (machineMaintenanceDocument.HasEntries())
                btnViewPdf.Visible = true;

            if (!unFinishedInterruptionCause.HasEntries())
            {
                InterruptionCause interruption = new InterruptionCause()
                {
                    ShiftID = StaticValues.shift.Id,
                    CompanyPersonID = frmOperator.Users.FirstOrDefault().CompanyPersonId,//activeProduction.StartCompanyPersonID,//frmOperator.Users.FirstOrDefault().CompanyPersonId,//ToolsMdiManager.frmOperatorActive.interrupstionCouseStartUser.CompanyPersonId,
                    WorkCenterID = frmOperator.machine.Id,
                    CouseID = opInterruptionCause.Id,
                    Description = MessageTextHelper.GetMessageText("000", "864", "OTOMATİK GÜNLÜK OTONOM BAKIM DURUŞU", "Message"),
                    InterruptionStartDate = DateTime.Now,
                    State = true,
                    OnlineState = frmOperator.shopOrderStatus != ShopOrderStatus.Start
                };

                InterruptionCauseManager.Current.Insert(interruption);
                InterruptionCauseHelper.CreateInterruption(frmOperator, interruption, opInterruptionCause);
                frmOperator.interruptionCause = interruption;
                frmOperator.interrupstionCouseStartUser = frmOperator.Users.FirstOrDefault();
            }
            else
                frmOperator.interruptionCause = unFinishedInterruptionCause.FirstOrDefault();


            frmOperator.interruptionCauseOptions = InterruptionCauseOptions.AutoMaintenance;

            List<vw_ShopOrderGridModel> shopOrderGridModels;
            shopOrderGridModels = frmOperator.vw_ShopOrderGridModels;

            if (frmOperator.shopOrderStatus != ShopOrderStatus.Start && opInterruptionCause.countPLC == "FALSE")
            {
                StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, true);
                if (frmOperator.panelDetail.OPCNodeIdInterruption != null && frmOperator.panelDetail.OPCNodeIdInterruption != "")
                    StaticValues.opcClient.WriteNode(frmOperator.panelDetail.OPCNodeIdInterruption, true);//jet resourcelerde duruş bilgisi gönderme
            }
            if (frmOperator.shopOrderStatus == ShopOrderStatus.Start && opInterruptionCause.countPLC == "TRUE")
                StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, false);
        }

        public List<AutoMaintanenceCheckModel> autoMaintanenceChecks = new List<AutoMaintanenceCheckModel>();
        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            autoMaintanenceChecks.Clear();
            bool checklistFlag = true;

            foreach (var item in flowLayoutPanel2.Controls)
            {
                if (item is ucMachineAutoMaintanenceCheckListComponent)
                {
                    ucMachineAutoMaintanenceCheckListComponent checklist = (ucMachineAutoMaintanenceCheckListComponent)item;
                    if (checklist.check)
                    {
                        autoMaintanenceChecks.Add(checklist.selected);
                    }
                    else
                        checklistFlag = false;
                }
            }

            if (!checklistFlag)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "970", "Gerekli işlemlerin hepsi onaylanmamıştır", "Message"));
            }
            else
            {
                frmOperator.container.Visible = false;

                //bu production'da AutonomousMaintenanceIsChecked true edilecek!!
                frmOperator.shopOrderProduction.AutonomousMaintenanceIsChecked = true;//db
                ShopOrderProductionManager.Current.UpdateIsAutoMaintenanceChecked(frmOperator.shopOrderProduction);//db

                StaticValues.opcClient.MachineLock(frmOperator.panelDetail.OPCNodeIdMachineControl, false);

                //TO DO: revize edilecek!!
                //InterruptionCauseManager.Current.GetUnfinishedInterruptionCausesByMachineId(ToolsMdiManager.frmOperatorActive.machine.Id);
                InterruptionCauseHelper.StopInterruption(frmOperator, frmOperator.Users.FirstOrDefault());
            }
        }

        private void btnViewPdf_Click(object sender, EventArgs e)
        {
            string documentFilePath = machineMaintenanceDocument.FirstOrDefault().FilePdfPath;
            FrmPdfViewer frmPdfViewer = new FrmPdfViewer(documentFilePath);
            frmPdfViewer.ShowDialog();
        }
    }

}
