using System;
using System.Collections.Generic;
using System.Linq;
using IBLManager;
using BLEntities.Model;
using Utilities;
using BLEntities.SQLServer ;
using DAL;
using BLEntities.Accessories;
using BLEntities.Entities;
using ExportConfiguration;
using BLManager.Resources;

namespace BLManager
{
    public class EgedExportDataSQLBLManager : IExportRouteBLManager
    {
        
        List<RouteLine> SelectedRouteLinesList;
        
        public EgedExportDataSQLBLManager(List<RouteLine> selectedRouteLinesList)
        {
            this.SelectedRouteLinesList = selectedRouteLinesList;
        }

        #region IExportBLManager Members

        private EgedExportRouteToSqldal dalEgedSQLExport = null;
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

        /// <summary>
        /// ExportData
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportData(ref string message)
        {
            bool result;
            var egedBlManager = new EgedBlManager();
            OnChanged(new ImportToSQLArgs
            {
                MaxProgressBarValue = 10,
                ProgressBarValue = 3,
                Message = BLManagerResource.BuildingShapeFileTranscadMacro
            });
            try
            {
                if (SelectedRouteLinesList.IsListFull())
                    SelectedRouteLinesList.ForEach(
                        rl =>
                        rl.ShapeFile = BLSharedUtils.GetShapeFileContent(new EgedBlManager().GetShapeFileFolder, rl.ID));
                else
                    throw new ApplicationException(BLManagerResource.RouteExportListIsEmpty);

                insertImportControlOperator.ImportFinishDate = insertImportControlOperator.ImportStartDate;
                insertImportControlOperator.Status = null;
                dalEgedSQLExport = new EgedExportRouteToSqldal(insertImportControlOperator);
                // Get insertImportControlOperator from DB and run Browser Command
                int importControlOperator = dalEgedSQLExport.GetAdddedInsertImportControlOperatorId();
                dalEgedSQLExport.Changed += DalSqlExportChanged;

                List<RouteStop> listRouteStops = (from p in GlobalData.RouteModel.RouteStopList
                                                  join o in SelectedRouteLinesList on p.RouteId equals o.ID
                                                  select p).ToList();

                result = dalEgedSQLExport.ExportRouteData(SelectedRouteLinesList, listRouteStops,
                                                               ExportConfigurator.GetConfig().
                                                                   PathToExportConstraintViolationFolder, ref message);
                if (result)
                {
                    ProcessLauncher.OpenExplorerWithURL(
                        string.Format(ExportConfigurator.GetConfig().TrafficLicensingUrl,
                                      importControlOperator));
                }
            }
            catch (Exception exp)
            {
                if (exp is OperationCanceledException)
                {
                    message = BLManagerResource.OperationCanceledByUser;
                    return false;
                }
                throw;
            }
            return result; 
       }



        private void DalSqlExportChanged(object sender, ImportToSQLArgs e)
        {
            OnChanged(e);
            if (IsCanceledByUser)
            {
                if (dalEgedSQLExport != null)
                {
                    dalEgedSQLExport.IsCanceledByUser = IsCanceledByUser;
                }
            }
        }

        ///// <summary>
        ///// Set Export Operator Data
        ///// </summary>
        //private InsertImportControlOperator SetExportOperatorData(ref string errorString)
        //{
           
        //    try{
        //        //string tvunaExportApplication = ExportConfiguration.ExportConfigurator.GetConfig().TvunaExportApplication;
        //        string tvunaExportApplication = ExportConfiguration.ExportConfigurator.GetConfig().TrafficLicensingURL;
               
        //        if (!IOHelper.IsFileExists(tvunaExportApplication))
        //            throw new ApplicationException(Resources.BLManagerResource.TvunaApplicationFileWasNotFound);
        //        Process p = new Process();
        //        StreamWriter sw;
        //        StreamReader sr;
        //        StreamReader err;
        //        ProcessStartInfo psI = new ProcessStartInfo(tvunaExportApplication);
        //        psI.Arguments = GlobalData.LoginUser.UserOperator.IdOperator.ToString();
        //        psI.UseShellExecute = false;
        //        psI.RedirectStandardInput = true;
        //        psI.RedirectStandardOutput = true;
        //        psI.RedirectStandardError = true;
        //        psI.CreateNoWindow = true;
        //        p.StartInfo = psI;
        //        p.Start();
        //        sw = p.StandardInput;
        //        sr = p.StandardOutput;
        //        err = p.StandardError;
        //        sw.AutoFlush = true;
        //        p.WaitForExit();
        //        p.Close();
        //        sw.Close();
        //        string resultFromCallable = sr.ReadToEnd();
        //        string errStr = err.ReadToEnd();
        //        if (!string.IsNullOrEmpty(resultFromCallable))
        //            //return JSONHelper.JsonDeSerialize<InsertImportControlOperator>(resultFromCallable);
        //            return JavaScriptSerializerHelper.DeSerialize<InsertImportControlOperator>(resultFromCallable);
        //        else
        //            return null;
        //    }
        //    catch(Exception exp)
        //    {
        //        Logger.LoggerManager.WriteToLog(exp);
        //        errorString = Resources.BLManagerResource.UnSuccesefullInputExport;
        //        return null;
        //    }
        //}

        
        #endregion

        public event EventHandler<ImportToSQLArgs> Changed;

        public bool IsCanceledByUser { get; set; }
       
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
                    ImportStartDate = DateTime.Now,
                    ImportFinishDate = DateTime.Today.Subtract(new TimeSpan(-24, 0, 0))
                   
                };
           return true; 

        }

    }
}
