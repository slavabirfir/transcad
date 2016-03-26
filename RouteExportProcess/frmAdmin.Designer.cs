namespace RouteExportProcess
{
    partial class frmAdmin
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
            this.rdSetBasedLine = new System.Windows.Forms.RadioButton();
            this.rdConfig = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdExportEggedStations = new System.Windows.Forms.RadioButton();
            this.rdJunctionVersion = new System.Windows.Forms.RadioButton();
            this.rdUpdatePhysicalStops = new System.Windows.Forms.RadioButton();
            this.rdUpdateEndPoints = new System.Windows.Forms.RadioButton();
            this.rdHorada = new System.Windows.Forms.RadioButton();
            this.rdCatalogTo7Pos = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.rdExportAdmin = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdSetBasedLine
            // 
            this.rdSetBasedLine.AutoSize = true;
            this.rdSetBasedLine.Location = new System.Drawing.Point(115, 39);
            this.rdSetBasedLine.Name = "rdSetBasedLine";
            this.rdSetBasedLine.Size = new System.Drawing.Size(136, 17);
            this.rdSetBasedLine.TabIndex = 1;
            this.rdSetBasedLine.Text = "קביעת מזהה קו בסיסי";
            this.rdSetBasedLine.UseVisualStyleBackColor = true;
            // 
            // rdConfig
            // 
            this.rdConfig.AutoSize = true;
            this.rdConfig.Location = new System.Drawing.Point(108, 62);
            this.rdConfig.Name = "rdConfig";
            this.rdConfig.Size = new System.Drawing.Size(143, 17);
            this.rdConfig.TabIndex = 2;
            this.rdConfig.Text = "הצג קובץ קונפיגורציה";
            this.rdConfig.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdExportAdmin);
            this.groupBox1.Controls.Add(this.rdExportEggedStations);
            this.groupBox1.Controls.Add(this.rdJunctionVersion);
            this.groupBox1.Controls.Add(this.rdUpdatePhysicalStops);
            this.groupBox1.Controls.Add(this.rdUpdateEndPoints);
            this.groupBox1.Controls.Add(this.rdHorada);
            this.groupBox1.Controls.Add(this.rdCatalogTo7Pos);
            this.groupBox1.Controls.Add(this.rdConfig);
            this.groupBox1.Controls.Add(this.rdSetBasedLine);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 236);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "אפשרויות";
            // 
            // rdExportEggedStations
            // 
            this.rdExportEggedStations.AutoSize = true;
            this.rdExportEggedStations.Location = new System.Drawing.Point(119, 154);
            this.rdExportEggedStations.Name = "rdExportEggedStations";
            this.rdExportEggedStations.Size = new System.Drawing.Size(132, 17);
            this.rdExportEggedStations.TabIndex = 7;
            this.rdExportEggedStations.TabStop = true;
            this.rdExportEggedStations.Text = "ייצוא של תחנות אגד";
            this.rdExportEggedStations.UseVisualStyleBackColor = true;
            // 
            // rdJunctionVersion
            // 
            this.rdJunctionVersion.AutoSize = true;
            this.rdJunctionVersion.Location = new System.Drawing.Point(25, 131);
            this.rdJunctionVersion.Name = "rdJunctionVersion";
            this.rdJunctionVersion.Size = new System.Drawing.Size(226, 17);
            this.rdJunctionVersion.TabIndex = 6;
            this.rdJunctionVersion.TabStop = true;
            this.rdJunctionVersion.Text = "בניית קובץ לקביעת גרסת המוקדים לקו";
            this.rdJunctionVersion.UseVisualStyleBackColor = true;
            // 
            // rdUpdatePhysicalStops
            // 
            this.rdUpdatePhysicalStops.AutoSize = true;
            this.rdUpdatePhysicalStops.Location = new System.Drawing.Point(63, 108);
            this.rdUpdatePhysicalStops.Name = "rdUpdatePhysicalStops";
            this.rdUpdatePhysicalStops.Size = new System.Drawing.Size(188, 17);
            this.rdUpdatePhysicalStops.TabIndex = 4;
            this.rdUpdatePhysicalStops.Text = "עדכן של שכבת התחנות הפיזיות";
            this.rdUpdatePhysicalStops.UseVisualStyleBackColor = true;
            // 
            // rdUpdateEndPoints
            // 
            this.rdUpdateEndPoints.AutoSize = true;
            this.rdUpdateEndPoints.Location = new System.Drawing.Point(101, 85);
            this.rdUpdateEndPoints.Name = "rdUpdateEndPoints";
            this.rdUpdateEndPoints.Size = new System.Drawing.Size(150, 17);
            this.rdUpdateEndPoints.TabIndex = 3;
            this.rdUpdateEndPoints.Text = "עדכן של שכבת הרחובות";
            this.rdUpdateEndPoints.UseVisualStyleBackColor = true;
            // 
            // rdHorada
            // 
            this.rdHorada.AutoSize = true;
            this.rdHorada.Checked = true;
            this.rdHorada.Location = new System.Drawing.Point(96, 16);
            this.rdHorada.Name = "rdHorada";
            this.rdHorada.Size = new System.Drawing.Size(155, 17);
            this.rdHorada.TabIndex = 0;
            this.rdHorada.TabStop = true;
            this.rdHorada.Text = "חשב תחנת הורדה ראשונה";
            this.rdHorada.UseVisualStyleBackColor = true;
            // 
            // rdCatalogTo7Pos
            // 
            this.rdCatalogTo7Pos.AutoSize = true;
            this.rdCatalogTo7Pos.Location = new System.Drawing.Point(73, 213);
            this.rdCatalogTo7Pos.Name = "rdCatalogTo7Pos";
            this.rdCatalogTo7Pos.Size = new System.Drawing.Size(178, 17);
            this.rdCatalogTo7Pos.TabIndex = 5;
            this.rdCatalogTo7Pos.Text = "עדכון מספר מקט ל 7 פוזיציות";
            this.rdCatalogTo7Pos.UseVisualStyleBackColor = true;
            this.rdCatalogTo7Pos.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnRun);
            this.groupBox2.Location = new System.Drawing.Point(4, 245);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 57);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(106, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(187, 19);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "הרץ";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // rdExportAdmin
            // 
            this.rdExportAdmin.AutoSize = true;
            this.rdExportAdmin.Location = new System.Drawing.Point(102, 177);
            this.rdExportAdmin.Name = "rdExportAdmin";
            this.rdExportAdmin.Size = new System.Drawing.Size(149, 17);
            this.rdExportAdmin.TabIndex = 8;
            this.rdExportAdmin.TabStop = true;
            this.rdExportAdmin.Text = "ייצוא של מפה להשוואה";
            this.rdExportAdmin.UseVisualStyleBackColor = true;
            // 
            // frmAdmin
            // 
            this.ClientSize = new System.Drawing.Size(292, 314);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmAdmin";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdSetBasedLine;
        private System.Windows.Forms.RadioButton rdConfig;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.RadioButton rdCatalogTo7Pos;
        private System.Windows.Forms.RadioButton rdHorada;
        private System.Windows.Forms.RadioButton rdUpdatePhysicalStops;
        private System.Windows.Forms.RadioButton rdUpdateEndPoints;
        private System.Windows.Forms.RadioButton rdJunctionVersion;
        private System.Windows.Forms.RadioButton rdExportEggedStations;
        private System.Windows.Forms.RadioButton rdExportAdmin;

    }
}
