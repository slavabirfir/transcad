using System;
using System.Collections.Generic;
using Utilities;
using BLEntities.SQLServer;
using System.Data.SqlClient;
using Logger;
using BLEntities.Model;

namespace DAL
{
    public class AdminOperationSqldal
    {
        //TranscadOperators
        #region Private / Const
        //private readonly string connectionString = ConfigurationHelper.GetDBConnectionString("TranscadOperators");
        private SqlTransaction transaction = null;
        private SqlConnection connection = null;
        #endregion
        /// <summary>
        /// Export To Transcad Operator
        /// </summary>
        /// <param name="insertImportControlOperator"></param>
        /// <param name="lineDetailImportList"></param>
        /// <param name="lineDetailJunctionImportList"></param>
        /// <param name="lineDetailStationImportList"></param>
        public void ExportToTranscadOperator(InsertImportControlOperator insertImportControlOperator,List<LineDetailImport> lineDetailImportList,List<LineDetailJunctionImport> lineDetailJunctionImportList,List<LineDetailStationImport> lineDetailStationImportList,List<StationImport> stationImportList)
        {
            try
            {
                connection = new SqlConnection(DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false));
                connection.Open();
                transaction = connection.BeginTransaction();
                int importControlId = ImportControlOperatorInsert(insertImportControlOperator, connection, transaction);
                if (importControlId > 0)
                {
                    //StationImportInsert
                    if (stationImportList.IsListFull())
                        stationImportList.ForEach(e => StationImportInsert(e,connection,transaction,importControlId));

                    if (lineDetailImportList.IsListFull())
                        lineDetailImportList.ForEach(e => LineDetailImportInsertInsert(e, connection, transaction, importControlId));

                    if (lineDetailJunctionImportList.IsListFull())
                        lineDetailJunctionImportList.ForEach(e => LineDetailJunctionImportInsert(e, connection, transaction, importControlId));

                    if (lineDetailStationImportList.IsListFull())
                        lineDetailStationImportList.ForEach(e => LineDetailStationImportInsert(e, connection, transaction, importControlId));

                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                    const string errorMessage = "importControlId in the Admin Export Compare DataBase was not created";
                    LoggerManager.WriteToLog(errorMessage);
                    throw new ApplicationException(errorMessage);
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                transaction.Rollback();
                throw; 
            }
            finally 
            {
                connection.Close();
            }
        }


        /// <summary>
        /// Update Operator
        /// </summary>
        private static int ImportControlOperatorInsert(InsertImportControlOperator insertImportControlOperator, SqlConnection connection, SqlTransaction transaction)
        {
            var disInParams = new Dictionary<string, object>
                                  {
                                      {"OperatorId", insertImportControlOperator.OperatorId},
                                      {"ImportStartDate", insertImportControlOperator.ImportStartDate}
                                  };
            var disOutParams = new Dictionary<string, object> {{"ImportControlId", 0}};
            SQLServerHelper.ExecuteQueryWithOutputParameters("dbo.ImportControlOperatorInsert", disInParams, disOutParams, connection, transaction);
            return Convert.ToInt32(disOutParams["ImportControlId"]);
        }

        /// <summary>
        /// LineDetailImportInsertInsert
        /// </summary>
        /// <param name="lineDetailImport"></param>
        /// <returns></returns>
        private static bool LineDetailImportInsertInsert(LineDetailImport lineDetailImport, SqlConnection connection, SqlTransaction transaction,int importControlId)
        {
            var dis = new Dictionary<string, object>();
            dis.Add("ImportControlId", importControlId);
            dis.Add("OperatorId", lineDetailImport.OperatorId);

            dis.Add("ClusterId", lineDetailImport.ClusterId);
            dis.Add("LineSign", lineDetailImport.LineSign);
            dis.Add("OfficeLineId", lineDetailImport.OfficeLineId);
            dis.Add("Line", lineDetailImport.Line);
            
            dis.Add("Direction", lineDetailImport.Direction);
            dis.Add("LineAlternative", lineDetailImport.LineAlternative);
            dis.Add("ServiceType", lineDetailImport.ServiceType);
            dis.Add("LineDetailType", lineDetailImport.LineDetailType);
            dis.Add("DistrictId", lineDetailImport.DistrictId);
            dis.Add("DistrictSecId", lineDetailImport.DistrictSecId);
            dis.Add("IsBase", lineDetailImport.IsBase);

            return SQLServerHelper.ExecuteQuery("[dbo].[LineDetailImportInsert]", dis, connection, transaction);
        }

