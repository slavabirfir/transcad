namespace RouteExportProcess
{
    partial class frmListBoxes
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.lstAll = new System.Windows.Forms.ListBox();
            this.bindingSourceAll = new System.Windows.Forms.BindingSource(this.components);
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.bindingSourceSelected = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnRight);
            this.groupBox1.Controls.Add(this.btnLeft);
            this.groupBox1.Controls.Add(this.lstAll);
            this.groupBox1.Controls.Add(this.lstSelected);
            this.groupBox1.Location = new System.Drawing.Point(1, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 146);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "ערכים שנבחרו";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "ערכים אפשריים";
            // 
            // btnRight
            // 
            this.btnRight.Image = global::RouteExportProcess.Properties.Resources.right;
            this.btnRight.Location = new System.Drawing.Point(107, 91);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(38, 33);
            this.btnRight.TabIndex = 3;
            this.btnRight.Text = "->";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Image = global::RouteExportProcess.Properties.Resources.left;
            this.btnLeft.Location = new System.Drawing.Point(107, 52);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(38, 33);
            this.btnLeft.TabIndex = 2;
            this.btnLeft.Text = "->";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // lstAll
            // 
            this.lstAll.DataSource = this.bindingSourceAll;
            this.lstAll.FormattingEnabled = true;
            this.lstAll.Location = new System.Drawing.Point(155, 32);
            this.lstAll.Name = "lstAll";
            this.lstAll.Size = new System.Drawing.Size(87, 108);
            this.lstAll.TabIndex = 0;
            // 
            // bindingSourceAll
            // 
            this.bindingSourceAll.DataSource = typeof(BLEntities.Entities.BaseTableEntity);
            // 
            // lstSelected
            // 
            this.lstSelected.DataSource = this.bindingSourceSelected;
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.Location = new System.Drawing.Point(13, 32);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.Size = new System.Drawing.Size(83, 108);
            this.lstSelected.TabIndex = 1;
            // 
            // bindingSourceSelected
            // 
            this.bindingSourceSelected.DataSource = typeof(BLEntities.Entities.BaseTableEntity);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(1, 152);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "אשר";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmListBoxes
            // 
            this.ClientSize = new System.Drawing.Size(254, 178);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmListBoxes";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSelected)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.ListBox lstAll;
        private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource bindingSourceAll;
        private System.Windows.Forms.BindingSource bindingSourceSelected;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
