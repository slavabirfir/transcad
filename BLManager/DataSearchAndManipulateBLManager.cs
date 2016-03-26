using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Entities;
using BLEntities.Model;
using Utilities;
using System.ComponentModel;
using IDAL;
using DAL;
using ExportConfiguration;


namespace BLManager
{
    public class DataSearchAndManipulateBLManager : IDataSearchAndManipulateBLManager
    {
        private const string Delimiter = ",";

        #region Constructor
        private bool _isDescended;
        private readonly ITransCadBLManager _transCadBlManager = new TransCadBlManager(); 
        public DataSearchAndManipulateBLManager()
        {
            CurrentRouteLineSortedProperty = "catalog";
            LastRouteLineSortedProperty = CurrentRouteLineSortedProperty;
        }
        #endregion
        #region IDataSearchAndManipulateBLManager Members
        public string LastRouteLineSortedProperty { get; set; }
        public string CurrentRouteLineSortedProperty { get; set; }
        /// <summary>
        /// Is Variant Exists In Variant Num Table
        /// </summary>
        /// <param name="variant"></param>
        /// <returns></returns>
        public bool IsVariantExistsInVariantNumTable(string variant)
        {
            if (GlobalData.BaseTableEntityDictionary.ContainsKey(GlobalConst.baseVarConverter))
            {
                return GlobalData.BaseTableEntityDictionary[GlobalConst.baseVarConverter].Exists(el => el.Name == variant);  
            }
            return false;
        }

        /// <summary>
        /// Is Route Number Filtered
        /// </summary>
        /// <param name="rl"></param>
        /// <param name="routeNumber"></param>
        /// <returns></returns>
        private static bool IsRouteNumberFiltered(RouteLine rl,string routeNumber)
        {
            if (string.IsNullOrEmpty(routeNumber))
                return true;
            if (!rl.RouteNumber.HasValue)
            {
                return false ;
            }
            if (routeNumber.IndexOf(Delimiter) == -1)
            {
                {
                    return rl.RouteNumber.ToString().Equals(routeNumber);
                }
            }
            else
            {
                List<string> array = routeNumber.Split(Delimiter.ToCharArray()).ToList<string>();
                {
                    if (array.Exists(it =>it.Equals(string.Empty )))  
                        array.Remove(string.Empty);
                    return array.Exists(item => rl.RouteNumber.ToString().Equals(item));
                    
                }
            }
            return true; 
        }
        ///// <summary>
        ///// UpdateCatalogOfRouteLines
        ///// </summary>
        ///// <param name="routeLine"></param>
        ///// <returns></returns>
        public void UpdateCatalogOfRouteLines(CatalogInfo catalogInfo)
        {
            if (catalogInfo!=null)
            {
                if (GlobalData.CatalogInfolList.IsListFull() && GlobalData.RouteModel.RouteLineList.IsListFull())
                {
                    var list = GlobalData.RouteModel.RouteLineList.FindAll(rl => rl.Catalog == catalogInfo.Catalog);
                    var foundCatalogInfo = GlobalData.CatalogInfolList.Find(ci => ci.Catalog == catalogInfo.Catalog && ci.IdCluster == catalogInfo.IdCluster);
                    if (foundCatalogInfo == null)
                    {
                        GlobalData.CatalogInfolList.Add(catalogInfo);
                    }
                    else
                    {
                        foundCatalogInfo.Catalog = catalogInfo.Catalog;
                        foundCatalogInfo.IdCluster = catalogInfo.IdCluster;
                        foundCatalogInfo.RouteNumber = catalogInfo.RouteNumber;
                        foundCatalogInfo.IdServiceType = catalogInfo.IdServiceType;
                        foundCatalogInfo.IdZoneHead = catalogInfo.IdZoneHead;
                        foundCatalogInfo.IdZoneSubHead = catalogInfo.IdZoneSubHead;
                        foundCatalogInfo.IdExclusivityLineType = catalogInfo.IdExclusivityLineType;
                        foundCatalogInfo.AccountingGroupID = catalogInfo.AccountingGroupID;
                    }
                    foreach (var routeLine in list)
                    {
                        routeLine.Catalog = catalogInfo.Catalog;
                        routeLine.IdCluster = catalogInfo.IdCluster;
                        routeLine.RouteNumber = catalogInfo.RouteNumber ;
                        routeLine.IdServiceType = catalogInfo.IdServiceType;
                        routeLine.IdZoneHead = catalogInfo.IdZoneHead ;
                        routeLine.IdZoneSubHead = catalogInfo.IdZoneSubHead;
                        routeLine.IdExclusivityLine = catalogInfo.IdExclusivityLineType;
                        routeLine.IsNewEntity = false;
                        routeLine.AccountingGroupID = catalogInfo.AccountingGroupID;
                        _transCadBlManager.SaveRouteLine(routeLine);
                    }
                    
                }
            }
           

        }

       
        private static bool IsCatalogFiltered(RouteLine rl, string catalog)
        {
            if (string.IsNullOrEmpty(catalog))
                return true;
            if (!rl.Catalog.HasValue)
            {
                return false;
            }
            if (catalog.IndexOf(Delimiter) == -1)
            {
                {
                    return rl.RouteNumber.ToString().Equals(catalog);
                }
            }
            List<string> array = catalog.Split(Delimiter.ToCharArray()).ToList<string>();
            if (rl.RouteNumber.HasValue)
            {
                if (array.Exists(it => it.Equals(string.Empty)))
                    array.Remove(string.Empty);
                return array.Exists(item => rl.Catalog.ToString().Equals(item));

            }
            return true;
        }

       
        private static List<RouteStop> GetStationCatalogFiltered(int? idRouteLine,string catalog)
        {
            List<RouteStop> resultList = null;
            if (string.IsNullOrEmpty(catalog))
                return resultList;
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                resultList = new List<RouteStop>();
                List<string> array = catalog.Split(Delimiter.ToCharArray()).ToList<string>();
                if (array.Exists(it => it.Equals(string.Empty)))
                    array.Remove(string.Empty);
                array.ForEach(c =>
                    {
                        List<RouteStop> rsl = (GlobalData.RouteModel.RouteStopList.FindAll(p =>
                                p.StationCatalog.IndexOf(c) >= 0 &&
                                (idRouteLine == null ? true : p.RouteId == idRouteLine)));

                        if (rsl.IsListFull())
                        {
                            rsl.ForEach(rslS => 
                             {
                                 if (!resultList.Exists(resultListS => resultListS.ID == rslS.ID))
                                 {
                                     resultList.Add(rslS);
                                 }
                             }
                            );
                        }
                    });
                
            }
            return resultList;

        }



