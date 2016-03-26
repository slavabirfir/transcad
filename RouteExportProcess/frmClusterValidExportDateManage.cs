using System;
using System.ComponentModel;
using System.Windows.Forms;
using BLEntities.Entities;
using BLManager;
using GUIBase;
using RouteExportProcess.Resources;

namespace RouteExportProcess
{
    public partial class frmClusterValidExportDateManage : GUIBase.BaseForm
    {
        #region vars
        private BlClusterValidExportDateManage _blClusterValidExportDateManage;
        #endregion
        /// <summary>
        /// ctr
        /// </summary>
        public frmClusterValidExportDateManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// InitComponents
        /// </summary>
        protected override void InitComponents()
        {
            base.InitComponents();
            _blClusterValidExportDateManage = new BlClusterValidExportDateManage();
            lstOpers.DataSource = _blClusterValidExportDateManage.OperatorValidateLineExportDateData;
            cmdClusters.DisplayMember = "Name";
            cmdClusters.ValueMember = "ID";
            msValidateDateTime.Validating += MsValidateDateTimeValidating;
            msValidateDateTime.KeyDown += MsValidateDateTimeKeyDown;
        }
        /// <summary>
        /// btnClose_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// MsValidateDateTimeValidating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MsValidateDateTimeValidating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(msValidateDateTime.Text) && msValidateDateTime.Text != @"  /  /")
            {
                string str = msValidateDateTime.Text;
                string[] format = { "dd/MM/yyyy" };
                DateTime date;

                if (!DateTime.TryParseExact(str, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    e.Cancel = true;
                    errorProvider.SetError(msValidateDateTime, Resources.ResourceGUI.DateNotValid);
                }
            }
        }
        /// <summary>
        /// MsValidateDateTimeKeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MsValidateDateTimeKeyDown(object sender, KeyEventArgs e)
        {
            errorProvider.SetError(this.msValidateDateTime, string.Empty);
        }
        /// <summary>
        /// UpdateClusterList
        /// </summary>
        private void UpdateClusterList()
        {
            lstOperClusters.DataSource = null;
            if (lstOpers.SelectedIndex < 0) return;
            var operatorValidateLineExportDateSelected = lstOpers.SelectedItem as OperatorValidateLineExportDate;
            lstOperClusters.DataSource = _blClusterValidExportDateManage.GetClusterDataByOperatorValidateLineExportDateSelected(operatorValidateLineExportDateSelected);
        }

        /// <summary>
        /// lstOpers_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstOpers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            if (lstOpers.SelectedIndex < 0) return;
            var operatorValidateLineExportDateSelected = lstOpers.SelectedItem as OperatorValidateLineExportDate;
            cmdClusters.DataSource = _blClusterValidExportDateManage.GetClustersByOperatorId(operatorValidateLineExportDateSelected);
            lstOperClusters.DataSource = _blClusterValidExportDateManage.GetClusterDataByOperatorValidateLineExportDateSelected(operatorValidateLineExportDateSelected);
        }
        
       
        
        /// <summary>
        /// Clear
        /// </summary>
        private void Clear()
        {
            msValidateDateTime.Text = string.Empty;
            if (cmdClusters.DataSource != null && cmdClusters.Items.Count>0)
                cmdClusters.SelectedIndex = 0; // index of string.Empty value
        }

        /// <summary>
        /// IsSaveAddOperationEnabled
        /// </summary>
        /// <returns></returns>
        private bool IsSaveAddOperationEnabled()
        {
            if (lstOpers.SelectedItem != null && cmdClusters.SelectedIndex == 0)
            {
                GuiHelper.ShowErrorMessage(ResourceGUI.SelectOperatprAndCluster);
                return false;
            }
            if (string.IsNullOrEmpty(msValidateDateTime.Text) || msValidateDateTime.Text == @"  /  /")
            {
                if (!GuiHelper.ShowConfirmationMessage(ResourceGUI.SelectOperatprAndClusterWhenValidDateEmptyConfirm))
                {
                    return false;
                }
            }
            
            return true;
        }
        /// <summary>
        /// btnSaveRow_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveRow_Click(object sender, EventArgs e)
        {
            if (IsSaveAddOperationEnabled())
            {
                if (_blClusterValidExportDateManage != null)
                    _blClusterValidExportDateManage.SaveRow(lstOpers.SelectedItem as OperatorValidateLineExportDate,Convert.ToInt32(cmdClusters.SelectedValue), msValidateDateTime.Text);
                UpdateClusterList();
            }
        }
        /// <summary>
        /// btnDeleteRow_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (lstOperClusters.SelectedIndex >= 0)
            {
                var clusterValidateLineExportDate = lstOperClusters.SelectedItem as ClusterValidateLineExportDate;
                if (clusterValidateLineExportDate != null)
                {
                    if (GuiHelper.ShowConfirmationMessage(Resources.ResourceGUI.ConfirmDeleteRow))
                    {
                        _blClusterValidExportDateManage.DeleteClusterValidateLineExportDate(
                            lstOpers.SelectedItem as OperatorValidateLineExportDate, clusterValidateLineExportDate);
                        UpdateClusterList();
                    }
                }
            }
            
            
        }

        private void lstOperClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOperClusters.SelectedIndex < 0) return;
            var clusterValidateLineExportDate = lstOperClusters.SelectedItem as ClusterValidateLineExportDate;
            if (clusterValidateLineExportDate != null)
            {
                msValidateDateTime.Text = clusterValidateLineExportDate.ValidExportDate.HasValue ? clusterValidateLineExportDate.ValidExportDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                cmdClusters.SelectedValue = clusterValidateLineExportDate.IdCluster;
                if (cmdClusters.SelectedIndex == -1)
                {
                    GuiHelper.ShowErrorMessage(ResourceGUI.IdClusterNotDefinedInClusterToZoneFile);
                }
            }
            else
            {
                Clear();
            }
        }
    }
}
