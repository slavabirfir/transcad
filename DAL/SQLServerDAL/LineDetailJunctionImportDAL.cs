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
    public class LineDetailJunctionImportDAL : IImportIDAL<LineDetailJunctionImport>
    {

        #region IImportIDAL<LineDetailJunctionImport> Members
        public bool Save(LineDetailJunctionImport entity, DbConnection connection)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ImportControlId", entity.ImportControlId);
                param.Add("OperatorId", entity.OperatorId);
                param.Add("OfficeLineId", entity.OfficeLineId);
                param.Add("Direction", entity.Direction);
                param.Add("LineAlternative", entity.LineAlternative);
                param.Add("JunctionOrder", entity.JunctionOrder);
                param.Add("JunctionId", entity.JunctionId);
                param.Add("DistanceFromPreviousJunction", entity.DistanceFromPreviousJunction);
                param.Add("DistanceFromOriginJunction", entity.DistanceFromOriginJunction);
                return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_InsertLineDetailJunctionImport]", param, (SqlConnection)connection );
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
