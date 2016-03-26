using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using BLEntities.Model;
using Utilities;
using BLEntities.Entities;
using System.Data;

namespace DAL
{
    public class EgedSQLServerDAL
    {
        #region const
        private const int Egedaccountgroupingdefault = 0;
        private const string Eged = "אגד";
        private const string Transcadoperatorsconnectionstringkey = "TranscadOperators";
        private const string Emptyroaddescription = "-";
        #endregion

        public static string Connectionstringtranscadoperator = ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey);

        public static string ConnectionstringtranscadoperatorReadOnly = string.Format("{0}{1}",ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey), DalShared.ApplicationIntentReadOnly);

        //public static string CONNECTIONSTRINGTRAFFICLICENSING = ConfigurationHelper.GetDBConnectionString(TRANSPORTLISENCINGSTRINGCONNECTIONSTRINGKEY);

        /// <summary>
        /// GetPhysicalStopsRawData
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, PhysicalStop> GetPhysicalStopsRawDataDictionary()
        {

            var dic = new Dictionary<int, PhysicalStop>();
            DataTable dt = SQLServerHelper.GetData("[dbo].[EgedGetAllStations]", null, ConnectionstringtranscadoperatorReadOnly);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    dic.Add(Convert.ToInt32(row["StationId"]), new PhysicalStop
                    {
                        ID = Convert.ToInt32(row["StationId"]),
                        Name = row["StationName"].ToString(),
                        Latitude = Convert.ToInt32(row["Lat"]),
                        LatitudeN = Convert.ToInt32(row["Lat"]),
                        Longitude = Convert.ToInt32(row["Long"]),
                        LongitudeN = Convert.ToInt32(row["Long"]),
                        CityCode = row["CityId"].ToString(),
                        StationCatalog = row["StationId"].ToString(),
                        House = string.IsNullOrEmpty(row["HouseNumber"].ToString()) ? 0 : Convert.ToInt32(row["HouseNumber"]),
                        Street = row["StreetName"].ToString(),
                        IdStationStatus = Convert.ToInt32(row["StationStatusId"]),
                        IdStationType = Convert.ToInt32(row["StationTypeId"])
                    });
                }
            }
            return dic;
        }
        /// <summary>
        /// GetNispah2
        /// </summary>
        /// <returns></returns>
        public DataTable GetNispah2() 
        {
            return SQLServerHelper.GetData("[dbo].[EgedGetNispah2]", null, ConnectionstringtranscadoperatorReadOnly);
        }
        /// <summary>
        /// GetNispah1And3Represent
        /// </summary>
        /// <returns></returns>
        public DataTable GetNispah1And3Represent() 
        {
            return SQLServerHelper.GetData("dbo.EgedGetNispah1And3Represent", null, ConnectionstringtranscadoperatorReadOnly);
        }
        

        /// <summary>
        /// Get Route Stops Raw Data
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<RouteStop>> GetRouteStopsRawDataDictionary()
        {
            var dicRouteStops = new Dictionary<int, List<RouteStop>>();
            var dt = SQLServerHelper.GetData("dbo.EgedGetNispah3", null, ConnectionstringtranscadoperatorReadOnly);
            int counter = 0;
            int lineId = -1;
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["IdLine"]) != lineId)
                    {
                        if (!dicRouteStops.ContainsKey(Convert.ToInt32(row["IdLine"])))
                            dicRouteStops.Add(Convert.ToInt32(row["IdLine"]), new List<RouteStop>());
                        lineId = Convert.ToInt32(row["IdLine"]);
                    }
                    dicRouteStops[lineId].Add(new RouteStop
                    {
                        ID = ++counter,
                        IdStationType = (int?)Convert.ToInt32(row["StationActivityType"]),
                        Horada = (row["FirstDropStationId"] == DBNull.Value || row["FirstDropStationId"].ToString() == "0" ? null : (int?)Convert.ToInt32(row["FirstDropStationId"])),
                        Longitude = 0,
                        Latitude = 0,
                        Milepost = row["DistanceFromPreviousStation"] == DBNull.Value ? 0 : Convert.ToDouble(row["DistanceFromPreviousStation"]),
                        MilepostRounded = row["DistanceFromPreviousStation"] == DBNull.Value ? 0 : Convert.ToInt32(row["DistanceFromPreviousStation"]),
                        MilepostFromOriginStation = row["DistanceFromOriginStation"] == DBNull.Value ? 0 : Convert.ToInt32(row["DistanceFromOriginStation"]),
                        PhysicalStopId = row["StationId"] == DBNull.Value ? 0 : Convert.ToInt32(row["StationId"]),
                        PhysicalStop = new PhysicalStop(),
                        RouteId = Convert.ToInt32(row["IdLine"]),
                        StationCatalog = row["StationId"].ToString(),
                        RouteLine = new RouteLine { ID = Convert.ToInt32(row["IdLine"]) },
                        Name = row["StationId"].ToString(),
                        Duration = row["Duration"] == DBNull.Value ? 0 : (float)Convert.ToDouble((row["Duration"])),
                        Platform = row["StationPlatform"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["StationPlatform"].ToString()),
                        Ordinal = Convert.ToInt32(row["StationOrder"])
                    });
                }
            }
            return dicRouteStops;
        }


        /// <summary>
        /// Get Route Stops Raw Data
        /// </summary>
        /// <returns></returns>
        public List<RouteStop> GetRouteStopsRawData()
        {

            var lst = new List<RouteStop>();
            DataTable dt = SQLServerHelper.GetData("dbo.EgedGetNispah3", null, ConnectionstringtranscadoperatorReadOnly);
            int counter = 0;
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new RouteStop
                    {
                        ID = ++counter,
                        IdStationType = (int?)Convert.ToInt32(row["StationActivityType"]),
                        Horada = (row["FirstDropStationId"] == DBNull.Value || row["FirstDropStationId"].ToString() == "0" ? null : (int?)Convert.ToInt32(row["FirstDropStationId"])),
                        Longitude = 0,
                        Latitude = 0,
                        Milepost = row["DistanceFromPreviousStation"] == DBNull.Value ? 0 : Convert.ToDouble(row["DistanceFromPreviousStation"]),
                        MilepostRounded = row["DistanceFromPreviousStation"] == DBNull.Value ? 0 : Convert.ToInt32(row["DistanceFromPreviousStation"]),
                        MilepostFromOriginStation = row["DistanceFromOriginStation"] == DBNull.Value ? 0 : Convert.ToInt32(row["DistanceFromOriginStation"]),
                        PhysicalStopId = row["StationId"] == DBNull.Value ? 0 : Convert.ToInt32(row["StationId"]),
                        PhysicalStop = new PhysicalStop(),
                        RouteId = Convert.ToInt32(row["IdLine"]),
                        StationCatalog = row["StationId"].ToString(),
                        RouteLine = new RouteLine { ID = Convert.ToInt32(row["IdLine"]) },
                        Name = row["StationId"].ToString(),
                        Duration = row["Duration"] == DBNull.Value ? 0 : (float)Convert.ToDouble((row["Duration"])),
                        Platform = row["StationPlatform"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["StationPlatform"].ToString()),
                        Ordinal = Convert.ToInt32(row["StationOrder"])
                    });
                }
            }
            return lst;
        }
        /// <summary>
        /// Get Route LinesRawData
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RouteLine> GetRouteLinesRawDataDictionary()
        {
            var dic = new Dictionary<int, RouteLine>();
            DataTable dt = SQLServerHelper.GetData("dbo.EgedGetNispah1", null, ConnectionstringtranscadoperatorReadOnly);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    dic.Add(Convert.ToInt32(row["IdLine"]), new RouteLine
                    {
                        ID = Convert.ToInt32(row["IdLine"]),
                        Name = String.Concat(row["OfficeLineId"].ToString(), row["Direction"].ToString(), row["LineAlternative"].ToString()),
                        Signpost = row["LineSign"].ToString(),
                        Dir = (int?)Convert.ToInt32(row["Direction"]),
                        Var = row["LineAlternative"].ToString(),
                        IdOperator = Convert.ToInt32(row["OperatorId"]),
                        IdCluster = Convert.ToInt32(row["ClusterId"]),
                        IdServiceType = Convert.ToInt32(row["ServiceType"]),
                        Catalog = (int?)Convert.ToInt32(row["OfficeLineId"]),
                        IdZoneHead = Convert.ToInt32(row["DistrictId"]),
                        IdZoneSubHead = row["DistrictSecId"] == DBNull.Value ? 0 : Convert.ToInt32(row["DistrictSecId"]),
                        RouteNumber = (int?)Convert.ToInt32(row["Line"]),
                        IsBase = Convert.ToBoolean(row["IsBase"]),
                        IdExclusivityLine = Convert.ToInt32(row["ExclusivityLine"]),
                        Accessibility = Convert.ToBoolean(row["Accessibility"]),
                        Company = Eged,
                        AccountingGroupID = row["AccountingGroupId"] == DBNull.Value ? Egedaccountgroupingdefault : Convert.ToInt32(row["AccountingGroupId"]),
                        RoadDescription = Emptyroaddescription
                    });
                }

            }
            return dic;
        }


        /// <summary>
        /// Get Route Node List
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetRouteNodeList(int routeId)
        {
            var routeNodeDictionary = new Dictionary<int, int>();
            var dis = new Dictionary<string, object>();
            dis.Add("IdLine", routeId);
            DataTable dt = SQLServerHelper.GetData("dbo.EgedGetRouteNodesNispah2", dis, ConnectionstringtranscadoperatorReadOnly);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    routeNodeDictionary.Add(Convert.ToInt32(dr["OrderNode"]), Convert.ToInt32(dr["NodeId"]));
                }
            }
            return routeNodeDictionary;
        }

        /// <summary>
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool SaveRouteLine(RouteLine routeLine)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"IdLine", routeLine.ID},
                              {"ClusterId", routeLine.IdCluster},
                              {"LineSign", routeLine.Signpost},
                              {"OfficeLineId", routeLine.Catalog},
                              {"Line", routeLine.RouteNumber},
                              {"Direction", routeLine.Dir},
                              {"LineAlternative", routeLine.Var},
                              {"ServiceType", routeLine.IdServiceType},
                              {"DistrictId", routeLine.IdZoneHead},
                              {"DistrictSecId", routeLine.IdZoneSubHead == 0 ? null : (int?) routeLine.IdZoneSubHead},
                              {"IsBase", routeLine.IsBase},
                              {"ExclusivityLine", routeLine.IdExclusivityLine},
                              {"Accessibility", routeLine.Accessibility ? 1 : 0},
                              {"AccountingGroupId", routeLine.AccountingGroupID}
                          };

            return SQLServerHelper.ExecuteQuery("[dbo].[EgedUpdateNispah1]", dis, Connectionstringtranscadoperator);
        }


        /// <summary>
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        //public bool SaveStation(PhysicalStop physicalStop)
        //{
        //    Dictionary<string, object> dis = new Dictionary<string, object>();
        //    dis.Add("StationId", physicalStop.ID);
        //    dis.Add("StationName", physicalStop.Name);
        //    dis.Add("Lat", physicalStop.Latitude);
        //    dis.Add("Long", physicalStop.Longitude);
        //    dis.Add("CityId",int.Parse(physicalStop.CityCode));
        //    dis.Add("StreetName", physicalStop.Street);
        //    dis.Add("HouseNumber", physicalStop.House);
        //    dis.Add("StationStatusId", physicalStop.IdStationStatus);
        //    dis.Add("StationTypeId", physicalStop.IdStationType);
        //    dis.Add("CityLinkCode",(byte) physicalStop.CityLinkCode);
        //    return SQLServerHelper.ExecuteQuery("[dbo].[EgedStationUpdate]", dis, CONNECTIONSTRINGTRANSCADOPERATOR);
        //}




        /// <summary>
        /// InsertStation
        /// </summary>
        /// <param name="physicalStop"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool InsertStation(PhysicalStop physicalStop, SqlConnection connection, SqlTransaction transaction)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"StationId", Convert.ToInt32(physicalStop.StationCatalog)},
                              {"StationName", physicalStop.Name},
                              {"Lat", Convert.ToDecimal(physicalStop.Latitude)},
                              {"Long", Convert.ToDecimal(physicalStop.Longitude)},
                              {"CityId", Convert.ToInt32(physicalStop.CityCode)},
                              {"StreetName", physicalStop.Street},
                              {"HouseNumber", physicalStop.House.ToString()},
                              {"StationStatusId", physicalStop.IdStationStatus},
                              {"StationTypeId", physicalStop.IdStationType}
                          };

            return SQLServerHelper.ExecuteQuery("[dbo].[EgedInsertStation]", dis, connection, transaction);
        }

        /// <summary>
        /// Delete All Station
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool DeleteAllStation(SqlConnection connection, SqlTransaction transaction)
        {
            return SQLServerHelper.ExecuteQuery("[dbo].[EgedDeleteAllStation]", null, connection, transaction);
        }

        /// <summary>
        /// GetNispah2CoordinateList
        /// </summary>
        /// <param name="IdLine"></param>
        /// <returns></returns>
        public List<Coord> GetNispah2CoordinateList(int IdLine)
        {
            var dis = new Dictionary<string, object> {{"IdLine", IdLine}};
            DataTable dt = SQLServerHelper.GetData("[dbo].[EgedGetCoordinatesByIdLine]", dis, ConnectionstringtranscadoperatorReadOnly);
            if (dt.IsDataTableFull())
                return (from row in dt.AsEnumerable() select new Coord { Longitude = row.Field<int>("Long"), Latitude = row.Field<int>("Lat") }).ToList<Coord>();
            return null;
        }


        /// <summary>
        /// Save Route Stop
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public bool SaveRouteStop(RouteStop routeStop)
        {
            short? floor = null;
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {
                var ps = GlobalData.RouteModel.PhysicalStopList.FirstOrDefault(el => el.ID == routeStop.PhysicalStopId);
                if (ps!=null)
                {
                   floor = ps.Floor;
                }
            }
            var dis = new Dictionary<string, object>
                          {
                              {"IdLine", routeStop.RouteId},
                              {"StationId", Convert.ToInt32(routeStop.StationCatalog)},
                              {"StationOrder", routeStop.Ordinal},
                              {"StationActivityType", routeStop.IdStationType},
                              {"FirstDropStationId", routeStop.Horada},
                              {"StationFloor", floor},
                              {"StationPlatform", routeStop.Platform}
                          };

            return SQLServerHelper.ExecuteQuery("[dbo].[EgedUpdateNispah3]", dis, Connectionstringtranscadoperator);
        }
        /// <summary>
        /// InsertEndPoint
        /// </summary>
        /// <param name="eP"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public object InsertEndPoint(EndPoint eP, SqlConnection connection, SqlTransaction transaction)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"Long", eP.Longitude},
                              {"Lat", eP.Latitude},
                              {"NodeId", eP.NoteID},
                              {"ID", eP.ID}
                          };
            return SQLServerHelper.ExecuteQuery("[dbo].[EgedInsertendPoints]", dis, connection, transaction);
        }
        /// <summary>
        /// Delete All End Points
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public bool DeleteAllEndPoints(SqlConnection connection, SqlTransaction transaction)
        {
            return SQLServerHelper.ExecuteQuery("[dbo].[EgedDeleteAllEndPoints]", null, connection, transaction);
        }
    }
}
