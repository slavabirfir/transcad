using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using BLEntities.Entities;
using IBLManager;
using BLManager;
using Logger;
using Utilities;
using GUIBase;
using BLEntities.Model;
using RouteExportProcess.Resources;

namespace RouteExportProcess
{
    public partial class frmLineDetails : BaseForm
    {
        #region private
        private readonly RouteLine routeLine;
        private readonly ILineDetailsPresenter _lineDetailsPresenter ;
        private readonly List<RouteLine> _filteredList;
        private bool _isSaved = true;
        #endregion
        /// <summary>
        /// LineDetails constractor
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="routeLine"></param>
        /// <param name="filteredList"></param>
        public frmLineDetails(string caption, RouteLine routeLine, List<RouteLine> filteredList)
            : base(caption)
        {
            InitializeComponent();
            _lineDetailsPresenter = new LineDetailsPresenter();
            this.routeLine = routeLine;
            _filteredList = filteredList;
            InitData();
            txtRouteDesc.Enabled = txtDefinitios.Enabled = !_lineDetailsPresenter.IsEgedOperator;

            msValidateDateTime.Enabled = _lineDetailsPresenter.IsSuperViser;
            
            msValidateDateTime.Visible = !_lineDetailsPresenter.IsEgedOperator;
            lblValidDateExport.Visible = !_lineDetailsPresenter.IsEgedOperator;

            msValidateDateTime.Validating += MsValidateDateTimeValidating;
            msValidateDateTime.KeyDown += MsValidateDateTimeKeyDown;
            SetLineEnabledControls(!_lineDetailsPresenter.IsEgedOperator);
        }

        void MsValidateDateTimeValidating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(msValidateDateTime.Text) && msValidateDateTime.Text!=@"  /  /")
            {
                var str = msValidateDateTime.Text;
                string[] format = {"dd/MM/yyyy"};
                DateTime date;

                if (!DateTime.TryParseExact(str, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                     e.Cancel = true;
                     errorProvider.SetError(msValidateDateTime, Resources.ResourceGUI.DateNotValid);
                }
            }            
        }

        void MsValidateDateTimeKeyDown(object sender, KeyEventArgs e)
        {
            errorProvider.SetError(msValidateDateTime, string.Empty );
        }

