namespace RouteExportProcess
{
    partial class frmRouteExportProccess
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRouteExportProccess));
            this.grBxLines = new System.Windows.Forms.GroupBox();
            this.chartSystemLines = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartPysicalStops = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartSystemStops = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbPhysicalStops = new System.Windows.Forms.TabPage();
            this.dtgPhysicalStops = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.longitudeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.latitudeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StationStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndSourcePhysicalStops = new System.Windows.Forms.BindingSource(this.components);
            this.tbRouteStops = new System.Windows.Forms.TabPage();
            this.gbShowStation = new System.Windows.Forms.GroupBox();
            this.rdShowByCatalogStation = new System.Windows.Forms.RadioButton();
            this.rdShowByNameStation = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnStationTypeUpdate = new System.Windows.Forms.Button();
            this.cmbStationType = new System.Windows.Forms.ComboBox();
            this.picBox4 = new System.Windows.Forms.PictureBox();
            this.picBox2 = new System.Windows.Forms.PictureBox();
            this.picBox3 = new System.Windows.Forms.PictureBox();
            this.picBox0 = new System.Windows.Forms.PictureBox();
            this.picBox1 = new System.Windows.Forms.PictureBox();
            this.txtlblErrorDataRouteStop = new System.Windows.Forms.TextBox();
            this.chLabelRouteStops = new System.Windows.Forms.CheckBox();
            this.lstBoxRouteSopsErrors = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSearchStationName = new System.Windows.Forms.Button();
            this.btnSearchStationCatalog = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRouteStopSearchCatalogStation = new System.Windows.Forms.TextBox();
            this.btnRouteStopSearchClear = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSearchStationName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRouteStopSearchCatalog = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRouteStopSearchVariant = new System.Windows.Forms.TextBox();
            this.txtRouteStopSearchDirection = new System.Windows.Forms.TextBox();
            this.txtRouteStopSearchRouteNumber = new System.Windows.Forms.TextBox();
            this.cbRouteStopsErrorsOnly = new System.Windows.Forms.CheckBox();
            this.dtgRouteStops = new System.Windows.Forms.DataGridView();
            this.routeLineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Physical_Stop_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.physicalStopDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StationTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordinalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.milepostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.horadaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bndSourceRouteStops = new System.Windows.Forms.BindingSource(this.components);
            this.tbRouteLines = new System.Windows.Forms.TabPage();
            this.bndSourceRouteLine = new System.Windows.Forms.BindingSource(this.components);
            this.btnCalcFirstStationForAllRoutes = new System.Windows.Forms.Button();
            this.chkPathFlow = new System.Windows.Forms.CheckBox();
            this.chkPathName = new System.Windows.Forms.CheckBox();
            this.txtDuplicated = new System.Windows.Forms.TextBox();
            this.cmbRouteLinesField = new System.Windows.Forms.ComboBox();
            this.cbLabeling = new System.Windows.Forms.CheckBox();
            this.txtlblErrorDataRouteLine = new System.Windows.Forms.TextBox();
            this.lstBoxRouteLineErrors = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClusters = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSearchCluster = new System.Windows.Forms.TextBox();
            this.btnCatalogRouteLineSearch = new System.Windows.Forms.Button();
            this.btnRouteNumberSearch = new System.Windows.Forms.Button();
            this.chBoxRouteSearchIsNewRoutes = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSearchCatalog = new System.Windows.Forms.TextBox();
            this.btnRouteLineSearchClear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtValiant = new System.Windows.Forms.TextBox();
            this.txtDirection = new System.Windows.Forms.TextBox();
            this.txtRouteNumber = new System.Windows.Forms.TextBox();
            this.cbRouteLinesErrorsOnly = new System.Windows.Forms.CheckBox();
            this.dtgRouteSystem = new System.Windows.Forms.DataGridView();
            this.ClusterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Catalog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RouteNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signpostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.varDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dirDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsBase = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.roadDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbRouteData = new System.Windows.Forms.TabControl();
            this.grBxPhysicalStation = new System.Windows.Forms.GroupBox();
            this.grBxStops = new System.Windows.Forms.GroupBox();
            this.menuStripRouteLines = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnRouteLineDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnRouteLineZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoomCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoomAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInitMapResize = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRouteStops = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRouteStopsData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolBarMenuRouteStopDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRouteStopZoomCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRouteStopZoomGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInitResizeMap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGetRouteLineStops = new System.Windows.Forms.ToolStripMenuItem();
            this.imageCustomList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.txtOperatorName = new System.Windows.Forms.TextBox();
            this.grBxLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSystemLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPysicalStops)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSystemStops)).BeginInit();
            this.tbPhysicalStops.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgPhysicalStops)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourcePhysicalStops)).BeginInit();
            this.tbRouteStops.SuspendLayout();
            this.gbShowStation.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRouteStops)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceRouteStops)).BeginInit();
            this.tbRouteLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceRouteLine)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRouteSystem)).BeginInit();
            this.tbRouteData.SuspendLayout();
            this.grBxPhysicalStation.SuspendLayout();
            this.grBxStops.SuspendLayout();
            this.menuStripRouteLines.SuspendLayout();
            this.mnuRouteStopsData.SuspendLayout();
            this.SuspendLayout();
            // 
            // grBxLines
            // 
            this.grBxLines.Controls.Add(this.chartSystemLines);
            this.grBxLines.Location = new System.Drawing.Point(5, 3);
            this.grBxLines.Name = "grBxLines";
            this.grBxLines.Size = new System.Drawing.Size(231, 131);
            this.grBxLines.TabIndex = 0;
            this.grBxLines.TabStop = false;
            this.grBxLines.Text = "קוים";
            // 
            // chartSystemLines
            // 
            this.chartSystemLines.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.BottomRight;
            chartArea4.Name = "ChartArea1";
            this.chartSystemLines.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartSystemLines.Legends.Add(legend4);
            this.chartSystemLines.Location = new System.Drawing.Point(6, 18);
            this.chartSystemLines.Name = "chartSystemLines";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartSystemLines.Series.Add(series4);
            this.chartSystemLines.Size = new System.Drawing.Size(216, 107);
            this.chartSystemLines.TabIndex = 1;
            this.chartSystemLines.Text = "chart1";
            this.chartSystemLines.DoubleClick += new System.EventHandler(this.ChartSystemLinesDoubleClick);
            this.chartSystemLines.MouseEnter += new System.EventHandler(this.chartSystemLines_MouseEnter);
            this.chartSystemLines.MouseLeave += new System.EventHandler(this.chartSystemLines_MouseLeave);
            // 
            // chartPysicalStops
            // 
            chartArea5.Name = "ChartArea1";
            this.chartPysicalStops.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartPysicalStops.Legends.Add(legend5);
            this.chartPysicalStops.Location = new System.Drawing.Point(8, 18);
            this.chartPysicalStops.Name = "chartPysicalStops";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartPysicalStops.Series.Add(series5);
            this.chartPysicalStops.Size = new System.Drawing.Size(216, 107);
            this.chartPysicalStops.TabIndex = 3;
            this.chartPysicalStops.Text = "chart1";
            this.chartPysicalStops.DoubleClick += new System.EventHandler(this.PhysicalStops_DoubleClick);
            // 
            // chartSystemStops
            // 
            chartArea6.Name = "ChartArea1";
            this.chartSystemStops.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartSystemStops.Legends.Add(legend6);
            this.chartSystemStops.Location = new System.Drawing.Point(8, 19);
            this.chartSystemStops.Name = "chartSystemStops";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chartSystemStops.Series.Add(series6);
            this.chartSystemStops.Size = new System.Drawing.Size(216, 106);
            this.chartSystemStops.TabIndex = 2;
            this.chartSystemStops.Text = "0";
            this.chartSystemStops.DoubleClick += new System.EventHandler(this.ChartSystemStopsDoubleClick);
            this.chartSystemStops.MouseEnter += new System.EventHandler(this.chartSystemLines_MouseEnter);
            this.chartSystemStops.MouseLeave += new System.EventHandler(this.chartSystemLines_MouseLeave);
            // 
            // tbPhysicalStops
            // 
            this.tbPhysicalStops.Controls.Add(this.dtgPhysicalStops);
            this.tbPhysicalStops.Location = new System.Drawing.Point(4, 22);
            this.tbPhysicalStops.Name = "tbPhysicalStops";
            this.tbPhysicalStops.Padding = new System.Windows.Forms.Padding(3);
            this.tbPhysicalStops.Size = new System.Drawing.Size(708, 322);
            this.tbPhysicalStops.TabIndex = 2;
            this.tbPhysicalStops.UseVisualStyleBackColor = true;
            // 
            // dtgPhysicalStops
            // 
            this.dtgPhysicalStops.AllowUserToAddRows = false;
            this.dtgPhysicalStops.AllowUserToDeleteRows = false;
            this.dtgPhysicalStops.AllowUserToOrderColumns = true;
            this.dtgPhysicalStops.AllowUserToResizeColumns = false;
            this.dtgPhysicalStops.AllowUserToResizeRows = false;
            this.dtgPhysicalStops.AutoGenerateColumns = false;
            this.dtgPhysicalStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgPhysicalStops.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.longitudeDataGridViewTextBoxColumn1,
            this.latitudeDataGridViewTextBoxColumn1,
            this.directionDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.StationStatus,
            this.StationType});
            this.dtgPhysicalStops.DataSource = this.bndSourcePhysicalStops;
            this.dtgPhysicalStops.Location = new System.Drawing.Point(6, 6);
            this.dtgPhysicalStops.Name = "dtgPhysicalStops";
            this.dtgPhysicalStops.ReadOnly = true;
            this.dtgPhysicalStops.RowHeadersVisible = false;
            this.dtgPhysicalStops.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgPhysicalStops.Size = new System.Drawing.Size(694, 308);
            this.dtgPhysicalStops.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "StationCatalog";
            this.Column1.HeaderText = "מקט תחנה";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // longitudeDataGridViewTextBoxColumn1
            // 
            this.longitudeDataGridViewTextBoxColumn1.DataPropertyName = "Longitude";
            this.longitudeDataGridViewTextBoxColumn1.HeaderText = "Longitude";
            this.longitudeDataGridViewTextBoxColumn1.Name = "longitudeDataGridViewTextBoxColumn1";
            this.longitudeDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // latitudeDataGridViewTextBoxColumn1
            // 
            this.latitudeDataGridViewTextBoxColumn1.DataPropertyName = "Latitude";
            this.latitudeDataGridViewTextBoxColumn1.HeaderText = "Latitude";
            this.latitudeDataGridViewTextBoxColumn1.Name = "latitudeDataGridViewTextBoxColumn1";
            this.latitudeDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // directionDataGridViewTextBoxColumn
            // 
            this.directionDataGridViewTextBoxColumn.DataPropertyName = "Direction";
            this.directionDataGridViewTextBoxColumn.HeaderText = "כיוון";
            this.directionDataGridViewTextBoxColumn.MinimumWidth = 60;
            this.directionDataGridViewTextBoxColumn.Name = "directionDataGridViewTextBoxColumn";
            this.directionDataGridViewTextBoxColumn.ReadOnly = true;
            this.directionDataGridViewTextBoxColumn.Width = 60;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.FillWeight = 110F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "שם תחנה";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 110;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 110;
            // 
            // StationStatus
            // 
            this.StationStatus.DataPropertyName = "StationStatus";
            this.StationStatus.HeaderText = "סטטוס תחנה";
            this.StationStatus.Name = "StationStatus";
            this.StationStatus.ReadOnly = true;
            // 
            // StationType
            // 
            this.StationType.DataPropertyName = "StationType";
            this.StationType.HeaderText = "סוג תחנה";
            this.StationType.Name = "StationType";
            this.StationType.ReadOnly = true;
            // 
            // bndSourcePhysicalStops
            // 
            this.bndSourcePhysicalStops.AllowNew = false;
            this.bndSourcePhysicalStops.DataSource = typeof(BLEntities.Entities.PhysicalStop);
            // 
            // tbRouteStops
            // 
            this.tbRouteStops.Controls.Add(this.gbShowStation);
            this.tbRouteStops.Controls.Add(this.groupBox3);
            this.tbRouteStops.Controls.Add(this.picBox4);
            this.tbRouteStops.Controls.Add(this.picBox2);
            this.tbRouteStops.Controls.Add(this.picBox3);
            this.tbRouteStops.Controls.Add(this.picBox0);
            this.tbRouteStops.Controls.Add(this.picBox1);
            this.tbRouteStops.Controls.Add(this.txtlblErrorDataRouteStop);
            this.tbRouteStops.Controls.Add(this.chLabelRouteStops);
            this.tbRouteStops.Controls.Add(this.lstBoxRouteSopsErrors);
            this.tbRouteStops.Controls.Add(this.groupBox4);
            this.tbRouteStops.Controls.Add(this.dtgRouteStops);
            this.tbRouteStops.Location = new System.Drawing.Point(4, 22);
            this.tbRouteStops.Name = "tbRouteStops";
            this.tbRouteStops.Padding = new System.Windows.Forms.Padding(3);
            this.tbRouteStops.Size = new System.Drawing.Size(708, 322);
            this.tbRouteStops.TabIndex = 1;
            this.tbRouteStops.UseVisualStyleBackColor = true;
            // 
            // gbShowStation
            // 
            this.gbShowStation.Controls.Add(this.rdShowByCatalogStation);
            this.gbShowStation.Controls.Add(this.rdShowByNameStation);
            this.gbShowStation.Enabled = false;
            this.gbShowStation.Location = new System.Drawing.Point(417, 240);
            this.gbShowStation.Name = "gbShowStation";
            this.gbShowStation.Size = new System.Drawing.Size(202, 40);
            this.gbShowStation.TabIndex = 32;
            this.gbShowStation.TabStop = false;
            // 
            // rdShowByCatalogStation
            // 
            this.rdShowByCatalogStation.AutoSize = true;
            this.rdShowByCatalogStation.Location = new System.Drawing.Point(2, 14);
            this.rdShowByCatalogStation.Name = "rdShowByCatalogStation";
            this.rdShowByCatalogStation.Size = new System.Drawing.Size(97, 17);
            this.rdShowByCatalogStation.TabIndex = 1;
            this.rdShowByCatalogStation.Text = "לפי מקט תחנה";
            this.rdShowByCatalogStation.UseVisualStyleBackColor = true;
            this.rdShowByCatalogStation.CheckedChanged += new System.EventHandler(this.rdShowByCatalogStation_CheckedChanged);
            // 
            // rdShowByNameStation
            // 
            this.rdShowByNameStation.AutoSize = true;
            this.rdShowByNameStation.Checked = true;
            this.rdShowByNameStation.Location = new System.Drawing.Point(104, 14);
            this.rdShowByNameStation.Name = "rdShowByNameStation";
            this.rdShowByNameStation.Size = new System.Drawing.Size(92, 17);
            this.rdShowByNameStation.TabIndex = 0;
            this.rdShowByNameStation.TabStop = true;
            this.rdShowByNameStation.Text = "לפי שם תחנה";
            this.rdShowByNameStation.UseVisualStyleBackColor = true;
            this.rdShowByNameStation.CheckedChanged += new System.EventHandler(this.rdShowByNameStation_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnStationTypeUpdate);
            this.groupBox3.Controls.Add(this.cmbStationType);
            this.groupBox3.Location = new System.Drawing.Point(6, 239);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 45);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "עדכון סוג תחנה לרשומות מסומנות";
            // 
            // btnStationTypeUpdate
            // 
            this.btnStationTypeUpdate.Location = new System.Drawing.Point(6, 16);
            this.btnStationTypeUpdate.Name = "btnStationTypeUpdate";
            this.btnStationTypeUpdate.Size = new System.Drawing.Size(46, 23);
            this.btnStationTypeUpdate.TabIndex = 33;
            this.btnStationTypeUpdate.Text = "עדכן";
            this.btnStationTypeUpdate.UseVisualStyleBackColor = true;
            this.btnStationTypeUpdate.Click += new System.EventHandler(this.btnStationTypeUpdate_Click);
            // 
            // cmbStationType
            // 
            this.cmbStationType.FormattingEnabled = true;
            this.cmbStationType.Location = new System.Drawing.Point(87, 17);
            this.cmbStationType.Name = "cmbStationType";
            this.cmbStationType.Size = new System.Drawing.Size(121, 21);
            this.cmbStationType.TabIndex = 32;
            // 
            // picBox4
            // 
            this.picBox4.Location = new System.Drawing.Point(296, 260);
            this.picBox4.Margin = new System.Windows.Forms.Padding(0);
            this.picBox4.Name = "picBox4";
            this.picBox4.Size = new System.Drawing.Size(18, 20);
            this.picBox4.TabIndex = 14;
            this.picBox4.TabStop = false;
            this.picBox4.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picBox2
            // 
            this.picBox2.Location = new System.Drawing.Point(346, 260);
            this.picBox2.Margin = new System.Windows.Forms.Padding(0);
            this.picBox2.Name = "picBox2";
            this.picBox2.Size = new System.Drawing.Size(18, 20);
            this.picBox2.TabIndex = 12;
            this.picBox2.TabStop = false;
            this.picBox2.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picBox3
            // 
            this.picBox3.Location = new System.Drawing.Point(321, 260);
            this.picBox3.Margin = new System.Windows.Forms.Padding(0);
            this.picBox3.Name = "picBox3";
            this.picBox3.Size = new System.Drawing.Size(18, 20);
            this.picBox3.TabIndex = 11;
            this.picBox3.TabStop = false;
            this.picBox3.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picBox0
            // 
            this.picBox0.Location = new System.Drawing.Point(396, 260);
            this.picBox0.Margin = new System.Windows.Forms.Padding(0);
            this.picBox0.Name = "picBox0";
            this.picBox0.Size = new System.Drawing.Size(18, 20);
            this.picBox0.TabIndex = 10;
            this.picBox0.TabStop = false;
            this.picBox0.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picBox1
            // 
            this.picBox1.Location = new System.Drawing.Point(371, 260);
            this.picBox1.Margin = new System.Windows.Forms.Padding(0);
            this.picBox1.Name = "picBox1";
            this.picBox1.Size = new System.Drawing.Size(18, 20);
            this.picBox1.TabIndex = 9;
            this.picBox1.TabStop = false;
            this.picBox1.Click += new System.EventHandler(this.picBox_Click);
            // 
            // txtlblErrorDataRouteStop
            // 
            this.txtlblErrorDataRouteStop.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtlblErrorDataRouteStop.ForeColor = System.Drawing.Color.Red;
            this.txtlblErrorDataRouteStop.Location = new System.Drawing.Point(415, 286);
            this.txtlblErrorDataRouteStop.Name = "txtlblErrorDataRouteStop";
            this.txtlblErrorDataRouteStop.ReadOnly = true;
            this.txtlblErrorDataRouteStop.Size = new System.Drawing.Size(285, 13);
            this.txtlblErrorDataRouteStop.TabIndex = 6;
            // 
            // chLabelRouteStops
            // 
            this.chLabelRouteStops.AutoSize = true;
            this.chLabelRouteStops.Location = new System.Drawing.Point(625, 244);
            this.chLabelRouteStops.Name = "chLabelRouteStops";
            this.chLabelRouteStops.Size = new System.Drawing.Size(75, 17);
            this.chLabelRouteStops.TabIndex = 29;
            this.chLabelRouteStops.Text = "הצג תחנה";
            this.chLabelRouteStops.UseVisualStyleBackColor = true;
            this.chLabelRouteStops.CheckedChanged += new System.EventHandler(this.chLabelRouteStops_CheckedChanged);
            // 
            // lstBoxRouteSopsErrors
            // 
            this.lstBoxRouteSopsErrors.FormattingEnabled = true;
            this.lstBoxRouteSopsErrors.Location = new System.Drawing.Point(6, 286);
            this.lstBoxRouteSopsErrors.Name = "lstBoxRouteSopsErrors";
            this.lstBoxRouteSopsErrors.Size = new System.Drawing.Size(408, 30);
            this.lstBoxRouteSopsErrors.TabIndex = 30;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSearchStationName);
            this.groupBox4.Controls.Add(this.btnSearchStationCatalog);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txtRouteStopSearchCatalogStation);
            this.groupBox4.Controls.Add(this.btnRouteStopSearchClear);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.txtSearchStationName);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.txtRouteStopSearchCatalog);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.txtRouteStopSearchVariant);
            this.groupBox4.Controls.Add(this.txtRouteStopSearchDirection);
            this.groupBox4.Controls.Add(this.txtRouteStopSearchRouteNumber);
            this.groupBox4.Controls.Add(this.cbRouteStopsErrorsOnly);
            this.groupBox4.Location = new System.Drawing.Point(7, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(695, 44);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "חיפוש מהיר";
            // 
            // btnSearchStationName
            // 
            this.btnSearchStationName.Location = new System.Drawing.Point(43, 16);
            this.btnSearchStationName.Name = "btnSearchStationName";
            this.btnSearchStationName.Size = new System.Drawing.Size(17, 21);
            this.btnSearchStationName.TabIndex = 26;
            this.toolTipBaseForm.SetToolTip(this.btnSearchStationName, "לבחירת שם תחנה");
            this.btnSearchStationName.UseVisualStyleBackColor = true;
            this.btnSearchStationName.Click += new System.EventHandler(this.btnSearchStationName_Click);
            // 
            // btnSearchStationCatalog
            // 
            this.btnSearchStationCatalog.Location = new System.Drawing.Point(492, 16);
            this.btnSearchStationCatalog.Name = "btnSearchStationCatalog";
            this.btnSearchStationCatalog.Size = new System.Drawing.Size(17, 21);
            this.btnSearchStationCatalog.TabIndex = 20;
            this.toolTipBaseForm.SetToolTip(this.btnSearchStationCatalog, "בבחירת מקט תחנה");
            this.btnSearchStationCatalog.UseVisualStyleBackColor = true;
            this.btnSearchStationCatalog.Click += new System.EventHandler(this.btnSearchStationCatalog_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(568, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "מקט תחנה";
            // 
            // txtRouteStopSearchCatalogStation
            // 
            this.txtRouteStopSearchCatalogStation.Location = new System.Drawing.Point(509, 16);
            this.txtRouteStopSearchCatalogStation.MaxLength = 5;
            this.txtRouteStopSearchCatalogStation.Name = "txtRouteStopSearchCatalogStation";
            this.txtRouteStopSearchCatalogStation.Size = new System.Drawing.Size(56, 20);
            this.txtRouteStopSearchCatalogStation.TabIndex = 19;
            this.txtRouteStopSearchCatalogStation.TextChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // btnRouteStopSearchClear
            // 
            this.btnRouteStopSearchClear.Location = new System.Drawing.Point(5, 13);
            this.btnRouteStopSearchClear.Name = "btnRouteStopSearchClear";
            this.btnRouteStopSearchClear.Size = new System.Drawing.Size(35, 23);
            this.btnRouteStopSearchClear.TabIndex = 27;
            this.btnRouteStopSearchClear.Text = "נקה";
            this.toolTipBaseForm.SetToolTip(this.btnRouteStopSearchClear, "נקה");
            this.btnRouteStopSearchClear.UseVisualStyleBackColor = true;
            this.btnRouteStopSearchClear.Click += new System.EventHandler(this.BtnRouteStopSearchClearClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(164, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "שם תחנה";
            // 
            // txtSearchStationName
            // 
            this.txtSearchStationName.Location = new System.Drawing.Point(60, 16);
            this.txtSearchStationName.MaxLength = 20;
            this.txtSearchStationName.Name = "txtSearchStationName";
            this.txtSearchStationName.Size = new System.Drawing.Size(104, 20);
            this.txtSearchStationName.TabIndex = 25;
            this.txtSearchStationName.TextChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(465, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "מקט";
            // 
            // txtRouteStopSearchCatalog
            // 
            this.txtRouteStopSearchCatalog.Location = new System.Drawing.Point(426, 16);
            this.txtRouteStopSearchCatalog.MaxLength = 5;
            this.txtRouteStopSearchCatalog.Name = "txtRouteStopSearchCatalog";
            this.txtRouteStopSearchCatalog.Size = new System.Drawing.Size(37, 20);
            this.txtRouteStopSearchCatalog.TabIndex = 21;
            this.txtRouteStopSearchCatalog.TextChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(247, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "חלופה";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(312, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "כיוון";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(386, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "מס. קו";
            // 
            // txtRouteStopSearchVariant
            // 
            this.txtRouteStopSearchVariant.Location = new System.Drawing.Point(219, 16);
            this.txtRouteStopSearchVariant.MaxLength = 2;
            this.txtRouteStopSearchVariant.Name = "txtRouteStopSearchVariant";
            this.txtRouteStopSearchVariant.Size = new System.Drawing.Size(27, 20);
            this.txtRouteStopSearchVariant.TabIndex = 24;
            this.txtRouteStopSearchVariant.TextChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // txtRouteStopSearchDirection
            // 
            this.txtRouteStopSearchDirection.Location = new System.Drawing.Point(289, 16);
            this.txtRouteStopSearchDirection.MaxLength = 2;
            this.txtRouteStopSearchDirection.Name = "txtRouteStopSearchDirection";
            this.txtRouteStopSearchDirection.Size = new System.Drawing.Size(20, 20);
            this.txtRouteStopSearchDirection.TabIndex = 23;
            this.txtRouteStopSearchDirection.TextChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // txtRouteStopSearchRouteNumber
            // 
            this.txtRouteStopSearchRouteNumber.Location = new System.Drawing.Point(349, 17);
            this.txtRouteStopSearchRouteNumber.MaxLength = 5;
            this.txtRouteStopSearchRouteNumber.Name = "txtRouteStopSearchRouteNumber";
            this.txtRouteStopSearchRouteNumber.Size = new System.Drawing.Size(33, 20);
            this.txtRouteStopSearchRouteNumber.TabIndex = 22;
            this.txtRouteStopSearchRouteNumber.TextChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // cbRouteStopsErrorsOnly
            // 
            this.cbRouteStopsErrorsOnly.AutoSize = true;
            this.cbRouteStopsErrorsOnly.BackColor = System.Drawing.Color.Red;
            this.cbRouteStopsErrorsOnly.Location = new System.Drawing.Point(631, 18);
            this.cbRouteStopsErrorsOnly.Name = "cbRouteStopsErrorsOnly";
            this.cbRouteStopsErrorsOnly.Size = new System.Drawing.Size(58, 17);
            this.cbRouteStopsErrorsOnly.TabIndex = 18;
            this.cbRouteStopsErrorsOnly.Text = "שגוים";
            this.cbRouteStopsErrorsOnly.UseVisualStyleBackColor = false;
            this.cbRouteStopsErrorsOnly.CheckedChanged += new System.EventHandler(this.RouteStopList_TextChanged);
            // 
            // dtgRouteStops
            // 
            this.dtgRouteStops.AllowUserToAddRows = false;
            this.dtgRouteStops.AllowUserToResizeColumns = false;
            this.dtgRouteStops.AllowUserToResizeRows = false;
            this.dtgRouteStops.AutoGenerateColumns = false;
            this.dtgRouteStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgRouteStops.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.routeLineDataGridViewTextBoxColumn,
            this.Physical_Stop_ID,
            this.physicalStopDataGridViewTextBoxColumn,
            this.StationTypeName,
            this.ordinalDataGridViewTextBoxColumn,
            this.milepostDataGridViewTextBoxColumn,
            this.horadaDataGridViewTextBoxColumn,
            this.IsSelected});
            this.dtgRouteStops.DataSource = this.bndSourceRouteStops;
            this.dtgRouteStops.Location = new System.Drawing.Point(6, 57);
            this.dtgRouteStops.MultiSelect = false;
            this.dtgRouteStops.Name = "dtgRouteStops";
            this.dtgRouteStops.RowHeadersVisible = false;
            this.dtgRouteStops.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgRouteStops.Size = new System.Drawing.Size(696, 181);
            this.dtgRouteStops.TabIndex = 28;
            this.dtgRouteStops.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DtgRouteStopsMouseDown);
            this.dtgRouteStops.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dtgRouteStops_UserDeletingRow);
            // 
            // routeLineDataGridViewTextBoxColumn
            // 
            this.routeLineDataGridViewTextBoxColumn.DataPropertyName = "RouteLine";
            this.routeLineDataGridViewTextBoxColumn.HeaderText = "אשכול/מקט/ש\'/כ\'/ח\'";
            this.routeLineDataGridViewTextBoxColumn.MinimumWidth = 145;
            this.routeLineDataGridViewTextBoxColumn.Name = "routeLineDataGridViewTextBoxColumn";
            this.routeLineDataGridViewTextBoxColumn.ReadOnly = true;
            this.routeLineDataGridViewTextBoxColumn.Width = 145;
            // 
            // Physical_Stop_ID
            // 
            this.Physical_Stop_ID.DataPropertyName = "StationCatalog";
            this.Physical_Stop_ID.HeaderText = "מקט ת";
            this.Physical_Stop_ID.MinimumWidth = 65;
            this.Physical_Stop_ID.Name = "Physical_Stop_ID";
            this.Physical_Stop_ID.ReadOnly = true;
            this.Physical_Stop_ID.Width = 65;
            // 
            // physicalStopDataGridViewTextBoxColumn
            // 
            this.physicalStopDataGridViewTextBoxColumn.DataPropertyName = "PhysicalStop";
            this.physicalStopDataGridViewTextBoxColumn.HeaderText = "שם תחנה";
            this.physicalStopDataGridViewTextBoxColumn.MinimumWidth = 80;
            this.physicalStopDataGridViewTextBoxColumn.Name = "physicalStopDataGridViewTextBoxColumn";
            this.physicalStopDataGridViewTextBoxColumn.ReadOnly = true;
            this.physicalStopDataGridViewTextBoxColumn.Width = 80;
            // 
            // StationTypeName
            // 
            this.StationTypeName.DataPropertyName = "StationTypeName";
            this.StationTypeName.HeaderText = "פעילות";
            this.StationTypeName.MinimumWidth = 80;
            this.StationTypeName.Name = "StationTypeName";
            this.StationTypeName.ReadOnly = true;
            this.StationTypeName.Width = 80;
            // 
            // ordinalDataGridViewTextBoxColumn
            // 
            this.ordinalDataGridViewTextBoxColumn.DataPropertyName = "Ordinal";
            this.ordinalDataGridViewTextBoxColumn.HeaderText = "סידורי ";
            this.ordinalDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.ordinalDataGridViewTextBoxColumn.Name = "ordinalDataGridViewTextBoxColumn";
            this.ordinalDataGridViewTextBoxColumn.ReadOnly = true;
            this.ordinalDataGridViewTextBoxColumn.Width = 50;
            // 
            // milepostDataGridViewTextBoxColumn
            // 
            this.milepostDataGridViewTextBoxColumn.DataPropertyName = "MilepostRounded";
            this.milepostDataGridViewTextBoxColumn.HeaderText = "מ. מצטבר";
            this.milepostDataGridViewTextBoxColumn.MinimumWidth = 80;
            this.milepostDataGridViewTextBoxColumn.Name = "milepostDataGridViewTextBoxColumn";
            this.milepostDataGridViewTextBoxColumn.ReadOnly = true;
            this.milepostDataGridViewTextBoxColumn.Width = 80;
            // 
            // horadaDataGridViewTextBoxColumn
            // 
            this.horadaDataGridViewTextBoxColumn.DataPropertyName = "StationCatalogHorada";
            this.horadaDataGridViewTextBoxColumn.HeaderText = "ת. הורדה";
            this.horadaDataGridViewTextBoxColumn.MinimumWidth = 78;
            this.horadaDataGridViewTextBoxColumn.Name = "horadaDataGridViewTextBoxColumn";
            this.horadaDataGridViewTextBoxColumn.ReadOnly = true;
            this.horadaDataGridViewTextBoxColumn.Width = 78;
            // 
            // IsSelected
            // 
            this.IsSelected.DataPropertyName = "IsSelected";
            this.IsSelected.FalseValue = "false";
            this.IsSelected.HeaderText = "סמן";
            this.IsSelected.MinimumWidth = 48;
            this.IsSelected.Name = "IsSelected";
            this.IsSelected.ToolTipText = "סמן";
            this.IsSelected.TrueValue = "true";
            this.IsSelected.Width = 48;
            // 
            // bndSourceRouteStops
            // 
            this.bndSourceRouteStops.AllowNew = false;
            this.bndSourceRouteStops.DataSource = typeof(BLEntities.Entities.RouteStop);
            this.bndSourceRouteStops.DataSourceChanged += new System.EventHandler(this.bndSourceRouteStops_DataSourceChanged);
            this.bndSourceRouteStops.CurrentItemChanged += new System.EventHandler(this.BndSourceRouteStopsCurrentItemChanged);
            // 
            // tbRouteLines
            // 
            this.tbRouteLines.Controls.Add(this.txtOperatorName);
            this.tbRouteLines.Controls.Add(this.label4);
            this.tbRouteLines.Controls.Add(this.btnCalcFirstStationForAllRoutes);
            this.tbRouteLines.Controls.Add(this.chkPathFlow);
            this.tbRouteLines.Controls.Add(this.chkPathName);
            this.tbRouteLines.Controls.Add(this.txtDuplicated);
            this.tbRouteLines.Controls.Add(this.cmbRouteLinesField);
            this.tbRouteLines.Controls.Add(this.cbLabeling);
            this.tbRouteLines.Controls.Add(this.txtlblErrorDataRouteLine);
            this.tbRouteLines.Controls.Add(this.lstBoxRouteLineErrors);
            this.tbRouteLines.Controls.Add(this.groupBox2);
            this.tbRouteLines.Controls.Add(this.dtgRouteSystem);
            this.tbRouteLines.Location = new System.Drawing.Point(4, 22);
            this.tbRouteLines.Name = "tbRouteLines";
            this.tbRouteLines.Padding = new System.Windows.Forms.Padding(3);
            this.tbRouteLines.Size = new System.Drawing.Size(708, 322);
            this.tbRouteLines.TabIndex = 0;
            this.tbRouteLines.UseVisualStyleBackColor = true;
            // 
            // bndSourceRouteLine
            // 
            this.bndSourceRouteLine.AllowNew = false;
            this.bndSourceRouteLine.DataSource = typeof(BLEntities.Entities.RouteLine);
            this.bndSourceRouteLine.CurrentItemChanged += new System.EventHandler(this.BndSourceRouteLineCurrentItemChanged);
            // 
            // btnCalcFirstStationForAllRoutes
            // 
            this.btnCalcFirstStationForAllRoutes.Location = new System.Drawing.Point(4, 243);
            this.btnCalcFirstStationForAllRoutes.Name = "btnCalcFirstStationForAllRoutes";
            this.btnCalcFirstStationForAllRoutes.Size = new System.Drawing.Size(181, 23);
            this.btnCalcFirstStationForAllRoutes.TabIndex = 17;
            this.btnCalcFirstStationForAllRoutes.Tag = "submCalcFirstDropStationForAllSystem";
            this.btnCalcFirstStationForAllRoutes.Text = "חשב תחנת הורדה ראשונה לכל הקוים";
            this.btnCalcFirstStationForAllRoutes.UseVisualStyleBackColor = true;
            this.btnCalcFirstStationForAllRoutes.Visible = false;
            this.btnCalcFirstStationForAllRoutes.Click += new System.EventHandler(this.BtnRunClick);
            // 
            // chkPathFlow
            // 
            this.chkPathFlow.AutoSize = true;
            this.chkPathFlow.Location = new System.Drawing.Point(184, 272);
            this.chkPathFlow.Name = "chkPathFlow";
            this.chkPathFlow.Size = new System.Drawing.Size(118, 17);
            this.chkPathFlow.TabIndex = 16;
            this.chkPathFlow.Text = "הצג כיוון של דרך";
            this.chkPathFlow.UseVisualStyleBackColor = true;
            this.chkPathFlow.CheckedChanged += new System.EventHandler(this.chkPathFlow_CheckedChanged);
            // 
            // chkPathName
            // 
            this.chkPathName.AutoSize = true;
            this.chkPathName.Location = new System.Drawing.Point(308, 272);
            this.chkPathName.Name = "chkPathName";
            this.chkPathName.Size = new System.Drawing.Size(88, 17);
            this.chkPathName.TabIndex = 15;
            this.chkPathName.Text = "הצג שם דרך";
            this.chkPathName.UseVisualStyleBackColor = true;
            this.chkPathName.CheckedChanged += new System.EventHandler(this.chkPathName_CheckedChanged);
            // 
            // txtDuplicated
            // 
            this.txtDuplicated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDuplicated.ForeColor = System.Drawing.Color.Red;
            this.txtDuplicated.Location = new System.Drawing.Point(403, 306);
            this.txtDuplicated.Name = "txtDuplicated";
            this.txtDuplicated.ReadOnly = true;
            this.txtDuplicated.Size = new System.Drawing.Size(300, 13);
            this.txtDuplicated.TabIndex = 7;
            this.txtDuplicated.TabStop = false;
            // 
            // cmbRouteLinesField
            // 
            this.cmbRouteLinesField.DisplayMember = "HebrewName";
            this.cmbRouteLinesField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRouteLinesField.FormattingEnabled = true;
            this.cmbRouteLinesField.Location = new System.Drawing.Point(257, 245);
            this.cmbRouteLinesField.Name = "cmbRouteLinesField";
            this.cmbRouteLinesField.Size = new System.Drawing.Size(139, 21);
            this.cmbRouteLinesField.TabIndex = 12;
            this.cmbRouteLinesField.ValueMember = "EnglishName";
            this.cmbRouteLinesField.SelectedIndexChanged += new System.EventHandler(this.CmbRouteLinesFieldSelectedIndexChanged);
            // 
            // cbLabeling
            // 
            this.cbLabeling.AutoSize = true;
            this.cbLabeling.Location = new System.Drawing.Point(533, 266);
            this.cbLabeling.Name = "cbLabeling";
            this.cbLabeling.Size = new System.Drawing.Size(172, 17);
            this.cbLabeling.TabIndex = 11;
            this.cbLabeling.Text = "הצג מידע של קו על מפה לפי";
            this.cbLabeling.UseVisualStyleBackColor = true;
            this.cbLabeling.CheckedChanged += new System.EventHandler(this.CbLabelingCheckedChanged);
            // 
            // txtlblErrorDataRouteLine
            // 
            this.txtlblErrorDataRouteLine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtlblErrorDataRouteLine.ForeColor = System.Drawing.Color.Red;
            this.txtlblErrorDataRouteLine.Location = new System.Drawing.Point(403, 287);
            this.txtlblErrorDataRouteLine.Name = "txtlblErrorDataRouteLine";
            this.txtlblErrorDataRouteLine.ReadOnly = true;
            this.txtlblErrorDataRouteLine.Size = new System.Drawing.Size(300, 13);
            this.txtlblErrorDataRouteLine.TabIndex = 4;
            this.txtlblErrorDataRouteLine.TabStop = false;
            // 
            // lstBoxRouteLineErrors
            // 
            this.lstBoxRouteLineErrors.FormattingEnabled = true;
            this.lstBoxRouteLineErrors.Location = new System.Drawing.Point(7, 289);
            this.lstBoxRouteLineErrors.Name = "lstBoxRouteLineErrors";
            this.lstBoxRouteLineErrors.Size = new System.Drawing.Size(389, 30);
            this.lstBoxRouteLineErrors.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClusters);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtSearchCluster);
            this.groupBox2.Controls.Add(this.btnCatalogRouteLineSearch);
            this.groupBox2.Controls.Add(this.btnRouteNumberSearch);
            this.groupBox2.Controls.Add(this.chBoxRouteSearchIsNewRoutes);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtSearchCatalog);
            this.groupBox2.Controls.Add(this.btnRouteLineSearchClear);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtValiant);
            this.groupBox2.Controls.Add(this.txtDirection);
            this.groupBox2.Controls.Add(this.txtRouteNumber);
            this.groupBox2.Controls.Add(this.cbRouteLinesErrorsOnly);
            this.groupBox2.Location = new System.Drawing.Point(7, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(698, 44);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "חיפוש מהיר";
            // 
            // btnClusters
            // 
            this.btnClusters.Location = new System.Drawing.Point(479, 14);
            this.btnClusters.Name = "btnClusters";
            this.btnClusters.Size = new System.Drawing.Size(16, 21);
            this.btnClusters.TabIndex = 14;
            this.toolTipBaseForm.SetToolTip(this.btnClusters, "לבחירת מקט");
            this.btnClusters.UseVisualStyleBackColor = true;
            this.btnClusters.Click += new System.EventHandler(this.BtnClustersClick);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(544, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "אשכול";
            // 
            // txtSearchCluster
            // 
            this.txtSearchCluster.Location = new System.Drawing.Point(494, 15);
            this.txtSearchCluster.Name = "txtSearchCluster";
            this.txtSearchCluster.Size = new System.Drawing.Size(50, 20);
            this.txtSearchCluster.TabIndex = 12;
            this.txtSearchCluster.TextChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // btnCatalogRouteLineSearch
            // 
            this.btnCatalogRouteLineSearch.Location = new System.Drawing.Point(337, 14);
            this.btnCatalogRouteLineSearch.Name = "btnCatalogRouteLineSearch";
            this.btnCatalogRouteLineSearch.Size = new System.Drawing.Size(16, 21);
            this.btnCatalogRouteLineSearch.TabIndex = 3;
            this.toolTipBaseForm.SetToolTip(this.btnCatalogRouteLineSearch, "לבחירת מקט");
            this.btnCatalogRouteLineSearch.UseVisualStyleBackColor = true;
            this.btnCatalogRouteLineSearch.Click += new System.EventHandler(this.btnCatalogRouteLineSearch_Click);
            // 
            // btnRouteNumberSearch
            // 
            this.btnRouteNumberSearch.Location = new System.Drawing.Point(162, 14);
            this.btnRouteNumberSearch.Name = "btnRouteNumberSearch";
            this.btnRouteNumberSearch.Size = new System.Drawing.Size(16, 21);
            this.btnRouteNumberSearch.TabIndex = 5;
            this.toolTipBaseForm.SetToolTip(this.btnRouteNumberSearch, "לבחירת מספר קו");
            this.btnRouteNumberSearch.UseVisualStyleBackColor = true;
            this.btnRouteNumberSearch.Click += new System.EventHandler(this.btnRouteNumberSearch_Click);
            // 
            // chBoxRouteSearchIsNewRoutes
            // 
            this.chBoxRouteSearchIsNewRoutes.AutoSize = true;
            this.chBoxRouteSearchIsNewRoutes.BackColor = System.Drawing.Color.Yellow;
            this.chBoxRouteSearchIsNewRoutes.Location = new System.Drawing.Point(587, 18);
            this.chBoxRouteSearchIsNewRoutes.Name = "chBoxRouteSearchIsNewRoutes";
            this.chBoxRouteSearchIsNewRoutes.Size = new System.Drawing.Size(49, 17);
            this.chBoxRouteSearchIsNewRoutes.TabIndex = 1;
            this.chBoxRouteSearchIsNewRoutes.Text = "חדש";
            this.chBoxRouteSearchIsNewRoutes.UseVisualStyleBackColor = false;
            this.chBoxRouteSearchIsNewRoutes.CheckedChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(454, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "מקט";
            // 
            // txtSearchCatalog
            // 
            this.txtSearchCatalog.Location = new System.Drawing.Point(352, 15);
            this.txtSearchCatalog.Name = "txtSearchCatalog";
            this.txtSearchCatalog.Size = new System.Drawing.Size(101, 20);
            this.txtSearchCatalog.TabIndex = 2;
            this.txtSearchCatalog.TextChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // btnRouteLineSearchClear
            // 
            this.btnRouteLineSearchClear.Location = new System.Drawing.Point(4, 13);
            this.btnRouteLineSearchClear.Name = "btnRouteLineSearchClear";
            this.btnRouteLineSearchClear.Size = new System.Drawing.Size(34, 23);
            this.btnRouteLineSearchClear.TabIndex = 9;
            this.btnRouteLineSearchClear.Text = "נקה";
            this.toolTipBaseForm.SetToolTip(this.btnRouteLineSearchClear, "נקה");
            this.btnRouteLineSearchClear.UseVisualStyleBackColor = true;
            this.btnRouteLineSearchClear.Click += new System.EventHandler(this.BtnRouteLineSearchClearClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "חלופה";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "כיוון";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(292, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "מס. קו";
            // 
            // txtValiant
            // 
            this.txtValiant.Location = new System.Drawing.Point(48, 15);
            this.txtValiant.Name = "txtValiant";
            this.txtValiant.Size = new System.Drawing.Size(17, 20);
            this.txtValiant.TabIndex = 7;
            this.txtValiant.TextChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // txtDirection
            // 
            this.txtDirection.Location = new System.Drawing.Point(109, 15);
            this.txtDirection.Name = "txtDirection";
            this.txtDirection.Size = new System.Drawing.Size(17, 20);
            this.txtDirection.TabIndex = 6;
            this.txtDirection.TextChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // txtRouteNumber
            // 
            this.txtRouteNumber.Location = new System.Drawing.Point(177, 15);
            this.txtRouteNumber.Name = "txtRouteNumber";
            this.txtRouteNumber.Size = new System.Drawing.Size(109, 20);
            this.txtRouteNumber.TabIndex = 4;
            this.txtRouteNumber.TextChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // cbRouteLinesErrorsOnly
            // 
            this.cbRouteLinesErrorsOnly.AutoSize = true;
            this.cbRouteLinesErrorsOnly.BackColor = System.Drawing.Color.Red;
            this.cbRouteLinesErrorsOnly.Location = new System.Drawing.Point(638, 18);
            this.cbRouteLinesErrorsOnly.Name = "cbRouteLinesErrorsOnly";
            this.cbRouteLinesErrorsOnly.Size = new System.Drawing.Size(54, 17);
            this.cbRouteLinesErrorsOnly.TabIndex = 0;
            this.cbRouteLinesErrorsOnly.Text = "שגוי ";
            this.cbRouteLinesErrorsOnly.UseVisualStyleBackColor = false;
            this.cbRouteLinesErrorsOnly.CheckedChanged += new System.EventHandler(this.TxtRouteLineFastSearchTextChanged);
            // 
            // dtgRouteSystem
            // 
            this.dtgRouteSystem.AllowUserToAddRows = false;
            this.dtgRouteSystem.AllowUserToDeleteRows = false;
            this.dtgRouteSystem.AllowUserToResizeColumns = false;
            this.dtgRouteSystem.AllowUserToResizeRows = false;
            this.dtgRouteSystem.AutoGenerateColumns = false;
            this.dtgRouteSystem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dtgRouteSystem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgRouteSystem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClusterName,
            this.Catalog,
            this.RouteNumber,
            this.signpostDataGridViewTextBoxColumn,
            this.varDataGridViewTextBoxColumn,
            this.dirDataGridViewTextBoxColumn,
            this.ServiceTypeName,
            this.IsBase,
            this.roadDescriptionDataGridViewTextBoxColumn});
            this.dtgRouteSystem.DataSource = this.bndSourceRouteLine;
            this.dtgRouteSystem.Location = new System.Drawing.Point(6, 57);
            this.dtgRouteSystem.MultiSelect = false;
            this.dtgRouteSystem.Name = "dtgRouteSystem";
            this.dtgRouteSystem.ReadOnly = true;
            this.dtgRouteSystem.RowHeadersVisible = false;
            this.dtgRouteSystem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgRouteSystem.Size = new System.Drawing.Size(699, 182);
            this.dtgRouteSystem.TabIndex = 10;
            this.dtgRouteSystem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DtgRouteSystemMouseDown);
            this.dtgRouteSystem.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.DtgRouteSystemUserDeletingRow);
            this.dtgRouteSystem.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dtgRouteSystem_DataError);
            // 
            // ClusterName
            // 
            this.ClusterName.DataPropertyName = "ClusterName";
            this.ClusterName.HeaderText = "אשכול";
            this.ClusterName.MinimumWidth = 90;
            this.ClusterName.Name = "ClusterName";
            this.ClusterName.ReadOnly = true;
            this.ClusterName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ClusterName.Width = 90;
            // 
            // Catalog
            // 
            this.Catalog.DataPropertyName = "Catalog";
            this.Catalog.HeaderText = "מקט";
            this.Catalog.MinimumWidth = 53;
            this.Catalog.Name = "Catalog";
            this.Catalog.ReadOnly = true;
            this.Catalog.Width = 53;
            // 
            // RouteNumber
            // 
            this.RouteNumber.DataPropertyName = "RouteNumber";
            this.RouteNumber.HeaderText = "מספר קו";
            this.RouteNumber.MinimumWidth = 74;
            this.RouteNumber.Name = "RouteNumber";
            this.RouteNumber.ReadOnly = true;
            this.RouteNumber.Width = 74;
            // 
            // signpostDataGridViewTextBoxColumn
            // 
            this.signpostDataGridViewTextBoxColumn.DataPropertyName = "Signpost";
            this.signpostDataGridViewTextBoxColumn.HeaderText = "שילוט";
            this.signpostDataGridViewTextBoxColumn.MinimumWidth = 70;
            this.signpostDataGridViewTextBoxColumn.Name = "signpostDataGridViewTextBoxColumn";
            this.signpostDataGridViewTextBoxColumn.ReadOnly = true;
            this.signpostDataGridViewTextBoxColumn.Width = 70;
            // 
            // varDataGridViewTextBoxColumn
            // 
            this.varDataGridViewTextBoxColumn.DataPropertyName = "Var";
            this.varDataGridViewTextBoxColumn.HeaderText = "חלופה";
            this.varDataGridViewTextBoxColumn.MinimumWidth = 65;
            this.varDataGridViewTextBoxColumn.Name = "varDataGridViewTextBoxColumn";
            this.varDataGridViewTextBoxColumn.ReadOnly = true;
            this.varDataGridViewTextBoxColumn.Width = 65;
            // 
            // dirDataGridViewTextBoxColumn
            // 
            this.dirDataGridViewTextBoxColumn.DataPropertyName = "Dir";
            this.dirDataGridViewTextBoxColumn.HeaderText = "כיוון";
            this.dirDataGridViewTextBoxColumn.MinimumWidth = 59;
            this.dirDataGridViewTextBoxColumn.Name = "dirDataGridViewTextBoxColumn";
            this.dirDataGridViewTextBoxColumn.ReadOnly = true;
            this.dirDataGridViewTextBoxColumn.Width = 59;
            // 
            // ServiceTypeName
            // 
            this.ServiceTypeName.DataPropertyName = "ServiceTypeName";
            this.ServiceTypeName.HeaderText = "סוג שרות";
            this.ServiceTypeName.MinimumWidth = 90;
            this.ServiceTypeName.Name = "ServiceTypeName";
            this.ServiceTypeName.ReadOnly = true;
            this.ServiceTypeName.Width = 90;
            // 
            // IsBase
            // 
            this.IsBase.DataPropertyName = "IsBase";
            this.IsBase.HeaderText = "בסיס";
            this.IsBase.MinimumWidth = 40;
            this.IsBase.Name = "IsBase";
            this.IsBase.ReadOnly = true;
            this.IsBase.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsBase.Width = 58;
            // 
            // roadDescriptionDataGridViewTextBoxColumn
            // 
            this.roadDescriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.roadDescriptionDataGridViewTextBoxColumn.DataPropertyName = "RoadDescription";
            this.roadDescriptionDataGridViewTextBoxColumn.HeaderText = "ת. דרך";
            this.roadDescriptionDataGridViewTextBoxColumn.MaxInputLength = 200;
            this.roadDescriptionDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.roadDescriptionDataGridViewTextBoxColumn.Name = "roadDescriptionDataGridViewTextBoxColumn";
            this.roadDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.roadDescriptionDataGridViewTextBoxColumn.Width = 120;
            // 
            // tbRouteData
            // 
            this.tbRouteData.Controls.Add(this.tbRouteLines);
            this.tbRouteData.Controls.Add(this.tbRouteStops);
            this.tbRouteData.Controls.Add(this.tbPhysicalStops);
            this.tbRouteData.Location = new System.Drawing.Point(0, 140);
            this.tbRouteData.Name = "tbRouteData";
            this.tbRouteData.SelectedIndex = 0;
            this.tbRouteData.Size = new System.Drawing.Size(716, 348);
            this.tbRouteData.TabIndex = 0;
            this.tbRouteData.SelectedIndexChanged += new System.EventHandler(this.TbRouteDataSelectedIndexChanged);
            // 
            // grBxPhysicalStation
            // 
            this.grBxPhysicalStation.Controls.Add(this.chartPysicalStops);
            this.grBxPhysicalStation.Location = new System.Drawing.Point(486, 3);
            this.grBxPhysicalStation.Name = "grBxPhysicalStation";
            this.grBxPhysicalStation.Size = new System.Drawing.Size(230, 131);
            this.grBxPhysicalStation.TabIndex = 2;
            this.grBxPhysicalStation.TabStop = false;
            this.grBxPhysicalStation.Text = "תחנות פיזיות";
            // 
            // grBxStops
            // 
            this.grBxStops.Controls.Add(this.chartSystemStops);
            this.grBxStops.Location = new System.Drawing.Point(246, 3);
            this.grBxStops.Name = "grBxStops";
            this.grBxStops.Size = new System.Drawing.Size(230, 131);
            this.grBxStops.TabIndex = 2;
            this.grBxStops.TabStop = false;
            this.grBxStops.Text = "תחנות בקו";
            // 
            // menuStripRouteLines
            // 
            this.menuStripRouteLines.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnRouteLineDetails,
            this.mnRouteLineZoom,
            this.mnuRouteStops});
            this.menuStripRouteLines.Name = "menuStripRouteLines";
            this.menuStripRouteLines.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStripRouteLines.Size = new System.Drawing.Size(115, 70);
            // 
            // mnRouteLineDetails
            // 
            this.mnRouteLineDetails.Image = ((System.Drawing.Image)(resources.GetObject("mnRouteLineDetails.Image")));
            this.mnRouteLineDetails.Name = "mnRouteLineDetails";
            this.mnRouteLineDetails.Size = new System.Drawing.Size(114, 22);
            this.mnRouteLineDetails.Text = "נספח 1";
            this.mnRouteLineDetails.Click += new System.EventHandler(this.MnRouteLineDetailsClick);
            // 
            // mnRouteLineZoom
            // 
            this.mnRouteLineZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuZoomCurrent,
            this.mnuZoomAll,
            this.mnuInitMapResize});
            this.mnRouteLineZoom.Image = ((System.Drawing.Image)(resources.GetObject("mnRouteLineZoom.Image")));
            this.mnRouteLineZoom.Name = "mnRouteLineZoom";
            this.mnRouteLineZoom.Size = new System.Drawing.Size(114, 22);
            this.mnRouteLineZoom.Text = "Zoom";
            // 
            // mnuZoomCurrent
            // 
            this.mnuZoomCurrent.Image = ((System.Drawing.Image)(resources.GetObject("mnuZoomCurrent.Image")));
            this.mnuZoomCurrent.Name = "mnuZoomCurrent";
            this.mnuZoomCurrent.Size = new System.Drawing.Size(192, 22);
            this.mnuZoomCurrent.Text = "רשומה נוכחית";
            this.mnuZoomCurrent.Click += new System.EventHandler(this.MnuZoomCurrentClick);
            // 
            // mnuZoomAll
            // 
            this.mnuZoomAll.Image = ((System.Drawing.Image)(resources.GetObject("mnuZoomAll.Image")));
            this.mnuZoomAll.Name = "mnuZoomAll";
            this.mnuZoomAll.Size = new System.Drawing.Size(192, 22);
            this.mnuZoomAll.Text = "כל הקבוצה";
            this.mnuZoomAll.Click += new System.EventHandler(this.MnuZoomAllClick);
            // 
            // mnuInitMapResize
            // 
            this.mnuInitMapResize.Image = ((System.Drawing.Image)(resources.GetObject("mnuInitMapResize.Image")));
            this.mnuInitMapResize.Name = "mnuInitMapResize";
            this.mnuInitMapResize.Size = new System.Drawing.Size(192, 22);
            this.mnuInitMapResize.Text = "חזור לתצוגה ראשונית";
            this.mnuInitMapResize.Click += new System.EventHandler(this.mnuInitMapResize_Click);
            // 
            // mnuRouteStops
            // 
            this.mnuRouteStops.Image = ((System.Drawing.Image)(resources.GetObject("mnuRouteStops.Image")));
            this.mnuRouteStops.Name = "mnuRouteStops";
            this.mnuRouteStops.Size = new System.Drawing.Size(114, 22);
            this.mnuRouteStops.Text = "נספח 3";
            this.mnuRouteStops.Click += new System.EventHandler(this.MnuRouteStopsClick);
            // 
            // mnuRouteStopsData
            // 
            this.mnuRouteStopsData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarMenuRouteStopDetails,
            this.toolStripMenuItem2,
            this.mnuGetRouteLineStops});
            this.mnuRouteStopsData.Name = "menuStripRouteLines";
            this.mnuRouteStopsData.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mnuRouteStopsData.Size = new System.Drawing.Size(202, 70);
            // 
            // toolBarMenuRouteStopDetails
            // 
            this.toolBarMenuRouteStopDetails.Image = ((System.Drawing.Image)(resources.GetObject("toolBarMenuRouteStopDetails.Image")));
            this.toolBarMenuRouteStopDetails.Name = "toolBarMenuRouteStopDetails";
            this.toolBarMenuRouteStopDetails.Size = new System.Drawing.Size(201, 22);
            this.toolBarMenuRouteStopDetails.Text = "פרטי תחנה בקו";
            this.toolBarMenuRouteStopDetails.Click += new System.EventHandler(this.ToolBarMenuRouteStopDetailsClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRouteStopZoomCurrent,
            this.tbRouteStopZoomGroup,
            this.mnuInitResizeMap});
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(201, 22);
            this.toolStripMenuItem2.Text = "Zoom";
            // 
            // tbRouteStopZoomCurrent
            // 
            this.tbRouteStopZoomCurrent.Image = ((System.Drawing.Image)(resources.GetObject("tbRouteStopZoomCurrent.Image")));
            this.tbRouteStopZoomCurrent.Name = "tbRouteStopZoomCurrent";
            this.tbRouteStopZoomCurrent.Size = new System.Drawing.Size(192, 22);
            this.tbRouteStopZoomCurrent.Text = "רשומה נוכחית";
            this.tbRouteStopZoomCurrent.Click += new System.EventHandler(this.TbRouteStopZoomCurrentClick);
            // 
            // tbRouteStopZoomGroup
            // 
            this.tbRouteStopZoomGroup.Image = ((System.Drawing.Image)(resources.GetObject("tbRouteStopZoomGroup.Image")));
            this.tbRouteStopZoomGroup.Name = "tbRouteStopZoomGroup";
            this.tbRouteStopZoomGroup.Size = new System.Drawing.Size(192, 22);
            this.tbRouteStopZoomGroup.Text = "כל הקבוצה";
            this.tbRouteStopZoomGroup.Click += new System.EventHandler(this.tbRouteStopZoomGroup_Click);
            // 
            // mnuInitResizeMap
            // 
            this.mnuInitResizeMap.Image = ((System.Drawing.Image)(resources.GetObject("mnuInitResizeMap.Image")));
            this.mnuInitResizeMap.Name = "mnuInitResizeMap";
            this.mnuInitResizeMap.Size = new System.Drawing.Size(192, 22);
            this.mnuInitResizeMap.Text = "חזור לתצוגה ראשונית";
            this.mnuInitResizeMap.Click += new System.EventHandler(this.mnuInitMapResize_Click);
            // 
            // mnuGetRouteLineStops
            // 
            this.mnuGetRouteLineStops.Image = ((System.Drawing.Image)(resources.GetObject("mnuGetRouteLineStops.Image")));
            this.mnuGetRouteLineStops.Name = "mnuGetRouteLineStops";
            this.mnuGetRouteLineStops.Size = new System.Drawing.Size(201, 22);
            this.mnuGetRouteLineStops.Text = "הצג רק תחנות של הקו ";
            this.mnuGetRouteLineStops.Click += new System.EventHandler(this.mnuGetRouteLineStops_Click);
            // 
            // imageCustomList
            // 
            this.imageCustomList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageCustomList.ImageStream")));
            this.imageCustomList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageCustomList.Images.SetKeyName(0, "BUS0.BMP");
            this.imageCustomList.Images.SetKeyName(1, "BUS1.bmp");
            this.imageCustomList.Images.SetKeyName(2, "BUS2.bmp");
            this.imageCustomList.Images.SetKeyName(3, "BUS3.bmp");
            this.imageCustomList.Images.SetKeyName(4, "BUS4.bmp");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(668, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "מפעיל";
            // 
            // txtOperatorName
            // 
            this.txtOperatorName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperatorName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSourceRouteLine, "OperatorName", true));
            this.txtOperatorName.ForeColor = System.Drawing.Color.Black;
            this.txtOperatorName.Location = new System.Drawing.Point(403, 247);
            this.txtOperatorName.Name = "txtOperatorName";
            this.txtOperatorName.ReadOnly = true;
            this.txtOperatorName.Size = new System.Drawing.Size(259, 13);
            this.txtOperatorName.TabIndex = 20;
            this.txtOperatorName.TabStop = false;
            // 
            // frmRouteExportProccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 508);
            this.Controls.Add(this.tbRouteData);
            this.Controls.Add(this.grBxPhysicalStation);
            this.Controls.Add(this.grBxStops);
            this.Controls.Add(this.grBxLines);
            this.MaximizeBox = false;
            this.Name = "frmRouteExportProccess";
            this.Load += new System.EventHandler(this.RouteExportProccess_Load);
            this.Shown += new System.EventHandler(this.frmRouteExportProccess_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RouteExportProccess_FormClosed);
            this.grBxLines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSystemLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPysicalStops)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSystemStops)).EndInit();
            this.tbPhysicalStops.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgPhysicalStops)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourcePhysicalStops)).EndInit();
            this.tbRouteStops.ResumeLayout(false);
            this.tbRouteStops.PerformLayout();
            this.gbShowStation.ResumeLayout(false);
            this.gbShowStation.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRouteStops)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceRouteStops)).EndInit();
            this.tbRouteLines.ResumeLayout(false);
            this.tbRouteLines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndSourceRouteLine)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRouteSystem)).EndInit();
            this.tbRouteData.ResumeLayout(false);
            this.grBxPhysicalStation.ResumeLayout(false);
            this.grBxStops.ResumeLayout(false);
            this.menuStripRouteLines.ResumeLayout(false);
            this.mnuRouteStopsData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grBxLines;
        private System.Windows.Forms.TabPage tbPhysicalStops;
        private System.Windows.Forms.TabPage tbRouteStops;
        private System.Windows.Forms.TabPage tbRouteLines;
        private System.Windows.Forms.DataGridView dtgRouteSystem;
        private System.Windows.Forms.TabControl tbRouteData;
        private System.Windows.Forms.DataGridView dtgPhysicalStops;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dtgRouteStops;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbRouteLinesErrorsOnly;
        private System.Windows.Forms.ListBox lstBoxRouteSopsErrors;
        private System.Windows.Forms.ListBox lstBoxRouteLineErrors;
        private System.Windows.Forms.CheckBox cbRouteStopsErrorsOnly;
        private System.Windows.Forms.BindingSource bndSourceRouteLine;
        private System.Windows.Forms.BindingSource bndSourceRouteStops;
        private System.Windows.Forms.BindingSource bndSourcePhysicalStops;
        private System.Windows.Forms.DataGridViewTextBoxColumn routeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txtlblErrorDataRouteLine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtValiant;
        private System.Windows.Forms.TextBox txtDirection;
        private System.Windows.Forms.TextBox txtRouteNumber;
        private System.Windows.Forms.Button btnRouteLineSearchClear;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSystemLines;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPysicalStops;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSystemStops;
        private System.Windows.Forms.GroupBox grBxPhysicalStation;
        private System.Windows.Forms.GroupBox grBxStops;
        private System.Windows.Forms.ContextMenuStrip menuStripRouteLines;
        private System.Windows.Forms.ToolStripMenuItem mnRouteLineDetails;
        private System.Windows.Forms.ToolStripMenuItem mnRouteLineZoom;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomCurrent;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomAll;
       
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSearchCatalog;
        private System.Windows.Forms.CheckBox chBoxRouteSearchIsNewRoutes;
        private System.Windows.Forms.ToolStripMenuItem mnuRouteStops;
        private System.Windows.Forms.Button btnRouteStopSearchClear;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSearchStationName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRouteStopSearchCatalog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRouteStopSearchVariant;
        private System.Windows.Forms.TextBox txtRouteStopSearchDirection;
        private System.Windows.Forms.TextBox txtRouteStopSearchRouteNumber;
        private System.Windows.Forms.ContextMenuStrip mnuRouteStopsData;
        private System.Windows.Forms.ToolStripMenuItem toolBarMenuRouteStopDetails;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tbRouteStopZoomCurrent;
        private System.Windows.Forms.ToolStripMenuItem tbRouteStopZoomGroup;
        private System.Windows.Forms.ComboBox cmbRouteLinesField;
        private System.Windows.Forms.CheckBox cbLabeling;
        private System.Windows.Forms.TextBox txtDuplicated;
        private System.Windows.Forms.CheckBox chLabelRouteStops;
        private System.Windows.Forms.TextBox txtlblErrorDataRouteStop;
        private System.Windows.Forms.ImageList imageCustomList;
        private System.Windows.Forms.PictureBox picBox4;
        private System.Windows.Forms.PictureBox picBox2;
        private System.Windows.Forms.PictureBox picBox3;
        private System.Windows.Forms.PictureBox picBox0;
        private System.Windows.Forms.PictureBox picBox1;
        private System.Windows.Forms.Button btnCatalogRouteLineSearch;
        private System.Windows.Forms.Button btnRouteNumberSearch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRouteStopSearchCatalogStation;
        private System.Windows.Forms.Button btnSearchStationCatalog;
        private System.Windows.Forms.Button btnSearchStationName;
        private System.Windows.Forms.ToolStripMenuItem mnuGetRouteLineStops;
        private System.Windows.Forms.ToolStripMenuItem mnuInitMapResize;
        private System.Windows.Forms.ToolStripMenuItem mnuInitResizeMap;
        private System.Windows.Forms.CheckBox chkPathName;
        private System.Windows.Forms.CheckBox chkPathFlow;
        private System.Windows.Forms.Button btnClusters;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSearchCluster;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStationTypeUpdate;
        private System.Windows.Forms.ComboBox cmbStationType;
        private System.Windows.Forms.GroupBox gbShowStation;
        private System.Windows.Forms.RadioButton rdShowByCatalogStation;
        private System.Windows.Forms.RadioButton rdShowByNameStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn routeLineDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Physical_Stop_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn physicalStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordinalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn milepostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn horadaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn b56DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClusterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Catalog;
        private System.Windows.Forms.DataGridViewTextBoxColumn RouteNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn signpostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn varDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dirDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceTypeName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn roadDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnCalcFirstStationForAllRoutes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn longitudeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn latitudeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn directionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOperatorName;
    }
}

