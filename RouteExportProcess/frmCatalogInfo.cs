using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLEntities.Entities;
using BLManager;
using GUIBase;
using IBLManager;
using BLEntities.Model;
using Utilities;

namespace RouteExportProcess
{
    public partial class frmCatalogInfo : BaseForm
    {
        private readonly ClusterToZoneManagerBL _custerToZoneManagerBl = new ClusterToZoneManagerBL();
        private readonly RouteLine _routeLineSelected;
        private readonly ILineDetailsPresenter _lineDetailsPresenter  ;
        private bool _isSaved = true;
        private readonly CatalogInfo _newCatatlog ; 
        public frmCatalogInfo()
        {
            InitializeComponent();
        }
        public frmCatalogInfo(string caption,  RouteLine routeLineSelected, CatalogInfo newCatatlog)
            : base(caption) 
        {
            InitializeComponent();
            _routeLineSelected = routeLineSelected;
            
            _lineDetailsPresenter= new LineDetailsPresenter();
            _newCatatlog = newCatatlog;
            InitData();
        }
        /// <summary>
        /// Init Base Combo
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="lst"></param>
        protected void InitBaseCombo(ComboBox cmb, List<BaseTableEntity> lst,int value)
        {
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "ID";
            cmb.DataSource = lst;
            foreach (BaseTableEntity item in cmb.Items)
            {
                if (item.ID == value)
                {
                    cmb.SelectedValue = value;
                    break; 
                }
            }
        }

        /// <summary>
        /// InitData
        /// </summary>
        private void InitData()
        {
            var idCluster = _newCatatlog == null ? _routeLineSelected.IdCluster : _newCatatlog.IdCluster;
            var accountingGroupById = _newCatatlog == null ? _routeLineSelected.AccountingGroupID : _newCatatlog.AccountingGroupID;
            InitBaseCombo(cmbClustername, this._lineDetailsPresenter.GetClusters(), idCluster);

            cmbAccGroup.DataSource = (new BLCreateNewCatalog()).GetAccountingGroupByOperatorAndCluster(new BaseTableEntity { ID = idCluster }, accountingGroupById);
            if (accountingGroupById > 0 && cmbAccGroup.DataSource!=null)
            {
                cmbAccGroup.SelectedItem = (cmbAccGroup.DataSource as List<AccountingGroup>).FindLast(el => el.AccountingGroupID == accountingGroupById);
            }
            InitBaseCombo(cmbServiceType, _lineDetailsPresenter.GetServiceType(), _newCatatlog== null ? _routeLineSelected.IdServiceType : 0);
            InitBaseCombo(cmbExclusivityLine, _lineDetailsPresenter.GetExclusivityLineType(), _newCatatlog == null ? _routeLineSelected.IdExclusivityLine : 0);
            txtRouteNumber.Text = _newCatatlog == null ? _routeLineSelected.RouteNumber.ToString() : _newCatatlog.RouteNumber.ToString();
            var clusterId = _newCatatlog == null ? _routeLineSelected.IdCluster : _newCatatlog.IdCluster;
            var clusterToZone = _custerToZoneManagerBl.GetClusterToZoneByClusterID(clusterId);
            if (clusterToZone == null || !clusterToZone.ClusterStateList.IsListFull())
            {
                errProvider.SetError(txtMainZone, Resources.ResourceGUI.ZoneNotDefinedinXML);
                errProvider.SetError(txtSubZone, Resources.ResourceGUI.ZoneNotDefinedinXML);
            }
            else
            {
                // TODO : MultiState
                txtMainZone.Text = clusterToZone.ClusterStateList[0].MainZoneName;
                txtSubZone.Text = clusterToZone.ClusterStateList[0].SubZoneName;
            }
        }

        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close(); 
        }
        

