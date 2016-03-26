using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ExportConfiguration;
using Utilities;
using BLManager;
using IBLManager;
using GUIBase;
using BLEntities.Model;
using BLEntities.Entities;
using System.Windows.Forms.DataVisualization.Charting;
using BLEntities.Accessories;
using RouteExportProcess.Resources;
using System.IO;
using System.Threading;

namespace RouteExportProcess
{
    public partial class frmRouteExportProccess : BaseForm
    {
        #region Private Date
        bool _isRouteStopZoomWasDone;
        ITransCadBLManager _blManager;
        readonly IPresentationRouteMainForm _presentation;
        readonly IDataSearchAndManipulateBLManager _dataSearchAndManipulateBlManager;
        ProcessWaitingManager _processWaitingManager = new ProcessWaitingManager();
        private readonly AdministrationBl _administrationBl = new AdministrationBl();
        readonly MenuItem _submExportBaseDasaToLicensing = new MenuItem(ResourceGUI.submExportBaseDasaToLicensing);//"ייצא תשתיות למערכת הרישוי"
        readonly MenuItem _submExportLinesToLicensingSystem = new MenuItem(ResourceGUI.submExportLinesToLicensingSystem);//"ייצא מזהי קווים לרישוי"
        readonly MenuItem _submExportPriceListToLicensingSystem = new MenuItem(ResourceGUI.submExportPriceListToLicensingSystem);//"ייצוא של מחירונים למערכת רישוי"
        readonly MenuItem _submUpdateAreaPriceDetails = new MenuItem(ResourceGUI.submUpdateAreaPriceDetails);//
        private System.Threading.Timer _transcadActivity;
        #endregion

        #region Constractor
        public frmRouteExportProccess()
        {
            InitializeComponent();
            _presentation = new PresentationRouteMainForm();
            _dataSearchAndManipulateBlManager = new DataSearchAndManipulateBLManager();

        }
        /// <summary>
        /// Show Graphs
        /// </summary>
        /// <param name="isVisible"></param>
        private void SetGraphVisibility(bool isVisible)
        {
            //chartSystemStops.Visible = isVisible;
            //chartPysicalStops.Visible = isVisible;
            //chartSystemLines.Visible = isVisible;

            grBxPhysicalStation.Visible = isVisible;
            grBxStops.Visible = isVisible;
            grBxLines.Visible = isVisible;
        }


        #endregion

