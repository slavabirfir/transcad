namespace RouteExportProcess
{
    partial class frmZoneImportAreaChanged
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgData = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplaceZoneCode = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bnSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.lblTextBoxWarning = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bnSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgData);
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(578, 364);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "אזורי מחיר שהשתנו";
            // 
            // dgData
            // 
            this.dgData.AllowUserToAddRows = false;
            this.dgData.AllowUserToDeleteRows = false;
            this.dgData.AllowUserToOrderColumns = true;
            this.dgData.AllowUserToResizeColumns = false;
            this.dgData.AllowUserToResizeRows = false;
            this.dgData.AutoGenerateColumns = false;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.ReplaceZoneCode});
            this.dgData.DataSource = this.bnSource;
            this.dgData.Location = new System.Drawing.Point(6, 19);
            this.dgData.Name = "dgData";
            this.dgData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgData.Size = new System.Drawing.Size(566, 338);
            this.dgData.TabIndex = 0;
            this.dgData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgData_MouseDown);
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.FillWeight = 150F;
            this.iDDataGridViewTextBoxColumn.HeaderText = "קוד איזור";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Width = 150;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.FillWeight = 200F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "שם איזור";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 200;
            // 
            // ReplaceZoneCode
            // 
            this.ReplaceZoneCode.DataPropertyName = "ReplaceZoneCode";
            this.ReplaceZoneCode.FillWeight = 150F;
            this.ReplaceZoneCode.HeaderText = "האם להחליף קוד איזור?";
            this.ReplaceZoneCode.Name = "ReplaceZoneCode";
            this.ReplaceZoneCode.Width = 150;
            // 
            // bnSource
            // 
            this.bnSource.DataSource = typeof(BLManager.BLPriceArea);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnAccept);
            this.groupBox2.Location = new System.Drawing.Point(7, 420);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(578, 45);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "פעולות";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(411, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "בטל";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(492, 17);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "אשר";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // lblTextBoxWarning
            // 
            this.lblTextBoxWarning.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblTextBoxWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTextBoxWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTextBoxWarning.Location = new System.Drawing.Point(12, 372);
            this.lblTextBoxWarning.Multiline = true;
            this.lblTextBoxWarning.Name = "lblTextBoxWarning";
            this.lblTextBoxWarning.ReadOnly = true;
            this.lblTextBoxWarning.Size = new System.Drawing.Size(567, 46);
            this.lblTextBoxWarning.TabIndex = 2;
            this.lblTextBoxWarning.Text = "שים לב, להלו רשימת האזורים שהשתנו, עליך לסמן רק את האזורים שברצונך לשנות עבורם את" +
                " קוד האזור";
            // 
            // frmZoneImportAreaChanged
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 469);
            this.Controls.Add(this.lblTextBoxWarning);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmZoneImportAreaChanged";
            this.Text = "אזורי מחיר שהשתנו";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bnSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.BindingSource bnSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn replaceZoneCodecDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ReplaceZoneCode;
        private System.Windows.Forms.TextBox lblTextBoxWarning;
    }
}