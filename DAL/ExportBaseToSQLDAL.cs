using System;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using BLEntities.Entities;
using BLEntities.Accessories;
using BLEntities.SQLServer;
using DAL.SQLServerDAL;
using Utilities;
using System.Data.SqlClient;
using Logger;
using BLEntities.Model;
using System.Data;

namespace DAL
{
    public class ExportBaseToSqldal : IExportInfrastructureToSQLDAL
    {
        /// <summary>
        /// private
        /// </summary>
        private static readonly DateTime Now = DateTime.Now;
        //private string ConnectionStringOperators = ConfigurationHelper.GetDBConnectionString("TransportLisencingString");
        //private string ConnectionStringPlanningFirm = ConfigurationHelper.GetDBConnectionString("TrafficLisencingFirm");
        private SqlConnection _connection;

        #region IExportToSQLDAL Members

        private void InsertCities(List<CityImport> lstCities, SqlConnection connection)
        {
            IImportIDAL<CityImport> dal = new CityImportDAL();
            if (lstCities.IsListFull())
            {
                int counter = 0;
                lstCities.ForEach(c =>
                {
                    dal.Save(c, connection);
                    OnChanged(new ImportToSQLArgs
                    {
                        MaxProgressBarValue = lstCities.Count,
                        ProgressBarValue = ++counter,
                        Message = Resources.ResourceDAL.MessageProgressCity
                    });
                });
            }
            
        }
       
