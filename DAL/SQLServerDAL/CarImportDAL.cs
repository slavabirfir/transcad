using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using BLEntities.SQLServer ;
using Utilities;
using System.Data.SqlClient;
using System.Data.Common;

namespace DAL.SQLServerDAL
{
    public class CarImportDAL : IImportIDAL<CarImport> 
    {

        #region IImportIDAL<CarImport> Members

        public bool Save(CarImport entity, DbConnection connection)
        {
            try{
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("ImportControlId", entity.ImportControlId);
            param.Add("IdOperator", entity.IdOperator);
            param.Add("IdCluster", entity.IdCluster);
            param.Add("Catalog", entity.Catalog);
            param.Add("Signpost", entity.Signpost);
            param.Add("Direction", entity.Direction);
            param.Add("VarNum", entity.VarNum);
            param.Add("IdVehicleSize", entity.IdVehicleSize);
            param.Add("IdVehicleType", entity.IdVehicleType);
            
            return SQLServerHelper.ExecuteQuery("dbo.procInsertCarImport", param, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(String.Concat(exp.Message,Environment.NewLine, entity.ToString(), exp));
                throw appEx; 
            }
        }
        #endregion
    }
}
