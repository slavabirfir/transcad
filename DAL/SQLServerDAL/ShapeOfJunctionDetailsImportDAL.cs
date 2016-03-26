using System;
using BLEntities.SQLServer ;
using IDAL;
using System.Data;
using System.Data.Common;
using Utilities;
using System.Data.SqlClient;

namespace DAL.SQLServerDAL
{
    public class ShapeOfJunctionDetailsImportDal : IImportIDAL<ShapeOfJunctionDetailsImport>
    {
        #region IImportIDAL<LineDetailImport> Members
        public bool Save(ShapeOfJunctionDetailsImport entity, DbConnection connection)
        {
            try
            {
                var inputParameters = new SqlParameter[8];
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
                                             SqlDbType = SqlDbType.VarChar,
                                             Size = 2,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[5] = new SqlParameter
                                         {
                                             ParameterName = "Order",
                                             Value = entity.Order,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[6] = new SqlParameter
                                         {
                                             ParameterName = "Lat",
                                             Value = entity.Lat,
                                             SqlDbType = SqlDbType.Decimal,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[7] = new SqlParameter
                                         {
                                             ParameterName = "Long",
                                             Value = entity.Long,
                                             SqlDbType = SqlDbType.Decimal,
                                             Direction = ParameterDirection.Input
                                         };


                return SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_InsertShapeOfJunctionDetailsImport]", inputParameters, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                throw new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
            }

        }

        public bool Delete(LineDetailImport entity, DbConnection connection, DbTransaction transact)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
