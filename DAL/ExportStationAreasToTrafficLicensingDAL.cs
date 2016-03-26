using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using BLEntities.Entities;
using IDAL;
using Logger;
using Microsoft.SqlServer.Types;
using Utilities;

namespace DAL
{
    public class ExportStationAreasToTrafficLicensingDal : IExportStationAreasToTrafficLicensingDal
    {
        public void DeleteImporControl(ImporControl imporControl, DbConnection connection)
        {
            var param = new Dictionary<string, object> {{"ImportControlId", imporControl.ImportControlId}};
            SQLServerHelper.ExecuteQuery("[import].[TC_DeleteZoneImport]", param,(SqlConnection)connection);
        }
        
        public int InsertImportControl(ImporControl imporControl, DbConnection connection)
        {
            var param = new Dictionary<string, object>{
                                                        {"StatusId", imporControl.Status},
                                                        {"FromDate", imporControl.FromDate}};
            var outparam = new Dictionary<string, object> { { "ImportControlId", imporControl.ImportControlId } };
            SQLServerHelper.ExecuteQueryWithOutputParameters("[import].[TC_InsertImporControl]", param, outparam,
                                                             (SqlConnection) connection);
            
            return imporControl.ImportControlId = Convert.ToInt32(outparam["ImportControlId"]);
        }

        public void UpdateImportControl(ImporControl imporControl, DbConnection connection)
        {
            var param = new Dictionary<string, object> {
                                {"ImportControlId", imporControl.ImportControlId},
                                {"StatusId", imporControl.Status}
            };
            SQLServerHelper.ExecuteQuery("[import].[TC_UpdateImportControlStatus]", param, (SqlConnection) connection);
        }

        public void Save(PhysicalStop psStop, DbConnection connection)
        {
            
                if (psStop.PriceZoneArea == null)
                {
                    string erMes = string.Format("The {0} Station Catalog doesn't have Price Zone Area. Saving is impossible",psStop.StationCatalog);
                    LoggerManager.WriteToLog(erMes );
                    throw new ApplicationException(erMes);
                }
                PriceZoneArea entity = psStop.PriceZoneArea;
                try
                {
                var inputParameters = new SqlParameter[8];

                inputParameters[0] = new SqlParameter
                {
                    ParameterName = "ImportControlId",
                    Value = entity.ImportControlId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                inputParameters[1] = new SqlParameter
                {
                    ParameterName = "ZoneCode",
                    Value = entity.TatFull,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                inputParameters[2] = new SqlParameter
                {
                    ParameterName = "ZoneDescription",
                    Value = entity.PName,
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Direction = ParameterDirection.Input
                };

                inputParameters[3] = new SqlParameter
                {
                    ParameterName = "StationId",
                    Value = int.Parse(psStop.StationCatalog),
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
                    gb.SetSrid(entity.ID);
                    gb.BeginGeometry(OpenGisGeometryType.Polygon);
                    // Define the first element (figure) 
                    gb.BeginFigure(entity.ShapeGeomteryCoords[0].Longitude.ConvertFromIntToTvunaDoubleFormat(), entity.ShapeGeomteryCoords[0].Latitude.ConvertFromIntToTvunaDoubleFormat());
                    for (int i = 1; i < entity.ShapeGeomteryCoords.Count; i++)
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

                inputParameters[7] = new SqlParameter
                {
                    ParameterName = "Ring",
                    Value = entity.Ring.HasValue ? (object) entity.Ring : DBNull.Value,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                SQLServerHelper.ExecuteQueryBySqlParameter("[import].[TC_lnsertZoneImport]", inputParameters, (SqlConnection)connection);
            }
            catch (Exception exp)
            {
                var appEx = new ApplicationException(String.Concat(exp.Message, Environment.NewLine, entity.ToString(), exp));
                throw appEx;
            }
        }

        public bool ExportPriceArea(List<PhysicalStop> physicalStops, ref string errorMessage)
        {
            var connection = new SqlConnection(ConfigurationHelper.GetDbConnectionString("TrafficLiceningReform"));
            var controllerEntity = new ImporControl { FromDate = DateTime.Now, Status = 0 };
            try
            {
                connection.Open();
                InsertImportControl(controllerEntity, connection);
                physicalStops.Where(ps=>ps.PriceZoneArea!=null).ToList().ForEach(pa =>
                                           {
                                               pa.PriceZoneArea.ImportControlId = controllerEntity.ImportControlId;
                                               Save(pa, connection);
                                           });
                controllerEntity.Status = 1;
                UpdateImportControl(controllerEntity, connection);
                return true;
            }
            catch (Exception exp)
            {
                controllerEntity.Status = 3;
                errorMessage = exp.Message;
                LoggerManager.WriteToLog(exp);
                try
                {
                    DeleteImporControl(controllerEntity, connection);
                }
                catch(Exception inexp)
                {
                    LoggerManager.WriteToLog(inexp);
                    errorMessage = string.Concat(exp.Message," -|- ",inexp.Message);
                }
                return false;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
