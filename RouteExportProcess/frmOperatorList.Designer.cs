namespace RouteExportProcess
{
    partial class FrmOperatorList
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
            this.components = new System.ComponentModel.Container();
            this.lstOperatorList = new System.Windows.Forms.ListBox();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExportErrors = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lstOperatorList
            // 
            this.lstOperatorList.DataSource = this.bndSource;
            this.lstOperatorList.DisplayMember = "OperatorWorkSpace";
            this.lstOperatorList.FormattingEnabled = true;
            this.lstOperatorList.Location = new System.Drawing.Point(-1, 0);
            this.lstOperatorList.Name = "lstOperatorList";
            this.lstOperatorList.Size = new System.Drawing.Size(254, 251);
            this.lstOperatorList.TabIndex = 0;
            // 
            // bndSource
            // 
            this.bndSource.DataSource = typeof(BLEntities.Accessories.Operator);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(-1, 260);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(74, 23);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "בחר מפעיל";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelectClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(209, 260);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(44, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "בטל";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnExportErrors
            // 
            this.btnExportErrors.Location = new System.Drawing.Point(79, 260);
            this.btnExportErrors.Name = "btnExportErrors";
            this.btnExportErrors.Size = new System.Drawing.Size(50, 23);
            this.btnExportErrors.TabIndex = 3;
            this.btnExportErrors.Text = "שגוים";
            this.btnExportErrors.UseVisualStyleBackColor = true;
            this.btnExportErrors.Click += new System.EventHandler(this.BtnExportErrorsClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(191, 260);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(15, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "עדכן ";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdateClick);
            // 
            // frmOperatorList
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(254, 285);
            this.ControlBox = false;
            this.Controls.Add(this.btnExportErrors);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lstOperatorList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOperatorList";
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstOperatorList;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExportErrors;
        private System.Windows.Forms.Button btnUpdate;
    }
}
