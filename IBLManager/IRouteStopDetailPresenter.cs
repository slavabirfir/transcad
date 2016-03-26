using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace IBLManager
{
    public interface IRouteStopDetailPresenter
    {
        bool SaveRouteStop(RouteStop rs);
        bool IsRouteStopValid(RouteStop routeStop);
        void SetNotInUseFloorAndPlatform(RouteStop routeStop);
        bool IsExistsHoradaStationInLine(int routeId);
        void SetHoradaInSpecificRouteLine(int routeId);
        RouteStop GetLastFilterdItem(List<RouteStop> filteredList, RouteStop current);
        RouteStop GetFirstFilterdItem(List<RouteStop> filteredList, RouteStop current);
        RouteStop GetNextFilterdItem(List<RouteStop> filteredList, RouteStop current);
        RouteStop GetPrevFilterdItem(List<RouteStop> filteredList, RouteStop current);
        List<BaseTableEntity> GetStationType();
        List<BaseTableEntity> GetStationCatalogHorada(RouteStop routeStop);
        StationOrderConst GetStationOrder(RouteStop routeStop);
        bool IsLoweringStation(BaseTableEntity be);
    }
}
