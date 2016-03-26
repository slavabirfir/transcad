using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using System.ComponentModel;

namespace IBLManager
{
    public interface IDataSearchAndManipulateBLManager
    {
        int? IDRouteLineForStationsQuery { get; set; }

        void UpdateBatchStationType(List<RouteStop> routeStops, int routeStop);

        string LastRouteLineSortedProperty { get; set; }
        string CurrentRouteLineSortedProperty { get; set; }

        List<RouteLine> GetSearchedSortedRouteLines(string routeNumber, 
            string direction, string variant,bool isNotValidOnly,
             string catalog, string cluster, bool isNewEntity);

        List<RouteStop> GetSearchedRouteStops(string routeNumber,
            string direction, string variant, bool isNotValidOnly,
            string stationName, string catalog, string catalogStation);

        List<CatalogInfo> GetCatalogListEntities();
        List<RouteLine> GetRouteLineSortedList(string name);


        List<BaseTableEntity> GetClusters();
        List<BaseTableEntity> GetRouteType();
        List<BaseTableEntity> GetServiceType();
        //List<BaseTableEntity> GetZonesHead();
        //List<BaseTableEntity> GetZonesSubHead();
        List<BaseTableEntity> GetSeasonal();
        List<string> GetDirectionList();

        List<BaseTableEntity> GetStationType();

        int RouteLineCount { get; }
        int RouteStopsCount { get; }
        int PhisicalStopsCount { get; }

        bool IsRouteLineValid(RouteLine routeLine);
        bool IsRouteStopValid(RouteStop routeStop);

        List<string> GetCatalogList();
        List<string> GetClusterList();

        List<string> GetRouteNumberList();
        List<string> GetStationCatalogList();
        List<string> GetStationNameList();

        List<BaseTableEntity> GetVehicleTypes();
        List<BaseTableEntity> GetVehicleSizes();
        
        bool IsVariantExistsInVariantNumTable(string variant);

        StationOrderConst GetStationOrder(RouteStop routeStop);

        List<BaseTableEntity> GetStationCatalogHorada(RouteStop routeStop);

        void UpdateCatalogOfRouteLines(CatalogInfo catalogInfo);
        void SetCatalogDataByValueChanging(int? catalog, RouteLine routeLine);


        //void ExportRouteLinesToExcel(string folderName,List<RouteLine> routes);
        //void ExportRouteStopToExcel(string folderName,List<RouteStop> stops);
        string ExportRouteLinesToExcel(string folderName, List<RouteLine> routes);
        string ExportRouteStopToExcel(string folderName, List<RouteStop> stops);


        bool IsSelectableRouteStop(RouteStop rs);

        void ShowLinePassInStationToolBox();

        List<BaseTableEntity> GetExclusivityLineType();
    }
}
