using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLEntities.Entities;
using BLManager;
using IBLManager;
using Utilities;
using GUIBase;
using RouteExportProcess.Resources;

namespace RouteExportProcess
{
    public partial class frmRouteStopDetail : GUIBase.BaseForm
    {
        #region private
        private readonly RouteStop _routeStop = null;
        private bool _isSaved = true;
        private readonly IRouteStopDetailPresenter _routeStopDetailPresenter = null;
        private readonly List<RouteStop> _filteredList;
        readonly bool _isNotEged = !(new TransCadBlManager().IsEgedOperator);
        #endregion
        public frmRouteStopDetail()
        {
            InitializeComponent();
            _routeStopDetailPresenter = new RouteStopDetailPresenter();
        }
        /// <summary>
        /// Route Stop Detail
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="routeStop"></param>
        /// <param name="filteredList"></param>
        public frmRouteStopDetail(string caption, RouteStop routeStop, List<RouteStop> filteredList)
        {
            InitializeComponent();
            _routeStopDetailPresenter = new RouteStopDetailPresenter();
            this._routeStop = routeStop;
            bndSource.DataSource = routeStop;
            _filteredList = filteredList;
            Text = caption;

            ckBoxNotInUse.Enabled = txtPlatform.Enabled = btnCalcAutoHorada.Enabled = _isNotEged;
                
            InitBaseCombo(cmbStationType, _routeStopDetailPresenter.GetStationType());
            cmbStationHorada.Enabled = _isNotEged;
            SetEnableDisableControls();
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
        /// Verify Save Status
        /// </summary>
        private void VerifySaveStatus()
        {
            lstStatus.Items.Clear();
            if (!_isSaved)
            {
                if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ConfirmRouteStopSave))
                {
                    if (_routeStopDetailPresenter.IsRouteStopValid(bndSource.Current as RouteStop))
                    {
                        BtnSaveClick(null, null);
                    }
                    else
                    {
                        BindingContext[bndSource.Current].CancelCurrentEdit();
                        _isSaved = true;
                    }
                }
                else
                {
                    BindingContext[bndSource.Current].CancelCurrentEdit();
                    _isSaved = true;
                }
            }
            else
            {
                _isSaved = true;
            }



        }

