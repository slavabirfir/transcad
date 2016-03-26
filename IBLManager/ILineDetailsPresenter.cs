using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace IBLManager
{
    public interface ILineDetailsPresenter
    {
        void UpdateClusterZone(RouteLine routeLine, ClusterState clusterState);
        RouteLine GetLastFilterdItem(List<RouteLine> filteredList, RouteLine current);
        RouteLine GetFirstFilterdItem(List<RouteLine> filteredList, RouteLine current);
        RouteLine GetNextFilterdItem(List<RouteLine> filteredList, RouteLine current);
        RouteLine GetPrevFilterdItem(List<RouteLine> filteredList, RouteLine current);
        List<BaseTableEntity> GetClusters();
        List<BaseTableEntity> GetRouteType();
        List<BaseTableEntity> GetServiceType();
        List<BaseTableEntity> GetExclusivityLineType();
        List<BaseTableEntity> GetVehicleTypes();
        List<BaseTableEntity> GetVehicleSizes();
        List<BaseTableEntity> GetSelectedVehicleTypes(string delimitedValues);
        List<BaseTableEntity> GetSelectedVehicleSizes(string delimitedValues);
        List<BaseTableEntity> GetSeasonal();
        List<string> GetDirections();
        bool SaveRouteLine(RouteLine routeLine);//,ClusterState clusterState);

        bool ShowSelectStateForm(RouteLine routeLine);

        CatalogInfo AddCatalogToCatalogList(int catalog,  int idCluster,int accountingGroupId, int routeNumber);
        void RemoveCatalogFromList(CatalogInfo catalogInfo);
        bool IsRouteLineValid(RouteLine routeLine);
        bool IsVariantExistsInVariantNumTable(string variant);
        void UpdateCatalogOfRouteLines(CatalogInfo catalogInfo);
        bool IsServiceTypeDisable(int routeType);
        void SetInitiateCatalogData(RouteLine routeLine);
        int  CollectServiceType { get; }
        void SetCatalogDataByValueChanging(int? catalog, RouteLine routeLine);
        CatalogInfo GetCatalogInfo(int? catalog);
        bool IsLuzOfLineExists(RouteLine routeLine);
        bool IsEgedOperator { get; }
        List<AccountingGroup> GetAccountingGroupByOperatorAndCluster(BaseTableEntity cluster, int currentAccountGroupByOperatorId);
        bool UpdateCatalogWithSelectedCatalog(RouteLine routeLine, int catalog, string newRouteNumber, ref string error);
        bool UpdateClusterForSameRouteLines(RouteLine rouleLine, BaseTableEntity cluster);
        bool UpdateAccountingGroupForSameRouteLines(RouteLine rouleLine, AccountingGroup accountiongGroup);
        bool IsSuperViser { get; }
        bool IsPlanningFirm { get; }
        List<CatalogInfo> GetCatalogsOfSameRouteNumber(RouteLine rouleLine, string newRouteNumber);
    }
}
