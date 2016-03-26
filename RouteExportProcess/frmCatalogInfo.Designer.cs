namespace RouteExportProcess
{
    partial class frmCatalogInfo
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
            this.cmbAccGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbExclusivityLine = new System.Windows.Forms.ComboBox();
            this.txtRouteNumber = new System.Windows.Forms.TextBox();
            this.txtMainZone = new System.Windows.Forms.TextBox();
            this.txtSubZone = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbServiceType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbClustername = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbAccGroup);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbExclusivityLine);
            this.groupBox1.Controls.Add(this.txtRouteNumber);
            this.groupBox1.Controls.Add(this.txtMainZone);
            this.groupBox1.Controls.Add(this.txtSubZone);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbServiceType);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbClustername);
            this.groupBox1.Location = new System.Drawing.Point(2, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 122);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "פרטי מקט לעדכון";
            // 
            // cmbAccGroup
            // 
            this.cmbAccGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccGroup.FormattingEnabled = true;
            this.cmbAccGroup.Location = new System.Drawing.Point(249, 49);
            this.cmbAccGroup.Name = "cmbAccGroup";
            this.cmbAccGroup.Size = new System.Drawing.Size(121, 21);
            this.cmbAccGroup.TabIndex = 3;
            this.cmbAccGroup.SelectedIndexChanged += new System.EventHandler(this.cmbAccGroup_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "ייחודיות";
            this.label1.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(404, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "קבוצת התחשבנות";
            // 
            // cmbExclusivityLine
            // 
            this.cmbExclusivityLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExclusivityLine.FormattingEnabled = true;
            this.cmbExclusivityLine.Location = new System.Drawing.Point(25, 49);
            this.cmbExclusivityLine.Name = "cmbExclusivityLine";
            this.cmbExclusivityLine.Size = new System.Drawing.Size(87, 21);
            this.cmbExclusivityLine.TabIndex = 2;
            this.cmbExclusivityLine.Visible = false;
            // 
            // txtRouteNumber
            // 
            this.txtRouteNumber.Location = new System.Drawing.Point(364, 92);
            this.txtRouteNumber.Name = "txtRouteNumber";
            this.txtRouteNumber.ReadOnly = true;
            this.txtRouteNumber.Size = new System.Drawing.Size(87, 20);
            this.txtRouteNumber.TabIndex = 3;
            this.txtRouteNumber.TabStop = false;
            // 
            // txtMainZone
            // 
            this.txtMainZone.Location = new System.Drawing.Point(190, 92);
            this.txtMainZone.Name = "txtMainZone";
            this.txtMainZone.ReadOnly = true;
            this.txtMainZone.Size = new System.Drawing.Size(87, 20);
            this.txtMainZone.TabIndex = 4;
            this.txtMainZone.TabStop = false;
            // 
            // txtSubZone
            // 
            this.txtSubZone.Location = new System.Drawing.Point(29, 92);
            this.txtSubZone.Name = "txtSubZone";
            this.txtSubZone.ReadOnly = true;
            this.txtSubZone.Size = new System.Drawing.Size(83, 20);
            this.txtSubZone.TabIndex = 5;
            this.txtSubZone.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(461, 95);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 35;
            this.label16.Text = "שילוט";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(116, 95);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "מחוז משנה";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(282, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "מחוז ראשי";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(178, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "סוג שרות";
            // 
            // cmbServiceType
            // 
            this.cmbServiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServiceType.FormattingEnabled = true;
            this.cmbServiceType.Location = new System.Drawing.Point(80, 20);
            this.cmbServiceType.Name = "cmbServiceType";
            this.cmbServiceType.Size = new System.Drawing.Size(87, 21);
            this.cmbServiceType.TabIndex = 1;
            this.cmbServiceType.SelectedIndexChanged += new System.EventHandler(this.CmbClusternameSelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(458, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "אשכול";
            // 
            // cmbClustername
            // 
            this.cmbClustername.DisplayMember = "Name";
            this.cmbClustername.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClustername.Enabled = false;
            this.cmbClustername.FormattingEnabled = true;
            this.cmbClustername.Location = new System.Drawing.Point(249, 20);
            this.cmbClustername.Name = "cmbClustername";
            this.cmbClustername.Size = new System.Drawing.Size(121, 21);
            this.cmbClustername.TabIndex = 0;
            this.cmbClustername.ValueMember = "ID";
            this.cmbClustername.SelectedIndexChanged += new System.EventHandler(this.CmbClusternameSelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Location = new System.Drawing.Point(2, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 52);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "פעולות";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(15, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "שמור";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(96, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            // 
            // errProvider
            // 
            this.errProvider.ContainerControl = this;
            this.errProvider.RightToLeft = true;
            // 
            // frmCatalogInfo
            // 
            this.ClientSize = new System.Drawing.Size(521, 192);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCatalogInfo";
            this.Load += new System.EventHandler(this.frmCatalogInfo_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCatalogInfo_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbServiceType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbClustername;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.TextBox txtSubZone;
        private System.Windows.Forms.TextBox txtMainZone;
        private System.Windows.Forms.TextBox txtRouteNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbExclusivityLine;
        private System.Windows.Forms.ComboBox cmbAccGroup;
        private System.Windows.Forms.Label label4;
    }
}
