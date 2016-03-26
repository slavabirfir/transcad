using System;
using BLEntities.SQLServer ;
using Utilities;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.SqlServer.Types;
using System.Data;

namespace DAL.SQLServerDAL
{
    public class ZoneImportDAL  
    {
       
        public bool Save(ZoneImport entity, DbConnection connection)
        {
            try{
                var inputParameters = new SqlParameter[7];
                
                inputParameters[0] = new SqlParameter
                                         {
                                             ParameterName = "ImportZoneControlOperatorId",
                                             Value = entity.ImportZoneControlOperatorId,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[1] = new SqlParameter
                                         {
                                             ParameterName = "ZoneCode",
                                             Value = entity.ZoneCode,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[2] = new SqlParameter
                                         {
                                             ParameterName = "ZoneDescription",
                                             Value = entity.ZoneDescription,
                                             SqlDbType = SqlDbType.VarChar,
                                             Size = 50,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[3] = new SqlParameter
                                         {
                                             ParameterName = "StationId",
                                             Value = entity.StationId,
                                             SqlDbType = SqlDbType.Int,
                                             Direction = ParameterDirection.Input
                                         };

                inputParameters[4] = new SqlParameter
                                         {
                                             ParameterName = "ShapeGeometry",
                                             SqlDbType = SqlDbType.Udt,
                                             UdtTypeName = "geometry",
                                             Direction = ParameterDirection.Input
                                         };


                if (entity.ShapeGeomteryCoords.IsListFull())
            {
                var gb = new SqlGeometryBuilder();
                gb.SetSrid(entity.ZoneCode);
                gb.BeginGeometry(OpenGisGeometryType.Polygon);
                // Define the first element (figure) 
                gb.BeginFigure(entity.ShapeGeomteryCoords[0].Longitude.ConvertFromIntToTvunaDoubleFormat(), entity.ShapeGeomteryCoords[0].Latitude.ConvertFromIntToTvunaDoubleFormat());
                for (int i = 1; i < entity.ShapeGeomteryCoords.Count;i++ )
                {
                    gb.AddLine(entity.ShapeGeomteryCoords[i].Longitude.ConvertFromIntToTvunaDoubleFormat(), entity.ShapeGeomteryCoords[i].Latitude.ConvertFromIntToTvunaDoubleFormat());
                }
                gb.AddLine(entity.ShapeGeomteryCoords[0].Longitude.ConvertFromIntToTvunaDoubleFormat(), entity.ShapeGeomteryCoords[0].Latitude.ConvertFromIntToTvunaDoubleFormat());
                gb.EndFigure();
                gb.EndGeometry();
                inputParameters[4].Value = gb.ConstructedGeometry;
            }   
            else
            {
                inputParameters[4].Value = DBNull.Value;
            }

            inputParameters[5] = new SqlParameter
                                     {
                                         ParameterName = "ShapeFile",
                                         Value = entity.ShapeFile,
                                         SqlDbType = SqlDbType.VarBinary,
                                         Direction = ParameterDirection.Input
                                     };

                inputParameters[6] = new SqlParameter
                                         {
                                             ParameterName = "Area",
                                             Value = entity.Area,
                                             SqlDbType = SqlDbType.Float,
                                             Direction = ParameterDirection.Input
                                         };


                return SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_InsertZoneImport]", inputParameters, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                var appEx = new ApplicationException(String.Concat(exp.Message,Environment.NewLine, entity.ToString(), exp));
                throw appEx; 
            }
        }
       
    }
}
