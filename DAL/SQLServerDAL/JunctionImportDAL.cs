using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.SQLServer ;
using IDAL;
using System.Transactions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Utilities;

namespace DAL.SQLServerDAL
{
    public class JunctionImportDAL : IImportIDAL<JunctionImport>
    {

        #region IImportIDAL<JunctionImport> Members

         

        public bool Save(JunctionImport entity, DbConnection connection)
        {
            try{
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("JunctionId", entity.JunctionId);
            param.Add("Lat", entity.Lat);
            param.Add("Long", entity.Long);
            param.Add("JunctionDesc", entity.JunctionDesc);
            param.Add("CityId", entity.CityId);
            return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_InsertJunctionImport]", param, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
                throw appEx;
            }
        }

       

        #endregion
    }
}
