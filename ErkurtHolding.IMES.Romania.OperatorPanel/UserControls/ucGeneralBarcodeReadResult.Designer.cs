namespace ErkurtHolding.IMES.Romania.OperatorPanel.UserControls
{
    partial class ucGeneralBarcodeReadResult
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.background = new System.Windows.Forms.Panel();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.lblAmount = new DevExpress.XtraEditors.LabelControl();
            this.lblWorkCenter = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.background.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.background);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(933, 369);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Tag = "100";
            this.groupControl1.Text = "Barkod Okuma";
            // 
            // background
            // 
            this.background.BackColor = System.Drawing.Color.Green;
            this.background.Controls.Add(this.lblStatus);
            this.background.Controls.Add(this.lblAmount);
            this.background.Controls.Add(this.lblWorkCenter);
            this.background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.background.ForeColor = System.Drawing.Color.White;
            this.background.Location = new System.Drawing.Point(2, 41);
            this.background.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.background.Name = "background";
            this.background.Size = new System.Drawing.Size(929, 326);
            this.background.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Appearance.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Appearance.Options.UseFont = true;
            this.lblStatus.Appearance.Options.UseTextOptions = true;
            this.lblStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblStatus.Location = new System.Drawing.Point(-3, 220);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(928, 62);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "OK / NOK";
            // 
            // lblAmount
            // 
            this.lblAmount.Appearance.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.lblAmount.Appearance.Options.UseFont = true;
            this.lblAmount.Appearance.Options.UseTextOptions = true;
            this.lblAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblAmount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblAmount.Location = new System.Drawing.Point(3, 130);
            this.lblAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(928, 82);
            this.lblAmount.TabIndex = 1;
            this.lblAmount.Text = "Amount";
            // 
            // lblWorkCenter
            // 
            this.lblWorkCenter.Appearance.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.lblWorkCenter.Appearance.Options.UseFont = true;
            this.lblWorkCenter.Appearance.Options.UseTextOptions = true;
            this.lblWorkCenter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblWorkCenter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblWorkCenter.Location = new System.Drawing.Point(3, 17);
            this.lblWorkCenter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblWorkCenter.Name = "lblWorkCenter";
            this.lblWorkCenter.Size = new System.Drawing.Size(928, 106);
            this.lblWorkCenter.TabIndex = 0;
            this.lblWorkCenter.Text = "WorkCenter";
            // 
            // ucGeneralBarcodeReadResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucGeneralBarcodeReadResult";
            this.Size = new System.Drawing.Size(933, 369);
            this.Tag = "039";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.background.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Panel background;
        private DevExpress.XtraEditors.LabelControl lblAmount;
        private DevExpress.XtraEditors.LabelControl lblWorkCenter;
        private DevExpress.XtraEditors.LabelControl lblStatus;
    }
}
