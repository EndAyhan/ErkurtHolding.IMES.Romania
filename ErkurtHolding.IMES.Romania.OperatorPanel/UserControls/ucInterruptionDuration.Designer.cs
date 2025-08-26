namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    partial class ucInterruptionDuration
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblDuration = new DevExpress.XtraEditors.LabelControl();
            this.lblStartDate = new DevExpress.XtraEditors.LabelControl();
            this.lblDescription = new DevExpress.XtraEditors.LabelControl();
            this.lblAlan2 = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnLockFalse = new DevExpress.XtraEditors.SimpleButton();
            this.btnLockTrue = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblDuration);
            this.groupControl1.Controls.Add(this.lblStartDate);
            this.groupControl1.Controls.Add(this.lblDescription);
            this.groupControl1.Controls.Add(this.lblAlan2);
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(521, 369);
            this.groupControl1.TabIndex = 60;
            this.groupControl1.Tag = "100";
            this.groupControl1.Text = "Yeni Duruş Olayı Ekle";
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
            this.lblDuration.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDuration.Location = new System.Drawing.Point(2, 261);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(517, 89);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "00:00:00";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Appearance.Options.UseFont = true;
            this.lblStartDate.Appearance.Options.UseTextOptions = true;
            this.lblStartDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblStartDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblStartDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStartDate.Location = new System.Drawing.Point(2, 211);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(517, 50);
            this.lblStartDate.TabIndex = 1;
            this.lblStartDate.Text = "00:00:00";
            // 
            // lblDescription
            // 
            this.lblDescription.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Appearance.Options.UseFont = true;
            this.lblDescription.Appearance.Options.UseTextOptions = true;
            this.lblDescription.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescription.Location = new System.Drawing.Point(2, 161);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(517, 50);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "ÇAY MOLASINA ÇIKTIK";
            // 
            // lblAlan2
            // 
            this.lblAlan2.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlan2.Appearance.Options.UseFont = true;
            this.lblAlan2.Appearance.Options.UseTextOptions = true;
            this.lblAlan2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblAlan2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblAlan2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlan2.Location = new System.Drawing.Point(2, 111);
            this.lblAlan2.Name = "lblAlan2";
            this.lblAlan2.Size = new System.Drawing.Size(517, 50);
            this.lblAlan2.TabIndex = 3;
            this.lblAlan2.Text = "GENEL";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLockTrue);
            this.panel1.Controls.Add(this.btnLockFalse);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 88);
            this.panel1.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnLockFalse
            // 
            this.btnLockFalse.Appearance.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.btnLockFalse.Appearance.Options.UseFont = true;
            this.btnLockFalse.Location = new System.Drawing.Point(360, 1);
            this.btnLockFalse.Name = "btnLockFalse";
            this.btnLockFalse.Size = new System.Drawing.Size(154, 43);
            this.btnLockFalse.TabIndex = 0;
            this.btnLockFalse.Text = "MAKİNE KİLİT KALDIR";
            this.btnLockFalse.Click += new System.EventHandler(this.btnLockFalse_Click);
            // 
            // btnLockTrue
            // 
            this.btnLockTrue.Appearance.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.btnLockTrue.Appearance.Options.UseFont = true;
            this.btnLockTrue.Location = new System.Drawing.Point(360, 44);
            this.btnLockTrue.Name = "btnLockTrue";
            this.btnLockTrue.Size = new System.Drawing.Size(154, 43);
            this.btnLockTrue.TabIndex = 1;
            this.btnLockTrue.Text = "MAKİNE KİLİT AKTİF";
            this.btnLockTrue.Click += new System.EventHandler(this.btnLockTrue_Click);
            // 
            // ucInterruptionDration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.LookAndFeel.SkinName = "Office 2019 Colorful";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "ucInterruptionDration";
            this.Size = new System.Drawing.Size(521, 369);
            this.Tag = "041";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.LabelControl lblDescription;
        private DevExpress.XtraEditors.LabelControl lblAlan2;
        private DevExpress.XtraEditors.LabelControl lblDuration;
        private DevExpress.XtraEditors.LabelControl lblStartDate;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnLockTrue;
        private DevExpress.XtraEditors.SimpleButton btnLockFalse;
    }
}