        /// <summary>
        /// btn Save Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            lstStatus.Items.Clear();
            if (bndSource.Current != null && bndSource.Current is RouteStop)
            {
                var rs = bndSource.Current as RouteStop;
                if (cmbStationHorada.SelectedItem!=null)
                {
                    rs.Horada = Convert.ToInt32(cmbStationHorada.SelectedValue);
                }
                if (_routeStopDetailPresenter.SaveRouteStop(rs))
                {
                    lstStatus.Items.Add(Resources.ResourceGUI.SaveRouteStopDoneWithSucces);
                }
                else
                {
                    SetErrorInfo();
                    GuiHelper.ShowErrorMessage(Resources.ResourceGUI.SaveFailed);//this.BindingContext[this.bndSource.Current].CancelCurrentEdit();
                }
            }
            _isSaved = true; 
        }
        /// <summary>
        /// Set Error Info
        /// </summary>
        private void SetErrorInfo()
        {
            lstStatus.Items.Clear();
            if (bndSource.Current != null && bndSource.Current is RouteStop)
            {
                var stop = bndSource.Current as RouteStop;
                if (!_routeStopDetailPresenter.IsRouteStopValid(stop))
                {
                    lstStatus.Items.Add(string.Format(ResourceGUI.ValidationRouteStopNotPassed, stop.Name));
                    stop.ValidationErrors.ForEach(ve => lstStatus.Items.Add(string.Concat("---", ve.ErrorMessage)));
                }
                else
                {
                    lstStatus.Items.Add(string.Format(Resources.ResourceGUI.ValidationRouteLinePassed, stop.Name));
                }
            }
        }
        ///// <summary>
        ///// txtHorada KeyPress
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void txtHorada_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar != (Char)Keys.Enter && e.KeyChar != (Char)Keys.Back && !Char.IsNumber(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //}

        /// <summary>
        /// btnNext Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextClick(object sender, EventArgs e)
        {
            VerifySaveStatus();
            if (bndSource.Current != null)
            {
                var current = bndSource.Current as RouteStop;
                bndSource.DataSource = _routeStopDetailPresenter.GetNextFilterdItem(this._filteredList, current);
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
            if (bndSource.Current != null)
            {
                var current = bndSource.Current as RouteStop;
                bndSource.DataSource = _routeStopDetailPresenter.GetFirstFilterdItem(_filteredList, current);
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
            if (bndSource.Current != null)
            {
                var current = bndSource.Current as RouteStop;
                
                var lst = _routeStopDetailPresenter.GetLastFilterdItem(this._filteredList, current);
                bndSource.DataSource = lst;
                _isSaved = true;
            }            
        }
        /// <summary>
        /// Verify Save Status
        /// </summary>
        //private void VerifySaveStatus()
        //{
        //    lstStatus.Items.Clear();
        //    if (!isSaved)
        //    {
        //        if (GUIBase.GUIHelper.ShowConfirmationMessage(Resources.ResourceGUI.ConfirmRouteStopSave))
        //            btnSave_Click(null, null);
        //        else
        //        {
        //            this.BindingContext[this.bndSource.Current].CancelCurrentEdit();
        //            isSaved = true;
        //        }
        //    }
        //    else
        //    {
        //        isSaved = true;
        //    }
            
        //}
        /// <summary>
        /// btnPrev Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrevClick(object sender, EventArgs e)
        {
            VerifySaveStatus();
            if (bndSource.Current != null)
            {
                var current = bndSource.Current as RouteStop;
                bndSource.DataSource = _routeStopDetailPresenter.GetPrevFilterdItem(_filteredList, current);
                _isSaved = true;
            }
        }

        /// <summary>
        /// bndSource CurrentChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BndSourceCurrentChanged(object sender, EventArgs e)
        {
            if (bndSource.Current != null && bndSource.Current is RouteStop)
            {
                var rs = (bndSource.Current as RouteStop);
                rs.ArchiveObject();
                var isLoweringStation = BLSharedUtils.IsLoweringStation(rs);
                InitBaseCombo(cmbStationHorada, _routeStopDetailPresenter.GetStationCatalogHorada(rs));
                ckBoxNotInUse.Checked = false;
                if (!isLoweringStation && cmbStationHorada.DataSource != null && (cmbStationHorada.DataSource as List<BaseTableEntity>).IsListFull())
                {
                    cmbStationHorada.Enabled = _isNotEged;
                    cmbStationHorada.MaxDropDownItems = ((List<BaseTableEntity>) cmbStationHorada.DataSource).Count;
                }
                else
                {
                    cmbStationHorada.Enabled = false;
                    cmbStationHorada.MaxDropDownItems = 1;
                }
            }
            SetErrorInfo();
            SetEnableDisableControls();
        }
        /// <summary>
        /// Set Enable Disable Controls
        /// </summary>
        private void SetEnableDisableControls()
        {
            if (bndSource.Current != null)
            {
                var current = bndSource.Current as RouteStop;
                cmbStationType.Enabled = 
                    (_routeStopDetailPresenter.GetStationOrder(current) == StationOrderConst.Regular);
                cmbStationHorada.Enabled = _isNotEged;
                ckBoxNotInUse.Enabled = txtPlatform.Enabled = BLSharedUtils.IsCentralBusStation(current);
            }
            
        }
        /// <summary>
        /// bndSource Current Item Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BndSourceCurrentItemChanged(object sender, EventArgs e)
        {
            _isSaved = false;
        }
        /// <summary>
        /// txtHorada Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void txtHorada_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtHorada.Text))
        //    {
        //        (bndSource.Current as RouteStop).Horada = null;
        //        txtHorada.Text = "0";
        //        e.Cancel = false ;
        //        return; 
        //    }
        //    if (!Validators.IsNumeric(txtHorada.Text))
        //    {
        //        errorProvider.SetError(txtHorada, Resources.ResourceGUI.NumericValueOnly);
        //        txtHorada.Text = (bndSource.Current as RouteStop).Horada.ToString();
        //        e.Cancel = true;
        //    }
        //    else
        //        errorProvider.SetError(txtHorada, string.Empty); 
        //}
        /// <summary>
        /// Route Stop Detail Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteStopDetail_Load(object sender, EventArgs e)
        {
            _isSaved = true ;
        }
        /// <summary>
        /// Route Stop Detail Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteStopDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            VerifySaveStatus();
        }

        //private void txtHorada_Validated(object sender, EventArgs e)
        //{
        //    if (txtHorada.Text == "0")
        //    {
        //        (bndSource.Current as RouteStop).Horada = null;
        //        txtHorada.Text = null;
        //        return;
        //    }
        //}
        /// <summary>
        /// cmb Station Type Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbStationTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStationType.SelectedIndex >= 0)
            {
                var be = cmbStationType.SelectedItem as BaseTableEntity;
                var isLoweringStation = _routeStopDetailPresenter.IsLoweringStation(be);
                
                cmbStationHorada.Enabled =_isNotEged && !isLoweringStation;
                if (cmbStationHorada.DataSource!=null && ((List<BaseTableEntity>)cmbStationHorada.DataSource).Count > 0)
                    cmbStationHorada.MaxDropDownItems = ((List<BaseTableEntity>) cmbStationHorada.DataSource).Count;
            }
               
        }
        /// <summary>
        /// isSaved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbStationHoradaSelectedIndexChanged(object sender, EventArgs e)
        {
            _isSaved = false;
        }
        /// <summary>
        /// btn CalcAuto Horada Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void BtnCalcAutoHoradaClick(object sender, EventArgs e)  TODO
        //{
        //    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeHoradaStationsForAllLone))
        //    {
        //        if (_routeStopDetailPresenter.IsExistsHoradaStationInLine(_routeStop.Route_ID))
        //        {
        //            if (!GuiHelper.ShowConfirmationMessage(ResourceGUI.LineHasHoradaStationDoYouWantToContinue))
        //            {
        //                return;
        //            }
        //        }
        //        _routeStopDetailPresenter.SetHoradaInSpecificRouteLine(_routeStop.Route_ID);
        //    }
        //}

        private void BtnCalcAutoHoradaClick(object sender, EventArgs e)
        {
            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeHoradaStationsForAllLone))
                _routeStopDetailPresenter.SetHoradaInSpecificRouteLine(_routeStop.RouteId);
        }



        /// <summary>
        /// CkBoxNotInUseCheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkBoxNotInUseCheckedChanged(object sender, EventArgs e)
        {
            if (bndSource.Current == null || !(bndSource.Current is RouteStop) || !ckBoxNotInUse.Checked) return;
            _routeStopDetailPresenter.SetNotInUseFloorAndPlatform(bndSource.Current as RouteStop);
            bndSource.ResetCurrentItem();
        }
    }
}