        /// <summary>
        /// Line Detail Junction Import Insert
        /// </summary>
        /// <param name="lineDetailJunctionImport"></param>
        /// <returns></returns>
        private static bool LineDetailJunctionImportInsert(LineDetailJunctionImport lineDetailJunctionImport, SqlConnection connection, SqlTransaction transaction, int importControlId)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"ImportControlId", importControlId},
                              {"OperatorId", lineDetailJunctionImport.OperatorId},
                              {"OfficeLineId", lineDetailJunctionImport.OfficeLineId},
                              {"Direction", lineDetailJunctionImport.Direction},
                              {"LineAlternative", lineDetailJunctionImport.LineAlternative},
                              {"JunctionOrder", lineDetailJunctionImport.JunctionOrder},
                              {"JunctionId", lineDetailJunctionImport.JunctionId},
                              {"DistanceFromPreviousJunction", lineDetailJunctionImport.DistanceFromPreviousJunction},
                              {"DistanceFromOriginJunction", lineDetailJunctionImport.DistanceFromOriginJunction}
                          };

            return SQLServerHelper.ExecuteQuery("dbo.LineDetailJunctionImportInsert", dis, connection, transaction);
        }

        /// <summary>
        /// Line Detail Station ImportInsert
        /// </summary>
        /// <param name="lineDetailStationImport"></param>
        /// <returns></returns>
        private static bool LineDetailStationImportInsert(LineDetailStationImport lineDetailStationImport, SqlConnection connection, SqlTransaction transaction, int importControlId)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"ImportControlId", importControlId},
                              {"OperatorId", lineDetailStationImport.OperatorId},
                              {"OfficeLineId", lineDetailStationImport.OfficeLineId},
                              {"Direction", lineDetailStationImport.Direction},
                              {"LineAlternative", lineDetailStationImport.LineAlternative},
                              {"StationId", lineDetailStationImport.StationId},
                              {"StationOrder", lineDetailStationImport.StationOrder},
                              {"StationActivityType", lineDetailStationImport.StationActivityType},
                              {"FirstDropStationId", lineDetailStationImport.FirstDropStationId},
                              {"DistanceFromPreviousStation", lineDetailStationImport.DistanceFromPreviousStation},
                              {"DistanceFromOriginStation", lineDetailStationImport.DistanceFromOriginStation},
                              {"Duration", lineDetailStationImport.Duration}
                          };
            return SQLServerHelper.ExecuteQuery("dbo.LineDetailStationImportInsert", dis, connection, transaction);
        }


/// <summary>
        /// StationImportInsert
/// </summary>
/// <param name="stationImport"></param>
/// <param name="connection"></param>
/// <param name="transaction"></param>
/// <param name="importControlId"></param>
/// <returns></returns>
       private static bool StationImportInsert(StationImport stationImport, SqlConnection connection, SqlTransaction transaction, int importControlId)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"ImportControlId", importControlId},
                              {"StationId", stationImport.StationId},
                              {"StationName", stationImport.StationName},
                              {"Lat", stationImport.Lat},
                              {"Long", stationImport.Long},
                              {"CityId", stationImport.CityId},
                              {"StreetName", stationImport.StreetName},
                              {"HouseNumber", stationImport.HouseNumber},
                              {"LatDifferrent", stationImport.LatDifferrent},
                              {"LongDifferrent", stationImport.LongDifferrent}
                          };
    return SQLServerHelper.ExecuteQuery("[dbo].[StationImportInsert]", dis, connection, transaction);
        }  



    }
}
