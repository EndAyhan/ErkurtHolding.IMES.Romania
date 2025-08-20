namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms
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
            this.lblLabel = new DevExpress.XtraEditors.LabelControl();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.lblLabel.Location = new System.Drawing.Point(24, 19);
            this.lblLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(182, 44);
            this.lblLabel.TabIndex = 3;
            this.lblLabel.Text = "Admin Şifresi:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(215, 15);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(348, 50);
            this.txtPassword.TabIndex = 2;
            // 
            // FrmAdminLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 80);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.txtPassword);
            this.Name = "FrmAdminLogin";
            this.Text = "Admin Login";
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblLabel;
        private DevExpress.XtraEditors.TextEdit txtPassword;
    }
}