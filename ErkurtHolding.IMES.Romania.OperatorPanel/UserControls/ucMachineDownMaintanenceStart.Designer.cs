namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    partial class ucMachineDownMaintanenceStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMachineDownMaintanenceStart));
            this.grpMain = new DevExpress.XtraEditors.GroupControl();
            this.btnMachineLockActive = new DevExpress.XtraEditors.SimpleButton();
            this.btnMachineLockFalse = new DevExpress.XtraEditors.SimpleButton();
            this.btnLogOut = new DevExpress.XtraEditors.SimpleButton();
            this.lblDuration = new DevExpress.XtraEditors.LabelControl();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUnboundColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grpMain)).BeginInit();
            this.grpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            this.grpMain.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.grpMain.AppearanceCaption.Options.UseFont = true;
            this.grpMain.Controls.Add(this.btnMachineLockActive);
            this.grpMain.Controls.Add(this.btnMachineLockFalse);
            this.grpMain.Controls.Add(this.btnLogOut);
            this.grpMain.Controls.Add(this.lblDuration);
            this.grpMain.Controls.Add(this.btnLogin);
            this.grpMain.Controls.Add(this.gridControl1);
            this.grpMain.Location = new System.Drawing.Point(0, 0);
            this.grpMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(1000, 577);
            this.grpMain.TabIndex = 0;
            this.grpMain.Text = "Arıza Başlama Zamanı : 01.01.2023";
            // 
            // btnMachineLockActive
            // 
            this.btnMachineLockActive.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachineLockActive.Appearance.Options.UseFont = true;
            this.btnMachineLockActive.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnMachineLockActive.ImageOptions.Image")));
            this.btnMachineLockActive.Location = new System.Drawing.Point(709, 91);
            this.btnMachineLockActive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMachineLockActive.Name = "btnMachineLockActive";
            this.btnMachineLockActive.Size = new System.Drawing.Size(268, 52);
            this.btnMachineLockActive.TabIndex = 6;
            this.btnMachineLockActive.Tag = "403";
            this.btnMachineLockActive.Text = "Mak. Kilit Aktif";
            this.btnMachineLockActive.Click += new System.EventHandler(this.btnMachineLockActive_Click);
            // 
            // btnMachineLockFalse
            // 
            this.btnMachineLockFalse.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachineLockFalse.Appearance.Options.UseFont = true;
            this.btnMachineLockFalse.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnMachineLockFalse.ImageOptions.Image")));
            this.btnMachineLockFalse.Location = new System.Drawing.Point(709, 32);
            this.btnMachineLockFalse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMachineLockFalse.Name = "btnMachineLockFalse";
            this.btnMachineLockFalse.Size = new System.Drawing.Size(268, 52);
            this.btnMachineLockFalse.TabIndex = 5;
            this.btnMachineLockFalse.Tag = "402";
            this.btnMachineLockFalse.Text = "Mak. Kilit Kaldır";
            this.btnMachineLockFalse.Click += new System.EventHandler(this.btnMachineLockFalse_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.Appearance.Options.UseFont = true;
            this.btnLogOut.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnLogOut.ImageOptions.Image")));
            this.btnLogOut.Location = new System.Drawing.Point(547, 91);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(155, 52);
            this.btnLogOut.TabIndex = 4;
            this.btnLogOut.Tag = "401";
            this.btnLogOut.Text = "Çıkış Yap";
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // lblDuration
            // 
            this.lblDuration.Appearance.Font = new System.Drawing.Font("Segoe UI", 50.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblDuration.Appearance.Options.UseFont = true;
            this.lblDuration.Appearance.Options.UseForeColor = true;
            this.lblDuration.Appearance.Options.UseTextOptions = true;
            this.lblDuration.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDuration.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDuration.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDuration.Location = new System.Drawing.Point(2, 32);
            this.lblDuration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(536, 118);
            this.lblDuration.TabIndex = 3;
            this.lblDuration.Text = "00:00:00";
            // 
            // btnLogin
            // 
            this.btnLogin.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Appearance.Options.UseFont = true;
            this.btnLogin.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.ImageOptions.Image")));
            this.btnLogin.Location = new System.Drawing.Point(547, 32);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(155, 52);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Tag = "400";
            this.btnLogin.Text = "Giriş Yap";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControl1.Location = new System.Drawing.Point(2, 150);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(996, 425);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gcUnboundColumn});
            this.gridView1.DetailHeight = 431;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.RowHeight = 49;
            this.gridView1.RowSeparatorHeight = 4;
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.Caption = "Personel Adı";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.MinWidth = 27;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Tag = "500";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 100;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "EmployeeId";
            this.gridColumn2.FieldName = "IfsEmplooyeId";
            this.gridColumn2.MinWidth = 27;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Width = 100;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.Caption = "Müdahale Başlama Zamanı";
            this.gridColumn3.DisplayFormat.FormatString = "d/M/yyyy HH:mm:ss";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "StartDate";
            this.gridColumn3.MinWidth = 27;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Tag = "501";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 100;
            // 
            // gcUnboundColumn
            // 
            this.gcUnboundColumn.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gcUnboundColumn.AppearanceHeader.Options.UseFont = true;
            this.gcUnboundColumn.Caption = "Geçen Süre";
            this.gcUnboundColumn.FieldName = "gcUnboundColumn";
            this.gcUnboundColumn.MinWidth = 27;
            this.gcUnboundColumn.Name = "gcUnboundColumn";
            this.gcUnboundColumn.Tag = "502";
            this.gcUnboundColumn.UnboundDataType = typeof(double);
            this.gcUnboundColumn.Visible = true;
            this.gcUnboundColumn.VisibleIndex = 2;
            this.gcUnboundColumn.Width = 100;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ucMachineDownMaintanenceStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpMain);
            this.LookAndFeel.SkinName = "Office 2019 Colorful";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucMachineDownMaintanenceStart";
            this.Size = new System.Drawing.Size(1000, 577);
            this.Tag = "046";
            ((System.ComponentModel.ISupportInitialize)(this.grpMain)).EndInit();
            this.grpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grpMain;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gcUnboundColumn;
        private DevExpress.XtraEditors.LabelControl lblDuration;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btnLogOut;
        private DevExpress.XtraEditors.SimpleButton btnMachineLockFalse;
        private DevExpress.XtraEditors.SimpleButton btnMachineLockActive;
    }
}
