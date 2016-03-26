namespace RouteExportProcess
{
    partial class frmClusterValidExportDateManage
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
            this.lstOperClusters = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstOpers = new System.Windows.Forms.ListBox();
            this.clusterValidateLineExportDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            this.btnSaveRow = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.msValidateDateTime = new System.Windows.Forms.MaskedTextBox();
            this.cmdClusters = new System.Windows.Forms.ComboBox();
            this.baseClassBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clusterValidateLineExportDateBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseClassBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstOperClusters);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lstOpers);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(615, 139);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lstOperClusters
            // 
            this.lstOperClusters.FormattingEnabled = true;
            this.lstOperClusters.Location = new System.Drawing.Point(6, 19);
            this.lstOperClusters.Name = "lstOperClusters";
            this.lstOperClusters.Size = new System.Drawing.Size(300, 108);
            this.lstOperClusters.TabIndex = 1;
            this.lstOperClusters.SelectedIndexChanged += new System.EventHandler(this.lstOperClusters_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "אשכולות של מפעיל";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(568, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "מפעיל";
            // 
            // lstOpers
            // 
            this.lstOpers.FormattingEnabled = true;
            this.lstOpers.Location = new System.Drawing.Point(420, 19);
            this.lstOpers.Name = "lstOpers";
            this.lstOpers.Size = new System.Drawing.Size(142, 108);
            this.lstOpers.TabIndex = 0;
            this.lstOpers.SelectedIndexChanged += new System.EventHandler(this.lstOpers_SelectedIndexChanged);
            // 
            // clusterValidateLineExportDateBindingSource
            // 
            this.clusterValidateLineExportDateBindingSource.DataSource = typeof(BLEntities.Entities.ClusterValidateLineExportDate);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnDeleteRow);
            this.groupBox2.Controls.Add(this.btnSaveRow);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.msValidateDateTime);
            this.groupBox2.Controls.Add(this.cmdClusters);
            this.groupBox2.Location = new System.Drawing.Point(12, 157);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(615, 50);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(6, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(46, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.Location = new System.Drawing.Point(58, 14);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(82, 23);
            this.btnDeleteRow.TabIndex = 5;
            this.btnDeleteRow.Text = "מחק רשומה";
            this.btnDeleteRow.UseVisualStyleBackColor = true;
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // btnSaveRow
            // 
            this.btnSaveRow.Location = new System.Drawing.Point(146, 14);
            this.btnSaveRow.Name = "btnSaveRow";
            this.btnSaveRow.Size = new System.Drawing.Size(84, 23);
            this.btnSaveRow.TabIndex = 4;
            this.btnSaveRow.Text = "עדכן רשומה";
            this.btnSaveRow.UseVisualStyleBackColor = true;
            this.btnSaveRow.Click += new System.EventHandler(this.btnSaveRow_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(309, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "ת. תוקף של אשכול";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(566, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "אשכול";
            // 
            // msValidateDateTime
            // 
            this.msValidateDateTime.Location = new System.Drawing.Point(236, 17);
            this.msValidateDateTime.Mask = "00/00/0000";
            this.msValidateDateTime.Name = "msValidateDateTime";
            this.msValidateDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.msValidateDateTime.Size = new System.Drawing.Size(70, 20);
            this.msValidateDateTime.TabIndex = 3;
            // 
            // cmdClusters
            // 
            this.cmdClusters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdClusters.FormattingEnabled = true;
            this.cmdClusters.Location = new System.Drawing.Point(420, 16);
            this.cmdClusters.Name = "cmdClusters";
            this.cmdClusters.Size = new System.Drawing.Size(142, 21);
            this.cmdClusters.TabIndex = 2;
            // 
            // baseClassBindingSource
            // 
            this.baseClassBindingSource.DataSource = typeof(BLEntities.Entities.BaseClass);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 200;
            this.errorProvider.ContainerControl = this;
            // 
            // frmClusterValidExportDateManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 214);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClusterValidExportDateManage";
            this.Text = "ניהול תאריך ייצוא של אשכול";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clusterValidateLineExportDateBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseClassBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstOpers;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox msValidateDateTime;
        private System.Windows.Forms.ComboBox cmdClusters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource clusterValidateLineExportDateBindingSource;
        private System.Windows.Forms.BindingSource baseClassBindingSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnSaveRow;
        private System.Windows.Forms.Button btnDeleteRow;
        private System.Windows.Forms.ListBox lstOperClusters;
    }
}