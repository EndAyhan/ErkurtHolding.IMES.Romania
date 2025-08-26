namespace ErkurtHolding.IMES.Romania.OperatorPanel.Components
{
    partial class ucMachineAutoMaintanenceCheckListComponent
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
            this.chkCheck = new DevExpress.XtraEditors.CheckEdit();
            this.glueComponents = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chkCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glueComponents.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // chkCheck
            // 
            this.chkCheck.Location = new System.Drawing.Point(3, 2);
            this.chkCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCheck.Name = "chkCheck";
            this.chkCheck.Properties.Caption = "";
            this.chkCheck.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgCheckBox1;
            this.chkCheck.Properties.CheckBoxOptions.SvgImageSize = new System.Drawing.Size(28, 28);
            this.chkCheck.Size = new System.Drawing.Size(43, 39);
            this.chkCheck.TabIndex = 5;
            // 
            // glueComponents
            // 
            this.glueComponents.Location = new System.Drawing.Point(72, 5);
            this.glueComponents.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glueComponents.Name = "glueComponents";
            this.glueComponents.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.glueComponents.Properties.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
            this.glueComponents.Properties.Appearance.Options.UseFont = true;
            this.glueComponents.Properties.Appearance.Options.UseForeColor = true;
            this.glueComponents.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glueComponents.Properties.DisplayMember = "MaintenanceDescription";
            this.glueComponents.Properties.NullText = "";
            this.glueComponents.Properties.PopupView = this.gridLookUpEdit1View;
            this.glueComponents.Properties.ValueMember = "Id";
            this.glueComponents.Size = new System.Drawing.Size(879, 34);
            this.glueComponents.TabIndex = 4;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3});
            this.gridLookUpEdit1View.DetailHeight = 431;
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Açıklama";
            this.gridColumn3.FieldName = "MaintenanceDescription";
            this.gridColumn3.MinWidth = 27;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 100;
            // 
            // ucMachineAutoMaintanenceCheckListComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkCheck);
            this.Controls.Add(this.glueComponents);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucMachineAutoMaintanenceCheckListComponent";
            this.Size = new System.Drawing.Size(971, 46);
            ((System.ComponentModel.ISupportInitialize)(this.chkCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glueComponents.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.CheckEdit chkCheck;
        private DevExpress.XtraEditors.GridLookUpEdit glueComponents;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}