        private void InsertJunctions(List<JunctionImport> lstJunctions, SqlConnection connection)
        {
            IImportIDAL<JunctionImport> dal = new JunctionImportDAL();
            if (lstJunctions.IsListFull())
            {
                int counter = 0;
                lstJunctions.ForEach(c =>
                {
                    dal.Save(c, connection);

                    OnChanged(new ImportToSQLArgs
                    {
                        MaxProgressBarValue = lstJunctions.Count,
                        ProgressBarValue = ++counter,
                        Message = Resources.ResourceDAL.MessageProgressJunction
                    });

                });
            }
            
        }

        
        private void InsertStations(List<StationImport> lstStations, SqlConnection connection)
        {
            IImportIDAL<StationImport> dal = new StationImportDAL();
            if (lstStations.IsListFull())
            {
                int counter = 0;
                lstStations.ForEach(c =>
                {
                    dal.Save(c, connection);
                    OnChanged(new ImportToSQLArgs
                    {
                        MaxProgressBarValue = lstStations.Count,
                        ProgressBarValue = ++counter,
                        Message = Resources.ResourceDAL.MessageProgressStation
                    });
                });
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
        /// <summary>
        /// On Changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(ImportToSQLArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
        
        private static void InsertImportControlInfrastructure(SqlConnection connection)
        {
            var param = new Dictionary<string, object> {{"CreateDate", Now}};
            SQLServerHelper.ExecuteQuery("[import].[ExternalImport_InsertImportControlInfrastructure]", param, (SqlConnection)connection);
           
        }

        
        private void UpdateImportControlInfrastructure(SqlConnection connection)
        {
            var param = new Dictionary<string, object> {{"CreateDate", Now}, {"completed", Now}};
            const string updateInfrastructureCity = "dbo.UpdateInfrastructureCity";
            const string updateInfrastructureJunction = "dbo.UpdateInfrastructureJunction";
            const string updateInfrastructureStation = "dbo.UpdateInfrastructureStation";

            OnChanged(new ImportToSQLArgs
            {
                MaxProgressBarValue = 20,
                ProgressBarValue = 10,
                Message = Resources.ResourceDAL.Exec_UpdateInfrastructureCity
            });
            SQLServerHelper.ExecuteQuery(updateInfrastructureCity, null, connection);

            OnChanged(new ImportToSQLArgs
            {
                MaxProgressBarValue = 20,
                ProgressBarValue = 14,
                Message = Resources.ResourceDAL.Exec_UpdateInfrastructureJunction
            });
            SQLServerHelper.ExecuteQuery(updateInfrastructureJunction, null, connection);

            OnChanged(new ImportToSQLArgs
            {
                MaxProgressBarValue = 20,
                ProgressBarValue = 18,
                Message = Resources.ResourceDAL.Exec_UpdateInfrastructureStation
            });
            SQLServerHelper.ExecuteQuery(updateInfrastructureStation, null, connection);
            SQLServerHelper.ExecuteQuery("[import].[ExternalUpdate_InsertImportControlInfrastructure]", param, connection);
            
        }

        /// <summary>
        /// Delete Data
        /// </summary>
        /// <param name="oprGroup"></param>
        private void DeleteData(EnmOperatorGroup oprGroup)
        {
            _connection = new SqlConnection(DAL.DalShared.GetConnectionString(oprGroup,false));
            try
            {
                _connection.Open();
                DeleteBaseData(_connection);
            }
            catch (Exception expDelete)
            {
                LoggerManager.WriteToLog(expDelete);
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        /// <summary>
        /// Export Infrasructure Data
        /// </summary>
        /// <param name="lstCities"></param>
        /// <param name="lstJunctions"></param>
        /// <param name="lstStations"></param>
        /// <param name="message"></param>
        /// <param name="oprGroup"></param>
        /// <returns></returns>
        private void ExportInfrasructureDataByOperGroup(List<CityImport> lstCities, List<JunctionImport> lstJunctions, List<StationImport> lstStations, ref string message, EnmOperatorGroup oprGroup)
        {
            var lstStationsPerOper = lstStations;
            if (oprGroup == EnmOperatorGroup.Operator)
            {
                lstStationsPerOper = lstStations.FindAll(ps => ps.StationStatusId == 1);
            }
            _connection = new SqlConnection(DalShared.GetConnectionString(oprGroup,false));
            try
            {
                _connection.Open();
                //transaction = connection.BeginTransaction((IsolationLevel) ExportConfigurator.GetConfig().TransactionLevel);
                InsertCities(lstCities, _connection);
                InsertJunctions(lstJunctions, _connection);
                InsertStations(lstStationsPerOper, _connection);
                // inser row into the table InsertImportControlInfrastructure 
                InsertImportControlInfrastructure(_connection);
                // Update infrastructure
                UpdateImportControlInfrastructure(_connection);
                _connection.Close();
                _connection.Dispose();
                return;
            }
            catch (Exception exp)
            {
                _connection.Close();
                _connection.Dispose();
                throw;
            }

        }


        /// <summary>
        /// Export to SQL
        /// </summary>
        /// <param name="lstCities"></param>
        /// <param name="lstJunctions"></param>
        /// <param name="lstStations"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportInfrasructureData(List<CityImport> lstCities, List<JunctionImport> lstJunctions, List<StationImport> lstStations, ref string message)
        {
            try
            {
                ExportInfrasructureDataByOperGroup(lstCities, lstJunctions, SetNullInStationOperatorIdForPlanningFirm(lstStations), ref message, EnmOperatorGroup.PlanningFirm);
                ExportInfrasructureDataByOperGroup(lstCities, lstJunctions, lstStations, ref message, EnmOperatorGroup.Operator);
                return true;
            }
            catch (Exception exp)
            {

                LoggerManager.WriteToLog(GetAdditionalInfo());
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
                LoggerManager.WriteToLog(GetAdditionalInfo());
                LoggerManager.WriteToLog(exp);
                DeleteData(EnmOperatorGroup.PlanningFirm);
                DeleteData(EnmOperatorGroup.Operator);
                throw;
            }

        }

        private static List<StationImport> SetNullInStationOperatorIdForPlanningFirm(List<StationImport> lstStations)
        {
            var listStationForPlanningFirm = new List<StationImport>();
            if (lstStations!=null && lstStations.Any())
            {
                
                lstStations.ForEach(s=>
                                        {
                                            var stationImport = (StationImport)s.DeepCopy();
                                            stationImport.AreaOperatorId = null;
                                            listStationForPlanningFirm.Add(stationImport);
                                        });
            }
            return listStationForPlanningFirm;
        }

        /// <summary>
        /// Delete Base Data
        /// </summary>
        /// <param name="connection"></param>
        private static void DeleteBaseData(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var param = new Dictionary<string, object> {{"CreateDate", Now}};
            const string externalDeleteInsertImportControlInfrastructure = "[import].[ExternalDelete_InsertImportControlInfrastructure]";
            SQLServerHelper.ExecuteQuery(externalDeleteInsertImportControlInfrastructure, param, connection);
        }

        public event EventHandler<ImportToSQLArgs> Changed;

        #endregion
    }
}
