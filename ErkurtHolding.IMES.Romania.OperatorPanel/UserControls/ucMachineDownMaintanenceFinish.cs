using ErkurtHolding.IMES.Business;
using ErkurtHolding.IMES.Business.ImesManager;
using ErkurtHolding.IMES.Entity.ImesDataModel;
using ErkurtHolding.IMES.Entity.Views;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Models;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucMachineDownMaintanenceFinish : DevExpress.XtraEditors.XtraUserControl
    {
        DateTime endDate;

        List<MtchngSptmDscvrs> mtchngSptmDscvrses;
        List<vw_InventoryStock> selectedStocks = new List<vw_InventoryStock>();
        vw_InventoryStock selectedStock;

        MachineDownProductModel selectedMachineDownProduct;
        List<MachineDownProductModel> machineDownProductList = new List<MachineDownProductModel>();

        Fault fault;
        List<FaultIssueDetail> faultIssueDetails = new List<FaultIssueDetail>();
        ActiveSeparate activeSeparate;
        UserModel userModel;
        Machine machine;
        public ucMachineDownMaintanenceFinish(Fault _fault, UserModel _userModel, Machine _machine)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            fault = _fault;
            userModel = _userModel;
            machine = _machine;
            activeSeparate = ActiveSeparateManager.Current.GetActiveSeparateBYIMES_ID(fault.Imes_ID);
            var prm = activeSeparate.woNo.CreateParameters("@WoNo");
            prm.Add("@Date", fault.RegisterDate);
            grpMain.Text = MessageTextHelper.ReplaceParameters(MessageTextHelper.GetMessageText("000", "866", "İş Emri No : @WoNo - Başlangıç Tarihi : @Date", "Message"), prm);
            InitData();
            gridControl1.DataSource = machineDownProductList;
        }

        #region INIT DATA
        private void InitData()
        {
            try
            {
                mtchngSptmDscvrses = MtchngSptmDscvrsManager.Current.GetMtchngSptmDscvrsesByBranchID(StaticValues.panel.BranchId);
                if (mtchngSptmDscvrses == null)
                {
                    mtchngSptmDscvrses = new List<MtchngSptmDscvrs>();
                }

                var groups = (from p in mtchngSptmDscvrses
                              group p by new { p.workTypeDesc, p.workTypeId }
                                     into g
                              select new { workTypeDesc = g.Key.workTypeDesc, workTypeId = g.Key.workTypeDesc }).ToList();


                glueMachineDown.Properties.DataSource = groups;

                List<EquipmentSerial> nodeEquipments;

                var parentEquipment = EquipmentSerialManager.Current.GetEquipmentSerialByMchCode((Guid)machine.BranchId, machine.alan1);
                nodeEquipments = EquipmentSerialManager.Current.GetEquipmentSerialBySupMchCode((Guid)machine.BranchId, machine.alan1);

                if (nodeEquipments == null)
                    nodeEquipments = new List<EquipmentSerial>();

                parentEquipment.groupId = "0";

                nodeEquipments.Add(parentEquipment);

                treeListLookUpEditEquipmentSerial.Properties.DataSource = nodeEquipments;
            }
            catch
            {

            }

        }

        #endregion

        #region EDITOR READ ONLY STATUS
        private void EditorReadOnly(bool readonlyStatus)
        {
            pnlControlCheckQuality.Visible = readonlyStatus;
            glueDiscoveryCode.ReadOnly = readonlyStatus;
            glueMachineDown.ReadOnly = readonlyStatus;
            glueSymptomCode.ReadOnly = readonlyStatus;
            treeListLookUpEditEquipmentSerial.ReadOnly = readonlyStatus;
            spnQuantity.ReadOnly = readonlyStatus;
            btnEditSelectedInventoryStock.ReadOnly = true;
            btnEditSelectedInventoryStock.Properties.Buttons[0].Visible = !readonlyStatus;
        }
        #endregion


        #region BUTTON EDIT CLICK EVENT
        private void brnEditSelectedInventoryStock_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmInventoryStock frmStock = new FrmInventoryStock();
            if (frmStock.ShowDialog() == DialogResult.OK)
            {
                if (machineDownProductList.Any(x => x.ProductId == frmStock.selectedStock.Id))
                {
                    ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "971", "Mevcut lot numarası veya barkod numarası daha önceden tamamlanmıştır", "Message"));
                    return;
                }
                selectedStock = frmStock.selectedStock;
                btnEditSelectedInventoryStock.Text = frmStock.selectedStock.description;
            }
        }
        #endregion


        #region INSERT / REMOVE BUTTON CLICK EVENT

        private void barBtnInsert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (selectedStock == null)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "972", "Öncelikle stok seçmelisiniz", "Message"));
                return;
            }

            if (spnQuantity.Value == 0)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "973", "Adet bilgisi girmelisiniz", "Message"));
                return;
            }

            try
            {
                selectedMachineDownProduct = new MachineDownProductModel();
                selectedMachineDownProduct.ProductId = selectedStock.Id;
                selectedMachineDownProduct.ProductDescription = selectedStock.description;
                selectedMachineDownProduct.PartNo = selectedStock.PartNo;
                selectedMachineDownProduct.Quantity = (double)spnQuantity.Value;

                selectedStocks.Add(selectedStock);
                machineDownProductList.Add(selectedMachineDownProduct);


                btnEditSelectedInventoryStock.Text = "";
                spnQuantity.Value = 0;
                gridControl1.BeginInit();
                gridControl1.DataSource = machineDownProductList;
                gridControl1.EndInit();
                gridControl1.Refresh();
                gridView1.RefreshData();
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        private void barBtnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedRows = gridView2.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "976", "Öncelikle çıkartmak istediğiniz malzemeyi seçmelisiniz", "Message"));
                return;
            }
            List<MachineDownProductModel> rowsList = new List<MachineDownProductModel>();
            List<vw_InventoryStock> rowsStock = new List<vw_InventoryStock>();
            foreach (var selectedRow in selectedRows)
            {
                rowsList.Add(machineDownProductList[selectedRow]);
                rowsStock.Add(selectedStocks[selectedRow]);
            }
            foreach (var selectedRow in rowsList)
            {
                machineDownProductList.Remove(selectedRow);
            }
            foreach (var selectedRow in rowsStock)
            {
                selectedStocks.Remove(selectedRow);
            }
            gridControl1.Refresh();
            gridView2.RefreshData();
            gridView2.ClearSelection();
        }

        #endregion

        #region FINISH BUTTON CLICK EVENT
        private void barBtnFinish_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InitData();

            if (glueMachineDown.EditValue == null || glueMachineDown.Text == "")
            {
                ToolsMessageBox.Warning(this, MessageTextHelper.GetMessageText("000", "977", "Arıza sebep bilgisi girilmesi zorunludur", "Message"));
                return;
            }

            EditorReadOnly(true);
            pnlControlCheckQuality.Visible = true;
        }
        #endregion

        #region QUALITY CONTROL BUTTONS CLICK EVENT (CHECK - REJECT)
        private void btnCheck_Click(object sender, EventArgs e)
        {
            FrmUserLogin frmLogin = new FrmUserLogin(false);
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {

                if (selectedStocks.Count > 0)
                {
                    foreach (var item in selectedStocks)
                    {
                        if (faultIssueDetails.Any(x => x.InventoryStockID == item.Id && x.IfsSend))
                            continue;

                        FaultIssueDetail faultIssueDetail = new FaultIssueDetail();
                        faultIssueDetail.FaultID = fault.Id;
                        faultIssueDetail.SignatureUserID = ToolsMdiManager.frmOperatorActive.machineDownFinishUser.CompanyPersonId;
                        faultIssueDetail.BranchID = item.BranchID;
                        faultIssueDetail.InventoryStockID = item.Id;
                        faultIssueDetail.Quantity = machineDownProductList.First(x => x.ProductId == item.Id).Quantity;
                        faultIssueDetail.LotBatchNo = item.lotBatchNo;
                        faultIssueDetail.Barcode = item.alan1;
                        faultIssueDetail.Location = item.locationNo;
                        faultIssueDetail.IfsSend = true;
                        var result = FaultIssueDetailManager.Current.Insert(faultIssueDetail);
                        faultIssueDetails.Add(faultIssueDetail);
                    }
                }

                fault.wo_no = activeSeparate.woNo.ToString();
                fault.QualityCheckDate = DateTime.Now;
                fault.QualityCheckUser = frmLogin.userModel.CompanyPersonId;
                fault.QualityUserNote = txtQualtyUserNote.Text;
                fault.InterventionEndDate = endDate;
                fault.TechStaffDescription = txtDescription.Text;
                fault.ActualFinishDate = DateTime.Now;
                fault.ErrDiscoveryCode = glueDiscoveryCode.EditValue == null || glueDiscoveryCode.Text == "" ? "KSF" : glueDiscoveryCode.Text;
                fault.ErrSymptomCode = glueSymptomCode.EditValue == null || glueSymptomCode.Text == "" ? "SMT" : glueSymptomCode.Text;
                fault.Active = false;
                try
                {
                    var mtchngSptmDscvrs = mtchngSptmDscvrses.First(x => x.workTypeDesc == glueMachineDown.EditValue.ToString());
                    fault.WorkTypeID = mtchngSptmDscvrs.workTypeId;
                    fault.ErrDescription = $"{mtchngSptmDscvrs.workTypeDesc} {fault.ErrDescription.Substring(fault.ErrDescription.IndexOf('-'))}";
                }
                catch { }
                FaultManager.Current.Update(fault);
                ToolsMdiManager.frmOperatorActive.RefreshFaultGridModel();

                btnCheck.Enabled = false;
                barBtnInsert.Enabled = false;
                barBtnFinish.Enabled = false;
                barBtnRemove.Enabled = false;
                btnRehjection.Enabled = false;
                timer2.Enabled = false;
                timer2.Stop();
                txtDescription.ReadOnly = true;
                txtQualtyUserNote.ReadOnly = true;

            }
        }

        private void btnRehjection_Click(object sender, EventArgs e)
        {
            FrmUserLogin frmLogin = new FrmUserLogin(true);
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                EditorReadOnly(false);
                pnlControlCheckQuality.Visible = false;
            }
        }
        #endregion

        private void pnlControlCheckQuality_VisibleChanged(object sender, EventArgs e)
        {
            if (pnlControlCheckQuality.Visible)
            {
                endDate = DateTime.Now;
                lblStartDate.Text = endDate.ToString("dd/MM/yyyy HH:mm:ss");
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }
        }

        bool darkred = false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (darkred)
            {

                lblDuration.ForeColor = panel1.BackColor = System.Drawing.Color.Red;
                darkred = false;
            }
            else
            {
                panel1.BackColor = System.Drawing.SystemColors.HotTrack;
                darkred = true;
            }

            lblDuration.Text = (endDate - DateTime.Now).ToString(@"dd\.hh\:mm\:ss");
        }


        #region EDIT VALUE CHANGE EVENTS
        string selectedWorkTypeId;

        private void treeListLookUpEditEquipmentSerial_EditValueChanged(object sender, EventArgs e)
        {

            InitDataDiscoveryCode();

        }
        private void glueMachineDown_EditValueChanged(object sender, EventArgs e)
        {
            InitDataDiscoveryCode();
        }

        private void glueDiscoveryCode_EditValueChanged(object sender, EventArgs e)
        {
            if (glueDiscoveryCode.EditValue == null || glueDiscoveryCode.Text == "")
                glueSymptomCode.ReadOnly = true;
            else
            {
                EquipmentSerial result = (EquipmentSerial)treeListLookUpEditEquipmentSerial.GetSelectedDataRow();
                selectedWorkTypeId = glueMachineDown.EditValue.ToString();
                var selectedDiscovery = glueDiscoveryCode.EditValue.ToString();

                var symptomes = mtchngSptmDscvrses.Where(x => x.workTypeDesc == selectedWorkTypeId && x.mchType == result.mchType && x.categoryId == result.categoryId && x.errDiscoverCode == selectedDiscovery).ToList();
                glueSymptomCode.Properties.DataSource = symptomes;
                glueSymptomCode.ReadOnly = false;
            }
        }
        #endregion

        private void InitDataDiscoveryCode()
        {
            try
            {
                EquipmentSerial result = (EquipmentSerial)treeListLookUpEditEquipmentSerial.GetSelectedDataRow();
                selectedWorkTypeId = glueMachineDown.EditValue.ToString();
                if (result == null)
                    return;
                var discoveryCodes = mtchngSptmDscvrses.Where(x => x.workTypeDesc == selectedWorkTypeId && x.mchType == result.mchType && x.categoryId == result.categoryId).ToList();

                var group = (from p in discoveryCodes
                             group p by new { p.errDiscoverCode, p.errDiscoverCodeDesc }
                                 into g
                             select new { errDiscoverCode = g.Key.errDiscoverCode, errDiscoverCodeDesc = g.Key.errDiscoverCodeDesc }).ToList();

                glueDiscoveryCode.Properties.DataSource = group;

                glueDiscoveryCode.EditValue = null;
                glueSymptomCode.EditValue = null;
            }
            catch
            {
            }
        }
    }
}
