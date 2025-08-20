namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
{
    partial class FrmOperator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint("Üretim", new object[] {
            ((object)(0D))});
            DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint("Hedef", new object[] {
            ((object)(0D))});
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repositoryItemTimeEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.repositoryItemDateEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repositoryItemTimeEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.oeePanel = new ErkurtHolding.IMES.Romania.OperatorPanel.UserControls.ucOEEPanelAdvanced();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gcTotalProductionAmount = new DevExpress.XtraGauges.Win.GaugeControl();
            this.lgTotalProduction = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearGauge();
            this.lsrbcTotalProduction = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleRangeBarComponent();
            this.lscTotalProduction = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblPlcLabel = new DevExpress.XtraEditors.LabelControl();
            this.lblPLC = new DevExpress.XtraEditors.LabelControl();
            this.lblTotalProductionCountLabel = new DevExpress.XtraEditors.LabelControl();
            this.lblTotalProductionCount = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrentAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.lblBoxAmount = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrentAmount = new DevExpress.XtraEditors.LabelControl();
            this.lblRealizeAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.lblBoxAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.lblRealizeAmount = new DevExpress.XtraEditors.LabelControl();
            this.lblScrapCount = new DevExpress.XtraEditors.LabelControl();
            this.lblScrapCountLabel = new DevExpress.XtraEditors.LabelControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.ccProductionPerformance = new DevExpress.XtraCharts.ChartControl();
            this.gcTargetProductionAmount = new DevExpress.XtraGauges.Win.GaugeControl();
            this.lgTargetProduction = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearGauge();
            this.lsrbcTargetProduction = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleRangeBarComponent();
            this.lscTargetProduction = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent();
            this.pnlReadValueLabels = new DevExpress.XtraEditors.PanelControl();
            this.lblValue9 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription9 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue8 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription8 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue7 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription7 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue6 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl35 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue5 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl40 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue4 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl48 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue3 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl45 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue2 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl42 = new DevExpress.XtraEditors.LabelControl();
            this.lblValue1 = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl38 = new DevExpress.XtraEditors.LabelControl();
            this.container = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.lblWorkCenterStartTime = new DevExpress.XtraEditors.LabelControl();
            this.lblWorkOrderStopWatch = new DevExpress.XtraEditors.LabelControl();
            this.lblMachineState = new DevExpress.XtraEditors.LabelControl();
            this.pnlMachineStateColor = new DevExpress.XtraEditors.PanelControl();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.lblProcessBarcode = new DevExpress.XtraEditors.LabelControl();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.xtcDetails = new DevExpress.XtraTab.XtraTabControl();
            this.xtpWorkShopOrder = new DevExpress.XtraTab.XtraTabPage();
            this.gcWorkShopOrder = new DevExpress.XtraGrid.GridControl();
            this.gwWorkShopOrder = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUnboundColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTimeEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.xtpInterruption = new DevExpress.XtraTab.XtraTabPage();
            this.gcInterruption = new DevExpress.XtraGrid.GridControl();
            this.gvInterruption = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtpMachineDown = new DevExpress.XtraTab.XtraTabPage();
            this.gcFaults = new DevExpress.XtraGrid.GridControl();
            this.gvFaults = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtpPersonel = new DevExpress.XtraTab.XtraTabPage();
            this.gcPerson = new DevExpress.XtraGrid.GridControl();
            this.gvPerson = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUnboundPersonPassingTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtpTimes = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl7 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem4 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem5 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem6 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblTotalWorkTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lblTotalInterruptionTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblTotalMachineDownTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblSetupTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblLastItemProductionTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblMeanProductionTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.tmrWorkShopOrder = new System.Windows.Forms.Timer(this.components);
            this.tmrMailControl = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lgTotalProduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsrbcTotalProduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lscTotalProduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ccProductionPerformance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgTargetProduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsrbcTargetProduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lscTargetProduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlReadValueLabels)).BeginInit();
            this.pnlReadValueLabels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMachineStateColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtcDetails)).BeginInit();
            this.xtcDetails.SuspendLayout();
            this.xtpWorkShopOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcWorkShopOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gwWorkShopOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit4)).BeginInit();
            this.xtpInterruption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcInterruption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInterruption)).BeginInit();
            this.xtpMachineDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcFaults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFaults)).BeginInit();
            this.xtpPersonel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPerson)).BeginInit();
            this.xtpTimes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).BeginInit();
            this.panelControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalWorkTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalInterruptionTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalMachineDownTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSetupTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLastItemProductionTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMeanProductionTime)).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.MaskSettings.Set("mask", "g");
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // repositoryItemTimeEdit1
            // 
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTimeEdit1.DisplayFormat.FormatString = "h:m";
            this.repositoryItemTimeEdit1.MaskSettings.Set("mask", "h:m");
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            this.repositoryItemTimeEdit1.UseMaskAsDisplayFormat = true;
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.MaskSettings.Set("mask", "g");
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // repositoryItemTimeEdit2
            // 
            this.repositoryItemTimeEdit2.AutoHeight = false;
            this.repositoryItemTimeEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTimeEdit2.DisplayFormat.FormatString = "h:m";
            this.repositoryItemTimeEdit2.MaskSettings.Set("mask", "h:m");
            this.repositoryItemTimeEdit2.Name = "repositoryItemTimeEdit2";
            this.repositoryItemTimeEdit2.UseMaskAsDisplayFormat = true;
            // 
            // repositoryItemDateEdit3
            // 
            this.repositoryItemDateEdit3.AutoHeight = false;
            this.repositoryItemDateEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit3.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit3.MaskSettings.Set("mask", "g");
            this.repositoryItemDateEdit3.Name = "repositoryItemDateEdit3";
            // 
            // repositoryItemTimeEdit3
            // 
            this.repositoryItemTimeEdit3.AutoHeight = false;
            this.repositoryItemTimeEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTimeEdit3.DisplayFormat.FormatString = "h:m";
            this.repositoryItemTimeEdit3.MaskSettings.Set("mask", "h:m");
            this.repositoryItemTimeEdit3.Name = "repositoryItemTimeEdit3";
            this.repositoryItemTimeEdit3.UseMaskAsDisplayFormat = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.oeePanel);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.groupControl3);
            this.panelControl1.Controls.Add(this.pnlReadValueLabels);
            this.panelControl1.Controls.Add(this.container);
            this.panelControl1.Controls.Add(this.panelControl4);
            this.panelControl1.Controls.Add(this.lblStatus);
            this.panelControl1.Controls.Add(this.lblProcessBarcode);
            this.panelControl1.Controls.Add(this.txtBarcode);
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1666, 929);
            this.panelControl1.TabIndex = 2;
            // 
            // oeePanel
            // 
            this.oeePanel.frmOperator = null;
            this.oeePanel.Location = new System.Drawing.Point(12, 109);
            this.oeePanel.Name = "oeePanel";
            this.oeePanel.Size = new System.Drawing.Size(933, 520);
            this.oeePanel.TabIndex = 37;
            this.oeePanel.Load += new System.EventHandler(this.ucOEEPanelAdvanced1_Load);
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.gcTotalProductionAmount);
            this.groupControl2.Location = new System.Drawing.Point(954, 92);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(696, 111);
            this.groupControl2.TabIndex = 35;
            this.groupControl2.Text = "Toplam Gerçekleşen Üretim";
            // 
            // gcTotalProductionAmount
            // 
            this.gcTotalProductionAmount.AutoLayout = false;
            this.gcTotalProductionAmount.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gcTotalProductionAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTotalProductionAmount.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.lgTotalProduction});
            this.gcTotalProductionAmount.LayoutPadding = new DevExpress.XtraGauges.Core.Base.Thickness(6, 8, 6, 8);
            this.gcTotalProductionAmount.Location = new System.Drawing.Point(2, 33);
            this.gcTotalProductionAmount.Name = "gcTotalProductionAmount";
            this.gcTotalProductionAmount.Size = new System.Drawing.Size(692, 76);
            this.gcTotalProductionAmount.TabIndex = 3;
            // 
            // lgTotalProduction
            // 
            this.lgTotalProduction.Bounds = new System.Drawing.Rectangle(7, 8, 667, 83);
            this.lgTotalProduction.Name = "lgTotalProduction";
            this.lgTotalProduction.Orientation = DevExpress.XtraGauges.Core.Model.ScaleOrientation.Horizontal;
            this.lgTotalProduction.RangeBars.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleRangeBarComponent[] {
            this.lsrbcTotalProduction});
            this.lgTotalProduction.Scales.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent[] {
            this.lscTotalProduction});
            // 
            // lsrbcTotalProduction
            // 
            this.lsrbcTotalProduction.AppearanceRangeBar.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:DodgerBlue");
            this.lsrbcTotalProduction.EndOffset = 20F;
            this.lsrbcTotalProduction.LinearScale = this.lscTotalProduction;
            this.lsrbcTotalProduction.Name = "linearGauge2_RangeBar1";
            this.lsrbcTotalProduction.StartOffset = 4F;
            // 
            // lscTotalProduction
            // 
            this.lscTotalProduction.AppearanceMajorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTotalProduction.AppearanceMajorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTotalProduction.AppearanceMinorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTotalProduction.AppearanceMinorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTotalProduction.AppearanceScale.Brush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#4D4D4D");
            this.lscTotalProduction.AppearanceScale.Width = 4F;
            this.lscTotalProduction.AppearanceTickmarkText.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.lscTotalProduction.AppearanceTickmarkText.TextBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#4D4D4D");
            this.lscTotalProduction.EndPoint = new DevExpress.XtraGauges.Core.Base.PointF2D(40F, 20F);
            this.lscTotalProduction.MajorTickCount = 6;
            this.lscTotalProduction.MajorTickmark.FormatString = "{0:F0}";
            this.lscTotalProduction.MajorTickmark.ShapeOffset = -2F;
            this.lscTotalProduction.MajorTickmark.ShapeScale = new DevExpress.XtraGauges.Core.Base.FactorF2D(1.1F, 1F);
            this.lscTotalProduction.MajorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style28_1;
            this.lscTotalProduction.MajorTickmark.TextOffset = 13F;
            this.lscTotalProduction.MajorTickmark.TextOrientation = DevExpress.XtraGauges.Core.Model.LabelOrientation.BottomToTop;
            this.lscTotalProduction.MaxValue = 0F;
            this.lscTotalProduction.MinorTickCount = 4;
            this.lscTotalProduction.MinorTickmark.ShapeOffset = -14F;
            this.lscTotalProduction.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style28_1;
            this.lscTotalProduction.MinorTickmark.ShowTick = false;
            this.lscTotalProduction.Name = "scale2";
            this.lscTotalProduction.StartPoint = new DevExpress.XtraGauges.Core.Base.PointF2D(40F, 300F);
            this.lscTotalProduction.ZOrder = -1;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.lblPlcLabel);
            this.groupControl1.Controls.Add(this.lblPLC);
            this.groupControl1.Controls.Add(this.lblTotalProductionCountLabel);
            this.groupControl1.Controls.Add(this.lblTotalProductionCount);
            this.groupControl1.Controls.Add(this.lblCurrentAmountLabel);
            this.groupControl1.Controls.Add(this.lblBoxAmount);
            this.groupControl1.Controls.Add(this.lblCurrentAmount);
            this.groupControl1.Controls.Add(this.lblRealizeAmountLabel);
            this.groupControl1.Controls.Add(this.lblBoxAmountLabel);
            this.groupControl1.Controls.Add(this.lblRealizeAmount);
            this.groupControl1.Controls.Add(this.lblScrapCount);
            this.groupControl1.Controls.Add(this.lblScrapCountLabel);
            this.groupControl1.Location = new System.Drawing.Point(954, 448);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(698, 198);
            this.groupControl1.TabIndex = 34;
            this.groupControl1.Text = "Üretim Miktarları";
            // 
            // lblPlcLabel
            // 
            this.lblPlcLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblPlcLabel.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblPlcLabel.Appearance.Options.UseFont = true;
            this.lblPlcLabel.Appearance.Options.UseForeColor = true;
            this.lblPlcLabel.Appearance.Options.UseTextOptions = true;
            this.lblPlcLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblPlcLabel.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblPlcLabel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblPlcLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPlcLabel.Location = new System.Drawing.Point(228, 163);
            this.lblPlcLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblPlcLabel.Name = "lblPlcLabel";
            this.lblPlcLabel.Size = new System.Drawing.Size(213, 26);
            this.lblPlcLabel.TabIndex = 33;
            this.lblPlcLabel.Text = "PLC SAYAÇ";
            // 
            // lblPLC
            // 
            this.lblPLC.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblPLC.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.lblPLC.Appearance.Options.UseFont = true;
            this.lblPLC.Appearance.Options.UseForeColor = true;
            this.lblPLC.Appearance.Options.UseTextOptions = true;
            this.lblPLC.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblPLC.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPLC.Location = new System.Drawing.Point(230, 123);
            this.lblPLC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblPLC.Name = "lblPLC";
            this.lblPLC.Size = new System.Drawing.Size(213, 38);
            this.lblPLC.TabIndex = 34;
            this.lblPLC.Text = "0";
            // 
            // lblTotalProductionCountLabel
            // 
            this.lblTotalProductionCountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalProductionCountLabel.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalProductionCountLabel.Appearance.Options.UseFont = true;
            this.lblTotalProductionCountLabel.Appearance.Options.UseForeColor = true;
            this.lblTotalProductionCountLabel.Appearance.Options.UseTextOptions = true;
            this.lblTotalProductionCountLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblTotalProductionCountLabel.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblTotalProductionCountLabel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblTotalProductionCountLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTotalProductionCountLabel.Location = new System.Drawing.Point(6, 77);
            this.lblTotalProductionCountLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblTotalProductionCountLabel.Name = "lblTotalProductionCountLabel";
            this.lblTotalProductionCountLabel.Size = new System.Drawing.Size(213, 26);
            this.lblTotalProductionCountLabel.TabIndex = 6;
            this.lblTotalProductionCountLabel.Text = "İŞ EMRİ MİKTARI";
            // 
            // lblTotalProductionCount
            // 
            this.lblTotalProductionCount.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalProductionCount.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.lblTotalProductionCount.Appearance.Options.UseFont = true;
            this.lblTotalProductionCount.Appearance.Options.UseForeColor = true;
            this.lblTotalProductionCount.Appearance.Options.UseTextOptions = true;
            this.lblTotalProductionCount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblTotalProductionCount.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblTotalProductionCount.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblTotalProductionCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTotalProductionCount.Location = new System.Drawing.Point(6, 37);
            this.lblTotalProductionCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblTotalProductionCount.Name = "lblTotalProductionCount";
            this.lblTotalProductionCount.Size = new System.Drawing.Size(213, 38);
            this.lblTotalProductionCount.TabIndex = 7;
            this.lblTotalProductionCount.Text = "0";
            // 
            // lblCurrentAmountLabel
            // 
            this.lblCurrentAmountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentAmountLabel.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblCurrentAmountLabel.Appearance.Options.UseFont = true;
            this.lblCurrentAmountLabel.Appearance.Options.UseForeColor = true;
            this.lblCurrentAmountLabel.Appearance.Options.UseTextOptions = true;
            this.lblCurrentAmountLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblCurrentAmountLabel.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblCurrentAmountLabel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblCurrentAmountLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCurrentAmountLabel.Location = new System.Drawing.Point(452, 163);
            this.lblCurrentAmountLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblCurrentAmountLabel.Name = "lblCurrentAmountLabel";
            this.lblCurrentAmountLabel.Size = new System.Drawing.Size(240, 26);
            this.lblCurrentAmountLabel.TabIndex = 9;
            this.lblCurrentAmountLabel.Text = "ANLIK";
            // 
            // lblBoxAmount
            // 
            this.lblBoxAmount.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblBoxAmount.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblBoxAmount.Appearance.Options.UseFont = true;
            this.lblBoxAmount.Appearance.Options.UseForeColor = true;
            this.lblBoxAmount.Appearance.Options.UseTextOptions = true;
            this.lblBoxAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblBoxAmount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblBoxAmount.Location = new System.Drawing.Point(465, 37);
            this.lblBoxAmount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblBoxAmount.Name = "lblBoxAmount";
            this.lblBoxAmount.Size = new System.Drawing.Size(213, 38);
            this.lblBoxAmount.TabIndex = 32;
            this.lblBoxAmount.Text = "0";
            // 
            // lblCurrentAmount
            // 
            this.lblCurrentAmount.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblCurrentAmount.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.lblCurrentAmount.Appearance.Options.UseFont = true;
            this.lblCurrentAmount.Appearance.Options.UseForeColor = true;
            this.lblCurrentAmount.Appearance.Options.UseTextOptions = true;
            this.lblCurrentAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblCurrentAmount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCurrentAmount.Location = new System.Drawing.Point(465, 123);
            this.lblCurrentAmount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblCurrentAmount.Name = "lblCurrentAmount";
            this.lblCurrentAmount.Size = new System.Drawing.Size(213, 38);
            this.lblCurrentAmount.TabIndex = 25;
            this.lblCurrentAmount.Text = "0";
            // 
            // lblRealizeAmountLabel
            // 
            this.lblRealizeAmountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblRealizeAmountLabel.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblRealizeAmountLabel.Appearance.Options.UseFont = true;
            this.lblRealizeAmountLabel.Appearance.Options.UseForeColor = true;
            this.lblRealizeAmountLabel.Appearance.Options.UseTextOptions = true;
            this.lblRealizeAmountLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblRealizeAmountLabel.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblRealizeAmountLabel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblRealizeAmountLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRealizeAmountLabel.Location = new System.Drawing.Point(6, 163);
            this.lblRealizeAmountLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblRealizeAmountLabel.Name = "lblRealizeAmountLabel";
            this.lblRealizeAmountLabel.Size = new System.Drawing.Size(213, 26);
            this.lblRealizeAmountLabel.TabIndex = 16;
            this.lblRealizeAmountLabel.Text = "GERÇEKLEŞEN";
            // 
            // lblBoxAmountLabel
            // 
            this.lblBoxAmountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblBoxAmountLabel.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblBoxAmountLabel.Appearance.Options.UseFont = true;
            this.lblBoxAmountLabel.Appearance.Options.UseForeColor = true;
            this.lblBoxAmountLabel.Appearance.Options.UseTextOptions = true;
            this.lblBoxAmountLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblBoxAmountLabel.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblBoxAmountLabel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblBoxAmountLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblBoxAmountLabel.Location = new System.Drawing.Point(452, 77);
            this.lblBoxAmountLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblBoxAmountLabel.Name = "lblBoxAmountLabel";
            this.lblBoxAmountLabel.Size = new System.Drawing.Size(240, 26);
            this.lblBoxAmountLabel.TabIndex = 31;
            this.lblBoxAmountLabel.Text = "KASA ETİKETİ MİKTARI";
            // 
            // lblRealizeAmount
            // 
            this.lblRealizeAmount.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblRealizeAmount.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.lblRealizeAmount.Appearance.Options.UseFont = true;
            this.lblRealizeAmount.Appearance.Options.UseForeColor = true;
            this.lblRealizeAmount.Appearance.Options.UseTextOptions = true;
            this.lblRealizeAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblRealizeAmount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRealizeAmount.Location = new System.Drawing.Point(6, 123);
            this.lblRealizeAmount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblRealizeAmount.Name = "lblRealizeAmount";
            this.lblRealizeAmount.Size = new System.Drawing.Size(213, 38);
            this.lblRealizeAmount.TabIndex = 17;
            this.lblRealizeAmount.Text = "0";
            // 
            // lblScrapCount
            // 
            this.lblScrapCount.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblScrapCount.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblScrapCount.Appearance.Options.UseFont = true;
            this.lblScrapCount.Appearance.Options.UseForeColor = true;
            this.lblScrapCount.Appearance.Options.UseTextOptions = true;
            this.lblScrapCount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblScrapCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblScrapCount.Location = new System.Drawing.Point(230, 37);
            this.lblScrapCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblScrapCount.Name = "lblScrapCount";
            this.lblScrapCount.Size = new System.Drawing.Size(213, 38);
            this.lblScrapCount.TabIndex = 13;
            this.lblScrapCount.Text = "0";
            // 
            // lblScrapCountLabel
            // 
            this.lblScrapCountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblScrapCountLabel.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblScrapCountLabel.Appearance.Options.UseFont = true;
            this.lblScrapCountLabel.Appearance.Options.UseForeColor = true;
            this.lblScrapCountLabel.Appearance.Options.UseTextOptions = true;
            this.lblScrapCountLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblScrapCountLabel.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblScrapCountLabel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblScrapCountLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblScrapCountLabel.Location = new System.Drawing.Point(228, 77);
            this.lblScrapCountLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblScrapCountLabel.Name = "lblScrapCountLabel";
            this.lblScrapCountLabel.Size = new System.Drawing.Size(213, 26);
            this.lblScrapCountLabel.TabIndex = 12;
            this.lblScrapCountLabel.Text = "ŞÜPHELİ ÜRÜN";
            // 
            // groupControl3
            // 
            this.groupControl3.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupControl3.AppearanceCaption.Options.UseFont = true;
            this.groupControl3.Controls.Add(this.ccProductionPerformance);
            this.groupControl3.Controls.Add(this.gcTargetProductionAmount);
            this.groupControl3.Location = new System.Drawing.Point(954, 211);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(696, 229);
            this.groupControl3.TabIndex = 36;
            this.groupControl3.Text = "Hedeflenen Üretim";
            // 
            // ccProductionPerformance
            // 
            this.ccProductionPerformance.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.PaneLayout.Direction = DevExpress.XtraCharts.PaneLayoutDirection.Horizontal;
            xyDiagram1.Rotated = true;
            this.ccProductionPerformance.Diagram = xyDiagram1;
            this.ccProductionPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ccProductionPerformance.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.ccProductionPerformance.Location = new System.Drawing.Point(2, 102);
            this.ccProductionPerformance.Name = "ccProductionPerformance";
            series1.Name = "Series 1";
            seriesPoint1.ColorSerializable = "#548DD4";
            seriesPoint2.ColorSerializable = "#75BF55";
            series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint1,
            seriesPoint2});
            this.ccProductionPerformance.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.ccProductionPerformance.Size = new System.Drawing.Size(692, 125);
            this.ccProductionPerformance.TabIndex = 5;
            // 
            // gcTargetProductionAmount
            // 
            this.gcTargetProductionAmount.AutoLayout = false;
            this.gcTargetProductionAmount.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gcTargetProductionAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcTargetProductionAmount.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.lgTargetProduction});
            this.gcTargetProductionAmount.LayoutPadding = new DevExpress.XtraGauges.Core.Base.Thickness(6, 8, 6, 8);
            this.gcTargetProductionAmount.Location = new System.Drawing.Point(2, 33);
            this.gcTargetProductionAmount.Name = "gcTargetProductionAmount";
            this.gcTargetProductionAmount.Size = new System.Drawing.Size(692, 69);
            this.gcTargetProductionAmount.TabIndex = 4;
            // 
            // lgTargetProduction
            // 
            this.lgTargetProduction.Bounds = new System.Drawing.Rectangle(7, 8, 667, 83);
            this.lgTargetProduction.Name = "lgTargetProduction";
            this.lgTargetProduction.Orientation = DevExpress.XtraGauges.Core.Model.ScaleOrientation.Horizontal;
            this.lgTargetProduction.RangeBars.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleRangeBarComponent[] {
            this.lsrbcTargetProduction});
            this.lgTargetProduction.Scales.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent[] {
            this.lscTargetProduction});
            // 
            // lsrbcTargetProduction
            // 
            this.lsrbcTargetProduction.AppearanceRangeBar.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:DodgerBlue");
            this.lsrbcTargetProduction.EndOffset = 20F;
            this.lsrbcTargetProduction.LinearScale = this.lscTargetProduction;
            this.lsrbcTargetProduction.Name = "linearGauge2_RangeBar1";
            this.lsrbcTargetProduction.StartOffset = 4F;
            // 
            // lscTargetProduction
            // 
            this.lscTargetProduction.AppearanceMajorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTargetProduction.AppearanceMajorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTargetProduction.AppearanceMinorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTargetProduction.AppearanceMinorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.lscTargetProduction.AppearanceScale.Brush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#4D4D4D");
            this.lscTargetProduction.AppearanceScale.Width = 4F;
            this.lscTargetProduction.AppearanceTickmarkText.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.lscTargetProduction.AppearanceTickmarkText.TextBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#4D4D4D");
            this.lscTargetProduction.EndPoint = new DevExpress.XtraGauges.Core.Base.PointF2D(40F, 20F);
            this.lscTargetProduction.MajorTickCount = 6;
            this.lscTargetProduction.MajorTickmark.FormatString = "{0:F0}";
            this.lscTargetProduction.MajorTickmark.ShapeOffset = -2F;
            this.lscTargetProduction.MajorTickmark.ShapeScale = new DevExpress.XtraGauges.Core.Base.FactorF2D(1.1F, 1F);
            this.lscTargetProduction.MajorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style28_1;
            this.lscTargetProduction.MajorTickmark.TextOffset = 13F;
            this.lscTargetProduction.MajorTickmark.TextOrientation = DevExpress.XtraGauges.Core.Model.LabelOrientation.BottomToTop;
            this.lscTargetProduction.MaxValue = 0F;
            this.lscTargetProduction.MinorTickCount = 4;
            this.lscTargetProduction.MinorTickmark.ShapeOffset = -14F;
            this.lscTargetProduction.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style28_1;
            this.lscTargetProduction.MinorTickmark.ShowTick = false;
            this.lscTargetProduction.Name = "scale2";
            this.lscTargetProduction.StartPoint = new DevExpress.XtraGauges.Core.Base.PointF2D(40F, 300F);
            this.lscTargetProduction.ZOrder = -1;
            // 
            // pnlReadValueLabels
            // 
            this.pnlReadValueLabels.Controls.Add(this.lblValue9);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription9);
            this.pnlReadValueLabels.Controls.Add(this.lblValue8);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription8);
            this.pnlReadValueLabels.Controls.Add(this.lblValue7);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription7);
            this.pnlReadValueLabels.Controls.Add(this.lblValue6);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription6);
            this.pnlReadValueLabels.Controls.Add(this.labelControl35);
            this.pnlReadValueLabels.Controls.Add(this.lblValue5);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription5);
            this.pnlReadValueLabels.Controls.Add(this.labelControl40);
            this.pnlReadValueLabels.Controls.Add(this.lblValue4);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription4);
            this.pnlReadValueLabels.Controls.Add(this.labelControl48);
            this.pnlReadValueLabels.Controls.Add(this.lblValue3);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription3);
            this.pnlReadValueLabels.Controls.Add(this.labelControl45);
            this.pnlReadValueLabels.Controls.Add(this.lblValue2);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription2);
            this.pnlReadValueLabels.Controls.Add(this.labelControl42);
            this.pnlReadValueLabels.Controls.Add(this.lblValue1);
            this.pnlReadValueLabels.Controls.Add(this.lblDescription1);
            this.pnlReadValueLabels.Controls.Add(this.labelControl38);
            this.pnlReadValueLabels.Location = new System.Drawing.Point(954, 655);
            this.pnlReadValueLabels.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlReadValueLabels.Name = "pnlReadValueLabels";
            this.pnlReadValueLabels.Size = new System.Drawing.Size(698, 266);
            this.pnlReadValueLabels.TabIndex = 16;
            // 
            // lblValue9
            // 
            this.lblValue9.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue9.Appearance.Options.UseFont = true;
            this.lblValue9.Appearance.Options.UseForeColor = true;
            this.lblValue9.Appearance.Options.UseTextOptions = true;
            this.lblValue9.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue9.Location = new System.Drawing.Point(472, 192);
            this.lblValue9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue9.Name = "lblValue9";
            this.lblValue9.Size = new System.Drawing.Size(170, 29);
            this.lblValue9.TabIndex = 44;
            this.lblValue9.Text = "-";
            // 
            // lblDescription9
            // 
            this.lblDescription9.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription9.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription9.Appearance.Options.UseFont = true;
            this.lblDescription9.Appearance.Options.UseForeColor = true;
            this.lblDescription9.Appearance.Options.UseTextOptions = true;
            this.lblDescription9.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription9.Location = new System.Drawing.Point(472, 229);
            this.lblDescription9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription9.Name = "lblDescription9";
            this.lblDescription9.Size = new System.Drawing.Size(170, 29);
            this.lblDescription9.TabIndex = 43;
            this.lblDescription9.Text = "-";
            // 
            // lblValue8
            // 
            this.lblValue8.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue8.Appearance.Options.UseFont = true;
            this.lblValue8.Appearance.Options.UseForeColor = true;
            this.lblValue8.Appearance.Options.UseTextOptions = true;
            this.lblValue8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue8.Location = new System.Drawing.Point(246, 192);
            this.lblValue8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue8.Name = "lblValue8";
            this.lblValue8.Size = new System.Drawing.Size(170, 29);
            this.lblValue8.TabIndex = 42;
            this.lblValue8.Text = "-";
            // 
            // lblDescription8
            // 
            this.lblDescription8.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription8.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription8.Appearance.Options.UseFont = true;
            this.lblDescription8.Appearance.Options.UseForeColor = true;
            this.lblDescription8.Appearance.Options.UseTextOptions = true;
            this.lblDescription8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription8.Location = new System.Drawing.Point(246, 229);
            this.lblDescription8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription8.Name = "lblDescription8";
            this.lblDescription8.Size = new System.Drawing.Size(170, 29);
            this.lblDescription8.TabIndex = 41;
            this.lblDescription8.Text = "-";
            // 
            // lblValue7
            // 
            this.lblValue7.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue7.Appearance.Options.UseFont = true;
            this.lblValue7.Appearance.Options.UseForeColor = true;
            this.lblValue7.Appearance.Options.UseTextOptions = true;
            this.lblValue7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue7.Location = new System.Drawing.Point(18, 192);
            this.lblValue7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue7.Name = "lblValue7";
            this.lblValue7.Size = new System.Drawing.Size(170, 29);
            this.lblValue7.TabIndex = 40;
            this.lblValue7.Text = "-";
            // 
            // lblDescription7
            // 
            this.lblDescription7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription7.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription7.Appearance.Options.UseFont = true;
            this.lblDescription7.Appearance.Options.UseForeColor = true;
            this.lblDescription7.Appearance.Options.UseTextOptions = true;
            this.lblDescription7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription7.Location = new System.Drawing.Point(18, 229);
            this.lblDescription7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription7.Name = "lblDescription7";
            this.lblDescription7.Size = new System.Drawing.Size(170, 29);
            this.lblDescription7.TabIndex = 39;
            this.lblDescription7.Text = "-";
            // 
            // lblValue6
            // 
            this.lblValue6.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue6.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue6.Appearance.Options.UseFont = true;
            this.lblValue6.Appearance.Options.UseForeColor = true;
            this.lblValue6.Appearance.Options.UseTextOptions = true;
            this.lblValue6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue6.Location = new System.Drawing.Point(472, 106);
            this.lblValue6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue6.Name = "lblValue6";
            this.lblValue6.Size = new System.Drawing.Size(170, 29);
            this.lblValue6.TabIndex = 37;
            this.lblValue6.Text = "-";
            // 
            // lblDescription6
            // 
            this.lblDescription6.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription6.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription6.Appearance.Options.UseFont = true;
            this.lblDescription6.Appearance.Options.UseForeColor = true;
            this.lblDescription6.Appearance.Options.UseTextOptions = true;
            this.lblDescription6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription6.Location = new System.Drawing.Point(472, 143);
            this.lblDescription6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription6.Name = "lblDescription6";
            this.lblDescription6.Size = new System.Drawing.Size(170, 29);
            this.lblDescription6.TabIndex = 36;
            this.lblDescription6.Text = "-";
            // 
            // labelControl35
            // 
            this.labelControl35.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl35.Appearance.Options.UseBackColor = true;
            this.labelControl35.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl35.Location = new System.Drawing.Point(482, 174);
            this.labelControl35.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl35.Name = "labelControl35";
            this.labelControl35.Size = new System.Drawing.Size(150, 3);
            this.labelControl35.TabIndex = 38;
            // 
            // lblValue5
            // 
            this.lblValue5.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue5.Appearance.Options.UseFont = true;
            this.lblValue5.Appearance.Options.UseForeColor = true;
            this.lblValue5.Appearance.Options.UseTextOptions = true;
            this.lblValue5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue5.Location = new System.Drawing.Point(246, 106);
            this.lblValue5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue5.Name = "lblValue5";
            this.lblValue5.Size = new System.Drawing.Size(170, 29);
            this.lblValue5.TabIndex = 34;
            this.lblValue5.Text = "-";
            // 
            // lblDescription5
            // 
            this.lblDescription5.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription5.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription5.Appearance.Options.UseFont = true;
            this.lblDescription5.Appearance.Options.UseForeColor = true;
            this.lblDescription5.Appearance.Options.UseTextOptions = true;
            this.lblDescription5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription5.Location = new System.Drawing.Point(246, 143);
            this.lblDescription5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription5.Name = "lblDescription5";
            this.lblDescription5.Size = new System.Drawing.Size(170, 29);
            this.lblDescription5.TabIndex = 33;
            this.lblDescription5.Text = "-";
            // 
            // labelControl40
            // 
            this.labelControl40.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl40.Appearance.Options.UseBackColor = true;
            this.labelControl40.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl40.Location = new System.Drawing.Point(255, 174);
            this.labelControl40.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl40.Name = "labelControl40";
            this.labelControl40.Size = new System.Drawing.Size(150, 3);
            this.labelControl40.TabIndex = 35;
            // 
            // lblValue4
            // 
            this.lblValue4.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue4.Appearance.Options.UseFont = true;
            this.lblValue4.Appearance.Options.UseForeColor = true;
            this.lblValue4.Appearance.Options.UseTextOptions = true;
            this.lblValue4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue4.Location = new System.Drawing.Point(18, 106);
            this.lblValue4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue4.Name = "lblValue4";
            this.lblValue4.Size = new System.Drawing.Size(170, 29);
            this.lblValue4.TabIndex = 31;
            this.lblValue4.Text = "-";
            // 
            // lblDescription4
            // 
            this.lblDescription4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription4.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription4.Appearance.Options.UseFont = true;
            this.lblDescription4.Appearance.Options.UseForeColor = true;
            this.lblDescription4.Appearance.Options.UseTextOptions = true;
            this.lblDescription4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription4.Location = new System.Drawing.Point(18, 143);
            this.lblDescription4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription4.Name = "lblDescription4";
            this.lblDescription4.Size = new System.Drawing.Size(170, 29);
            this.lblDescription4.TabIndex = 30;
            this.lblDescription4.Text = "-";
            // 
            // labelControl48
            // 
            this.labelControl48.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl48.Appearance.Options.UseBackColor = true;
            this.labelControl48.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl48.Location = new System.Drawing.Point(18, 174);
            this.labelControl48.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl48.Name = "labelControl48";
            this.labelControl48.Size = new System.Drawing.Size(170, 3);
            this.labelControl48.TabIndex = 32;
            // 
            // lblValue3
            // 
            this.lblValue3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue3.Appearance.Options.UseFont = true;
            this.lblValue3.Appearance.Options.UseForeColor = true;
            this.lblValue3.Appearance.Options.UseTextOptions = true;
            this.lblValue3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue3.Location = new System.Drawing.Point(472, 17);
            this.lblValue3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue3.Name = "lblValue3";
            this.lblValue3.Size = new System.Drawing.Size(170, 29);
            this.lblValue3.TabIndex = 28;
            this.lblValue3.Text = "-";
            // 
            // lblDescription3
            // 
            this.lblDescription3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription3.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription3.Appearance.Options.UseFont = true;
            this.lblDescription3.Appearance.Options.UseForeColor = true;
            this.lblDescription3.Appearance.Options.UseTextOptions = true;
            this.lblDescription3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription3.Location = new System.Drawing.Point(472, 52);
            this.lblDescription3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription3.Name = "lblDescription3";
            this.lblDescription3.Size = new System.Drawing.Size(170, 29);
            this.lblDescription3.TabIndex = 27;
            this.lblDescription3.Text = "-";
            // 
            // labelControl45
            // 
            this.labelControl45.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl45.Appearance.Options.UseBackColor = true;
            this.labelControl45.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl45.Location = new System.Drawing.Point(482, 83);
            this.labelControl45.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl45.Name = "labelControl45";
            this.labelControl45.Size = new System.Drawing.Size(150, 3);
            this.labelControl45.TabIndex = 29;
            // 
            // lblValue2
            // 
            this.lblValue2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue2.Appearance.Options.UseFont = true;
            this.lblValue2.Appearance.Options.UseForeColor = true;
            this.lblValue2.Appearance.Options.UseTextOptions = true;
            this.lblValue2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue2.Location = new System.Drawing.Point(246, 17);
            this.lblValue2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue2.Name = "lblValue2";
            this.lblValue2.Size = new System.Drawing.Size(170, 29);
            this.lblValue2.TabIndex = 25;
            this.lblValue2.Text = "-";
            // 
            // lblDescription2
            // 
            this.lblDescription2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription2.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription2.Appearance.Options.UseFont = true;
            this.lblDescription2.Appearance.Options.UseForeColor = true;
            this.lblDescription2.Appearance.Options.UseTextOptions = true;
            this.lblDescription2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription2.Location = new System.Drawing.Point(246, 52);
            this.lblDescription2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription2.Name = "lblDescription2";
            this.lblDescription2.Size = new System.Drawing.Size(170, 29);
            this.lblDescription2.TabIndex = 24;
            this.lblDescription2.Text = "-";
            // 
            // labelControl42
            // 
            this.labelControl42.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl42.Appearance.Options.UseBackColor = true;
            this.labelControl42.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl42.Location = new System.Drawing.Point(255, 83);
            this.labelControl42.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl42.Name = "labelControl42";
            this.labelControl42.Size = new System.Drawing.Size(150, 3);
            this.labelControl42.TabIndex = 26;
            // 
            // lblValue1
            // 
            this.lblValue1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblValue1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(94)))), ((int)(((byte)(135)))));
            this.lblValue1.Appearance.Options.UseFont = true;
            this.lblValue1.Appearance.Options.UseForeColor = true;
            this.lblValue1.Appearance.Options.UseTextOptions = true;
            this.lblValue1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblValue1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblValue1.Location = new System.Drawing.Point(18, 17);
            this.lblValue1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblValue1.Name = "lblValue1";
            this.lblValue1.Size = new System.Drawing.Size(170, 29);
            this.lblValue1.TabIndex = 16;
            this.lblValue1.Text = "-";
            // 
            // lblDescription1
            // 
            this.lblDescription1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDescription1.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription1.Appearance.Options.UseFont = true;
            this.lblDescription1.Appearance.Options.UseForeColor = true;
            this.lblDescription1.Appearance.Options.UseTextOptions = true;
            this.lblDescription1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription1.Location = new System.Drawing.Point(18, 52);
            this.lblDescription1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDescription1.Name = "lblDescription1";
            this.lblDescription1.Size = new System.Drawing.Size(170, 29);
            this.lblDescription1.TabIndex = 15;
            this.lblDescription1.Text = "-";
            // 
            // labelControl38
            // 
            this.labelControl38.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl38.Appearance.Options.UseBackColor = true;
            this.labelControl38.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl38.Location = new System.Drawing.Point(18, 83);
            this.labelControl38.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl38.Name = "labelControl38";
            this.labelControl38.Size = new System.Drawing.Size(170, 3);
            this.labelControl38.TabIndex = 17;
            // 
            // container
            // 
            this.container.Location = new System.Drawing.Point(1602, 0);
            this.container.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(64, 51);
            this.container.TabIndex = 2;
            this.container.Visible = false;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.lblWorkCenterStartTime);
            this.panelControl4.Controls.Add(this.lblWorkOrderStopWatch);
            this.panelControl4.Controls.Add(this.lblMachineState);
            this.panelControl4.Controls.Add(this.pnlMachineStateColor);
            this.panelControl4.Location = new System.Drawing.Point(12, 8);
            this.panelControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(934, 89);
            this.panelControl4.TabIndex = 2;
            // 
            // lblWorkCenterStartTime
            // 
            this.lblWorkCenterStartTime.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblWorkCenterStartTime.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblWorkCenterStartTime.Appearance.Options.UseFont = true;
            this.lblWorkCenterStartTime.Appearance.Options.UseForeColor = true;
            this.lblWorkCenterStartTime.Location = new System.Drawing.Point(408, 22);
            this.lblWorkCenterStartTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblWorkCenterStartTime.Name = "lblWorkCenterStartTime";
            this.lblWorkCenterStartTime.Size = new System.Drawing.Size(0, 43);
            this.lblWorkCenterStartTime.TabIndex = 9;
            // 
            // lblWorkOrderStopWatch
            // 
            this.lblWorkOrderStopWatch.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblWorkOrderStopWatch.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblWorkOrderStopWatch.Appearance.Options.UseFont = true;
            this.lblWorkOrderStopWatch.Appearance.Options.UseForeColor = true;
            this.lblWorkOrderStopWatch.Location = new System.Drawing.Point(57, 29);
            this.lblWorkOrderStopWatch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblWorkOrderStopWatch.Name = "lblWorkOrderStopWatch";
            this.lblWorkOrderStopWatch.Size = new System.Drawing.Size(0, 21);
            this.lblWorkOrderStopWatch.TabIndex = 8;
            // 
            // lblMachineState
            // 
            this.lblMachineState.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblMachineState.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblMachineState.Appearance.Options.UseFont = true;
            this.lblMachineState.Appearance.Options.UseForeColor = true;
            this.lblMachineState.Location = new System.Drawing.Point(66, 22);
            this.lblMachineState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblMachineState.Name = "lblMachineState";
            this.lblMachineState.Size = new System.Drawing.Size(157, 43);
            this.lblMachineState.TabIndex = 7;
            this.lblMachineState.Text = "Çalışıyor";
            // 
            // pnlMachineStateColor
            // 
            this.pnlMachineStateColor.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(191)))), ((int)(((byte)(85)))));
            this.pnlMachineStateColor.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(191)))), ((int)(((byte)(85)))));
            this.pnlMachineStateColor.Appearance.Options.UseBackColor = true;
            this.pnlMachineStateColor.Appearance.Options.UseBorderColor = true;
            this.pnlMachineStateColor.Appearance.Options.UseFont = true;
            this.pnlMachineStateColor.Appearance.Options.UseForeColor = true;
            this.pnlMachineStateColor.Appearance.Options.UseImage = true;
            this.pnlMachineStateColor.Appearance.Options.UseTextOptions = true;
            this.pnlMachineStateColor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlMachineStateColor.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMachineStateColor.Location = new System.Drawing.Point(2, 2);
            this.pnlMachineStateColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlMachineStateColor.Name = "pnlMachineStateColor";
            this.pnlMachineStateColor.Size = new System.Drawing.Size(22, 85);
            this.pnlMachineStateColor.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.lblStatus.Appearance.Options.UseFont = true;
            this.lblStatus.Appearance.Options.UseForeColor = true;
            this.lblStatus.Appearance.Options.UseTextOptions = true;
            this.lblStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblStatus.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblStatus.Location = new System.Drawing.Point(954, 45);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(696, 49);
            this.lblStatus.TabIndex = 13;
            // 
            // lblProcessBarcode
            // 
            this.lblProcessBarcode.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblProcessBarcode.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lblProcessBarcode.Appearance.Options.UseFont = true;
            this.lblProcessBarcode.Appearance.Options.UseForeColor = true;
            this.lblProcessBarcode.Appearance.Options.UseTextOptions = true;
            this.lblProcessBarcode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblProcessBarcode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblProcessBarcode.Location = new System.Drawing.Point(954, 9);
            this.lblProcessBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblProcessBarcode.Name = "lblProcessBarcode";
            this.lblProcessBarcode.Size = new System.Drawing.Size(246, 26);
            this.lblProcessBarcode.TabIndex = 12;
            this.lblProcessBarcode.Text = "Proses Barkodu";
            this.lblProcessBarcode.Visible = false;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBarcode.Location = new System.Drawing.Point(1209, 8);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBarcode.MaximumSize = new System.Drawing.Size(198, 31);
            this.txtBarcode.MinimumSize = new System.Drawing.Size(198, 31);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.PeachPuff;
            this.txtBarcode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBarcode.Size = new System.Drawing.Size(198, 26);
            this.txtBarcode.TabIndex = 5;
            this.txtBarcode.Visible = false;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.panelControl6);
            this.panelControl3.Location = new System.Drawing.Point(12, 640);
            this.panelControl3.LookAndFeel.SkinName = "Metropolis";
            this.panelControl3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(934, 282);
            this.panelControl3.TabIndex = 1;
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.xtcDetails);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl6.Location = new System.Drawing.Point(2, 2);
            this.panelControl6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(930, 278);
            this.panelControl6.TabIndex = 3;
            // 
            // xtcDetails
            // 
            this.xtcDetails.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 10F);
            this.xtcDetails.AppearancePage.Header.ForeColor = System.Drawing.Color.Gray;
            this.xtcDetails.AppearancePage.Header.Options.UseFont = true;
            this.xtcDetails.AppearancePage.Header.Options.UseForeColor = true;
            this.xtcDetails.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.xtcDetails.AppearancePage.HeaderActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.xtcDetails.AppearancePage.HeaderActive.Options.UseFont = true;
            this.xtcDetails.AppearancePage.HeaderActive.Options.UseForeColor = true;
            this.xtcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtcDetails.HeaderButtons = ((DevExpress.XtraTab.TabButtons)(((DevExpress.XtraTab.TabButtons.Prev | DevExpress.XtraTab.TabButtons.Next) 
            | DevExpress.XtraTab.TabButtons.Default)));
            this.xtcDetails.Location = new System.Drawing.Point(2, 2);
            this.xtcDetails.LookAndFeel.SkinName = "Office 2019 White";
            this.xtcDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xtcDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtcDetails.Name = "xtcDetails";
            this.xtcDetails.SelectedTabPage = this.xtpWorkShopOrder;
            this.xtcDetails.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.xtcDetails.Size = new System.Drawing.Size(926, 274);
            this.xtcDetails.TabIndex = 0;
            this.xtcDetails.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtpWorkShopOrder,
            this.xtpInterruption,
            this.xtpMachineDown,
            this.xtpPersonel,
            this.xtpTimes});
            // 
            // xtpWorkShopOrder
            // 
            this.xtpWorkShopOrder.Controls.Add(this.gcWorkShopOrder);
            this.xtpWorkShopOrder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtpWorkShopOrder.Name = "xtpWorkShopOrder";
            this.xtpWorkShopOrder.Size = new System.Drawing.Size(924, 233);
            this.xtpWorkShopOrder.Text = "İş Emirleri";
            // 
            // gcWorkShopOrder
            // 
            this.gcWorkShopOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcWorkShopOrder.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gcWorkShopOrder.Location = new System.Drawing.Point(0, 0);
            this.gcWorkShopOrder.LookAndFeel.SkinName = "Office 2019 White";
            this.gcWorkShopOrder.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcWorkShopOrder.MainView = this.gwWorkShopOrder;
            this.gcWorkShopOrder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gcWorkShopOrder.Name = "gcWorkShopOrder";
            this.gcWorkShopOrder.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit4,
            this.repositoryItemTimeEdit4});
            this.gcWorkShopOrder.Size = new System.Drawing.Size(924, 233);
            this.gcWorkShopOrder.TabIndex = 3;
            this.gcWorkShopOrder.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gwWorkShopOrder});
            // 
            // gwWorkShopOrder
            // 
            this.gwWorkShopOrder.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.gridColumn11,
            this.gridColumn7,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn8,
            this.gcUnboundColumn,
            this.gridColumn12});
            this.gwWorkShopOrder.DetailHeight = 508;
            this.gwWorkShopOrder.GridControl = this.gcWorkShopOrder;
            this.gwWorkShopOrder.Name = "gwWorkShopOrder";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn5.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn5.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn5.Caption = "İş Emri No";
            this.gridColumn5.FieldName = "orderNo";
            this.gridColumn5.MinWidth = 30;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.OptionsColumn.ReadOnly = true;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 84;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn11.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn11.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn11.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn11.AppearanceHeader.Options.UseFont = true;
            this.gridColumn11.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn11.Caption = "Operasyon No";
            this.gridColumn11.FieldName = "operationNo";
            this.gridColumn11.MinWidth = 30;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            this.gridColumn11.OptionsColumn.ReadOnly = true;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 99;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn7.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn7.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn7.Caption = "Hedef";
            this.gridColumn7.FieldName = "revisedQtyDue";
            this.gridColumn7.MinWidth = 30;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.OptionsColumn.ReadOnly = true;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            this.gridColumn7.Width = 81;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn9.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn9.AppearanceHeader.ForeColor = System.Drawing.Color.DimGray;
            this.gridColumn9.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn9.AppearanceHeader.Options.UseFont = true;
            this.gridColumn9.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn9.Caption = "Gerçekleşen";
            this.gridColumn9.FieldName = "qtyComplate";
            this.gridColumn9.MinWidth = 30;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            this.gridColumn9.OptionsColumn.ReadOnly = true;
            this.gridColumn9.UnboundDataType = typeof(decimal);
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 4;
            this.gridColumn9.Width = 87;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn10.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn10.AppearanceHeader.ForeColor = System.Drawing.Color.DimGray;
            this.gridColumn10.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn10.AppearanceHeader.Options.UseFont = true;
            this.gridColumn10.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn10.Caption = "Başlangıç Zamanı";
            this.gridColumn10.ColumnEdit = this.repositoryItemDateEdit4;
            this.gridColumn10.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn10.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn10.FieldName = "opStartDate";
            this.gridColumn10.MinWidth = 30;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.AllowFocus = false;
            this.gridColumn10.OptionsColumn.ReadOnly = true;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 5;
            this.gridColumn10.Width = 111;
            // 
            // repositoryItemDateEdit4
            // 
            this.repositoryItemDateEdit4.AutoHeight = false;
            this.repositoryItemDateEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit4.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit4.MaskSettings.Set("mask", "g");
            this.repositoryItemDateEdit4.Name = "repositoryItemDateEdit4";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn8.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn8.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn8.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn8.AppearanceHeader.Options.UseFont = true;
            this.gridColumn8.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn8.Caption = "Ürün Adı";
            this.gridColumn8.FieldName = "productDescription";
            this.gridColumn8.MinWidth = 30;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.OptionsColumn.ReadOnly = true;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 2;
            this.gridColumn8.Width = 297;
            // 
            // gcUnboundColumn
            // 
            this.gcUnboundColumn.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gcUnboundColumn.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gcUnboundColumn.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gcUnboundColumn.AppearanceHeader.Options.UseBackColor = true;
            this.gcUnboundColumn.AppearanceHeader.Options.UseFont = true;
            this.gcUnboundColumn.AppearanceHeader.Options.UseForeColor = true;
            this.gcUnboundColumn.Caption = "Geçen Süre";
            this.gcUnboundColumn.FieldName = "gcUnboundColumn";
            this.gcUnboundColumn.MinWidth = 30;
            this.gcUnboundColumn.Name = "gcUnboundColumn";
            this.gcUnboundColumn.OptionsColumn.AllowEdit = false;
            this.gcUnboundColumn.OptionsColumn.AllowFocus = false;
            this.gcUnboundColumn.OptionsColumn.ReadOnly = true;
            this.gcUnboundColumn.UnboundDataType = typeof(double);
            this.gcUnboundColumn.Visible = true;
            this.gcUnboundColumn.VisibleIndex = 6;
            this.gcUnboundColumn.Width = 102;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Id";
            this.gridColumn12.FieldName = "Id";
            this.gridColumn12.MinWidth = 30;
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Width = 112;
            // 
            // repositoryItemTimeEdit4
            // 
            this.repositoryItemTimeEdit4.AutoHeight = false;
            this.repositoryItemTimeEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTimeEdit4.DisplayFormat.FormatString = "h:m";
            this.repositoryItemTimeEdit4.MaskSettings.Set("mask", "h:m");
            this.repositoryItemTimeEdit4.Name = "repositoryItemTimeEdit4";
            this.repositoryItemTimeEdit4.UseMaskAsDisplayFormat = true;
            // 
            // xtpInterruption
            // 
            this.xtpInterruption.Controls.Add(this.gcInterruption);
            this.xtpInterruption.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtpInterruption.Name = "xtpInterruption";
            this.xtpInterruption.Size = new System.Drawing.Size(924, 233);
            this.xtpInterruption.Text = "Duruş Detayı";
            // 
            // gcInterruption
            // 
            this.gcInterruption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcInterruption.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gcInterruption.Location = new System.Drawing.Point(0, 0);
            this.gcInterruption.LookAndFeel.SkinName = "Office 2019 White";
            this.gcInterruption.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcInterruption.MainView = this.gvInterruption;
            this.gcInterruption.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gcInterruption.Name = "gcInterruption";
            this.gcInterruption.Size = new System.Drawing.Size(924, 233);
            this.gcInterruption.TabIndex = 2;
            this.gcInterruption.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvInterruption});
            // 
            // gvInterruption
            // 
            this.gvInterruption.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn6});
            this.gvInterruption.DetailHeight = 508;
            this.gvInterruption.GridControl = this.gcInterruption;
            this.gvInterruption.Name = "gvInterruption";
            this.gvInterruption.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn3, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Id";
            this.gridColumn1.FieldName = "Id";
            this.gridColumn1.MinWidth = 30;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 112;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn2.Caption = "Sebep";
            this.gridColumn2.FieldName = "CauseDescription";
            this.gridColumn2.MinWidth = 30;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 112;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.Caption = "Başlangıç Zamanı";
            this.gridColumn3.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "InterruptionStartDate";
            this.gridColumn3.MinWidth = 30;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 112;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn4.Caption = "Bitiş Zamanı";
            this.gridColumn4.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn4.FieldName = "InterruptionFinishDate";
            this.gridColumn4.MinWidth = 30;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.OptionsColumn.ReadOnly = true;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 112;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn6.Caption = "Geçen Süre";
            this.gridColumn6.FieldName = "gcUnboundColumn";
            this.gridColumn6.MinWidth = 30;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.OptionsColumn.ReadOnly = true;
            this.gridColumn6.UnboundDataType = typeof(double);
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 112;
            // 
            // xtpMachineDown
            // 
            this.xtpMachineDown.Controls.Add(this.gcFaults);
            this.xtpMachineDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtpMachineDown.Name = "xtpMachineDown";
            this.xtpMachineDown.Size = new System.Drawing.Size(924, 233);
            this.xtpMachineDown.Text = "Arıza Detayı";
            // 
            // gcFaults
            // 
            this.gcFaults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcFaults.Location = new System.Drawing.Point(0, 0);
            this.gcFaults.MainView = this.gvFaults;
            this.gcFaults.Name = "gcFaults";
            this.gcFaults.Size = new System.Drawing.Size(924, 233);
            this.gcFaults.TabIndex = 0;
            this.gcFaults.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFaults});
            // 
            // gvFaults
            // 
            this.gvFaults.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19});
            this.gvFaults.DetailHeight = 437;
            this.gvFaults.GridControl = this.gcFaults;
            this.gvFaults.Name = "gvFaults";
            this.gvFaults.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn18, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn17
            // 
            this.gridColumn17.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn17.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn17.AppearanceHeader.Options.UseFont = true;
            this.gridColumn17.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn17.Caption = "Arıza";
            this.gridColumn17.FieldName = "ErrDescription";
            this.gridColumn17.MinWidth = 28;
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsColumn.AllowEdit = false;
            this.gridColumn17.OptionsColumn.AllowFocus = false;
            this.gridColumn17.OptionsColumn.ReadOnly = true;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 0;
            this.gridColumn17.Width = 105;
            // 
            // gridColumn18
            // 
            this.gridColumn18.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn18.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn18.AppearanceHeader.Options.UseFont = true;
            this.gridColumn18.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn18.Caption = "Başlangıc Tarihi";
            this.gridColumn18.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn18.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn18.FieldName = "RegisterDate";
            this.gridColumn18.MinWidth = 28;
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.OptionsColumn.AllowFocus = false;
            this.gridColumn18.OptionsColumn.ReadOnly = true;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 1;
            this.gridColumn18.Width = 105;
            // 
            // gridColumn19
            // 
            this.gridColumn19.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn19.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn19.AppearanceHeader.Options.UseFont = true;
            this.gridColumn19.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn19.Caption = "Bitiş Tarihi";
            this.gridColumn19.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn19.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn19.FieldName = "ActualFinishDate";
            this.gridColumn19.MinWidth = 28;
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.AllowEdit = false;
            this.gridColumn19.OptionsColumn.AllowFocus = false;
            this.gridColumn19.OptionsColumn.ReadOnly = true;
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 2;
            this.gridColumn19.Width = 105;
            // 
            // xtpPersonel
            // 
            this.xtpPersonel.Controls.Add(this.gcPerson);
            this.xtpPersonel.Name = "xtpPersonel";
            this.xtpPersonel.Size = new System.Drawing.Size(924, 233);
            this.xtpPersonel.Text = "Personel";
            // 
            // gcPerson
            // 
            this.gcPerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPerson.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gcPerson.Location = new System.Drawing.Point(0, 0);
            this.gcPerson.MainView = this.gvPerson;
            this.gcPerson.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gcPerson.Name = "gcPerson";
            this.gcPerson.Size = new System.Drawing.Size(924, 233);
            this.gcPerson.TabIndex = 0;
            this.gcPerson.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPerson});
            // 
            // gvPerson
            // 
            this.gvPerson.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gcUnboundPersonPassingTime});
            this.gvPerson.DetailHeight = 538;
            this.gvPerson.GridControl = this.gcPerson;
            this.gvPerson.Name = "gvPerson";
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn13.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn13.AppearanceHeader.Options.UseFont = true;
            this.gridColumn13.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn13.Caption = "CompanyId";
            this.gridColumn13.FieldName = "CompanyId";
            this.gridColumn13.MinWidth = 30;
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.OptionsColumn.AllowFocus = false;
            this.gridColumn13.OptionsColumn.AllowMove = false;
            this.gridColumn13.OptionsColumn.AllowShowHide = false;
            this.gridColumn13.OptionsColumn.AllowSize = false;
            this.gridColumn13.OptionsColumn.ReadOnly = true;
            this.gridColumn13.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn13.Width = 112;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn14.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn14.AppearanceHeader.Options.UseFont = true;
            this.gridColumn14.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn14.Caption = "Personel";
            this.gridColumn14.FieldName = "Name";
            this.gridColumn14.MinWidth = 30;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.AllowEdit = false;
            this.gridColumn14.OptionsColumn.AllowFocus = false;
            this.gridColumn14.OptionsColumn.AllowMove = false;
            this.gridColumn14.OptionsColumn.AllowShowHide = false;
            this.gridColumn14.OptionsColumn.AllowSize = false;
            this.gridColumn14.OptionsColumn.ReadOnly = true;
            this.gridColumn14.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 0;
            this.gridColumn14.Width = 112;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn15.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn15.AppearanceHeader.Options.UseFont = true;
            this.gridColumn15.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn15.Caption = "Seviye";
            this.gridColumn15.FieldName = "LaborClass";
            this.gridColumn15.MinWidth = 30;
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowEdit = false;
            this.gridColumn15.OptionsColumn.AllowFocus = false;
            this.gridColumn15.OptionsColumn.AllowMove = false;
            this.gridColumn15.OptionsColumn.AllowShowHide = false;
            this.gridColumn15.OptionsColumn.AllowSize = false;
            this.gridColumn15.OptionsColumn.ReadOnly = true;
            this.gridColumn15.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 1;
            this.gridColumn15.Width = 112;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn16.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumn16.AppearanceHeader.Options.UseFont = true;
            this.gridColumn16.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn16.Caption = "Giriş Zamanı";
            this.gridColumn16.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn16.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn16.FieldName = "StartDate";
            this.gridColumn16.MinWidth = 30;
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.AllowEdit = false;
            this.gridColumn16.OptionsColumn.AllowFocus = false;
            this.gridColumn16.OptionsColumn.AllowMove = false;
            this.gridColumn16.OptionsColumn.AllowShowHide = false;
            this.gridColumn16.OptionsColumn.AllowSize = false;
            this.gridColumn16.OptionsColumn.ReadOnly = true;
            this.gridColumn16.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 2;
            this.gridColumn16.Width = 112;
            // 
            // gcUnboundPersonPassingTime
            // 
            this.gcUnboundPersonPassingTime.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.gcUnboundPersonPassingTime.AppearanceHeader.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gcUnboundPersonPassingTime.AppearanceHeader.Options.UseFont = true;
            this.gcUnboundPersonPassingTime.AppearanceHeader.Options.UseForeColor = true;
            this.gcUnboundPersonPassingTime.Caption = "Geçen Süre";
            this.gcUnboundPersonPassingTime.FieldName = "gcUnboundPersonPassingTime";
            this.gcUnboundPersonPassingTime.MinWidth = 30;
            this.gcUnboundPersonPassingTime.Name = "gcUnboundPersonPassingTime";
            this.gcUnboundPersonPassingTime.OptionsColumn.AllowEdit = false;
            this.gcUnboundPersonPassingTime.OptionsColumn.AllowFocus = false;
            this.gcUnboundPersonPassingTime.OptionsColumn.AllowMove = false;
            this.gcUnboundPersonPassingTime.OptionsColumn.AllowShowHide = false;
            this.gcUnboundPersonPassingTime.OptionsColumn.AllowSize = false;
            this.gcUnboundPersonPassingTime.OptionsColumn.ReadOnly = true;
            this.gcUnboundPersonPassingTime.OptionsFilter.AllowAutoFilter = false;
            this.gcUnboundPersonPassingTime.UnboundDataType = typeof(int);
            this.gcUnboundPersonPassingTime.Visible = true;
            this.gcUnboundPersonPassingTime.VisibleIndex = 3;
            this.gcUnboundPersonPassingTime.Width = 112;
            // 
            // xtpTimes
            // 
            this.xtpTimes.Controls.Add(this.panelControl7);
            this.xtpTimes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtpTimes.Name = "xtpTimes";
            this.xtpTimes.PageVisible = false;
            this.xtpTimes.Size = new System.Drawing.Size(924, 233);
            this.xtpTimes.Text = "SÜRELER";
            // 
            // panelControl7
            // 
            this.panelControl7.Controls.Add(this.layoutControl1);
            this.panelControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl7.Location = new System.Drawing.Point(0, 0);
            this.panelControl7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl7.Name = "panelControl7";
            this.panelControl7.Size = new System.Drawing.Size(924, 233);
            this.panelControl7.TabIndex = 0;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(920, 229);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.simpleLabelItem1,
            this.simpleLabelItem3,
            this.simpleLabelItem4,
            this.simpleLabelItem5,
            this.simpleLabelItem6,
            this.simpleLabelItem2,
            this.lblTotalWorkTime,
            this.emptySpaceItem1,
            this.lblTotalInterruptionTime,
            this.lblTotalMachineDownTime,
            this.lblSetupTime,
            this.lblLastItemProductionTime,
            this.lblMeanProductionTime});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(30, 30, 31, 31);
            this.Root.Size = new System.Drawing.Size(894, 464);
            this.Root.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleLabelItem1.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.MaxSize = new System.Drawing.Size(0, 62);
            this.simpleLabelItem1.MinSize = new System.Drawing.Size(279, 62);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(408, 62);
            this.simpleLabelItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.simpleLabelItem1.Text = "Toplam Çalışma Süresi :";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(271, 32);
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.AllowHotTrack = false;
            this.simpleLabelItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleLabelItem3.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.simpleLabelItem3.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem3.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem3.Location = new System.Drawing.Point(0, 62);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.Size = new System.Drawing.Size(408, 66);
            this.simpleLabelItem3.Text = "Toplam Duruş Süresi :";
            this.simpleLabelItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(75, 62);
            // 
            // simpleLabelItem4
            // 
            this.simpleLabelItem4.AllowHotTrack = false;
            this.simpleLabelItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleLabelItem4.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.simpleLabelItem4.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem4.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem4.Location = new System.Drawing.Point(0, 128);
            this.simpleLabelItem4.Name = "simpleLabelItem4";
            this.simpleLabelItem4.Size = new System.Drawing.Size(408, 66);
            this.simpleLabelItem4.Text = "Toplam Arıza Süresi :";
            this.simpleLabelItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.simpleLabelItem4.TextSize = new System.Drawing.Size(75, 62);
            // 
            // simpleLabelItem5
            // 
            this.simpleLabelItem5.AllowHotTrack = false;
            this.simpleLabelItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleLabelItem5.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.simpleLabelItem5.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem5.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem5.Location = new System.Drawing.Point(0, 194);
            this.simpleLabelItem5.Name = "simpleLabelItem5";
            this.simpleLabelItem5.Size = new System.Drawing.Size(408, 66);
            this.simpleLabelItem5.Text = "Setup Süresi :";
            this.simpleLabelItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.simpleLabelItem5.TextSize = new System.Drawing.Size(75, 62);
            // 
            // simpleLabelItem6
            // 
            this.simpleLabelItem6.AllowHotTrack = false;
            this.simpleLabelItem6.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleLabelItem6.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.simpleLabelItem6.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem6.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem6.Location = new System.Drawing.Point(0, 260);
            this.simpleLabelItem6.Name = "simpleLabelItem6";
            this.simpleLabelItem6.Size = new System.Drawing.Size(408, 66);
            this.simpleLabelItem6.Text = "Son Parça Üretim Süresi :";
            this.simpleLabelItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.simpleLabelItem6.TextSize = new System.Drawing.Size(75, 62);
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.AllowHotTrack = false;
            this.simpleLabelItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleLabelItem2.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.simpleLabelItem2.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem2.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 326);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.Size = new System.Drawing.Size(408, 66);
            this.simpleLabelItem2.Text = "Ortalama Üretim Süresi :";
            this.simpleLabelItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(75, 62);
            // 
            // lblTotalWorkTime
            // 
            this.lblTotalWorkTime.AllowHotTrack = false;
            this.lblTotalWorkTime.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalWorkTime.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTotalWorkTime.AppearanceItemCaption.Options.UseFont = true;
            this.lblTotalWorkTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblTotalWorkTime.Location = new System.Drawing.Point(408, 0);
            this.lblTotalWorkTime.Name = "lblTotalWorkTime";
            this.lblTotalWorkTime.Size = new System.Drawing.Size(426, 62);
            this.lblTotalWorkTime.Text = "00:00";
            this.lblTotalWorkTime.TextSize = new System.Drawing.Size(271, 32);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 392);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(834, 10);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lblTotalInterruptionTime
            // 
            this.lblTotalInterruptionTime.AllowHotTrack = false;
            this.lblTotalInterruptionTime.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalInterruptionTime.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTotalInterruptionTime.AppearanceItemCaption.Options.UseFont = true;
            this.lblTotalInterruptionTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblTotalInterruptionTime.Location = new System.Drawing.Point(408, 62);
            this.lblTotalInterruptionTime.Name = "lblTotalInterruptionTime";
            this.lblTotalInterruptionTime.Size = new System.Drawing.Size(426, 66);
            this.lblTotalInterruptionTime.Text = "00:00";
            this.lblTotalInterruptionTime.TextSize = new System.Drawing.Size(271, 32);
            // 
            // lblTotalMachineDownTime
            // 
            this.lblTotalMachineDownTime.AllowHotTrack = false;
            this.lblTotalMachineDownTime.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalMachineDownTime.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTotalMachineDownTime.AppearanceItemCaption.Options.UseFont = true;
            this.lblTotalMachineDownTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblTotalMachineDownTime.Location = new System.Drawing.Point(408, 128);
            this.lblTotalMachineDownTime.Name = "lblTotalMachineDownTime";
            this.lblTotalMachineDownTime.Size = new System.Drawing.Size(426, 66);
            this.lblTotalMachineDownTime.Text = "00:00";
            this.lblTotalMachineDownTime.TextSize = new System.Drawing.Size(271, 32);
            // 
            // lblSetupTime
            // 
            this.lblSetupTime.AllowHotTrack = false;
            this.lblSetupTime.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSetupTime.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblSetupTime.AppearanceItemCaption.Options.UseFont = true;
            this.lblSetupTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblSetupTime.Location = new System.Drawing.Point(408, 194);
            this.lblSetupTime.Name = "lblSetupTime";
            this.lblSetupTime.Size = new System.Drawing.Size(426, 66);
            this.lblSetupTime.Text = "00:00";
            this.lblSetupTime.TextSize = new System.Drawing.Size(271, 32);
            // 
            // lblLastItemProductionTime
            // 
            this.lblLastItemProductionTime.AllowHotTrack = false;
            this.lblLastItemProductionTime.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLastItemProductionTime.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblLastItemProductionTime.AppearanceItemCaption.Options.UseFont = true;
            this.lblLastItemProductionTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblLastItemProductionTime.Location = new System.Drawing.Point(408, 260);
            this.lblLastItemProductionTime.Name = "lblLastItemProductionTime";
            this.lblLastItemProductionTime.Size = new System.Drawing.Size(426, 66);
            this.lblLastItemProductionTime.Text = "00:00";
            this.lblLastItemProductionTime.TextSize = new System.Drawing.Size(271, 32);
            // 
            // lblMeanProductionTime
            // 
            this.lblMeanProductionTime.AllowHotTrack = false;
            this.lblMeanProductionTime.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMeanProductionTime.AppearanceItemCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblMeanProductionTime.AppearanceItemCaption.Options.UseFont = true;
            this.lblMeanProductionTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblMeanProductionTime.Location = new System.Drawing.Point(408, 326);
            this.lblMeanProductionTime.Name = "lblMeanProductionTime";
            this.lblMeanProductionTime.Size = new System.Drawing.Size(426, 66);
            this.lblMeanProductionTime.Text = "00:00";
            this.lblMeanProductionTime.TextSize = new System.Drawing.Size(271, 32);
            // 
            // tmrWorkShopOrder
            // 
            this.tmrWorkShopOrder.Interval = 2000;
            // 
            // tmrMailControl
            // 
            this.tmrMailControl.Interval = 1000;
            // 
            // FrmOperator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1666, 929);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmOperator";
            this.Text = "FrmOperator";
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lgTotalProduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsrbcTotalProduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lscTotalProduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ccProductionPerformance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgTargetProduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsrbcTargetProduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lscTargetProduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlReadValueLabels)).EndInit();
            this.pnlReadValueLabels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.container)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMachineStateColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtcDetails)).EndInit();
            this.xtcDetails.ResumeLayout(false);
            this.xtpWorkShopOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcWorkShopOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gwWorkShopOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit4)).EndInit();
            this.xtpInterruption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcInterruption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInterruption)).EndInit();
            this.xtpMachineDown.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcFaults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFaults)).EndInit();
            this.xtpPersonel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPerson)).EndInit();
            this.xtpTimes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).EndInit();
            this.panelControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalWorkTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalInterruptionTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalMachineDownTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSetupTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLastItemProductionTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMeanProductionTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGauges.Win.GaugeControl gcTotalProductionAmount;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearGauge lgTotalProduction;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleRangeBarComponent lsrbcTotalProduction;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent lscTotalProduction;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblPlcLabel;
        private DevExpress.XtraEditors.LabelControl lblPLC;
        private DevExpress.XtraEditors.LabelControl lblTotalProductionCountLabel;
        private DevExpress.XtraEditors.LabelControl lblTotalProductionCount;
        private DevExpress.XtraEditors.LabelControl lblCurrentAmountLabel;
        private DevExpress.XtraEditors.LabelControl lblBoxAmount;
        private DevExpress.XtraEditors.LabelControl lblCurrentAmount;
        private DevExpress.XtraEditors.LabelControl lblRealizeAmountLabel;
        private DevExpress.XtraEditors.LabelControl lblBoxAmountLabel;
        private DevExpress.XtraEditors.LabelControl lblRealizeAmount;
        private DevExpress.XtraEditors.LabelControl lblScrapCount;
        private DevExpress.XtraEditors.LabelControl lblScrapCountLabel;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraCharts.ChartControl ccProductionPerformance;
        private DevExpress.XtraGauges.Win.GaugeControl gcTargetProductionAmount;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearGauge lgTargetProduction;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleRangeBarComponent lsrbcTargetProduction;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent lscTargetProduction;
        private DevExpress.XtraEditors.PanelControl pnlReadValueLabels;
        private DevExpress.XtraEditors.LabelControl lblValue9;
        private DevExpress.XtraEditors.LabelControl lblDescription9;
        private DevExpress.XtraEditors.LabelControl lblValue8;
        private DevExpress.XtraEditors.LabelControl lblDescription8;
        private DevExpress.XtraEditors.LabelControl lblValue7;
        private DevExpress.XtraEditors.LabelControl lblDescription7;
        private DevExpress.XtraEditors.LabelControl lblValue6;
        private DevExpress.XtraEditors.LabelControl lblDescription6;
        private DevExpress.XtraEditors.LabelControl labelControl35;
        private DevExpress.XtraEditors.LabelControl lblValue5;
        private DevExpress.XtraEditors.LabelControl lblDescription5;
        private DevExpress.XtraEditors.LabelControl labelControl40;
        private DevExpress.XtraEditors.LabelControl lblValue4;
        private DevExpress.XtraEditors.LabelControl lblDescription4;
        private DevExpress.XtraEditors.LabelControl labelControl48;
        private DevExpress.XtraEditors.LabelControl lblValue3;
        private DevExpress.XtraEditors.LabelControl lblDescription3;
        private DevExpress.XtraEditors.LabelControl labelControl45;
        private DevExpress.XtraEditors.LabelControl lblValue2;
        private DevExpress.XtraEditors.LabelControl lblDescription2;
        private DevExpress.XtraEditors.LabelControl labelControl42;
        private DevExpress.XtraEditors.LabelControl lblValue1;
        private DevExpress.XtraEditors.LabelControl lblDescription1;
        private DevExpress.XtraEditors.LabelControl labelControl38;
        public DevExpress.XtraEditors.PanelControl container;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.LabelControl lblWorkCenterStartTime;
        private DevExpress.XtraEditors.LabelControl lblWorkOrderStopWatch;
        private DevExpress.XtraEditors.LabelControl lblMachineState;
        private DevExpress.XtraEditors.PanelControl pnlMachineStateColor;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.LabelControl lblProcessBarcode;
        private DevExpress.XtraEditors.TextEdit txtBarcode;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraTab.XtraTabControl xtcDetails;
        private DevExpress.XtraTab.XtraTabPage xtpWorkShopOrder;
        private DevExpress.XtraGrid.GridControl gcWorkShopOrder;
        private DevExpress.XtraGrid.Views.Grid.GridView gwWorkShopOrder;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gcUnboundColumn;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit4;
        private DevExpress.XtraTab.XtraTabPage xtpInterruption;
        private DevExpress.XtraGrid.GridControl gcInterruption;
        private DevExpress.XtraGrid.Views.Grid.GridView gvInterruption;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraTab.XtraTabPage xtpMachineDown;
        private DevExpress.XtraGrid.GridControl gcFaults;
        private DevExpress.XtraGrid.Views.Grid.GridView gvFaults;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
        private DevExpress.XtraTab.XtraTabPage xtpPersonel;
        private DevExpress.XtraGrid.GridControl gcPerson;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPerson;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gcUnboundPersonPassingTime;
        private DevExpress.XtraTab.XtraTabPage xtpTimes;
        private DevExpress.XtraEditors.PanelControl panelControl7;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem3;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem4;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem5;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem6;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem2;
        private DevExpress.XtraLayout.SimpleLabelItem lblTotalWorkTime;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.SimpleLabelItem lblTotalInterruptionTime;
        private DevExpress.XtraLayout.SimpleLabelItem lblTotalMachineDownTime;
        private DevExpress.XtraLayout.SimpleLabelItem lblSetupTime;
        private DevExpress.XtraLayout.SimpleLabelItem lblLastItemProductionTime;
        private DevExpress.XtraLayout.SimpleLabelItem lblMeanProductionTime;
        private System.Windows.Forms.Timer tmrWorkShopOrder;
        private System.Windows.Forms.Timer tmrMailControl;
        private UserControls.ucOEEPanelAdvanced oeePanel;
    }
}