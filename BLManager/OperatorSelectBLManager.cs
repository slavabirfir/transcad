using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using BLEntities.Entities;
using BLManager.Resources;
using IBLManager;
using BLEntities.Accessories;
using IDAL;
using DAL;
using BLEntities.Model;
using System.Diagnostics;
using Utilities;
using System.IO;
using ExportConfiguration;
using System.Threading;
using System.Management;

namespace BLManager
{
    public class OperatorSelectBlManager : IOperatorSelectBlManager
    {
        readonly IInternalBaseDal _dalBaseSql = new TransportLicensingDal();
        private readonly IInternalBaseDal _internalBaseDal = new InternalTvunaImplementationDal();
        readonly ITransCadBLManager _transCadBlManager = new TransCadBlManager();
        public Operator SelectedOperator { get; set; }

        public OperatorSelectBlManager()
        {
            
        }
        public OperatorSelectBlManager(Operator oper)
        {
            SelectedOperator = oper;
        }

        #region IOperatorSelectBLManager Members
        /// <summary>
        /// Get Operator List
        /// </summary>
        /// <returns></returns>
        public List<Operator> GetOperatorList()
        {
            return GlobalData.OperatorList;
        }
        /// <summary>
        /// Is Show Update Operator Attribute Button
        /// </summary>
        /// <returns></returns>
        public bool IsShowUpdateOperatorAttributeButton()
        {
            return GlobalData.LoginUser.IsSuperViser;
        }

        public List<BaseTableEntity> ClustersByOperator { get; set; }

        public static string GetProcessOwnerByCommandLine(string processValue)
        {
            var query = "Select * from Win32_Process Where CommandLine Like '%" + processValue + "%'";

            var searcher = new ManagementObjectSearcher(query);
            var processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                object[] argList = { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    var owner = argList[1] + "\\" + argList[0];
                    return owner;
                }
            }

            return string.Empty;
        }


        //static void Main(string[] args)
        //{
        //    var lst = GetProcessList();
        //    if (lst != null && lst.Any())
        //    {
        //        lst.ForEach(e =>
        //        {
        //            var userName = GetProcessOwnerByCommandLine(e.Replace("\\", "%"));
        //            if (!string.IsNullOrEmpty(userName))
        //            {
        //                userName = GetLogin(userName);
        //                Console.WriteLine("Workspace {0} is launched by {1}", e, userName);
        //            }
        //        });
        //    }
        //    Console.ReadKey();
        //}

        public static string GetLogin(string s)
        {
            var stop = s.IndexOf("\\", StringComparison.Ordinal);
            return (stop > -1) ? s.Substring(stop + 1, s.Length - stop - 1) : null;
        }


        private static List<string> GetProcessList()
        {
            if (!File.Exists(ConfigurationManager.AppSettings["OperatorList"]))
            {
                throw new ApplicationException(string.Format("The OperatorList {0} was not found. Please check your config file", ConfigurationManager.AppSettings["OperatorList"]));
            }
            var doc = XDocument.Load(ConfigurationManager.AppSettings["OperatorList"]);
            var elems = (from p in doc.Descendants("PathToRSTWorkSpace")
                         select p).ToList();
            if (elems.Any())
            {
                var lst = new List<string>();
                elems.ForEach(e => lst.Add(e.Value));
                return lst;
            }
            return null;
        }

        public void SetSelectedTranscadClusterConfig(Operator operatorEntity,BaseTableEntity selectedCluster)
        {
            if (operatorEntity != null && operatorEntity.ListOfTranscadClusterConfig != null && operatorEntity.ListOfTranscadClusterConfig.Any() && selectedCluster != null)
            {
                operatorEntity.SelectedTranscadClusterConfig = operatorEntity.ListOfTranscadClusterConfig.Where(e => e.ClusterId == selectedCluster.ID).FirstOrDefault();
            }
        }

