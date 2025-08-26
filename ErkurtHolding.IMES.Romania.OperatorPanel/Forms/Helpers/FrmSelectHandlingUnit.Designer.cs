namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    partial class FrmSelectHandlingUnit
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
            this.gcHandlingUnits = new DevExpress.XtraGrid.GridControl();
            this.gvHandlingUnits = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtOrder = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtRessource = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcHandlingUnits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHandlingUnits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRessource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcHandlingUnits
            // 
            this.gcHandlingUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcHandlingUnits.Location = new System.Drawing.Point(2, 2);
            this.gcHandlingUnits.MainView = this.gvHandlingUnits;
            this.gcHandlingUnits.Name = "gcHandlingUnits";
            this.gcHandlingUnits.Size = new System.Drawing.Size(568, 228);
            this.gcHandlingUnits.TabIndex = 0;
            this.gcHandlingUnits.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHandlingUnits});
            this.gcHandlingUnits.DoubleClick += new System.EventHandler(this.gcHandlinbgUnits_DoubleClick);
            // 
            // gvHandlingUnits
            // 
            this.gvHandlingUnits.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gvHandlingUnits.GridControl = this.gcHandlingUnits;
            this.gvHandlingUnits.Name = "gvHandlingUnits";
            this.gvHandlingUnits.OptionsBehavior.Editable = false;
            this.gvHandlingUnits.OptionsMenu.ShowFooterItem = true;
            this.gvHandlingUnits.OptionsView.ShowFooter = true;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Barkod";
            this.gridColumn1.FieldName = "BoxBarcode";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Tag = "500";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 194;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Miktar";
            this.gridColumn2.FieldName = "Quantity";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Tag = "501";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 79;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Tarih";
            this.gridColumn3.DisplayFormat.FormatString = "dd.MM.yyy HH:mm:ss";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "CreatedAt";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Tag = "502";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 232;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtOrder);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.txtRessource);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(572, 80);
            this.panelControl1.TabIndex = 1;
            // 
            // txtOrder
            // 
            this.txtOrder.Location = new System.Drawing.Point(289, 35);
            this.txtOrder.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtOrder.Name = "txtOrder";
            this.txtOrder.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtOrder.Properties.Appearance.Options.UseFont = true;
            this.txtOrder.Properties.ReadOnly = true;
            this.txtOrder.Size = new System.Drawing.Size(267, 28);
            this.txtOrder.TabIndex = 37;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(289, 7);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(267, 21);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Tag = "201";
            this.labelControl2.Text = "İş Emri";
            // 
            // txtRessource
            // 
            this.txtRessource.Location = new System.Drawing.Point(14, 35);
            this.txtRessource.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtRessource.Name = "txtRessource";
            this.txtRessource.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtRessource.Properties.Appearance.Options.UseFont = true;
            this.txtRessource.Properties.ReadOnly = true;
            this.txtRessource.Size = new System.Drawing.Size(267, 28);
            this.txtRessource.TabIndex = 35;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(14, 7);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(265, 21);
            this.labelControl1.TabIndex = 34;
            this.labelControl1.Tag = "200";
            this.labelControl1.Text = "İş Merkezi";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcHandlingUnits);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 80);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(572, 232);
            this.panelControl2.TabIndex = 0;
            // 
            // FrmSelectHandlingUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 312);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectHandlingUnit";
            this.Tag = "027";
            this.Text = "Yarım Kasalar";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.gcHandlingUnits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHandlingUnits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRessource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txtOrder;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtRessource;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gcHandlingUnits;
        private DevExpress.XtraGrid.Views.Grid.GridView gvHandlingUnits;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}