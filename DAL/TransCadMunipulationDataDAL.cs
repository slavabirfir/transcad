using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using IDAL;
using System.Data;
using BLEntities.Entities;
using CaliperForm;
using ExportConfiguration;
using BLEntities.Accessories;
using Utilities;
using System.Diagnostics;
using Logger;
using System.Collections;
using BLEntities.Model;
using BLEntities.SQLServer;

namespace DAL
{
    public partial class TransCadMunipulationDataDAL : ITransCadMunipulationDataDAL
    {

        #region Const
        private const string IntegerConst = "Integer";
        private const string Characterconst = "Character";
        private const short IntegerWidthConst = 8;
        private const short IntegerWidthOneConst = 1;
        private const string Zerrocity = "0000";
        #endregion

        #region Private Members

        public List<string> PriceAreaReadOnlyFields
        {
            get
            {
                return new List<string> { "ZoneCode", "ZoneDescription"};
            }
        }


        /// <summary>
        /// RouteLineReadOnlyFields
        /// </summary>
        private static List<string> RouteLineReadOnlyFields
        {
            get
            {
                return new List<string> { "Route_ID", "Route_Name", "Hagdara", "Signpost", "Dir", "Var", "IdOperator", "IdCluster", "IdServiceType", "RouteNumber", "Catalog", "IdSeasonal", "RoadDescription", "IdZoneHead", "IdZoneSubHead", "IsBase", "IdExclusivityLine", "Accessibility", "AccountingGroupID", "ValidExportDate" };
            }
        }
        /// <summary>
        /// RouteLineReadOnlyFields
        /// </summary>
        private static List<string> RouteStopReadOnlyFields
        {
            get
            {
                return new List<string> { "Stop_UID", "Horada", "[B-56]", "Duration", "Platform" };
            }
        }
        /// <summary>
        /// RouteLineReadOnlyFields
        /// </summary>
        //private List<string> PhysicalStopReadOnlyFields
        //{
        //    get
        //    {
        //        return new List<string> { "ID", "Longitude", "Latitude", "Direction", "Name", "ID_SEKER" , 
        //        "LinkUserID"  , "Street" ,  "House" ,  "Across" ,  "CityCode",  "LINKID" ,  "LongitudeN",  "LatitudeN","StationTypeId","StationStatusId","CityLinkCode" , "FromPlatform" , "ToPlatform" };
        //    }
        //}
        private Connection _gisdk;
        private bool InitGSDK()
        {
            _gisdk = new Connection();
            _gisdk.CloseOnError = true;
            _gisdk.MappingServer = ExportConfigurator.GetConfig().MappingServer;
            bool isOpen = _gisdk.Open();
            return isOpen;
        }
        /// <summary>
        /// Close Connection
        /// </summary>
        public void CloseConnection()
        {
            if (_gisdk != null && _gisdk.IsConnected())
            {
                _gisdk.Close();
            }
        }


        private Connection Gisdk
        {
            get
            {
                if (_gisdk == null)
                {
                    InitGSDK();
                }
                if (!_gisdk.IsConnected())
                {
                    InitGSDK();
                }
                return _gisdk;
            }
        }
        #endregion