        private void ShowDataFromStorage()
        {
            _processWaitingManager = new ProcessWaitingManager
            {
                ShowProgressBar = false,
                ShowCancelButton = false
            };
            
            _processWaitingManager.ShowProcessWaitingForm();
            
            var isStartUpDone = _blManager.IsStartUpDone;
            //var lst = GlobalData.RouteModel.RouteLineList;//  TODO: need to be changed to bndSourceRouteLine.DataSource as List<RouteLine>; for selecting only selected rows. 
            var lst = bndSourceRouteLine.DataSource as List<RouteLine>;
            _blManager.BuildModelData(lst);
            ShowData(lst);
            if (isStartUpDone)
                SetErrorTotatls();
            if (!_blManager.IsEgedOperator)
            {
                SetGraphVisibility(isStartUpDone);
            }
            else
            {
                SetGraphVisibility(false);
                gbShowStation.Visible = false;
                chLabelRouteStops.Checked = false;
                chLabelRouteStops.Visible = false;
            }
            //btnRouteLineSearchClear_Click(null, null); // TODO: need to be deleted for selecting only selected rows.

            _processWaitingManager.CloseProcessWaitingForm();
            SelectFirstRow(dtgRouteSystem);
            SelectFirstRow(dtgRouteStops);
            if (isStartUpDone)
            {
                var lstLinesNotBelongToOperator = _blManager.LinesNotBelongToOperator(lst,GlobalData.LoginUser.UserOperator);
                if (lstLinesNotBelongToOperator.IsListFull())
                {
                    var linesByComma = BLSharedUtils.GetLineNamesSeparatedBy(lstLinesNotBelongToOperator,","); 
                    GuiHelper.ShowErrorMessage(string.Format(ResourceGUI.LinesNotBelongToOperator,GlobalData.LoginUser.UserOperator.OperatorName,linesByComma));
                    return; 
                }
            }
            _submExportLinesToLicensingSystem.Visible = isStartUpDone && _blManager.IsValidDataModel;
            _submExportBaseDasaToLicensing.Visible = _blManager.IsExportBaseEnabled;
            btnCalcFirstStationForAllRoutes.Visible = _blManager.IsDanLikeOperators;
            //טרנסקאד – מפת תח"צ". {0} משתמש {1} , מפעיל {2}, אשקול {3} 
            
            if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm)
                Text = string.Format(ResourceGUI.MainFormCaption, ResourceGUI.PlanningFirm,GlobalData.LoginUser.UserName, GlobalData.LoginUser.UserOperator.OperatorName, _blManager.GetSelectedClusterName());
            else
                Text = string.Format(ResourceGUI.MainFormCaption, string.Empty, GlobalData.LoginUser.UserName, GlobalData.LoginUser.UserOperator.OperatorName, _blManager.GetSelectedClusterName());
            dtgRouteSystem.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Descending;

            

        }
        /// <summary>
        /// SelectFirstRow
        /// </summary>
        /// <param name="dg"></param>
        private static void SelectFirstRow(DataGridView dg)
        {
            if (dg.Rows.IsListFull())
            {
                dg.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// RouteExportProccess Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteExportProccess_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _blManager = new TransCadBlManager();
                // Set Login User
                if (_blManager.SetLoginUserInfoAndVerifyToShowSelectOpersList())
                {
                    var opList = new FrmOperatorList();
                    if (opList.ShowDialog() == DialogResult.Ignore)
                    {
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    var operatorSelectBlManager = new OperatorSelectBlManager(GlobalData.LoginUser.UserOperator );
                    if (GlobalData.LoginUser.UserOperator.TransCadStatus == 0)
                    {
                        GuiHelper.ShowErrorMessage(ResourceGUI.OperatorWasNotConverted);
                        Application.Exit();  
                    }
                    var message = string.Empty;
                    if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig == null)
                    {
                        var messageException = string.Format("The {0} operator doesn't have SelectedTranscadClusterConfig.",GlobalData.LoginUser.UserOperator.OperatorName);
                        Logger.LoggerManager.WriteToLog(messageException);
                        throw new ApplicationException(messageException);
                    }

                    //if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig == null)
                    //{
                    //    var isFull = operatorSelectBlManager.SetSelectedTranscadClusterConfigAndTestClustersByOperatorIsListFull(GlobalData.LoginUser.UserOperator);
                    //    if (isFull)
                    //    {
                    //        var opList = new frmOperatorList();
                    //        if (opList.ShowDialog() == DialogResult.Ignore)
                    //        {
                    //            Application.Exit();
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {


                    //        if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig == null)
                    //        {
                    //            string messageException = string.Format("The {0} operator doesn't have SelectedTranscadClusterConfig. User defined cluster count list is full - {1}",GlobalData.LoginUser.UserOperator.OperatorName, isFull);
                    //            Logger.LoggerManager.WriteToLog(messageException);
                    //            throw new ApplicationException(messageException);
                    //        }
                    //    }
                    //}
                    if (!operatorSelectBlManager.OpenTranscadByWsPath(GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PathToRSTWorkSpace, ref message))
                    {
                        GuiHelper.ShowErrorMessage(message);
                        Application.Exit();
                    }
                }

                SetManagerControlVisibility();
                var errorData = string.Empty;
                if (!_blManager.IsEgedOperator)
                {
                    if (!_blManager.IsValidTransCadEnvironment(ref errorData))
                    {
                        GuiHelper.ShowErrorMessage(errorData);
                        Application.Exit();
                    }
                }
                else
                {
                    _blManager = new EgedBlManager();
                }
                ShowDataFromStorage();

                BuildMenuMain();

                Focus();

                RunTimers();

            }
            catch (Exception exp)
            {
                Logger.LoggerManager.WriteToLog(exp);
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void RunTimers()
        {
            if (ExportConfigurator.GetConfig().TranscadConnectivityTimerPeriod == 0) return;
 
           _transcadActivity = new System.Threading.Timer((obj)=>
              {
                  if (!BLSharedUtils.IsTranscadActive())
                  {
                     Application.Exit();
                  }
              }, null, 60000, ExportConfigurator.GetConfig().TranscadConnectivityTimerPeriod);
        }


        /// <summary>
        /// Set Manager Control Visibility
        /// </summary>
        private void SetManagerControlVisibility()
        {
            var isVisible = _presentation.IsVisibleManagerPart;
            grBxPhysicalStation.Visible = isVisible;
            if (!isVisible)
                tbRouteData.TabPages.RemoveAt(tbRouteData.TabPages.Count - 1);
        }

        /// <summary>
        /// Create Chart
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="fixCount"></param>
        /// <param name="notFixCount"></param>
        /// <param name="newRecords"></param>
        private static void CreateChart(Chart chart, int fixCount, int notFixCount, int newRecords)
        {
            const string defaultSeria = "defaultSeria";
            // string defaultChartArea = "ChartArea1";
            chart.Series.Clear();

            chart.Series.Add(defaultSeria);
            double[] yValues = { fixCount, notFixCount, newRecords };
            string[] xValues = { Resources.ResourceGUI.Fix, Resources.ResourceGUI.NOT_Fix, Resources.ResourceGUI.NewRecordsCount };
            chart.Series[defaultSeria].Points.DataBindXY(xValues, yValues);

            chart.Series[defaultSeria]["PieLabelStyle"] = "Inside";
            //Set the chart 
            chart.Series[defaultSeria].ChartType = SeriesChartType.Column;
            // Set the bar width
            chart.Series[defaultSeria]["PointWidth"] = "0.5";
            // Show data points labels
            chart.Series[defaultSeria].IsValueShownAsLabel = false;
            chart.Series[defaultSeria].ToolTip = Resources.ResourceGUI.DoubleClickToSeeReport;
            chart.Series[defaultSeria].IsVisibleInLegend = false;
            // Set data points label style
            chart.Series[defaultSeria]["BarLabelStyle"] = "Left";

            // Show chart as 3D
            //chart.ChartAreas[defaultChartArea].Area3DStyle.Enable3D = true;
            // Draw chart as 3D Cylinder
            chart.Series[defaultSeria]["DrawingStyle"] = "Cylinder";

            chart.Series[defaultSeria].LegendToolTip = Resources.ResourceGUI.DoubleClickToSeeReport;
            chart.Series[defaultSeria].LabelToolTip = Resources.ResourceGUI.DoubleClickToSeeReport;

        }



        /// <summary>
        /// SetTotatls
        /// </summary>
        private void SetErrorTotatls()
        {
            if (GlobalData.RouteModel != null)
            {
                CreateChart(chartSystemLines, _blManager.ValidListObjectCount<RouteLine>(GlobalData.RouteModel.RouteLineList),
                    _blManager.InValidListObjectCount<RouteLine>(GlobalData.RouteModel.RouteLineList),
                     _blManager.NewListObjectCount<RouteLine>(GlobalData.RouteModel.RouteLineList)

                    );

                CreateChart(chartSystemStops, _blManager.ValidListObjectCount<RouteStop>(GlobalData.RouteModel.RouteStopList),
                    _blManager.InValidListObjectCount<RouteStop>(GlobalData.RouteModel.RouteStopList),
                    _blManager.NewListObjectCount<RouteStop>(GlobalData.RouteModel.RouteStopList)
                    );

                CreateChart(chartPysicalStops, _blManager.ValidListObjectCount<PhysicalStop>(GlobalData.RouteModel.PhysicalStopList),
                    _blManager.InValidListObjectCount<PhysicalStop>(GlobalData.RouteModel.PhysicalStopList),
                    _blManager.NewListObjectCount<PhysicalStop>(GlobalData.RouteModel.PhysicalStopList)
                    );

            }
        }
        /// <summary>
        /// ShowData
        /// </summary>
        private void ShowData(List<RouteLine> selectedRows)
        {
            if (_blManager.IsEgedOperator)
            {
                chBoxRouteSearchIsNewRoutes.Visible = false;
                cbRouteLinesErrorsOnly.Visible = false;
                btnCalcFirstStationForAllRoutes.Visible = false;
                cbRouteStopsErrorsOnly.Visible = false;
                cbLabeling.Text = ResourceGUI.EgedShowRouteGISInfo;
            }

            bndSourceRouteLine.DataSource = selectedRows ?? GlobalData.RouteModel.RouteLineList;
            PaintErrorEntities(selectedRows ?? GlobalData.RouteModel.RouteLineList, dtgRouteSystem);
            bndSourceRouteStops.DataSource = GlobalData.RouteModel.RouteStopList;
            PaintErrorEntities(selectedRows ?? GlobalData.RouteModel.RouteLineList, dtgRouteSystem);
            StopsSelectColumnConfiguration();
            bndSourcePhysicalStops.DataSource = GlobalData.RouteModel.PhysicalStopList;
            PaintErrorEntities(GlobalData.RouteModel.PhysicalStopList, dtgRouteSystem);

            cmbRouteLinesField.DataSource = _presentation.GetRouteLinesFields();

        }
        /// <summary>
        /// Stops Select Column Configuration
        /// </summary>
        private void StopsSelectColumnConfiguration()
        {
            dtgRouteStops.Columns[dtgRouteStops.ColumnCount - 1].HeaderCell = new DGVColumnHeaderRouteStop { Caption = "סמן", SelectedColumnIndex = dtgRouteStops.ColumnCount - 1 };
            (dtgRouteStops.Columns[dtgRouteStops.ColumnCount - 1].HeaderCell as DGVColumnHeaderRouteStop).IsSelectabeFunction = _dataSearchAndManipulateBlManager.IsSelectableRouteStop;
            cmbStationType.DisplayMember = "Name";
            cmbStationType.ValueMember = "ID";
            cmbStationType.DataSource = _dataSearchAndManipulateBlManager.GetStationType();
            //UnselectAllStops(this.dtgRouteStops, this.dtgRouteStops.ColumnCount - 1);
        }
        /// <summary>
        /// Paint Erro Entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="dtv"></param>
        private void PaintErrorEntities<T>(List<T> lst, DataGridView dtv) where T : BaseClass
        {

            foreach (DataGridViewRow row in dtv.Rows)
            {
                if (row != null && row.DataBoundItem != null)
                {
                    var data = row.DataBoundItem as BaseClass;
                    PaintEntity(data, row);
                }
            }
            UpdateAccomulationInfo();
        }
        /// <summary>
        /// Paint Error Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="row"></param>
        private static void PaintEntity<T>(T data, DataGridViewRow row) where T : BaseClass
        {
            try
            {
                if (row.DataBoundItem == null)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            if (data == null || row == null || row.DataBoundItem == null)
                return;
            Color foreColor = Color.Black;
            if (data.BaseClassStyle != null && !string.IsNullOrEmpty(data.BaseClassStyle.ForeColor))
            {
                foreColor = Color.FromName(data.BaseClassStyle.ForeColor);
            }
            if (data.IsNewEntity)
            {
                PaintCells(row, Color.Yellow, foreColor);

            }
            else if (data.ValidationErrors.IsListFull())
            {
                PaintCells(row, Color.Red, foreColor);
            }
            else
            {
                PaintCells(row, Color.White, foreColor);
            }

        }

        /// <summary>
        /// Update Accomulation Info
        /// </summary>
        private void UpdateAccomulationInfo()
        {
            tbRouteLines.Text = string.Format(ResourceGUI.LinesDataAccomulation, _dataSearchAndManipulateBlManager.RouteLineCount);
            tbRouteStops.Text = string.Format(ResourceGUI.StopsDataAccomulation, _dataSearchAndManipulateBlManager.RouteStopsCount);
            tbPhysicalStops.Text = string.Format(ResourceGUI.PhisicalStopsDataAccomulation, _dataSearchAndManipulateBlManager.PhisicalStopsCount);
        }

       
        private static void PaintCells(DataGridViewRow row, Color backColor, Color foreColor)
        {
            if (row == null || row.DataBoundItem == null)
                return;

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style.BackColor = backColor;
                cell.Style.ForeColor = foreColor;
            }
        }


        /// <summary>
        /// Route Export Proccess Form Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteExportProccess_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// bndSourceRouteLine Current Item Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BndSourceRouteLineCurrentItemChanged(object sender, EventArgs e)
        {
            try
            {

                if (bndSourceRouteLine.Current != null)
                {
                    var routeLine = (bndSourceRouteLine.Current as RouteLine);

                    if (routeLine != null)
                    {
                        txtDuplicated.Text = routeLine.IdDuplicatedSourceID > 0 ? string.Format(Resources.ResourceGUI.DuplicatedLine, routeLine.ID, routeLine.Name) : string.Empty;
                        if (routeLine.ValidationErrors.IsListFull())
                        {
                            lstBoxRouteLineErrors.DataSource = ((BaseClass) bndSourceRouteLine.Current).ValidationErrors;

                            if (((RouteLine) bndSourceRouteLine.Current).IsNewEntity)
                            {
                                var boldFont = new Font(txtlblErrorDataRouteLine.Font, FontStyle.Bold);
                                txtlblErrorDataRouteLine.Font = boldFont;
                            }
                            else
                            {
                                var regularFont = new Font(txtlblErrorDataRouteLine.Font, FontStyle.Regular);
                                txtlblErrorDataRouteLine.Font = regularFont;
                            }
                            txtlblErrorDataRouteLine.Text = string.Format(ResourceGUI.ErrorRouteLine, (bndSourceRouteLine.Current as RouteLine).Name, (bndSourceRouteLine.Current as BaseClass).ValidationErrors.Count);
                            return;
                        }
                    }
                }
                lstBoxRouteLineErrors.DataSource = null;
                if (bndSourceRouteLine.Current != null)
                    txtlblErrorDataRouteLine.Text = string.Format(Resources.ResourceGUI.GoodRouteLine, ((RouteLine) bndSourceRouteLine.Current).Name);
            }
            catch
            {
                TxtRouteLineFastSearchTextChanged(null, null);
            }

        }


        /// <summary>
        /// txtRouteLineFastSearch TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtRouteLineFastSearchTextChanged(object sender, EventArgs e)
        {
            if (bndSourceRouteLine.DataSource != null)
            {
                bndSourceRouteLine.DataSource = _dataSearchAndManipulateBlManager.GetSearchedSortedRouteLines(txtRouteNumber.Text, txtDirection.Text, txtValiant.Text, cbRouteLinesErrorsOnly.Checked
                , txtSearchCatalog.Text, txtSearchCluster.Text, chBoxRouteSearchIsNewRoutes.Checked);
                _submExportLinesToLicensingSystem.Visible = false;
                PaintErrorEntities(bndSourceRouteLine.DataSource as List<BaseClass>, dtgRouteSystem);
            }
        }
        /// <summary>
        /// btn Route LineSearch Clear Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRouteLineSearchClearClick(object sender, EventArgs e)
        {
            txtRouteNumber.Text = string.Empty;
            txtDirection.Text = string.Empty;
            txtValiant.Text = string.Empty;
            txtSearchCatalog.Text = string.Empty;
            txtSearchCluster.Text = string.Empty;
            cbRouteLinesErrorsOnly.Checked = false;
            chBoxRouteSearchIsNewRoutes.Checked = false;
        }
        /// <summary>
        /// Physical Stops Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhysicalStops_DoubleClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// chartSystemStops DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartSystemStopsDoubleClick(object sender, EventArgs e)
        {
            var reportInfoBlManager = new ReportInfoBLManager();
            if (GlobalData.RouteModel != null && !_blManager.IsValidList(GlobalData.RouteModel.RouteStopList))
            {
                var errorReport = new frmReportErrorRouteStopResults(ResourceGUI.ErrorStopsReportCaption, reportInfoBlManager.GetRouteStopsErrorData(GlobalData.RouteModel.RouteStopList));
                errorReport.Show();
            }
            else
            {
                GuiHelper.ShowInfoMessage(Resources.ResourceGUI.NOT_ERROR_LINES_STOP);
            }
        }
        /// <summary>
        /// chart System Lines Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartSystemLinesDoubleClick(object sender, EventArgs e)
        {
            IReportInfoBLManager reportInfoBlManager = new ReportInfoBLManager();
            if (GlobalData.RouteModel != null && !_blManager.IsValidList(GlobalData.RouteModel.RouteLineList))
            {
                var errorReport = new frmReportErrorRouteLineResults(ResourceGUI.ErrorLinesReportCaption, reportInfoBlManager.GetRouteLinesErrorData(GlobalData.RouteModel.RouteLineList));
                errorReport.Show();
            }
            else
            {
                GuiHelper.ShowInfoMessage(ResourceGUI.NOT_ERROR_LINES);
            }

        }



        /// <summary>
        /// dtgRouteSystem Mouse Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtgRouteSystemMouseDown(object sender, MouseEventArgs e)
        {

            var hti = dtgRouteSystem.HitTest(e.X, e.Y);
            if (hti.RowIndex == -1)
            {
                if (hti.ColumnIndex >= 0)
                {
                    _dataSearchAndManipulateBlManager.CurrentRouteLineSortedProperty = dtgRouteSystem.Columns[hti.ColumnIndex].DataPropertyName;
                    TxtRouteLineFastSearchTextChanged(null, null);
                    ChangeSortDirectionIcon(routeLineSortInfo, hti.ColumnIndex, dtgRouteSystem);
                }

            }
            else
            {
                if (hti.ColumnIndex > -1 && hti.ColumnIndex > 1)
                {
                    if (hti.RowIndex > -1)
                    {
                        dtgRouteSystem.Rows[hti.RowIndex].Selected = true;
                        bndSourceRouteLine.Position = hti.RowIndex;
                        dtgRouteSystem.ContextMenuStrip = this.menuStripRouteLines;
                    }
                }
            }


        }
        /// <summary>
        /// mnRouteLineDetails Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnRouteLineDetailsClick(object sender, EventArgs e)
        {
            if (bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null)
            {
                var obj = bndSourceRouteLine.Current as RouteLine;
                var lineDetails = new frmLineDetails(Resources.ResourceGUI.LineDetailCaption, obj, bndSourceRouteLine.DataSource as List<RouteLine>);
                lineDetails.ShowDialog();
                PaintErrorEntities(GlobalData.RouteModel.RouteLineList, dtgRouteSystem);
                //bndSourceRouteLine.ResetCurrentItem();
                //bndSourceRouteLine.ResetBindings(false); 
            }
        }


        /// <summary>
        /// mnu Zoom Current Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuZoomCurrentClick(object sender, EventArgs e)
        {
            if (bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null)
            {
                var lst = new List<RouteLine> { bndSourceRouteLine.Current as RouteLine };
                _blManager.ZoomToRouteLinesLayerFeutureById(lst);
            }
        }
        /// <summary>
        /// mnu ZoomAll Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuZoomAllClick(object sender, EventArgs e)
        {
            if (bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null)
            {
                var lst = bndSourceRouteLine.DataSource as List<RouteLine>;
                if (lst.IsListFull() && lst.Count > 100)
                {
                    GuiHelper.ShowErrorMessage(ResourceGUI.YouCantZoomToMoreThen100Lines);
                    return;
                }

                _blManager.ZoomToRouteLinesLayerFeutureById(lst);
            }
        }
        /// <summary>
        /// cmbLineTypes Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbLineTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            TxtRouteLineFastSearchTextChanged(null, null);
        }
        /// <summary>
        /// mnu Route Stops Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuRouteStopsClick(object sender, EventArgs e)
        {
            if (bndSourceRouteLine.DataSource != null && bndSourceRouteLine.Current != null)
            {
                var rl = bndSourceRouteLine.Current as RouteLine;
                if (BLSharedUtils.IsLineHasStation(rl))
                {
                    BtnRouteStopSearchClearClick(null, null);
                    tbRouteData.SelectedIndex = 1;
                    PassRouteLineDataToRouteStopTab(rl);
                    bndSourceRouteStops.DataSource = BLSharedUtils.GetListRouteStopsByIdRouteLine(rl, _blManager.IsEgedOperator);
                    if (bndSourceRouteStops.DataSource != null)
                        PaintErrorEntities(bndSourceRouteStops.DataSource as List<BaseClass>, dtgRouteStops);
                }
                else
                {
                    GuiHelper.ShowInfoMessage(Resources.ResourceGUI.StationNoneForTheLine);
                }
            }
        }
        /// <summary>
        /// Pass Route Line Data To Route Stop Tab
        /// </summary>
        /// <param name="rl"></param>
        private void PassRouteLineDataToRouteStopTab(RouteLine rl)
        {

            if (rl != null)
            {
                _dataSearchAndManipulateBlManager.IDRouteLineForStationsQuery = rl.ID;
                txtRouteStopSearchCatalog.Text = rl.Catalog == null ? string.Empty : rl.Catalog.ToString();
                txtRouteStopSearchRouteNumber.Text = rl.RouteNumber == null ? string.Empty : rl.RouteNumber.ToString();
                txtRouteStopSearchDirection.Text = rl.Dir == null ? string.Empty : rl.Dir.ToString();
                txtRouteStopSearchVariant.Text = rl.Var ?? string.Empty;
            }
            else
            {
                _dataSearchAndManipulateBlManager.IDRouteLineForStationsQuery = null;
            }
        }

        /// <summary>
        /// dtgRouteSystem UserDeletingRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtgRouteSystemUserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!e.Row.IsNewRow && bndSourceRouteLine.Current != null)
            {
                var deletingObject = bndSourceRouteLine.Current as RouteLine;
                if (deletingObject != null)
                {
                    var name = deletingObject.Name;
                    var response = MessageBox.Show(string.Format(ResourceGUI.DeleteRouteLineConfirmation, deletingObject.Name),
                                                            GuiHelper.Caption, MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Question,
                                                            MessageBoxDefaultButton.Button2);
                    if (response == DialogResult.No)
                        e.Cancel = true;
                    else
                    {
                        var deletedItem = bndSourceRouteLine.Current as RouteLine;
                        if (_blManager.DeleteRouteLine(deletedItem))
                        {
                            bndSourceRouteLine.ResumeBinding();
                            if (bndSourceRouteLine.Count > 0)
                            {
                                bndSourceRouteLine.Position = 0;
                                bndSourceRouteLine.ResetItem(0);
                            }
                            GuiHelper.ShowInfoMessage(string.Format(ResourceGUI.DeleteRouteLineSuccessInfo, name));
                            UpdateAccomulationInfo();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// dtgRouteStops MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtgRouteStopsMouseDown(object sender, MouseEventArgs e)
        {
            var hti = dtgRouteStops.HitTest(e.X, e.Y);

            if (hti.ColumnIndex > -1 && hti.ColumnIndex > 1)
            {
                if (hti.RowIndex >= 0)
                {
                    dtgRouteStops.Rows[hti.RowIndex].Selected = true;
                    bndSourceRouteStops.Position = hti.RowIndex;
                    this.dtgRouteStops.ContextMenuStrip = mnuRouteStopsData;
                }
                if (hti.ColumnIndex == dtgRouteStops.ColumnCount - 1 && bndSourceRouteStops.Current != null && hti.RowIndex >= 0)
                {
                    if (!_dataSearchAndManipulateBlManager.IsSelectableRouteStop(bndSourceRouteStops.Current as RouteStop))
                    {
                        ((dtgRouteStops.Rows[hti.RowIndex].Cells[hti.ColumnIndex]) as DataGridViewCheckBoxCell).ReadOnly = true;
                        (bndSourceRouteStops.Current as RouteStop).IsSelected = false;
                    }
                    else
                        (bndSourceRouteStops.Current as RouteStop).IsSelected = !(bndSourceRouteStops.Current as RouteStop).IsSelected;
                    dtgRouteStops.Refresh();
                }
            }

        }
        /// <summary>
        /// dtg Route Stops Use Deleting Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgRouteStops_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (_blManager.IsEgedOperator)
                GuiHelper.ShowInfoMessage(ResourceGUI.EgedDeleteStationIsProhibited);
            else if (!e.Row.IsNewRow && bndSourceRouteLine.Current != null)
            {
                var deletingObject = bndSourceRouteStops.Current as RouteStop;
                var response = MessageBox.Show(string.Format(Resources.ResourceGUI.DeleteRouteStopConfirmation,
                    deletingObject.Name, deletingObject.RouteLine == null ? string.Empty : deletingObject.RouteLine.Name
                    ),
                    GuiHelper.Caption, MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question,
                                     MessageBoxDefaultButton.Button2);
                if (response == DialogResult.No)
                    e.Cancel = true;
                else
                    if (_blManager.DeleteRouteStop(bndSourceRouteStops.Current as RouteStop))
                    {
                        bndSourceRouteStops.ResetBindings(false);
                        bndSourceRouteStops.ResetCurrentItem();
                        GuiHelper.ShowInfoMessage(string.Format(Resources.ResourceGUI.DeleteRouteStopSuccessInfo, deletingObject.Name));
                        UpdateAccomulationInfo();
                    }
            }
        }

        /// <summary>
        /// tbRouteStopZoomCurrent Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbRouteStopZoomCurrentClick(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.DataSource != null && bndSourceRouteStops.Current != null)
            {
                var lst = new List<RouteStop> { bndSourceRouteStops.Current as RouteStop };
                _blManager.ZoomToRouteStopsLayerFeutureById(lst);
                _isRouteStopZoomWasDone = true;
            }
        }
        /// <summary>
        /// toolBarMenuRouteStopDetails Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBarMenuRouteStopDetailsClick(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.DataSource != null && bndSourceRouteStops.Current != null)
            {
                var obj = bndSourceRouteStops.Current as RouteStop;
                if (obj != null)
                {
                    var routeStopDetails = new frmRouteStopDetail(Resources.ResourceGUI.RouteStopDetailCaption, obj, bndSourceRouteStops.DataSource as List<RouteStop>);
                    routeStopDetails.ShowDialog();
                    bndSourceRouteStops.ResetBindings(false);
                }
            }
        }
        /// <summary>
        /// tbRouteStopZoomGroup Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRouteStopZoomGroup_Click(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.DataSource != null && bndSourceRouteStops.Current != null)
            {
                List<RouteStop> lst = bndSourceRouteStops.DataSource as List<RouteStop>;

                if (lst.IsListFull() && lst.Count > 100)
                {
                    GUIBase.GuiHelper.ShowErrorMessage(ResourceGUI.YouCantZoomToMoreThen100Lines);
                    return;
                }

                _blManager.ZoomToRouteStopsLayerFeutureById(lst);
                _isRouteStopZoomWasDone = true;
            }




        }
        /// <summary>
        /// cbLabeling CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbLabelingCheckedChanged(object sender, EventArgs e)
        {
            if (cmbRouteLinesField.DataSource == null || cmbRouteLinesField.SelectedIndex < 0) return;
            var field = cmbRouteLinesField.SelectedItem as TransCadField;
            if (field != null) _blManager.SetLabelRouteLines(cbLabeling.Checked, field.EnglishName);
        }
        /// <summary>
        /// cmbRouteLinesField SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbRouteLinesFieldSelectedIndexChanged(object sender, EventArgs e)
        {
            CbLabelingCheckedChanged(null, null);
        }
        /// <summary>
        /// btn Refresh Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefreshClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var errorData = string.Empty;
                _blManager.IsValidTransCadEnvironment(ref errorData);
                ShowDataFromStorage();
                BtnRouteStopSearchClearClick(null, null);
            }
            finally
            {
                Cursor = Cursors.Default;
                Activate();
            }
        }

