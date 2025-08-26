namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    partial class FrmAdminLogin
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
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(167, 4);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(309, 42);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // lblLabel
            // 
            this.lblLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12.25F, System.Drawing.FontStyle.Bold);
            this.lblLabel.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblLabel.Appearance.Options.UseFont = true;
            this.lblLabel.Appearance.Options.UseForeColor = true;
            this.lblLabel.Appearance.Options.UseTextOptions = true;
            this.lblLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblLabel.Location = new System.Drawing.Point(-3, 7);
            this.lblLabel.Margin = new System.Windows.Forms.Padding(4);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(162, 35);
            this.lblLabel.TabIndex = 1;
            this.lblLabel.Tag = "200";
            this.lblLabel.Text = "Admin Şifresi:";
            // 
            // FrmAdminLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 64);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.txtPassword);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAdminLogin";
            this.ShowInTaskbar = false;
            this.Tag = "011";
            this.Text = "Admin Login";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblLabel;
        private DevExpress.XtraEditors.TextEdit txtPassword;
    }
}