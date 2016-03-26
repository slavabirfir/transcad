using System;
using BLEntities.SQLServer ;
using IDAL;
using System.Data;
using System.Data.Common;
using Utilities;
using System.Data.SqlClient;

namespace DAL.SQLServerDAL
{
    public class LineDetailStationImportDAL : IImportIDAL<LineDetailStationImport>
    {

        #region IImportIDAL<LineDetailStationImport> Members

        public bool Save(LineDetailStationImport entity,  DbConnection connection)
        {
            try
            {
                var inputParameters = new SqlParameter[16];
                inputParameters[0] = new SqlParameter
                {
                     ParameterName = "ImportControlId",
                     Value = entity.ImportControlId,
                     SqlDbType = SqlDbType.Int,
                     Direction = ParameterDirection.Input
                };
                inputParameters[1] = new SqlParameter
                {
                    ParameterName = "OperatorId",
                    Value = entity.OperatorId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                inputParameters[2] = new SqlParameter
                {
                    ParameterName = "OfficeLineId",
                    Value = entity.OfficeLineId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                inputParameters[3] = new SqlParameter
                {
                    ParameterName = "Direction",
                    Value = entity.Direction,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                inputParameters[4] = new SqlParameter
                {
                    ParameterName = "LineAlternative",
                    Value = entity.LineAlternative,
                    Size = 2,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input
                };

                 inputParameters[5] = new SqlParameter
                {
                    ParameterName = "StationId",
                    Value = entity.StationId,
                    SqlDbType = SqlDbType.BigInt,
                    Direction = ParameterDirection.Input
                };
                

                inputParameters[6] = new SqlParameter
                {
                    ParameterName = "StationOrder",
                    Value = entity.StationOrder,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                inputParameters[7] = new SqlParameter
                {
                    ParameterName = "StationActivityType",
                    Value = entity.StationActivityType,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                inputParameters[8] = new SqlParameter
                {
                    ParameterName = "FirstDropStationId",
                    Value = entity.FirstDropStationId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                inputParameters[9] = new SqlParameter
                {
                    ParameterName = "DistanceFromPreviousStation",
                    Value = DalShared.IsOperatorsIDsNotCalcNispah2Distances() ? -1 : entity.DistanceFromPreviousStation,
                    //Value = entity.DistanceFromPreviousStation,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                inputParameters[10] = new SqlParameter
                {
                    ParameterName = "DistanceFromOriginStation",
                    //Value = entity.DistanceFromOriginStation,
                    Value = DalShared.IsOperatorsIDsNotCalcNispah2Distances() ? -1 : entity.DistanceFromOriginStation,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                inputParameters[11] = new SqlParameter
                {
                    ParameterName = "Duration",
                    Value = DalShared.IsOperatorsIDsNotCalcNispah2Distances() ? -1 : entity.Duration,
                    //Value = entity.Duration,
                    SqlDbType = SqlDbType.Float,
                    Direction = ParameterDirection.Input
                };

                inputParameters[12] = new SqlParameter
                {
                    ParameterName = "StationFloor",
                    Value = (!entity.StationFloor.HasValue ? (object) DBNull.Value : entity.StationFloor),
                    SqlDbType = SqlDbType.SmallInt,
                    Direction = ParameterDirection.Input
                };

                inputParameters[13] = new SqlParameter
                {
                    ParameterName = "StationPlatform",
                    Value = (string.IsNullOrEmpty(entity.StationPlatform) ? (object)DBNull.Value : entity.StationPlatform),
                    SqlDbType = SqlDbType.Char,
                    Size = 3,
                    Direction = ParameterDirection.Input
                };

                inputParameters[14] = new SqlParameter
                {
                    ParameterName = "B56Heb",
                    Value = null,
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Direction = ParameterDirection.Input
                };

                inputParameters[15] = new SqlParameter
                {
                    ParameterName = "B56Eng",
                    Value = null,
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Direction = ParameterDirection.Input
                };

                //var param = new Dictionary<string, object>
                //            {
                //                {"ImportControlId", entity.ImportControlId},
                //                {"OperatorId", entity.OperatorId},
                //                {"OfficeLineId", entity.OfficeLineId},
                //                {"Direction", entity.Direction},
                //                {"LineAlternative", entity.LineAlternative},
                //                {"StationId", entity.StationId},
                //                {"StationOrder", entity.StationOrder},
                //                {"StationActivityType", entity.StationActivityType},
                //                {"FirstDropStationId", entity.FirstDropStationId},
                //                {"DistanceFromPreviousStation", entity.DistanceFromPreviousStation},
                //                {"DistanceFromOriginStation", entity.DistanceFromOriginStation},
                //                {"Duration", String.Format("{0:F}", entity.Duration)},
                //                {"StationFloor", entity.StationFloor},
                //                {"StationPlatform", entity.StationPlatform}
                //            };
                //return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_InsertLineDetailStationImport]", param, (SqlConnection)connection);

                return SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_InsertLineDetailStationImport]", inputParameters, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                throw new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
            }
        }

        

        #endregion
    }
}
