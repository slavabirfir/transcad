using System;
using System.Collections.Generic;
using BLEntities.SQLServer ;
using IDAL;
using System.Data.Common;
using Utilities;
using System.Data.SqlClient;

namespace DAL.SQLServerDAL
{
    public class StationImportDAL : IImportIDAL<StationImport>
    {
        #region IImportIDAL<StationImport> Members
        public bool Save(StationImport entity,  DbConnection connection)
        {
            try{
                      var param = new Dictionary<string, object>
                            {
                                {"StationId", entity.StationId},
                                {"StationName", entity.StationName},
                                {"Lat", entity.Lat},
                                {"Long", entity.Long},
                                {"CityId", entity.CityId},
                                {"StreetName", entity.StreetName},
                                {"HouseNumber", entity.HouseNumber ?? String.Empty},
                                {"LatDifferrent", entity.LatDifferrent},
                                {"LongDifferrent", entity.LongDifferrent},
                                {"StationStatusId", entity.StationStatusId},
                                {"StationTypeId", entity.StationTypeId},
                                {"UpdateDate", entity.UpdateDate},
                                {"StationShortName", entity.StationShortName},
                                {"OperatorId", entity.AreaOperatorId}
                            };

                return SQLServerHelper.ExecuteQuery("[import].[ExternalImport_InsertStationImport]", param, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                throw new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
            }
        }

       

        #endregion
    }
}
