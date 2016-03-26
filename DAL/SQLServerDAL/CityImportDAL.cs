using System;
using System.Collections.Generic;
using BLEntities.SQLServer ;
using IDAL;
using Utilities;
using System.Data.SqlClient;
using System.Data.Common;

namespace DAL.SQLServerDAL
{
    public class CityImportDAL : IImportIDAL<CityImport> 
    {
        #region IImportIDAL<CityImport> Members
        public bool Save(CityImport entity,   DbConnection connection)
        {
            try{
            var param = new Dictionary<string, object> {{"CityId", entity.CityId}, {"CityName", entity.CityName} ,{"AuthorityId", entity.AuthorityId}};
            return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_InsertCityImport]", param, (SqlConnection)connection); 
                                                           
            }
                 
            catch (Exception exp)
            {
                throw new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
            }
        }
        #endregion
    }
}
