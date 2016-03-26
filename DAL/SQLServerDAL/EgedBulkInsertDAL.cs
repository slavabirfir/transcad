using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace DAL.SQLServerDAL
{
    public class EgedBulkInsertDAL
    {
        public bool BulkInsert(string fileName, string tableName, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                SqlParameter[] inputParameters = new SqlParameter[2];

                inputParameters[0] = new SqlParameter();
                inputParameters[0].ParameterName = "FilePath";
                inputParameters[0].Value = fileName;
                inputParameters[0].SqlDbType = SqlDbType.VarChar;
                inputParameters[0].Direction = ParameterDirection.Input;
                inputParameters[0].Size = 1000;

                inputParameters[1] = new SqlParameter();
                inputParameters[1].ParameterName = "Table";
                inputParameters[1].Value = tableName;
                inputParameters[1].SqlDbType = SqlDbType.VarChar;
                inputParameters[1].Size = 50;
                inputParameters[1].Direction = ParameterDirection.Input;

                return SQLServerHelper.ExecuteQueryBySqlParameter("[dbo].[EgedBulkInsert]", inputParameters, connection,transaction);
            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(String.Concat(exp.Message, Environment.NewLine, "[dbo].[EgedBulkInsert]", Environment.NewLine, fileName, Environment.NewLine,tableName), exp);
                throw appEx;
            }
        }
        /// <summary>
        /// GetBulkInsertErrors
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public DataTable GetBulkInsertErrors(SqlConnection connection,SqlTransaction transaction)
        {
            return SQLServerHelper.GetDataByConnection("[dbo].[GetEgedImportErrors]", null, connection, transaction);
        }

        /// <summary>
        /// GetNispah3View
        /// </summary>
        /// <returns></returns>
        public DataTable GetNispah3View(SqlConnection connection, SqlTransaction transaction)
        {
            return SQLServerHelper.GetDataByConnection("[dbo].[EgedGetNispah3View]", null, connection, transaction);
        }


        /// <summary>
        /// SavePrevData
        /// </summary>
        /// <param name="connection"></param>
        public void SavePrevData(SqlConnection connection,SqlTransaction transaction)
        {
            SQLServerHelper.ExecuteQuery("dbo.EgedSavePrevData", null, connection,transaction);
        }
        /// <summary>
        /// RestorePrevData
        /// </summary>
        /// <param name="connection"></param>
        public void RestorePrevData(DbConnection connection)
        {
            SQLServerHelper.ExecuteQuery("dbo.EgedRestorePrevData", null, (SqlConnection)connection);
        }

        
    }
}