        public bool IsTranscadActive()
        {
            try
            {
                return IsTranscadOpenedByProcessOwner("tcw",Environment.UserName);
            }
            catch (Exception)
            {
                return false;
            }
        }
        static bool IsTranscadOpenedByProcessOwner(string processName, string username)
        {
            string query = @"SELECT * FROM Win32_Process where Name LIKE '"+ processName + "%'";

            var moSearcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection moCollection = moSearcher.Get();

            foreach (ManagementObject mo in moCollection)
            {
                var args = new[] { string.Empty };
                int returnVal = Convert.ToInt32(mo.InvokeMethod("GetOwner", args));
                if (returnVal == 0)
                {
                    if (args[0] != null && args[0].Contains(username) || username.Contains(args[0]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        



        #region Private Functions
        /// <summary>
        /// Fill Data
        /// </summary>
        /// <param name="filedNames"></param>
        /// <param name="layer"></param>
        /// <param name="sentance"></param>
        /// <returns></returns>
        private DataTable FillData(string[] filedNames, string layer, string sentance)
        {
            try
            {
                Gisdk.DoFunction("SetLayer", layer);
                Gisdk.DoFunction("SetReportFileEncoding", "he-IL");
                String[,] options = null;
                object returnValues = Gisdk.DoFunction("SelectByQuery", "All Records", "several", sentance, options);
                GisdkDataAdapter dataAdapter = new GisdkDataAdapter(Gisdk, layer, "All Records", filedNames[0]);
                dataAdapter.ViewFieldNames = filedNames;
                dataAdapter.TableName = layer;
                dataAdapter.RowsPerFill = 499;
                DataSet ds = new DataSet();
                dataAdapter.TableName = layer;
                while ((dataAdapter.SetRows > 0) && (dataAdapter.RowsLeft > 0))
                {
                    int numRows = dataAdapter.Fill(ds);
                    dataAdapter.CurrentRow = dataAdapter.NextRow;
                }
                if (ds.IsDataSetHasTableFull())
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }

        }
        #endregion
        /// <summary>
        /// Init Map Resize
        /// </summary>
        /// <param name="layername"></param>
        public void InitMapResize(string layername)
        {
            try
            {

                //object mapName =Gisdk.DoFunction("GetMap");
                string mapName = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;

                object defaultScope = Gisdk.DoFunction("GetMapDefaultScope", mapName);
                Gisdk.DoFunction("SetMapScope", mapName, defaultScope);
                Gisdk.DoFunction("SetMapTitle", mapName, layername);
                Gisdk.DoFunction("RedrawMap", mapName);

            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Set Map Units
        /// </summary>
        /// <param name="units"></param>
        public void SetMapUnits(string units, string routeSystem)
        {
            try
            {
                object unitsOld = Gisdk.DoFunction("GetMapUnits", "Plural");
                Gisdk.DoFunction("SetLayer", routeSystem);
                if (unitsOld.ToString() != units)
                {
                    object rs_db = Gisdk.DoFunction("GetLayerDB", routeSystem);
                    //object units = GetMapUnits()
                    object[] arr = new object[1];
                    arr[0] = new object[2] { "Units", units };
                    Gisdk.DoFunction("ModifyRouteSystem", rs_db, arr);
                    Gisdk.DoFunction("ReloadRouteSystem", rs_db);
                    Gisdk.DoFunction("SetMapUnits", units);
                }

            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                throw exc;
            }
        }
        /// <summary>
        /// ReloadRouteSystem
        /// </summary>
        /// <param name="routeSystem"></param>
        public void ReloadRouteSystem(String routeSystem)
        {
            try
            {
                //object rs_db = Gisdk.DoFunction("GetLayerDB", routeSystem );
                Gisdk.DoFunction("ReloadRouteSystem", routeSystem);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                throw exc;
            }
        }


        /// <summary>
        /// Get Map Units
        /// </summary>
        /// <param name="layername"></param>
        public string GetMapUnits(string layerFile)
        {
            try
            {
                object info = Gisdk.DoFunction("GetRouteSystemInfo", layerFile);
                info = Gisdk.DoFunction("ExtractArray", info, 3);
                info = Gisdk.DoFunction("ExtractArray", info, 10);
                object[] arrayData = CaliperForm.Connection.ObjectToArray(info, "string");
                if (info != null && arrayData.Length == 2)
                {
                    return arrayData[1].ToString();
                }
                return string.Empty;

            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                return string.Empty;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// Set Read Only Field Set
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="fieldSet"></param>
        /// <param name="isReadOnly"></param>
        public void SetReadOnlyFieldSet(string layer, List<string> fieldSet, bool isReadOnly)
        {
            try
            {
                fieldSet.ForEach(f => Gisdk.DoFunction("SetFieldProtection", string.Concat(layer, ".", f), isReadOnly.ToString()));
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// Get Route Stops Raw Data
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<RouteStop>> GetRouteStopsRawDataDictionary()
        {
            const int delta = 49900;
            var dicRouteStops = new Dictionary<int, List<RouteStop>>();
            var filedNames = new[] { "ID", "Longitude", "Latitude", "Route_ID", "Milepost", "Physical_Stop_ID", "Stop_UID", "Horada", "[B-56]", "Duration",  "Platform" };
            var sentance = "SELECT ID,Longitude,Latitude,Route_ID,Milepost,Physical_Stop_ID,Stop_UID,Horada,[B-56],Duration, Platform WHERE ID>0";
            var dt = new DataTable();
            if (GlobalData.LoginUser.UserOperator.IdOperator == 3)
            {
                string[,] options = null;
                Gisdk.DoFunction("SetLayer", ExportConfigurator.GetConfig().RouteStopsLayerName);
                object returnValues = Gisdk.DoFunction("SelectByQuery", "All Records", "several", sentance, options);
                long counter = 0;
                while (counter <= Convert.ToInt64(returnValues))
                {
                    counter = counter + delta;
                    sentance = string.Concat("SELECT ID,Longitude,Latitude,Route_ID,Milepost,Physical_Stop_ID,Stop_UID,Horada,[B-56],Duration,Floor,Platform WHERE ID>=", counter - delta, " AND ID<", counter);
                    DataTable dtInner = FillData(filedNames, ExportConfigurator.GetConfig().RouteStopsLayerName, sentance);
                    dt.Merge(dtInner);
                }
            }
            else
            {
                dt = FillData(filedNames, ExportConfigurator.GetConfig().RouteStopsLayerName, sentance);
            }
            int lineId = -1;
            SetReadOnlyFieldSet(ExportConfigurator.GetConfig().RouteStopsLayerName, RouteStopReadOnlyFields, true);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["Route_ID"]) != lineId)
                    {
                        if (!dicRouteStops.ContainsKey(Convert.ToInt32(row["Route_ID"])))
                            dicRouteStops.Add(Convert.ToInt32(row["Route_ID"]), new List<RouteStop>());
                        lineId = Convert.ToInt32(row["Route_ID"]);
                    }
                    dicRouteStops[lineId].Add(new RouteStop
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        IdStationType = row["Stop_UID"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["Stop_UID"]),
                        Horada = (row["Horada"] == DBNull.Value || row["Horada"].ToString() == "0" ? null : (int?)Convert.ToInt32(row["Horada"])),
                        Longitude = row["Longitude"] == DBNull.Value ? 0 : Convert.ToInt32(row["Longitude"]),
                        Latitude = row["Latitude"] == DBNull.Value ? 0 : Convert.ToInt32(row["Latitude"]),
                        Milepost = row["Milepost"] == DBNull.Value ? 0 : Convert.ToDouble(row["Milepost"]),
                        PhysicalStopId = row["Physical_Stop_ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["Physical_Stop_ID"]),
                        PhysicalStop = new PhysicalStop(),
                        RouteId = Convert.ToInt32(row["Route_ID"]),
                        RouteLine = new RouteLine(),
                        Name = row["ID"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["ID"].ToString()),
                        Duration = row["Duration"] == DBNull.Value ? 0 : (float)Convert.ToDouble((row["Duration"])),
                        Platform = row["Platform"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Platform"].ToString())
                    });
                }
            }
            return dicRouteStops;

        }
        /// <summary>
        /// Get Route Lines Raw Data
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RouteLine> GetRouteLinesRawDataDictionary()
        {
            var dicRouteLineList = new Dictionary<int, RouteLine>();
            var filedNames = new[] { "Route_ID", "Route_Name", "Signpost", "Dir", "Var", "IdOperator", "IdCluster", "IdServiceType", "Catalog", "IdSeasonal", "RoadDescription", "RouteNumber", "IdZoneHead", "IdZoneSubHead", "IsBase", "Hagdara", "IdExclusivityLine", "Accessibility", "AccountingGroupID", "ValidExportDate" };
            const string sentance = "SELECT Route_ID,  Signpost,RouteNumber, Dir, Var, IdOperator, IdCluster,  IdServiceType, Catalog, IdSeasonal, RoadDescription, IdZoneHead, IdZoneSubHead,RouteNumber, IsBase, Hagdara, IdExclusivityLine,Accessibility,AccountingGroupID,ValidExportDate WHERE Route_ID>0";
            var dt = FillData(filedNames, ExportConfigurator.GetConfig().RouteSystemLayerName, sentance);
            SetReadOnlyFieldSet(ExportConfigurator.GetConfig().RouteSystemLayerName, RouteLineReadOnlyFields, true);
            if (dt.IsDataTableFull())
            {
                var counter = 0;
                
                foreach (DataRow row in dt.Rows)
                {
                    counter++;
                    dicRouteLineList.Add(Convert.ToInt32(row["Route_ID"]), new RouteLine
                    {
                        ID = Convert.ToInt32(row["Route_ID"]),
                        Name = row["Route_Name"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Route_Name"].ToString()),
                        Signpost = row["Signpost"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Signpost"].ToString()),
                        Dir = (row["Dir"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["Dir"])),
                        Var = row["Var"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Var"].ToString()),
                        IdOperator = row["IdOperator"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdOperator"]),
                        IdCluster = row["IdCluster"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdCluster"]),
                        IdServiceType = row["IdServiceType"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdServiceType"]),
                        Catalog = (row["Catalog"] == DBNull.Value || row["Catalog"].ToString() == "0" ? null : (int?)Convert.ToInt32(row["Catalog"])),
                        IdSeasonal = row["IdSeasonal"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdSeasonal"]),
                        RoadDescription = row["RoadDescription"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["RoadDescription"].ToString()),
                        IdZoneHead = row["IdZoneHead"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdZoneHead"]),
                        IdZoneSubHead = row["IdZoneSubHead"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdZoneSubHead"]),
                        RouteNumber = row["RouteNumber"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["RouteNumber"]),
                        IsBase = row["IsBase"] == DBNull.Value ? false : Convert.ToInt32(row["IsBase"]) > 0,
                        IdExclusivityLine = row["IdExclusivityLine"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdExclusivityLine"]),
                        Hagdara = row["Hagdara"] == DBNull.Value ? (short)0 : Convert.ToInt16(row["Hagdara"]),
                        Accessibility = row["Accessibility"] == DBNull.Value ? false : Convert.ToInt32(row["Accessibility"]) > 0,
                        AccountingGroupID = row["AccountingGroupID"] == DBNull.Value ? -1 : Convert.ToInt32(row["AccountingGroupID"]),
                        ValidExportDate = row["ValidExportDate"] == DBNull.Value || string.IsNullOrEmpty(row["ValidExportDate"].ToString()) ? null : row["ValidExportDate"].ToString(),
                    });
                }

            }
            return dicRouteLineList;
        }
        /// <summary>
        /// Get Physical Stops Raw Data
        /// </summary>
        /// <returns></returns>
        /// 
        public Dictionary<int, PhysicalStop> GetPhysicalStopsRawDataDictionary()
        {
            var dicPhysicalStopsRawDataDictionary = new Dictionary<int, PhysicalStop>();
            var filedNames = DalShared.IsTaxiOperator() ?
                new[] { "ID", "Longitude", "Latitude", "Direction", "Name", "ID_SEKER", "CorrectPhStopName", "LinkUserID", "Street", "House", "Across", "CityCode", "LongitudeN", "LatitudeN", "LINKID", "StationTypeId", "StationStatusId", "CityLinkCode", "FromPlatform", "ToPlatform", "Floor", "StationShortName", "IsTrainMainStation", "AreaOperatorID" } : 
                new[] { "ID", "Longitude", "Latitude", "Direction", "Name", "ID_SEKER", "CorrectPhStopName", "LinkUserID", "Street", "House", "Across", "CityCode", "LongitudeN", "LatitudeN", "LINKID", "StationTypeId", "StationStatusId", "CityLinkCode", "FromPlatform", "ToPlatform", "Floor", "StationShortName", "IsTrainMainStation" };

            var sentance = DalShared.IsTaxiOperator() ?
                "SELECT ID,Longitude,Latitude,Direction,Name,ID_SEKER,CorrectPhStopName, IsCityLinkedManualLinkUserID, Street, House, Across, CityCode, LongitudeN, LatitudeN, LINKID, StationTypeId , StationStatusId,CityLinkCode , FromPlatform, ToPlatform, Floor , StationShortName , IsTrainMainStation , AreaOperatorID  WHERE ID>0" :
                "SELECT ID,Longitude,Latitude,Direction,Name,ID_SEKER,CorrectPhStopName, IsCityLinkedManualLinkUserID, Street, House, Across, CityCode, LongitudeN, LatitudeN, LINKID, StationTypeId , StationStatusId,CityLinkCode , FromPlatform, ToPlatform, Floor , StationShortName , IsTrainMainStation  WHERE ID>0"; 
            var dt = FillData(filedNames, ExportConfigurator.GetConfig().PhisicalStopsLayerName, sentance);
            //SetReadOnlyFieldSet(ExportConfigurator.GetConfig().PhisicalStopsLayerName, PhysicalStopReadOnlyFields, true);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
               {
                    if (row["ID"] != DBNull.Value && Validators.IsNumeric(row["ID"]) && !dicPhysicalStopsRawDataDictionary.ContainsKey(Convert.ToInt32(row["ID"])))
                    {
                        dicPhysicalStopsRawDataDictionary.Add(Convert.ToInt32(row["ID"]), new PhysicalStop
                        {
                            Direction = row["Direction"] == DBNull.Value ? string.Empty : row["Direction"].ToString(),
                            ID = Convert.ToInt32(row["ID"]),
                            Latitude = row["Latitude"] == DBNull.Value ? 0 : Convert.ToInt32(row["Latitude"]),
                            Longitude = row["Longitude"] == DBNull.Value ? 0 : Convert.ToInt32(row["Longitude"]),
                            LatitudeN = row["LatitudeN"] == DBNull.Value ? 0 : Convert.ToInt32(row["LatitudeN"]),
                            LongitudeN = row["LongitudeN"] == DBNull.Value ? 0 : Convert.ToInt32(row["LongitudeN"]),
                            Name = row["CorrectPhStopName"] == DBNull.Value ? string.Empty :
                            StringHelper.ConvertToHebrewEncoding(row["CorrectPhStopName"].ToString()),
                            StationCatalog = row["ID_SEKER"] == DBNull.Value ? string.Empty : row["ID_SEKER"].ToString(),
                            StationShortName = row["StationShortName"] == DBNull.Value ? null : StringHelper.ConvertToHebrewEncoding(row["StationShortName"].ToString()),
                            LinkUserId = row["LinkUserID"] == DBNull.Value ? 0 : Convert.ToInt32(row["LinkUserID"]),
                            Street = row["Street"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Street"].ToString()),
                            House = row["House"] == DBNull.Value ? 0 : Convert.ToInt32(row["House"]),
                            Across = row["Across"] == DBNull.Value ? (byte)0 : Convert.ToByte(row["Across"]),

                            IdStationStatus = row["StationStatusId"] == DBNull.Value ? 0 : Convert.ToInt32(row["StationStatusId"]),
                            IdStationType = row["StationTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(row["StationTypeId"]),
                            CityLinkCode = row["CityLinkCode"] == DBNull.Value ? enmLinkStationCity.NonLinked : (enmLinkStationCity)Convert.ToInt16(row["CityLinkCode"]),
                            LinkId = row["LINKID"] == DBNull.Value ? 0 : Convert.ToInt32(row["LINKID"]),

                            FromPlatform = row["FromPlatform"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["FromPlatform"]),
                            ToPlatform = row["ToPlatform"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["ToPlatform"]),
                            Floor = row["Floor"] == DBNull.Value ? null : (short?)Convert.ToInt32(row["Floor"]),
                            CityCode = row["CityCode"] == DBNull.Value ? Zerrocity : StringHelper.ConvertToHebrewEncoding(row["CityCode"].ToString()),
                            IsTrainMainStation = row["IsTrainMainStation"] == DBNull.Value ? false : Convert.ToInt32(row["IsTrainMainStation"])==1,
                            //IsCityLinkedManual = row["IsCityLinkedManual"] == DBNull.Value ? false : Convert.ToInt32(row["IsCityLinkedManual"]) == 1
                            AreaOperatorId = DalShared.IsTaxiOperator() && row["AreaOperatorId"] != DBNull.Value ? (int?)row["AreaOperatorId"] : null 
                        });
                    }
                }
            }
            
            return dicPhysicalStopsRawDataDictionary;
        }


        public List<PhysicalStop> GetPhysicalStopsRawData()
        {
            //"ID","Name","Latitude","Longitude","Direction","ID_SEKER"
            var lst = new List<PhysicalStop>();
            var filedNames = new[] { "ID", "Longitude", "Latitude", "Direction", "Name", "ID_SEKER", "CorrectPhStopName", "LinkUserID", "Street", "House", "Across", "CityCode", "LongitudeN", "LatitudeN", "LINKID", "FromPlatform", "ToPlatform", "StationTypeId", "CityLinkCode", "StationStatusId", "Floor", "StationShortName", "IsTrainMainStation" };
            const string sentance = "SELECT ID,Longitude,Latitude,Direction,Name,ID_SEKER,CorrectPhStopName,LinkUserID, Street, House, Across, CityCode, LongitudeN, LatitudeN, LINKID ,FromPlatform ,ToPlatform, StationTypeId, CityLinkCode, StationStatusId, Floor, StationShortName ,IsTrainMainStation WHERE ID>0";
            //var filedNames = new[] { "ID", "Longitude", "Latitude", "Direction", "Name", "ID_SEKER", "CorrectPhStopName", "LinkUserID", "Street", "House", "Across", "CityCode", "LongitudeN", "LatitudeN", "LINKID", "FromPlatform", "ToPlatform", "StationTypeId", "CityLinkCode", "StationStatusId", "Floor", "StationShortName", "IsTrainMainStation", "IsCityLinkedManual" };
            //const string sentance = "SELECT ID,Longitude,Latitude,Direction,Name,ID_SEKER,CorrectPhStopName,LinkUserID, Street, House, Across, CityCode, LongitudeN, LatitudeN, LINKID ,FromPlatform ,ToPlatform, StationTypeId, CityLinkCode, StationStatusId, Floor, StationShortName ,IsTrainMainStation ,IsCityLinkedManual WHERE ID>0";
            var dt = FillData(filedNames, ExportConfigurator.GetConfig().PhisicalStopsLayerName, sentance);
            //SetReadOnlyFieldSet(ExportConfigurator.GetConfig().PhisicalStopsLayerName, PhysicalStopReadOnlyFields, true);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new PhysicalStop
                    {
                        Direction = row["Direction"] == DBNull.Value ? string.Empty : row["Direction"].ToString(),
                        ID = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"]),
                        Latitude = row["Latitude"] == DBNull.Value ? 0 : Convert.ToInt32(row["Latitude"]),
                        Longitude = row["Longitude"] == DBNull.Value ? 0 : Convert.ToInt32(row["Longitude"]),
                        LatitudeN = row["LatitudeN"] == DBNull.Value ? 0 : Convert.ToInt32(row["LatitudeN"]),
                        LongitudeN = row["LongitudeN"] == DBNull.Value ? 0 : Convert.ToInt32(row["LongitudeN"]),
                        Name = row["CorrectPhStopName"] == DBNull.Value ? string.Empty :
                        StringHelper.ConvertToHebrewEncoding(row["CorrectPhStopName"].ToString()),
                        StationCatalog = row["ID_SEKER"] == DBNull.Value ? string.Empty : row["ID_SEKER"].ToString(),
                        StationShortName = row["StationShortName"] == DBNull.Value ? null : StringHelper.ConvertToHebrewEncoding(row["StationShortName"].ToString()),

                        LinkUserId = row["LinkUserID"] == DBNull.Value ? 0 : Convert.ToInt32(row["LinkUserID"]),
                        Street = row["Street"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Street"].ToString()),
                        House = row["House"] == DBNull.Value ? 0 : Convert.ToInt32(row["House"]),
                        Across = row["Across"] == DBNull.Value ? (byte)0 : Convert.ToByte(row["Across"]),
                        LinkId = row["LINKID"] == DBNull.Value ? 0 : Convert.ToInt32(row["LINKID"]),
                        CityLinkCode = row["CityLinkCode"] == DBNull.Value ? enmLinkStationCity.NonLinked : (enmLinkStationCity)Convert.ToInt16(row["CityLinkCode"]),
                        CityCode = row["CityCode"] == DBNull.Value ? Zerrocity : StringHelper.ConvertToHebrewEncoding(row["CityCode"].ToString()),
                        FromPlatform = row["FromPlatform"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["FromPlatform"]),
                        ToPlatform = row["ToPlatform"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["ToPlatform"]),
                        IdStationType = row["StationTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(row["StationTypeId"]),
                        IdStationStatus = row["StationStatusId"] == DBNull.Value ? 0 : Convert.ToInt32(row["StationStatusId"]),
                        Floor = row["Floor"] == DBNull.Value ? null : (short?)Convert.ToInt32(row["Floor"]),
                        IsTrainMainStation = row["IsTrainMainStation"] == DBNull.Value ? false : Convert.ToInt32(row["IsTrainMainStation"]) == 1,
                        //IsCityLinkedManual = row["IsCityLinkedManual"] == DBNull.Value ? false : Convert.ToInt32(row["IsCityLinkedManual"]) == 1
                    });
                }
            }
            //PhysicalStop ps = lst.FindLast(f => f.ID == 1);
            return lst;
        }

        /// <summary>
        /// ExportEndPointsAsCSVFile
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportEndPointsAsCsvFile(string fileName)
        {
            try
            {
                Gisdk.DoFunction("SetLayer", ExportConfigurator.GetConfig().Endpoints);
                const string qry = "Select Longitude,Latitude,NodeID,ID where ID > 0";
                Gisdk.DoFunction("SelectByQuery", "Selection", "Several", qry);
                var options = new object[,] { { "Fields", new object[] { "Longitude", "Latitude", "NodeID","ID" } } };

                Gisdk.DoFunction("ExportCSV", string.Concat(ExportConfigurator.GetConfig().Endpoints, "|Selection"), fileName, options);

            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Is TranCad Process Is Launched
        /// </summary>
        /// <returns></returns>
        private bool IsTranCadProcessIsLaunched()
        {
            Process process = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.StartsWith("tcw"));
            return process != null;
        }
        /// <summary>
        /// Eged Get TransCad Current Envoromnet Info
        /// </summary>
        /// <returns></returns>
        public TransCadCurrentEnvoromnetInfo EgedGetTransCadCurrentEnvoromnetInfo()
        {
            TransCadCurrentEnvoromnetInfo transCadCurrentEnvoromnetInfo = new TransCadCurrentEnvoromnetInfo();
            try
            {
                if (!IsTranCadProcessIsLaunched())
                    throw new ApplicationException(Resources.ResourceDAL.TranCadIsNotRunning);
                transCadCurrentEnvoromnetInfo.MapLayerName = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                  
                object info = Gisdk.DoFunction("GetLayerInfo", ExportConfigurator.GetConfig().MapLayerName);
                object[] infoArray = CaliperForm.Connection.ObjectToArray(info, "string");
                transCadCurrentEnvoromnetInfo.MapLayerFile = infoArray[2].ToString();
                transCadCurrentEnvoromnetInfo.MapLayerFile = transCadCurrentEnvoromnetInfo.MapLayerFile.Replace(".BIN", ".dbd");
                transCadCurrentEnvoromnetInfo.IsNetworkDefined = false;
                info = Gisdk.DoFunction("GetLayers");
                info = Gisdk.DoFunction("ExtractArray", info, 1);
                object[] layersArrayData = CaliperForm.Connection.ObjectToArray(info, "string");
                transCadCurrentEnvoromnetInfo.LayersName = new List<string>();
                foreach (var item in layersArrayData)
                    transCadCurrentEnvoromnetInfo.LayersName.Add(item.ToString());
                transCadCurrentEnvoromnetInfo.IsMunLayerExists = transCadCurrentEnvoromnetInfo.LayersName.Exists(l => l == ExportConfigurator.GetConfig().MunLayer);
                return transCadCurrentEnvoromnetInfo;
            }
            catch 
            {
                //throw exc;
                return transCadCurrentEnvoromnetInfo;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// IsEgedWorkSpaceInTranscad
        /// </summary>
        /// <returns></returns>
        public bool IsEgedWorkSpaceInTranscad()
        {
            var gisdk = new Connection();
            try
            {
                gisdk.CloseOnError = true;
                gisdk.MappingServer = ExportConfigurator.GetConfig().MappingServer;
                bool isOpen = gisdk.Open();

                object info = Gisdk.DoFunction("GetLayerNames");
                object[] infoArray = CaliperForm.Connection.ObjectToArray(info, "string");

                if (infoArray == null)
                    return false;
                return infoArray.ToList().Exists(l => l != null && l.ToString().Equals(ExportConfigurator.GetConfig().EgedRouteSystemLayerName));
            }
            catch (Exception)
            {
                bool isClose = gisdk.Close();
                return false;
            }
            finally
            {
                bool isClose = gisdk.Close();
            }
        }
       

        /// <summary>
        /// IsEgedWorkSpaceInTranscad
        /// </summary>
        /// <returns></returns>
        public bool IsRouteSystemInTranscad()
        {
            try
            {
                object info = Gisdk.DoFunction("GetLayers");
                object[] infoArray = CaliperForm.Connection.ObjectToArray(info, "string");

                if (infoArray == null)
                    return false;
                return infoArray.ToList().Exists(l => l != null && l.Equals(ExportConfigurator.GetConfig().RouteSystemLayerName));
            }
            catch 
            {
                return false;
            }
        }


        /// <summary>
        /// Get TransCad Current Envoromnet Info
        /// </summary>
        /// <returns></returns>
        public TransCadCurrentEnvoromnetInfo GetTransCadCurrentEnvoromnetInfo()
        {
            var transCadCurrentEnvoromnetInfo = new TransCadCurrentEnvoromnetInfo();
            try
            {
                if (!IsTranCadProcessIsLaunched())
                {
                    throw new ApplicationException(Resources.ResourceDAL.TranCadIsNotRunning);
                }
                // Get Route System File
                object info = Gisdk.DoFunction("GetLayerInfo", ExportConfigurator.GetConfig().RouteStopsLayerName);
                object[] infoArray = Connection.ObjectToArray(info, "string");

                if (infoArray == null)
                    throw new ApplicationException(Resources.ResourceDAL.RouteSystemUnloaded);

                transCadCurrentEnvoromnetInfo.RouteSystemFile = infoArray[2].ToString();
                transCadCurrentEnvoromnetInfo.RouteSystemFile = transCadCurrentEnvoromnetInfo.RouteSystemFile.Replace("S.bin", ".rts");
                // Get Map Layer Name
                transCadCurrentEnvoromnetInfo.MapLayerName = ExportConfigurator.GetConfig().MapLayerName;// Gisdk.DoFunction("GetMap").ToString();
                // Get Map Layer File
                info = Gisdk.DoFunction("GetLayerInfo", ExportConfigurator.GetConfig().MapLayerName);
                infoArray = Connection.ObjectToArray(info, "string");
                transCadCurrentEnvoromnetInfo.MapLayerFile = infoArray[2].ToString();
                transCadCurrentEnvoromnetInfo.MapLayerFile = transCadCurrentEnvoromnetInfo.MapLayerFile.Replace(".BIN", ".dbd");
                //// Network
                //try
                //{
                //    Gisdk.DoFunction("ReadNetwork", ExportConfigurator.GetConfig().NetworkFile);
                //    transCadCurrentEnvoromnetInfo.IsNetworkDefined = true;
                //}
                //catch
                //{
                //    transCadCurrentEnvoromnetInfo.IsNetworkDefined = false;
                //}
                // Get Layers 
                info = Gisdk.DoFunction("GetLayers");
                info = Gisdk.DoFunction("ExtractArray", info, 1);
                object[] layersArrayData = CaliperForm.Connection.ObjectToArray(info, "string");
                transCadCurrentEnvoromnetInfo.LayersName = new List<string>();
                foreach (var item in layersArrayData)
                {
                    transCadCurrentEnvoromnetInfo.LayersName.Add(item.ToString());
                }
                transCadCurrentEnvoromnetInfo.IsMunLayerExists = transCadCurrentEnvoromnetInfo.LayersName.Exists(l => l == ExportConfigurator.GetConfig().MunLayer);
                return transCadCurrentEnvoromnetInfo;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// IsIs_STRAcrive
        /// </summary>
        /// <returns></returns>
        private bool IsIs_STRAcrive()
        {

            try
            {
                object value = Gisdk.DoFunction("GetMap");
                if (value == null)
                    return false;
                else
                    return value.ToString().StartsWith(ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// ShowStreetName
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isVisible"></param>
        public void ShowStreetName(string fieldName, bool isVisible)
        {
            try
            {

                if (IsIs_STRAcrive())
                {
                    //SetArrowheads("is_str|", "Flow")
                    object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;//Gisdk.DoFunction("GetMap");
                    SetLabelOnMap(map_name.ToString(), isVisible, fieldName, false);

                    ////string[] arrMapNames = map_name.ToString().Split(":".ToCharArray());
                    ////string map_namePart = map_name.ToString();
                    ////if (arrMapNames != null && arrMapNames.Length > 0)
                    ////    map_namePart = arrMapNames[0];
                    //object[] labelArray = null;
                    //if (isVisible)
                    //{
                    //    labelArray = new object[1];
                    //    labelArray[0] = new object[2] { "Visibility", "On" };
                    //}
                    //else
                    //{
                    //    labelArray = new object[1];
                    //    labelArray[0] = new object[2] { "Visibility", "Off" };
                    //}
                    //SetMapLabelOptions();
                    ////Gisdk.DoFunction("SetLayerVisibility", "is_mun", "False");
                    ////Gisdk.DoFunction("SetLayer", map_name.ToString());
                    ////Gisdk.DoFunction("SetLayerPosition", map_name, ExportConfigurator.GetConfig().RouteSystemLayerName,2);
                    //Gisdk.DoFunction("SetLabels", string.Concat(map_name, "|"), fieldName, labelArray);
                    //Gisdk.DoFunction("SetSelectAutoRedraw", "True");
                    //Gisdk.DoFunction("SetMapRedraw",map_name, "True");
                    //Gisdk.DoFunction("RedrawMap", map_name );
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="layer"></param>
        public void SetMapLabelOptions()
        {
            //            SetLabelOptions("U.S. States|", {
            //            {"Font", "Arial|Bold|22"},
            //            {"Color", ColorRGB(50000,50000,0)}
            //                })
            //Smart=True
            //Uniqueness=On
            //Kern To Fill=True
            //Font=David|Bold|22
            try
            {
                if (IsIs_STRAcrive())
                {
                    object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                    object[] labelArray = new object[1];
                    labelArray[0] = new object[2] { "Overlap", "False" };
                    // labelArray = new object[1];
                    Gisdk.DoFunction("SetMapLabelOptions", map_name, labelArray);
                    Gisdk.DoFunction("RedrawMap", map_name);
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Set Arrow Topology
        /// </summary>
        public void SetArrowTopology(string layer)
        {
            try
            {
                Gisdk.DoFunction("SetArrowheads", layer + "|", "Topology");
                Gisdk.DoFunction("SetSelectAutoRedraw", "True");
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }



        /// <summary>
        /// Show Street Flow
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isVisible"></param>
        public void ShowStreetFlow(bool isVisible)
        {
            try
            {
                string map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                if (IsIs_STRAcrive())
                {
                    string data = isVisible ? "Flow" : "None";
                    Gisdk.DoFunction("SetArrowheads", map_name + "|", data);
                    Gisdk.DoFunction("SetSelectAutoRedraw", "True");
                    Gisdk.DoFunction("RedrawMap", map_name);
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Save Map To Image
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveMapToImage(string fileName, string fileFormat)
        {
            try
            {
                // "JPEG" fileFormat
                //SetArrowheads("is_str|", "Flow")
                //object map_name = Gisdk.DoFunction("GetMap");
                object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                object[] arr = new object[1];
                arr[0] = new object[2] { "Quality", 100 };
                Gisdk.DoFunction("SaveMapToImage", map_name, fileName, fileFormat, arr);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Zoom To Layer Feuture By Id
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="IDs"></param>
        //public void ZoomToLayerFeutureById(string layerName,List<int> IDs,string title)
        //{
        //    try
        //    {
        //        if (!IsIs_STRAcrive())
        //            return;
        //        if (layerName.Equals(ExportConfiguration.ExportConfigurator.GetConfig().RouteSystemLayerName))
        //        {
        //            if (IDs.Count != 1)
        //                Gisdk.DoFunction("SetRouteSystemChanneling", layerName, "Sided", null);
        //            else
        //                Gisdk.DoFunction("SetRouteSystemChanneling", layerName, "None", null);
        //        }
        //        Gisdk.DoFunction("SetLayer",layerName);
        //        Gisdk.DoFunction("SetLayerVisibility", layerName, "True");
        //        //object map_name = Gisdk.DoFunction("GetMap");
        //        object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
        //        int[] idArray = IDs.ToArray();
        //        Gisdk.DoFunction("SelectByIDs","Selection","Several",idArray);
        //        object scope =  Gisdk.DoFunction("GetSetScope","Selection");
        //        Gisdk.DoFunction("SetMapScope",map_name,scope);
        //        Gisdk.DoFunction("SetMapTitle", map_name, title);
        //        //Gisdk.DoFunction("SetDisplayStatus", layerName + "|", "Invisible");

        //        Gisdk.DoFunction("SetDisplayStatus", layerName + "|Selection", "Active");
        //        // Gisdk.DoFunction("SetOffset", layerName + "|Selection", "Offset", 20);
        //        Gisdk.DoFunction("SetSelectDisplay", "True");
        //        Gisdk.DoFunction("SetSelectAutoRedraw","True"); 
        //        Gisdk.DoFunction("RedrawMap",map_name); 
        //    }
        //    catch (Exception exc)
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //        throw exc;
        //    }
        //    finally
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //    }
        //}

        /// <summary>
        /// Zoom To Layer Feuture By Id
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="IDs"></param>
        public void ZoomToLayerFeutureById(string layerName, List<int> IDs, string title)
        {
            try
            {
                Gisdk.DoFunction("SetLayer", layerName);
                Gisdk.DoFunction("SetSelectDisplay", "True");
                Gisdk.DoFunction("SetLayerVisibility", layerName, "True");
                if (layerName.ToLower().Equals(ExportConfiguration.ExportConfigurator.GetConfig().PhisicalStopsLayerName.ToLower()))
                    SetOffSet(string.Concat(layerName, "|"), "Offset", 10);
                object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                int[] idArray = IDs.ToArray();
                Gisdk.DoFunction("SelectByIDs", "Selection", "Several", idArray);
                Gisdk.DoFunction("SetRouteSystemChanneling", layerName, "Sided", null);
                object scope = Gisdk.DoFunction("GetSetScope", "Selection");
                Gisdk.DoFunction("SetMapScope", map_name, scope);
                Gisdk.DoFunction("SetMapTitle", map_name, title);
                Gisdk.DoFunction("SetDisplayStatus", layerName + "|", "Invisible");
                Gisdk.DoFunction("SetDisplayStatus", layerName + "|Selection", "Active");
                Gisdk.DoFunction("SetRouteSystemChanneling", layerName, "Sided", null);
                Gisdk.DoFunction("SetSelectAutoRedraw", "True");
                Gisdk.DoFunction("RedrawMap", map_name);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// EgedZoomToLayerFeutureById
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="IDs"></param>
        /// <param name="title"></param>
        //public void EgedZoomToLayerFeutureById(string layerName, List<int> IDs, string title, string whereClass)
        //{
        //    try     
        //    {
        //        int[] idArray = IDs.ToArray();
        //        StringBuilder sb = new StringBuilder(string.Concat("Select * from ", layerName, " where "));
        //        foreach (int item in idArray)
        //        {
        //            sb.AppendFormat(whereClass, item);
        //        }
        //        String strQuery = sb.ToString().Substring(0, sb.ToString().Length - 4);
        //        RunExportMacro("eZ", ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroFile, new List<object> { layerName, strQuery });
        //    }
        //    catch (Exception exc)
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //        throw exc;
        //    }
        //    finally
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //    }
        //}


        /// <summary>
        /// EgedZoomToStopLayerFeutureById
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="coordinares"></param>
        /// <param name="title"></param>
        /// <param name="whereClass"></param>
        //public void EgedZoomToStopLayerFeutureById(string layerName, List<Coord> coordinares, string title, string whereClass)
        //{
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder("Select * where ");
        //        foreach (Coord item in coordinares)
        //        {
        //            sb.AppendFormat(whereClass, item.Longitude, item.Latitude);
        //        }
        //        String strQuery = sb.ToString().Substring(0, sb.ToString().Length - 4);
        //        RunExportMacro("eZ", ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroFile, new List<object> { layerName, strQuery });

        //    }
        //    catch (Exception exc)
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //        throw exc;
        //    }
        //    finally
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //    }
        //}


        public void ExportFile(string layerName, string fileName)
        {
            try
            {
                object[] dummy = new object[] { };
                Gisdk.DoFunction("ExportArcViewShape", layerName, fileName, dummy);

            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }

        }
        /// <summary>
        /// Set Label On Map 
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isVisible"></param>
        /// <param name="fieldName"></param>
        public void SetLabelOnMap(string layerName, bool isVisible, string fieldName, bool isSelectAdded)
        {
            SetLabelOnMap(layerName, isVisible, fieldName, isSelectAdded, string.Empty);
        }
        /// <summary>
        /// Set Label Route Lines
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isVisible"></param>
        /// <param name="fieldName"></param>
        public void SetLabelOnMap(string layerName, bool isVisible, string fieldName, bool isSelectAdded, string imagePath)
        {
            try
            {
                //object map_name = Gisdk.DoFunction("GetMap");
                SetMapLabelOptions();
                object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                Gisdk.DoFunction("SetLayer", layerName);
                object[] labelArray = null;
                if (isVisible)
                {
                    labelArray = new object[7];
                    labelArray[0] = new object[2] { "Font", ExportConfigurator.GetConfig().LabelFont };
                    labelArray[1] = new object[2] { "Priority Expression", "Selection" };
                    labelArray[2] = new object[2] { "Smart", "False" };
                    labelArray[3] = new object[2] { "Uniqueness", "On" };
                    labelArray[4] = new object[2] { "Kern To Fill", "True" };
                    labelArray[5] = new object[2] { "Visibility", "On" };
                    labelArray[4] = new object[2] { "Rotation ", "True" };


                }
                else
                {
                    labelArray = new object[1];
                    labelArray[0] = new object[2] { "Visibility", "Off" };
                }
                string layerNameFull = String.Concat(layerName, "|");
                if (isSelectAdded)
                {
                    layerNameFull = String.Concat(layerNameFull, "Selection");
                }
                Gisdk.DoFunction("SetLabels", layerNameFull, fieldName, labelArray);
                if (isVisible && !string.IsNullOrEmpty(imagePath))
                {
                    Gisdk.DoFunction("SetIcon", layerName + "|Selection", "Color Bitmap", imagePath, 16);
                }
                Gisdk.DoFunction("SetLayerVisibility", layerName, isVisible ? "True" : "False");
                Gisdk.DoFunction("SetSelectAutoRedraw", "True");
                Gisdk.DoFunction("SetMapRedraw", map_name, "True");
                Gisdk.DoFunction("RedrawMap", map_name);
            }
            catch// (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                //throw exc;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }



        /// <summary>
        /// Delete Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="routeLineLayer"></param>
        /// <param name="lstDeletedStops"></param>
        /// <param name="routeStopLayer"></param>
        /// <returns></returns>
        public bool DeleteRouteLine(RouteLine routeLine, string routeLineLayer)
        {
            try
            {
                Gisdk.DoFunction("SetLayer", routeLineLayer);
                Gisdk.DoFunction("SetSelectDisplay", "True");
                //object map_name = Gisdk.DoFunction("GetMap");
                object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                string name = GetRouteNameByRouteID(routeLine.ID, routeLineLayer);
                Gisdk.DoFunction("DeleteRoute", routeLineLayer, name);
                Gisdk.DoFunction("RedrawMap", map_name);
                return true;
            }
            catch
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                //LoggerManager.WriteToLog(exc);
                return true;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeStop"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool DeleteRouteStop(RouteStop routeStop, string layer)
        {
            try
            {
                Gisdk.DoFunction("SetLayer", layer);
                Gisdk.DoFunction("DeleteRecord", layer, routeStop.ID.ToString());
                //object map_name = Gisdk.DoFunction("GetMap");
                object map_name = ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName;
                Gisdk.DoFunction("RedrawMap", map_name);
                return true;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        #region TableStructure
        public List<string> GetLayerFields(string layerName)
        {
            try
            {
                List<string> dataList = new List<string>();
                object fields = Gisdk.DoFunction("GetFields", layerName, "All");
                object fieldsFirstElement = Gisdk.DoFunction("ExtractArray", fields, 1);
                object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsFirstElement, "string");
                fieldsArrayData.ToList<object>().ForEach(o => dataList.Add(o.ToString()));
                return dataList;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Add New Field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="lstFields"></param>
        /// <param name="modData"></param>
        private void AddNewField(string fieldName, List<string> lstFields, List<object[]> modData, string fieldType, short width)
        {
            if (!modData.IsListFull() || !lstFields.IsListFull())
            {
                return;
            }
            if (!lstFields.Contains(fieldName)) //  "Integer" 0
            {
                object[] fieldsIdOperator = new object[] { fieldName, fieldType, width, 0, "false", null, null, null, null, null, null, fieldName };
                modData.Add(fieldsIdOperator);
            }
        }
        /// <summary>
        /// Add Route Line Fields
        /// </summary>
        /// <param name="layerName"></param>
        private void AddRouteLineFields(string layerName)
        {
            try
            {
                List<string> lstFields = GetLayerFields(layerName);
                List<object[]> modData = new List<object[]>();
                object strct = Gisdk.DoFunction("GetTableStructure", layerName);
                for (int i = 0; i < lstFields.Count; i++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", strct, i + 1);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    modData.Add(fieldsArrayData);
                }

                AddNewField("Catalog", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdOperator", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdCluster", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("RouteNumber", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdServiceType", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdSeasonal", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdZoneHead", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdZoneSubHead", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IsBase", lstFields, modData, IntegerConst, IntegerWidthOneConst);
                object[] modifiedFieldData = new object[modData.Count];
                string dataAll = string.Empty;
                for (int j = 0; j < modData.Count; j++)
                {
                    object[] arrData = new object[modData[j].Length + 1];
                    for (int k = 0; k < modData[j].Length; k++)
                    {
                        arrData[k] = modData[j][k];
                    }
                    arrData[2] = int.Parse(arrData[2].ToString());
                    arrData[3] = int.Parse(arrData[3].ToString());
                    arrData[modData[j].Length] = arrData[0];
                    modifiedFieldData[j] = arrData;
                }
                Gisdk.DoFunction("ModifyTable", layerName, modifiedFieldData);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Add Physical Stops Fields
        /// </summary>
        /// <param name="layerName"></param>
        private void AddPhysicalStopsFields(string layerName)
        {
            try
            {
                List<string> lstFields = GetLayerFields(layerName);
                List<object[]> modData = new List<object[]>();
                object strct = Gisdk.DoFunction("GetTableStructure", layerName);
                for (int i = 0; i < lstFields.Count; i++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", strct, i + 1);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    modData.Add(fieldsArrayData);
                }
                //AddNewField("StationCatalog", lstFields, modData,integerConst,integerWidthConst);
                AddNewField("CorrectPhStopName", lstFields, modData, Characterconst, 50);
                object[] modifiedFieldData = new object[modData.Count];
                string dataAll = string.Empty;
                for (int j = 0; j < modData.Count; j++)
                {
                    object[] arrData = new object[modData[j].Length + 1];
                    for (int k = 0; k < modData[j].Length; k++)
                    {
                        arrData[k] = modData[j][k];
                    }
                    arrData[2] = int.Parse(arrData[2].ToString());
                    arrData[3] = int.Parse(arrData[3].ToString());
                    arrData[modData[j].Length] = arrData[0];
                    modifiedFieldData[j] = arrData;
                }
                Gisdk.DoFunction("ModifyTable", layerName, modifiedFieldData);

                //object map_name = Gisdk.DoFunction("GetMap");
                //Gisdk.DoFunction("SetSelectAutoRedraw", "True");
                //Gisdk.DoFunction("RedrawMap", map_name); 

            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Add Route Stop Fields
        /// </summary>
        /// <param name="layerName"></param>
        private void AddRouteStopFields(string layerName)
        {
            try
            {
                List<string> lstFields = GetLayerFields(layerName);
                List<object[]> modData = new List<object[]>();
                object strct = Gisdk.DoFunction("GetTableStructure", layerName);
                for (int i = 0; i < lstFields.Count; i++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", strct, i + 1);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    modData.Add(fieldsArrayData);
                }

                AddNewField("IdStationType", lstFields, modData, IntegerConst, IntegerWidthConst);
                AddNewField("IdTempStatus", lstFields, modData, IntegerConst, IntegerWidthConst);
                //string fieldName = "IdStationType";
                //if (modData.IsListFull())
                //{
                //   if (!lstFields.Contains(fieldName))
                //   {
                //      object[] fieldsIdOperator = new object[] { fieldName, "Integer", 8, 0, "false", null, null, null, null, null, null, fieldName };
                //      modData.Insert(5,fieldsIdOperator);
                //   }
                //}
                object[] modifiedFieldData = new object[modData.Count];
                string dataAll = string.Empty;
                for (int j = 0; j < modData.Count; j++)
                {
                    object[] arrData = new object[modData[j].Length + 1];
                    for (int k = 0; k < modData[j].Length; k++)
                    {
                        arrData[k] = modData[j][k];
                    }
                    arrData[2] = int.Parse(arrData[2].ToString());
                    arrData[3] = int.Parse(arrData[3].ToString());
                    arrData[modData[j].Length] = arrData[0];
                    modifiedFieldData[j] = arrData;
                }
                Gisdk.DoFunction("ModifyTable", layerName, modifiedFieldData);

            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Delete Old Route Line Fields
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="deletedFeildList"></param>
        private void DeleteOldRouteLineFields(string layerName, List<string> deletedFeildList)
        {
            try
            {
                List<string> lstFields = GetLayerFields(layerName);
                List<object[]> modData = new List<object[]>();
                object strct = Gisdk.DoFunction("GetTableStructure", layerName);
                for (int i = 0; i < lstFields.Count; i++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", strct, i + 1);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    if (!deletedFeildList.Contains(lstFields[i]))
                    {
                        modData.Add(fieldsArrayData);
                    }
                }
                object[] modifiedFieldData = new object[modData.Count];
                string dataAll = string.Empty;
                for (int j = 0; j < modData.Count; j++)
                {
                    object[] arrData = new object[modData[j].Length + 1];
                    for (int k = 0; k < modData[j].Length; k++)
                    {
                        arrData[k] = modData[j][k];
                    }
                    arrData[2] = int.Parse(arrData[2].ToString());
                    arrData[3] = int.Parse(arrData[3].ToString());
                    arrData[modData[j].Length] = arrData[0];
                    modifiedFieldData[j] = arrData;
                }
                Gisdk.DoFunction("ModifyTable", layerName, modifiedFieldData);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// Delete Old Route Line Fields
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="deletedFeildList"></param>
        private void DeleteOldRouteStopFields(string layerName, List<string> deletedFeildList)
        {
            try
            {
                List<string> lstFields = GetLayerFields(layerName);
                List<object[]> modData = new List<object[]>();
                object strct = Gisdk.DoFunction("GetTableStructure", layerName);
                for (int i = 0; i < lstFields.Count; i++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", strct, i + 1);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    if (!deletedFeildList.Contains(lstFields[i]))
                    {
                        modData.Add(fieldsArrayData);
                    }
                }
                object[] modifiedFieldData = new object[modData.Count];
                string dataAll = string.Empty;
                for (int j = 0; j < modData.Count; j++)
                {
                    object[] arrData = new object[modData[j].Length + 1];
                    for (int k = 0; k < modData[j].Length; k++)
                    {
                        arrData[k] = modData[j][k];
                    }
                    arrData[2] = int.Parse(arrData[2].ToString());
                    arrData[3] = int.Parse(arrData[3].ToString());
                    arrData[modData[j].Length] = arrData[0];
                    modifiedFieldData[j] = arrData;
                }
                Gisdk.DoFunction("ModifyTable", layerName, modifiedFieldData);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// Is Real Data Has Approximatly Data
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="realData"></param>
        /// <returns></returns>
        private string GetRealDataHasApproximatlyData(string tableName, string realData)
        {
            TranslatorEntity entry = GlobalData.BaseTableTranslatorList.Find(e => e.EnglishName == tableName);
            if (entry != null)
            {
                if (entry.RealPossibleValues != null && entry.RealPossibleValues.Count > 0)
                {
                    foreach (var item in entry.RealPossibleValues.Keys)
                    {
                        List<string> values = entry.RealPossibleValues[item];
                        if (values != null && values.Exists(s => s == realData))
                        {
                            return item;
                        }
                    }

                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Replace Field Info
        /// </summary>
        /// <param name="layerName"></param>
        private void ReplaceRouteLineFieldInfo(string layerName)
        {
            int IdSeasonalConst = 0;// Temporary cast Seasenal
            try
            {
                string errorString = "Route System ID {0}. Field name {1} was not defined properly. The old value {2}. The new value {3}";
                string okayString = "Route System ID {0} was replaced succesefully";
                IInternalBaseDal dalSQLDB = new InternalTvunaImplementationDal();
                Dictionary<string, List<BaseTableEntity>> dicBaseTable = dalSQLDB.GetBaseTableEntityList(0);
                string[] filedNames = new string[] { "Route_ID", "Route_Name", "Catalog", "Signpost", "Dir", "Var", "VarNum", "Company", "Operator", "Cluster", "Branch_Name", "Route_Type", "Service_Type", "Route_Len", "Description", "MAKAT", "Seasonal", "RoadDescription", "Mahoz_Roshi", "Mahoz_Mishne" };
                string sentance = "SELECT Route_ID,Route_Name,Signpost,Dir,Var,VarNum,Company,Operator,Cluster,Branch_Name,Route_Type,Service_Type,Route_Len,Description,MAKAT,Seasonal,RoadDescription,Mahoz_Roshi,Mahoz_Mishne,IsNew WHERE Route_ID>0";
                DataTable dt = FillData(filedNames, ExportConfigurator.GetConfig().RouteSystemLayerName, sentance);

                //List<BaseLineInfo> rlBasedList = dalSQLServer.GetBaseLine(new Operator { IdOperator = 8 });
                List<BaseLineInfo> rlBasedList = null;

                if (dt.IsDataTableFull())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int idOperator = 0;
                        if (dr["Operator"] != DBNull.Value && Validators.IsNumeric(dr["Operator"].ToString()))
                            idOperator = int.Parse(dr["Operator"].ToString());
                        if (rlBasedList == null)
                        {
                            rlBasedList = _dalSqlServer.GetBaseLine(new Operator { IdOperator = idOperator });
                        }

                        //string[] setDataArray = new string[] { dr["Route_ID"].ToString()};
                        //object recordFound = Gisdk.DoFunction("LocateRecord", layerName + "|", "Route_ID",setDataArray, null);
                        int? catalog; short? direction; string variant = string.Empty;
                        try
                        {
                            catalog = dr["MAKAT"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["MAKAT"]);
                            direction = dr["Dir"] == DBNull.Value ? null : (short?)Convert.ToInt16(dr["Dir"].ToString());
                            variant = dr["Var"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(dr["Var"].ToString());
                            RecreatedArgs args = new RecreatedArgs();
                            Hashtable hs = new Hashtable();
                            hs.Add("Catalog", catalog);
                            int RouteNumber = 0;
                            if (dr["Signpost"] != DBNull.Value)
                            {
                                string signpost = dr["Signpost"].ToString();
                                if (signpost.StartsWith("0"))
                                {
                                    signpost = signpost.Substring(1);
                                    hs.Add("Signpost", signpost);
                                }
                                RouteNumber = dr["Signpost"].ToString().ExtractNumbersOnly();
                            }
                            hs.Add("RouteNumber", RouteNumber);
                            if (RouteNumber == 0)
                            {
                                args.ErrorString = string.Format(errorString, dr["Route_ID"].ToString(), "RouteNumber", "Null", "0");
                                if (RouteLineWasRecreated != null)
                                    RouteLineWasRecreated(null, args);
                            }

                            hs.Add("IdOperator", idOperator);
                            if (idOperator == 0)
                            {
                                args.ErrorString = string.Format(errorString, dr["Route_ID"].ToString(), "Operator", "Null", "0");
                                if (RouteLineWasRecreated != null)
                                    RouteLineWasRecreated(null, args);
                            }
                            int idCluster = 0;
                            if (dr["Cluster"] != DBNull.Value && Validators.IsNumeric(dr["Cluster"].ToString()))
                                idCluster = int.Parse(dr["Cluster"].ToString());
                            hs.Add("IdCluster", idCluster);
                            if (idCluster == 0)
                            {
                                args.ErrorString = string.Format(errorString, dr["Route_ID"].ToString(), "Cluster", "Null", "0");
                                if (RouteLineWasRecreated != null)
                                    RouteLineWasRecreated(null, args);
                            }

                            // IsBase
                            int varNum = -1;
                            if (dicBaseTable[GlobalConst.baseVarConverter] != null)
                            {

                                BaseTableEntity bte = (dicBaseTable[GlobalConst.baseVarConverter].Find(e => e.Name == variant));
                                if (bte != null)
                                    varNum = bte.ID;
                            }
                            if (rlBasedList.IsListFull() && rlBasedList.Exists(bl =>
                                bl.Catalog == catalog && bl.Direction == direction &&
                                bl.Variant == varNum.ToString() && bl.IdCluster == idCluster))
                            {
                                hs.Add("IsBase", 1);
                            }
                            else
                            {
                                hs.Add("IsBase", 0);
                            }




                            int IdServiceType = 0;
                            if (dicBaseTable[GlobalConst.baseServiceType] != null)
                            {
                                if (dr["Service_Type"] != DBNull.Value)
                                {
                                    string hebName = StringHelper.ConvertToHebrewEncoding(dr["Service_Type"].ToString());
                                    BaseTableEntity be = dicBaseTable[GlobalConst.baseServiceType].FirstOrDefault(it => (hebName.Equals(it.Name)));
                                    if (be != null)
                                        IdServiceType = be.ID;

                                    else
                                    {
                                        if (new List<string>() { "מיוחד", "אקספרס", "מיותר", "רגולר", "נ.מ.ק.", "נ.מ.ל.ק.", "מאסף" }.Exists(el => el.Trim().Equals(hebName.Trim())))
                                        {
                                            IdServiceType = 5;
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(hebName))
                                            {
                                                hebName = GetRealDataHasApproximatlyData(GlobalConst.baseServiceType, hebName);
                                                if (!string.IsNullOrEmpty(hebName))
                                                {
                                                    be = dicBaseTable[GlobalConst.baseRouteType].FirstOrDefault(it => (hebName.Equals(it.Name)));
                                                    if (be != null)
                                                        IdServiceType = be.ID;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            hs.Add("IdServiceType", IdServiceType);
                            if (IdServiceType == 0)
                            {
                                args.ErrorString = string.Format(errorString, dr["Route_ID"].ToString(), "IdServiceType", dr["Service_Type"] == DBNull.Value ? "Null" : StringHelper.ConvertToHebrewEncoding(dr["Service_Type"].ToString()), "0");
                                if (RouteLineWasRecreated != null)
                                    RouteLineWasRecreated(null, args);
                            }
                            int idSeasonal = 0;

                            idSeasonal = IdSeasonalConst;
                            hs.Add("IdSeasonal", idSeasonal);


                            //int idZoneHead = 0;
                            //if (GlobalData.ClusterToZoneDictionary.IsDictionaryFull())
                            //{
                            //    ClusterToZone clusterToZone = GlobalData.ClusterToZoneDictionary[idCluster];
                            //    if (clusterToZone != null)
                            //    {
                            //        clusterToZone.ClusterStateList.FirstOrDefault(s=>s.MainZoneID==)
                            //        idZoneHead = clusterToZone.MainZoneID;
                            //    }
                            //}
                            //hs.Add("IdZoneHead", idZoneHead);
                            //hs.Add("IdZoneSubHead", idZoneHead);
                            object data = CaliperForm.Connection.TableToArray(hs);
                            Gisdk.DoFunction("SetRecordValues", layerName, dr["Route_ID"].ToString(), data);
                            if (string.IsNullOrEmpty(args.ErrorString))
                            {
                                args.ErrorString = string.Format(okayString, dr["Route_ID"].ToString());
                                if (RouteLineWasRecreated != null)
                                    RouteLineWasRecreated(null, args);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        
        public void ReCreateTableStructure(string layerRouteLineName, string layerRouteStopName, string phisicalStopsLayerName)
        {
            try
            {
                // Route Lines
                AddRouteLineFields(layerRouteLineName);
                DeleteOldRouteLineFields(layerRouteLineName, new List<string> { "Flag" });
                ReplaceRouteLineFieldInfo(layerRouteLineName);
                DeleteOldRouteLineFields(layerRouteLineName, new List<string> { "MAKAT","Seasonal","CCSTYLE","Route_CID","Route_UID","VarNum","Company","Operator","Cluster",
                    "Branch_Name","Route_Type","Service_Type","From_Place","To_Place","Way_Place","Route_Len",
                "Stops_Num","License","Active","Count","History","Description","tmp_uid","Flag","Mahoz_Roshi","Mahoz_Mishne"});

                // Route Stops
                ReplaceRouteStopFieldInfo(layerRouteStopName);
                DeleteOldRouteStopFields(layerRouteStopName, new List<string> { "Route_UID", "Route_Name", "Stop_Name",
                "Ph_UID", "DistFromNTN" ,"BranchName","StopName","Ordinal","Embarkation","Landing"
                ,"TotalDistFromStart","Active","Index","Egged_id","RouteName"});


                DeleteOldRouteLineFields(phisicalStopsLayerName, new List<string> { "StopMakat", "CodeArea", 
                    "STOPNAME",
                    "ADRESS", "CITY",
                "Flag", "DanArea", "[City:1]" });

                //Phisical Stops
                //AddPhysicalStopsFields(phisicalStopsLayerName);
                // Replace Data in the PhysicalRouteStop layer
                //ReplacePhysicalRouteStopFieldInfo(phisicalStopsLayerName);

                return;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Replace Route Stop Field Info
        /// </summary>
        /// <param name="layerRouteStopName"></param>
        private void ReplaceRouteStopFieldInfo(string layerName)
        {
            try
            {
                string[] filedNames = new string[] { "ID", "Embarkation", "Landing", "Stop_UID" };
                string sentance = "SELECT ID,Embarkation,Landing,Stop_UID WHERE ID>0";
                DataTable dt = FillData(filedNames, ExportConfigurator.GetConfig().RouteStopsLayerName, sentance);
                if (dt.IsDataTableFull())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            RecreatedArgs args = new RecreatedArgs();
                            Hashtable hs = new Hashtable();
                            bool? bEmbarkation = dr["Embarkation"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(dr["Embarkation"]);
                            bool? bLanding = dr["Landing"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(dr["Landing"]);
                            int idStationType = 0;
                            if (bEmbarkation == true && bLanding == true)
                            {
                                idStationType = 3;
                            }
                            else if (bEmbarkation == true && bLanding == false)
                            {
                                idStationType = 1;
                            }
                            else if (bEmbarkation == false && bLanding == true)
                            {
                                idStationType = 2;
                            }
                            else
                            {
                                idStationType = 0;
                            }
                            hs.Add("Stop_UID", idStationType);


                            object data = CaliperForm.Connection.TableToArray(hs);
                            Gisdk.DoFunction("SetRecordValues", layerName, dr["ID"].ToString(), data);
                        }
                        catch (Exception exp)
                        {
                            LoggerManager.WriteToLog(exp);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        public event EventHandler<RecreatedArgs> RouteLineWasRecreated;



        /// <summary>
        /// Replace Physical Route Stop Field Info
        /// </summary>
        /// <param name="layerRouteStopName"></param>
        private void ReplacePhysicalRouteStopFieldInfo(string layerName)
        {
            try
            {
                string[] filedNames = new string[] { "ID", "Name" };
                string sentance = "SELECT ID,Name WHERE ID>0";
                DataTable dt = FillData(filedNames, layerName, sentance);
                if (dt.IsDataTableFull())
                {
                    int counter = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            RecreatedArgs args = new RecreatedArgs();
                            Hashtable hs = new Hashtable();
                            hs.Add("CorrectPhStopName", string.Format("{0} - {1}", dr["Name"].ToString(), ++counter));
                            object data = CaliperForm.Connection.TableToArray(hs);
                            Gisdk.DoFunction("SetRecordValues", layerName, dr["ID"].ToString(), data);
                        }
                        catch (Exception exp)
                        {
                            LoggerManager.WriteToLog(exp);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        #endregion
        #region Route Line
        /// <summary>
        /// Get Route Name By Route ID
        /// </summary>
        /// <param name="rl"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private string GetRouteNameByRouteID(int routeID, string layerName)
        {
            try
            {
                //if (Gisdk == null || !gisdk.IsConnected())
                //{
                //    gisdk = new Connection();
                //    gisdk.CloseOnError = true;
                //    gisdk.MappingServer = ExportConfigurator.GetConfig().MappingServer;
                //    gisdk.Open();
                //}
                //Gisdk.DoFunction("SetLayer", layerName);
                object name = Gisdk.DoFunction("GetRouteNam", layerName, routeID);
                if (name != null)
                    return name.ToString();
                else
                    return string.Empty;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return string.Empty;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Calculate Route Line
        /// </summary>
        /// <param name="rl"></param>
        /// <param name="layerName"></param>
        /// <param name="linkLayer"></param>
        /// <returns></returns>
        public decimal CalculateRouteLine(RouteLine rl, string layerName, string linkLayer)
        {

            List<RouteStop> rsFoundList = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == rl.ID);
            int last = (from p in rsFoundList
                        select p.Ordinal).ToList<int>().Max();

            var res = (from p in rsFoundList
                       where p.Ordinal == last
                       select p.Milepost).Single();

            return Convert.ToDecimal(res);

            // OLD version
            //decimal sum = 0;
            //try
            // {

            //   string name = GetRouteNameByRouteID(rl.ID, layerName);
            //   List<RouteLink> routeLinks = GetRouteLinkList(rl, layerName, linkLayer);
            //   if (routeLinks.IsListFull())
            //   {
            //       routeLinks.ForEach(routeLink => sum += routeLink.Length);
            //   }
            //   return sum;
            //}
            //catch (Exception exc)
            //{
            //    if (gisdk != null && gisdk.IsConnected())
            //        gisdk.Close();
            //    LoggerManager.WriteToLog(exc);
            //    return sum;

            //}
            //finally
            //{
            //    if (gisdk != null && gisdk.IsConnected())
            //        gisdk.Close();
            //}
        }
        #endregion

        #region ITransCadMunipulationDataDAL Members

        /// <summary>
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool SaveRouteZipFileInfo(int idRoute, string zipFile, string layerName)
        {
            try
            {
                SetReadOnlyFieldSet(layerName, RouteLineReadOnlyFields, false);
                Hashtable hs = new Hashtable();
                hs.Add("ZipFile", zipFile);
                object data = CaliperForm.Connection.TableToArray(hs);
                Gisdk.DoFunction("SetRecordValues", layerName, idRoute.ToString(), data);
                return true;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false;

            }
            finally
            {
                SetReadOnlyFieldSet(layerName, RouteLineReadOnlyFields, true);
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool SaveRouteLine(RouteLine routeLine, string layerName)
        {
            try
            {
                SetReadOnlyFieldSet(layerName, RouteLineReadOnlyFields, false);
                var hs = new Hashtable
                             {
                                 {"Catalog", routeLine.Catalog},
                                 {"Dir", routeLine.Dir},
                                 {"Var", StringHelper.ConvertToDefaultFrom1255(routeLine.Var)},
                                 {"RouteNumber", routeLine.RouteNumber},
                                 {"IdOperator", routeLine.IdOperator},
                                 {"IdCluster", routeLine.IdCluster},
                                 {"IdServiceType", routeLine.IdServiceType},
                                 {"IdSeasonal", routeLine.IdSeasonal},
                                 {"IdZoneHead", routeLine.IdZoneHead},
                                 {"IdZoneSubHead", routeLine.IdZoneSubHead},
                                 {"AccountingGroupID", routeLine.AccountingGroupID},
                                 {"IdExclusivityLine", routeLine.IdExclusivityLine},
                                 {"Signpost", StringHelper.ConvertToDefaultFrom1255(routeLine.Signpost)},
                                 {"Hagdara", routeLine.Hagdara},
                                 {"IsBase", routeLine.IsBase ? 1 : 0},
                                 {"RoadDescription", StringHelper.ConvertToDefaultFrom1255(routeLine.RoadDescription)},
                                 {"Accessibility", routeLine.Accessibility ? 1 : 0}
                             };
                if (GlobalData.LoginUser.IsSuperViser)
                {
                    if (!string.IsNullOrEmpty(routeLine.ValidExportDate))
                    {
                        var tmp = routeLine.ValidExportDate.Replace(" ", string.Empty);
                        if (tmp == "//")
                            routeLine.ValidExportDate = string.Empty;
                    }
                    else
                    {
                        routeLine.ValidExportDate = string.Empty;
                    }
                    hs.Add("ValidExportDate", routeLine.ValidExportDate);
                }
                var data = Connection.TableToArray(hs);
                Gisdk.DoFunction("SetRecordValues", layerName, routeLine.ID.ToString(), data);
                return true;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false;

            }
            finally
            {
                SetReadOnlyFieldSet(layerName, RouteLineReadOnlyFields, true);
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Save Route Stop
        /// </summary>
        /// <param name="routeStop"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public bool SaveRouteStop(RouteStop routeStop, string layerName)
        {
            try
            {
                SetReadOnlyFieldSet(layerName, RouteStopReadOnlyFields, false);
                var hs = new Hashtable
                             {
                                 {"Horada", routeStop.Horada ?? 0}, 
                                 {"Stop_UID", routeStop.IdStationType ?? 3}
                             };
                if (string.IsNullOrEmpty(routeStop.Platform))
                    routeStop.Platform = string.Empty;
                hs.Add("Platform", StringHelper.ConvertToDefaultFrom1255(routeStop.Platform));

                object data = Connection.TableToArray(hs);
                Gisdk.DoFunction("SetRecordValues", layerName, routeStop.ID.ToString(), data);
                return true;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false;

            }
            finally
            {
                SetReadOnlyFieldSet(layerName, RouteStopReadOnlyFields, true);
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        //<summary>
        //GetLinkIds
        //</summary>
        //<param name="routeLine"></param>
        //<returns></returns>
        public List<RouteLink> GetRouteLinkList(RouteLine routeLine, string layerName, string linkLayer)
        {
            List<RouteLink> results = new List<RouteLink>();
            try
            {
                if (routeLine == null) return null;
                int maxCounter = 1000000;
                string routeName = GetRouteNameByRouteID(routeLine.ID, layerName);
                while (string.IsNullOrEmpty(routeName))
                {
                    routeName = GetRouteNameByRouteID(routeLine.ID, layerName);
                    if (maxCounter == 0)
                        break;
                    maxCounter--;
                }
                if (string.IsNullOrEmpty(routeName)) return null;
                object links = Gisdk.DoFunction("GetRouteLinks", layerName, routeName);
                object arrlen = Gisdk.DoFunction("ArrayLength", links);

                for (int counter = 1; counter <= Convert.ToInt32(arrlen); counter++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", links, counter);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    int[] arrayData = new int[2] { int.Parse(fieldsArrayData[0].ToString()), int.Parse(fieldsArrayData[1].ToString()) };
                    RouteLink rl = new RouteLink
                    {
                        LinkId = arrayData[0],
                        TraverseDirection = arrayData[1]
                    };
                    object[] data = new object[1];
                    object[] datatest = new object[0];
                    data[0] = arrayData;
                    try
                    {

                        //object link_layer = Gisdk.DoFunction("GetLinkLayer", "Endpoints");
                        //object node_layer_name = Gisdk.DoFunction("GetNodeLayer", link_layer);
                        object nodeIds = Gisdk.DoFunction("GetNodesFromLinks", linkLayer, "ID", "NodeID", data);
                        object nodesElement = Gisdk.DoFunction("ExtractArray", nodeIds, 1);
                        object[] nodesElementData = CaliperForm.Connection.ObjectToArray(nodesElement, "int");
                        rl.NodeIdFirst = int.Parse(nodesElementData[0].ToString());
                        rl.NodeIdLast = int.Parse(nodesElementData[1].ToString());

                        object length = Gisdk.DoFunction("GetRecordValues", linkLayer, rl.LinkId.ToString(), new object[] { "Length" });
                        object lengthElement = Gisdk.DoFunction("ExtractArray", length, 1);
                        object[] lengthElementData = CaliperForm.Connection.ObjectToArray(lengthElement, "string");
                        rl.Length = Convert.ToInt32(Convert.ToDouble(lengthElementData[1]) * ExportConfigurator.GetConfig().LengthConverter);

                        results.Add(rl);
                    }
                    catch (Exception exp)
                    {
                        if (_gisdk != null && _gisdk.IsConnected())
                            _gisdk.Close();
                        LoggerManager.WriteToLog(exp);
                        throw exp;
                    }
                }
                return results;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }

        }


        /// <summary>
        /// Get Route Link List
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="layerName"></param>
        /// <param name="linkLayer"></param>
        /// <returns></returns>
        public List<RouteLink> GetRouteLinkListForDuration(RouteLine routeLine, string layerName, string linkLayer, string endPointLayer)
        {
            List<RouteLink> results = new List<RouteLink>();
            try
            {
                if (routeLine == null) return null;
                string routeName = GetRouteNameByRouteID(routeLine.ID, layerName);
                object links = Gisdk.DoFunction("GetRouteLinks", layerName, routeName);
                object arrlen = Gisdk.DoFunction("ArrayLength", links);
                for (int counter = 1; counter <= Convert.ToInt32(arrlen); counter++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", links, counter);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    int[] arrayData = new int[2] { int.Parse(fieldsArrayData[0].ToString()), int.Parse(fieldsArrayData[1].ToString()) };
                    RouteLink rl = GetLinkAttributesById(linkLayer, endPointLayer, arrayData[0]);
                    rl.TraverseDirection = arrayData[1];
                    results.Add(rl);
                }
                return results;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }

        }

        #endregion
        #region Base Data


        public List<PriceZoneArea> GetPriceZoneAreas(string nodeLayerName)
        {
            var lstResults = new List<PriceZoneArea>();
            try
            {
                var filedNames = new[] { "PName", "ZName", "TatFull", "ID", "Area","Ring" };
                const string sentance = "SELECT PName,ZName,TatFull,ID, Area, Ring WHERE ID>0";
                DataTable dt = FillData(filedNames, nodeLayerName, sentance);
                if (dt.IsDataTableFull())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string tatFull = (row["TatFull"] == DBNull.Value ? string.Empty : row["TatFull"].ToString());
                        var id = Convert.ToInt32(row["ID"]);
                        string zName = (row["ZName"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["ZName"].ToString()));
                        double? area = (row["Area"] == DBNull.Value ? (double?) null : Convert.ToDouble(row["Area"]));
                        int? ring = (row["Ring"] == DBNull.Value ? (int?) null : Convert.ToInt32(row["Ring"]));
                        string pName = (row["PName"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["PName"].ToString()));
                        lstResults.Add(new PriceZoneArea
                            {
                                ID = id,
                                ZName = zName,
                                PName = pName,
                                TatFull = tatFull,
                                Ring = ring,
                                Area = area
                            });
                        
                    }
                }

                return lstResults;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        public List<City> GetCities(string nodeLayerName)
        {
            var lstResults = new List<City>();
            try
            {
                var filedNames = new[] { "MUNICIPALC", "MUNICIPALN", "NATURALA0", "ID" };
                const string sentance = "SELECT MUNICIPALC,MUNICIPALN,NATURALA0,ID WHERE ID>0";
                DataTable dt = FillData(filedNames, nodeLayerName, sentance);
                if (dt.IsDataTableFull())
                {
                    lstResults.Add(new City { Code = "0000", Name = "שטח ללא שיפוט" });
                    foreach (DataRow row in dt.Rows)
                    {
                        string code = row["MUNICIPALC"].ToString();
                        var munId = Convert.ToInt32(row["ID"]);
                        string name = (row["MUNICIPALN"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["MUNICIPALN"].ToString()));
                        int? authorityId = row["NATURALA0"] == DBNull.Value || string.IsNullOrEmpty(row["NATURALA0"].ToString()) ? null : (int?) Convert.ToInt32(row["NATURALA0"]);
                        if (!lstResults.Exists(p => p.Name == name && p.Code == code))
                        {
                            lstResults.Add(new City
                            {
                                MunId = munId, 
                                Code = code,
                                Name = name,
                                AuthorityId = authorityId
                            });
                        }
                    }
                }

                return (from p in lstResults
                        orderby p.Code
                        select p).ToList().Distinct().ToList();
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

         
        private static string GetFieldReallyValue(IEnumerable<string> fields, string key)
        {
            var res = (from p in fields
                       where p.ToLower() == key.ToLower()
                       select p).SingleOrDefault();
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            throw new ApplicationException(string.Format("The {0} field was not found in the Is_STR layer.", key));
        }


        #region IS_STR_FIELDS
        const string userid = "userid";
        const string fjunction = "fjunction";
        const string tjunction = "tjunction";
        const string length = "length";
        const string street = "street";
        const string dir = "dir";
        #endregion

        private static Dictionary<string, string> SetIsStrFields(List<string> fields)
        {
            //"userid", "fjunction", "tjunction", "length", "street", "dir"
            var dic = new Dictionary<string, string>();
            if (fields.IsListFull())
            {
                dic.Add(userid, GetFieldReallyValue(fields, userid));
                dic.Add(fjunction, GetFieldReallyValue(fields, fjunction));
                dic.Add(tjunction, GetFieldReallyValue(fields, tjunction));
                dic.Add(length, GetFieldReallyValue(fields, length));
                dic.Add(street, GetFieldReallyValue(fields, street));
                dic.Add(dir, GetFieldReallyValue(fields, dir));
            }
            return dic;
        }

        /// <summary>
        /// Get Links
        /// </summary>
        /// <param name="nodeLayerName"></param>
        /// <returns></returns>
        public List<Link> GetLinks(string nodeLayerName, string endPoints, string mapLayer)
        {
            int counter;
            counter = 0;
            var lstResults = new List<Link>();
            try
            {
                List<string> isStrFileds = GetLayerFields(mapLayer);
                Dictionary<string, string> dicISSTRFields = SetIsStrFields(isStrFileds);
                int maxFetchCount = 20000;
                object nodes_count = Gisdk.DoFunction("GetRecordCount", nodeLayerName, null);
                string[] filedNames = new string[] { 
                    dicISSTRFields[userid],dicISSTRFields[fjunction],dicISSTRFields[tjunction],
                    dicISSTRFields[length],dicISSTRFields[street],dicISSTRFields[dir] };
                int initPart = Convert.ToInt32(nodes_count) / maxFetchCount;
                string sentance = string.Format(
                    "SELECT {0},{1},{2} ,{3},{4},{5} WHERE ID>={0} AND ID<{1}",
                    dicISSTRFields[userid],
                    dicISSTRFields[fjunction], dicISSTRFields[tjunction],
                    dicISSTRFields[length], dicISSTRFields[street], dicISSTRFields[dir]
                    );

                for (int i = 0; i <= initPart; i++)
                {
                    string sqlSentance = string.Format(sentance, i * maxFetchCount, (i + 1) * maxFetchCount);
                    DataTable dt = FillData(filedNames, nodeLayerName, sqlSentance);
                    if (dt.IsDataTableFull())
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            counter++;
                            Link lnk = new Link
                            {
                                LinkID = Convert.ToInt32(row["USERID"]) * 10 + 1,
                                NA = Convert.ToInt32(row["FNODE"]),
                                NB = Convert.ToInt32(row["TNODE"]),
                                Length = Convert.ToInt32(Convert.ToDouble(row["Length"]) * ExportConfigurator.GetConfig().LengthConverter),
                                Name = row["STREET"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["STREET"].ToString())
                            };
                            lstResults.Add(lnk);
                            Link lnk1 = new Link
                            {
                                LinkID = Convert.ToInt32(row["USERID"]) * 10 + 2,
                                NA = Convert.ToInt32(row["TJUNCTION"]),
                                NB = Convert.ToInt32(row["FJUNCTION"]),
                                Length = Convert.ToInt32(Convert.ToDouble(row["Length"]) * ExportConfigurator.GetConfig().LengthConverter),
                                Name = row["STREET"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["STREET"].ToString())
                            };
                            lstResults.Add(lnk1);
                        }
                    }

                }



                return lstResults;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Get Stops
        /// </summary>
        /// <param name="routeSystemLayer"></param>
        /// <param name="cityNameLayer"></param>
        /// <param name="linkLayer"></param>
        /// <returns></returns>
        //212
        //----"StopID",I,1,7,0,7,0,,,"",,Blank,
        //----"Name",C,8,50,0,50,0,,,"",,Blank,
        //"LinkID",I,58,9,0,9,0,,,"",,Blank,
        //"DistNA",I,67,10,0,10,0,,,"",,Blank,
        //"A",I,77,1,0,1,0,,,"",,Blank,
        //"B",I,78,1,0,1,0,,,"",,Blank,
        //"C",I,79,1,0,1,0,,,"",,Blank,
        //-----"Lat",R,80,9,0,9,6,,,"",,Blank,
        //----"Long",R,89,9,0,9,6,,,"",,Blank,
        //"Street",C,98,50,0,50,0,,,"",,Blank,
        //"House",I,148,4,0,4,0,,,"",,Blank,
        //"Across",I,152,1,0,1,0,,,"",,Blank,
        //"D",I,153,1,0,1,0,,,"",,Blank,
        //"Neighborhood",C,154,50,0,50,0,,,"",,Blank,
        //"CityCode",C,204,4,0,4,0,,,"",,Blank,
        //"ID_SEKER",I,208,5,0,5,0,,,"",,Blank,

        public List<Stop> GetStops(string routeSystemLayer, string cityNameLayer, string linkLayer)
        {
            var lstStops = new List<Stop>();
            const int devider = 1000000;
            foreach (var physicalStop in GlobalData.RouteModel.PhysicalStopList)
            {
                var stop = new Stop
                {
                    StopID = physicalStop.ID,
                    Name = physicalStop.Name,
                    StationShortName                    = physicalStop.StationShortName,
                    LinkID = Convert.ToInt32(physicalStop.LinkUserId),
                    DistNA = 0,
                    Lat = Convert.ToDecimal(physicalStop.Latitude) / devider,
                    Long = Convert.ToDecimal(physicalStop.Longitude) / devider,
                    Street = physicalStop.Street,
                    House = physicalStop.House,
                    Across = physicalStop.Across,
                    CityCode = physicalStop.CityCode,
                    ID_SEKER = Convert.ToInt32(physicalStop.StationCatalog),
                    LatDifferrent = Convert.ToDecimal(physicalStop.LatitudeN) / devider,
                    LongDifferrent = Convert.ToDecimal(physicalStop.LongitudeN) / devider,
                    StationStatusId = physicalStop.IdStationStatus,
                    StationTypeId = physicalStop.IdStationType,
                    IsTrainMainStation = physicalStop.IsTrainMainStation,
                    //IsCityLinkedManual = physicalStop.IsCityLinkedManual
                    AreaOperatorId = physicalStop.AreaOperatorId
                };
                lstStops.Add(stop);
            }
            //var tst = lstStops.Single(s => s.StopID == 108329);
            return lstStops;
        }
        public List<Node> GetNodes(string nodeLayerName, string cityNameLayer)
        {
            var lstResults = new List<Node>();
            try
            {
                const int maxFetchCount = 20000;
                object nodesCount = Gisdk.DoFunction("GetRecordCount", nodeLayerName, null);

                var filedNames = new[] { "NodeID", "ID", "Name", "Latitude", "Longitude", "TypeDavid", "CityCode" };
                int initPart = Convert.ToInt32(nodesCount) / maxFetchCount;
                const string sentance = "SELECT NodeID,Name,Latitude,Longitude,TypeDavid WHERE  ID>={0} AND ID<{1}";
                //var fieldMunicipalcArray = new object[] { "MUNICIPALC" };
                for (int i = 0; i <= initPart; i++)
                {
                    var sqlSentance = string.Format(sentance, i * maxFetchCount, (i + 1) * maxFetchCount);
                    var dt = FillData(filedNames, nodeLayerName, sqlSentance);
                    if (dt.IsDataTableFull())
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            lstResults.Add(new Node
                            {
                                NodeID = row["NodeID"].ToString(),
                                Name = (row["Name"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["Name"].ToString())),
                                CityCode = row["CityCode"].ToString(),
                                Lat = Convert.ToDecimal(row["Latitude"]) / 1000000,
                                Long = Convert.ToDecimal(row["Longitude"]) / 1000000,
                                TypeDavid = row["TypeDavid"].ToString()
                            });
                            //"NodeID","Name","CityCode","Lat","Long","TypeDavid","CityCode"
                        }
                    }

                }
                return lstResults;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        // Set offSet
        public void SetOffSet(string lyr_set_name, string offset_type, double? offset)
        {
            try
            {
                Gisdk.DoFunction("SetOffset", lyr_set_name, offset_type, offset);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        #endregion

        #region ITransCadMunipulationDataDAL Members

        /// <summary>
        /// Get Map Title
        /// </summary>
        /// <param name="map_name"></param>
        /// <returns></returns>
        public string GetMapTitle(string map_name)
        {
            try
            {
                return Gisdk.DoFunction("GetMapTitle", map_name).ToString();
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return string.Empty;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        // <summary>
        // Get Line
        // </summary>
        // <param name="linkID"></param>
        //public void GetLine(int linkID)
        //{
        //    try
        //    {
        //        object data = Gisdk.DoFunction("GetLine", linkID);
        //        object infoArray = CaliperForm.Connection.ObjectToArray(data,"System.__ComObject");
        //        object data1 = Gisdk.DoFunction("ArrayToVector", data);

        //        object infoArray = Gisdk.DoFunction("ExtractArray", data, 1);
        //        object elem = ((object[])(data))[1];
        //        object[] dataArray = (object[])data;
        //        object infoArray = Gisdk.DoFunction("ExtractArray", data, 1);
        //    }
        //    catch (Exception exc)
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //        LoggerManager.WriteToLog(exc);

        //    }
        //    finally
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //    }
        //}
        /// <summary>
        /// Get Distance
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public int GetDistance(Coord c1, Coord c2)
        {
            var coord1 = Gisdk.DoFunction("Coord", c1.Longitude, c1.Latitude);
            var coord2 = Gisdk.DoFunction("Coord", c2.Longitude, c2.Latitude);
            object distance = Gisdk.DoFunction("GetDistance", coord1, coord2);
            return Convert.ToInt32(Convert.ToDouble(distance) * ExportConfigurator.GetConfig().LengthConverter);
        }
        /// <summary>
        /// Get Coordinate
        /// </summary>
        /// <param name="idPoint"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public Coord GetCoordinate(int idPoint, string layerName)
        {
            Coord coord = null;
            string[] filedNames = null;
            string sentance = null;
            filedNames = new string[] { "ID", "Longitude", "Latitude" };
            sentance = string.Format("SELECT ID,Longitude,Latitude WHERE ID={0}", idPoint);
            DataTable dt = FillData(filedNames, layerName, sentance);
            if (dt.IsDataTableFull() && dt.Rows.Count == 1)
            {
                coord = new Coord
                {
                    Latitude = dt.Rows[0]["Latitude"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["Latitude"]),
                    Longitude = dt.Rows[0]["Longitude"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["Longitude"]),
                };
            }
            return coord;
        }

        /// <summary>
        /// Get Station LinkID
        /// </summary>
        /// <param name="rs_layer"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<LinkStop> GetStationLinkID(string rs_layer, RouteLine routeLine)
        {
            List<LinkStop> lnkStopList = new List<LinkStop>();
            if (routeLine == null) return null;
            try
            {
                string routeName = GetRouteNameByRouteID(routeLine.ID, rs_layer);
                object data = Gisdk.DoFunction("GetRouteStops", rs_layer, routeName, null);
                object len = Gisdk.DoFunction("ArrayLength", data);
                for (int i = 1; i <= Convert.ToInt32(len); i++)
                {
                    object infoArray = Gisdk.DoFunction("ExtractArray", data, i);
                    object[] infoData = CaliperForm.Connection.ObjectToArray(infoArray, "string");
                    lnkStopList.Add(new LinkStop
                    {
                        LinkId = Convert.ToInt32(infoData[1]),
                        StopId = Convert.ToInt32(infoData[0])
                    });
                }
                return lnkStopList;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return lnkStopList;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();

            }
        }

        /// <summary>
        /// Calculate All Route Link List Duration
        /// </summary>
        /// <param name="macroName"></param>
        /// <param name="uiFile"></param>

        public void RunExportMacro(string macroName, string uiFile, List<object> parameters)
        {
            try
            {
                object[] parameterArray = new object[] { };
                if (parameters.IsListFull())
                    parameterArray = parameters.ToArray();
                Gisdk.DoMacro(macroName, uiFile, parameterArray);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// RunTranscadMacro
        /// </summary>
        /// <param name="macroName"></param>
        /// <param name="uiFile"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Object RunTranscadMacro(string macroName, string uiFile, List<object> parameters)
        {
            try
            {
                object[] parameterArray = new object[] { };
                if (parameters.IsListFull())
                    parameterArray = parameters.ToArray();
                return Gisdk.DoMacro(macroName, uiFile, parameterArray);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// IsEndPointHasEmptyCityOrTypeDavid
        /// </summary>
        /// <param name="endPoints_layer"></param>
        /// <returns></returns>
        public bool IsEndPointHasEmptyCityOrTypeDavid(string endPoints_layer)
        {
            try
            {
                //endPoints ="Endpoints"
                Gisdk.DoFunction("SetLayer", endPoints_layer);
                String qry = "Select * where CityCode =null || CityCode='' || TypeDavid=null || TypeDavid=''";
                object returnValues = Gisdk.DoFunction("SelectByQuery", "selectedData", "Several", qry, null);
                return Convert.ToInt64(returnValues) > 0;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return true;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();

            }
        }


        #endregion

        #region PriceArea Members
        public bool UpdatePriceListPolygon(PriceArea priceArea, string priceListPolygonName)
        {
            try  
            {
                SetReadOnlyFieldSet(priceListPolygonName, PriceAreaReadOnlyFields, false);
                Hashtable hs = new Hashtable();
                 
                hs.Add("ZoneDescription", StringHelper.ConvertToDefaultFrom1255(priceArea.Name));
                hs.Add("ZoneCode", priceArea.ID);
                
                if (hs.Count > 0)
                {
                    object data = CaliperForm.Connection.TableToArray(hs);
                    Gisdk.DoFunction("SetRecordValues", priceListPolygonName, priceArea.TranscadId.ToString(), data);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false;

            }
            finally
            {
                SetReadOnlyFieldSet(priceListPolygonName, PriceAreaReadOnlyFields, true);
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Get All Price List Polygon
        /// </summary>
        /// <param name="priceListPolygonName"></param>
        /// <returns></returns>
        public List<PriceArea> GetAllPriceListPolygon(string priceListPolygonName)
        {
            var lst = new List<PriceArea>();
            string[] filedNames = new string[] { "ID","ZoneCode", "ZoneDescription", "Area" };
            string sentance = "SELECT ID, ZoneCode,  ZoneDescription, Area WHERE ID>0";
            DataTable dt = FillData(filedNames, priceListPolygonName, sentance);
            if (dt.IsDataTableFull())
            {
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new PriceArea
                    {
                        ID = row["ZoneCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["ZoneCode"]),
                        Area = Convert.ToDouble(row["Area"]),
                        Name = row["ZoneDescription"] == DBNull.Value ? string.Empty : StringHelper.ConvertToHebrewEncoding(row["ZoneDescription"].ToString()),
                                                TranscadId = Convert.ToInt32(row["ID"])
                    });
                }

            }
            return lst;
        }
        
        /// <summary>
        /// GetTableStructure
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public Dictionary<String, String> GetTableStructure(string layerName)
        {
            try
            {
                List<string> lstFields = GetLayerFields(layerName);
                Dictionary<String, String> results = new Dictionary<string, string>();
                object strct = Gisdk.DoFunction("GetTableStructure", layerName);
                for (int i = 0; i < lstFields.Count; i++)
                {
                    object fieldsElement = Gisdk.DoFunction("ExtractArray", strct, i + 1);
                    object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(fieldsElement, "string");
                    results.Add(fieldsArrayData[0].ToString(), fieldsArrayData[1].ToString());
                }
                return results;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        #endregion
        /// <summary>
        /// Open Workspace
        /// </summary>
        /// <param name="wsFileName"></param>
        public void OpenWorkspace(string wsFileName)
        {
            try
            {
                Gisdk.DoFunction("OpenWorkspace", wsFileName,null);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// SavePhisicalStopStop
        /// </summary>
        /// <param name="physicalStop"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public bool SavePhisicalStopStop(PhysicalStop physicalStop, string layerName)
        {
            try
            {
                //SetReadOnlyFieldSet(layerName, PhysicalStopReadOnlyFields, false);
                var hs = new Hashtable
                             {
                                 {"CityLinkCode", (int) physicalStop.CityLinkCode},
                                 {"CityCode", physicalStop.CityCode}
                             };

                object data = Connection.TableToArray(hs);
                Gisdk.DoFunction("SetRecordValues", layerName, physicalStop.ID.ToString(), data);
                return true;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false;

            }
            finally
            {
                //SetReadOnlyFieldSet(layerName, PhysicalStopReadOnlyFields, true);
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        public void RenameLayer(string mapLayer, string newLayerName, object options)
        {
            try
            {
                Gisdk.DoFunction("RenameLayer", mapLayer, newLayerName, options);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }




        public void AddRouteSystemLayer(string mapName, string routeSystemName, string pathToRouteSystemFile, object options)
        {
            try
            {
                //var layers = Gisdk.DoFunction("AddRouteSystemLayer", mapName, routeSystemName, pathToRouteSystemFile, options);
                //object[] layersArray = CaliperForm.Connection.ObjectToArray(layers, "string");
                Gisdk.DoFunction("AddRouteSystemLayer", mapName, routeSystemName, pathToRouteSystemFile, options);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        public bool IsLayerExists(string layerName)
        {
            try
            {
                object info = Gisdk.DoFunction("GetLayerInfo", layerName);
                object[] infoArray = Connection.ObjectToArray(info, "string");
                if (infoArray!=null && infoArray.Length>=2)
                return (!string.IsNullOrEmpty(infoArray[2].ToString()));
                
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
            return false; 
        }
    }
}