        private string BuildErrorValidExportDateMessage(List<RouteLine> lst)
        {
            var sb = new StringBuilder();
            lst.ForEach(li=>sb.AppendLine(string.Format(ResourceGUI.LineErrorValidExportDate,li,li.ValidExportDate)));
            return sb.ToString();
        }

        /// <summary>
        /// btnCreateTextFiles Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateTextFilesClick(object sender, EventArgs e)
        {
            IExportRouteBLManager exportTextFileBlManager;
            var errorString = string.Empty;
            if (!GuiHelper.ShowConfirmationMessage(ResourceGUI.ExportQuestions))
                return;
            try
            {
                // Get Selected list
                var lst = bndSourceRouteLine.DataSource as List<RouteLine>;
                if (!lst.IsListFull())
                {
                    GuiHelper.ShowErrorMessage(ResourceGUI.RouteLineListEmpty);
                    return;
                }
                if (_blManager.IsEgedOperator)
                    exportTextFileBlManager = new EgedExportDataSQLBLManager(lst);
                else
                {
                    if (lst.IsListFull())
                    {
                        var blClusterValidExportDateManage = new BlClusterValidExportDateManage();
                        var reportResults = blClusterValidExportDateManage.GetExportValidationDateReport(lst);
                        if (reportResults.IsListFull())
                        {
                            var dr = new frmExportValidateDateOptions { IsInsideExportProcess = true, Routes = lst, ReportResults = reportResults }.ShowDialog();
                            if (dr!= DialogResult.Yes)
                                return;
                            var fixedList = blClusterValidExportDateManage.GetAllowedRoutesBasedOnExportValidatioDate(lst);
                            if (!fixedList.IsListFull())
                            {
                                GuiHelper.ShowErrorMessage(ResourceGUI.ExportToRishuiLocked);
                                return; 
                            }
                            exportTextFileBlManager = new ExportDataSqlblManager(fixedList);
                        }
                        else
                        {
                            exportTextFileBlManager = new ExportDataSqlblManager(lst);    
                        }
                        
                    }
                    else
                    {
                        return; 
                    }
                }
                if (!exportTextFileBlManager.IsImportManagerFilled(ref errorString))
                    GuiHelper.ShowErrorMessage(errorString);
                else
                {
                    Refresh();
                    _processWaitingManager.CloseProcessWaitingForm();
                    _processWaitingManager = new ProcessWaitingManager
                    {
                        ShowProgressBar = true,
                        ProgressBarInitValue = 5,
                        ProgressBarMaxValue = 10,
                        ProgressBarMessage = ResourceGUI.SystemTask,
                        ShowCancelButton = true    
                    };
                    _processWaitingManager.ShowProcessWaitingForm();
                    
                    exportTextFileBlManager.Changed += (o, eParams) =>
                    {
                        exportTextFileBlManager.IsCanceledByUser = _processWaitingManager.IsCanceledByUser;
                        Refresh();
                        if (!exportTextFileBlManager.IsCanceledByUser)
                        {
                            _processWaitingManager.ChangeProgressBar(eParams.ProgressBarValue,eParams.MaxProgressBarValue, eParams.Message);
                        }
                    };

                    if (!exportTextFileBlManager.ExportData(ref errorString))
                    {
                        _processWaitingManager.CloseProcessWaitingForm();
                        GuiHelper.ShowErrorMessage(errorString);
                    }
                    else
                    {
                        _processWaitingManager.CloseProcessWaitingForm();
                        GuiHelper.ShowInfoMessage(ResourceGUI.ExportProcessWasSuccesefulNonQuestion);
                        _submExportLinesToLicensingSystem.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                _processWaitingManager.CloseProcessWaitingForm();
                throw;
            }


        }
        
        /// <summary>
        /// btnBasedTable Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBasedTable_Click(object sender, EventArgs e)
        {
            frmBaseTableManager bsm = new frmBaseTableManager();
            bsm.ShowDialog();
        }

        /// <summary>
        /// chLabelRouteStops CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chLabelRouteStops_CheckedChanged(object sender, EventArgs e)
        {
            if (chLabelRouteStops.Checked)
            {
                picBox0.Image = imageCustomList.Images[0];
                picBox1.Image = imageCustomList.Images[1];
                picBox2.Image = imageCustomList.Images[2];
                picBox3.Image = imageCustomList.Images[3];
                picBox4.Image = imageCustomList.Images[4];
                gbShowStation.Enabled = true;
            }
            else
            {
                picBox0.Image = null;
                picBox1.Image = null;
                picBox2.Image = null;
                picBox3.Image = null;
                picBox4.Image = null;
                gbShowStation.Enabled = false;
            }
            //pictureBox1.Image = imageList1.Images[currentImage];
            if (!_isRouteStopZoomWasDone)
                TbRouteStopZoomCurrentClick(null, null);
            _blManager.SetLabelRouteStops(chLabelRouteStops.Checked, rdShowByNameStation.Checked, Globals.SelectedImageStopLabelIndex);


        }
        /// <summary>
        /// Route Stop List Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteStopList_TextChanged(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.DataSource != null)
            {
                //txtRouteStopSearchCatalogStation.Text 
                bndSourceRouteStops.DataSource = _dataSearchAndManipulateBlManager.GetSearchedRouteStops
                    (txtRouteStopSearchRouteNumber.Text, txtRouteStopSearchDirection.Text,
                    txtRouteStopSearchVariant.Text, cbRouteStopsErrorsOnly.Checked
                , txtSearchStationName.Text, txtRouteStopSearchCatalog.Text, txtRouteStopSearchCatalogStation.Text);
                // Commented on 18/09/2013
                //PaintErrorEntities(bndSourceRouteStops.DataSource as List<BaseClass>, dtgRouteStops);
            }
        }
        /// <summary>
        /// btnRouteStopSearchClear Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRouteStopSearchClearClick(object sender, EventArgs e)
        {
            txtRouteStopSearchRouteNumber.Text = string.Empty;
            txtRouteStopSearchDirection.Text = string.Empty;
            txtRouteStopSearchVariant.Text = string.Empty;
            cbRouteStopsErrorsOnly.Checked = false;
            txtSearchStationName.Text = string.Empty;
            txtRouteStopSearchCatalog.Text = string.Empty;
            txtRouteStopSearchCatalogStation.Text = string.Empty;
            _dataSearchAndManipulateBlManager.IDRouteLineForStationsQuery = null;
        }
        /// <summary>
        /// bndSourceRouteStops CurrentItemChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BndSourceRouteStopsCurrentItemChanged(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.Current != null)
            {
                var routeStop = (bndSourceRouteStops.Current as RouteStop);
                if (routeStop != null)
                {
                    routeStop.Validate();
                    if (routeStop.ValidationErrors.IsListFull())
                    {
                        lstBoxRouteSopsErrors.DataSource = (bndSourceRouteStops.Current as BaseClass).ValidationErrors;
                        txtlblErrorDataRouteStop.Text = string.Format(ResourceGUI.ErrorRouteStop,
                                                                      (bndSourceRouteStops.Current as RouteStop).Name,
                                                                      (bndSourceRouteStops.Current as BaseClass).ValidationErrors.Count);
                        return;
                    }
                }
            }
            lstBoxRouteSopsErrors.DataSource = null;
            if (bndSourceRouteStops.Current != null)
                txtlblErrorDataRouteStop.Text = string.Format(ResourceGUI.GoodRouteStop,
                    (bndSourceRouteStops.Current as RouteStop).Name);

        }