        //private bool IsStationCatalogFiltered(RouteStop rs, string catalog)
        //{
        //    if (string.IsNullOrEmpty(catalog))
        //        return true;

        //    //if (catalog.IndexOf(delimiter) == -1)
        //    //{
        //    //   //return rs.StationCatalog.Equals(catalog);
        //    //    return rs.StationCatalog.IndexOf(delimiter) >= 0;
        //    //}
        //    //else
        //    //{
        //        List<string> array = catalog.Split(delimiter.ToCharArray()).ToList<string>();
        //        if (array.Exists(it => it.Equals(string.Empty)))
        //            array.Remove(string.Empty);
        //        return array.Exists(item => rs.StationCatalog.ToString().IndexOf(catalog) >= 0);
        //    //}
            
        //}

        /// <summary>
        /// Is Cluster Filtered
        /// </summary>
        /// <param name="rl"></param>
        /// <param name="cluster" />
        /// <returns></returns>
        private static bool IsClusterFiltered(RouteLine rl, string cluster)
        {
            if (string.IsNullOrEmpty(cluster))
                return true;

            if (cluster.IndexOf(Delimiter) == -1)
            {
                if (rl!=null && rl.ClusterName!=null)
                    return rl.ClusterName.Equals(cluster);
                return false;
            }
            List<string> array = cluster.Split(Delimiter.ToCharArray()).ToList<string>();
            if (array.Exists(it => it.Equals(string.Empty)))
                array.Remove(string.Empty);
            return array.Exists(item => !string.IsNullOrEmpty(rl.ClusterName) &&  rl.ClusterName.ToString().Equals(item));
        }




       
        private static bool IsStationNameFiltered(RouteStop rs, string name)
        {
            if (string.IsNullOrEmpty(name))
                return true;

            if (name.IndexOf(Delimiter) == -1)
            {
                return rs.PhysicalStop.Name.IndexOf(name) >= 0;
            }
            List<string> array = name.Split(Delimiter.ToCharArray()).ToList<string>();
            if (array.Exists(it => it.Equals(string.Empty)))
                array.Remove(string.Empty);
            return array.Exists(item => rs.PhysicalStop.Name.IndexOf(item) >= 0);
        }



       
        public List<RouteLine> GetSearchedSortedRouteLines(string routeNumber, 
            string direction, string variant, 
            bool isNotValidOnly,
            string catalog, string cluster, bool isNewEntity)
        {
            if (GlobalData.RouteModel != null && GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                var results = (from p in GlobalData.RouteModel.RouteLineList
                              where
                                 (isNewEntity ? p.IsNewEntity : true)
                                 &&
                                 (isNotValidOnly ? p.ValidationErrors.IsListFull() && !p.IsNewEntity : true)
                                 &&
                                 ((!string.IsNullOrEmpty(catalog) ?
                                 IsCatalogFiltered(p, catalog) : true) 
                                 &&
                                 ((!string.IsNullOrEmpty(cluster) ?
                                 IsClusterFiltered(p, cluster) : true) 
                                 &&
                                 ((!string.IsNullOrEmpty(routeNumber) ?
                                 IsRouteNumberFiltered(p,routeNumber) : true) 
                                 &&
                                 (!string.IsNullOrEmpty(direction) ?
                                 p.Dir.ToString().ToLower().IndexOf(direction.ToLower()) >= 0 : true)
                                 &&
                                 (!string.IsNullOrEmpty(variant) ?
                                 p.Var.ToLower().IndexOf(variant.ToLower()) >= 0 : true))))
                                 select p).ToList<RouteLine>();
               
                    if (LastRouteLineSortedProperty != CurrentRouteLineSortedProperty)
                    {
                        _isDescended = false ; 
                    }
                    switch (CurrentRouteLineSortedProperty.ToLower())
                    {
                        case "catalog":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.Catalog descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.Catalog 
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended; 
                            }
                            else
                                results = (from p in results
                                           orderby p.Catalog
                                           select p).ToList<RouteLine>(); 
                            break;
                        case "routenumber":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.RouteNumber descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.RouteNumber
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended; 
                            }
                            else
                                results = (from p in results
                                           orderby p.RouteNumber
                                           select p).ToList<RouteLine>();
                            break;
                        case "roaddescription":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.RoadDescription descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.RoadDescription
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.RoadDescription
                                           select p).ToList<RouteLine>();
                            break;
                        case "clustername":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.ClusterName descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.ClusterName
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.ClusterName
                                           select p).ToList<RouteLine>();
                            break;
                        case "signpost":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.Signpost descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.Signpost
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.Signpost
                                           select p).ToList<RouteLine>();
                            break;

                        case "var":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.Var descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.Var
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.Var
                                           select p).ToList<RouteLine>();
                            break;
                        case "dir":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.Dir descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.Dir
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.Dir
                                           select p).ToList<RouteLine>();
                            break;
                        case "servicetypename":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.ServiceTypeName descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.ServiceTypeName
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.ServiceTypeName
                                           select p).ToList<RouteLine>();
                            break;
                        case "isbase":
                            if (CurrentRouteLineSortedProperty.Equals(LastRouteLineSortedProperty))
                            {
                                if (!_isDescended)
                                {
                                    results = (from p in results
                                               orderby p.IsBase descending
                                               select p).ToList<RouteLine>();
                                }
                                else
                                {
                                    results = (from p in results
                                               orderby p.IsBase
                                               select p).ToList<RouteLine>();
                                }
                                _isDescended = !_isDescended;
                            }
                            else
                                results = (from p in results
                                           orderby p.IsBase
                                           select p).ToList<RouteLine>();
                            break;

                    }
                    LastRouteLineSortedProperty = CurrentRouteLineSortedProperty;

                if (!isNotValidOnly)
                    results = results.Where(p => (p.ValidationErrors == null || !p.ValidationErrors.Any())).ToList();

                return results;

            }
            return null;
        }

        /// <summary>
        /// Set Cataog Data By Value Changing
        /// </summary>
        /// <param name="routeLine"></param>
        public void SetCatalogDataByValueChanging(int? catalog, RouteLine routeLine)
        {
            if (GlobalData.CatalogInfolList.IsListFull())
            {
                CatalogInfo catalogInfo = GlobalData.CatalogInfolList.Find(ci => ci.Catalog == catalog);
                if (catalogInfo != null)
                {
                    routeLine.Catalog = catalog;
                    routeLine.IdCluster = catalogInfo.IdCluster;
                    routeLine.IdServiceType = catalogInfo.IdServiceType;
                    routeLine.IdZoneHead = catalogInfo.IdZoneHead;
                    routeLine.IdZoneSubHead = catalogInfo.IdZoneSubHead;
                    routeLine.RouteNumber = catalogInfo.RouteNumber;
                    routeLine.IdExclusivityLine = catalogInfo.IdExclusivityLineType;
                }
            }
        }

        /// <summary>
        /// Get Searched Route Lines
        /// </summary>
        /// <param name="signpost"></param>
        /// <param name="direction"></param>
        /// <param name="variant"></param>
        /// <returns></returns>
        public List<RouteStop> GetSearchedRouteStops(string routeNumber, string direction, 
            string variant, bool isNotValidOnly, string stationName, string catalog,string catalogStation)
        {

            List<RouteStop> filteredRouteStops = GetStationCatalogFiltered(this.IDRouteLineForStationsQuery, catalogStation);
            if (GlobalData.RouteModel != null && GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                List<RouteStop> results = (from p in GlobalData.RouteModel.RouteStopList
                               where
                                  p.RouteLine!=null && p.PhysicalStop!=null &&
                                  (isNotValidOnly ? p.ValidationErrors.IsListFull() : true)
                                  &&
                                  ((filteredRouteStops==null ? true : filteredRouteStops.Exists(f => f.ID ==p.ID)))
                                  &&
                                  ((!string.IsNullOrEmpty(catalog) ?
                                  p.RouteLine.Catalog.ToString().ToLower().IndexOf(catalog.ToLower()) >= 0 : true)
                                  &&
                                  ((!string.IsNullOrEmpty(routeNumber) ?
                                  p.RouteLine.RouteNumber.ToString().ToLower().IndexOf(routeNumber.ToLower()) >= 0 : true)
                                  &&
                                  (!string.IsNullOrEmpty(direction) ?
                                  p.RouteLine.Dir.ToString().ToLower().IndexOf(direction.ToLower()) >= 0 : true)
                                   &&
                                  (!string.IsNullOrEmpty(stationName) ?
                                  IsStationNameFiltered(p,stationName) : true)
                                   &&
                                  (!string.IsNullOrEmpty(variant) ?
                                  p.RouteLine.Var.ToLower().IndexOf(variant.ToLower()) >= 0 : true)))
                                  
                                  orderby p.RouteLine.ID , p.Ordinal 
                               select p).ToList<RouteStop>();
                 
                return results;

            }
            return null;
        }



        /// <summary>
        /// Get Route Types
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetRouteType()
        {

            return  GetBaseTableDataList(GlobalConst.baseRouteType);

        }
       
        /// <summary>
        /// Get Base Table DataList
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static List<BaseTableEntity> GetBaseTableDataList(string tableName)
        {
            if (GlobalData.BaseTableEntityDictionary.ContainsKey(tableName))
            {
                List<BaseTableEntity> lst = GlobalData.BaseTableEntityDictionary[tableName];
                if (!lst.Exists(el => el.ID == 0) && tableName.ToUpper().Equals("TCGETLINETYPE"))
                {
                    lst.Add(new BaseTableEntity { ID = 0, Name = string.Empty });
                }
                List<BaseTableEntity> sortedList = (from p in lst
                                                    orderby p.ID
                                                    select p).ToList<BaseTableEntity>();
                return sortedList;
            }
            return null;
        }

        /// <summary>
        /// Get Station Type
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetStationType()
        {
            return GetBaseTableDataList(GlobalConst.baseStationType);
        }

        /// <summary>
        /// RouteLineCount
        /// </summary>
        public int RouteLineCount
        {
            get
            {
                if (GlobalData.RouteModel.RouteLineList.IsListFull())
                {
                    return GlobalData.RouteModel.RouteLineList.Count; 
                }
                return 0;
            }
        }
        /// <summary>
        /// Route Stops Count
        /// </summary>
        public int RouteStopsCount
        {
            get
            {
                if (GlobalData.RouteModel.RouteStopList.IsListFull())
                {
                    return GlobalData.RouteModel.RouteStopList.Count;
                }
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PhisicalStopsCount
        {
            get
            {
                if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
                {
                    return GlobalData.RouteModel.PhysicalStopList.Count;
                }
                return 0;
            }
        }

         

        /// <summary>
        /// Get Clusters
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetClusters()
        {
           
            return GetBaseTableDataList(GlobalConst.tblOperatorCluster);
        }
        /// <summary>
        /// Get Service Type
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetServiceType()
        {
            return GetBaseTableDataList(GlobalConst.baseServiceType);
        }

        /// <summary>
        /// Get Direction List
        /// </summary>
        /// <returns></returns>
        public List<string> GetDirectionList()
        {
            if (GlobalData.Directions.IsListFull())
            {
                if (!GlobalData.Directions.Exists(s => s == string.Empty))
                {
                    GlobalData.Directions.Insert(0,string.Empty);  
                }
            }
            return GlobalData.Directions;
        }

        ///// <summary>
        ///// Get Zones Head 
        ///// </summary>
        ///// <returns></returns>
        //public List<BaseTableEntity> GetZonesHead()
        //{
        //    return GetBaseTableDataList(GlobalConst.baseZone);
        //}
        ///// <summary>
        ///// Get Zones Sub Head
        ///// </summary>
        ///// <returns></returns>
        //public List<BaseTableEntity> GetZonesSubHead()
        //{
        //    return this.GetZonesHead();
        //}


        /// <summary>
        /// Get Seasonal
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetSeasonal()
        {
          return GetBaseTableDataList(GlobalConst.baseSeasonal);
        }
        /// <summary>
        /// Is Route Line Valid
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsRouteLineValid(RouteLine routeLine)
        {
            routeLine.Validate();
            return !routeLine.ValidationErrors.IsListFull();
        }
        /// <summary>
        /// Is Route Stop Valid
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
   
        public bool IsRouteStopValid(RouteStop routeStop)
        {
            routeStop.Validate();
            return !routeStop.ValidationErrors.IsListFull();
        }


        /// <summary>
        /// Get Station Order
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public StationOrderConst GetStationOrder(RouteStop routeStop)
        {
            if (routeStop == null || !GlobalData.RouteModel.RouteLineList.IsListFull() )
            {
                return StationOrderConst.Regular; 
            }
            if (routeStop.Ordinal == 1)
            {
                return StationOrderConst.First; 
            }
            int countStation = GlobalData.RouteModel.RouteStopList.Count(rs => rs.RouteId == routeStop.RouteId); 
            if (routeStop.Ordinal == countStation)
            {
                return StationOrderConst.Last;
            }
            return StationOrderConst.Regular; 

        }
        /// <summary>
        /// Get Vehicle Types
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetVehicleTypes()
        {
            return GetBaseTableDataList(GlobalConst.baseVehicleType);
        }
        /// <summary>
        /// Get Vehicle Sizes
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetVehicleSizes()
        {
            return GetBaseTableDataList(GlobalConst.baseVehicleSize);
        }
       


        #endregion

        #region Sorting Members


        public List<RouteLine> GetRouteLineSortedList(string name)
        {
           List<RouteLine> result = null;
           switch (name.ToLower())
	            {
                       case "catalog":
                        result = (from p in GlobalData.RouteModel.RouteLineList
                                 orderby p.Catalog
                                  select p).ToList<RouteLine>(); 
                           break; 
		               default:
                            break;
	            }
          
           return result;
        }

        /// <summary>
        /// Get Catalog List
        /// </summary>
        /// <returns></returns>
        public List<string> GetCatalogList()
        {
            if (GlobalData.CatalogInfolList.IsListFull())
            {
                var result = (from p in GlobalData.CatalogInfolList
                              orderby p.Catalog
                              select p.Catalog.HasValue ? p.Catalog.ToString() : string.Empty).ToList<string>();
                result = result.Distinct().ToList<string>();
                return result; 
            }
            else
              return null;
        }

        public List<CatalogInfo> GetCatalogListEntities()
        {
            if (GlobalData.CatalogInfolList.IsListFull())
            {
                return GlobalData.CatalogInfolList.Distinct().OrderBy(p=>p.Catalog).ToList();
            }
            else
                return null;
        }


        /// <summary>
        /// Get Route Number List
        /// </summary>
        /// <returns></returns>
        public List<string> GetRouteNumberList()
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                var result = (from p in GlobalData.RouteModel.RouteLineList
                              orderby p.RouteNumber
                              select p.RouteNumber.HasValue ? p.RouteNumber.ToString() : string.Empty).ToList<string>();
                result = result.Distinct().ToList<string>();
                return result; 
            }
            else
              return null;
        }
        /// <summary>
        /// Get Station CatalogList
        /// </summary>
        /// <returns></returns>
        public List<string> GetStationCatalogList()
        {
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                var result = (from p in GlobalData.RouteModel.RouteStopList
                              orderby p.StationCatalog
                              select p.StationCatalog).ToList<string>();
                result = result.Distinct().ToList<string>();
                return result;
            }
            else
                return null;
        }

        /// <summary>
        /// Get Station Name List
        /// </summary>
        /// <returns></returns>
        public List<string> GetStationNameList()
        {
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                var result = (from p in GlobalData.RouteModel.RouteStopList
                              orderby p.PhysicalStop.Name
                              select p.PhysicalStop.Name).ToList<string>();
                result = result.Distinct().ToList<string>();
                return result;
            }
            else
                return null;
        }

        #endregion

        /// <summary>
        /// Get Cluster List
        /// </summary>
        /// <returns></returns>
        public List<string> GetClusterList()
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                var result = (from p in GlobalData.RouteModel.RouteLineList
                              where !string.IsNullOrEmpty(p.ClusterName) 
                              orderby p.RouteNumber
                              select p.ClusterName).ToList<string>();
                result = result.Distinct().ToList<string>();
                return result;
            }
            return null;
        }

        /// <summary>
        /// IDRouteLineForStationsQuery
        /// </summary>
        public int? IDRouteLineForStationsQuery { get; set; }

        /// <summary>
        /// Update Batch Station Type
        /// </summary>
        /// <param name="routeStops"></param>
        /// <param name="idStationType"></param>
        public void UpdateBatchStationType(List<RouteStop> routeStops, int idStationType)
        {
            if (routeStops.IsListFull())
            {
                ITransCadBLManager manager = new TransCadBlManager();
                if (manager.IsEgedOperator)
                    manager = new EgedBlManager();
                routeStops.ForEach(rs => 
                {
                    rs.IsSelected = false;
                    rs.IdStationType = idStationType;
                    manager.SaveRouteStop(rs);
                    
                    //RouteStop rsFound = GlobalData.RouteModel.RouteStopList.Find(rsInner => rsInner.ID == rs.ID);
                    //if (rsFound != null)
                    //{
                    //    rsFound.IdStationType = rs.IdStationType;
                    //}
                }
                );
            }
        }


        #region IDataSearchAndManipulateBLManager Members
        private const string Comma = ",";

        /// <summary>
        /// Export RouteLines To Excel
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="routes"></param>
        //public void ExportRouteLinesToExcel(string folderName,List<RouteLine> routes)
        //{
        //    if (routes.IsListFull())
        //    {
        //        string fileName = string.Format("{0}{1}{2}{3}", folderName,"\\RouteExcel_", DateTime.Now.ToString("ddMMyyyy_hhmmss"), ".csv");
        //        var sb = new StringBuilder();
        //        sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}",
        //                    "שם מפעיל", COMMA,
        //                    "שם אשכול", COMMA,
        //                    "מס' קו", COMMA,
        //                    "מקט", COMMA,
        //                    "כיוון", COMMA,
        //                    "חלופה", COMMA,
        //                    "תיאור הדרך", COMMA,
        //                    "סוג שרות", COMMA,
        //                    "שילוט", COMMA,
        //                    "מחוז ראשי", COMMA,
        //                    "מחוז משנה", COMMA,
        //                    "זמן הגדרה"
        //                    ));
                
        //        routes.ForEach(rl => sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}",
        //                                                         GlobalData.LoginUser.UserOperator.OperatorName, COMMA,
        //                                                         rl.ClusterName, COMMA,
        //                                                         rl.RouteNumber, COMMA,
        //                                                         rl.Catalog, COMMA,
        //                                                         rl.Dir, COMMA,
        //                                                         rl.Var, COMMA,
        //                                                         rl.RoadDescription.Replace(COMMA,""), COMMA,
        //                                                         rl.ServiceTypeName,COMMA,
        //                                                         rl.Signpost,COMMA,
        //                                                         rl.ZoneHeadName,COMMA,
        //                                                         rl.ZoneSubHeadName,COMMA,
        //                                                         rl.Hagdara
        //                                               )));
        //        IOHelper.WriteToFile(fileName, sb.ToString());
        //        ProcessLauncher.RunProcess("explorer", folderName);
        //    }
            
        //}

        ///// <summary>
        ///// Export Route Stop To Excel
        ///// </summary>
        ///// <param name="folderName"></param>
        ///// <param name="stops"></param>
        //public void ExportRouteStopToExcel(string folderName,List<RouteStop> stops)
        //{
        //    if (stops.IsListFull())
        //    {
        //        string fileName = string.Format("{0}{1}{2}{3}", folderName, "\\StopExcel_", DateTime.Now.ToString("ddMMyyyy_hhmmss"), ".csv");
        //        var sb = new StringBuilder();
        //        if (GlobalData.LoginUser.UserOperator.OperatorGroup == enmOperatorGroup.Operator)
        //        {
        //            sb.AppendLine(string.Format(
        //                        "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}",
        //                        "שם מפעיל", COMMA,
        //                        "שם אשכול", COMMA,
        //                        "מס' קו", COMMA,
        //                        "מקט", COMMA,
        //                        "כיוון", COMMA,
        //                        "חלופה", COMMA,
        //                        "מספר סידורי", COMMA,
        //                        "מקט תחנה", COMMA,
        //                        "שם תחנה", COMMA,
        //                        "פעילות בתחנה", COMMA,
        //                        "מרחק בין תחנות", COMMA,
        //                        "מרחק מצטבר", COMMA,
        //                        "עיר", COMMA,
        //                        "Lat", COMMA,
        //                        "Long"
        //                        ));
        //        }
        //        else 
        //        {
        //            sb.AppendLine(string.Format(
        //                        "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}",
        //                        "שם מפעיל", COMMA,
        //                        "שם אשכול", COMMA,
        //                        "מס' קו", COMMA,
        //                        "מקט", COMMA,
        //                        "כיוון", COMMA,
        //                        "חלופה", COMMA,
        //                        "מספר סידורי", COMMA,
        //                        "מקט תחנה", COMMA,
        //                        "שם תחנה", COMMA,
        //                        "פעילות בתחנה", COMMA,
        //                        "מרחק בין תחנות", COMMA,
        //                        "מרחק מצטבר", COMMA,
        //                        "עיר", COMMA,
        //                        "Lat", COMMA,
        //                        "Long", COMMA,
        //                        "סטטוס תחנה"
        //                        ));
        //        }
        //        int distPrev = 0, distStart = 0;
        //        for (int i = 0; i < stops.Count; i++)
        //        {
        //            RouteStop rs = stops[i];
        //            if (i > 0)
        //            {
        //                distPrev = Convert.ToInt32((stops[i].Milepost - stops[i - 1].Milepost) * ExportConfiguration.ExportConfigurator.GetConfig().LengthConverter * GlobalConst.MileToKilometerCoeffitient);
        //                distStart += distPrev;
        //            }
        //            RouteLine rl = GlobalData.RouteModel.RouteLineList.FindLast(rlInner => rlInner.ID == rs.Route_ID);
        //            if (rl != null)
        //            {
        //                string stopCity = null;
        //                try
        //                {
        //                    stopCity = transCadBLManager.GetStopCity(rs);
        //                }
        //                catch
        //                {
        //                    stopCity = string.Empty ;
        //                }
        //                PhysicalStop ps = GlobalData.RouteModel.PhysicalStopList.FindLast(psInner => psInner.ID == rs.Physical_Stop_ID);
        //                if (GlobalData.LoginUser.UserOperator.OperatorGroup == enmOperatorGroup.Operator)
        //                {
        //                    sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}",
        //                        GlobalData.LoginUser.UserOperator.OperatorName, COMMA,
        //                        rl.ClusterName, COMMA,
        //                        rl.RouteNumber, COMMA,
        //                        rl.Catalog, COMMA,
        //                        rl.Dir, COMMA,
        //                        rl.Var, COMMA,
        //                        rs.Ordinal, COMMA,
        //                        ps.StationCatalog, COMMA,
        //                        ps.Name.Replace(COMMA, ""), COMMA,
        //                        rs.StationTypeName, COMMA,
        //                        distPrev, COMMA,
        //                        distStart, COMMA,
        //                        stopCity, COMMA,
        //                        rs.Latitude, COMMA,
        //                        rs.Longitude
        //                       ));
        //                }
        //                else
        //                {
        //                    sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}",
        //                       GlobalData.LoginUser.UserOperator.OperatorName, COMMA,
        //                       rl.ClusterName, COMMA,
        //                       rl.RouteNumber, COMMA,
        //                       rl.Catalog, COMMA,
        //                       rl.Dir, COMMA,
        //                       rl.Var, COMMA,
        //                       rs.Ordinal, COMMA,
        //                       ps.StationCatalog, COMMA,
        //                       ps.Name.Replace(COMMA, ""), COMMA,
        //                       rs.StationTypeName, COMMA,
        //                       distPrev, COMMA,
        //                       distStart, COMMA,
        //                       stopCity, COMMA,
        //                       rs.Latitude, COMMA,
        //                       rs.Longitude,COMMA, rs.PhysicalStop.StationStatus
        //                      ));
        //                }

        //            }
        //        }
        //        IOHelper.WriteToFile(fileName, sb.ToString());
        //        ProcessLauncher.RunProcess("explorer", folderName);
        //    }
        //}

        public string ExportRouteLinesToExcel(string folderName, List<RouteLine> routes)
        {
            if (routes.IsListFull())
            {
                string fileName = string.Format("{0}{1}{2}{3}", folderName, "\\RouteExcel_", DateTime.Now.ToString("ddMMyyyy_hhmmss"), ".csv");
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}",
                            "שם מפעיל", Comma,
                            "שם אשכול", Comma,
                            "מס' קו", Comma,
                            "מקט", Comma,
                            "כיוון", Comma,
                            "חלופה", Comma,
                            "תיאור הדרך", Comma,
                            "סוג שרות", Comma,
                            "שילוט", Comma,
                            "מחוז ראשי", Comma,
                            "מחוז משנה", Comma,
                            "זמן הגדרה"
                            ));

                routes.ForEach(rl => sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}",
                                                                 GlobalData.LoginUser.UserOperator.OperatorName, Comma,
                                                                 rl.ClusterName, Comma,
                                                                 rl.RouteNumber, Comma,
                                                                 rl.Catalog, Comma,
                                                                 rl.Dir, Comma,
                                                                 rl.Var, Comma,
                                                                 rl.RoadDescription.Replace(Comma, ""), Comma,
                                                                 rl.ServiceTypeName, Comma,
                                                                 rl.Signpost, Comma,
                                                                 rl.ZoneHeadName, Comma,
                                                                 rl.ZoneSubHeadName, Comma,
                                                                 rl.Hagdara
                                                       )));
                IoHelper.WriteToFile(fileName, sb.ToString());
                return fileName;
                //ProcessLauncher.RunProcess("explorer", folderName);
            }
            return string.Empty;
        }
        
        public string ExportRouteStopToExcel(string folderName, List<RouteStop> stops)
        {
            if (stops.IsListFull())
            {
                string fileName = string.Format("{0}{1}{2}{3}", folderName, "\\StopExcel_", DateTime.Now.ToString("ddMMyyyy_hhmmss"), ".csv");
                var sb = new StringBuilder();
                if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.Operator)
                {
                    sb.AppendLine(string.Format(
                                "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}",
                                "שם מפעיל", Comma,
                                "שם אשכול", Comma,
                                "מס' קו", Comma,
                                "מקט", Comma,
                                "כיוון", Comma,
                                "חלופה", Comma,
                                "מספר סידורי", Comma,
                                "מקט תחנה", Comma,
                                "שם תחנה", Comma,
                                "פעילות בתחנה", Comma,
                                "מרחק בין תחנות", Comma,
                                "מרחק מצטבר", Comma,
                                "עיר", Comma,
                                "Lat", Comma,
                                "Long"
                                ));
                }
                else
                {
                    sb.AppendLine(string.Format(
                                "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}",
                                "שם מפעיל", Comma,
                                "שם אשכול", Comma,
                                "מס' קו", Comma,
                                "מקט", Comma,
                                "כיוון", Comma,
                                "חלופה", Comma,
                                "מספר סידורי", Comma,
                                "מקט תחנה", Comma,
                                "שם תחנה", Comma,
                                "פעילות בתחנה", Comma,
                                "מרחק בין תחנות", Comma,
                                "מרחק מצטבר", Comma,
                                "עיר", Comma,
                                "Lat", Comma,
                                "Long", Comma,
                                "סטטוס תחנה"
                                ));
                }
                int distPrev = 0, distStart = 0;
                for (int i = 0; i < stops.Count; i++)
                {
                    RouteStop rs = stops[i];
                    if (i > 0)
                    {
                        distPrev = Convert.ToInt32((stops[i].Milepost - stops[i - 1].Milepost) * ExportConfiguration.ExportConfigurator.GetConfig().LengthConverter * GlobalConst.MileToKilometerCoeffitient);
                        distStart += distPrev;
                    }
                    RouteLine rl = GlobalData.RouteModel.RouteLineList.FindLast(rlInner => rlInner.ID == rs.RouteId);
                    if (rl != null)
                    {
                        string stopCity;
                        try
                        {
                            stopCity = _transCadBlManager.GetStopCity(rs);
                        }
                        catch
                        {
                            stopCity = string.Empty;
                        }
                        PhysicalStop ps = GlobalData.RouteModel.PhysicalStopList.FindLast(psInner => psInner.ID == rs.PhysicalStopId);
                        if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.Operator)
                        {
                            sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}",
                                GlobalData.LoginUser.UserOperator.OperatorName, Comma,
                                rl.ClusterName, Comma,
                                rl.RouteNumber, Comma,
                                rl.Catalog, Comma,
                                rl.Dir, Comma,
                                rl.Var, Comma,
                                rs.Ordinal, Comma,
                                ps.StationCatalog, Comma,
                                ps.Name.Replace(Comma, ""), Comma,
                                rs.StationTypeName, Comma,
                                distPrev, Comma,
                                distStart, Comma,
                                stopCity, Comma,
                                rs.Latitude, Comma,
                                rs.Longitude
                               ));
                        }
                        else
                        {
                            sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}",
                               GlobalData.LoginUser.UserOperator.OperatorName, Comma,
                               rl.ClusterName, Comma,
                               rl.RouteNumber, Comma,
                               rl.Catalog, Comma,
                               rl.Dir, Comma,
                               rl.Var, Comma,
                               rs.Ordinal, Comma,
                               ps.StationCatalog, Comma,
                               ps.Name.Replace(Comma, ""), Comma,
                               rs.StationTypeName, Comma,
                               distPrev, Comma,
                               distStart, Comma,
                               stopCity, Comma,
                               rs.Latitude, Comma,
                               rs.Longitude, Comma, rs.PhysicalStop.StationStatus
                              ));
                        }

                    }
                }
                IoHelper.WriteToFile(fileName, sb.ToString());
                return fileName;
                //ProcessLauncher.RunProcess("explorer", folderName);
            }
            return string.Empty;
        }



        /// <summary>
        /// Is Selectable Route Stop
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool IsSelectableRouteStop(RouteStop rs)
        {
            if (rs == null)
                return false;
            if (rs.Ordinal == 1)
                return false;
            var rsList = GlobalData.RouteModel.RouteStopList.FindAll(routeStops => routeStops.RouteId == rs.RouteId);
            if (rsList.Count == rs.Ordinal)
                return false;
            return true;
        }

        
        private static string GetStationNameByPhysicalStopId(int physicalStopId)
        {
            
            string name = string.Empty;
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {
                var ps = GlobalData.RouteModel.PhysicalStopList.FindLast(psInner => psInner.ID == physicalStopId);
                if (ps != null)
                    name = ps.Name;
            }
            return  name;
        }


        /// <summary>
        /// GetStationCatalogHorada
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public List<BaseTableEntity> GetStationCatalogHorada(RouteStop routeStop)
        {
            var lst = new List<BaseTableEntity>();
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                lst = (from   p in GlobalData.RouteModel.RouteStopList
                       where  p.RouteId == routeStop.RouteId &&
                              p.IdStationType >=2 &&
                              p.Ordinal > routeStop.Ordinal
                       orderby routeStop.Ordinal
                       select new BaseTableEntity { ID = p.Ordinal, Name = GetStationNameByPhysicalStopId(p.PhysicalStopId) }).ToList<BaseTableEntity>();
            }
            //lst.Insert(0,new BaseTableEntity { ID = 0, Name = String.Empty });  
            return lst;
        }
        /// <summary>
        /// ShowLinePassInStationToolBox
        /// </summary>
        public void ShowLinePassInStationToolBox()
        {
            const string macroName = "addST";
            ITransCadMunipulationDataDAL dalTranscad = new TransCadMunipulationDataDAL();
            dalTranscad.RunExportMacro(macroName,ExportConfigurator.GetConfig().ShowLineInStationToolBox, null);
        }

        /// <summary>
        /// GetExclusivityLineType
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetExclusivityLineType()
        {
            return GetBaseTableDataList(GlobalConst.baseExclusivityLine);
        }

        #endregion
    }
}
