using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.SQLServer;
using IDAL;
using System.Data.Common;
using System.Data.SqlClient;
using Utilities;

namespace DAL.SQLServerDAL
{
    public class InsertImportControlOperatorDAL 
    {
        /// <summary>
        /// add
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool Add(InsertImportControlOperator entity,  DbConnection connection)
        {
            var param = new Dictionary<string, object>
                            {
                                {"OperatorId", entity.OperatorId},
                                {"ImportStartDate", entity.ImportStartDate},
                                {"ImportFinishDate", entity.ImportFinishDate}
                            };
            if (entity.Status.HasValue)
                param.Add("Status", entity.Status);
            else
                param.Add("Status", DBNull.Value);
           
            var outparam = new Dictionary<string, object> {{"ImportControlId", -1}};
            if (SQLServerHelper.ExecuteQueryWithOutputParameters("[import].[InsertImportControlOperator]", param, outparam, (SqlConnection)connection))
            {
                entity.ImportControlId = Convert.ToInt32(outparam["ImportControlId"]);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool Save(InsertImportControlOperator entity, DbConnection connection)
        {
            var param = new Dictionary<string, object>
                            {
                                {"ImportControlId", entity.ImportControlId},
                                {"Status", entity.Status},
                                {"DataSource", entity.DataSource},
                            };
            return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_UpdateImportControlOperator]", param, (SqlConnection)connection);
        }

        
    }
}
