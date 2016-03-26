using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BLEntities.Entities;
using BLEntities.SQLServer;
using Utilities;
using System.Data.SqlClient;
using System.Data;

namespace DAL.SQLServerDAL
{
    public class ExternalImport_InsertImportZoneControlOperatorDAL
    {
        /// <summary>
        /// GetStationOperatorPriceAreaList
        /// </summary>
        /// <param name="inputParameter"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<StationOperatorPriceArea> GetStationOperatorPriceAreaList(DataTable inputParameter, DbConnection connection)
        {
            var result = new List<StationOperatorPriceArea>();
            var param = new Dictionary<string, object> {{"StationIds", inputParameter}};
            var data = SQLServerHelper.GetDataByConnection("[import].[ExternalImport_StationToZoneConnection]", param, (SqlConnection)connection);
            if (data!=null && data.IsDataTableFull())
            {
                return (from myRow in data.AsEnumerable()
                              select new StationOperatorPriceArea
                                         {
                                             StationId = myRow.Field<int>("StationId"),
                                             OperatorId = myRow.Field<int>("OperatorId"),
                                             ZoneCode = myRow.Field<int>("ZoneCode")
                                         }).ToList();
            }
            return result;

        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool Add(ExternalImport_InsertImportZoneControlOperator entity, DbConnection connection)
        {

            var param = new Dictionary<string, object> {{"OperatorId", entity.OperatorId}};
            if (entity.Status.HasValue)
                param.Add("Status", entity.Status);
            else
                param.Add("Status", DBNull.Value);

            if (entity.FromDate!=DateTime.MinValue)
                param.Add("FromDate", entity.FromDate);
            else
                param.Add("FromDate", DBNull.Value);
            
            var outparam = new Dictionary<string, object> {{"ImportZoneControlOperatorId", -1}};
            if (SQLServerHelper.ExecuteQueryWithOutputParameters("[import].[ExternalImport_InsertImportZoneControlOperator]", param, outparam, (SqlConnection)connection))
            {
                entity.ImportZoneControlOperatorId = Convert.ToInt32(outparam["ImportZoneControlOperatorId"]);
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
        public bool Save(ExternalImport_InsertImportZoneControlOperator entity, DbConnection connection)
        {
            var param = new Dictionary<string, object>
                            {
                                {"ImportZoneControlOperatorId", entity.ImportZoneControlOperatorId},
                                {"Status", (int) entity.Status},
                            };
            return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_UpdateImportZoneControlOperator]", param, (SqlConnection)connection);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool Delete(ExternalImport_InsertImportZoneControlOperator entity, DbConnection connection)
        {
            var param = new Dictionary<string, object>();
            param.Add("ImportZoneControlOperatorId", entity.ImportZoneControlOperatorId);
            return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_DeleteImportZoneControlOperator]", param, (SqlConnection)connection);
        }
    }
}
