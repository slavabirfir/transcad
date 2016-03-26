namespace RouteExportProcess
{
    partial class frmRecreateTableStructure
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
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lstRouteStopField = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstRouteLinesFields = new System.Windows.Forms.ListBox();
            this.btnShowRouteLineFile = new System.Windows.Forms.Button();
            this.lstRouteLines = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lstRouteStopField);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lstRouteLinesFields);
            this.groupBox1.Controls.Add(this.btnShowRouteLineFile);
            this.groupBox1.Controls.Add(this.lstRouteLines);
            this.groupBox1.Location = new System.Drawing.Point(2, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 393);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "קוים";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "שדות של טבלה קו תחנה";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(404, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "דוח תיקונים";
            // 
            // lstRouteStopField
            // 
            this.lstRouteStopField.FormattingEnabled = true;
            this.lstRouteStopField.Location = new System.Drawing.Point(11, 32);
            this.lstRouteStopField.Name = "lstRouteStopField";
            this.lstRouteStopField.Size = new System.Drawing.Size(190, 69);
            this.lstRouteStopField.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(353, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "שדות של טבלת קוים";
            // 
            // lstRouteLinesFields
            // 
            this.lstRouteLinesFields.FormattingEnabled = true;
            this.lstRouteLinesFields.Location = new System.Drawing.Point(281, 32);
            this.lstRouteLinesFields.Name = "lstRouteLinesFields";
            this.lstRouteLinesFields.Size = new System.Drawing.Size(190, 69);
            this.lstRouteLinesFields.TabIndex = 0;
            // 
            // btnShowRouteLineFile
            // 
            this.btnShowRouteLineFile.Location = new System.Drawing.Point(399, 364);
            this.btnShowRouteLineFile.Name = "btnShowRouteLineFile";
            this.btnShowRouteLineFile.Size = new System.Drawing.Size(75, 23);
            this.btnShowRouteLineFile.TabIndex = 3;
            this.btnShowRouteLineFile.Text = "הצג דוח";
            this.btnShowRouteLineFile.UseVisualStyleBackColor = true;
            this.btnShowRouteLineFile.Click += new System.EventHandler(this.btnShowRouteLineFile_Click);
            // 
            // lstRouteLines
            // 
            this.lstRouteLines.FormattingEnabled = true;
            this.lstRouteLines.Location = new System.Drawing.Point(11, 124);
            this.lstRouteLines.Name = "lstRouteLines";
            this.lstRouteLines.Size = new System.Drawing.Size(463, 238);
            this.lstRouteLines.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1, 411);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(95, 411);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "הרץ שינוי מבנה";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmRecreateTableStructure
            // 
            this.ClientSize = new System.Drawing.Size(485, 435);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRecreateTableStructure";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRecreateTableStructure_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnShowRouteLineFile;
        private System.Windows.Forms.ListBox lstRouteLines;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox lstRouteLinesFields;
        private System.Windows.Forms.ListBox lstRouteStopField;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
