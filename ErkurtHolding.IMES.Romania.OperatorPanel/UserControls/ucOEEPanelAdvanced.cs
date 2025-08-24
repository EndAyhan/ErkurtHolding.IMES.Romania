using DevExpress.Utils.Extensions;
using ErkurtHolding.IMES.Business.ReportManagers;
using ErkurtHolding.IMES.Entity.Reports;
using ErkurtHolding.IMES.Romania.OperatorPanel.Enums;
using ErkurtHolding.IMES.Romania.OperatorPanel.Forms;
using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using ErkurtHolding.IMES.Romania.OperatorPanel.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    public partial class ucOEEPanelAdvanced : DevExpress.XtraEditors.XtraUserControl
    {
        private Color green = Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(191)))), ((int)(((byte)(85)))));
        private Color yellow = Color.Goldenrod;
        private Color red = Color.OrangeRed;
        private Color gray = Color.Gray;

        public FrmOperator frmOperator { get; set; }

        public ucOEEPanelAdvanced()
        {
            InitializeComponent();

            ccQuality.Series[0].Points[0].Tag = "100";
            ccQuality.Series[0].Points[1].Tag = "101";

            ccPerformance.Series[0].Points[0].Tag = "102";
            ccPerformance.Series[0].Points[1].Tag = "103";

            ccAvailability.Series[0].Points[0].Tag = "104";
            ccAvailability.Series[0].Points[1].Tag = "105";
            ccAvailability.Series[0].Points[2].Tag = "106";

            LanguageHelper.InitializeLanguage(this);

            if (StaticValues.HideOEEPanel == "TRUE")
            {
                var pbErkurtHolding = new PictureBox();
                pbErkurtHolding.Image = global::ErkurtHolding.IMES.Romania.OperatorPanel.Properties.Resources.EH1;
                pbErkurtHolding.Location = new System.Drawing.Point(3, 3);
                pbErkurtHolding.Name = "pbErkurtHolding";
                pbErkurtHolding.Size = panelControl2.Size;
                pbErkurtHolding.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                panelControl2.AddControl(pbErkurtHolding);
                pbErkurtHolding.BringToFront();
            }

            Clear();
        }
        private void tmrOEE_Tick(object sender, EventArgs e)
        {
            RefreshOEEValues();
        }

        public void RefreshOEEValues()
        {
            try
            {
                if (frmOperator == null || frmOperator.shopOrderProduction == null || frmOperator.shopOrderStatus != ShopOrderStatus.End)
                {
                    Clear();
                    return;
                }

                if (StaticValues.opcClient.clientConnect == false)
                    StaticValues.opcClient.clientConnect = true;
                double sure = 0;
                double totalMinutes = (DateTime.Now - frmOperator.shopOrderProduction.OrderStartDate).TotalMinutes;
                if (frmOperator.panelDetail.OPCNodeIdSure != null && frmOperator.panelDetail.OPCNodeIdSure != "")
                {
                    if (double.TryParse(StaticValues.opcClient.ReadNode(frmOperator.panelDetail.OPCNodeIdSure), out sure))
                        sure /= 60;
                }
                else
                    sure = totalMinutes;
                double totalPlannedInterruptions = 0;
                double totalUnplannedInterruptions = 0;
                double totalFaults = 0;

                var interruptions = frmOperator.interruptionGridModel.interruptionCauseGridModels.Where(x => x.ShopOrderProductionID == frmOperator.shopOrderProduction.Id && x.InterruptionStartDate > frmOperator.shopOrderProduction.OrderStartDate);
                if (interruptions != null)
                {
                    totalPlannedInterruptions = interruptions.Where(x => x.operation_cause_alan5 == "TRUE").Sum(x => ((x.InterruptionFinishDate.HasValue ? x.InterruptionFinishDate : DateTime.Now) - x.InterruptionStartDate).GetValueOrDefault().TotalMinutes);
                    totalUnplannedInterruptions = interruptions.Where(x => x.operation_cause_alan5 == "FALSE").Sum(x => ((x.InterruptionFinishDate.HasValue ? x.InterruptionFinishDate : DateTime.Now) - x.InterruptionStartDate).GetValueOrDefault().TotalMinutes);
                }
                //18.07.2024 tarihinde yorum alınan satır değiştirilmiş olup aşağıdaki gibi listelenmiştir
                //totalFaults = frmOperator.faults.Where(x => x.RegisterDate > frmOperator.shopOrderProduction.OrderStartDate).Sum(x => ((x.ActualFinishDate > x.RegisterDate ? x.ActualFinishDate : DateTime.Now) - x.RegisterDate).TotalMinutes) * frmOperator.shopOrderOperations.Count;
                totalFaults = frmOperator.faults.Where(x => x.RegisterDate > frmOperator.shopOrderProduction.OrderStartDate).Sum(x => ((x.ActualFinishDate > x.RegisterDate ? x.ActualFinishDate : DateTime.Now) - x.RegisterDate).TotalMinutes);
                totalPlannedInterruptions *= (sure / totalMinutes);
                totalUnplannedInterruptions *= (sure / totalMinutes);
                totalFaults *= (sure / totalMinutes);
                var netPlannedTime = sure - totalPlannedInterruptions;
                var netTime = netPlannedTime - totalUnplannedInterruptions - totalFaults;

                // If not proses then netTime needs to be multiplied with how many workorders are running atm
                if (!frmOperator.shopOrderProduction.Process)
                {
                    netTime *= frmOperator.shopOrderOperations.Count;
                    netPlannedTime *= frmOperator.shopOrderOperations.Count;
                    //totalFaults *= frmOperator.shopOrderOperations.Count;// bunu daha sonra değerlendircez
                }

                double targetTime = 0;
                double targetQuantity = 0;
                double plcQuantity = 0;
                double totalQuantity = 0;
                double totalScrapQuantity = 0;
                double stdTime = 0;
                foreach (var shopOrderOperation in frmOperator.shopOrderOperations)
                {
                    stdTime = shopOrderOperation.machRunFactor * 60;
                    var shopOrderQuantity = (double)frmOperator.productionDetails.Where(x => x.ShopOrderOperationID == shopOrderOperation.Id && x.Active == true && x.ShopOrderProductionID == frmOperator.shopOrderProduction.Id).Sum(y => y.Quantity);
                    targetTime += stdTime * shopOrderQuantity; //bu parçayı üretmen gereken gerçek süre dk
                    totalQuantity += shopOrderQuantity;
                    targetQuantity += (netTime / frmOperator.shopOrderOperations.Count) / stdTime; // bu zamanda üretmen gereken gerçek miktar
                }
                totalScrapQuantity = (double)frmOperator.productionDetails.Where(x => x.ShopOrderProductionID == frmOperator.shopOrderProduction.Id && x.Active == true && x.ProductionStateID != StaticValues.specialCodeOk.Id).Sum(y => y.Quantity);

                double carpan = 1;
                double.TryParse(frmOperator.shopOrderOperations[0].alan1, out carpan);
                if (frmOperator.processNewActive)
                {
                    plcQuantity = (double)frmOperator.productionDetails.Where(x => x.ShopOrderProductionID == frmOperator.shopOrderProduction.Id && x.Active == true).Sum(y => y.Quantity - y.ManualInput) / carpan;
                }
                else
                {
                    plcQuantity = (double)frmOperator.productionDetails.Where(x => x.ShopOrderOperationID == frmOperator.shopOrderOperations[0].Id && x.ShopOrderProductionID == frmOperator.shopOrderProduction.Id && x.Active == true).Sum(y => y.Quantity - y.ManualInput) / carpan;
                }

                var performance = netTime == 0 ? 0 : targetTime / netTime * 100;
                var realPerformance = performance;
                if (performance > 100) performance = 100;
                pPerformance.BackColor = getColor(performance);
                ascPerformance.Value = (float)performance;
                lblPerformance.Text = $"{performance:n0} %";
                siPerformance.StateIndex = getSi(performance);
                //ccPerformance.Series[0].Points[0].Values[0] = Math.Round(targetTime, 0);
                //ccPerformance.Series[0].Points[1].Values[0] = Math.Round(netTime, 0);
                ccPerformance.Series[0].Points[0].Values[0] = Math.Round(targetQuantity, 0);
                ccPerformance.Series[0].Points[1].Values[0] = Math.Round(totalQuantity, 0);
                ccPerformance.RefreshData();
                frmOperator.SetProductionPerformanceChart(targetQuantity, totalQuantity);

                var availability = netPlannedTime == 0 ? 0 : netTime / (netPlannedTime) * 100;
                pAvailability.BackColor = getColor(availability);
                ascAvailability.Value = (float)availability;
                lblAvailability.Text = $"{availability:n0} %";
                siAvailability.StateIndex = getSi(availability);
                ccAvailability.Series[0].Points[0].Values[0] = Math.Round(sure, 0);
                ccAvailability.Series[0].Points[1].Values[0] = Math.Round(netTime, 0);
                ccAvailability.Series[0].Points[2].Values[0] = Math.Round(totalPlannedInterruptions, 0);
                ccAvailability.RefreshData();

                double quality = 100;
                if (totalQuantity > 0)
                    quality = ((totalQuantity - totalScrapQuantity) / totalQuantity) * 100;
                pQuality.BackColor = getColor(quality);
                ascQuality.Value = (float)quality;
                lblQuality.Text = $"{quality:n0} %";
                siQuality.StateIndex = getSi(quality);
                ccQuality.Series[0].Points[0].Values[0] = Math.Round(totalQuantity, 0);
                ccQuality.Series[0].Points[1].Values[0] = Math.Round(totalScrapQuantity, 0);
                ccQuality.RefreshData();

                float oee = 0;
                float realOee = (float)(realPerformance * availability * quality / 10000);
                if (realOee > 100)
                    oee = 100;
                else oee = realOee;
                pOEE.BackColor = getColor(oee);
                ascOEE.Value = oee;
                lblOEE.Text = $"{oee:n0} %";
                siOEE.StateIndex = getSi(oee);

                if (frmOperator.resourceStatus != null)
                {
                    var resourceOEEValues = new ResourceOEEValues()
                    {
                        MachineId = frmOperator.resource.Id,
                        ShopOrderProductionId = frmOperator.shopOrderProduction.Id,
                        PLCCounter = plcQuantity,
                        PlannedInterruptionDuration = totalPlannedInterruptions,
                        UnplannedInterruptionDuration = totalUnplannedInterruptions,
                        FaultDuration = totalFaults,
                        Availability = availability,
                        Performance = realPerformance,
                        Quality = quality,
                        OEE = realOee
                    };
                    ResourceOEEValuesManager.Current.Insert(resourceOEEValues);
                }
                else
                    frmOperator.resourceStatus = ResourceStatusManager.Current.GetActiveResourceStatusByMachineIdAndShopOrderProductionId(frmOperator.resource.Id, frmOperator.shopOrderProduction.Id, (int)ResourceWorkingStatus.Working);
            }
            catch (Exception ex)
            {
                ToolsMessageBox.Error(this, ex);
            }
        }

        public void Clear()
        {
            pOEE.BackColor = gray;
            pAvailability.BackColor = gray;
            pPerformance.BackColor = gray;
            pQuality.BackColor = gray;

            ascOEE.Value = 0;
            ascAvailability.Value = 0;
            ascPerformance.Value = 0;
            ascQuality.Value = 0;

            lblOEE.Text = "-";
            lblAvailability.Text = "-";
            lblPerformance.Text = "-";
            lblQuality.Text = "-";

            siOEE.StateIndex = 0;
            siAvailability.StateIndex = 0;
            siPerformance.StateIndex = 0;
            siQuality.StateIndex = 0;

            ccAvailability.Series[0].Points[0].Values[0] = 0;
            ccAvailability.Series[0].Points[1].Values[0] = 0;
            ccAvailability.Series[0].Points[2].Values[0] = 0;
            ccAvailability.RefreshData();

            ccPerformance.Series[0].Points[0].Values[0] = 0;
            ccPerformance.Series[0].Points[1].Values[0] = 0;
            if (frmOperator != null)
                frmOperator.SetProductionPerformanceChart(0, 0);
            ccPerformance.RefreshData();

            ccQuality.Series[0].Points[0].Values[0] = 0;
            ccQuality.Series[0].Points[1].Values[0] = 0;
            ccQuality.RefreshData();
        }

        private Color getColor(double value)
        {
            if (value <= 70)
                return red;
            else if (value > 70 && value < 85)
                return yellow;
            return green;
        }

        private int getSi(double value)
        {
            if (value <= 70)
                return 1;
            else if (value > 70 && value < 85)
                return 2;
            return 3;
        }
    }
}
