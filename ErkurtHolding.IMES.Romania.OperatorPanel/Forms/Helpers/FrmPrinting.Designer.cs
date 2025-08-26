namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    partial class FrmPrinting
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
            this.gcBox = new DevExpress.XtraEditors.GroupControl();
            this.lblPrintingText = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcBox)).BeginInit();
            this.gcBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcBox
            // 
            this.gcBox.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.gcBox.Appearance.Options.UseBackColor = true;
            this.gcBox.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcBox.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            this.gcBox.AppearanceCaption.Options.UseFont = true;
            this.gcBox.AppearanceCaption.Options.UseForeColor = true;
            this.gcBox.Controls.Add(this.lblPrintingText);
            this.gcBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBox.Location = new System.Drawing.Point(0, 0);
            this.gcBox.Margin = new System.Windows.Forms.Padding(4);
            this.gcBox.Name = "gcBox";
            this.gcBox.Size = new System.Drawing.Size(367, 130);
            this.gcBox.TabIndex = 0;
            this.gcBox.Tag = "100";
            this.gcBox.Text = "Etiket Yazdırılıyor..";
            // 
            // lblPrintingText
            // 
            this.lblPrintingText.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lblPrintingText.Appearance.Options.UseFont = true;
            this.lblPrintingText.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPrintingText.Location = new System.Drawing.Point(16, 59);
            this.lblPrintingText.Margin = new System.Windows.Forms.Padding(4);
            this.lblPrintingText.Name = "lblPrintingText";
            this.lblPrintingText.Size = new System.Drawing.Size(338, 20);
            this.lblPrintingText.TabIndex = 0;
            this.lblPrintingText.Tag = "200";
            this.lblPrintingText.Text = "Etiket Yazdırılıyor...";
            // 
            // FrmPrinting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 130);
            this.Controls.Add(this.gcBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmPrinting";
            this.Tag = "023";
            this.Text = "FrmPrinting";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FrmPrinting_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gcBox)).EndInit();
            this.gcBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblPrintingText;
        private DevExpress.XtraEditors.GroupControl gcBox;
    }
}