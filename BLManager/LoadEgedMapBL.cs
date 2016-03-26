using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Utilities;
using Logger;
using System.Data.SqlClient;
using DAL.SQLServerDAL;
using IDAL;
using DAL;
using ExportConfiguration;
using System.Data;
using BLManager.Resources;
using BLEntities.Entities;
using BLEntities.Model;

namespace BLManager
{
    /// <summary>
    /// Load map eged BL
    /// </summary>
    public class LoadEgedMapBL
    {
        /// <summary>
        /// Private const
        /// </summary>
        private const string Linedetailimport = "lineDetailImportTranscad.txt";

        private const string Linedetailstation = "lineDetailStationTranscad.txt";
        private const string Nispah3Layer = "Nispah3";
        private const string Csv = ".csv";
        private string _tabMif = "Maslulim.TAB";
        private const string Nispah1Table = "dbo.Nispah1";
        private const string Nispah2Table = "dbo.LineNodeRaw";
        private const string Nispah3Table = "dbo.Nispah3";
        private const string Egedcoordinatefile = "dataEged.txt";
        private const string Stationfarawayfromroute = "StationFarAwayFromRoute.csv";
        private const string Egederrorfile = "egedErrors.csv";
        readonly ITransCadMunipulationDataDAL _transcadDal = new TransCadMunipulationDataDAL();
        private const string Dateformat = "dd_MM_yyyy HH_mm_ss";
        private const string Transcadoperatorsconnectionstringkey = "TranscadOperators";
        private readonly EgedBulkInsertDAL _dal = new EgedBulkInsertDAL();
        public String LoaderFolder { get; set; }
        private string _backUpFolder;


        private bool IsFarAwayStationsFromRoutesFull(String errorStationFarAwayFileName, SqlConnection connection, SqlTransaction transaction)
        {
            var nispah3View = _dal.GetNispah3View(connection, transaction);
            var fileName = Path.Combine(LoaderFolder, String.Concat(Nispah3Layer, Csv));
            BLSharedUtils.WriteCsvFile(fileName, nispah3View);
            IoHelper.DeleteFile(errorStationFarAwayFileName);
            int result = Convert.ToInt32(_transcadDal.RunTranscadMacro(ExportConfigurator.GetConfig().EgedFarAwayStationMacro, ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroFile,
                 new List<object> { fileName, errorStationFarAwayFileName }));
            IoHelper.DeleteFiles(LoaderFolder, string.Concat(Nispah3Layer, "*.*"), false);
            return result > 0;

        }
        /// <summary>
        /// LoadMap
        /// </summary>
        public void LoadMap()
        {
            var shapeFileFolder = new EgedBlManager().GetShapeFileFolder;

            //throw new ApplicationException("Found corrupted data in the source files, please contact your administrator");

            VerifyFileLoadSet();
            try
            {
                BackUpPreviouslyLodedFiles(shapeFileFolder);
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
            }
            //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap Started");
            if (IoHelper.IsFolderExists(shapeFileFolder))
                Directory.Delete(shapeFileFolder, true);
            IoHelper.CreateFolder(shapeFileFolder);
            //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap RunTranscadMacro Before");
            _transcadDal.RunTranscadMacro(ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroName, ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroFile,
                 new List<object> { Path.Combine(_backUpFolder, _tabMif), Path.Combine(_backUpFolder, Egedcoordinatefile), shapeFileFolder, GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PathToRSTWorkSpace });
            //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap RunTranscadMacro After");
            string connectionString = ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey);
            var connection = new SqlConnection(connectionString);
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap BeginTransaction");
                _dal.BulkInsert(Path.Combine(_backUpFolder, Egedcoordinatefile), Nispah2Table, connection, transaction);
                //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap EGEDCOORDINATEFILE");
                _dal.BulkInsert(Path.Combine(_backUpFolder, Linedetailstation), Nispah3Table, connection, transaction);
                //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap LINEDETAILSTATION");
                _dal.BulkInsert(Path.Combine(_backUpFolder, Linedetailimport), Nispah1Table, connection, transaction);
                //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap LINEDETAILIMPORT");
                DataTable dataErrors = _dal.GetBulkInsertErrors(connection, transaction);
                //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap GetBulkInsertErrors");
                string fileNameStati0NFarAwayLog = Path.Combine(LoaderFolder, Stationfarawayfromroute);
                bool isErrorInTheProcess = false; // Will be changed
                //if (IsFarAwayStationsFromRoutesFull(fileNameStati0nFarAwayLog,connection, transaction))
                //{
                //    isErrorInTheProcess = true;
                //    ProcessLauncher.RunProcess("notepad.exe", "\"" + fileNameStati0nFarAwayLog + "\"");
                //}
                if (dataErrors.IsDataTableFull())
                {
                    //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap IsDataTableFull");
                    isErrorInTheProcess = true;
                    string errorFileFullPath = Path.Combine(LoaderFolder, Egederrorfile);
                    BLSharedUtils.WriteCsvFile(errorFileFullPath, dataErrors);
                    ProcessLauncher.RunProcess("notepad.exe", "\"" + errorFileFullPath + "\"");
                }

