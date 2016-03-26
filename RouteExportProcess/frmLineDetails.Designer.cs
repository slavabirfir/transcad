namespace RouteExportProcess
{
    partial class frmLineDetails
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
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.groupBoxContainer = new System.Windows.Forms.GroupBox();
            this.msValidateDateTime = new System.Windows.Forms.MaskedTextBox();
            this.bndSourceRouteLine = new System.Windows.Forms.BindingSource(this.components);
            this.lblValidDateExport = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbServiceType = new System.Windows.Forms.ComboBox();
            this.cmbAccountingGroup = new System.Windows.Forms.ComboBox();
            this.ckBoxAccessibility = new System.Windows.Forms.CheckBox();
            this.ckBIsBase = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRouteDesc = new System.Windows.Forms.TextBox();
            this.cmbEshkol = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRouteNumber = new System.Windows.Forms.TextBox();
            this.txtDefinitios = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMakat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVariant = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSignPost = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnVerifyValidation = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lstStatus = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceRouteLine)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLast);
            this.groupBox1.Controls.Add(this.btnNext);
            this.groupBox1.Controls.Add(this.btnPrev);
            this.groupBox1.Controls.Add(this.btnFirst);
            this.groupBox1.Location = new System.Drawing.Point(353, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ניווט";
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(148, 15);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(46, 23);
            this.btnLast.TabIndex = 14;
            this.btnLast.Text = "<<";
            this.toolTipBaseForm.SetToolTip(this.btnLast, "לקו אחרון");
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.BtnLastClick);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(102, 15);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 23);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = "<";
            this.toolTipBaseForm.SetToolTip(this.btnNext, "לקו הבא");
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(59, 15);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(37, 23);
            this.btnPrev.TabIndex = 16;
            this.btnPrev.Text = ">";
            this.toolTipBaseForm.SetToolTip(this.btnPrev, "לקו קודם");
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.BtnPrevClick);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(10, 15);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(43, 23);
            this.btnFirst.TabIndex = 15;
            this.btnFirst.Text = ">>";
            this.toolTipBaseForm.SetToolTip(this.btnFirst, "לקו ראשון");
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.BtnFirstClick);
            // 
            // groupBoxContainer
            // 
            this.groupBoxContainer.Controls.Add(this.msValidateDateTime);
            this.groupBoxContainer.Controls.Add(this.lblValidDateExport);
            this.groupBoxContainer.Controls.Add(this.label10);
            this.groupBoxContainer.Controls.Add(this.cmbServiceType);
            this.groupBoxContainer.Controls.Add(this.cmbAccountingGroup);
            this.groupBoxContainer.Controls.Add(this.ckBoxAccessibility);
            this.groupBoxContainer.Controls.Add(this.ckBIsBase);
            this.groupBoxContainer.Controls.Add(this.label20);
            this.groupBoxContainer.Controls.Add(this.cmbDirection);
            this.groupBoxContainer.Controls.Add(this.label2);
            this.groupBoxContainer.Controls.Add(this.txtRouteDesc);
            this.groupBoxContainer.Controls.Add(this.cmbEshkol);
            this.groupBoxContainer.Controls.Add(this.label15);
            this.groupBoxContainer.Controls.Add(this.txtRouteNumber);
            this.groupBoxContainer.Controls.Add(this.txtDefinitios);
            this.groupBoxContainer.Controls.Add(this.label14);
            this.groupBoxContainer.Controls.Add(this.txtMakat);
            this.groupBoxContainer.Controls.Add(this.label3);
            this.groupBoxContainer.Controls.Add(this.txtVariant);
            this.groupBoxContainer.Controls.Add(this.label5);
            this.groupBoxContainer.Controls.Add(this.label8);
            this.groupBoxContainer.Controls.Add(this.txtSignPost);
            this.groupBoxContainer.Controls.Add(this.label16);
            this.groupBoxContainer.Controls.Add(this.label4);
            this.groupBoxContainer.Location = new System.Drawing.Point(1, 2);
            this.groupBoxContainer.Name = "groupBoxContainer";
            this.groupBoxContainer.Size = new System.Drawing.Size(555, 171);
            this.groupBoxContainer.TabIndex = 1;
            this.groupBoxContainer.TabStop = false;
            this.groupBoxContainer.Text = "פרטי מזהה קו";
            // 
            // msValidateDateTime
            // 
            this.msValidateDateTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "ValidExportDate", true));
            this.msValidateDateTime.Location = new System.Drawing.Point(338, 139);
            this.msValidateDateTime.Mask = "00/00/0000";
            this.msValidateDateTime.Name = "msValidateDateTime";
            this.msValidateDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.msValidateDateTime.Size = new System.Drawing.Size(70, 20);
            this.msValidateDateTime.TabIndex = 40;
            // 
            // bndSourceRouteLine
            // 
            this.bndSourceRouteLine.DataSource = typeof(BLEntities.Entities.RouteLine);
            this.bndSourceRouteLine.CurrentChanged += new System.EventHandler(this.BndSourceRouteLineCurrentChanged);
            this.bndSourceRouteLine.CurrentItemChanged += new System.EventHandler(this.BndSourceRouteLineCurrentItemChanged);
            // 
            // lblValidDateExport
            // 
            this.lblValidDateExport.AutoSize = true;
            this.lblValidDateExport.Location = new System.Drawing.Point(432, 142);
            this.lblValidDateExport.Name = "lblValidDateExport";
            this.lblValidDateExport.Size = new System.Drawing.Size(116, 13);
            this.lblValidDateExport.TabIndex = 39;
            this.lblValidDateExport.Text = "תוקף לייצוא מתאריך";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(209, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "סוג שרות";
            // 
            // cmbServiceType
            // 
            this.cmbServiceType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bndSourceRouteLine, "IdServiceType", true));
            this.cmbServiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServiceType.FormattingEnabled = true;
            this.cmbServiceType.Location = new System.Drawing.Point(130, 68);
            this.cmbServiceType.Name = "cmbServiceType";
            this.cmbServiceType.Size = new System.Drawing.Size(73, 21);
            this.cmbServiceType.TabIndex = 6;
            this.cmbServiceType.SelectedIndexChanged += new System.EventHandler(this.CmbServiceTypeSelectedIndexChanged);
            // 
            // cmbAccountingGroup
            // 
            this.cmbAccountingGroup.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bndSourceRouteLine, "AccountingGroupID", true));
            this.cmbAccountingGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountingGroup.FormattingEnabled = true;
            this.cmbAccountingGroup.Location = new System.Drawing.Point(6, 26);
            this.cmbAccountingGroup.Name = "cmbAccountingGroup";
            this.cmbAccountingGroup.Size = new System.Drawing.Size(125, 21);
            this.cmbAccountingGroup.TabIndex = 2;
            this.cmbAccountingGroup.SelectedIndexChanged += new System.EventHandler(this.CmbAccountingGroupSelectedIndexChanged);
            // 
            // ckBoxAccessibility
            // 
            this.ckBoxAccessibility.AutoSize = true;
            this.ckBoxAccessibility.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSourceRouteLine, "Accessibility", true));
            this.ckBoxAccessibility.Location = new System.Drawing.Point(6, 111);
            this.ckBoxAccessibility.Name = "ckBoxAccessibility";
            this.ckBoxAccessibility.Size = new System.Drawing.Size(63, 17);
            this.ckBoxAccessibility.TabIndex = 10;
            this.ckBoxAccessibility.Text = "נגישות";
            this.ckBoxAccessibility.UseVisualStyleBackColor = true;
            // 
            // ckBIsBase
            // 
            this.ckBIsBase.AutoSize = true;
            this.ckBIsBase.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSourceRouteLine, "IsBase", true));
            this.ckBIsBase.Location = new System.Drawing.Point(107, 112);
            this.ckBIsBase.Name = "ckBIsBase";
            this.ckBIsBase.Size = new System.Drawing.Size(88, 17);
            this.ckBIsBase.TabIndex = 9;
            this.ckBIsBase.Text = "חלופת בסיס";
            this.ckBIsBase.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(130, 31);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(74, 13);
            this.label20.TabIndex = 34;
            this.label20.Text = "ק. התחשבנות";
            // 
            // cmbDirection
            // 
            this.cmbDirection.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "Dir", true));
            this.cmbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Location = new System.Drawing.Point(361, 68);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(47, 21);
            this.cmbDirection.TabIndex = 4;
            this.cmbDirection.SelectedIndexChanged += new System.EventHandler(this.CmbDirectionSelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "אשכול";
            // 
            // txtRouteDesc
            // 
            this.txtRouteDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "RoadDescription", true));
            this.txtRouteDesc.Location = new System.Drawing.Point(204, 109);
            this.txtRouteDesc.MaxLength = 32;
            this.txtRouteDesc.Name = "txtRouteDesc";
            this.txtRouteDesc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtRouteDesc.Size = new System.Drawing.Size(246, 20);
            this.txtRouteDesc.TabIndex = 8;
            // 
            // cmbEshkol
            // 
            this.cmbEshkol.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bndSourceRouteLine, "IdCluster", true));
            this.cmbEshkol.DisplayMember = "Catalog";
            this.cmbEshkol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEshkol.FormattingEnabled = true;
            this.cmbEshkol.Location = new System.Drawing.Point(204, 28);
            this.cmbEshkol.Name = "cmbEshkol";
            this.cmbEshkol.Size = new System.Drawing.Size(103, 21);
            this.cmbEshkol.TabIndex = 1;
            this.cmbEshkol.ValueMember = "Catalog";
            this.cmbEshkol.SelectedIndexChanged += new System.EventHandler(this.CmbEshkolSelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(474, 109);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "תיאור מסלול";
            // 
            // txtRouteNumber
            // 
            this.txtRouteNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "RouteNumber", true));
            this.txtRouteNumber.Location = new System.Drawing.Point(362, 29);
            this.txtRouteNumber.MaxLength = 3;
            this.txtRouteNumber.Name = "txtRouteNumber";
            this.txtRouteNumber.Size = new System.Drawing.Size(46, 20);
            this.txtRouteNumber.TabIndex = 0;
            this.txtRouteNumber.Leave += new System.EventHandler(this.TxtRouteNumberLeave);
            this.txtRouteNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtRouteNumberKeyPress);
            // 
            // txtDefinitios
            // 
            this.txtDefinitios.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "Hagdara", true));
            this.txtDefinitios.Location = new System.Drawing.Point(7, 69);
            this.txtDefinitios.Name = "txtDefinitios";
            this.txtDefinitios.Size = new System.Drawing.Size(47, 20);
            this.txtDefinitios.TabIndex = 7;
            this.txtDefinitios.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDefinitiosKeyPress);
            this.txtDefinitios.Validating += new System.ComponentModel.CancelEventHandler(this.TxtDefinitiosValidating);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(55, 72);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "זמן הגדרה";
            // 
            // txtMakat
            // 
            this.txtMakat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMakat.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "Catalog", true));
            this.txtMakat.Enabled = false;
            this.txtMakat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMakat.Location = new System.Drawing.Point(460, 31);
            this.txtMakat.Name = "txtMakat";
            this.txtMakat.ReadOnly = true;
            this.txtMakat.Size = new System.Drawing.Size(44, 14);
            this.txtMakat.TabIndex = 20;
            this.txtMakat.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(313, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "חלופה";
            // 
            // txtVariant
            // 
            this.txtVariant.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "Var", true));
            this.txtVariant.Location = new System.Drawing.Point(270, 69);
            this.txtVariant.MaxLength = 1;
            this.txtVariant.Name = "txtVariant";
            this.txtVariant.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtVariant.Size = new System.Drawing.Size(37, 20);
            this.txtVariant.TabIndex = 5;
            this.txtVariant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtVariant.Validating += new System.ComponentModel.CancelEventHandler(this.TxtVariantValidating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(419, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "כיוון";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(520, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "מקט";
            // 
            // txtSignPost
            // 
            this.txtSignPost.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "Signpost", true));
            this.txtSignPost.Location = new System.Drawing.Point(460, 69);
            this.txtSignPost.MaxLength = 5;
            this.txtSignPost.Name = "txtSignPost";
            this.txtSignPost.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSignPost.Size = new System.Drawing.Size(44, 20);
            this.txtSignPost.TabIndex = 3;
            this.txtSignPost.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSignPostValidating);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(414, 32);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "מס. קו";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(508, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "שילוט";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnVerifyValidation);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Location = new System.Drawing.Point(10, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(208, 44);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "פעילות";
            // 
            // btnVerifyValidation
            // 
            this.btnVerifyValidation.Location = new System.Drawing.Point(55, 13);
            this.btnVerifyValidation.Name = "btnVerifyValidation";
            this.btnVerifyValidation.Size = new System.Drawing.Size(86, 23);
            this.btnVerifyValidation.TabIndex = 12;
            this.btnVerifyValidation.Text = "וודא תקינות";
            this.toolTipBaseForm.SetToolTip(this.btnVerifyValidation, "וודא תקינות");
            this.btnVerifyValidation.UseVisualStyleBackColor = true;
            this.btnVerifyValidation.Click += new System.EventHandler(this.BtnVerifyValidationClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(147, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(48, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "סגור";
            this.toolTipBaseForm.SetToolTip(this.btnClose, "סגור");
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(43, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "שמור";
            this.toolTipBaseForm.SetToolTip(this.btnSave, "שמור");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 200;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.RightToLeft = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lstStatus);
            this.groupBox6.Location = new System.Drawing.Point(0, 179);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(555, 89);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "סטטוס פעולה";
            // 
            // lstStatus
            // 
            this.lstStatus.FormattingEnabled = true;
            this.lstStatus.Location = new System.Drawing.Point(11, 19);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(534, 56);
            this.lstStatus.TabIndex = 14;
            // 
            // frmLineDetails
            // 
            this.ClientSize = new System.Drawing.Size(557, 320);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxContainer);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmLineDetails";
            this.Load += new System.EventHandler(this.LineDetails_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LineDetails_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxContainer.ResumeLayout(false);
            this.groupBoxContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceRouteLine)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.GroupBox groupBoxContainer;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnVerifyValidation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtVariant;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSignPost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRouteDesc;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDefinitios;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.BindingSource bndSourceRouteLine;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListBox lstStatus;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.TextBox txtMakat;
        private System.Windows.Forms.TextBox txtRouteNumber;
        private System.Windows.Forms.CheckBox ckBIsBase;
        private System.Windows.Forms.CheckBox ckBoxAccessibility;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEshkol;
        private System.Windows.Forms.ComboBox cmbAccountingGroup;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbServiceType;
        private System.Windows.Forms.Label lblValidDateExport;
        private System.Windows.Forms.MaskedTextBox msValidateDateTime;
    }
}
