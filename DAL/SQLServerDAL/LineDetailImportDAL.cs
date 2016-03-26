using System;
using BLEntities.SQLServer ;
using IDAL;
using System.Data;
using System.Data.Common;
using Utilities;
using System.Data.SqlClient;

namespace DAL.SQLServerDAL
{
    public class LineDetailImportDAL  : IImportIDAL<LineDetailImport>
    {

        #region IImportIDAL<LineDetailImport> Members

       
        public bool Save(LineDetailImport entity,   DbConnection connection)
        {

            try
            {
                var inputParameters = new SqlParameter[18];
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
                                             ParameterName = "ClusterId",
                                             Value = entity.ClusterId,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[3] = new SqlParameter
                                         {
                                             ParameterName = "LineSign",
                                             Value = entity.LineSign,
                                             SqlDbType = SqlDbType.VarChar,
                                             Size = 20,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[4] = new SqlParameter
                                         {
                                             ParameterName = "OfficeLineId",
                                             Value = entity.OfficeLineId,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[5] = new SqlParameter
                                         {
                                             ParameterName = "Line",
                                             Value = entity.Line,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[6] = new SqlParameter
                                         {
                                             ParameterName = "Direction",
                                             Value = entity.Direction,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[7] = new SqlParameter
                                         {
                                             ParameterName = "LineAlternative",
                                             Value = entity.LineAlternative,
                                             SqlDbType = SqlDbType.VarChar,
                                             Size = 2,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[8] = new SqlParameter
                                         {
                                             ParameterName = "ServiceType",
                                             Value = entity.ServiceType,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[9] = new SqlParameter
                                         {
                                             ParameterName = "LineDetailType",
                                             Value = entity.LineDetailType,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[10] = new SqlParameter
                                          {
                                              ParameterName = "DistrictId",
                                              Value = entity.DistrictId,
                                              SqlDbType = SqlDbType.Int,
                                              Direction = ParameterDirection.Input
                                          };

                inputParameters[11] = new SqlParameter
                                          {
                                              ParameterName = "DistrictSecId",
                                              Value = entity.DistrictSecId,
                                              SqlDbType = SqlDbType.Int,
                                              Direction = ParameterDirection.Input
                                          };

                inputParameters[12] = new SqlParameter
                                          {
                                              ParameterName = "IsBase",
                                              Value = entity.IsBase,
                                              SqlDbType = SqlDbType.Bit,
                                              Direction = ParameterDirection.Input
                                          };

                inputParameters[13] = new SqlParameter
                                          {
                                              ParameterName = "ExclusivityLine",
                                              Value = entity.IdExclusivityLine,
                                              SqlDbType = SqlDbType.Int,
                                              Direction = ParameterDirection.Input
                                          };

                inputParameters[14] = new SqlParameter
                                          {
                                              ParameterName = "ConfirmedForSaturday",
                                              Value = entity.ConfirmedForSaturday,
                                              SqlDbType = SqlDbType.Bit,
                                              Direction = ParameterDirection.Input
                                          };

                inputParameters[15] = new SqlParameter
                                          {
                                              ParameterName = "Shape",
                                              Value = entity.Shape ?? new byte[] {},
                                              SqlDbType = SqlDbType.VarBinary,
                                              Direction = ParameterDirection.Input,
                                              Size = -1
                                          };

                inputParameters[16] = new SqlParameter
                                          {
                                              ParameterName = "Accessibility",
                                              Value = entity.Accessibility,
                                              SqlDbType = SqlDbType.Bit,
                                              Direction = ParameterDirection.Input
                                          };

                inputParameters[17] = new SqlParameter
                                          {
                                              ParameterName = "AccountingGroupId",
                                              Value = entity.AccountingGroupId,
                                              SqlDbType = SqlDbType.Int,
                                              Direction = ParameterDirection.Input
                                          };

                return SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_InsertLineDetailImport]", inputParameters, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                var appEx = new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
                throw appEx;
            }

        }

        public bool Delete(LineDetailImport entity, DbConnection connection, DbTransaction transact)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Insert Nispah 1 And 2
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ImportControlId"></param>
        /// <param name="dtLineDetailImport"></param>
        /// <returns></returns>
        public bool EgedInsertNispah2And3(DbConnection connection, int ImportControlId, DataTable dtLineDetailImport)
        {
             try
            {
                var inputParameters = new SqlParameter[2];
                inputParameters[0] = new SqlParameter();
                inputParameters[0].ParameterName = "LineDetailImport";
                inputParameters[0].Value = dtLineDetailImport;
                inputParameters[0].SqlDbType = System.Data.SqlDbType.Structured;
                inputParameters[0].Direction = ParameterDirection.Input;

                inputParameters[1] = new SqlParameter();
                inputParameters[1].ParameterName = "ImportControlId";
                inputParameters[1].Value = ImportControlId;
                inputParameters[1].SqlDbType = SqlDbType.Int;
                inputParameters[1].Direction = ParameterDirection.Input;

                return SQLServerHelper.ExecuteQueryBySqlParameter("[dbo].[EgedInsertNispah2And3]", inputParameters, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(exp.Message);
                throw appEx;
            }

        }


        #endregion
    }
}