                if (isErrorInTheProcess)
                {
                    throw new ApplicationException(BLManagerResource.IntegrityProbleInEgedLoad);
                }
                transaction.Commit();
                //LoggerManager.WriteToLog("^^^^^^^^^^^^ Eged LoadMap transaction.Commit() OKAY");
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                try
                {
                    if (IoHelper.IsFolderExists(_backUpFolder))
                    {
                        IoHelper.DeleteFolder(shapeFileFolder, true);
                        IoHelper.CopyDirectory(Path.Combine(_backUpFolder, GlobalData.LoginUser.UserOperator.IdOperator.ToString()), shapeFileFolder, true);
                        IoHelper.DeleteFolder(_backUpFolder, true);
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        if (transaction != null) transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                    LoggerManager.WriteToLog(e);
                }
                throw;

            }
            finally
            {

                connection.Close();
                connection.Dispose();
            }
        }


        /// <summary>
        /// VerifyFileLoadSet
        /// </summary>
        private void VerifyFileLoadSet()
        {
            if (!IoHelper.IsFolderExists(LoaderFolder))
                throw new ApplicationException(Resources.BLManagerResource.EgedLoadFolderDoesNotExists);
            if (!IoHelper.IsFileExists(Path.Combine(LoaderFolder, Linedetailimport)))
                throw new ApplicationException(string.Format(Resources.BLManagerResource.EgedLoadFailedBcauseFileWasNotFound, Linedetailimport));
            if (!IoHelper.IsFileExists(Path.Combine(LoaderFolder, Linedetailstation)))
                throw new ApplicationException(string.Format(Resources.BLManagerResource.EgedLoadFailedBcauseFileWasNotFound, Linedetailstation));
            string[] filePaths = Directory.GetFiles(LoaderFolder, _tabMif);
            if (filePaths != null && filePaths.Length == 1)
            {
                _tabMif = Path.GetFileName(filePaths[0]);
            }
            else
                throw new ApplicationException(string.Format(Resources.BLManagerResource.EgedLoadFailedBcauseFileWasNotFound, _tabMif));

        }
        /// <summary>
        /// BackUpPreviouslyLodedFiles
        /// </summary>
        private void BackUpPreviouslyLodedFiles(String shapeFileFolder)
        {
            //if (!IOHelper.IsFolderExists(EGEDHISTORYMAP))
            //    IOHelper.CreateFolder(EGEDHISTORYMAP);
            _backUpFolder = Path.Combine(LoaderFolder, DateTime.Now.ToString(Dateformat));
            IoHelper.CreateFolder(_backUpFolder);
            IoHelper.CopyFile(Path.Combine(LoaderFolder, Linedetailimport), Path.Combine(_backUpFolder, Linedetailimport));
            IoHelper.CopyFile(Path.Combine(LoaderFolder, Linedetailstation), Path.Combine(_backUpFolder, Linedetailstation));
            IoHelper.CopyFile(Path.Combine(LoaderFolder, _tabMif), Path.Combine(_backUpFolder, _tabMif));
            IoHelper.CopyDirectory(shapeFileFolder, Path.Combine(_backUpFolder, GlobalData.LoginUser.UserOperator.IdOperator.ToString()), true);
        }
        /// <summary>
        /// GetListEndPointsFromFile
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static List<EndPoint> GetListEndPointsFromFile(string fileName)
        {

            using (var file = new StreamReader(fileName))
            {
                string line;
                var lst = new List<EndPoint>();
                while ((line = file.ReadLine()) != null)
                {
                    string[] arr = line.Split(",".ToCharArray());
                    lst.Add(new EndPoint
                    {
                        Longitude = int.Parse(arr[0]),
                        Latitude = int.Parse(arr[1]),
                        NoteID = int.Parse(arr[2]),
                        ID = int.Parse(arr[3])
                    });
                }
                return lst;
            }

        }


        public void LoadEndPointsFromTranscadAndPhysicalStopsForEgedLoadMapProcess()
        {
            ITransCadMunipulationDataDAL dalTranscad = new TransCadMunipulationDataDAL();
            string fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, "Endpoints.txt");
            dalTranscad.ExportEndPointsAsCsvFile(fileName);
            var listEndPoint = GetListEndPointsFromFile(fileName);
            var dicPhysicalStop = dalTranscad.GetPhysicalStopsRawDataDictionary();

            if (!dicPhysicalStop.IsDictionaryFull())
            {
                throw new ApplicationException(BLManagerResource.TranscadPhysicalDBListEmpty);
            }
            var listPhysicalStops = dicPhysicalStop.Values.ToList();
            listPhysicalStops = BLSharedUtils.FilterTrainPhysicalStops(listPhysicalStops);
            if (!listPhysicalStops.IsListFull())
            {
                throw new ApplicationException("Check your transcad environment : the physical stop layer is empty");
            }
            if (!listEndPoint.IsListFull())
            {
                throw new ApplicationException("Check your transcad environment : the End Point layer is empty");
            }
            var connection = new SqlConnection(ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey));
            SqlTransaction transaction = null;
            try
            {
                var egedSqlServerDal = new EgedSQLServerDAL();
                connection.Open();
                transaction = connection.BeginTransaction();
                egedSqlServerDal.DeleteAllEndPoints(connection, transaction);
                listEndPoint.ForEach(endPoint => egedSqlServerDal.InsertEndPoint(endPoint, connection, transaction));
                egedSqlServerDal.DeleteAllStation(connection, transaction);
                listPhysicalStops.ForEach(ps => egedSqlServerDal.InsertStation(ps, connection, transaction));
                transaction.Commit();
                // ??????
                //EgedBLManager egedBLManager = new EgedBLManager();
                //egedBLManager.BuildModelData();
            }

            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                if (transaction != null) transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
