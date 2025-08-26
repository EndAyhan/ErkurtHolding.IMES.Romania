namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    partial class ucMachineDownDuration
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
            this.lblErrDescription = new DevExpress.XtraEditors.LabelControl();
            this.lblWorkTypeID = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblDuration);
            this.groupControl1.Controls.Add(this.lblStartDate);
            this.groupControl1.Controls.Add(this.lblErrDescription);
            this.groupControl1.Controls.Add(this.lblWorkTypeID);
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(863, 528);
            this.groupControl1.TabIndex = 61;
            this.groupControl1.Tag = "100";
            this.groupControl1.Text = "İş Merkezi Arızalı Müdahale Bekliyor";
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
            this.lblDuration.Location = new System.Drawing.Point(2, 322);
            this.lblDuration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(859, 110);
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
            this.lblStartDate.Location = new System.Drawing.Point(2, 260);
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(859, 62);
            this.lblStartDate.TabIndex = 1;
            this.lblStartDate.Text = "00:00:00";
            // 
            // lblErrDescription
            // 
            this.lblErrDescription.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrDescription.Appearance.Options.UseFont = true;
            this.lblErrDescription.Appearance.Options.UseTextOptions = true;
            this.lblErrDescription.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblErrDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblErrDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblErrDescription.Location = new System.Drawing.Point(2, 198);
            this.lblErrDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblErrDescription.Name = "lblErrDescription";
            this.lblErrDescription.Size = new System.Drawing.Size(859, 62);
            this.lblErrDescription.TabIndex = 4;
            this.lblErrDescription.Text = "ÇAY MOLASINA ÇIKTIK";
            // 
            // lblWorkTypeID
            // 
            this.lblWorkTypeID.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkTypeID.Appearance.Options.UseFont = true;
            this.lblWorkTypeID.Appearance.Options.UseTextOptions = true;
            this.lblWorkTypeID.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblWorkTypeID.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblWorkTypeID.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWorkTypeID.Location = new System.Drawing.Point(2, 136);
            this.lblWorkTypeID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblWorkTypeID.Name = "lblWorkTypeID";
            this.lblWorkTypeID.Size = new System.Drawing.Size(859, 62);
            this.lblWorkTypeID.TabIndex = 3;
            this.lblWorkTypeID.Text = "GENEL";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 28);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(859, 108);
            this.panel1.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ucMachineDownDuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.LookAndFeel.SkinName = "Office 2019 Colorful";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucMachineDownDuration";
            this.Size = new System.Drawing.Size(863, 528);
            this.Tag = "044";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblDuration;
        private DevExpress.XtraEditors.LabelControl lblStartDate;
        private DevExpress.XtraEditors.LabelControl lblErrDescription;
        private DevExpress.XtraEditors.LabelControl lblWorkTypeID;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
    }
}
