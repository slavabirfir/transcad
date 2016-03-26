namespace RouteExportProcess
{
    partial class frmExportClusters
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
            this.lstData = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdContinue = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstData
            // 
            this.lstData.CheckOnClick = true;
            this.lstData.FormattingEnabled = true;
            this.lstData.Location = new System.Drawing.Point(12, 22);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(268, 169);
            this.lstData.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(1, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 200);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "רשימה של כל האשכולות";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdCancel);
            this.groupBox2.Controls.Add(this.cmdContinue);
            this.groupBox2.Location = new System.Drawing.Point(1, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 53);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(110, 19);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "בטל";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdContinue
            // 
            this.cmdContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdContinue.Location = new System.Drawing.Point(204, 19);
            this.cmdContinue.Name = "cmdContinue";
            this.cmdContinue.Size = new System.Drawing.Size(75, 23);
            this.cmdContinue.TabIndex = 0;
            this.cmdContinue.Text = "המשך";
            this.cmdContinue.UseVisualStyleBackColor = true;
            this.cmdContinue.Click += new System.EventHandler(this.cmdContinue_Click);
            // 
            // frmExportClusters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 268);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmExportClusters";
            this.Text = "בחר אשכולות לקליטה";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lstData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdContinue;
    }
}