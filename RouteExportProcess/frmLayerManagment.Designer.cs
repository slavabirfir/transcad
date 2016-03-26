namespace RouteExportProcess
{
    partial class frmLayerManagment
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstData = new System.Windows.Forms.CheckedListBox();
            this.grpFields = new System.Windows.Forms.GroupBox();
            this.dtGFields = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pcbLayerColor = new System.Windows.Forms.PictureBox();
            this.btnLayerColor = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.bndSourceLayers = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLayerColor)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.grpFields);
            this.groupBox1.Location = new System.Drawing.Point(1, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 382);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "פירוט שכבות";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstData);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 134);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "שכבות מידע נוספות";
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.Location = new System.Drawing.Point(13, 19);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(356, 109);
            this.lstData.TabIndex = 8;
            this.lstData.SelectedIndexChanged += new System.EventHandler(this.lstData_SelectedIndexChanged);
            this.lstData.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstData_ItemCheck);
            // 
            // grpFields
            // 
            this.grpFields.Controls.Add(this.dtGFields);
            this.grpFields.Controls.Add(this.label1);
            this.grpFields.Controls.Add(this.label2);
            this.grpFields.Controls.Add(this.pcbLayerColor);
            this.grpFields.Controls.Add(this.btnLayerColor);
            this.grpFields.Location = new System.Drawing.Point(6, 159);
            this.grpFields.Name = "grpFields";
            this.grpFields.Size = new System.Drawing.Size(380, 217);
            this.grpFields.TabIndex = 13;
            this.grpFields.TabStop = false;
            this.grpFields.Text = "הגדרות של שכבה פעילה";
            // 
            // dtGFields
            // 
            this.dtGFields.AllowUserToAddRows = false;
            this.dtGFields.AllowUserToDeleteRows = false;
            this.dtGFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGFields.ColumnHeadersVisible = false;
            this.dtGFields.Location = new System.Drawing.Point(191, 39);
            this.dtGFields.MultiSelect = false;
            this.dtGFields.Name = "dtGFields";
            this.dtGFields.RowHeadersVisible = false;
            this.dtGFields.RowHeadersWidth = 50;
            this.dtGFields.Size = new System.Drawing.Size(140, 172);
            this.dtGFields.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(337, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "שדות";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "צבע";
            // 
            // pcbLayerColor
            // 
            this.pcbLayerColor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pcbLayerColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbLayerColor.Location = new System.Drawing.Point(45, 34);
            this.pcbLayerColor.Name = "pcbLayerColor";
            this.pcbLayerColor.Size = new System.Drawing.Size(78, 23);
            this.pcbLayerColor.TabIndex = 9;
            this.pcbLayerColor.TabStop = false;
            // 
            // btnLayerColor
            // 
            this.btnLayerColor.Location = new System.Drawing.Point(13, 34);
            this.btnLayerColor.Name = "btnLayerColor";
            this.btnLayerColor.Size = new System.Drawing.Size(24, 23);
            this.btnLayerColor.TabIndex = 2;
            this.btnLayerColor.Text = "...";
            this.toolTipBaseForm.SetToolTip(this.btnLayerColor, "הצג חלון של צבעים");
            this.btnLayerColor.UseVisualStyleBackColor = true;
            this.btnLayerColor.Click += new System.EventHandler(this.btnLayerColor_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnApprove);
            this.groupBox2.Location = new System.Drawing.Point(3, 391);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(218, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(308, 15);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "אשר";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // bndSourceLayers
            // 
            this.bndSourceLayers.DataSource = typeof(BLEntities.Entities.Layer);
            // 
            // frmLayerManagment
            // 
            this.ClientSize = new System.Drawing.Size(402, 441);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmLayerManagment";
            this.Text = "ניהול שכבות מידע";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLayerManagment_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.grpFields.ResumeLayout(false);
            this.grpFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLayerColor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lstData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.BindingSource bndSourceLayers;
        private System.Windows.Forms.GroupBox grpFields;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLayerColor;
        private System.Windows.Forms.PictureBox pcbLayerColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dtGFields;
        private System.Windows.Forms.Label label1;
    }
}
