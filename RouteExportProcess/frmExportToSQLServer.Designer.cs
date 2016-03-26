namespace RouteExportProcess
{
    partial class frmExportToSQLServer
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
            this.chBoxPrevInfra = new System.Windows.Forms.CheckBox();
            this.bndSourceExportToSQL = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPeriodName = new System.Windows.Forms.ComboBox();
            this.cmbYears = new System.Windows.Forms.ComboBox();
            this.rdEvents = new System.Windows.Forms.RadioButton();
            this.rdNotPeriod = new System.Windows.Forms.RadioButton();
            this.rdPeriodBase = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveInfraAsLocal = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceExportToSQL)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chBoxPrevInfra);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtTo);
            this.groupBox1.Controls.Add(this.dtFrom);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbPeriodName);
            this.groupBox1.Controls.Add(this.cmbYears);
            this.groupBox1.Controls.Add(this.rdEvents);
            this.groupBox1.Controls.Add(this.rdNotPeriod);
            this.groupBox1.Controls.Add(this.rdPeriodBase);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 183);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "קליטה מהטרנסכד למפעיל";
            // 
            // chBoxPrevInfra
            // 
            this.chBoxPrevInfra.AutoSize = true;
            this.chBoxPrevInfra.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSourceExportToSQL, "TakeLastExportedInfrastructure", true));
            this.chBoxPrevInfra.Location = new System.Drawing.Point(348, 157);
            this.chBoxPrevInfra.Name = "chBoxPrevInfra";
            this.chBoxPrevInfra.Size = new System.Drawing.Size(195, 20);
            this.chBoxPrevInfra.TabIndex = 11;
            this.chBoxPrevInfra.Text = "לקלוט תשתית שמורה לוקלית ";
            this.chBoxPrevInfra.UseVisualStyleBackColor = true;
            // 
            // bndSourceExportToSQL
            // 
            this.bndSourceExportToSQL.DataSource = typeof(BLEntities.Accessories.ExportToSQL);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "עד תאריך";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(496, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "מתאריך";
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(77, 119);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(121, 22);
            this.dtTo.TabIndex = 6;
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(373, 122);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(121, 22);
            this.dtFrom.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "שם תקופה";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(517, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "שנה";
            // 
            // cmbPeriodName
            // 
            this.cmbPeriodName.FormattingEnabled = true;
            this.cmbPeriodName.Location = new System.Drawing.Point(77, 73);
            this.cmbPeriodName.Name = "cmbPeriodName";
            this.cmbPeriodName.Size = new System.Drawing.Size(121, 24);
            this.cmbPeriodName.TabIndex = 4;
            // 
            // cmbYears
            // 
            this.cmbYears.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bndSourceExportToSQL, "Year", true));
            this.cmbYears.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceExportToSQL, "Year", true));
            this.cmbYears.DataSource = this.bndSourceExportToSQL;
            this.cmbYears.FormattingEnabled = true;
            this.cmbYears.Location = new System.Drawing.Point(373, 73);
            this.cmbYears.Name = "cmbYears";
            this.cmbYears.Size = new System.Drawing.Size(121, 24);
            this.cmbYears.TabIndex = 3;
            // 
            // rdEvents
            // 
            this.rdEvents.AutoSize = true;
            this.rdEvents.Location = new System.Drawing.Point(259, 33);
            this.rdEvents.Name = "rdEvents";
            this.rdEvents.Size = new System.Drawing.Size(59, 20);
            this.rdEvents.TabIndex = 2;
            this.rdEvents.Text = "אירוע";
            this.rdEvents.UseVisualStyleBackColor = true;
            // 
            // rdNotPeriod
            // 
            this.rdNotPeriod.AutoSize = true;
            this.rdNotPeriod.Location = new System.Drawing.Point(336, 33);
            this.rdNotPeriod.Name = "rdNotPeriod";
            this.rdNotPeriod.Size = new System.Drawing.Size(89, 20);
            this.rdNotPeriod.TabIndex = 1;
            this.rdNotPeriod.Text = "ללא תקופה";
            this.rdNotPeriod.UseVisualStyleBackColor = true;
            // 
            // rdPeriodBase
            // 
            this.rdPeriodBase.AutoSize = true;
            this.rdPeriodBase.Checked = true;
            this.rdPeriodBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.rdPeriodBase.Location = new System.Drawing.Point(440, 33);
            this.rdPeriodBase.Name = "rdPeriodBase";
            this.rdPeriodBase.Size = new System.Drawing.Size(95, 20);
            this.rdPeriodBase.TabIndex = 0;
            this.rdPeriodBase.TabStop = true;
            this.rdPeriodBase.Text = "תקופת בסיס";
            this.rdPeriodBase.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveInfraAsLocal);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnLoad);
            this.groupBox2.Location = new System.Drawing.Point(12, 193);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 41);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnSaveInfraAsLocal
            // 
            this.btnSaveInfraAsLocal.Location = new System.Drawing.Point(336, 12);
            this.btnSaveInfraAsLocal.Name = "btnSaveInfraAsLocal";
            this.btnSaveInfraAsLocal.Size = new System.Drawing.Size(129, 23);
            this.btnSaveInfraAsLocal.TabIndex = 9;
            this.btnSaveInfraAsLocal.Text = "שמור תשתית לוקלי";
            this.btnSaveInfraAsLocal.UseVisualStyleBackColor = true;
            this.btnSaveInfraAsLocal.Click += new System.EventHandler(this.btnSaveInfraAsLocal_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(255, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(471, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "קלוט";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // frmExportToSQLServer
            // 
            this.ClientSize = new System.Drawing.Size(578, 245);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmExportToSQLServer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceExportToSQL)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPeriodName;
        private System.Windows.Forms.ComboBox cmbYears;
        private System.Windows.Forms.RadioButton rdEvents;
        private System.Windows.Forms.RadioButton rdNotPeriod;
        private System.Windows.Forms.RadioButton rdPeriodBase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.BindingSource bndSourceExportToSQL;
        private System.Windows.Forms.CheckBox chBoxPrevInfra;
        private System.Windows.Forms.Button btnSaveInfraAsLocal;

    }
}