        /// <summary>
        /// picBox Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox pb = sender as PictureBox;
                Globals.SelectedImageStopLabelIndex = int.Parse(pb.Name.GetLastChar());
                if (!_isRouteStopZoomWasDone)
                    TbRouteStopZoomCurrentClick(null, null);
                _blManager.SetLabelRouteStops(chLabelRouteStops.Checked, rdShowByNameStation.Checked, Globals.SelectedImageStopLabelIndex);
            }
        }
        /// <summary>
        /// btnCatalogRouteLineSearch Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCatalogRouteLineSearch_Click(object sender, EventArgs e)
        {

            var searhcForm = new frmListSearchCatalog(_dataSearchAndManipulateBlManager.GetCatalogListEntities(), Resources.ResourceGUI.CatalogFilter, txtSearchCatalog.Text);
            if (searhcForm.ShowDialog() == DialogResult.OK)
            {
                txtSearchCatalog.Text = searhcForm.Results;
            }
        }

        /// <summary>
        /// btnRouteNumberSearch Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRouteNumberSearch_Click(object sender, EventArgs e)
        {
            frmListSearch searhcForm = new frmListSearch(_dataSearchAndManipulateBlManager.GetRouteNumberList(), Resources.ResourceGUI.RouteNumberFilter, txtRouteNumber.Text);
            if (searhcForm.ShowDialog() == DialogResult.OK)
            {
                txtRouteNumber.Text = searhcForm.Results;
            }
        }
        /// <summary>
        /// btnSearchStationCatalog Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchStationCatalog_Click(object sender, EventArgs e)
        {
            frmListSearch searhcForm = new frmListSearch(_dataSearchAndManipulateBlManager.GetStationCatalogList(), Resources.ResourceGUI.StationCatalogFilter, txtRouteStopSearchCatalogStation.Text);
            if (searhcForm.ShowDialog() == DialogResult.OK)
            {
                txtRouteStopSearchCatalogStation.Text = searhcForm.Results;
            }
        }
        /// <summary>
        /// btnSearchStationName Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchStationName_Click(object sender, EventArgs e)
        {
            frmListSearch searhcForm = new frmListSearch(_dataSearchAndManipulateBlManager.GetStationNameList(), Resources.ResourceGUI.StationNameFilter, txtSearchStationName.Text);
            if (searhcForm.ShowDialog() == DialogResult.OK)
            {
                txtSearchStationName.Text = searhcForm.Results;
            }
        }
        /// <summary>
        /// tbRouteData Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbRouteDataSelectedIndexChanged(object sender, EventArgs e)
        {

            if (tbRouteData.SelectedIndex == 1)
            {
                _isRouteStopZoomWasDone = false;
                bndSourceRouteStops.ResetBindings(false);
                if (bndSourceRouteStops.DataSource != null)
                    PaintErrorEntities(bndSourceRouteStops.DataSource as List<BaseClass>, dtgRouteStops);
            }
            else
            {
                if (bndSourceRouteLine.Current != null && dtgRouteSystem.SelectedRows.IsListFull())
                {
                    var rl = bndSourceRouteLine.Current as RouteLine;
                    if (rl != null) 
                        rl.Validate();
                    bndSourceRouteStops.ResetBindings(false);
                    BndSourceRouteLineCurrentItemChanged(null, null);
                    PaintEntity(bndSourceRouteLine.Current as RouteLine, dtgRouteSystem.SelectedRows[0]);
                    UpdateAccomulationInfo();
                }
            }
        }
        /// <summary>
        /// mnu Get Route Line Stops Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGetRouteLineStops_Click(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.Current != null)
            {
                RouteStop routeStop = bndSourceRouteStops.Current as RouteStop;
                if (routeStop.RouteLine != null)
                {
                    cbRouteStopsErrorsOnly.Checked = false;
                    PassRouteLineDataToRouteStopTab(routeStop.RouteLine);
                }
            }
        }
        /// <summary>
        /// menu Init Map Resize Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuInitMapResize_Click(object sender, EventArgs e)
        {
            _blManager.InitMapResize();
        }
        /// <summary>
        /// btn Export Base Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportBase_Click(object sender, EventArgs e)
        {
            IReportInfoBLManager reportInfoBlManager = new ReportInfoBLManager();
            _blManager.ValidatePhysicalStopsData();
            if (GlobalData.RouteModel != null && !_blManager.IsValidList(GlobalData.RouteModel.PhysicalStopList))
            {
                GuiHelper.ShowInfoMessage(ResourceGUI.ERROR_PHYSICAL_STOP_BEFORE_SQL_EXPORT);
                var errorReport = new frmReportErrorRouteLineResults(Resources.ResourceGUI.ErrorLinesReportCaption, reportInfoBlManager.GetPhysicalStopsErrorData(GlobalData.RouteModel.PhysicalStopList));
                errorReport.Show();
                return;
            }

            string errorString = string.Empty;
            if (!GuiHelper.ShowConfirmationMessage(ResourceGUI.ExportBaseDataQuestions))
                return;
            IExportBaseBlManager exportBaseBlManager = new ExportBaseToSQLBL();
           
            Refresh();
            _processWaitingManager = new ProcessWaitingManager { ShowProgressBar = true, ProgressBarInitValue = 3, ProgressBarMaxValue = 10};
            _processWaitingManager.ShowProcessWaitingForm();
            exportBaseBlManager.Changed += exportBaseBLManager_Changed;

            if (!exportBaseBlManager.ExportBaseData(ref errorString))
            {
                _processWaitingManager.CloseProcessWaitingForm();
                GuiHelper.ShowErrorMessage(errorString);
            }
            else
            {
                _processWaitingManager.CloseProcessWaitingForm();
                GuiHelper.ShowInfoMessage(ResourceGUI.ExportProcessWasSuccesefulSQL);
                //this.btnExportBase.Enabled = false;
                Activate();
                //Application.Exit(); 
            }
        }
        /// <summary>
        /// export Base BL Manager Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportBaseBLManager_Changed(object sender, ImportToSQLArgs e)
        {
            _processWaitingManager.ChangeProgressBar(e.ProgressBarValue, e.MaxProgressBarValue, e.Message);
        }


        private void chkPathName_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbRouteLinesField.DataSource != null && cmbRouteLinesField.SelectedIndex >= 0)
            {

                _blManager.ShowStreetName(chkPathName.Checked);
            }
        }
        /// <summary>
        /// chk Path Flow Checked Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPathFlow_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbRouteLinesField.DataSource != null && cmbRouteLinesField.SelectedIndex >= 0)
            {

                _blManager.ShowStreetFlow(chkPathFlow.Checked);
            }
        }
        /// <summary>
        /// btn Save Map As Image Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveMapAsImage_Click(object sender, EventArgs e)
        {
            const string transcadMap = "TRANSCADMAP";
            var sfDialog = new SaveFileDialog
                               {
                                   Title = Resources.ResourceGUI.SelectFileImageNameToSave
                               };
            string fileName;
            sfDialog.Filter = "Images files (*.jpg)|*.jpg";
            sfDialog.FilterIndex = 2;
            sfDialog.FileName = transcadMap;// StringHelper.ConvertToDefaultFrom1255(transcadBLManager.GetMapTitle());
            sfDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            sfDialog.RestoreDirectory = true;

            if (sfDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = sfDialog.FileName;
                try
                {
                    _blManager.SaveMapToImage(fileName);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// dtgRouteSystem DataError
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgRouteSystem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (bndSourceRouteLine.Count > 0)
            {
                bndSourceRouteLine.Position = 0;
                BndSourceRouteLineCurrentItemChanged(null, null);
                TxtRouteLineFastSearchTextChanged(null, null);
            }
        }
        /// <summary>
        /// btnLayersImp Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLayersImp_Click(object sender, EventArgs e)
        {
            var layerManagment = new frmLayerManagment();
            layerManagment.ShowDialog();
            _submExportBaseDasaToLicensing.Visible = _blManager.IsExportBaseEnabled;
        }


        /// <summary>
        /// frmRouteExportProccess Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRouteExportProccess_Shown(object sender, EventArgs e)
        {
            Focus();
            Activate();
        }

        /// <summary>
        /// btn Clusters Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClustersClick(object sender, EventArgs e)
        {

            var searhcForm = new frmListSearch(_dataSearchAndManipulateBlManager.GetClusterList(), Resources.ResourceGUI.ClustertFilter, txtSearchCluster.Text);
            if (searhcForm.ShowDialog() == DialogResult.OK)
            {
                txtSearchCluster.Text = searhcForm.Results;
            }
        }
        /// <summary>
        /// chart System Lines Mouse Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartSystemLines_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        /// <summary>
        /// chart System Lines Mouse Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartSystemLines_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }


        /// <summary>
        /// Selected Stops
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private List<RouteStop> SelectedStops(DataGridView grid, int column)
        {
            var results = new List<RouteStop>();
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (bool.Parse(grid.Rows[i].Cells[column].Value.ToString()))
                {
                    results.Add((grid.Rows[i].DataBoundItem as RouteStop));
                }
            }
            return results;
        }

        /// <summary>
        /// Un select All Stops
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column"></param>
        private void UnselectAllStops(DataGridView grid, int column)
        {
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (bool.Parse(grid.Rows[i].Cells[column].Value.ToString()))
                {
                    grid.Rows[i].Cells[column].Value = false;
                }
            }
        }
        /// <summary>
        /// btnStationTypeUpdate Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStationTypeUpdate_Click(object sender, EventArgs e)
        {
            BaseTableEntity stationType = null;
            if (cmbStationType.SelectedIndex >= 0)
            {
                stationType = cmbStationType.SelectedItem as BaseTableEntity;
                if (stationType.ID == 0)
                {
                    GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseStationType);
                    return;
                }
            }
            else
            {
                GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseStationType);
                return;
            }
            List<RouteStop> selectedStop = SelectedStops(this.dtgRouteStops, this.dtgRouteStops.ColumnCount - 1);
            if (selectedStop.IsListFull())
            {
                _dataSearchAndManipulateBlManager.UpdateBatchStationType(selectedStop, stationType.ID);
                //bndSourceRouteStops.ResetBindings(false);
                //bndSourceRouteStops.ResumeBinding();
                //RouteStopList_TextChanged(null, null);
                //this.dtgRouteStops.Refresh();
                UnselectAllStops(this.dtgRouteStops, this.dtgRouteStops.ColumnCount - 1);
                RouteStopList_TextChanged(null, null);

            }
            else
            {
                GuiHelper.ShowErrorMessage(Resources.ResourceGUI.SelectRowsForUpdate);
                return;
            }
        }
        /// <summary>
        /// rdShowByNameStation CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdShowByNameStation_CheckedChanged(object sender, EventArgs e)
        {
            chLabelRouteStops_CheckedChanged(null, null);
        }
        /// <summary>
        /// rdShowByCatalogStation CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdShowByCatalogStation_CheckedChanged(object sender, EventArgs e)
        {
            chLabelRouteStops_CheckedChanged(null, null);
        }


        private SortInfo routeLineSortInfo = new SortInfo();
        private SortInfo routeStopSortInfo = new SortInfo();
        private void ChangeSortDirectionIcon(SortInfo routeLineSortInfo, int columnIndex, DataGridView grid)
        {
            if (routeLineSortInfo.LastColumnIndex == columnIndex)
            {
                if (routeLineSortInfo.LastSortOrder == SortOrder.Ascending)
                {
                    grid.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    routeLineSortInfo.LastSortOrder = SortOrder.Descending;
                }
                else
                {
                    grid.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    routeLineSortInfo.LastSortOrder = SortOrder.Ascending;
                }
            }
            else
            {
                grid.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                routeLineSortInfo.LastSortOrder = SortOrder.Ascending;
                routeLineSortInfo.LastColumnIndex = columnIndex;
            }
            grid.Refresh();
        }

        /// <summary>
        /// btn Excel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void BtnExcelClick(object sender, EventArgs e)
        //{
        //    string folderName;
        //    var fbd = new FolderBrowserDialog
        //                  {
        //                      ShowNewFolderButton = true,
        //                      RootFolder = Environment.SpecialFolder.Desktop,
        //                      Description = ResourceGUI.ChooseExportToExcelFolder
        //                  };
        //    if (fbd.ShowDialog() == DialogResult.OK)
        //        folderName = fbd.SelectedPath;
        //    else
        //        return;
        //    try
        //    {
        //        processWaitingManager.ShowProcessWaitingForm();

        //        if (tbRouteData.SelectedTab.Name.Equals(tbRouteLines.Name))
        //        {
        //            dataSearchAndManipulateBLManager.ExportRouteLinesToExcel(folderName, bndSourceRouteLine.DataSource as List<RouteLine>);
        //        }
        //        else if (tbRouteData.SelectedTab.Name.Equals(tbRouteStops.Name))
        //        {
        //            dataSearchAndManipulateBLManager.ExportRouteStopToExcel(folderName, bndSourceRouteStops.DataSource as List<RouteStop>);
        //        }
        //    }
        //    finally
        //    {
        //        processWaitingManager.CloseProcessWaitingForm();
        //    }

        //}

        private static string GetExcelExportFolderName()
        {
            var folderName = Path.Combine(ExportConfigurator.GetConfig().TempPathForAllUsers, GlobalData.LoginUser.UserOperator.IdOperator.ToString());
            if (!IoHelper.IsFolderExists(folderName))
                IoHelper.CreateFolder(folderName);
            //folderName = Path.Combine(folderName, DateTime.Now.ToString("ddMMyyyy hhmmss"));
            //IOHelper.CreateFolder(folderName);
            return folderName;
        }

        private void BtnExcelClick(object sender, EventArgs e)
        {
            //string folderName;
            //var fbd = new FolderBrowserDialog
            //                              {
            //                                  RootFolder = Environment.Ge(ExportConfigurator.GetConfig().TempPathForAllUsers),
            //                                  ShowNewFolderButton = true,
            //                                  Description = ResourceGUI.ChooseExportToExcelFolder
            //                              };

            //if (fbd.ShowDialog() == DialogResult.OK)
            //    folderName = fbd.SelectedPath;
            //else
            //    return;
            string folderName = GetExcelExportFolderName();

            //processWaitingManager.ShowProcessWaitingForm();

            if (tbRouteData.SelectedTab.Name.Equals(tbRouteLines.Name))
            {
                ShowExcelExportSuccess(_dataSearchAndManipulateBlManager.ExportRouteLinesToExcel(folderName, bndSourceRouteLine.DataSource as List<RouteLine>));
            }
            else if (tbRouteData.SelectedTab.Name.Equals(tbRouteStops.Name))
            {
                ShowExcelExportSuccess(_dataSearchAndManipulateBlManager.ExportRouteStopToExcel(folderName, bndSourceRouteStops.DataSource as List<RouteStop>));
            }



        }

        private static void ShowExcelExportSuccess(string fileNameCreated)
        {
            //processWaitingManager.CloseProcessWaitingForm();
            //processWaitingManager.CloseProcessWaitingForm();

            if (!string.IsNullOrEmpty(fileNameCreated))
            {
                GuiHelper.ShowInfoMessage(string.Format(ResourceGUI.ExportExcelCreated, fileNameCreated));
            }
        }

        /// <summary>
        /// bndSourceRouteStops DataSourceChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bndSourceRouteStops_DataSourceChanged(object sender, EventArgs e)
        {
            if (bndSourceRouteStops.DataSource != null)
            {
                var rsList = bndSourceRouteStops.DataSource as List<RouteStop>;
                if (rsList != null) rsList.ForEach(rs => rs.IsSelected = false);
                if ((dtgRouteStops.Columns[dtgRouteStops.ColumnCount - 1].HeaderCell as DGVColumnHeaderRouteStop) != null)
                {
                    (dtgRouteStops.Columns[dtgRouteStops.ColumnCount - 1].HeaderCell as DGVColumnHeaderRouteStop).CheckAll = false;
                    //(dtgRouteStops.Columns[dtgRouteStops.ColumnCount - 1].HeaderCell as DGVColumnHeader)(null);
                }
                dtgRouteStops.Refresh();
            }
        }
        /// <summary>
        /// Show Line Pass In Stastion Tool Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowStations_Click(object sender, EventArgs e)
        {
            _dataSearchAndManipulateBlManager.ShowLinePassInStationToolBox();
        }


        private void BuildMenuMain()
        {
            // Create a main menu object. 
            var myMenu = new MainMenu();
            // Add top-level menu items to the menu. 
            var mAdministrator = new MenuItem(ResourceGUI.mAdministrator);//"מנהל תשתיות"
            myMenu.MenuItems.Add(mAdministrator);
            mAdministrator.Visible = _blManager.IsAdminFormEnabled;
            mAdministrator.MenuItems.Add(_submExportBaseDasaToLicensing);
            if (_blManager.IsExportBaseEnabled)
            {
                _submExportBaseDasaToLicensing.Click += btnExportBase_Click;
            }
            // ????
            if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm)
            {
                //mAdministrator.MenuItems.Add(submExportBaseDasaToLicensing);
                var submUpdateStationLinkToCity = new MenuItem(ResourceGUI.submUpdateStationLinkToCity);//"שיוך של תחנות לעיר"
                mAdministrator.MenuItems.Add(submUpdateStationLinkToCity);
                submUpdateStationLinkToCity.Click += (o, s) =>
                {
                    var linkStationToMunForm = new frmLinkStationToMun();
                    linkStationToMunForm.ShowDialog();
                };

            }
            if (!_blManager.IsEgedOperator)
            {
                var submCalcFirstDropStationForAllSystem = new MenuItem(ResourceGUI.submCalcFirstDropStationForAllSystem); // "חשב תחנת הורדה ראשונה לכל ה-RS."
                mAdministrator.MenuItems.Add(submCalcFirstDropStationForAllSystem);
                submCalcFirstDropStationForAllSystem.Click += BtnRunClick;
                submCalcFirstDropStationForAllSystem.Tag = "submCalcFirstDropStationForAllSystem";

                var submSetBasedLine = new MenuItem(ResourceGUI.submSetBasedLine);//"קבע מזהה קו בסיסי"
                mAdministrator.MenuItems.Add(submSetBasedLine);
                submSetBasedLine.Click += BtnRunClick;
                submSetBasedLine.Tag = "submSetBasedLine";

                // ----------------- Production env is unvisible -----
                var submStationAreaPriceChange = new MenuItem(ResourceGUI.submStationAreaPriceChange);//"בדיקת שינוי תחנה באזור מחיר"
                mAdministrator.MenuItems.Add(submStationAreaPriceChange);
                submStationAreaPriceChange.Click += BtnRunClick;
                submStationAreaPriceChange.Tag = "submStationAreaPriceChange";

                //submStationAreaPriceChange.Visible = false; 

                var submExportStationLinkedToPriceArea = new MenuItem(ResourceGUI.submExportStationLinkedToPriceArea);//"קליטת עדכון שיוך תחנות לאיזור המחיר"
                mAdministrator.MenuItems.Add(submExportStationLinkedToPriceArea);
                submExportStationLinkedToPriceArea.Click += BtnRunClick;
                submExportStationLinkedToPriceArea.Tag = "submExportStationLinkedToPriceArea";
                submExportStationLinkedToPriceArea.Visible = BLSharedUtils.IsTaxiOperator();

                var submExportStationPriceAreaImport = new MenuItem(ResourceGUI.submExportStationPriceAreaImport);   //קליטת אזורי מחיר
                mAdministrator.MenuItems.Add(submExportStationPriceAreaImport);
                submExportStationPriceAreaImport.Click += BtnRunClick;
                submExportStationPriceAreaImport.Tag = "submExportStationPriceAreaImport";
                submExportStationPriceAreaImport.Visible = BLSharedUtils.IsTaxiOperator();

                // ------------------------------------------------------------------
            }
            if (_blManager.IsAdminFormEnabled)
            {
                var submClusterValidDateExportManage = new MenuItem(ResourceGUI.submClusterValidDateExportManage);//"ניהול תאריכי ייצוא של אשכולות"
                mAdministrator.MenuItems.Add(submClusterValidDateExportManage);
                submClusterValidDateExportManage.Click += (o, s) => new frmClusterValidExportDateManage().ShowDialog();

                var submShowReportExportDateValidationForCurrenrOperator = new MenuItem(ResourceGUI.submClusterValidDateExportManageOfOperator);//"דוח תאריכי ייצוא לרישוי"
                mAdministrator.MenuItems.Add(submShowReportExportDateValidationForCurrenrOperator);
                submShowReportExportDateValidationForCurrenrOperator.Click += (o, s) => new frmExportValidateDateOptions
                {
                    Routes = GlobalData.RouteModel.RouteLineList,
                    ReportResults = null,
                    IsInsideExportProcess = false
                }.ShowDialog();

                var convertIstrShapeToTab = new MenuItem(ResourceGUI.convertIstrShapeToTab);//"המרה של קובץ בפורמט שייפ לטאב"
                mAdministrator.MenuItems.Add(convertIstrShapeToTab);
                convertIstrShapeToTab.Click += BtnRunClick;
                convertIstrShapeToTab.Tag = "convertIstrShapeToTab";

                var loginManagment = new MenuItem(ResourceGUI.LoginManagment);//"ניהול משתמשים במערכת"
                mAdministrator.MenuItems.Add(loginManagment);
                loginManagment.Click += BtnRunClick;
                loginManagment.Tag = "loginManagment";

                var linkstationToMunMenu = new MenuItem(ResourceGUI.linkstationToMun);//"שיוך תחנה לשטח מוניציפאלי"
                mAdministrator.MenuItems.Add(linkstationToMunMenu);
                linkstationToMunMenu.Click += BtnRunClick;
                linkstationToMunMenu.Tag = "linkstationToMun";
                linkstationToMunMenu.Visible = BLSharedUtils.IsTaxiOperator();

                var mnuVerifyBasedMap = new MenuItem(ResourceGUI.verifyBasedMap);// בדיקות תקינות לאחר הפצת תשתית למפעילים
                mAdministrator.MenuItems.Add(mnuVerifyBasedMap);
                mnuVerifyBasedMap.Click += BtnRunClick;
                mnuVerifyBasedMap.Tag = "mnuVerifyBasedMap";
                mnuVerifyBasedMap.Visible = BLSharedUtils.IsTaxiOperator() && false;
            }

            var submShowConfigFile = new MenuItem(ResourceGUI.submShowConfigFile);//"הצג קובץ קונפיגורציה"
            mAdministrator.MenuItems.Add(submShowConfigFile);
            submShowConfigFile.Click += new EventHandler(BtnRunClick);
            submShowConfigFile.Tag = "submShowConfigFile";

            var submUpdateStreetLayer = new MenuItem(ResourceGUI.submUpdateStreetLayer);//"עדכן את שכבת הרחובות"
            mAdministrator.MenuItems.Add(submUpdateStreetLayer);
            submUpdateStreetLayer.Tag = "submUpdateStreetLayer";
            submUpdateStreetLayer.Click += new EventHandler(BtnRunClick);

            var submAppendOperatorRouteSystems = new MenuItem(ResourceGUI.submAppendOperatorRouteSystems);//"צרף רוט סיסטם של כול המפעילים"
            mAdministrator.MenuItems.Add(submAppendOperatorRouteSystems);
            submAppendOperatorRouteSystems.Tag = "submAppendOperatorRouteSystems";
            submAppendOperatorRouteSystems.Click += BtnRunClick;

            submAppendOperatorRouteSystems.Visible = false;

            if (!_blManager.IsEgedOperator)
            {
                MenuItem submUpdatePhysicalStopsLayer = new MenuItem(ResourceGUI.submUpdatePhysicalStopsLayer);//"עדכן את שכבת התחנות הפיזיות"
                mAdministrator.MenuItems.Add(submUpdatePhysicalStopsLayer);
                submUpdatePhysicalStopsLayer.Tag = "submUpdatePhysicalStopsLayer";
                submUpdatePhysicalStopsLayer.Click += new EventHandler(BtnRunClick);

                MenuItem submBuildLineJunctionFile = new MenuItem(ResourceGUI.submBuildLineJunctionFile);//"בנה קובץ לקביעת מספר המוקדים בקו"
                mAdministrator.MenuItems.Add(submBuildLineJunctionFile);
                submBuildLineJunctionFile.Tag = "submBuildLineJunctionFile";
                submBuildLineJunctionFile.Click += new EventHandler(BtnRunClick);
            }
            MenuItem submExportEggedStations = new MenuItem(ResourceGUI.submExportEggedStations);//"ייצא את תחנות אגד"
            mAdministrator.MenuItems.Add(submExportEggedStations);
            submExportEggedStations.Tag = "submExportEggedStations";
            submExportEggedStations.Click += new EventHandler(BtnRunClick);

            MenuItem submExportEgedCSV = new MenuItem(ResourceGUI.submEgedExportProcess);//"ייצא מפה להשוואה לבסיס הנתונים"
            mAdministrator.MenuItems.Add(submExportEgedCSV);
            submExportEgedCSV.Tag = "submExportEgedCSV";
            submExportEgedCSV.Click += BtnRunClick;

            MenuItem mExport = new MenuItem(ResourceGUI.mExport);//"ייצוא"
            myMenu.MenuItems.Add(mExport);
            if (!_blManager.IsEgedOperator)
            {
                MenuItem submCheckMapBeforeExport = new MenuItem(ResourceGUI.submCheckMapBeforeExport);//"בדוק מזהי קווים ותחנות לפני ייצוא לרישוי"
                mExport.MenuItems.Add(submCheckMapBeforeExport);
                submCheckMapBeforeExport.Click += BtnRefreshClick;
            }

            mExport.MenuItems.Add(_submExportLinesToLicensingSystem);
            _submExportLinesToLicensingSystem.Visible = _blManager.IsEgedOperator;
            _submExportLinesToLicensingSystem.Click += BtnCreateTextFilesClick;
            if (!_blManager.IsEgedOperator)
            {
                MenuItem submExportLinesToExcel = new MenuItem(ResourceGUI.submExportLinesToExcel);//"ייצא רשימת קווים לקובץ אקסל"
                mExport.MenuItems.Add(submExportLinesToExcel);
                submExportLinesToExcel.Click += new EventHandler(BtnExcelClick);
            }
            MenuItem submExportMapToImage = new MenuItem(ResourceGUI.submExportMapToImage);//"ייצא מפה לקובץ תמונה"
            mExport.MenuItems.Add(submExportMapToImage);
            submExportMapToImage.Click += new EventHandler(btnSaveMapAsImage_Click);

            MenuItem mAdditionalInfo = new MenuItem(ResourceGUI.mAdditionalInfo);//"מידע נוסף"
            myMenu.MenuItems.Add(mAdditionalInfo);

            MenuItem submShowAdditionalLayersInMap = new MenuItem(ResourceGUI.submShowAdditionalLayersInMap);//"הצג שכבות נוספות במפה"
            mAdditionalInfo.MenuItems.Add(submShowAdditionalLayersInMap);
            submShowAdditionalLayersInMap.Click += btnLayersImp_Click;

            MenuItem submShowConvertCodesOfBasedTable = new MenuItem(ResourceGUI.submShowConvertCodesOfBasedTable);//"הצג קודי המרה של מערכת הרישוי"
            mAdditionalInfo.MenuItems.Add(submShowConvertCodesOfBasedTable);
            submShowConvertCodesOfBasedTable.Click += btnBasedTable_Click;

            MenuItem submShowLinesPassingViaStation = new MenuItem(ResourceGUI.submShowLinesPassingViaStation);//"הצג קווים העוברים בתחנה"
            mAdditionalInfo.MenuItems.Add(submShowLinesPassingViaStation);
            submShowLinesPassingViaStation.Click += btnShowStations_Click;

            MenuItem mPriceList = new MenuItem(ResourceGUI.mPriceList);//"מחירונים"
            myMenu.MenuItems.Add(mPriceList);

            MenuItem submEnablePriceListToLicensingSystem = new MenuItem(ResourceGUI.submEnablePriceListToLicensingSystem);//"לאפשר לעבודעם  תפריט אזורי מחיר"
            mPriceList.MenuItems.Add(submEnablePriceListToLicensingSystem);
            submEnablePriceListToLicensingSystem.Click += SubmEnablePriceListToLicensingSystemClick;


            //MenuItem submExportPriceListToLicensingSystem = new MenuItem(ResourceGUI.submExportPriceListToLicensingSystem);
            mPriceList.MenuItems.Add(_submExportPriceListToLicensingSystem);//"ייצוא של מחירונים למערכת רישוי"
            _submExportPriceListToLicensingSystem.Enabled = false;
            _submExportPriceListToLicensingSystem.Click += SubmExportPriceListToLicensingSystemClick;

            //MenuItem submUpdateAreaPriceDetails = new MenuItem(ResourceGUI.submUpdateAreaPriceDetails);//"עדכון פרטים של אזורי מחיר"
            _submUpdateAreaPriceDetails.Enabled = false; ;//"עדכון פרטים של אזורי מחיר"
            mPriceList.MenuItems.Add(_submUpdateAreaPriceDetails);
            _submUpdateAreaPriceDetails.Click += SubmUpdateAreaPriceDetailsClick;

            // Assign the menu to the form. 
            myMenu.RightToLeft = RightToLeft.Yes;

            if (_blManager.IsEgedOperator)
            {
                var mEged = new MenuItem(ResourceGUI.mEgedMapLoad);//"אגד"
                myMenu.MenuItems.Add(mEged);
                var submImportEgedMap = new MenuItem(ResourceGUI.submImportEgedMap);//
                mEged.MenuItems.Add(submImportEgedMap);
                submImportEgedMap.Click += SubmLoadEgedMapClick;

                //MenuItem submImportEndPointsAndPhysicalStops = new MenuItem(ResourceGUI.submImportEndPointsAndPhysicalStops);//
                //mEged.MenuItems.Add(submImportEndPointsAndPhysicalStops);
                //submImportEndPointsAndPhysicalStops.Click += new EventHandler(submImportEndPointsAndPhysicalStops_Click);
            }

            //Exit Menu
            var mExit = new MenuItem(ResourceGUI.ExitFromApplication);//"יציאה"
            myMenu.MenuItems.Add(mExit);
            mExit.Click += ((sender, e) =>
            {
                _presentation.ExitFormApplication();
                Close();
            });
            Menu = myMenu;
        }


        //private void BuildMenuMain1()
        //{
        //    // Create a main menu object. 
        //    var myMenu = new MainMenu();
        //    // Add top-level menu items to the menu. 
        //    var mAdministrator = new MenuItem(ResourceGUI.mAdministrator);//"מנהל תשתיות"
        //    myMenu.MenuItems.Add(mAdministrator);
        //    mAdministrator.Visible = _blManager.IsAdminFormEnabled;
        //    mAdministrator.MenuItems.Add(_submExportBaseDasaToLicensing);
        //    if (_blManager.IsExportBaseEnabled)
        //    {
        //        _submExportBaseDasaToLicensing.Click += btnExportBase_Click;
        //    }
        //    // ????
        //    if ( GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm)
        //    {
        //        //mAdministrator.MenuItems.Add(submExportBaseDasaToLicensing);
        //        var submUpdateStationLinkToCity = new MenuItem(ResourceGUI.submUpdateStationLinkToCity);//"שיוך של תחנות לעיר"
        //        mAdministrator.MenuItems.Add(submUpdateStationLinkToCity);
        //        submUpdateStationLinkToCity.Click += (o, s) =>
        //        {
        //            var linkStationToMunForm = new frmLinkStationToMun();
        //            linkStationToMunForm.ShowDialog();
        //        };

        //    }
        //    if (!_blManager.IsEgedOperator)
        //    {
        //        var submCalcFirstDropStationForAllSystem = new MenuItem(ResourceGUI.submCalcFirstDropStationForAllSystem); // "חשב תחנת הורדה ראשונה לכל ה-RS."
        //        mAdministrator.MenuItems.Add(submCalcFirstDropStationForAllSystem);
        //        submCalcFirstDropStationForAllSystem.Click += BtnRunClick;
        //        submCalcFirstDropStationForAllSystem.Tag = "submCalcFirstDropStationForAllSystem";

        //        var submSetBasedLine = new MenuItem(ResourceGUI.submSetBasedLine);//"קבע מזהה קו בסיסי"
        //        mAdministrator.MenuItems.Add(submSetBasedLine);
        //        submSetBasedLine.Click += BtnRunClick;
        //        submSetBasedLine.Tag = "submSetBasedLine";

                
        //        //var submExportStationLinkedToPriceArea = new MenuItem(ResourceGUI.submExportStationLinkedToPriceArea);   //"קליטת עדכון שיוך תחנות לאיזור המחיר"
        //        //mAdministrator.MenuItems.Add(submExportStationLinkedToPriceArea);
        //        //submExportStationLinkedToPriceArea.Click += BtnRunClick;
        //        //submExportStationLinkedToPriceArea.Tag = "submExportStationLinkedToPriceArea";
        //        //submExportStationLinkedToPriceArea.Visible = BLSharedUtils.IsTaxiOperator();
                
        //        var submExportStationPriceAreaImport = new MenuItem(ResourceGUI.submExportStationPriceAreaImport);   //קליטת אזורי מחיר
        //        mAdministrator.MenuItems.Add(submExportStationPriceAreaImport);
        //        submExportStationPriceAreaImport.Click += BtnRunClick;
        //        submExportStationPriceAreaImport.Tag = "submExportStationPriceAreaImport";
        //        submExportStationPriceAreaImport.Visible = BLSharedUtils.IsTaxiOperator();

        //    }
        //    if (_blManager.IsAdminFormEnabled)
        //    {
        //        var submClusterValidDateExportManage = new MenuItem(ResourceGUI.submClusterValidDateExportManage);//"ניהול תאריכי ייצוא של אשכולות"
        //        mAdministrator.MenuItems.Add(submClusterValidDateExportManage);
        //        submClusterValidDateExportManage.Click += (o, s) => new frmClusterValidExportDateManage().ShowDialog();

        //        var submShowReportExportDateValidationForCurrenrOperator = new MenuItem(ResourceGUI.submClusterValidDateExportManageOfOperator);//"דוח תאריכי ייצוא לרישוי"
        //        mAdministrator.MenuItems.Add(submShowReportExportDateValidationForCurrenrOperator);
        //        submShowReportExportDateValidationForCurrenrOperator.Click += (o, s) => new frmExportValidateDateOptions
        //                                                                {
        //                                                                    Routes = GlobalData.RouteModel.RouteLineList,
        //                                                                    ReportResults = null,
        //                                                                    IsInsideExportProcess = false
        //                                                                 }.ShowDialog();

        //        var convertIstrShapeToTab = new MenuItem(ResourceGUI.convertIstrShapeToTab);//"המרה של קובץ בפורמט שייפ לטאב"
        //        mAdministrator.MenuItems.Add(convertIstrShapeToTab);
        //        convertIstrShapeToTab.Click += BtnRunClick;
        //        convertIstrShapeToTab.Tag = "convertIstrShapeToTab";

        //        var loginManagment = new MenuItem(ResourceGUI.LoginManagment);//"ניהול משתמשים במערכת"
        //        mAdministrator.MenuItems.Add(loginManagment);
        //        loginManagment.Click += BtnRunClick;
        //        loginManagment.Tag = "loginManagment";
                
        //        var linkstationToMunMenu = new MenuItem(ResourceGUI.linkstationToMun);//"שיוך תחנה לשטח מוניציפאלי"
        //        mAdministrator.MenuItems.Add(linkstationToMunMenu);
        //        linkstationToMunMenu.Click += BtnRunClick;
        //        linkstationToMunMenu.Tag = "linkstationToMun";
        //        linkstationToMunMenu.Visible = BLSharedUtils.IsTaxiOperator();

        //        var mnuVerifyBasedMap = new MenuItem(ResourceGUI.verifyBasedMap);// בדיקות תקינות לאחר הפצת תשתית למפעילים
        //        mAdministrator.MenuItems.Add(mnuVerifyBasedMap);
        //        mnuVerifyBasedMap.Click += BtnRunClick;
        //        mnuVerifyBasedMap.Tag = "mnuVerifyBasedMap";
        //        mnuVerifyBasedMap.Visible = BLSharedUtils.IsTaxiOperator() && false;
        //    }

        //    var submShowConfigFile = new MenuItem(ResourceGUI.submShowConfigFile);//"הצג קובץ קונפיגורציה"
        //    mAdministrator.MenuItems.Add(submShowConfigFile);
        //    submShowConfigFile.Click += new EventHandler(BtnRunClick);
        //    submShowConfigFile.Tag = "submShowConfigFile";

        //    var submUpdateStreetLayer = new MenuItem(ResourceGUI.submUpdateStreetLayer);//"עדכן את שכבת הרחובות"
        //    mAdministrator.MenuItems.Add(submUpdateStreetLayer);
        //    submUpdateStreetLayer.Tag = "submUpdateStreetLayer";
        //    submUpdateStreetLayer.Click += new EventHandler(BtnRunClick);

        //    var submAppendOperatorRouteSystems = new MenuItem(ResourceGUI.submAppendOperatorRouteSystems);//"צרף רוט סיסטם של כול המפעילים"
        //    mAdministrator.MenuItems.Add(submAppendOperatorRouteSystems);
        //    submAppendOperatorRouteSystems.Tag = "submAppendOperatorRouteSystems";
        //    submAppendOperatorRouteSystems.Click += BtnRunClick;

        //    submAppendOperatorRouteSystems.Visible = false; 

        //    if (!_blManager.IsEgedOperator)
        //    {
        //        MenuItem submUpdatePhysicalStopsLayer = new MenuItem(ResourceGUI.submUpdatePhysicalStopsLayer);//"עדכן את שכבת התחנות הפיזיות"
        //        mAdministrator.MenuItems.Add(submUpdatePhysicalStopsLayer);
        //        submUpdatePhysicalStopsLayer.Tag = "submUpdatePhysicalStopsLayer";
        //        submUpdatePhysicalStopsLayer.Click += new EventHandler(BtnRunClick);

        //        MenuItem submBuildLineJunctionFile = new MenuItem(ResourceGUI.submBuildLineJunctionFile);//"בנה קובץ לקביעת מספר המוקדים בקו"
        //        mAdministrator.MenuItems.Add(submBuildLineJunctionFile);
        //        submBuildLineJunctionFile.Tag = "submBuildLineJunctionFile";
        //        submBuildLineJunctionFile.Click += new EventHandler(BtnRunClick);
        //    }
        //    MenuItem submExportEggedStations = new MenuItem(ResourceGUI.submExportEggedStations);//"ייצא את תחנות אגד"
        //    mAdministrator.MenuItems.Add(submExportEggedStations);
        //    submExportEggedStations.Tag = "submExportEggedStations";
        //    submExportEggedStations.Click += new EventHandler(BtnRunClick);

        //    MenuItem submExportEgedCSV = new MenuItem(ResourceGUI.submEgedExportProcess);//"ייצא מפה להשוואה לבסיס הנתונים"
        //    mAdministrator.MenuItems.Add(submExportEgedCSV);
        //    submExportEgedCSV.Tag = "submExportEgedCSV";
        //    submExportEgedCSV.Click += BtnRunClick;

        //    MenuItem mExport = new MenuItem(ResourceGUI.mExport);//"ייצוא"
        //    myMenu.MenuItems.Add(mExport);
        //    if (!_blManager.IsEgedOperator)
        //    {
        //        MenuItem submCheckMapBeforeExport = new MenuItem(ResourceGUI.submCheckMapBeforeExport);//"בדוק מזהי קווים ותחנות לפני ייצוא לרישוי"
        //        mExport.MenuItems.Add(submCheckMapBeforeExport);
        //        submCheckMapBeforeExport.Click += BtnRefreshClick;
        //    }

        //    mExport.MenuItems.Add(_submExportLinesToLicensingSystem);
        //    _submExportLinesToLicensingSystem.Visible = _blManager.IsEgedOperator;
        //    _submExportLinesToLicensingSystem.Click += BtnCreateTextFilesClick;
        //    if (!_blManager.IsEgedOperator)
        //    {
        //        MenuItem submExportLinesToExcel = new MenuItem(ResourceGUI.submExportLinesToExcel);//"ייצא רשימת קווים לקובץ אקסל"
        //        mExport.MenuItems.Add(submExportLinesToExcel);
        //        submExportLinesToExcel.Click += new EventHandler(BtnExcelClick);
        //    }
        //    MenuItem submExportMapToImage = new MenuItem(ResourceGUI.submExportMapToImage);//"ייצא מפה לקובץ תמונה"
        //    mExport.MenuItems.Add(submExportMapToImage);
        //    submExportMapToImage.Click += new EventHandler(btnSaveMapAsImage_Click);

        //    MenuItem mAdditionalInfo = new MenuItem(ResourceGUI.mAdditionalInfo);//"מידע נוסף"
        //    myMenu.MenuItems.Add(mAdditionalInfo);

        //    MenuItem submShowAdditionalLayersInMap = new MenuItem(ResourceGUI.submShowAdditionalLayersInMap);//"הצג שכבות נוספות במפה"
        //    mAdditionalInfo.MenuItems.Add(submShowAdditionalLayersInMap);
        //    submShowAdditionalLayersInMap.Click += btnLayersImp_Click;

        //    MenuItem submShowConvertCodesOfBasedTable = new MenuItem(ResourceGUI.submShowConvertCodesOfBasedTable);//"הצג קודי המרה של מערכת הרישוי"
        //    mAdditionalInfo.MenuItems.Add(submShowConvertCodesOfBasedTable);
        //    submShowConvertCodesOfBasedTable.Click += btnBasedTable_Click;

        //    MenuItem submShowLinesPassingViaStation = new MenuItem(ResourceGUI.submShowLinesPassingViaStation);//"הצג קווים העוברים בתחנה"
        //    mAdditionalInfo.MenuItems.Add(submShowLinesPassingViaStation);
        //    submShowLinesPassingViaStation.Click += btnShowStations_Click;

        //    MenuItem mPriceList = new MenuItem(ResourceGUI.mPriceList);//"מחירונים"
        //    myMenu.MenuItems.Add(mPriceList);

        //    MenuItem submEnablePriceListToLicensingSystem = new MenuItem(ResourceGUI.submEnablePriceListToLicensingSystem);//"לאפשר לעבודעם  תפריט אזורי מחיר"
        //    mPriceList.MenuItems.Add(submEnablePriceListToLicensingSystem);
        //    submEnablePriceListToLicensingSystem.Click += SubmEnablePriceListToLicensingSystemClick;


        //    //MenuItem submExportPriceListToLicensingSystem = new MenuItem(ResourceGUI.submExportPriceListToLicensingSystem);
        //    mPriceList.MenuItems.Add(_submExportPriceListToLicensingSystem);//"ייצוא של מחירונים למערכת רישוי"
        //    _submExportPriceListToLicensingSystem.Enabled = false;
        //    _submExportPriceListToLicensingSystem.Click += SubmExportPriceListToLicensingSystemClick;

        //    //MenuItem submUpdateAreaPriceDetails = new MenuItem(ResourceGUI.submUpdateAreaPriceDetails);//"עדכון פרטים של אזורי מחיר"
        //    _submUpdateAreaPriceDetails.Enabled = false; ;//"עדכון פרטים של אזורי מחיר"
        //    mPriceList.MenuItems.Add(_submUpdateAreaPriceDetails);
        //    _submUpdateAreaPriceDetails.Click += SubmUpdateAreaPriceDetailsClick;

        //    // Assign the menu to the form. 
        //    myMenu.RightToLeft = RightToLeft.Yes;

        //    if (_blManager.IsEgedOperator)
        //    {
        //        var mEged = new MenuItem(ResourceGUI.mEgedMapLoad);//"אגד"
        //        myMenu.MenuItems.Add(mEged);
        //        var submImportEgedMap = new MenuItem(ResourceGUI.submImportEgedMap);//
        //        mEged.MenuItems.Add(submImportEgedMap);
        //        submImportEgedMap.Click += SubmLoadEgedMapClick;

        //        //MenuItem submImportEndPointsAndPhysicalStops = new MenuItem(ResourceGUI.submImportEndPointsAndPhysicalStops);//
        //        //mEged.MenuItems.Add(submImportEndPointsAndPhysicalStops);
        //        //submImportEndPointsAndPhysicalStops.Click += new EventHandler(submImportEndPointsAndPhysicalStops_Click);

               

        //    }
        //    if (!_blManager.IsEgedOperator)
        //    {
        //        var submStationAreaPriceChange = new MenuItem(ResourceGUI.submStationAreaPriceChange);//"בדיקת שינוי תחנה באזור מחיר"
        //        mAdministrator.MenuItems.Add(submStationAreaPriceChange);
        //        submStationAreaPriceChange.Click += BtnRunClick;
        //        submStationAreaPriceChange.Tag = "submStationAreaPriceChange";
        //        submStationAreaPriceChange.Enabled = false;
        //        // ------------------------------------------------------------------
        //    }
        //    //Exit Menu
        //    var mExit = new MenuItem(ResourceGUI.ExitFromApplication);//"יציאה"
        //    myMenu.MenuItems.Add(mExit);
        //    mExit.Click += ((sender, e) =>
        //                        {
        //                            _presentation.ExitFormApplication();
        //                            Close();
        //                        });
        //    Menu = myMenu;
        //}
        
        /// <summary>
        /// submLoadEgedMap_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmLoadEgedMapClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (!GuiHelper.ShowConfirmationMessage(string.Format(ResourceGUI.LoadEgedMap, ExportConfigurator.GetConfig().EgedFileSourceFolder)))
                    return;
                _processWaitingManager.ShowProcessWaitingForm();
                var loadEgedMapBl = new LoadEgedMapBL { LoaderFolder = ExportConfigurator.GetConfig().EgedFileSourceFolder };
                loadEgedMapBl.LoadMap();
                _processWaitingManager.CloseProcessWaitingForm();
                GuiHelper.ShowInfoMessage(ResourceGUI.EgedMadWasSuccesefullyLoaded);
                Application.Exit();
            }
            catch (Exception)
            {
                _processWaitingManager.CloseProcessWaitingForm();
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
                Activate();
            }
        }
        /// <summary>
        /// submUpdateAreaPriceDetails Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SubmUpdateAreaPriceDetailsClick(object sender, EventArgs e)
        {
            //frmPriceList priceList = new frmPriceList();
            //priceList.ShowDialog();
            var priceListPolygonBl = new PriceListPolygonBl();
            priceListPolygonBl.ShowPriceAreaPolygonToolBox();
        }
        /// <summary>
        /// submExportPriceListToLicensingSystem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmExportPriceListToLicensingSystemClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (!GuiHelper.ShowConfirmationMessage(ResourceGUI.PriceListPocessRunning))
                    return;
                var priceListPolygonBl = new PriceListPolygonBl();
                var changedAreaList = priceListPolygonBl.GetChangedAreaListAndUpdateZeroZoneCodes();
                if (changedAreaList.IsListFull())
                {
                    var zoneImportAreaChangedForm = new frmZoneImportAreaChanged(changedAreaList);
                    zoneImportAreaChangedForm.ShowDialog();
                }
                _processWaitingManager.CloseProcessWaitingForm();

                _processWaitingManager = new ProcessWaitingManager
                {
                    ShowCancelButton = true
                };

                _processWaitingManager.ShowProcessWaitingForm();
                var message = string.Empty;
                priceListPolygonBl.Changed += (o,a) =>
                                                  {
                                                      priceListPolygonBl.IsCanceledByUser = _processWaitingManager.IsCanceledByUser;
                                                  };
                if (!priceListPolygonBl.ExportPriceListDb(ref message))
                {
                    _processWaitingManager.CloseProcessWaitingForm();
                    GuiHelper.ShowInfoMessage(message);
                }
                else
                {
                    _processWaitingManager.CloseProcessWaitingForm();
                    GuiHelper.ShowInfoMessage(ResourceGUI.PriceListPassedSuccesefuly);
                }
            }
            catch (Exception)
            {
                _processWaitingManager.CloseProcessWaitingForm();
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
                Activate();
            }
        }



        private void SubmEnablePriceListToLicensingSystemClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (!GuiHelper.ShowConfirmationMessage(ResourceGUI.EnableAreaPriceMenu))
                    return;
                var enablePriceListPolygonBl = new EnablePriceListPolygonBl();
                if (enablePriceListPolygonBl.IsAreaLayerAppended())
                {
                    _submExportPriceListToLicensingSystem.Enabled = true;
                    _submUpdateAreaPriceDetails.Enabled = true;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                Activate();
            }
        }
        

        /// <summary>
        /// btnRun Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string senderName;
                if (sender is Menu)
                {
                    senderName = (sender as Menu).Tag.ToString();
                }
                else
                    senderName = ((Control) sender).Tag.ToString();
                if (senderName != "loginManagment" && senderName != "linkstationToMun" && senderName != "submExportStationPriceAreaImport")
                    _processWaitingManager.ShowProcessWaitingForm();
                switch (senderName)
                {
                    case "linkstationToMun":
                        var frmLinkstationToMunArea = new FrmLinkstationToMunArea();
                        frmLinkstationToMunArea.ShowDialog();
                        break;
                    case "loginManagment" :
                        var frmLoginsManagment = new FrmLoginsManagment();
                        frmLoginsManagment.ShowDialog();
                        break;

                    case "mnuVerifyBasedMap" :
                        if (_administrationBl.VerifyBasedMap())
                        {
                            _processWaitingManager.CloseProcessWaitingForm();
                            GuiHelper.ShowInfoMessage(ResourceGUI.DistributionInfrastructureVerifiedSuccesefully);
                        }
                        else
                        {
                            _processWaitingManager.CloseProcessWaitingForm();
                            GuiHelper.ShowInfoMessage(ResourceGUI.DistributionInfrastructureVerifiedFailed);
                        }
                        break;
                    case "convertIstrShapeToTab":
                        var zipFile = _administrationBl.ConvertIstrShapeToTabAndGetZipFile();
                        _processWaitingManager.CloseProcessWaitingForm();
                        if (!string.IsNullOrEmpty(zipFile))
                            GuiHelper.ShowInfoMessage(string.Format(ResourceGUI.TABFileWasCreated,zipFile));
                        break;
                    case "submAppendOperatorRouteSystems" :
                        _administrationBl.AppendOperatorRouteSystems();
                        break;
                    case "submShowConfigFile":
                        _administrationBl.DecryptConfigFile();
                        break;
                    case "submSetBasedLine":
                        {
                            _administrationBl.SetBaseRouteLines();
                            _processWaitingManager.CloseProcessWaitingForm();
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                                Application.Exit();
                        }
                        break;
                    case "submCalcFirstDropStationForAllSystem":
                        {
                            _administrationBl.CalcStationHorada();
                            _processWaitingManager.CloseProcessWaitingForm();
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                                Application.Exit();
                        }
                        break;
                    case "submUpdateStreetLayer":
                        {
                            _administrationBl.UpdateEndPoints();
                            _processWaitingManager.CloseProcessWaitingForm();
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                                Application.Exit();
                        }
                        break;

                    case "submExportEgedCSV":
                        {
                            var fbd = new FolderBrowserDialog
                                          {
                                              ShowNewFolderButton = true,
                                              Description = Resources.ResourceGUI.SelectEgedFolderCSVFile
                                          };
                            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                            {
                                var folder = fbd.SelectedPath;
                                _administrationBl.ExportEgedCsv(folder);
                            }


                            break;
                        }
                    case "submUpdatePhysicalStopsLayer":
                        {
                            _administrationBl.UpdatePhysicalStops();
                            _processWaitingManager.CloseProcessWaitingForm();
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                                Application.Exit();
                        }
                        break;
                    case "submExportStationPriceAreaImport" :
                        {
                            _processWaitingManager = new ProcessWaitingManager { ShowProgressBar = true, ProgressBarInitValue = 1, ProgressBarMaxValue = 10 };
                            _processWaitingManager.ShowProcessWaitingForm();
                            var openFileDialog = new OpenFileDialog
                            {
                                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                Filter = string.Format("(*.{0})|*.{0}", "zip"),
                                FilterIndex = 1,
                                Title = ResourceGUI.submExportStationPriceAreaImport,
                                RestoreDirectory = true
                            };
                            if (openFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog.FileName))
                            {
                                var manager = new BlPriceAreaToStationManager(openFileDialog.FileName);
                                manager.Changed += (sebderO,msg) => _processWaitingManager.ChangeProgressBar(msg.ProgressBarValue,msg.MaxProgressBarValue,msg.Message);
                                if (!manager.ValidateInput())
                                {
                                    GuiHelper.ShowErrorMessage(manager.ErrorMessage);
                                    _processWaitingManager.CloseProcessWaitingForm();
                                    break;
                                }
                                if (!manager.ImportInput())
                                {
                                    _processWaitingManager.CloseProcessWaitingForm();
                                    GuiHelper.ShowErrorMessage(manager.ErrorMessage);
                                    break;
                                }
                                if (!manager.CheckDataValidity())
                                {
                                    _processWaitingManager.CloseProcessWaitingForm();
                                    GuiHelper.ShowErrorMessage(manager.ErrorMessage);
                                    break;
                                }
                                if (!manager.ExportToLicensingSystem())
                                {
                                    _processWaitingManager.CloseProcessWaitingForm();
                                    GuiHelper.ShowErrorMessage(manager.ErrorMessage);
                                    break;
                                }
                                _processWaitingManager.CloseProcessWaitingForm();
                                GuiHelper.ShowInfoMessage(ResourceGUI.PriceListPassedSuccesefuly);
                                break;
                            }
                            _processWaitingManager.CloseProcessWaitingForm();
                            break;
                        }
                    case "submExportStationLinkedToPriceArea" :
                        {
                            var openFileDialog1 = new OpenFileDialog
                                                      {
                                                          InitialDirectory =
                                                              Environment.GetFolderPath(
                                                                  Environment.SpecialFolder.Desktop),
                                                          Filter = string.Format("(*.{0})|*.{0}", "csv"),
                                                          FilterIndex = 1,
                                                          Title =
                                                              Resources.ResourceGUI.
                                                              SelectStationCSVFileOfProcessExportSationToPriceAreaAllOperators,
                                                          RestoreDirectory = true
                                                      };
                            if (openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
                            {
                                    var blExportPriceAreaOfStationForAllOperators = new BlExportPriceAreaOfStationForAllOperators();
                                    var errorMessage = string.Empty;
                                    if (blExportPriceAreaOfStationForAllOperators.CheckDataValidity(openFileDialog1.FileName, ref errorMessage))
                                    {
                                        if (!blExportPriceAreaOfStationForAllOperators.ExportPriceListDb(ref errorMessage))
                                        {
                                            _processWaitingManager.CloseProcessWaitingForm();
                                            GuiHelper.ShowInfoMessage(errorMessage);
                                        }
                                        else
                                        {
                                            _processWaitingManager.CloseProcessWaitingForm();
                                            GuiHelper.ShowInfoMessage(ResourceGUI.PriceListPassedSuccesefuly);
                                        } 
                                    }
                                    else
                                    {
                                        _processWaitingManager.CloseProcessWaitingForm();
                                        GuiHelper.ShowErrorMessage(errorMessage);
                                    }
                            }
                            _processWaitingManager.CloseProcessWaitingForm();
                            break;
                        }
                    case "submStationAreaPriceChange" :
                        {
                            var openFileDialog1 = new OpenFileDialog
                                                      {
                                                          InitialDirectory =
                                                              Environment.GetFolderPath(
                                                                  Environment.SpecialFolder.Desktop),
                                                          Filter = string.Format("(*.{0})|*.{0}", "csv"),
                                                          FilterIndex = 1,
                                                          Title =
                                                              ResourceGUI.
                                                              SelectStationCSVFileOfProcessAreaPriceNewLocation,
                                                          RestoreDirectory = true
                                                      };
                            if (openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
                            {
                                string filename = openFileDialog1.FileName;
                                var fbd = new FolderBrowserDialog
                                              {
                                                  ShowNewFolderButton = true,
                                                  Description =
                                                      ResourceGUI.ChooseStationAreaPriceReportVersionFileBuilddFolder
                                              };
                                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                                {
                                    string folder = fbd.SelectedPath;
                                    var blComparePriceAreaOfStation = new BlComparePriceAreaOfStation();
                                    string errorMessage = string.Empty; 
                                    if (blComparePriceAreaOfStation.CheckDataValidity(filename,ref errorMessage))
                                    {
                                        blComparePriceAreaOfStation.BuildReport(folder);
                                        GuiHelper.ShowInfoMessage(ResourceGUI.PriceListFileCreatedSuccefully);
                                    }
                                    else
                                    {
                                        _processWaitingManager.CloseProcessWaitingForm();
                                        GuiHelper.ShowErrorMessage(errorMessage);
                                    }
                                }
                            }
                            _processWaitingManager.CloseProcessWaitingForm();
                        }
                        break;


                    case "submBuildLineJunctionFile":
                        {
                            var openFileDialog1 = new OpenFileDialog
                                                      {
                                                          InitialDirectory =
                                                              Environment.GetFolderPath(
                                                                  Environment.SpecialFolder.ApplicationData),
                                                          Filter = string.Format("(*.{0})|*.{0}", "csv"),
                                                          FilterIndex = 1,
                                                          Title = Resources.ResourceGUI.CSVFileJunctionVersionNonHeader,
                                                          RestoreDirectory = true
                                                      };
                            if (openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
                            {
                                string filename = openFileDialog1.FileName;
                                var fbd = new FolderBrowserDialog
                                              {
                                                  ShowNewFolderButton = true,
                                                  Description =
                                                      Resources.ResourceGUI.ChooseJunctionVersionFileBuilddFolder
                                              };
                                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                                {
                                    string folder = fbd.SelectedPath;

                                    //if (Convert.ToBoolean(ExportConfiguration.ExportConfigurator.GetConfig().DevelopmentEnv))
                                    _administrationBl.BuildJunctionVersionFileAndWriteToSqlServer(filename, folder);
                                    //else
                                    //    administrationBL.BuildJunctionVersionFileAndWriteInText(filename, folder);
                                }
                            }
                            _processWaitingManager.CloseProcessWaitingForm();
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                                Application.Exit();
                        }
                        break;
                    case "submExportEggedStations":
                        {
                            //processWaitingManager.ShowProcessWaitingForm();
                            _administrationBl.ExportEggedStations(ExportConfigurator.GetConfig().EggedExportSationFolder);
                            _processWaitingManager.CloseProcessWaitingForm();
                            //if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                            //    Application.Exit();
                        }
                        break;
                    case "submExportMapForDataComparing":
                        {
                            if (GuiHelper.ShowConfirmationMessage(ResourceGUI.AdminExportQuestions))
                            {
                                Refresh();
                                _administrationBl.ExportMapForCompare();
                                _processWaitingManager.CloseProcessWaitingForm();
                                if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                                    Application.Exit();
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            finally
            {
                _processWaitingManager.CloseProcessWaitingForm();
                Cursor = Cursors.Default;
                Activate();
            }
        }
    }

    public class SortInfo
    {
        public int LastColumnIndex { get; set; }
        public SortOrder LastSortOrder { get; set; }
    }
}