        public bool SetSelectedTranscadClusterConfigAndTestClustersByOperatorIsListFull(Operator operatorEntity)
        {
            ClustersByOperator = new List<BaseTableEntity>();
            if (operatorEntity == null || !operatorEntity.ListOfTranscadClusterConfig.IsListFull())
                return false; 
            IInternalBaseDal internalBaseDal = new InternalTvunaImplementationDal();
            var dbClusters = internalBaseDal.GetClustersByOpearatorId(operatorEntity.IdOperator);
            if (!dbClusters.IsListFull())
                throw new ApplicationException(string.Format(BLManagerResource.TheOperDoesntHaveClusters, operatorEntity.OperatorName));

            var mainCluster = operatorEntity.ListOfTranscadClusterConfig.FirstOrDefault(c => c.ClusterId == -1);
            if (mainCluster!=null)
            {
                SetSelectedTranscadClusterConfig(operatorEntity, new BaseTableEntity{ID = -1,Name  = "*"});
                return false;
            }
            if (dbClusters.Count == 1)
            {
                SetSelectedTranscadClusterConfig(operatorEntity, dbClusters.First());
                return false;
            }
            
            dbClusters.ForEach(clusDb =>
            {
             if (operatorEntity.ListOfTranscadClusterConfig.Exists(c=>c.ClusterId== clusDb.ID))
                ClustersByOperator.Add(new BaseTableEntity{ ID = clusDb.ID, Name = clusDb.Name });
            });
              
            if (operatorEntity.ListOfTranscadClusterConfig.IsListFull())
                operatorEntity.ListOfTranscadClusterConfig.ForEach(cl =>
               {
                   if (!dbClusters.Exists(c => c.ID == cl.ClusterId))
                       throw new ApplicationException(string.Format(BLManagerResource.OperatorHasClusterIdInXMLThatNotFoundInDB, cl.ClusterId)); 
            });

            if (ClustersByOperator.IsListFull())
            {
                if (operatorEntity.SelectedTranscadClusterConfig == null)
                {
                    SetSelectedTranscadClusterConfig(operatorEntity, ClustersByOperator.First());
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Get Active Directory Groups OperatorList
        /// </summary>
        /// <returns></returns>
        public List<Operator> GetActiveDirectoryGroupsOperatorList()
        {
            return GlobalData.LoginUser.ActiveDirectotyOperatorList; 
        }

        /// <summary>
        /// Set Active Operator
        /// </summary>
        public void SetActiveOperator(Operator oper)
        {
            GlobalData.LoginUser.UserOperator.IdOperator = oper.IdOperator;
            _transCadBlManager.SetUserOperator(GlobalData.LoginUser, oper.OperatorName);
        }


        private string GetProcessOwner(string txtProcessName)
        {

            var processName = txtProcessName;
            var processOwner = "";
            var x = new ObjectQuery("Select * From Win32_Process where Name='" + processName + "'");
            var mos = new ManagementObjectSearcher(x);
            foreach (ManagementObject mo in mos.Get())
            {
                var s = new string[2];
                mo.InvokeMethod("GetOwner", s);
                processOwner = s[0];
                break;
            }

            return processOwner;
        }


        /// <summary>
        /// Open Transcad By WSPath
        /// </summary>
        /// <param name="workSpacePath"></param>
        /// <param name="message"></param>
        //public bool OpenTranscadByWSPath(string workSpacePath, ref string message)
        //{
        //    if (string.IsNullOrEmpty(workSpacePath) || !IOHelper.IsFileExists(workSpacePath))
        //    {
        //        message = Resources.BLManagerResource.OperatorWorkSpaceNotDefined;
        //        return false;
        //    }
        //    string processName = "tcw.exe";
        //    string processOwner = GetProcessOwner(processName);
        //    if (Environment.UserName == processOwner)
        //    {
        //        Logger.LoggerManager.WriteToLog(string.Format("User {0} opened before Transcad", Environment.UserName));
        //        throw new ApplicationException(Resources.BLManagerResource.CloseTranscadAndRunApplicationAgain);
        //    }

        //    ProcessStartInfo info = new ProcessStartInfo(ExportConfigurator.GetConfig().Transcadexe, workSpacePath);
        //    info.CreateNoWindow = true;
        //    info.UseShellExecute = false;
        //    Process p = new Process();
        //    p.StartInfo = info;
        //    p.Start();
        //    Thread.Sleep(11000);
        //    return true;


        //    //ProcessStartInfo f = new ProcessStartInfo(ExportConfigurator.GetConfig().Transcadexe); // C:\Program Files\TransCAD\tcw.exe @"C:\Program Files (x86)\TransCAD\tcw.exe"
        //    //f.Arguments = workSpacePath;
        //    //// Pass the ProcessStartInfo object to the Start function. 
        //    //System.Diagnostics.Process.Start(f);
        //    //Thread.Sleep(5000); // to get time to run up to the transcad process
        //    //return true;



        //    ////ProcessStartInfo f = new ProcessStartInfo(workSpacePath);
        //    ////Logger.LoggerManager.WriteToLog("workSpacePath is " + workSpacePath);
        //    ////try
        //    ////{
        //    ////    // Pass the ProcessStartInfo object to the Start function. 
        //    ////    System.Diagnostics.Process.Start(f);
        //    ////    Thread.Sleep(5000); // to get time to run up to the transcad process
        //    ////    return true;
        //    ////}
        //    ////catch (Exception ex)
        //    ////{
        //    ////    System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    ////    return false;
        //    ////}


        //    //ITransCadMunipulationDataDAL transcadDAL = new TransCadMunipulationDataDAL();
        //    ////if (transCadBLManager.IsEgedOperator && transcadDAL.IsEgedWorkSpaceInTranscad())
        //    ////    return true;
        //    //if (transCadBLManager.IsEgedOperator)
        //    //{
        //    //    var tcw = RegisterHelper.GetRegisterValue(@"HKEY_CLASSES_ROOT\Caliper.CDF\Shell\Open\Command", string.Empty, string.Empty);
        //    //    if (string.IsNullOrEmpty(tcw))    
        //    //        tcw = @"C:\Program Files\TransCAD\tcw.exe -q";
        //    //    else
        //    //        tcw = tcw.Replace(" -q", string.Empty).Replace("\"", string.Empty);
        //    //    try
        //    //    {
        //    //        ProcessStartInfo f = new ProcessStartInfo(tcw);
        //    //        try
        //    //        {
        //    //            // Pass the ProcessStartInfo object to the Start function. 
        //    //            System.Diagnostics.Process.Start(workSpacePath);
        //    //            //transcadDAL.OpenWorkspace(workSpacePath);
        //    //            return true;
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    //            return false;
        //    //        }


        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    //        return false;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ProcessStartInfo f = new ProcessStartInfo(workSpacePath);
        //    //    Logger.LoggerManager.WriteToLog("workSpacePath is " + workSpacePath);
        //    //    try 
        //    //    {
        //    //        // Pass the ProcessStartInfo object to the Start function. 
        //    //        System.Diagnostics.Process.Start(f);
        //    //        return true;
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    //        return false;
        //    //    }
        //    //}

        //}

        public bool OpenTranscadByWsPath(string workSpacePath, ref string message)
        {
            if (string.IsNullOrEmpty(workSpacePath) || !IoHelper.IsFileExists(workSpacePath))
            {
                message = BLManagerResource.OperatorWorkSpaceNotDefined;
                return false;
            }
            
            var userName = _internalBaseDal.TranscadLoginIsFileOpened(workSpacePath);
            
            if (!string.IsNullOrEmpty(userName))
            {
                message =string.Format(BLManagerResource.WorSpaceAlreadyOccupiedByOtherUser,workSpacePath,GlobalData.LoginUser.UserName);
                return false;
            }
            
            var info = new ProcessStartInfo(ExportConfigurator.GetConfig().Transcadexe, workSpacePath)
                           {CreateNoWindow = true,UseShellExecute = false};
            new Process() {StartInfo = info}.Start();//Thread.Sleep(8000);
            var manualResetEvent = new ManualResetEvent(false);
            manualResetEvent.WaitOne(7000);
            
            if (GlobalData.LoginUser.IsSuperViser)
            {
                _internalBaseDal.TranscadLoginDelete(GlobalData.LoginUser.UserName);
            }
            
            _internalBaseDal.TranscadLoginInsert(new TranscadLogin
            {
                ClusterName = GetSelectedClusterName(),
                LoginDate = DateTime.Now,
                OperatorName = GlobalData.LoginUser.UserOperator.OperatorName,
                UserName = GlobalData.LoginUser.UserName,
                WorkspaceFile = workSpacePath
            });
            return true;


            //ProcessStartInfo f = new ProcessStartInfo(ExportConfigurator.GetConfig().Transcadexe); // C:\Program Files\TransCAD\tcw.exe @"C:\Program Files (x86)\TransCAD\tcw.exe"
            //f.Arguments = workSpacePath;
            //// Pass the ProcessStartInfo object to the Start function. 
            //System.Diagnostics.Process.Start(f);
            //Thread.Sleep(5000); // to get time to run up to the transcad process
            //return true;



            ////ProcessStartInfo f = new ProcessStartInfo(workSpacePath);
            ////Logger.LoggerManager.WriteToLog("workSpacePath is " + workSpacePath);
            ////try
            ////{
            ////    // Pass the ProcessStartInfo object to the Start function. 
            ////    System.Diagnostics.Process.Start(f);
            ////    Thread.Sleep(5000); // to get time to run up to the transcad process
            ////    return true;
            ////}
            ////catch (Exception ex)
            ////{
            ////    System.Diagnostics.Debug.WriteLine(ex.ToString());
            ////    return false;
            ////}


            //ITransCadMunipulationDataDAL transcadDAL = new TransCadMunipulationDataDAL();
            ////if (transCadBLManager.IsEgedOperator && transcadDAL.IsEgedWorkSpaceInTranscad())
            ////    return true;
            //if (transCadBLManager.IsEgedOperator)
            //{
            //    var tcw = RegisterHelper.GetRegisterValue(@"HKEY_CLASSES_ROOT\Caliper.CDF\Shell\Open\Command", string.Empty, string.Empty);
            //    if (string.IsNullOrEmpty(tcw))    
            //        tcw = @"C:\Program Files\TransCAD\tcw.exe -q";
            //    else
            //        tcw = tcw.Replace(" -q", string.Empty).Replace("\"", string.Empty);
            //    try
            //    {
            //        ProcessStartInfo f = new ProcessStartInfo(tcw);
            //        try
            //        {
            //            // Pass the ProcessStartInfo object to the Start function. 
            //            System.Diagnostics.Process.Start(workSpacePath);
            //            //transcadDAL.OpenWorkspace(workSpacePath);
            //            return true;
            //        }
            //        catch (Exception ex)
            //        {
            //            System.Diagnostics.Debug.WriteLine(ex.ToString());
            //            return false;
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        System.Diagnostics.Debug.WriteLine(ex.ToString());
            //        return false;
            //    }
            //}
            //else
            //{
            //    ProcessStartInfo f = new ProcessStartInfo(workSpacePath);
            //    Logger.LoggerManager.WriteToLog("workSpacePath is " + workSpacePath);
            //    try 
            //    {
            //        // Pass the ProcessStartInfo object to the Start function. 
            //        System.Diagnostics.Process.Start(f);
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Diagnostics.Debug.WriteLine(ex.ToString());
            //        return false;
            //    }
            //}

        }

        private string GetSelectedClusterName()
        {
            if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig == null)
                return BLManagerResource.All;
            var clustres = _internalBaseDal.GetClustersByOpearatorId(GlobalData.LoginUser.UserOperator.IdOperator);
            var cluster = clustres.SingleOrDefault(e => e.ID == GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.ClusterId);
            if (cluster != null)
                return cluster.Name;
            return string.Empty;

        }

        /// <summary>
        /// Update Operator
        /// </summary>
        /// <param name="operatorEntity"></param>
        /// <returns></returns>
        public bool UpdateOperator(Operator operatorEntity)
        {
            return _dalBaseSql.UpdateOperator(operatorEntity);
           
        }

        /// <summary>
        /// IsConstraintViolationFolderFull
        /// </summary>
        /// <returns></returns>
        public bool IsConstraintViolationFolderFull()
        {
            var filePaths = Directory.GetFiles(ExportConfigurator.GetConfig().PathToExportConstraintViolationFolder, "EXPORT_CONSTRAINT_*.csv");
            return filePaths.IsListFull() && !ExportConfigurator.GetConfig().IsAuthenticationserviceActive;
        }
        /// <summary>
        /// ShowConstraintViolationFolder
        /// </summary>
        public void ShowConstraintViolationFolder()
        {
            ProcessLauncher.RunProcess("explorer", ExportConfigurator.GetConfig().PathToExportConstraintViolationFolder);
        }

        #endregion
    }
}
