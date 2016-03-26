using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLEntities.Entities;
using BLEntities.Report;

namespace RouteExportProcess
{
    public partial class frmReportErrorRouteStopResults : GUIBase.BaseForm
    {

        private List<ErrorReportEntity> lstData = null;

        public frmReportErrorRouteStopResults()
        {
            InitializeComponent();
        }
        public frmReportErrorRouteStopResults(string caption, List<ErrorReportEntity> lstData)
            : base(caption)
        {
            InitializeComponent();
            this.lstData = lstData; 
        }
        
        private void frmReportErrorResults_Load(object sender, EventArgs e)
        {
            this.reportViewer.Visible = true;

            this.reportViewer.LocalReport.DataSources[0].Value = lstData;
            this.reportViewer.RefreshReport();
        }
    }
}
