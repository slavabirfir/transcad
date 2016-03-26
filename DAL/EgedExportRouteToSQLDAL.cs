using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using BLEntities.Model;
using Utilities;
using BLEntities.SQLServer ;
using BLEntities.Accessories;
using BLEntities.Entities;
using System.Data.SqlClient;
using IDAL;
using DAL.SQLServerDAL;
using Logger;
using System.Data;

namespace DAL
{
    public class EgedExportRouteToSqldal
    {
        #region const
        public const int Defaultarea = -1;
        #endregion
        public event EventHandler<ImportToSQLArgs> Changed;
        private readonly IImportIDAL<ShapeOfJunctionDetailsImport> _dalShapeOfJunctionDetailsImport = new ShapeOfJunctionDetailsImportDal();
        public bool IsCanceledByUser { get; set; }
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
        /// insertImportControlOperatorDAL
        /// </summary>
        private readonly InsertImportControlOperatorDAL _insertImportControlOperatorDal = new InsertImportControlOperatorDAL();
        private const string Transcadoperatorsconnectionstringkey = "TranscadOperators";
        /// <summary>
        /// private
        /// </summary>
        private SqlConnection _connection;
        //private SqlTransaction transaction = null;
        private readonly EgedSQLServerDAL _egedSqlServerDal = new EgedSQLServerDAL();
        private readonly InsertImportControlOperator _insertImportControlOperatorInner;


        #region IExportRouteToSQLDAL Members

        public EgedExportRouteToSqldal(InsertImportControlOperator insertImportControlOperator)
        {
            _insertImportControlOperatorInner = insertImportControlOperator;
        }
        /// <summary>
        /// Create Line Detail Import Data Table Structure
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateLineDetailImportDataTableStructure()
        {
              var dt = new DataTable("LineDetailImport");
              dt.Columns.Add(new DataColumn("OfficeLineId",typeof(int)));
              dt.Columns.Add(new DataColumn("Direction",typeof(int)));
              dt.Columns.Add(new DataColumn("LineAlternative",typeof(string)));
              return dt;
        }
        /// <summary>
        /// Add Row To Line Detail Import DataTable
        /// </summary>
        /// <param name="dtImportLines"></param>
        /// <param name="routeLine"></param>
        private static void AddRowToLineDetailImportDataTable(DataTable dtImportLines, RouteLine routeLine)
        {
            DataRow dr = dtImportLines.NewRow();
            dr["OfficeLineId"] = int.Parse(routeLine.Catalog.ToString());
            dr["Direction"] = (int)routeLine.Dir;
            dr["LineAlternative"] = routeLine.Var;
            dtImportLines.Rows.Add(dr);
        }


        /// <summary>
        /// GetAdddedInsertImportControlOperatorID
        /// </summary>
        /// <returns></returns>
        public int GetAdddedInsertImportControlOperatorId()
        {
            _connection = new SqlConnection(DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false));
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
        /// Export Route Data
        /// </summary>
        /// <param name="lstRouteLines"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportRouteData(List<RouteLine> lstRouteLines, List<RouteStop> listRouteStops,string pathToExportConstraintViolationFolder, ref string message)
        {
            var dalRouteImport = new LineDetailImportDAL();
            //IImportIDAL<LineDetailStationImport> dalRouteStopImport = new LineDetailStationImportDAL();
            //IImportIDAL<LineDetailJunctionImport> dalRouteJunctionImport = new LineDetailJunctionImportDAL();
            DataTable dtImportLines = CreateLineDetailImportDataTableStructure(); 
            var routeImports = new List<LineDetailImport>();
            int counter  =0;
            string nispa2CoordsFolder = null;
            //if (Convert.ToBoolean(ExportConfigurator.GetConfig().DevelopmentEnv))
            //{
                nispa2CoordsFolder = DalShared.GetNispah2CoordsFolder(); 
            //}
            lstRouteLines.ForEach(routeLine =>
            {
                List<Coord> coorList = _egedSqlServerDal.GetNispah2CoordinateList(routeLine.ID);
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
                        OfficeLineId = int.Parse(routeLine.Catalog.ToString()),
                        LineSign = routeLine.Signpost,
                        Direction = (byte)routeLine.Dir,
                        LineAlternative = routeLine.Var,
                        ServiceType = routeLine.IdServiceType,
                        LineDetailType = 1,// Urban line
                        Line = (int)routeLine.RouteNumber,
                        DistrictId = routeLine.IdZoneHead,     //routeLine.IdZoneHead, ExportRouteToSQLDAL.DEFAULTAREA, 
                        DistrictSecId = routeLine.IdZoneSubHead,   //routeLine.IdZoneSubHead, ExportRouteToSQLDAL.DEFAULTAREA
                        IsBase = routeLine.IsBase,
                        IdExclusivityLine = routeLine.IdExclusivityLine,
                        ConfirmedForSaturday = routeLine.ConfirmedForSaturday,
                        Shape = routeLine.ShapeFile,
                        ShapeGeomteryCoords = coorList,
                        Accessibility = routeLine.Accessibility,
                        AccountingGroupId = routeLine.AccountingGroupID
                    }
                );
                AddRowToLineDetailImportDataTable(dtImportLines, routeLine);
            });
            _connection = new SqlConnection(DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false));
            _connection.Open();
            
            try
            {
                //insertImportControlOperatorDAL.Add(insertImportControlOperatorInner, connection);
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
                         dalRouteImport.Save(routeImport, _connection);
                         DalShared.BuildNispah2(nispa2CoordsFolder, routeImport, _dalShapeOfJunctionDetailsImport, _connection);
                     }
                );
                LoggerManager.WriteToLog("*********** The eged loading Data to Traffic DB has  " + routeImports.Count.ToString() + " routes");
                int progressBarValue = 5;
                if (routeImports.Count <= progressBarValue)
                {
                    progressBarValue = routeImports.Count;
                }
                OnChanged(new ImportToSQLArgs
                {
                    MaxProgressBarValue = routeImports.Count,
                    ProgressBarValue = progressBarValue,
                    Message = Resources.ResourceDAL.MessageProgressRouteNodesSave
                });
                
                using( var connectionTranscad = new SqlConnection(ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey)))
                {
                    try
                    {
                        connectionTranscad.Open();
                        dtImportLines.AcceptChanges();
                        dalRouteImport.EgedInsertNispah2And3(connectionTranscad, _insertImportControlOperatorInner.ImportControlId, dtImportLines);
                    }
                    finally
                    {
                        connectionTranscad.Close();
                        connectionTranscad.Dispose();
                    }

                }
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
                throw (exp);
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

            
            DateTime now = DateTime.Now;  
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

                //SQLServerHelper.ExecuteQuery("[import].[DeleteImportControlOperator]", param, connection);
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
