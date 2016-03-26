using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Accessories;
using BLEntities.Model;
using BLEntities.Entities;
using BLManager.Resources;
using Utilities;
using System.IO;
using System.Data;
using IDAL;
using DAL;
using ExportConfiguration;

namespace BLManager
{
    public static class BLSharedUtils
    {

        private static ITransCadMunipulationDataDAL _transCadFetchData = new TransCadMunipulationDataDAL();

        //public const string SEPARATOR = ";";
        /// <summary>
        /// WriteCSVErrorFile
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataTable"></param>
        public static void WriteCsvFile(string fileName, DataTable dataTable)
        {
            WriteCsvFile(fileName, dataTable, Encoding.UTF8);
        }

        public static void WriteCsvFile(string fileName, DataTable dataTable,Encoding encoding)
        {
            using (var writer = new CsvWriter())
            {
                writer.WriteCsv(dataTable, fileName, encoding);//
            }
        }
        /// <summary>
        /// IsTaxiOperator
        /// </summary>
        /// <returns></returns>
        public static bool IsTaxiOperator()
        {
            return DalShared.IsTaxiOperator();
        }

        public static bool IsTrainOperator()
        {
            return DalShared.IsTrainOperator();
        }

        //public static bool IsOperatorsIDsNotCalcNispah2Distances()
        //{
        //    string[] operatorsIDsNotCalcNispah2DistancesArray = ExportConfigurator.GetConfig().OperatorsIDsNotCalcNispah2Distances.Split(SEPARATOR.ToCharArray());
        //    if (operatorsIDsNotCalcNispah2DistancesArray.Any())
        //        return operatorsIDsNotCalcNispah2DistancesArray.FirstOrDefault(el => el == GlobalData.LoginUser.UserOperator.IdOperator.ToString()) != null;
        //    return false; 
        //}

        public static void UpdateBeforeLoad()
        {
            const string setUnits = "Kilometers";
            _transCadFetchData.GetMapUnits(ExportConfigurator.GetConfig().RouteSystemLayerName);
            _transCadFetchData.SetMapUnits(setUnits, ExportConfigurator.GetConfig().RouteSystemLayerName);
            _transCadFetchData.GetMapUnits(ExportConfigurator.GetConfig().RouteSystemLayerName);

        }

        public static bool IsTranscadActive()
        {
            return _transCadFetchData.IsTranscadActive();
        }


        public static byte[] GetShapeFileContent(String getShapeFileFolder, int id)
        {
            var pattern = string.Format("{0}\\{1}", getShapeFileFolder, id);
            var zipDestinationFile = string.Concat(pattern, Zipper.ZIPEXTENTION);
            Zipper.ZIPFoder(pattern, zipDestinationFile);

            using (var fs = new FileStream(zipDestinationFile, FileMode.Open, FileAccess.Read))
            {
                var b = new Byte[fs.Length];
                fs.Read(b, 0, b.Length);
                return b;
            }
        }

        public static bool IsLineHasStation(RouteLine rl)
        {
            bool isLineHasStation = false;
            if (GlobalData.RouteModel.RouteStopList.IsListFull() && rl != null)
            {
                isLineHasStation = GlobalData.RouteModel.RouteStopList.Exists(rs => rs.RouteId == rl.ID);
            }
            return isLineHasStation;

        }



        /// <summary>
        /// Update Drop First Station Data
        /// </summary>
        /// <param name="horadaList"></param>
        public static void UpdateDropFirstStationData(List<RouteStop> horadaList)
        {
            if (horadaList.IsListFull())
            {
                for (int i = 0; i < horadaList.Count - 1; i++)
                {

                    RouteStop rs = horadaList[i];
                    RouteStop horadaRouteStop = horadaList.FindLast(rsInner => rsInner.Ordinal == rs.Horada);
                    if (horadaRouteStop != null)
                    {
                        rs.StationCatalogHorada = horadaRouteStop.StationCatalog;
                        rs.StationNameHorada = GetStationNameByPhysicalStopidId(horadaRouteStop.PhysicalStopId);
                    }
                }
                horadaList[horadaList.Count - 1].StationCatalogHorada = String.Empty;
                horadaList[horadaList.Count - 1].Horada = 0;
            }
        }
        /// <summary>
        /// Update Station Catalog Horada
        /// </summary>
        /// <param name="routeID"></param>
        public static void UpdateStationCatalogHorada(int routeID)
        {
            List<RouteStop> horadaList = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == routeID).OrderBy(rs => rs.Milepost).ToList<RouteStop>();
            UpdateDropFirstStationData(horadaList);
        }

