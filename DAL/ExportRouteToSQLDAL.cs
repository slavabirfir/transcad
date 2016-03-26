using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using BLEntities.Accessories;
using BLEntities.SQLServer ;
using DAL.SQLServerDAL;
using Utilities;
using System.Data.SqlClient;
using BLEntities.Entities;
using ExportConfiguration;
using System.Data;
using BLEntities.Model;
using Logger;
using System.IO;

namespace DAL
{
    public class ExportRouteToSqldal 
    {
        #region const
        public const int DEFAULTAREA = -1;
        #endregion
        public event EventHandler<ImportToSQLArgs> Changed;
        private readonly IImportIDAL<ShapeOfJunctionDetailsImport> _dalShapeOfJunctionDetailsImport = new ShapeOfJunctionDetailsImportDal();
        private readonly IImportIDAL<LineDetailImport> _dalRouteImport = new LineDetailImportDAL();
        //private readonly IImportIDAL<CarImport> dalCarImport = new CarImportDAL();
        private readonly IImportIDAL<LineDetailStationImport> _dalRouteStopImport = new LineDetailStationImportDAL();
        private readonly IImportIDAL<LineDetailJunctionImport> _dalRouteJunctionImport = new LineDetailJunctionImportDAL();
        /// <summary>
        /// On Changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(ImportToSQLArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
                if (IsCanceledByUser)
                    throw new OperationCanceledException();
            }
        }

        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false);
            }
        }


        public string ConnectionStringReadOnly
        {
            get
            {
                return DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup, true);
            }
        }

        private readonly InsertImportControlOperatorDAL _insertImportControlOperatorDal = new InsertImportControlOperatorDAL();
        private SqlConnection _connection;
        //private SqlTransaction transaction = null;
        private readonly ITransCadMunipulationDataDAL _transCadMunipulationDataDal = new TransCadMunipulationDataDAL();

        private readonly InsertImportControlOperator _insertImportControlOperatorInner;


        #region IExportRouteToSQLDAL Members

        public ExportRouteToSqldal(InsertImportControlOperator insertImportControlOperator)
        {
            _insertImportControlOperatorInner = insertImportControlOperator;
        }
        public bool IsCanceledByUser { get; set; }

        /// <summary>
        /// Export Route Data
        /// </summary>
        /// <param name="lstRouteLines"></param>
        /// <param name="pathToExportConstraintViolationFolder"></param>
        /// <param name="message"></param>
        /// <param name="listRouteStops"></param>
        /// <returns></returns>
        public bool ExportRouteData(List<RouteLine> lstRouteLines, List<RouteStop> listRouteStops,string pathToExportConstraintViolationFolder, ref string message)
        {
            if (pathToExportConstraintViolationFolder == null)
                throw new ArgumentNullException("pathToExportConstraintViolationFolder");
            var routeImports = new List<LineDetailImport>();
            //List<CarImport> carImports = new List<CarImport>();
            var routeStopImports = new List<LineDetailStationImport>();
            var routeJunctionImports = new List<LineDetailJunctionImport>();
            int counter  =0;
            string nispa2CoordsFolder;
            //if (Convert.ToBoolean(ExportConfigurator.GetConfig().DevelopmentEnv))
            //{
                nispa2CoordsFolder = DalShared.GetNispah2CoordsFolder();
            //}
            lstRouteLines.ForEach(routeLine =>
            {
                var coorList = DalShared.GetShapeGeomteryCoords(routeLine.ID);
                OnChanged(new ImportToSQLArgs
                {
                    MaxProgressBarValue = lstRouteLines.Count,
                    ProgressBarValue = counter++,
                    Message = Resources.ResourceDAL.MessageProgressRouteSystemBuild
                });


                routeImports.Add(new LineDetailImport
                    {
                        ImportControlId = _insertImportControlOperatorInner.ImportControlId,
                        OperatorId = routeLine.IdOperator,
                        ClusterId = routeLine.IdCluster,
                        OfficeLineId =int.Parse(routeLine.Catalog.ToString()),
                        LineSign = routeLine.Signpost,
                        Direction = (byte)routeLine.Dir,
                        LineAlternative = routeLine.Var,
                        ServiceType = routeLine.IdServiceType,
                        LineDetailType = 1,// Urban line
                        Line =(int) routeLine.RouteNumber,
                        DistrictId = routeLine.IdZoneHead,     //routeLine.IdZoneHead, ExportRouteToSQLDAL.DEFAULTAREA, 
                        DistrictSecId = routeLine.IdZoneSubHead,   //routeLine.IdZoneSubHead, ExportRouteToSQLDAL.DEFAULTAREA
                        IsBase = routeLine.IsBase,
                        IdExclusivityLine = 0,// routeLine.IdExclusivityLine, Changed on the 01/03/2012 according Tamar request
                        ConfirmedForSaturday = routeLine.ConfirmedForSaturday,
                        Shape = routeLine.ShapeFile,
                        ShapeGeomteryCoords = coorList,
                        Accessibility = routeLine.Accessibility,
                        AccountingGroupId = routeLine.AccountingGroupID
                       
                    }
                );
                //carImports.Add(new CarImport
                //{
                //    ImportControlId = insertImportControlOperatorInner.ImportControlId,
                //    IdOperator = routeLine.IdOperator,
                //    IdCluster = routeLine.IdCluster,
                //    Catalog = routeLine.Catalog.ToString().ToFixLength(5),
                //    Signpost = routeLine.Signpost.ToFixLength(5),
                //    Direction = (byte)routeLine.Dir,
                //    VarNum = routeLine.VarNum,
                //    IdVehicleType = 2,
                //    IdVehicleSize = 1,
                //});
                List<RouteStop> orderedList = (from p in listRouteStops orderby p.Ordinal where p.RouteId == routeLine.ID select p).ToList<RouteStop>();
                if (orderedList.IsListFull())
                {
                    int distPrev = 0, distStart = 0;
                    
                    for (int i = 0; i < orderedList.Count; i++)
                    {
                        var it = orderedList[i];
                        if (i > 0)
                        {
                            distPrev = Convert.ToInt32(orderedList[i].MilepostRounded - orderedList[i - 1].MilepostRounded);
                            for (int j = 0; j <= i; j++)
                            {
                                if (j == 0)
                                    distStart = 0;
                                else
                                    distStart = distStart + Convert.ToInt32(orderedList[j].MilepostRounded - orderedList[j - 1].MilepostRounded);
                            }
                        }
                        routeStopImports.Add(new LineDetailStationImport
                        {
                            ImportControlId = _insertImportControlOperatorInner.ImportControlId,
                            OperatorId = routeLine.IdOperator,
                            OfficeLineId =int.Parse(routeLine.Catalog.ToString()),
                            Direction = (byte)routeLine.Dir,
                            LineAlternative = routeLine.Var,
                            StationId = Convert.ToInt64(it.PhysicalStop.StationCatalog),// it.PhysicalStop.ID,   it.PhysicalStop.StationCatalog
                            StationOrder = it.Ordinal,
                            StationType = 2,
                            StationActivityType = it.IdStationType ?? 0,
                            FirstDropStationId =it.Horada ?? 0,
                            DistanceFromPreviousStation = distPrev,
                            DistanceFromOriginStation = distStart,
                            Duration = it.Duration / 60, // convert to min
                            StationPlatform = it.Platform,
                            StationFloor = it.PhysicalStop.Floor
                        });
                    }
                }
                List<RouteLink> linkIds = _transCadMunipulationDataDal.GetRouteLinkList(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MapLayerName);
                if (linkIds.IsListFull())
                {
                    int nodeSeq = 0;
                    int distPrev = 0, distStart = 0;
                    
                    for (int i = 0; i < linkIds.Count; i++)
                    {
                        RouteLink routeLink = linkIds[i];
                        if (i > 0)
                        {
                            distPrev = linkIds[i - 1].Length;
                            distStart += linkIds[i - 1].Length;
                        }
                        routeJunctionImports.Add(
                            new LineDetailJunctionImport
                            {
                                ImportControlId = _insertImportControlOperatorInner.ImportControlId,
                                OperatorId = routeLine.IdOperator,
                                OfficeLineId =int.Parse(routeLine.Catalog.ToString()),
                                Direction = (byte)routeLine.Dir,
                                LineAlternative = routeLine.Var,
                                JunctionId = routeLink.NodeIdFirst,
                                JunctionOrder = (++nodeSeq),
                                DistanceFromPreviousJunction = distPrev,
                                DistanceFromOriginJunction = distStart
                            }
                        );
                        if (i == linkIds.Count - 1)
                        {
                            distPrev = linkIds[i].Length;
                            distStart += linkIds[i].Length;
                            routeJunctionImports.Add(
                            new LineDetailJunctionImport
                            {
                                ImportControlId = _insertImportControlOperatorInner.ImportControlId,
                                OperatorId = routeLine.IdOperator,
                                OfficeLineId =int.Parse( routeLine.Catalog.ToString()),
                                Direction = (byte)routeLine.Dir,
                                LineAlternative = routeLine.Var,
                                JunctionId = routeLink.NodeIdLast,
                                JunctionOrder = (++nodeSeq),
                                DistanceFromPreviousJunction = distPrev,
                                DistanceFromOriginJunction = distStart
                            }
                        );}
                    }
                }
            }
            );
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
            //transaction = connection.BeginTransaction((IsolationLevel)ExportConfigurator.GetConfig().TransactionLevel);
            try
            {
                //insertImportControlOperatorDAL.Add(insertImportControlOperatorInner, connection); passed to the BL
                counter = 0;
                routeImports.ForEach(routeImport =>
                     {
                         OnChanged(new ImportToSQLArgs
                         {
                             MaxProgressBarValue = routeImports.Count,
                             ProgressBarValue = counter++,
                             Message = Resources.ResourceDAL.MessageProgressRouteSystemSave
                         });
                         routeImport.ImportControlId = _insertImportControlOperatorInner.ImportControlId;
                         _dalRouteImport.Save(routeImport, _connection);
                         DalShared.BuildNispah2(nispa2CoordsFolder, routeImport, _dalShapeOfJunctionDetailsImport, _connection);
                     }
                );
                //carImports.ForEach(carImport => dalCarImport.Save(carImport,   connection, transaction));
                counter = 0;
                routeJunctionImports.ForEach(routeJunctionImport =>
                    {
                        OnChanged(new ImportToSQLArgs
                        {
                            MaxProgressBarValue = routeJunctionImports.Count,
                            ProgressBarValue = counter++,
                            Message = Resources.ResourceDAL.MessageProgressRouteNodesSave
                        });

                        routeJunctionImport.ImportControlId = _insertImportControlOperatorInner.ImportControlId;
                        _dalRouteJunctionImport.Save(routeJunctionImport, _connection);
                    });
                counter = 0;
                routeStopImports.ForEach(routeStopImport =>
                    {
                        OnChanged(new ImportToSQLArgs
                        {
                            MaxProgressBarValue = routeStopImports.Count,
                            ProgressBarValue = counter++,
                            Message = Resources.ResourceDAL.MessageProgressRouteStopsSave
                        });

                        routeStopImport.ImportControlId = _insertImportControlOperatorInner.ImportControlId;
                        _dalRouteStopImport.Save(routeStopImport, _connection);
                    });
                // check forein key constaint
                if (!IsForeinConstraintPassed(_connection, _insertImportControlOperatorInner.ImportControlId, _insertImportControlOperatorInner.OperatorId, pathToExportConstraintViolationFolder))
                {
                    message = Resources.ResourceDAL.ForeinConstrantViolated;
                    try
                    {
                        DeleteLineData(_connection);
                    }
                    catch (Exception expDelete)
                    {
                        LoggerManager.WriteToLog(expDelete);
                    }
                    return false;
                }
                UpdateImportControlOperator();
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(GetAdditionalInfo());
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
                try
                {
                    DeleteLineData(_connection);
                }
                catch (Exception expDelete)
                {
                    LoggerManager.WriteToLog(expDelete);
                }
                _transCadMunipulationDataDal.CloseConnection();
                throw;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
            return true;
        }
        private void UpdateImportControlOperator()
        {
            if (DalShared.IsTaxiOperator())
            {
                _insertImportControlOperatorInner.DataSource = 9;
                _insertImportControlOperatorInner.Status = 1;

            }
            else
            {
                _insertImportControlOperatorInner.DataSource = 1;
                _insertImportControlOperatorInner.Status = DalShared.IsTrainOperator() ? 1 : 0;
            }
            _insertImportControlOperatorDal.Save(_insertImportControlOperatorInner, _connection);
        }

        /// <summary>
        /// GetAdddedInsertImportControlOperatorID
        /// </summary>
        /// <returns></returns>
        public int GetAdddedInsertImportControlOperatorId()
        {
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
            try
            {
                _insertImportControlOperatorDal.Add(_insertImportControlOperatorInner, _connection);
                return _insertImportControlOperatorInner.ImportControlId;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        /// <summary>
        /// Is Forein Constraint Passed
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="controlOperatorId"></param>
        /// <param name="pathToExportConstraintViolationFolder"></param>
        /// <returns></returns>
        private static bool IsForeinConstraintPassed(SqlConnection connection,int controlOperatorId,int idOperator, string pathToExportConstraintViolationFolder)
        {
            var param = new Dictionary<string, object> {{"ImportControlId", controlOperatorId}};
            bool isForeinConstraintPassed = true;
            DataTable dtJunctions = SQLServerHelper.GetDataByConnection("[import].[ExternalImport_JunctionsExistsStatus]", param, (SqlConnection)connection);
            DataTable dtStations = SQLServerHelper.GetDataByConnection("[import].[ExternalImport_StationsExistsStatus]", param, (SqlConnection)connection);

            var now = DateTime.Now;  
            if (dtJunctions.IsDataTableFull())
            {
                var sbJunction = new StringBuilder();
                
                isForeinConstraintPassed = false;
                
                string fileJunctionName = string.Format("EXPORT_CONSTRAINT_JUNCTION_{0}_{1}.csv", idOperator,now.ToString("ddMMyyyy_hh_mm_ss"));
                sbJunction.AppendLine(string.Format("{0},{1},{2},{3},{4}", "OfficeLineId", "Direction", "LineAlternative", "JunctionOrder", "JunctionId"));
                foreach (DataRow dr in dtJunctions.Rows)
                    sbJunction.AppendLine(string.Format("{0},{1},{2},{3},{4}", dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString()));
                using (var sw = new StreamWriter(Path.Combine(pathToExportConstraintViolationFolder,fileJunctionName) , false, Encoding.GetEncoding(1255)))
                {
                    sw.Write(sbJunction.ToString());
                }
            }

            if (dtStations.IsDataTableFull())
            {
                var sbStation = new StringBuilder();
                isForeinConstraintPassed = false;
                string fileStationName = string.Format("EXPORT_CONSTRAINT_STATION_{0}_{1}.csv", idOperator, now.ToString("ddMMyyyy_hh_mm_ss")); 
                sbStation.AppendLine(string.Format("{0},{1},{2},{3},{4}", "OfficeLineId", "Direction", "LineAlternative", "StationId", "StationOrder"));
                foreach (DataRow dr in dtStations.Rows)
                    sbStation.AppendLine(string.Format("{0},{1},{2},{3},{4}", dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString()));
                using (var sw = new StreamWriter(Path.Combine(pathToExportConstraintViolationFolder, fileStationName), false, Encoding.GetEncoding(1255)))
                {
                    sw.Write(sbStation.ToString());
                }
            }

            return isForeinConstraintPassed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        private void DeleteLineData(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            if (_insertImportControlOperatorInner != null && _insertImportControlOperatorInner.ImportControlId > 0)
            {
                var param = new Dictionary<string, object> {{"ImportControlId", _insertImportControlOperatorInner.ImportControlId}};
                SQLServerHelper.ExecuteQuery("[import].[ExternalImport_DeleteLineDetailStationImport]", param, connection);
                SQLServerHelper.ExecuteQuery("[import].[ExternalImport_DeleteLineDetailJunctionImport]", param, connection);
                SQLServerHelper.ExecuteQuery("[import].[ExternalImport_DeleteShapeOfJunctionDetailsImport]", param, connection);
                SQLServerHelper.ExecuteQuery("[import].[ExternalImport_DeleteLineDetailImport]", param, connection);
                
                _insertImportControlOperatorInner.Status = 3; 
                param.Add("Status", _insertImportControlOperatorInner.Status);
                param.Add("DataSource", 1);
                SQLServerHelper.ExecuteQuery("[import].[ExternalImport_UpdateImportControlOperator]", param, connection);
            }

        }
        /// <summary>
        /// GetAdditionalInfo
        /// </summary>
        /// <returns></returns>
        private static string GetAdditionalInfo()
        {
            return string.Format("Loading process faild for user {0} for operator {1}", GlobalData.LoginUser.UserName, GlobalData.LoginUser.UserOperator.OperatorName);
        }
        #endregion
    }
}
