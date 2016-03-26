namespace RouteExportProcess
{
    sealed partial class FrmClusterStateSelect
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.lstviewcategories = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnApprove);
            this.groupBox1.Location = new System.Drawing.Point(1, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 54);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(294, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "סגור";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(375, 19);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "אשר";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApproveClick);
            // 
            // lstviewcategories
            // 
            this.lstviewcategories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstviewcategories.Location = new System.Drawing.Point(1, 3);
            this.lstviewcategories.MultiSelect = false;
            this.lstviewcategories.Name = "lstviewcategories";
            this.lstviewcategories.RightToLeftLayout = true;
            this.lstviewcategories.Size = new System.Drawing.Size(458, 136);
            this.lstviewcategories.TabIndex = 2;
            this.lstviewcategories.UseCompatibleStateImageBehavior = false;
            // 
            // FrmClusterStateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 193);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstviewcategories);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmClusterStateSelect";
            this.Text = "בחירת מחוז ראשי ומשני של אשכול ";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.ListView lstviewcategories;
        private System.Windows.Forms.Button btnCancel;
    }
}