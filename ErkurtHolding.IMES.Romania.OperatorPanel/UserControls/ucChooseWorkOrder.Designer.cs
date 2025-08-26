namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    partial class ucChooseWorkOrder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucChooseWorkOrder));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barBtnChoose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.batBtnCancel = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.gcWorkOrders = new DevExpress.XtraGrid.GridControl();
            this.gvWorkOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PartNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PartName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OrderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcWorkOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWorkOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(1083, 19);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Tag = "200";
            this.labelControl1.Text = "İŞ EMRİ SEÇME EKRANI";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(1083, 0);
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowItemAnimatedHighlighting = false;
            this.barManager1.AllowMdiChildButtons = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.CloseButtonAffectAllTabs = false;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barBtnChoose,
            this.batBtnCancel});
            this.barManager1.MainMenu = this.bar1;
            this.barManager1.MaxItemId = 3;
            this.barManager1.RightToLeft = DevExpress.Utils.DefaultBoolean.True;
            // 
            // bar1
            // 
            this.bar1.BarName = "Main Menu";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnChoose),
            new DevExpress.XtraBars.LinkPersistInfo(this.batBtnCancel)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.RotateWhenVertical = false;
            this.bar1.Text = "Main Menu";
            // 
            // barBtnChoose
            // 
            this.barBtnChoose.Caption = "İş Emri Seç";
            this.barBtnChoose.Id = 1;
            this.barBtnChoose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barBtnChoose.ImageOptions.SvgImage")));
            this.barBtnChoose.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.barBtnChoose.ImageOptions.SvgImageSize = new System.Drawing.Size(64, 64);
            this.barBtnChoose.ItemAppearance.Normal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.barBtnChoose.ItemAppearance.Normal.ForeColor = System.Drawing.SystemColors.GrayText;
            this.barBtnChoose.ItemAppearance.Normal.Options.UseFont = true;
            this.barBtnChoose.ItemAppearance.Normal.Options.UseForeColor = true;
            this.barBtnChoose.Name = "barBtnChoose";
            this.barBtnChoose.Tag = "400";
            this.barBtnChoose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnChoose_ItemClick);
            // 
            // batBtnCancel
            // 
            this.batBtnCancel.Caption = "Vazgeç";
            this.batBtnCancel.Id = 2;
            this.batBtnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("batBtnCancel.ImageOptions.SvgImage")));
            this.batBtnCancel.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
            this.batBtnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(64, 64);
            this.batBtnCancel.ItemAppearance.Normal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.batBtnCancel.ItemAppearance.Normal.ForeColor = System.Drawing.SystemColors.GrayText;
            this.batBtnCancel.ItemAppearance.Normal.Options.UseFont = true;
            this.batBtnCancel.ItemAppearance.Normal.Options.UseForeColor = true;
            this.batBtnCancel.Name = "batBtnCancel";
            this.batBtnCancel.Tag = "401";
            this.batBtnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.batBtnCancel_ItemClick);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 560);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(1083, 123);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 560);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1083, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 560);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // gcWorkOrders
            // 
            this.gcWorkOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcWorkOrders.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcWorkOrders.Location = new System.Drawing.Point(0, 19);
            this.gcWorkOrders.MainView = this.gvWorkOrders;
            this.gcWorkOrders.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcWorkOrders.MenuManager = this.barManager1;
            this.gcWorkOrders.Name = "gcWorkOrders";
            this.gcWorkOrders.Size = new System.Drawing.Size(1083, 541);
            this.gcWorkOrders.TabIndex = 4;
            this.gcWorkOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvWorkOrders});
            // 
            // gvWorkOrders
            // 
            this.gvWorkOrders.Appearance.HeaderPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.gvWorkOrders.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvWorkOrders.Appearance.Row.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.gvWorkOrders.Appearance.Row.Options.UseFont = true;
            this.gvWorkOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Id,
            this.PartNo,
            this.PartName,
            this.OrderNo});
            this.gvWorkOrders.DetailHeight = 431;
            this.gvWorkOrders.GridControl = this.gcWorkOrders;
            this.gvWorkOrders.Name = "gvWorkOrders";
            this.gvWorkOrders.OptionsView.ShowGroupPanel = false;
            this.gvWorkOrders.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gvWorkOrders_SelectionChanged);
            // 
            // Id
            // 
            this.Id.Caption = "Id";
            this.Id.FieldName = "Id";
            this.Id.MinWidth = 27;
            this.Id.Name = "Id";
            this.Id.Width = 100;
            // 
            // PartNo
            // 
            this.PartNo.Caption = "Ürün Kodu";
            this.PartNo.FieldName = "PartNo";
            this.PartNo.MinWidth = 27;
            this.PartNo.Name = "PartNo";
            this.PartNo.Tag = "500";
            this.PartNo.Visible = true;
            this.PartNo.VisibleIndex = 0;
            this.PartNo.Width = 216;
            // 
            // PartName
            // 
            this.PartName.Caption = "Ürün Açıklaması";
            this.PartName.FieldName = "productDescription";
            this.PartName.MinWidth = 27;
            this.PartName.Name = "PartName";
            this.PartName.Tag = "501";
            this.PartName.Visible = true;
            this.PartName.VisibleIndex = 1;
            this.PartName.Width = 639;
            // 
            // OrderNo
            // 
            this.OrderNo.Caption = "İş Emri No";
            this.OrderNo.FieldName = "orderNo";
            this.OrderNo.MinWidth = 27;
            this.OrderNo.Name = "OrderNo";
            this.OrderNo.Tag = "502";
            this.OrderNo.Visible = true;
            this.OrderNo.VisibleIndex = 2;
            this.OrderNo.Width = 189;
            // 
            // ucChooseWorkOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcWorkOrders);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucChooseWorkOrder";
            this.Size = new System.Drawing.Size(1083, 683);
            this.Tag = "038";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcWorkOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWorkOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarLargeButtonItem barBtnChoose;
        private DevExpress.XtraBars.BarLargeButtonItem batBtnCancel;
        private DevExpress.XtraGrid.GridControl gcWorkOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView gvWorkOrders;
        private DevExpress.XtraGrid.Columns.GridColumn Id;
        private DevExpress.XtraGrid.Columns.GridColumn PartNo;
        private DevExpress.XtraGrid.Columns.GridColumn PartName;
        private DevExpress.XtraGrid.Columns.GridColumn OrderNo;
    }
}
