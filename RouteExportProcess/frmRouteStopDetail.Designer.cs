namespace RouteExportProcess
{
    partial class frmRouteStopDetail
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
            this.txtRouteName = new System.Windows.Forms.TextBox();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ckBoxNotInUse = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPlatform = new System.Windows.Forms.TextBox();
            this.cmbStationHorada = new System.Windows.Forms.ComboBox();
            this.txtDistanceFromPrevStationFrom = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrdinalNumber = new System.Windows.Forms.TextBox();
            this.txtStationCatalog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStationType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDefinitios = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lstStatus = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCalcAutoHorada = new System.Windows.Forms.Button();
            this.btnVerifyValidation = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRouteName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 37);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "פרטי קו";
            // 
            // txtRouteName
            // 
            this.txtRouteName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "RouteLine", true));
            this.txtRouteName.Location = new System.Drawing.Point(8, 11);
            this.txtRouteName.Name = "txtRouteName";
            this.txtRouteName.ReadOnly = true;
            this.txtRouteName.Size = new System.Drawing.Size(312, 20);
            this.txtRouteName.TabIndex = 0;
            // 
            // bndSource
            // 
            this.bndSource.DataSource = typeof(BLEntities.Entities.RouteStop);
            this.bndSource.CurrentChanged += new System.EventHandler(this.BndSourceCurrentChanged);
            this.bndSource.CurrentItemChanged += new System.EventHandler(this.BndSourceCurrentItemChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(326, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "אשכול/מקט/שילוט/כיוון/חלופה";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.cmbStationHorada);
            this.groupBox2.Controls.Add(this.txtDistanceFromPrevStationFrom);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtOrdinalNumber);
            this.groupBox2.Controls.Add(this.txtStationCatalog);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbStationType);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtDefinitios);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Location = new System.Drawing.Point(3, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(509, 116);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "פרטי תחנה";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ckBoxNotInUse);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.txtPlatform);
            this.groupBox5.Location = new System.Drawing.Point(13, 69);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(323, 39);
            this.groupBox5.TabIndex = 44;
            this.groupBox5.TabStop = false;
            // 
            // ckBoxNotInUse
            // 
            this.ckBoxNotInUse.AutoSize = true;
            this.ckBoxNotInUse.Location = new System.Drawing.Point(235, 15);
            this.ckBoxNotInUse.Name = "ckBoxNotInUse";
            this.ckBoxNotInUse.Size = new System.Drawing.Size(86, 17);
            this.ckBoxNotInUse.TabIndex = 8;
            this.ckBoxNotInUse.Text = "לא בשימוש";
            this.ckBoxNotInUse.UseVisualStyleBackColor = true;
            this.ckBoxNotInUse.CheckedChanged += new System.EventHandler(this.CkBoxNotInUseCheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(175, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "רציף";
            // 
            // txtPlatform
            // 
            this.txtPlatform.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "Platform", true));
            this.txtPlatform.Location = new System.Drawing.Point(116, 13);
            this.txtPlatform.MaxLength = 3;
            this.txtPlatform.Name = "txtPlatform";
            this.txtPlatform.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtPlatform.Size = new System.Drawing.Size(53, 20);
            this.txtPlatform.TabIndex = 10;
            this.txtPlatform.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbStationHorada
            // 
            this.cmbStationHorada.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bndSource, "Horada", true));
            this.cmbStationHorada.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationHorada.FormattingEnabled = true;
            this.cmbStationHorada.Location = new System.Drawing.Point(278, 48);
            this.cmbStationHorada.MaxDropDownItems = 20;
            this.cmbStationHorada.Name = "cmbStationHorada";
            this.cmbStationHorada.Size = new System.Drawing.Size(127, 21);
            this.cmbStationHorada.TabIndex = 4;
            this.cmbStationHorada.SelectedIndexChanged += new System.EventHandler(this.CmbStationHoradaSelectedIndexChanged);
            // 
            // txtDistanceFromPrevStationFrom
            // 
            this.txtDistanceFromPrevStationFrom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "MilepostRounded", true));
            this.txtDistanceFromPrevStationFrom.Location = new System.Drawing.Point(342, 75);
            this.txtDistanceFromPrevStationFrom.Name = "txtDistanceFromPrevStationFrom";
            this.txtDistanceFromPrevStationFrom.ReadOnly = true;
            this.txtDistanceFromPrevStationFrom.Size = new System.Drawing.Size(62, 20);
            this.txtDistanceFromPrevStationFrom.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(403, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "מרחק מי ת. קודמת";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(222, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "מ. סידורי";
            // 
            // txtOrdinalNumber
            // 
            this.txtOrdinalNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "Ordinal", true));
            this.txtOrdinalNumber.Location = new System.Drawing.Point(129, 19);
            this.txtOrdinalNumber.Name = "txtOrdinalNumber";
            this.txtOrdinalNumber.ReadOnly = true;
            this.txtOrdinalNumber.Size = new System.Drawing.Size(94, 20);
            this.txtOrdinalNumber.TabIndex = 2;
            // 
            // txtStationCatalog
            // 
            this.txtStationCatalog.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "StationCatalog", true));
            this.txtStationCatalog.Location = new System.Drawing.Point(13, 19);
            this.txtStationCatalog.Name = "txtStationCatalog";
            this.txtStationCatalog.ReadOnly = true;
            this.txtStationCatalog.Size = new System.Drawing.Size(54, 20);
            this.txtStationCatalog.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "מקט תחנה";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "פ. בתחנה";
            // 
            // cmbStationType
            // 
            this.cmbStationType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bndSource, "IdStationType", true));
            this.cmbStationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationType.FormattingEnabled = true;
            this.cmbStationType.Location = new System.Drawing.Point(129, 48);
            this.cmbStationType.Name = "cmbStationType";
            this.cmbStationType.Size = new System.Drawing.Size(94, 21);
            this.cmbStationType.TabIndex = 5;
            this.cmbStationType.SelectedIndexChanged += new System.EventHandler(this.CmbStationTypeSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(405, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "ת. הורדה ראשונה";
            // 
            // txtDefinitios
            // 
            this.txtDefinitios.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSource, "PhysicalStop", true));
            this.txtDefinitios.Location = new System.Drawing.Point(278, 19);
            this.txtDefinitios.Name = "txtDefinitios";
            this.txtDefinitios.ReadOnly = true;
            this.txtDefinitios.Size = new System.Drawing.Size(164, 20);
            this.txtDefinitios.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(448, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "שם תחנה";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lstStatus);
            this.groupBox6.Location = new System.Drawing.Point(3, 177);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(509, 73);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "סטטוס פעולה";
            // 
            // lstStatus
            // 
            this.lstStatus.FormattingEnabled = true;
            this.lstStatus.Location = new System.Drawing.Point(11, 19);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(486, 43);
            this.lstStatus.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCalcAutoHorada);
            this.groupBox3.Controls.Add(this.btnVerifyValidation);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Location = new System.Drawing.Point(3, 256);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(321, 44);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "פעילות";
            // 
            // btnCalcAutoHorada
            // 
            this.btnCalcAutoHorada.Location = new System.Drawing.Point(52, 15);
            this.btnCalcAutoHorada.Name = "btnCalcAutoHorada";
            this.btnCalcAutoHorada.Size = new System.Drawing.Size(129, 23);
            this.btnCalcAutoHorada.TabIndex = 14;
            this.btnCalcAutoHorada.Text = "חשב ת. הורדה לכל קו ";
            this.toolTipBaseForm.SetToolTip(this.btnCalcAutoHorada, "וודא תקינות");
            this.btnCalcAutoHorada.UseVisualStyleBackColor = true;
            this.btnCalcAutoHorada.Click += new System.EventHandler(this.BtnCalcAutoHoradaClick);
            // 
            // btnVerifyValidation
            // 
            this.btnVerifyValidation.Location = new System.Drawing.Point(187, 15);
            this.btnVerifyValidation.Name = "btnVerifyValidation";
            this.btnVerifyValidation.Size = new System.Drawing.Size(83, 23);
            this.btnVerifyValidation.TabIndex = 13;
            this.btnVerifyValidation.Text = "וודא תקינות";
            this.toolTipBaseForm.SetToolTip(this.btnVerifyValidation, "וודא תקינות");
            this.btnVerifyValidation.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(272, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(43, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "סגור";
            this.toolTipBaseForm.SetToolTip(this.btnClose, "סגור");
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(42, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "שמור";
            this.toolTipBaseForm.SetToolTip(this.btnSave, "שמור");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnLast);
            this.groupBox4.Controls.Add(this.btnNext);
            this.groupBox4.Controls.Add(this.btnPrev);
            this.groupBox4.Controls.Add(this.btnFirst);
            this.groupBox4.Location = new System.Drawing.Point(325, 256);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(187, 44);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ניווט";
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(136, 15);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(46, 23);
            this.btnLast.TabIndex = 16;
            this.btnLast.Text = "<<";
            this.toolTipBaseForm.SetToolTip(this.btnLast, "לקו תחנה אחרון");
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.BtnLastClick);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(93, 15);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 23);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = "<";
            this.toolTipBaseForm.SetToolTip(this.btnNext, "לקו תחנה הבא");
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(53, 15);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(37, 23);
            this.btnPrev.TabIndex = 18;
            this.btnPrev.Text = ">";
            this.toolTipBaseForm.SetToolTip(this.btnPrev, "לקו תחנה קודם");
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.BtnPrevClick);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(7, 15);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(43, 23);
            this.btnFirst.TabIndex = 19;
            this.btnFirst.Text = ">>";
            this.toolTipBaseForm.SetToolTip(this.btnFirst, "לקו תחנה ראשון");
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.BtnFirstClick);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 200;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.RightToLeft = true;
            // 
            // frmRouteStopDetail
            // 
            this.ClientSize = new System.Drawing.Size(511, 307);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmRouteStopDetail";
            this.Load += new System.EventHandler(this.RouteStopDetail_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RouteStopDetail_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListBox lstStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnVerifyValidation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.TextBox txtRouteName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDefinitios;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStationType;
        private System.Windows.Forms.TextBox txtStationCatalog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrdinalNumber;
        private System.Windows.Forms.TextBox txtDistanceFromPrevStationFrom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStationHorada;
        private System.Windows.Forms.Button btnCalcAutoHorada;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPlatform;
        private System.Windows.Forms.CheckBox ckBoxNotInUse;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}
