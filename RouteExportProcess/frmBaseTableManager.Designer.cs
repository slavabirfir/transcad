namespace RouteExportProcess
{
    partial class frmBaseTableManager
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
            this.dtGridData = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.lstBoxTableName = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtGridData);
            this.groupBox1.Controls.Add(this.lstBoxTableName);
            this.groupBox1.Location = new System.Drawing.Point(0, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 183);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ניהול טבלאות בסיס";
            // 
            // dtGridData
            // 
            this.dtGridData.AllowUserToAddRows = false;
            this.dtGridData.AllowUserToDeleteRows = false;
            this.dtGridData.AutoGenerateColumns = false;
            this.dtGridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.dtGridData.DataSource = this.bndSource;
            this.dtGridData.Location = new System.Drawing.Point(15, 19);
            this.dtGridData.Name = "dtGridData";
            this.dtGridData.ReadOnly = true;
            this.dtGridData.RowHeadersVisible = false;
            this.dtGridData.Size = new System.Drawing.Size(247, 158);
            this.dtGridData.TabIndex = 1;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "קוד";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Width = 50;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "שם";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 175;
            // 
            // bndSource
            // 
            this.bndSource.AllowNew = false;
            this.bndSource.DataSource = typeof(BLEntities.Entities.BaseTableEntity);
            // 
            // lstBoxTableName
            // 
            this.lstBoxTableName.FormattingEnabled = true;
            this.lstBoxTableName.Location = new System.Drawing.Point(268, 19);
            this.lstBoxTableName.Name = "lstBoxTableName";
            this.lstBoxTableName.Size = new System.Drawing.Size(153, 160);
            this.lstBoxTableName.TabIndex = 0;
            this.lstBoxTableName.SelectedIndexChanged += new System.EventHandler(this.lstBoxTableName_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Location = new System.Drawing.Point(0, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "פעילות";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(346, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmBaseTableManager
            // 
            this.ClientSize = new System.Drawing.Size(428, 245);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmBaseTableManager";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstBoxTableName;
        private System.Windows.Forms.DataGridView dtGridData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    }
}