        /// <summary>
        /// Clear Combo Error
        /// </summary>
        /// <param name="sender"></param>
        private void ClearComboError(object sender)
        {
            if (((ComboBox) sender).SelectedValue.ToString() != "0")
            {
                errProvider.SetError(sender as ComboBox, string.Empty);
            }
        }
        /// <summary>
        /// cmb Cluster name Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbClusternameSelectedIndexChanged(object sender, EventArgs e)
        {
            _isSaved = false;
            ClearComboError(sender);
            
        }
        /// <summary>
        /// Set Combo Error
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="message"></param>
        private bool TrySetComboError(ComboBox cmb, string message, bool checkOnZerro)
        {
            if (cmb.SelectedValue == null  || (checkOnZerro && cmb.SelectedValue.ToString() == "0"))
            {
                errProvider.SetError(cmb, message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// btn Save Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool iscmbClusternameNotValid = TrySetComboError(cmbClustername, Resources.ResourceGUI.cmbClusternameIsEmpty,true);
            bool iscmbServiceTypeNotValid = TrySetComboError(cmbServiceType, Resources.ResourceGUI.cmbServiceTypeIsEmpty,true);
            AccountingGroup ag = null;
            bool iscmbAccountingGroupNotValid = false;
            if (cmbAccGroup.DataSource != null || cmbAccGroup.SelectedItem != null)
            {
                ag = cmbAccGroup.SelectedItem as AccountingGroup;
                if (ag.AccountingGroupID ==-1)
                {
                    errProvider.SetError(cmbAccGroup, Resources.ResourceGUI.cmbAccountingGroupIsEmpty);
                    iscmbAccountingGroupNotValid = true;
                }
            }
            else 
            {
                errProvider.SetError(cmbAccGroup, Resources.ResourceGUI.cmbAccountingGroupIsEmpty);
                iscmbAccountingGroupNotValid = true;
            }
            bool iscmbExclusivityLineTypeNotValid = TrySetComboError(cmbExclusivityLine, Resources.ResourceGUI.cmbExclusivityIsEmpty, false);
            bool istxtRouteNumberValid = false;
            if (string.IsNullOrEmpty(txtRouteNumber.Text))
            {
                errProvider.SetError(txtRouteNumber, Resources.ResourceGUI.txtRouteNumberIsEmpty);
                istxtRouteNumberValid = true;
            }
            
            if (iscmbClusternameNotValid || iscmbServiceTypeNotValid
                || istxtRouteNumberValid || iscmbExclusivityLineTypeNotValid || iscmbAccountingGroupNotValid)
            {
                return; 
            }
            ClusterToZone clusterToZone = _custerToZoneManagerBl.GetClusterToZoneByClusterID(int.Parse(cmbClustername.SelectedValue.ToString()));
            if (clusterToZone == null || !clusterToZone.ClusterStateList.IsListFull())
            {
                errProvider.SetError(txtMainZone, Resources.ResourceGUI.ZoneNotDefinedinXML);
                errProvider.SetError(txtSubZone, Resources.ResourceGUI.ZoneNotDefinedinXML);
                return; 
            }

            CatalogInfo current = _lineDetailsPresenter.GetCatalogInfo(this._routeLineSelected.Catalog);

            if (current == null || current.RouteLineBelongCounter==0 || GuiHelper.ShowConfirmationMessage(
               string.Format(Resources.ResourceGUI.YouchangeswillaffectSameRouteLines, current.RouteLineBelongCounter)))
            {
                var cInfo = new CatalogInfo
                {
                    Catalog = this._routeLineSelected.Catalog,

                    AccountingGroupID = ag==null ? 0 : ag.AccountingGroupID,

                    IdCluster = cmbClustername.SelectedValue != null ? int.Parse(cmbClustername.SelectedValue.ToString()) : 0,
                    RouteNumber = string.IsNullOrEmpty(txtRouteNumber.Text) ? null : (int?)int.Parse(txtRouteNumber.Text),
                    IdExclusivityLineType = cmbExclusivityLine.SelectedValue != null ? int.Parse(cmbExclusivityLine.SelectedValue.ToString()) : 0,
                    IdServiceType = cmbServiceType.SelectedValue != null ? int.Parse(cmbServiceType.SelectedValue.ToString()) : 0,
                    IdZoneHead = clusterToZone.ClusterStateList[0].MainZoneId,
                    IdZoneSubHead = clusterToZone.ClusterStateList[0].SubZoneId,
                    IsNew = false,
                    RouteLineBelongCounter =0
                };
                _lineDetailsPresenter.UpdateCatalogOfRouteLines(cInfo);
            }
            _isSaved = true;
            DialogResult = DialogResult.OK;
            //this.Close();  
        }
        /// <summary>
        /// txtRouteNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtRouteNumberKeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numeric’s
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
            
        }
        /// <summary>
        /// txt Route Number Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteNumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRouteNumber.Text)) 
                errProvider.SetError(txtRouteNumber, string.Empty );
        }

        /// <summary>
        /// Verify Save Status
        /// </summary>
        private void VerifySaveStatus()
        {
            if (!_isSaved)
            {
                if (GUIBase.GuiHelper.ShowConfirmationMessage(Resources.ResourceGUI.ConfirmRouteLineSave))
                {
                    btnSave_Click(null, null);
                }
            }
            else
            {
                _isSaved = true;
            }
        }

        /// <summary>
        /// frm Catalog Info Form Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCatalogInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            VerifySaveStatus();
        }

        private void frmCatalogInfo_Load(object sender, EventArgs e)
        {
            _isSaved = true; 
        }
        /// <summary>
        /// txt RouteNumber Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteNumber_Validating(object sender, CancelEventArgs e)
        {
            // Deny first 0
            if (!string.IsNullOrEmpty(txtRouteNumber.Text) && (txtRouteNumber.Text.StartsWith("0")))
            {
                e.Cancel = true;
                errProvider.SetError(txtRouteNumber, Resources.ResourceGUI.txtRouteNumberHasFirstZero);
            }
            else
            {
                errProvider.SetError(txtRouteNumber, string.Empty);
            }
        }

        private void cmbAccGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isSaved = false;
            ClearComboError(sender);
        }
    }
}
