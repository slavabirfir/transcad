using System.Collections.Generic;
using BLEntities.Entities;

namespace IBLManager
{
    public interface  IBlLinkedMunicipalityStations
    {
        List<BaseTableEntity> GetSelectedStations(string station, bool isAll);
        bool LinkStationtoMunArea(BaseTableEntity physicalStop, int munId);
        List<City> GetMunAreasByPhysicalStop(BaseTableEntity physicalStop);
        string GetCityNameByPs(BaseTableEntity physicalStop);
        string StationCount { get; }

        void ShowStationOnMap(BaseTableEntity baseTableEntity);
    }
}
