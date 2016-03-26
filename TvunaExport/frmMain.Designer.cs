namespace TvunaExport
{
    partial class frmMain
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnApprove = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.txtPeriodID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOperator = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtPImportFinishDate = new System.Windows.Forms.DateTimePicker();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnApprove);
            this.groupBox2.Location = new System.Drawing.Point(4, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(177, 19);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(88, 23);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtToDate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtFromDate);
            this.groupBox1.Controls.Add(this.txtPeriodID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtOperator);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtPImportFinishDate);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 189);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "To Date";
            // 
            // dtToDate
            // 
            this.dtToDate.CustomFormat = "dd/MM/yyyy";
            this.dtToDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "ToDate", true));
            this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtToDate.Location = new System.Drawing.Point(177, 152);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(88, 20);
            this.dtToDate.TabIndex = 8;
            // 
            // bndSource
            // 
            this.bndSource.DataSource = typeof(TvunaExport.BL.InsertImportControlOperator);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "From Date";
            // 
            // dtFromDate
            // 
            this.dtFromDate.CustomFormat = "dd/MM/yyyy";
            this.dtFromDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "FromDate", true));
            this.dtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFromDate.Location = new System.Drawing.Point(177, 121);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(88, 20);
            this.dtFromDate.TabIndex = 6;
            // 
            // txtPeriodID
            // 
            this.txtPeriodID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "PeriodId", true));
            this.txtPeriodID.Location = new System.Drawing.Point(177, 89);
            this.txtPeriodID.Name = "txtPeriodID";
            this.txtPeriodID.Size = new System.Drawing.Size(88, 20);
            this.txtPeriodID.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "PeriodID";
            // 
            // txtOperator
            // 
            this.txtOperator.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtOperator.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "OperatorId", true));
            this.txtOperator.Location = new System.Drawing.Point(177, 19);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.ReadOnly = true;
            this.txtOperator.Size = new System.Drawing.Size(88, 20);
            this.txtOperator.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IdOperator";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ImportFinishDate";
            // 
            // dtPImportFinishDate
            // 
            this.dtPImportFinishDate.CustomFormat = "dd/MM/yyyy";
            this.dtPImportFinishDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "ImportFinishDate", true));
            this.dtPImportFinishDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPImportFinishDate.Location = new System.Drawing.Point(177, 55);
            this.dtPImportFinishDate.Name = "dtPImportFinishDate";
            this.dtPImportFinishDate.Size = new System.Drawing.Size(88, 20);
            this.dtPImportFinishDate.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(83, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 263);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Tvuna Export Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtPImportFinishDate;
        private System.Windows.Forms.TextBox txtOperator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.TextBox txtPeriodID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.Button btnClose;
    }
}

