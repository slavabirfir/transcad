using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using BLEntities.SQLServer ;
using Utilities;
using System.Data.SqlClient;
using System.Data.Common;
using BLEntities.Entities;
using Microsoft.SqlServer.Types;
using System.Data;
using BLEntities.Accessories;

namespace DAL.SQLServerDAL
{
    public class JunctionVersionSQLServerExportDAL  
    {
        public bool Save(JunctionVersionSQLServerExport entity, DbConnection connection, DbTransaction transaction)
        {
            try{
                SqlParameter[] inputParameters = new SqlParameter[4];
                
                inputParameters[0] = new SqlParameter();
                inputParameters[0].ParameterName = "JunctionVersion";
                inputParameters[0].Value = entity.JunctionVersion;
                inputParameters[0].SqlDbType = SqlDbType.Int;
                inputParameters[0].Direction = ParameterDirection.Input;
                
                inputParameters[1] = new SqlParameter();
                inputParameters[1].ParameterName = "Shape";
                inputParameters[1].Value = entity.Shape;
                inputParameters[1].SqlDbType = SqlDbType.VarBinary;
                inputParameters[1].Direction = ParameterDirection.Input;
                inputParameters[1].Size = -1;
                
                inputParameters[2] = new SqlParameter();
                inputParameters[2].ParameterName = "CreateDate";
                inputParameters[2].Value = entity.CreateDate;
                inputParameters[2].SqlDbType = SqlDbType.Date;
                inputParameters[2].Direction = ParameterDirection.Input;
                
                inputParameters[3] = new SqlParameter();
                inputParameters[3].ParameterName = "ShapeG";
                inputParameters[3].SqlDbType = SqlDbType.Udt;
                inputParameters[3].UdtTypeName = "geometry";  
                inputParameters[3].Direction = ParameterDirection.Input;

            if (entity.ShapeGeomteryCoords.IsListFull())
            {
                SqlGeometryBuilder gb = new SqlGeometryBuilder();
                gb.SetSrid(entity.JunctionVersion);
                gb.BeginGeometry(OpenGisGeometryType.LineString);
                // Define the first element (figure) 
                gb.BeginFigure(entity.ShapeGeomteryCoords[0].Longitude.ConvertFromIntToTvunaDoubleFormat(), entity.ShapeGeomteryCoords[0].Latitude.ConvertFromIntToTvunaDoubleFormat());
                for (int i = 1; i < entity.ShapeGeomteryCoords.Count;i++ )
                {
                    gb.AddLine(entity.ShapeGeomteryCoords[i].Longitude.ConvertFromIntToTvunaDoubleFormat(), entity.ShapeGeomteryCoords[i].Latitude.ConvertFromIntToTvunaDoubleFormat());
                }
                gb.EndFigure();
                gb.EndGeometry();
                inputParameters[3].Value = gb.ConstructedGeometry;
            }

            return SQLServerHelper.ExecuteQueryBySqlParameter("[import].[ExternalImport_InsertUpdateShapeOfJunctionTranscad]", inputParameters, (SqlConnection)connection, (SqlTransaction)transaction);
            }
            catch (Exception exp)
            {
                ApplicationException appEx = new ApplicationException(String.Concat(exp.Message,Environment.NewLine, entity.ToString(), exp));
                throw appEx; 
            }
        }
       
    }
}
