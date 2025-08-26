namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    partial class ucInterruptionCause
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucInterruptionCause));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblEquipment = new DevExpress.XtraEditors.LabelControl();
            this.cmbResources = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.flpDescriptions = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.flpContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblLine = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.deStartDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbResources.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblEquipment);
            this.groupControl1.Controls.Add(this.cmbResources);
            this.groupControl1.Controls.Add(this.panelControl2);
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Controls.Add(this.btnSave);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.btnCancel);
            this.groupControl1.Controls.Add(this.labelControl16);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.lblLine);
            this.groupControl1.Controls.Add(this.labelControl14);
            this.groupControl1.Controls.Add(this.deStartDate);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl12);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1005, 571);
            this.groupControl1.TabIndex = 59;
            this.groupControl1.Tag = "100";
            this.groupControl1.Text = "Yeni Duruş Olayı Ekle";
            // 
            // lblEquipment
            // 
            this.lblEquipment.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipment.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblEquipment.Appearance.Options.UseFont = true;
            this.lblEquipment.Appearance.Options.UseForeColor = true;
            this.lblEquipment.Appearance.Options.UseTextOptions = true;
            this.lblEquipment.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblEquipment.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblEquipment.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblEquipment.Location = new System.Drawing.Point(17, 231);
            this.lblEquipment.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.lblEquipment.Name = "lblEquipment";
            this.lblEquipment.Size = new System.Drawing.Size(187, 263);
            this.lblEquipment.TabIndex = 61;
            // 
            // cmbResources
            // 
            this.cmbResources.EditValue = "";
            this.cmbResources.Location = new System.Drawing.Point(17, 175);
            this.cmbResources.Margin = new System.Windows.Forms.Padding(4);
            this.cmbResources.Name = "cmbResources";
            this.cmbResources.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbResources.Properties.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbResources.Properties.Appearance.Options.UseFont = true;
            this.cmbResources.Properties.Appearance.Options.UseForeColor = true;
            this.cmbResources.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbResources.Properties.SelectAllItemVisible = false;
            this.cmbResources.Size = new System.Drawing.Size(187, 26);
            this.cmbResources.TabIndex = 60;
            this.cmbResources.EditValueChanged += new System.EventHandler(this.cmbResources_EditValueChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.flpDescriptions);
            this.panelControl2.Location = new System.Drawing.Point(276, 164);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(5);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(712, 54);
            this.panelControl2.TabIndex = 59;
            // 
            // flpDescriptions
            // 
            this.flpDescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpDescriptions.Location = new System.Drawing.Point(0, 0);
            this.flpDescriptions.Margin = new System.Windows.Forms.Padding(4);
            this.flpDescriptions.Name = "flpDescriptions";
            this.flpDescriptions.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.flpDescriptions.Size = new System.Drawing.Size(712, 54);
            this.flpDescriptions.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.flpContainer);
            this.panelControl1.Location = new System.Drawing.Point(276, 230);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(714, 257);
            this.panelControl1.TabIndex = 51;
            // 
            // flpContainer
            // 
            this.flpContainer.AutoScroll = true;
            this.flpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpContainer.Location = new System.Drawing.Point(2, 2);
            this.flpContainer.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.flpContainer.Name = "flpContainer";
            this.flpContainer.Size = new System.Drawing.Size(710, 253);
            this.flpContainer.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Enabled = false;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.Location = new System.Drawing.Point(828, 491);
            this.btnSave.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSave.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnSave.Size = new System.Drawing.Size(163, 60);
            this.btnSave.TabIndex = 56;
            this.btnSave.Tag = "400";
            this.btnSave.Text = "Kaydet";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(293, 129);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(440, 21);
            this.labelControl4.TabIndex = 52;
            this.labelControl4.Tag = "204";
            this.labelControl4.Text = "Duruş Sebebi";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(588, 46);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(229, 21);
            this.labelControl1.TabIndex = 55;
            this.labelControl1.Tag = "203";
            this.labelControl1.Text = "Açıklama";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.Location = new System.Drawing.Point(650, 491);
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnCancel.Size = new System.Drawing.Size(167, 60);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Tag = "401";
            this.btnCancel.Text = "Vazgeç";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelControl16
            // 
            this.labelControl16.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl16.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelControl16.Appearance.Options.UseFont = true;
            this.labelControl16.Appearance.Options.UseForeColor = true;
            this.labelControl16.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl16.Location = new System.Drawing.Point(17, 46);
            this.labelControl16.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(187, 20);
            this.labelControl16.TabIndex = 42;
            this.labelControl16.Tag = "200";
            this.labelControl16.Text = "Başlangıç Zamanı";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(353, 46);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(209, 21);
            this.labelControl2.TabIndex = 54;
            this.labelControl2.Tag = "202";
            this.labelControl2.Text = "Duruş Seçimi";
            // 
            // lblLine
            // 
            this.lblLine.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.lblLine.Appearance.Options.UseBackColor = true;
            this.lblLine.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblLine.Location = new System.Drawing.Point(353, 86);
            this.lblLine.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(217, 6);
            this.lblLine.TabIndex = 53;
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl14.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelControl14.Appearance.Options.UseFont = true;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl14.Location = new System.Drawing.Point(17, 140);
            this.labelControl14.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(187, 20);
            this.labelControl14.TabIndex = 46;
            this.labelControl14.Tag = "201";
            this.labelControl14.Text = "Ekipman";
            // 
            // deStartDate
            // 
            this.deStartDate.EditValue = null;
            this.deStartDate.Location = new System.Drawing.Point(17, 81);
            this.deStartDate.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.deStartDate.Name = "deStartDate";
            this.deStartDate.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.deStartDate.Properties.Appearance.Options.UseFont = true;
            this.deStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStartDate.Properties.DisplayFormat.FormatString = "G";
            this.deStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartDate.Properties.MaskSettings.Set("mask", "G");
            this.deStartDate.Properties.ReadOnly = true;
            this.deStartDate.Size = new System.Drawing.Size(187, 26);
            this.deStartDate.TabIndex = 41;
            this.deStartDate.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.deStartDate_QueryPopUp);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl5.Appearance.Options.UseBackColor = true;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(276, 89);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(643, 2);
            this.labelControl5.TabIndex = 50;
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelControl12.Appearance.Options.UseBackColor = true;
            this.labelControl12.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl12.Location = new System.Drawing.Point(235, 46);
            this.labelControl12.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(3, 492);
            this.labelControl12.TabIndex = 49;
            // 
            // ucInterruptionCause
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.LookAndFeel.SkinName = "Office 2019 Colorful";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucInterruptionCause";
            this.Size = new System.Drawing.Size(1005, 571);
            this.Tag = "040";
            this.Load += new System.EventHandler(this.ucInterruptionCause_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbResources.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.FlowLayoutPanel flpDescriptions;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.FlowLayoutPanel flpContainer;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lblLine;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.DateEdit deStartDate;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cmbResources;
        private DevExpress.XtraEditors.LabelControl lblEquipment;
    }
}
