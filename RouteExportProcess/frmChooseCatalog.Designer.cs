namespace RouteExportProcess
{
    partial class frmChooseCatalog
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
            this.grpCatalogList = new System.Windows.Forms.GroupBox();
            this.dtgData = new System.Windows.Forms.DataGridView();
            this.catalogDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromPathCityNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clusterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.rdNewCatalog = new System.Windows.Forms.RadioButton();
            this.rdExistCatalog = new System.Windows.Forms.RadioButton();
            this.btnApprove = new System.Windows.Forms.Button();
            this.lblTextBoxWarning = new System.Windows.Forms.TextBox();
            this.grpCatalogList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCatalogList
            // 
            this.grpCatalogList.Controls.Add(this.dtgData);
            this.grpCatalogList.Location = new System.Drawing.Point(24, 61);
            this.grpCatalogList.Name = "grpCatalogList";
            this.grpCatalogList.Size = new System.Drawing.Size(367, 136);
            this.grpCatalogList.TabIndex = 0;
            this.grpCatalogList.TabStop = false;
            this.grpCatalogList.Text = "רשימת מקטים קיימים";
            // 
            // dtgData
            // 
            this.dtgData.AllowUserToAddRows = false;
            this.dtgData.AllowUserToDeleteRows = false;
            this.dtgData.AllowUserToResizeColumns = false;
            this.dtgData.AllowUserToResizeRows = false;
            this.dtgData.AutoGenerateColumns = false;
            this.dtgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.catalogDataGridViewTextBoxColumn,
            this.fromPathCityNameDataGridViewTextBoxColumn,
            this.clusterNameDataGridViewTextBoxColumn});
            this.dtgData.DataSource = this.bndSource;
            this.dtgData.Location = new System.Drawing.Point(15, 19);
            this.dtgData.MultiSelect = false;
            this.dtgData.Name = "dtgData";
            this.dtgData.ReadOnly = true;
            this.dtgData.RowHeadersVisible = false;
            this.dtgData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgData.ShowCellErrors = false;
            this.dtgData.Size = new System.Drawing.Size(337, 108);
            this.dtgData.TabIndex = 2;
            this.dtgData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dtgData_DataBindingComplete);
            // 
            // catalogDataGridViewTextBoxColumn
            // 
            this.catalogDataGridViewTextBoxColumn.DataPropertyName = "Catalog";
            this.catalogDataGridViewTextBoxColumn.FillWeight = 70F;
            this.catalogDataGridViewTextBoxColumn.HeaderText = "מקט";
            this.catalogDataGridViewTextBoxColumn.Name = "catalogDataGridViewTextBoxColumn";
            this.catalogDataGridViewTextBoxColumn.ReadOnly = true;
            this.catalogDataGridViewTextBoxColumn.Width = 70;
            // 
            // fromPathCityNameDataGridViewTextBoxColumn
            // 
            this.fromPathCityNameDataGridViewTextBoxColumn.DataPropertyName = "FromPathCityName";
            this.fromPathCityNameDataGridViewTextBoxColumn.FillWeight = 120F;
            this.fromPathCityNameDataGridViewTextBoxColumn.HeaderText = "עיר מוצא";
            this.fromPathCityNameDataGridViewTextBoxColumn.Name = "fromPathCityNameDataGridViewTextBoxColumn";
            this.fromPathCityNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.fromPathCityNameDataGridViewTextBoxColumn.Width = 120;
            // 
            // clusterNameDataGridViewTextBoxColumn
            // 
            this.clusterNameDataGridViewTextBoxColumn.DataPropertyName = "ClusterName";
            this.clusterNameDataGridViewTextBoxColumn.FillWeight = 120F;
            this.clusterNameDataGridViewTextBoxColumn.HeaderText = "אשכול ";
            this.clusterNameDataGridViewTextBoxColumn.Name = "clusterNameDataGridViewTextBoxColumn";
            this.clusterNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.clusterNameDataGridViewTextBoxColumn.Width = 120;
            // 
            // bndSource
            // 
            this.bndSource.AllowNew = false;
            this.bndSource.DataSource = typeof(BLManager.CatalogPresenter);
            // 
            // rdNewCatalog
            // 
            this.rdNewCatalog.AutoSize = true;
            this.rdNewCatalog.Checked = true;
            this.rdNewCatalog.Location = new System.Drawing.Point(7, 18);
            this.rdNewCatalog.Name = "rdNewCatalog";
            this.rdNewCatalog.Size = new System.Drawing.Size(72, 17);
            this.rdNewCatalog.TabIndex = 0;
            this.rdNewCatalog.TabStop = true;
            this.rdNewCatalog.Text = "מקט חדש";
            this.rdNewCatalog.UseVisualStyleBackColor = true;
            this.rdNewCatalog.CheckedChanged += new System.EventHandler(this.rdNewCatalog_CheckedChanged);
            // 
            // rdExistCatalog
            // 
            this.rdExistCatalog.AutoSize = true;
            this.rdExistCatalog.Location = new System.Drawing.Point(7, 41);
            this.rdExistCatalog.Name = "rdExistCatalog";
            this.rdExistCatalog.Size = new System.Drawing.Size(96, 17);
            this.rdExistCatalog.TabIndex = 1;
            this.rdExistCatalog.Text = "בחר מקט קיים";
            this.rdExistCatalog.UseVisualStyleBackColor = true;
            this.rdExistCatalog.CheckedChanged += new System.EventHandler(this.rdExistCatalog_CheckedChanged);
            // 
            // btnApprove
            // 
            this.btnApprove.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApprove.Location = new System.Drawing.Point(4, 269);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 3;
            this.btnApprove.Text = "אשר";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // lblTextBoxWarning
            // 
            this.lblTextBoxWarning.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblTextBoxWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTextBoxWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTextBoxWarning.Location = new System.Drawing.Point(4, 203);
            this.lblTextBoxWarning.Multiline = true;
            this.lblTextBoxWarning.Name = "lblTextBoxWarning";
            this.lblTextBoxWarning.ReadOnly = true;
            this.lblTextBoxWarning.Size = new System.Drawing.Size(387, 60);
            this.lblTextBoxWarning.TabIndex = 3;
            this.lblTextBoxWarning.Text = "שים לב, מספר הקו שרשמת כבר קיים.  באפשרותך לשייכו כמזהה נוסף למק\"ט קיים, או ליצור" +
                " מק\"ט חדש";
            // 
            // frmChooseCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 294);
            this.Controls.Add(this.lblTextBoxWarning);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.rdExistCatalog);
            this.Controls.Add(this.rdNewCatalog);
            this.Controls.Add(this.grpCatalogList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmChooseCatalog";
            this.Text = "בחר מקט";
            this.grpCatalogList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCatalogList;
        private System.Windows.Forms.RadioButton rdNewCatalog;
        private System.Windows.Forms.RadioButton rdExistCatalog;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.TextBox lblTextBoxWarning;
        private System.Windows.Forms.DataGridView dtgData;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn catalogDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromPathCityNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clusterNameDataGridViewTextBoxColumn;
    }
}