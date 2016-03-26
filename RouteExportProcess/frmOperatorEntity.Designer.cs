namespace RouteExportProcess
{
    partial class frmOperatorEntity
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
            this.btnChooseWS = new System.Windows.Forms.Button();
            this.btnChooseRS = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkCopverted = new System.Windows.Forms.CheckBox();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtWSpath = new System.Windows.Forms.TextBox();
            this.txtRSPath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnChooseWS);
            this.groupBox1.Controls.Add(this.btnChooseRS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkCopverted);
            this.groupBox1.Controls.Add(this.txtWSpath);
            this.groupBox1.Controls.Add(this.txtRSPath);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "נתוני מפעיל";
            // 
            // btnChooseWS
            // 
            this.btnChooseWS.Location = new System.Drawing.Point(6, 71);
            this.btnChooseWS.Name = "btnChooseWS";
            this.btnChooseWS.Size = new System.Drawing.Size(24, 23);
            this.btnChooseWS.TabIndex = 6;
            this.btnChooseWS.Text = "...";
            this.btnChooseWS.UseVisualStyleBackColor = true;
            this.btnChooseWS.Click += new System.EventHandler(this.btnChooseWS_Click);
            // 
            // btnChooseRS
            // 
            this.btnChooseRS.Location = new System.Drawing.Point(6, 33);
            this.btnChooseRS.Name = "btnChooseRS";
            this.btnChooseRS.Size = new System.Drawing.Size(24, 23);
            this.btnChooseRS.TabIndex = 5;
            this.btnChooseRS.Text = "...";
            this.btnChooseRS.UseVisualStyleBackColor = true;
            this.btnChooseRS.Click += new System.EventHandler(this.btnChooseRS_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(371, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "נתיב לקובץ  WorkSpace";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(364, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "נתיב לקובץ  Route system";
            // 
            // chkCopverted
            // 
            this.chkCopverted.AutoSize = true;
            this.chkCopverted.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.bndSource, "TransCadStatus", true));
            this.chkCopverted.Location = new System.Drawing.Point(214, 109);
            this.chkCopverted.Name = "chkCopverted";
            this.chkCopverted.Size = new System.Drawing.Size(146, 17);
            this.chkCopverted.TabIndex = 2;
            this.chkCopverted.Text = "האם עבר המרה בהצלחה";
            this.chkCopverted.UseVisualStyleBackColor = true;
            // 
            // bndSource
            // 
            this.bndSource.DataSource = typeof(BLEntities.Accessories.Operator);
            // 
            // txtWSpath
            // 
            this.txtWSpath.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtWSpath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWSpath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "PathToRSTWorkSpace", true));
            this.txtWSpath.Location = new System.Drawing.Point(36, 73);
            this.txtWSpath.Name = "txtWSpath";
            this.txtWSpath.ReadOnly = true;
            this.txtWSpath.Size = new System.Drawing.Size(324, 20);
            this.txtWSpath.TabIndex = 1;
            // 
            // txtRSPath
            // 
            this.txtRSPath.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtRSPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRSPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "PathToRSTFile", true));
            this.txtRSPath.Location = new System.Drawing.Point(36, 35);
            this.txtRSPath.Name = "txtRSPath";
            this.txtRSPath.ReadOnly = true;
            this.txtRSPath.Size = new System.Drawing.Size(324, 20);
            this.txtRSPath.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Location = new System.Drawing.Point(13, 162);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(507, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(343, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(424, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "שמור";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // frmOperatorEntity
            // 
            this.ClientSize = new System.Drawing.Size(532, 229);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmOperatorEntity";
            this.Text = "עדכון של פרטי מפעיל";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCopverted;
        private System.Windows.Forms.TextBox txtWSpath;
        private System.Windows.Forms.TextBox txtRSPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChooseWS;
        private System.Windows.Forms.Button btnChooseRS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bndSource;
    }
}