        /// <summary>
        /// InitData
        /// </summary>
        private void InitData()
        {
            cmbDirection.DataSource = _lineDetailsPresenter.GetDirections();

            cmbEshkol.DisplayMember = "Name";
            cmbEshkol.ValueMember  = "ID";

            cmbAccountingGroup.DisplayMember = "AccountingGroupDesc";
            cmbAccountingGroup.ValueMember = "AccountingGroupID";
             
            cmbEshkol.DataSource = _lineDetailsPresenter.GetClusters();
            bndSourceRouteLine.DataSource = routeLine;


            InitBaseCombo(cmbServiceType, _lineDetailsPresenter.GetServiceType(), routeLine.IsNewEntity ? 0 : routeLine.IdServiceType);

            Text = string.Format(ResourceGUI.Nispah1Form, GlobalData.LoginUser.UserOperator.OperatorName);
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
        /// btnNext Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextClick(object sender, EventArgs e)
        {
            VerifySaveStatus();
            if (bndSourceRouteLine.Current != null)
            {
                var current = bndSourceRouteLine.Current as RouteLine;
                bndSourceRouteLine.DataSource = _lineDetailsPresenter.GetNextFilterdItem(this._filteredList, current);
                _isSaved = true;
            }
        }
        /// <summary>
        /// btnLast Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLastClick(object sender, EventArgs e)
        {
            VerifySaveStatus();
            if (bndSourceRouteLine.Current != null)
            {
                var current = bndSourceRouteLine.Current as RouteLine;
                var lst = _lineDetailsPresenter.GetLastFilterdItem(_filteredList, current);
                bndSourceRouteLine.DataSource = lst;
                _isSaved = true;
            }
        }
        /// <summary>
        /// btnFirst Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFirstClick(object sender, EventArgs e)
        {
            VerifySaveStatus();
            if (bndSourceRouteLine.Current != null)
            {
                var current = bndSourceRouteLine.Current as RouteLine;
                bndSourceRouteLine.DataSource = _lineDetailsPresenter.GetFirstFilterdItem(_filteredList, current);
                _isSaved = true;
            }
        }
        /// <summary>
        /// btnPrev Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrevClick(object sender, EventArgs e)
        {
            VerifySaveStatus();
            if (bndSourceRouteLine.Current != null)
            {
                var current = bndSourceRouteLine.Current as RouteLine;
                bndSourceRouteLine.DataSource = _lineDetailsPresenter.GetPrevFilterdItem(_filteredList, current);
                _isSaved = true;
            }
        }

        /// <summary>
        /// VerifySaveStatus
        /// </summary>
        private void VerifySaveStatus()
        {
            lstStatus.Items.Clear();
            if (!_isSaved)
            {

                if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ConfirmRouteLineSave))
                {
                    if (_lineDetailsPresenter.IsRouteLineValid(bndSourceRouteLine.Current as RouteLine))
                    {
                        BtnSaveClick(null, null);
                    }
                    else
                    {
                        BindingContext[bndSourceRouteLine.Current].CancelCurrentEdit();
                        _isSaved = true;
                    }
                }
                else
                {
                    BindingContext[bndSourceRouteLine.Current].CancelCurrentEdit();
                    _isSaved = true;
                }
            }
            else
            {
                _isSaved = true;
            }
        }

        /// <summary>
        /// Set Combo Error
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="message"></param>
        private bool TrySetComboError(ComboBox cmb, string message, bool checkOnZerro)
        {
            if (cmb.SelectedValue == null || (checkOnZerro && cmb.SelectedValue.ToString() == "0"))
            {
                errorProvider.SetError(cmb, message);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Clear Combo Error
        /// </summary>
        /// <param name="sender"></param>
        private void ClearComboError(object sender)
        {
            if (((ComboBox) sender).SelectedValue.ToString() != "0")
            {
                errorProvider.SetError(sender as ComboBox, string.Empty);
            }
        }


        private void SetErrorInfoAndBinding()
        {
            SetErrorInfo();
            GuiHelper.ShowErrorMessage(ResourceGUI.SaveFailed);
            BindingContext[bndSourceRouteLine.Current].CancelCurrentEdit();

            bndSourceRouteLine.ResetCurrentItem();
        }

        /// <summary>
        /// btnSave Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            lstStatus.Items.Clear();
            if (TrySetComboError(cmbServiceType, ResourceGUI.cmbServiceTypeIsEmpty, true))
            {
                SetErrorInfoAndBinding();
                return; 
            }
            if (bndSourceRouteLine.Current != null)
            {
                var rl = bndSourceRouteLine.Current as RouteLine;
                //ClusterState clusterState = null;
                //if (_lineDetailsPresenter.ShowSelectStateForm(rl))
                //{
                //    var stateSelect = new FrmClusterStateSelect(GlobalData.ClusterToZoneDictionary[rl.IdCluster].ClusterStateList, GlobalData.ClusterToZoneDictionary[rl.IdCluster].ClusterName);
                //    stateSelect.ShowDialog();
                //    clusterState = stateSelect.SelectedClusterState;
                //}
                if (_lineDetailsPresenter.SaveRouteLine(rl))//, clusterState))
                  lstStatus.Items.Add(string.Format(ResourceGUI.SaveRouteLineDoneWithSuccese, rl.Name));
                else
                    SetErrorInfoAndBinding();
                if (rl != null) ckBIsBase.Enabled = !rl.IsBase;
            }

            _isSaved = true;
        }
        /// <summary>
        /// bndSourceRouteLine CurrentItemChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BndSourceRouteLineCurrentItemChanged(object sender, EventArgs e)
        {
            _isSaved = false;
        }
        /// <summary>
        /// LineDetails Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineDetails_Load(object sender, EventArgs e)
        {
            _isSaved = true;

        }
        /// <summary>
        /// LineDetails FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            VerifySaveStatus();
        }

        /// <summary>
        /// SetLineEnabledControls
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetLineEnabledControls(bool isEnable)
        {
            txtVariant.Enabled = isEnable;
            txtSignPost.Enabled = isEnable;
            cmbDirection.Enabled = isEnable;
            cmbAccountingGroup.Enabled = isEnable;
            cmbEshkol.Enabled = isEnable;
            if (isEnable)// From 15/01/2014 Tamar && !_lineDetailsPresenter.IsPlanningFirm)
                txtRouteNumber.Enabled = (cmbEshkol.SelectedIndex >= 0 && bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null);
            else
                txtRouteNumber.Enabled = false;
            cmbServiceType.Enabled = isEnable;
        }

        /// <summary>
        /// Set Error Info
        /// </summary>
        private void SetErrorInfo()
        {
            lstStatus.Items.Clear();
            if (bndSourceRouteLine.Current != null)
            { 
                var line = bndSourceRouteLine.Current as RouteLine;
                if (line != null)
                {
                    var isLuz = line.Catalog == null ? false : _lineDetailsPresenter.IsLuzOfLineExists(line);
                    SetLineEnabledControls(!isLuz);
                }
                if (!_lineDetailsPresenter.IsRouteLineValid(line))
                {
                    if (line != null)
                    {
                        lstStatus.Items.Add(string.Format(ResourceGUI.ValidationRouteLineNotPassed, line.Name));
                        line.ValidationErrors.ForEach(ve => lstStatus.Items.Add(string.Concat("---", ve.ErrorMessage)));
                    }
                }
                else
                {
                    if (line != null)
                        lstStatus.Items.Add(string.Format(ResourceGUI.ValidationRouteLinePassed, line.Name));
                }
            }
            else
                SetLineEnabledControls(true);
        }
        /// <summary>
        /// bnd Source Route Line Current Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void BndSourceRouteLineCurrentChanged(object sender, EventArgs e)
        //{
        //    cmbDirection.SelectedIndex = 0;
        //    if (bndSourceRouteLine.Current != null)
        //    {
        //        var rl = (bndSourceRouteLine.Current as RouteLine);
        //        if (rl != null)
        //        {
        //            rl.ArchiveObject();
        //            ckBIsBase.Enabled = !rl.IsBase;
        //        }
        //        SetInfoMessage(bndSourceRouteLine.Current as RouteLine);
        //        if (rl != null)
        //        {
        //            if (cmbEshkol.DataSource!=null && rl.IdCluster>0)
        //            {
        //                var lst = cmbEshkol.DataSource as List<BaseTableEntity>;
        //                if (lst!=null)
        //                {
        //                    for (var i = 0; i < lst.Count;i++)
        //                    {
        //                        if (lst[i].ID == rl.IdCluster)
        //                        {
        //                            cmbEshkol.SelectedIndex = i;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                cmbEshkol.SelectedIndex = -1;
        //            }
        //            cmbAccountingGroup.SelectedValue = rl.AccountingGroupID;
        //        }
        //    }
        //    SetErrorInfo();

        //}

        private void BndSourceRouteLineCurrentChanged(object sender, EventArgs e)
        {
            cmbDirection.SelectedIndex = 0;
            if (bndSourceRouteLine.Current != null)
            {
                var rl = (bndSourceRouteLine.Current as RouteLine);
                if (rl != null)
                {
                    rl.ArchiveObject();
                    ckBIsBase.Enabled = !rl.IsBase;
                }
                SetInfoMessage(bndSourceRouteLine.Current as RouteLine);
                if (rl != null)
                {
                    cmbEshkol.SelectedValue = rl.IdCluster;
                    cmbAccountingGroup.SelectedValue = rl.AccountingGroupID;
                }
            }
            SetErrorInfo();

        }


        /// <summary>
        /// Set Info Message
        /// </summary>
        private void SetInfoMessage(RouteLine routeLine)
        {
            if (routeLine != null)
                groupBoxContainer.Text = string.Format(Resources.ResourceGUI.RouteLineDetails, routeLine.Name);

        }
        /// <summary>
        /// btn Verify Validation Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVerifyValidationClick(object sender, EventArgs e)
        {
            SetErrorInfo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtRouteNumberKeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numeric’s
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)))
                e.Handled = true;

        }
        /// <summary>
        /// txt Variant Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtVariantValidating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtVariant.Text))
            {
                if (!_lineDetailsPresenter.IsVariantExistsInVariantNumTable(txtVariant.Text))
                {
                    errorProvider.SetError(txtVariant, Resources.ResourceGUI.VariantNotExistsInVarNumberTable);
                    e.Cancel = true;
                    return;
                }

            }
            errorProvider.SetError(txtVariant, string.Empty);
        }

       

        /// <summary>
        /// txt Sign Post Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSignPostValidating(object sender, CancelEventArgs e)
        {
            // Deny first 0
            if (!string.IsNullOrEmpty(txtSignPost.Text) && (txtSignPost.Text.StartsWith("0")))
            {
                e.Cancel = true;
                errorProvider.SetError(txtSignPost, ResourceGUI.txtSignPostHasFirstZero);
            }

            else
            {
                errorProvider.SetError(txtSignPost, string.Empty);
            }
        }


        /// <summary>
        /// txt Definitios Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDefinitiosValidating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtDefinitios.Text))
            {
                txtDefinitios.Text = @"0";
            }
        }
        /// <summary>
        /// txtDefinitios Key Press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDefinitiosKeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numeric’s
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) || txtDefinitios.Text.Length >= 4)
                e.Handled = true;
        }
        /// <summary>
        /// cmb Eshkol Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbEshkolSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEshkol.SelectedIndex >= 0 && bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null  )
            {
                var cluster = cmbEshkol.SelectedItem as BaseTableEntity;
                var line = bndSourceRouteLine.Current as RouteLine;
                if (cluster != null)
                    if (line != null)
                        if (!line.IsNewEntity && cluster.ID != line.IdCluster)
                        {
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.LineDetailEshkolConfirmation))
                            {
                                _lineDetailsPresenter.UpdateClusterForSameRouteLines(bndSourceRouteLine.Current as RouteLine, cmbEshkol.SelectedItem as BaseTableEntity);
                                cmbAccountingGroup.DataSource = _lineDetailsPresenter.GetAccountingGroupByOperatorAndCluster(cmbEshkol.SelectedItem as BaseTableEntity, 0);
                            }
                            else
                            {
                                cmbEshkol.SelectedValue = line.IdCluster;
                            }
                        }
                if (!_lineDetailsPresenter.IsEgedOperator)// From 15/01/2014 && !_lineDetailsPresenter.IsPlanningFirm)
                     txtRouteNumber.Enabled = true;
                _isSaved = false;
            }
            else
            {
                txtRouteNumber.Enabled = false;
            }
        }
        /// <summary>
        /// Update Route Line GUI
        /// </summary>
        /// <param name="routeLine"></param>
        private void UpdateRouteLineGui(RouteLine routeLine)
        {
            bndSourceRouteLine.ResetBindings(false);
        }
        /// <summary>
        /// txtRouteNumber_Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtRouteNumberLeave(object sender, EventArgs e)
        {
            var processWaitingManager = new ProcessWaitingManager();

            try
            {
                processWaitingManager.CloseProcessWaitingForm();
                // from 15/01/2014 Tamar
                //if (_lineDetailsPresenter.IsPlanningFirm) return;
                if (bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null &&
                    !string.IsNullOrEmpty(txtRouteNumber.Text))
                {
                    var rouleLine = bndSourceRouteLine.Current as RouteLine;
                    var sameRouteNumberCatalogs = _lineDetailsPresenter.GetCatalogsOfSameRouteNumber(rouleLine,
                                                                                                     txtRouteNumber.Text);
                    var chooseCatalogForm = new frmChooseCatalog(sameRouteNumberCatalogs);
                    if (sameRouteNumberCatalogs.IsListFull())
                        chooseCatalogForm.ShowDialog();
                    var rl = bndSourceRouteLine.Current as RouteLine;
                    if (rl!=null && _lineDetailsPresenter.ShowSelectStateForm(rouleLine))
                    {
                        var stateSelect =
                            new FrmClusterStateSelect(
                                GlobalData.ClusterToZoneDictionary[rl.IdCluster].ClusterStateList,
                                GlobalData.ClusterToZoneDictionary[rl.IdCluster].ClusterName);
                        stateSelect.ShowDialog();
                        _lineDetailsPresenter.UpdateClusterZone(rl, stateSelect.SelectedClusterState);
                    }
                    string error = string.Empty;
                    processWaitingManager.ShowProcessWaitingForm();
                    _lineDetailsPresenter.UpdateCatalogWithSelectedCatalog(rouleLine, chooseCatalogForm.SelectedCatalog,txtRouteNumber.Text, ref error);
                    processWaitingManager.CloseProcessWaitingForm();
                    processWaitingManager.CloseProcessWaitingForm();
                    if (!string.IsNullOrEmpty(error))
                    {
                        if (rl != null) 
                            rl.CancelEdit();
                        GuiHelper.ShowErrorMessage(error);
                    }
                }
            }
            finally
            {
                processWaitingManager.CloseProcessWaitingForm();
                processWaitingManager.CloseProcessWaitingForm();
                processWaitingManager.CloseProcessWaitingForm();
            }

        }
        /// <summary>
        /// cmbAccountingGroup_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbAccountingGroupSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccountingGroup.SelectedIndex>=0 && bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null && cmbAccountingGroup.SelectedIndex >= 0)
            {
                var accountingGroup = cmbAccountingGroup.SelectedItem as AccountingGroup;
                var line = bndSourceRouteLine.Current as RouteLine;
                if (accountingGroup != null)
                    if (line != null)
                        if (!line.IsNewEntity && accountingGroup.AccountingGroupID != line.AccountingGroupID)
                        {
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.LineDetailAccountingGroupConfirmation))
                            {
                                _lineDetailsPresenter.UpdateAccountingGroupForSameRouteLines(bndSourceRouteLine.Current as RouteLine, cmbAccountingGroup.SelectedItem as AccountingGroup);
                            }
                            else
                            {
                                cmbAccountingGroup.SelectedValue = line.AccountingGroupID;
                            }
                        }
            }
        }

        private void CmbServiceTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            _isSaved = false;
            ClearComboError(sender);
        }

        private void CmbDirectionSelectedIndexChanged(object sender, EventArgs e)
        {
            _isSaved = false;
        }
        
    }


}
