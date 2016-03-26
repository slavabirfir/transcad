using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using System.IO;
using BLEntities.Model;
using Utilities;
using BLEntities.SQLServer ;
using DAL;
using BLEntities.Accessories;
using BLEntities.Entities;
using ExportConfiguration;
using BLManager.Resources;
using System.Data;


namespace BLManager
{
    public class ExportDataSqlblManager : IExportRouteBLManager
    {
        private const string Illegalstopsduration = "IllegalStopsDuration.csv";

        //List<BaseTableEntity> SelectedCusters = null;
        readonly List<RouteLine> _selectedRouteLinesList;
        //public ExportDataSQLBLManager(List<BaseTableEntity> SelectedCusters)
        //{
        //    this.SelectedCusters = SelectedCusters;
        //}


        public ExportDataSqlblManager(List<RouteLine> selectedRouteLinesList)
        {
            _selectedRouteLinesList = selectedRouteLinesList;
        }

        #region IExportBLManager Members
       
        private ExportRouteToSqldal dalSQLExport = null;
        private InsertImportControlOperator insertImportControlOperator = null;
        /// <summary>
        /// On Changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(ImportToSQLArgs e)
        {
            if (this.Changed != null)
            {
                Changed(this, e);
                if (IsCanceledByUser)
                    throw new OperationCanceledException();
            }
        }




        public bool IsCanceledByUser { get; set; }

       /// <summary>
        /// Export Data
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportData(ref string message)
        {

            ITransCadBLManager transCadBLManager = new TransCadBlManager();
            bool result = false; 
            try
            {
                OnChanged(new ImportToSQLArgs
                              {
                                  MaxProgressBarValue = 10,
                                  ProgressBarValue = 3,
                                  Message = Resources.BLManagerResource.BuildingShapeFileTranscadMacro
                              });
                if (_selectedRouteLinesList.IsListFull())
                {

                    StringBuilder sb = new StringBuilder();
                    _selectedRouteLinesList.ForEach(rl =>
                                                       {
                                                           if (
                                                               sb.ToString().IndexOf(string.Format("IdCluster = {0}",
                                                                                                   rl.IdCluster)) == -1)
                                                               sb.AppendFormat("IdCluster = {0} OR ", rl.IdCluster);
                                                       });

                    string str = sb.ToString().Substring(0, sb.ToString().Length - 3);
                    transCadBLManager.RunExportMacro(str, _selectedRouteLinesList);
                }
                else
                    throw new ApplicationException(BLManagerResource.RouteExportListIsEmpty);
                // Get Last Duration Values
                transCadBLManager.RePopulateRouteStopList();
                // Check if exists al least one stop duration either null or o or next less then previously value
                List<RouteStop> listRouteStops = (from p in GlobalData.RouteModel.RouteStopList
                                                  join o in _selectedRouteLinesList on p.RouteId equals o.ID
                                                  select p).ToList();
                DataTable illegalStops = GetIllegalStops(listRouteStops);
                if (illegalStops.IsDataTableFull())
                {
                    string errorFileFullPath =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                                     Illegalstopsduration);
                        //GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                    BLSharedUtils.WriteCsvFile(errorFileFullPath, illegalStops);
                    ProcessLauncher.RunProcess("notepad.exe", "\"" + errorFileFullPath + "\"");
                    throw new ApplicationException(BLManagerResource.IllegalStopsFoundIntoTheExportCalculation);
                }


                //------------------------transCadBLManager.BuildModelData();
                insertImportControlOperator.ImportFinishDate = insertImportControlOperator.ImportStartDate;
                insertImportControlOperator.Status = null;
                dalSQLExport = new ExportRouteToSqldal(insertImportControlOperator);
                // Get insertImportControlOperator from DB and run Browser Command
                int importControlOperator = dalSQLExport.GetAdddedInsertImportControlOperatorId();

                dalSQLExport.Changed += new EventHandler<ImportToSQLArgs>(dalSQLExport_Changed);
                result = dalSQLExport.ExportRouteData(_selectedRouteLinesList, listRouteStops,
                                                           ExportConfigurator.GetConfig().
                                                               PathToExportConstraintViolationFolder, ref message);
                if (result)
                {
                    if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm)
                        ProcessLauncher.OpenExplorerWithURL(
                            string.Format(
                                ExportConfigurator.GetConfig().TrafficLicensingPlanningURL,
                                importControlOperator));
                    else
                        ProcessLauncher.OpenExplorerWithURL(
                            string.Format(ExportConfiguration.ExportConfigurator.GetConfig().TrafficLicensingUrl,
                                          importControlOperator));
                }
            }
            catch(Exception exp)
            {
                if (exp is OperationCanceledException)
                {
                    message = Resources.BLManagerResource.OperationCanceledByUser;
                    transCadBLManager.CloseConnection();
                    return false;
                }
                throw;
            }
            return result; 
       }
       /// <summary>
        /// Get Illegal Stops
       /// </summary>
       /// <returns></returns>
        private DataTable GetIllegalStops(List<RouteStop> listRouteStops)
       {
           DataTable dt = new DataTable();
           dt.Columns.Add(new DataColumn("TranscadLineId", typeof(int)));
           dt.Columns.Add(new DataColumn("TranscadStopId", typeof(int)));
           dt.Columns.Add(new DataColumn("TranscadPhysicalStop", typeof(string)));
           _selectedRouteLinesList.ForEach(rl => 
           {
               List<RouteStop> routeStopList = (from rs in listRouteStops
                                                   where rs.RouteId == rl.ID
                                                   orderby rs.Ordinal
                                                   select rs).ToList<RouteStop>();
                   if (routeStopList.IsListFull())
                   {
                       for (int i = 1; i < routeStopList.Count; i++)
                       {
                           if (routeStopList[i].Duration - routeStopList[i - 1].Duration <= 0)
                           {
                               DataRow dr = dt.NewRow();
                               dr[0] = routeStopList[i].RouteId;
                               dr[1] = routeStopList[i].ID;
                               dr[2] = routeStopList[i].PhysicalStopId;
                               dt.Rows.Add(dr);
                               dt.AcceptChanges();
                           }
                       }
                   }

           });
           return dt;
       }

       private void dalSQLExport_Changed(object sender, ImportToSQLArgs e)
       {
            OnChanged(e);
            if (IsCanceledByUser)
            {
                if (dalSQLExport!=null)
                {
                    dalSQLExport.IsCanceledByUser = IsCanceledByUser;
                }
            }
           
       }
      

        
        #endregion

        public event EventHandler<ImportToSQLArgs> Changed;



        /// <summary>
        /// Is Import Manager Filled
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsImportManagerFilled(ref string message)
        {
            insertImportControlOperator = new InsertImportControlOperator
                {
                    OperatorId = GlobalData.LoginUser.UserOperator.IdOperator,
                    ImportStartDate =DateTime.Now,
                    ImportFinishDate = DateTime.Today.Subtract(new TimeSpan(-24, 0, 0))
                    
                };
            // new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,0,0,0,0,DateTimeKind.Local);   
            return true; 

        }





        
    }
}
