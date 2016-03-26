using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using BLEntities.Entities;
using Utilities;

namespace DAL.SQLServerDAL
{
    public class PriceImportDAL
    {
        public int GetMaxNumInUseByAutoNumKod(int autoNumKod,DbConnection connection)
        {
            try
            {
                SqlParameter[] inputParameters = new SqlParameter[2];

                inputParameters[0] = new SqlParameter();
                inputParameters[0].ParameterName = "AutoNumKod";
                inputParameters[0].Value = autoNumKod;
                inputParameters[0].SqlDbType = SqlDbType.Int;
                inputParameters[0].Direction = ParameterDirection.Input;
                
                inputParameters[1] = new SqlParameter();
                inputParameters[1].ParameterName = "MaxNumInUse";
                inputParameters[1].SqlDbType = SqlDbType.Int;
                inputParameters[1].Value = 0;
                inputParameters[1].Direction = ParameterDirection.Output;

                SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_GetMaxNumInUseByAutoNumKod]", inputParameters, (SqlConnection)connection);
                return Convert.ToInt32(inputParameters[1].Value);

                
            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(String.Concat(exp.Message, exp));
                throw appEx;
            }
        }


     


        /// <summary>
        /// Is Area Was Changed
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool IsAreaWasChanged(PriceArea entity, DbConnection connection)
        {
            try
            {
                SqlParameter[] inputParameters = new SqlParameter[3];

                inputParameters[0] = new SqlParameter();
                inputParameters[0].ParameterName = "ZoneCode";
                inputParameters[0].Value = entity.ID;
                inputParameters[0].SqlDbType = SqlDbType.Int;
                inputParameters[0].Direction = ParameterDirection.Input;

                inputParameters[1] = new SqlParameter();
                inputParameters[1].ParameterName = "Area";
                inputParameters[1].Value = entity.Area;
                inputParameters[1].SqlDbType = SqlDbType.Float;
                inputParameters[1].Direction = ParameterDirection.Input;

                inputParameters[2] = new SqlParameter();
                inputParameters[2].ParameterName = "IsNewArea";
                inputParameters[2].SqlDbType = SqlDbType.Bit;
                inputParameters[2].Direction = ParameterDirection.Output;

                SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_IsAreaNewToZoneCode]", inputParameters, (SqlConnection)connection);
                return Convert.ToBoolean(inputParameters[2].Value);

            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
                throw appEx;
            }
        }

    }
}
