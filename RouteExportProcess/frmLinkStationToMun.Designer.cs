namespace RouteExportProcess
{
    partial class frmLinkStationToMun
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnBelong = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchCity = new System.Windows.Forms.TextBox();
            this.txtSearchStation = new System.Windows.Forms.TextBox();
            this.lstCity = new System.Windows.Forms.ListBox();
            this.lstStations = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnBelong);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSearchCity);
            this.groupBox1.Controls.Add(this.txtSearchStation);
            this.groupBox1.Controls.Add(this.lstCity);
            this.groupBox1.Controls.Add(this.lstStations);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(576, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::RouteExportProcess.Properties.Resources.right;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClear.Location = new System.Drawing.Point(217, 157);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(58, 36);
            this.btnClear.TabIndex = 6;
            this.toolTipBaseForm.SetToolTip(this.btnClear, "מחק שיוך");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnBelong
            // 
            this.btnBelong.BackgroundImage = global::RouteExportProcess.Properties.Resources.left;
            this.btnBelong.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBelong.Location = new System.Drawing.Point(217, 115);
            this.btnBelong.Name = "btnBelong";
            this.btnBelong.Size = new System.Drawing.Size(58, 36);
            this.btnBelong.TabIndex = 5;
            this.toolTipBaseForm.SetToolTip(this.btnBelong, "בצע שיוך");
            this.btnBelong.UseVisualStyleBackColor = true;
            this.btnBelong.Click += new System.EventHandler(this.btnBelong_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "חיפוש מהיר שם עיר";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(447, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "חיפוש מהיר מקט תחנה";
            // 
            // txtSearchCity
            // 
            this.txtSearchCity.Location = new System.Drawing.Point(18, 19);
            this.txtSearchCity.Name = "txtSearchCity";
            this.txtSearchCity.Size = new System.Drawing.Size(82, 20);
            this.txtSearchCity.TabIndex = 3;
            this.txtSearchCity.TextChanged += new System.EventHandler(this.txtSearchCity_TextChanged);
            // 
            // txtSearchStation
            // 
            this.txtSearchStation.Location = new System.Drawing.Point(338, 19);
            this.txtSearchStation.Name = "txtSearchStation";
            this.txtSearchStation.Size = new System.Drawing.Size(103, 20);
            this.txtSearchStation.TabIndex = 0;
            this.txtSearchStation.TextChanged += new System.EventHandler(this.txtSearchStation_TextChanged);
            // 
            // lstCity
            // 
            this.lstCity.FormattingEnabled = true;
            this.lstCity.Location = new System.Drawing.Point(18, 43);
            this.lstCity.Name = "lstCity";
            this.lstCity.Size = new System.Drawing.Size(193, 238);
            this.lstCity.TabIndex = 4;
            // 
            // lstStations
            // 
            this.lstStations.FormattingEnabled = true;
            this.lstStations.Location = new System.Drawing.Point(281, 43);
            this.lstStations.Name = "lstStations";
            this.lstStations.Size = new System.Drawing.Size(279, 238);
            this.lstStations.TabIndex = 1;
            this.lstStations.Click += new System.EventHandler(this.lstStations_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnReport);
            this.groupBox3.Location = new System.Drawing.Point(12, 308);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(576, 63);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "פעולות";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(281, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 29);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(392, 19);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(168, 29);
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "דוח תחנות ללא שיוך לעיר";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(281, 19);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(51, 20);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "רענן";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmLinkStationToMun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 383);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLinkStationToMun";
            this.Text = "שיוך תחנה לעיר";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSearchCity;
        private System.Windows.Forms.TextBox txtSearchStation;
        private System.Windows.Forms.ListBox lstCity;
        private System.Windows.Forms.ListBox lstStations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBelong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnRefresh;
    }
}