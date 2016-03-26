using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLEntities.Entities;
using Utilities;
using BLManager;

namespace RouteExportProcess
{
    public partial class frmExportValidateDateOptions : GUIBase.BaseForm
    {
        #region vars
        private BlClusterValidExportDateManage _blClusterValidExportDateManage;
        #endregion

        #region props
        public bool IsInsideExportProcess { get; set; }
        public List<RouteLine> Routes { get; set; }
        public List<string>  ReportResults { get; set; }
        #endregion

        /// <summary>
        /// cntr
        /// </summary>
        public frmExportValidateDateOptions()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Init Components
        /// </summary>
        protected override void InitComponents()
        {
            base.InitComponents();
            _blClusterValidExportDateManage = new BlClusterValidExportDateManage();
            gbExport.Visible = IsInsideExportProcess;
            var result = ReportResults ?? _blClusterValidExportDateManage.GetExportValidationDateReport(Routes);
            if (result!=null && result.IsListFull())
            {
                lstReport.DataSource = result;
                btnPrint.Enabled = true;
            }
            else
            {
                lstReport.DataSource = null;
                btnPrint.Enabled = false;
            }

        }
        /// <summary>
        /// btn Close Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// btn Cancel Export Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelExport_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// btnContinueExport_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinueExport_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }
        /// <summary>
        /// btn Print Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            _blClusterValidExportDateManage.ShowReport(lstReport.DataSource as List<string>);
        }
    }
}