        /// <summary>
        /// Get Station Name By PhysicalStopidID
        /// </summary>
        /// <param name="physicalStopid"></param>
        /// <returns></returns>
        public static string GetStationNameByPhysicalStopidId(int physicalStopid)
        {
            string name = string.Empty;
            PhysicalStop ps = GlobalData.RouteModel.PhysicalStopList.FindLast(psInner => psInner.ID == physicalStopid);
            if (ps != null)
                name = ps.Name;
            return name;
        }

        /// <summary>
        /// Set Route Stop Station Type 
        /// </summary>
        /// <param name="routeStop"></param>
        public static void SetRouteStopStationType(RouteStop routeStop)
        {
            if (routeStop != null)
            {
                if (GlobalData.BaseTableEntityDictionary[GlobalConst.baseStationType] != null)
                {
                    BaseTableEntity be = GlobalData.BaseTableEntityDictionary[GlobalConst.baseStationType].Find(it => it.ID == routeStop.IdStationType);
                    if (be != null)
                    {
                        routeStop.StationTypeName = be.Name;
                    }
                }
            }
        }



        //        StationActivityType	StationActivityTypeDesc
        //0	לא מוגדר
        //1	אסוף בלבד
        //2	הורדה בלבד
        //3	אסוף והורדה
        //4	א+ה) מוגבלים         (
        //5	הורדה)מוגבלת         (

       

        public static List<PhysicalStop> FilterTrainPhysicalStops(List<PhysicalStop> psList)
        {
            if (psList.IsListFull())
            {
                var psTrains = psList.Where(ps=>ps.IdStationType ==7);
                if (psTrains.Any())
                {
                    var queryStationCatalog = (from ps in psTrains
                                               group ps by ps.StationCatalog
                                               into newGroup
                                               where newGroup.Count() > 1
                                               orderby newGroup.Key
                                               select newGroup).ToList();
                    foreach (var nameGroup in queryStationCatalog)
                    {
                        var mainStation = nameGroup.FirstOrDefault(e => e.IsTrainMainStation);
                        if (mainStation==null)
                        {
                            throw new ApplicationException(string.Format("Inside the {0} StationCatalog group the IsTrainMainStation true value was not found. Please check this group", nameGroup.Key));
                        }
                        foreach (var ps in nameGroup.Where(ps => !ps.IsTrainMainStation))
                        {
                            psList.Remove(ps);
                        }
                    }
                }
            }
            return psList;
        }
        public static List<RouteLine> LinesNotBelongToOperator(List<RouteLine> lines, Operator oper)
        {
            if (lines.IsListFull())
            {
                return lines.Where(l => l.IdOperator != oper.IdOperator && !l.IsNewEntity).ToList();
            }
            return (List<RouteLine>)Enumerable.Empty<RouteLine>();
        }

        public static string GetLineNamesSeparatedBy(List<RouteLine> list, string separator)
        {
            var sb = new StringBuilder();
            if (list.IsListFull())
            {
                list.OrderBy(l=>l.Catalog).ToList().ForEach(l => {
                    if (l.Catalog.HasValue)
                        sb.Append(l.Catalog.ToString()).Append(separator);
                });
                sb.Remove(sb.Length-1, 1);
            }

            return sb.ToString();
        }
    
        public static List<Stop> FilterTrainStops(List<Stop> stopList)
        {
            if (stopList.IsListFull())
            {
                var stopListTrains = stopList.Where(s => s.StationTypeId == 7);
                if (stopListTrains.Any())
                {
                    var queryStationCatalogByName = (from s in stopListTrains
                                                   group s by s.Name
                                                   into newGroup
                                                   where newGroup.Count() > 1
                                                   orderby newGroup.Key
                                                   select new
                      {
                          StationName = newGroup.Key,
                          Count = newGroup.Select(x => x.ID_SEKER).Distinct().Count()
                      });
                    

                    foreach (var stationNameGroup in queryStationCatalogByName)
                    {
                        if (stationNameGroup.Count>1)
                        {
                            throw new ApplicationException(string.Format(BLManagerResource.DifferentSationCatalogFor7, stationNameGroup));
                        }
                    }

                    var queryStationCatalog = (from s in stopListTrains
                                               group s by s.ID_SEKER
                                                   into newGroup
                                                   where newGroup.Count() > 1
                                                   orderby newGroup.Key
                                                   select newGroup).ToList();
                    foreach (var nameGroup in queryStationCatalog)
                    {
                        var mainStation = nameGroup.FirstOrDefault(e => e.IsTrainMainStation);
                        if (mainStation == null)
                        {
                            throw new ApplicationException(string.Format(Resources.BLManagerResource.IsTrainMainStationFor7WasNotFound, nameGroup.Key));
                        }
                        foreach (var ps in nameGroup.Where(ps => !ps.IsTrainMainStation))
                        {
                            stopList.Remove(ps);
                        }
                    }
                }
            }
            return stopList;
        }


