namespace RouteExportProcess
{
    partial class FrmLoginsManagment
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
            this.dtData = new System.Windows.Forms.DataGridView();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.grButtons = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatorNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clusterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workspaceFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.grButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtData
            // 
            this.dtData.AllowUserToAddRows = false;
            this.dtData.AllowUserToResizeColumns = false;
            this.dtData.AllowUserToResizeRows = false;
            this.dtData.AutoGenerateColumns = false;
            this.dtData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userNameDataGridViewTextBoxColumn,
            this.operatorNameDataGridViewTextBoxColumn,
            this.clusterNameDataGridViewTextBoxColumn,
            this.loginDateDataGridViewTextBoxColumn,
            this.workspaceFileDataGridViewTextBoxColumn});
            this.dtData.DataSource = this.bndSource;
            this.dtData.Location = new System.Drawing.Point(7, 9);
            this.dtData.MultiSelect = false;
            this.dtData.Name = "dtData";
            this.dtData.ReadOnly = true;
            this.dtData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtData.Size = new System.Drawing.Size(669, 276);
            this.dtData.TabIndex = 0;
            this.dtData.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.DtDataUserDeletingRow);
            // 
            // bndSource
            // 
            this.bndSource.DataSource = typeof(BLEntities.Entities.TranscadLogin);
            // 
            // grButtons
            // 
            this.grButtons.Controls.Add(this.btnRefresh);
            this.grButtons.Controls.Add(this.btnClose);
            this.grButtons.Location = new System.Drawing.Point(6, 281);
            this.grButtons.Name = "grButtons";
            this.grButtons.Size = new System.Drawing.Size(670, 52);
            this.grButtons.TabIndex = 1;
            this.grButtons.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(508, 19);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "הבא שנית";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefreshClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(589, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.FillWeight = 80F;
            this.userNameDataGridViewTextBoxColumn.HeaderText = "משתמש";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.userNameDataGridViewTextBoxColumn.Width = 80;
            // 
            // operatorNameDataGridViewTextBoxColumn
            // 
            this.operatorNameDataGridViewTextBoxColumn.DataPropertyName = "OperatorName";
            this.operatorNameDataGridViewTextBoxColumn.FillWeight = 80F;
            this.operatorNameDataGridViewTextBoxColumn.HeaderText = "מפעיל";
            this.operatorNameDataGridViewTextBoxColumn.Name = "operatorNameDataGridViewTextBoxColumn";
            this.operatorNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.operatorNameDataGridViewTextBoxColumn.Width = 80;
            // 
            // clusterNameDataGridViewTextBoxColumn
            // 
            this.clusterNameDataGridViewTextBoxColumn.DataPropertyName = "ClusterName";
            this.clusterNameDataGridViewTextBoxColumn.FillWeight = 80F;
            this.clusterNameDataGridViewTextBoxColumn.HeaderText = "אשכול";
            this.clusterNameDataGridViewTextBoxColumn.Name = "clusterNameDataGridViewTextBoxColumn";
            this.clusterNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.clusterNameDataGridViewTextBoxColumn.Width = 80;
            // 
            // loginDateDataGridViewTextBoxColumn
            // 
            this.loginDateDataGridViewTextBoxColumn.DataPropertyName = "LoginDate";
            this.loginDateDataGridViewTextBoxColumn.FillWeight = 80F;
            this.loginDateDataGridViewTextBoxColumn.HeaderText = "הופעל ב";
            this.loginDateDataGridViewTextBoxColumn.Name = "loginDateDataGridViewTextBoxColumn";
            this.loginDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.loginDateDataGridViewTextBoxColumn.Width = 80;
            // 
            // workspaceFileDataGridViewTextBoxColumn
            // 
            this.workspaceFileDataGridViewTextBoxColumn.DataPropertyName = "WorkspaceFile";
            this.workspaceFileDataGridViewTextBoxColumn.FillWeight = 290F;
            this.workspaceFileDataGridViewTextBoxColumn.HeaderText = "מיקום הקובץ";
            this.workspaceFileDataGridViewTextBoxColumn.Name = "workspaceFileDataGridViewTextBoxColumn";
            this.workspaceFileDataGridViewTextBoxColumn.ReadOnly = true;
            this.workspaceFileDataGridViewTextBoxColumn.Width = 290;
            // 
            // FrmLoginsManagment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 334);
            this.Controls.Add(this.grButtons);
            this.Controls.Add(this.dtData);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLoginsManagment";
            this.Text = "ניהול משתמשים של המערכת";
            ((System.ComponentModel.ISupportInitialize)(this.dtData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.grButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtData;
        private System.Windows.Forms.GroupBox grButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatorNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clusterNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn workspaceFileDataGridViewTextBoxColumn;
    }
}