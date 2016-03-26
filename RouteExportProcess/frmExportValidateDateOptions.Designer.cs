namespace RouteExportProcess
{
    partial class frmExportValidateDateOptions
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
            this.lstReport = new System.Windows.Forms.ListBox();
            this.gbExport = new System.Windows.Forms.GroupBox();
            this.btnContinueExport = new System.Windows.Forms.Button();
            this.btnCancelExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gbExport.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstReport);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 219);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = " GUIBase.BaseForm";
            this.groupBox1.Text = "דוח";
            // 
            // lstReport
            // 
            this.lstReport.FormattingEnabled = true;
            this.lstReport.Location = new System.Drawing.Point(15, 19);
            this.lstReport.Name = "lstReport";
            this.lstReport.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstReport.Size = new System.Drawing.Size(559, 186);
            this.lstReport.TabIndex = 0;
            // 
            // gbExport
            // 
            this.gbExport.Controls.Add(this.btnContinueExport);
            this.gbExport.Controls.Add(this.btnCancelExport);
            this.gbExport.Location = new System.Drawing.Point(330, 237);
            this.gbExport.Name = "gbExport";
            this.gbExport.Size = new System.Drawing.Size(265, 41);
            this.gbExport.TabIndex = 1;
            this.gbExport.TabStop = false;
            this.gbExport.Tag = " GUIBase.BaseForm";
            // 
            // btnContinueExport
            // 
            this.btnContinueExport.Location = new System.Drawing.Point(6, 12);
            this.btnContinueExport.Name = "btnContinueExport";
            this.btnContinueExport.Size = new System.Drawing.Size(124, 23);
            this.btnContinueExport.TabIndex = 4;
            this.btnContinueExport.Text = "המשך בתהליך ייצוא";
            this.btnContinueExport.UseVisualStyleBackColor = true;
            this.btnContinueExport.Click += new System.EventHandler(this.btnContinueExport_Click);
            // 
            // btnCancelExport
            // 
            this.btnCancelExport.Location = new System.Drawing.Point(141, 12);
            this.btnCancelExport.Name = "btnCancelExport";
            this.btnCancelExport.Size = new System.Drawing.Size(118, 23);
            this.btnCancelExport.TabIndex = 3;
            this.btnCancelExport.Text = "בטל תהליך ייצוא";
            this.btnCancelExport.UseVisualStyleBackColor = true;
            this.btnCancelExport.Click += new System.EventHandler(this.btnCancelExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(73, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(61, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Location = new System.Drawing.Point(12, 237);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(142, 41);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(6, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(61, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "הצג דוח";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmExportValidateDateOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 290);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbExport);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExportValidateDateOptions";
            this.Text = "דוח תאריכי ייצוא לרישוי";
            this.groupBox1.ResumeLayout(false);
            this.gbExport.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstReport;
        private System.Windows.Forms.GroupBox gbExport;
        private System.Windows.Forms.Button btnContinueExport;
        private System.Windows.Forms.Button btnCancelExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPrint;
    }
}