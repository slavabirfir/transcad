using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Entities;
using Utilities;
using Logger;
using IDAL;
using DAL;
using BLEntities.Model;
using ExportConfiguration;
using System.IO;
using BLEntities.Accessories;

namespace BLManager
{
    public class ExportRouteDataBLManager : IExportRouteBLManager
    {
        #region IExportBLManager Members
        ITransCadBLManager transCadBLManager = null;
        IExportToTextDAL exportToTextDAL = null;
        ITransCadMunipulationDataDAL transCadFetchData = null;
        /// <summary>
        /// Get Route Back Folder Name
        /// </summary>
        /// <returns></returns>
        private string GetRouteBackFolderName()
        {
            DateTime minTime = DateTime.Now;
            string filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder, ExportConfigurator.GetConfig().kv_cars);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                ExportConfigurator.GetConfig().kv_maslul);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                ExportConfigurator.GetConfig().kv_mezahim);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                ExportConfigurator.GetConfig().kv_tachana);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            return minTime.ToString("ddMMyyyy hh_mm_ss");
        }
        /// <summary>
        /// Export Data
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportData(ref string message)
        {
            string folderName = GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder; 
            if (!IOHelper.IsFolderExists(folderName))
            {
                message = Resources.BLManagerResource.ExportFailed_FolderNotExixts;
                return false; 
            }
            try{
                BLSharedUtils.UpdateBeforeLoad();
                string setUnits = "Kilometers";
                transCadFetchData.SetMapUnits(setUnits, ExportConfigurator.GetConfig().RouteSystemLayerName);
                try
                {
                    GlobalData.RouteModel.RouteLineList.ForEach(delegate(RouteLine routeLine)
                    {
                        routeLine.Route_Len = transCadFetchData.CalculateRouteLine(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MapLayerName);
                        if (GlobalData.BaseTableEntityDictionary[GlobalConst.baseVarConverter] != null)
                        {
                            BaseTableEntity baseTableEntity = GlobalData.BaseTableEntityDictionary[GlobalConst.baseVarConverter].Find(be => be.Name == routeLine.Var);
                            if (baseTableEntity != null)
                            {
                                routeLine.VarNum = baseTableEntity.ID;
                            }
                            else
                            {
                                routeLine.VarNum = 0;// routeLine.Var;
                            }
                        }
                    });
                }
                catch { }
                
                string backUpFolder = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder, string.Concat("Routes_", GetRouteBackFolderName()));
                string file = string.Empty;  
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().kv_mezahim);
                ExportBackUp(file, backUpFolder);

                RouteLine[] localCopyArray = new RouteLine[GlobalData.RouteModel.RouteLineList.Count] ;
                GlobalData.RouteModel.RouteLineList.CopyTo(localCopyArray);
                List<RouteLine> rlList = localCopyArray.ToList<RouteLine>();
                rlList.ForEach(item =>
                    {
                        
                        //transCadBLManager.CalculateRouteLineStopsDuration(item);
                        item.TextExportCatalog = transCadBLManager.BuildExportCatalog(item); 
                    });
                //
                List<BaseTableEntity> AllClusters = null;
                if (GlobalData.RouteModel.RouteLineList.IsListFull())
                {
                    var result = (from p in GlobalData.RouteModel.RouteLineList
                                  where !string.IsNullOrEmpty(p.ClusterName)
                                  orderby p.RouteNumber
                                  select new BaseTableEntity { ID = p.IdCluster, Name = p.ClusterName });
                    AllClusters = result.Distinct(new BLManager.BLExportClusters.BaseTableEntityComparer()).ToList();
                }

                transCadBLManager.RunExportMacro("1=1", GlobalData.RouteModel.RouteLineList);
                //===== Makat
                exportToTextDAL.ExportRouteLines(rlList,
                   file, ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed, ExportConfiguration.ExportConfigurator.GetConfig().kv_mezahim, message); 
                    return false; 
                }

                //// == kv.Duration
                //file = IOHelper.CombinePath(
                //   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,ExportConfiguration.ExportConfigurator.GetConfig().kv_stop_Duration);
                //ExportBackUp(file, backUpFolder);

                //exportToTextDAL.ExportRouteStopDuration(rlList,
                //    GlobalData.RouteModel.RouteStopList,
                //   file, ref message);
                //if (!string.IsNullOrEmpty(message))
                //{
                //    message = string.Format(Resources.BLManagerResource.ExportProcessFailed,ExportConfiguration.ExportConfigurator.GetConfig().kv_stop_Duration, message);
                //    return false;
                //}


                //===== Kv_Car
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().kv_cars);
                ExportBackUp(file, backUpFolder);
                exportToTextDAL.ExportKvCars(rlList,
                    IOHelper.CombinePath(
                    GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                    ExportConfiguration.ExportConfigurator.GetConfig().kv_cars), ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed,
                        ExportConfiguration.ExportConfigurator.GetConfig().kv_cars, message);
                    return false;
                }
                //Kv_Tachana
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,ExportConfiguration.ExportConfigurator.GetConfig().kv_tachana);
                ExportBackUp(file, backUpFolder);

                exportToTextDAL.ExportRouteStops(rlList,
                    GlobalData.RouteModel.RouteStopList,
                   file, ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed,
                        ExportConfiguration.ExportConfigurator.GetConfig().kv_tachana, message);
                    return false;
                }

                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().kv_maslul);

                ExportBackUp(file, backUpFolder);
                //===== Kv_Maslul
                exportToTextDAL.ExportkvMaslul(rlList,
                    file,
                    ExportConfiguration.ExportConfigurator.GetConfig().RouteSystemLayerName,
                    ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName
                    , ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed,
                        ExportConfiguration.ExportConfigurator.GetConfig().kv_maslul, message);
                    return false;
                }
                
                //string shapeFile = string.Empty;  
                
                //shapeFile = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                //                                 ExportConfiguration.ExportConfigurator.GetConfig().RouteSystemLayerName + GlobalConst.ShapeExtentionSHP);

                //ExportShapeBackUp(IOHelper.CombinePath(
                //   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                //   ExportConfiguration.ExportConfigurator.GetConfig().RouteSystemLayerName), backUpFolder);

                //transCadFetchData.ExportFile(ExportConfiguration.ExportConfigurator.GetConfig().RouteSystemLayerName, shapeFile);

                //shapeFile = IOHelper.CombinePath(
                //   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                //   ExportConfiguration.ExportConfigurator.GetConfig().RouteStopsLayerName + GlobalConst.ShapeExtentionSHX);
                //ExportShapeBackUp(IOHelper.CombinePath(
                //   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                //   ExportConfiguration.ExportConfigurator.GetConfig().RouteStopsLayerName), backUpFolder);
                //transCadFetchData.ExportFile(ExportConfiguration.ExportConfigurator.GetConfig().RouteStopsLayerName, shapeFile);

                message = folderName;
                // Send Email 
                try
                {
                    if (ExportConfigurator.GetConfig().IsSendMail)
                    {
                       MailSender.SendMail(
                       string.Format(Resources.BLManagerResource.ExportMapEmailSubject, DateTime.Now.ToShortDateString(), GlobalData.LoginUser.UserOperator.OperatorName),
                       string.Format(Resources.BLManagerResource.ExportMapEmailBody, DateTime.Now.ToShortDateString(), GlobalData.LoginUser.UserOperator.OperatorName,
                       GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder, GlobalData.LoginUser.UserName, DateTime.Now.ToString()),

                       ExportConfigurator.GetConfig().MailSender,
                       ExportConfigurator.GetConfig().MailRecipientSuperviser,
                       ExportConfigurator.GetConfig().MailCredentialUser,
                       ExportConfigurator.GetConfig().MailCredentialPassword,
                       ExportConfigurator.GetConfig().MailServer,
                       ExportConfigurator.GetConfig().MailPort,
                       ExportConfigurator.GetConfig().MailEnableSsl,
                       ExportConfigurator.GetConfig().MailUseDefaultCredentials
                       );
                       if (!string.IsNullOrEmpty(ExportConfigurator.GetConfig().MailRecipientTvuna))
                       {
                           MailSender.SendMail(
                           string.Format(Resources.BLManagerResource.ExportMapEmailSubject, GlobalData.LoginUser.UserOperator.OperatorName),
                           string.Format(Resources.BLManagerResource.ExportMapEmailBody, GlobalData.LoginUser.UserOperator.OperatorName,
                           GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder, GlobalData.LoginUser.UserName, DateTime.Now.ToString()),
                           ExportConfigurator.GetConfig().MailSender,
                           ExportConfigurator.GetConfig().MailRecipientTvuna,
                           ExportConfigurator.GetConfig().MailCredentialUser,
                           ExportConfigurator.GetConfig().MailCredentialPassword,
                           ExportConfigurator.GetConfig().MailServer,
                           ExportConfigurator.GetConfig().MailPort,
                           ExportConfigurator.GetConfig().MailEnableSsl,
                           ExportConfigurator.GetConfig().MailUseDefaultCredentials);
                       }
                    }
                }
                catch (Exception exp)
                {
                    LoggerManager.WriteToLog(exp);
                    return true;
                }
                return true; 
 
            }
            catch(Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = Resources.BLManagerResource.ExportFailed_NotAllFilesCreated;
                return false; 
            }
            
        }
        /// <summary>
        /// Export Data BL Manager Constractor
        /// </summary>
        public ExportRouteDataBLManager()
        {
            transCadBLManager = new TransCadBLManager();
            exportToTextDAL = new ExportRouteToTextDAL();
            transCadFetchData = new TransCadMunipulationDataDAL();
        }
        /// <summary>
        /// Export Back Up
        /// </summary>
        private void ExportBackUp(string fileName, string backUpFolder)
        {
            if (IOHelper.IsFileExists(fileName))
            {
                if (!IOHelper.IsFolderExists(backUpFolder))
                {
                    IOHelper.CreateFolder(backUpFolder);
                }
                IOHelper.CopyFile(fileName, IOHelper.CombinePath(backUpFolder, Path.GetFileName(fileName)));
            }
        }
        /// <summary>
        /// Export Shape Back Up
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="backUpFolder"></param>
        private void ExportShapeBackUp(string fileName, string backUpFolder)
        {
            if (IOHelper.IsFileExists(fileName + GlobalConst.ShapeExtentionSHP) && 
                IOHelper.IsFileExists(fileName + GlobalConst.ShapeExtentionSHX))
            {
                if (!IOHelper.IsFolderExists(backUpFolder))
                {
                    IOHelper.CreateFolder(backUpFolder);
                }
                IOHelper.CopyFile(fileName + GlobalConst.ShapeExtentionSHP, IOHelper.CombinePath(backUpFolder, Path.GetFileName(fileName + GlobalConst.ShapeExtentionSHP)));
                IOHelper.CopyFile(fileName + GlobalConst.ShapeExtentionSHX, IOHelper.CombinePath(backUpFolder, Path.GetFileName(fileName + GlobalConst.ShapeExtentionSHX)));
            }
        }

        public event EventHandler DataPassed;
        public void DataPassedEvent()
        {
             if (DataPassed != null)
                 this.DataPassed(this, new EventArgs());
        }
        #endregion

        #region IExportRouteBLManager Members


        public event EventHandler<ImportToSQLArgs> Changed;

        #endregion

        #region IExportRouteBLManager Members


        public bool IsImportManagerFilled(ref string message)
        {
            return true;
        }

        #endregion



      
    }
}
