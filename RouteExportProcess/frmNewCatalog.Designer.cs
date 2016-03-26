namespace RouteExportProcess
{
    partial class frmNewCatalog
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbAccGroup = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCatalog = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstEshkol = new System.Windows.Forms.ComboBox();
            this.txtRouteNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(141, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(63, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "שמור";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Location = new System.Drawing.Point(72, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(54, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "הקלד מס. קו";
            // 
            // errProvider
            // 
            this.errProvider.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbAccGroup);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtCatalog);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lstEshkol);
            this.groupBox1.Controls.Add(this.txtRouteNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 146);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // cmbAccGroup
            // 
            this.cmbAccGroup.FormattingEnabled = true;
            this.cmbAccGroup.Location = new System.Drawing.Point(11, 67);
            this.cmbAccGroup.Name = "cmbAccGroup";
            this.cmbAccGroup.Size = new System.Drawing.Size(121, 21);
            this.cmbAccGroup.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(86, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "בחר קבוצת התחשבנות";
            // 
            // txtCatalog
            // 
            this.txtCatalog.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtCatalog.Location = new System.Drawing.Point(72, 119);
            this.txtCatalog.MaxLength = 5;
            this.txtCatalog.Name = "txtCatalog";
            this.txtCatalog.Size = new System.Drawing.Size(63, 20);
            this.txtCatalog.TabIndex = 3;
            this.txtCatalog.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCatalog_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(148, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "הקלד מקט";
            // 
            // lstEshkol
            // 
            this.lstEshkol.FormattingEnabled = true;
            this.lstEshkol.Location = new System.Drawing.Point(14, 19);
            this.lstEshkol.Name = "lstEshkol";
            this.lstEshkol.Size = new System.Drawing.Size(121, 21);
            this.lstEshkol.TabIndex = 0;
            this.lstEshkol.SelectedIndexChanged += new System.EventHandler(this.lstEshkol_SelectedIndexChanged);
            // 
            // txtRouteNumber
            // 
            this.txtRouteNumber.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtRouteNumber.Location = new System.Drawing.Point(103, 96);
            this.txtRouteNumber.MaxLength = 3;
            this.txtRouteNumber.Name = "txtRouteNumber";
            this.txtRouteNumber.Size = new System.Drawing.Size(32, 20);
            this.txtRouteNumber.TabIndex = 2;
            this.txtRouteNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRouteNumber_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "בחר אשכול";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Location = new System.Drawing.Point(7, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 56);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // frmNewCatalog
            // 
            this.ClientSize = new System.Drawing.Size(230, 221);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewCatalog";
            this.Text = "יצירת מקט חדש";
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRouteNumber;
        private System.Windows.Forms.ComboBox lstEshkol;
        private System.Windows.Forms.TextBox txtCatalog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbAccGroup;
        private System.Windows.Forms.Label label4;
    }
}