        public static bool IsLoweringStation(RouteStop rs)
        {
            var isLoweringStation = rs != null && (rs.IdStationType == 2 || rs.IdStationType == 4);
            if (isLoweringStation)
            {
                rs.Horada = 0;
                rs.StationCatalogHorada = string.Empty;
                rs.StationNameHorada = string.Empty;
            }
            return isLoweringStation;
        }
        /// <summary>
        /// IsCentralBusStation
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static bool IsCentralBusStation(RouteStop rs)
        {
            //PhysicalStop.IdStationType = 2
            return rs != null && rs.PhysicalStop != null && rs.PhysicalStop.IdStationType == 8;

        }


        /// <summary>
        /// Get List Route Stops By ID RouteLine
        /// </summary>
        /// <param name="rl"></param>
        /// <returns></returns>
        public static List<RouteStop> GetListRouteStopsByIdRouteLine(RouteLine rl, bool isEgedOperator)
        {
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                List<RouteStop> lst = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == rl.ID);
                if (isEgedOperator)
                {
                    lst = (from p in lst
                           orderby p.Ordinal
                           select p).ToList<RouteStop>();
                }
                else
                {
                    int rowNumbwer = 1;
                    lst = (from p in lst
                           orderby p.Milepost
                           select p).ToList<RouteStop>();
                    lst.ForEach(rs => rs.Ordinal = rowNumbwer++);
                }
                return lst;
            }
            return null;
        }

        /// <summary>
        /// BuildCatalogInfoList
        /// </summary>
        public static void BuildCatalogInfoList()
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                GlobalData.CatalogInfolList = new List<CatalogInfo>();
                var result = (from p in GlobalData.RouteModel.RouteLineList
                              orderby p.Catalog
                              where p.Catalog != 0 //&& p.Catalog.HasValue
                              select new
                              {
                                  Catalog = p.Catalog.HasValue ? p.Catalog.ToString() : string.Empty,
                                  IdCluster = p.IdCluster
                              }).ToList();
                result = result.Distinct().ToList();
                var lstResulst = new List<CatalogInfo>();

                IInternalBaseDal dal = new TransportLicensingDal();
                List<City> cities  = dal.GetCityListFromExternalSource();
                cities.Insert(0, new
                                City
                                {
                                    Name = Resources.BLManagerResource.MunicipalityArea,
                                    Code = Resources.BLManagerResource.Code0000
                                });

                foreach (var item in result)
                {
                    List<RouteLine> routeLineList =
                        GlobalData.RouteModel.RouteLineList.FindAll(rl => (rl.Catalog ==
                        (string.IsNullOrEmpty(item.Catalog) ? null : (int?)int.Parse(item.Catalog))
                        && rl.IdCluster == item.IdCluster));

                    if (routeLineList.IsListFull())
                    {
                        RouteLine firstRouteLine = routeLineList[0];
                        RouteLine baseLine = GlobalData.RouteModel.RouteLineList.Where(p => p.Catalog == firstRouteLine.Catalog &&
                                                                                          p.IdCluster == firstRouteLine.IdCluster).OrderBy(p => p.Dir).OrderBy(p => p.VarNum).FirstOrDefault();
                        // 
                        String fromPathCityName = string.Empty;  
                        if (baseLine!=null)
                        {
                          RouteStop routeStopFirst = GlobalData.RouteModel.RouteStopList.Where(p=>p.RouteId == baseLine.ID).OrderBy(p=>p.MilepostFromOriginStation).FirstOrDefault();
                          if (routeStopFirst != null)
                          {
                              PhysicalStop physicalStop = GlobalData.RouteModel.PhysicalStopList.Where(p => p.ID == routeStopFirst.PhysicalStopId).SingleOrDefault();
                              if (physicalStop != null)
                              {
                                 City city = cities.SingleOrDefault(c => c.Code == physicalStop.CityCode);
                                 if (city != null)
                                 {
                                     fromPathCityName = city.Name; 
                                 }
                              }
                          }
                        }
                        if (firstRouteLine != null)
                        {
                            CatalogInfo ci = new CatalogInfo
                            {
                                Catalog = firstRouteLine.Catalog,
                                FromPathCityName = fromPathCityName,
                                IsNew = false,
                                IdExclusivityLineType = firstRouteLine.IdExclusivityLine,
                                IdServiceType = firstRouteLine.IdServiceType,
                                IdZoneHead = firstRouteLine.IdZoneHead,
                                IdCluster = firstRouteLine.IdCluster,
                                IdZoneSubHead = firstRouteLine.IdZoneSubHead,
                                AccountingGroupID = firstRouteLine.AccountingGroupID,
                                RouteNumber = firstRouteLine.RouteNumber,
                                RouteLineBelongCounter = routeLineList.Count
                            };
                            GlobalData.CatalogInfolList.Add(ci);
                        };
                    }
                }
            }
        }
    }
}
