using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using IDAL;
using DAL;
using BLEntities.Model;
using BLEntities.Entities;
using ExportConfiguration;
using Utilities;
using System.IO;
using Logger;
using BLEntities.Accessories;

namespace BLManager
{
    public class ExportBaseToTextBL : IExportBaseBLManager
    {
        IExportBaseDAL exportToTextDAL = new ExportBaseToTextDAL();
        ITransCadMunipulationDataDAL dalTranCad = new TransCadMunipulationDataDAL();
        #region IExportBaseBLManager Members

        

        /// <summary>
        /// Get Base Back Folder Name
        /// </summary>
        /// <returns></returns>
        private string GetBaseBackFolderName()
        {
          
            DateTime minTime = DateTime.Now;
            string filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder, ExportConfigurator.GetConfig().zg_ktaim);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                ExportConfigurator.GetConfig().zg_mokdim);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                ExportConfigurator.GetConfig().zg_tachana);
            if (IOHelper.IsFileExists(filePath))
            {
                DateTime creationTime = File.GetLastWriteTime(filePath);
                if (creationTime < minTime)
                {
                    minTime = creationTime;
                }
            }
            filePath = IOHelper.CombinePath(GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                ExportConfigurator.GetConfig().tv_yeshuv);
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
        /// Export Base Data
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool ExportBaseData(ref string message)
        {
            message = string.Empty;
            try
            {
                string folderName = GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder;
                if (!IOHelper.IsFolderExists(folderName))
                {
                    message = Resources.BLManagerResource.ExportFailed_FolderNotExixts;
                    return false;
                }

                string setUnits = "KiloMeters";
                dalTranCad.SetMapUnits(setUnits, ExportConfigurator.GetConfig().RouteSystemLayerName);

                string backUpFolder = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder, string.Concat("ZG_", GetBaseBackFolderName()));

                string file = string.Empty;
                //--------------------------- tv_yeshuvim -----------------------
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().tv_yeshuv);

                ExportBackUp(file, backUpFolder);
                List<City> cityList = dalTranCad.GetCities(ExportConfigurator.GetConfig().MunLayer);
                cityList = cityList.FindAll(c => !c.Code.Equals(string.Empty));

                exportToTextDAL.Exporttv_yeshuv(cityList,
                   file, ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed, ExportConfiguration.ExportConfigurator.GetConfig().kv_mezahim, message);
                    return false;
                }

                //--------------------------- zg_ktaim --------------------
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().zg_ktaim);

                ExportBackUp(file, backUpFolder);
                List<Link> linkList = dalTranCad.GetLinks(ExportConfigurator.GetConfig().MapLayerName,
                    ExportConfigurator.GetConfig().Endpoints, ExportConfigurator.GetConfig().MapLayerName);
                exportToTextDAL.Exportzg_ktaim(linkList, file, ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed, ExportConfiguration.ExportConfigurator.GetConfig().kv_mezahim, message);
                    return false;
                }


                //--------------------------- zg_mokdim
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().zg_mokdim);

                ExportBackUp(file, backUpFolder);

                List<Node> nodeList = dalTranCad.GetNodes(ExportConfigurator.GetConfig().Endpoints, ExportConfigurator.GetConfig().MunLayer);
                exportToTextDAL.Exportzg_mokdim(nodeList,
                   file, ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed, ExportConfiguration.ExportConfigurator.GetConfig().kv_mezahim, message);
                    return false;
                }
                //--------------------------- zg_tachana ---------------------
                file = IOHelper.CombinePath(
                   GlobalData.LoginUser.UserOperator.PathToRSTRouteLineExportFolder,
                   ExportConfiguration.ExportConfigurator.GetConfig().zg_tachana);

                ExportBackUp(file, backUpFolder);

                List<Stop> stopList = dalTranCad.GetStops(ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MunLayer, ExportConfigurator.GetConfig().MapLayerName);
                exportToTextDAL.Exportzg_tachana(stopList, file, ref message);
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format(Resources.BLManagerResource.ExportProcessFailed, ExportConfiguration.ExportConfigurator.GetConfig().kv_mezahim, message);
                    return false;
                }

                message = folderName;
                // Send Email 
                try
                {
                    if (ExportConfigurator.GetConfig().IsSendMail)
                    {
                        MailSender.SendMail(
                        string.Format(Resources.BLManagerResource.ExportBaseEmailSubject, DateTime.Now.ToShortDateString(), GlobalData.LoginUser.UserOperator.OperatorName),
                        string.Format(Resources.BLManagerResource.ExportBaseEmailBody, DateTime.Now.ToShortDateString(), GlobalData.LoginUser.UserOperator.OperatorName),

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
                            string.Format(Resources.BLManagerResource.ExportBaseEmailSubject, GlobalData.LoginUser.UserOperator.OperatorName),
                            string.Format(Resources.BLManagerResource.ExportBaseEmailBody, GlobalData.LoginUser.UserOperator.OperatorName,
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
                message = exp.Message;
                return false;
            }
        }

        /// <summary>
        /// On Changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(ImportToSQLArgs e)
        {
            if (this.Changed != null)
                Changed(this, e);
        }


        public event EventHandler<ImportToSQLArgs> Changed;

        #endregion
    }
}
