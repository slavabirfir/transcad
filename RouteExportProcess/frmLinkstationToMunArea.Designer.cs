namespace RouteExportProcess
{
    partial class FrmLinkstationToMunArea
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
            this.cmdClose = new System.Windows.Forms.Button();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.txtMuniSel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLinkStationToArea = new System.Windows.Forms.Button();
            this.btnShowCloseAreas = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStationCatalogSearch = new System.Windows.Forms.TextBox();
            this.lstAreas = new System.Windows.Forms.ListBox();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.lstStations = new System.Windows.Forms.ListBox();
            this.chAllNonLinked = new System.Windows.Forms.CheckBox();
            this.btnShowStationOnMap = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnShowStationOnMap);
            this.groupBox1.Controls.Add(this.cmdClose);
            this.groupBox1.Controls.Add(this.txtSN);
            this.groupBox1.Controls.Add(this.txtMuniSel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnLinkStationToArea);
            this.groupBox1.Controls.Add(this.btnShowCloseAreas);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtStationCatalogSearch);
            this.groupBox1.Controls.Add(this.lstAreas);
            this.groupBox1.Controls.Add(this.lstStations);
            this.groupBox1.Controls.Add(this.chAllNonLinked);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 378);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(178, 338);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(116, 23);
            this.cmdClose.TabIndex = 7;
            this.cmdClose.Text = "סגור";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.CmdCloseClick);
            // 
            // txtSN
            // 
            this.txtSN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSN.Location = new System.Drawing.Point(178, 61);
            this.txtSN.Name = "txtSN";
            this.txtSN.ReadOnly = true;
            this.txtSN.Size = new System.Drawing.Size(102, 13);
            this.txtSN.TabIndex = 10;
            this.txtSN.TabStop = false;
            // 
            // txtMuniSel
            // 
            this.txtMuniSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMuniSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMuniSel.Location = new System.Drawing.Point(23, 84);
            this.txtMuniSel.Name = "txtMuniSel";
            this.txtMuniSel.ReadOnly = true;
            this.txtMuniSel.Size = new System.Drawing.Size(149, 24);
            this.txtMuniSel.TabIndex = 9;
            this.txtMuniSel.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "אזור שיוך של תחנה במערכת";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(213, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "איזורים מוניציפאליים הסמוכים לעדכון";
            // 
            // btnLinkStationToArea
            // 
            this.btnLinkStationToArea.Location = new System.Drawing.Point(335, 338);
            this.btnLinkStationToArea.Name = "btnLinkStationToArea";
            this.btnLinkStationToArea.Size = new System.Drawing.Size(120, 23);
            this.btnLinkStationToArea.TabIndex = 6;
            this.btnLinkStationToArea.Text = "שייך תחנה לאזור";
            this.btnLinkStationToArea.UseVisualStyleBackColor = true;
            this.btnLinkStationToArea.Click += new System.EventHandler(this.BtnLinkStationToAreaClick);
            // 
            // btnShowCloseAreas
            // 
            this.btnShowCloseAreas.Location = new System.Drawing.Point(335, 185);
            this.btnShowCloseAreas.Name = "btnShowCloseAreas";
            this.btnShowCloseAreas.Size = new System.Drawing.Size(116, 23);
            this.btnShowCloseAreas.TabIndex = 3;
            this.btnShowCloseAreas.Text = "הצג אזורים סמוכים";
            this.btnShowCloseAreas.UseVisualStyleBackColor = true;
            this.btnShowCloseAreas.Click += new System.EventHandler(this.BtnShowCloseAreasClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(341, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "חיפוש לפי מקט תחנה";
            // 
            // txtStationCatalogSearch
            // 
            this.txtStationCatalogSearch.Location = new System.Drawing.Point(284, 58);
            this.txtStationCatalogSearch.Name = "txtStationCatalogSearch";
            this.txtStationCatalogSearch.Size = new System.Drawing.Size(57, 20);
            this.txtStationCatalogSearch.TabIndex = 1;
            this.txtStationCatalogSearch.TextChanged += new System.EventHandler(this.TxtStationCatalogSearchTextChanged);
            // 
            // lstAreas
            // 
            this.lstAreas.DataSource = this.bndSource;
            this.lstAreas.DisplayMember = "Name";
            this.lstAreas.FormattingEnabled = true;
            this.lstAreas.Location = new System.Drawing.Point(178, 246);
            this.lstAreas.Name = "lstAreas";
            this.lstAreas.Size = new System.Drawing.Size(274, 82);
            this.lstAreas.TabIndex = 5;
            this.lstAreas.ValueMember = "MunId";
            // 
            // bndSource
            // 
            this.bndSource.DataSource = typeof(BLEntities.Entities.City);
            // 
            // lstStations
            // 
            this.lstStations.FormattingEnabled = true;
            this.lstStations.Location = new System.Drawing.Point(178, 84);
            this.lstStations.Name = "lstStations";
            this.lstStations.Size = new System.Drawing.Size(274, 95);
            this.lstStations.TabIndex = 2;
            this.lstStations.SelectedIndexChanged += new System.EventHandler(this.LstStationsSelectedIndexChanged);
            // 
            // chAllNonLinked
            // 
            this.chAllNonLinked.AutoSize = true;
            this.chAllNonLinked.Checked = true;
            this.chAllNonLinked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chAllNonLinked.Location = new System.Drawing.Point(250, 20);
            this.chAllNonLinked.Name = "chAllNonLinked";
            this.chAllNonLinked.Size = new System.Drawing.Size(202, 17);
            this.chAllNonLinked.TabIndex = 0;
            this.chAllNonLinked.Text = "הצג כול התחנות ללא שיוך לאיזור";
            this.chAllNonLinked.UseVisualStyleBackColor = true;
            this.chAllNonLinked.CheckedChanged += new System.EventHandler(this.ChAllNonLinkedCheckedChanged);
            // 
            // btnShowStationOnMap
            // 
            this.btnShowStationOnMap.Location = new System.Drawing.Point(178, 185);
            this.btnShowStationOnMap.Name = "btnShowStationOnMap";
            this.btnShowStationOnMap.Size = new System.Drawing.Size(116, 23);
            this.btnShowStationOnMap.TabIndex = 4;
            this.btnShowStationOnMap.Text = "הצג תחנה על מפה";
            this.btnShowStationOnMap.UseVisualStyleBackColor = true;
            this.btnShowStationOnMap.Click += new System.EventHandler(this.BtnShowStationOnMapClick);
            // 
            // FrmLinkstationToMunArea
            // 
            this.ClientSize = new System.Drawing.Size(489, 394);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLinkstationToMunArea";
            this.Text = "שיוך תחנה לשטח מוניציפאלי";
            this.Load += new System.EventHandler(this.FrmLinkstationToMunArea_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstStations;
        private System.Windows.Forms.CheckBox chAllNonLinked;
        private System.Windows.Forms.ListBox lstAreas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStationCatalogSearch;
        private System.Windows.Forms.Button btnLinkStationToArea;
        private System.Windows.Forms.Button btnShowCloseAreas;
        private System.Windows.Forms.BindingSource bndSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMuniSel;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button btnShowStationOnMap;
    }
}
