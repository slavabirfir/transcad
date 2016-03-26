using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BLEntities.Entities;
using BLEntities.Model;
using DAL;
using ExportConfiguration;
using IBLManager;
using IDAL;
using Utilities;

namespace BLManager
{
    public class BlLinkedMunicipalityStations : IBlLinkedMunicipalityStations
    {
        private int _stationCount;
        private readonly ITransCadMunipulationDataDAL _transCadMunipulationDataDal;
        private readonly List<City> _cites;
        public BlLinkedMunicipalityStations()
        {
            _transCadMunipulationDataDal = new TransCadMunipulationDataDAL();
            _cites = _transCadMunipulationDataDal.GetCities(ExportConfigurator.GetConfig().MunLayer);
        }

        #region IBlLinkedMunicipalityStations Members

        public List<BaseTableEntity> GetSelectedStations(string catalog, bool isAll)
        {
            if (GlobalData.RouteModel.PhysicalStopList!=null && GlobalData.RouteModel.PhysicalStopList.Any())
            {
                var lst = (from p in GlobalData.RouteModel.PhysicalStopList
                        where (string.IsNullOrEmpty(catalog) ? true : p.StationCatalog.Contains(catalog)) &&
                              (isAll ? true : p.CityCode == "0000") // p.CityLinkCode == enmLinkStationCity.Manual
                        select  new BaseTableEntity
                                    {
                                        ID = p.ID,
                                        Name = string.Format("{0} - {1}",p.StationCatalog,p.Name) 
                                    }).OrderBy(p=>p.Name).ToList();
                _stationCount = lst.Count;
                return  lst.OrderBy(s=>s.Name).ToList();
            }
            return null;
        }

        public bool LinkStationtoMunArea(BaseTableEntity stationCatalog, int munId)
        {
            var city = _cites.SingleOrDefault(c => c.MunId == munId);
            var ps = GlobalData.RouteModel.PhysicalStopList.SingleOrDefault(p => p.ID == stationCatalog.ID);
            if (city != null && ps != null)
            {
                ps.CityCode = city.Code;
                ps.CityLinkCode = enmLinkStationCity.Manual;
                return _transCadMunipulationDataDal.SavePhisicalStopStop(ps,
                                                                         ExportConfiguration.ExportConfigurator.
                                                                             GetConfig().PhisicalStopsLayerName);
            }
            return false;
                
        }

        public List<City> GetMunAreasByPhysicalStop(BaseTableEntity physicalStop)
        {
            var citiesListforPs = new List<City>();
            var ps = GlobalData.RouteModel.PhysicalStopList.SingleOrDefault(p => p.ID == physicalStop.ID);
            if (ps == null) return citiesListforPs;
 
            var munIds = _transCadMunipulationDataDal.GetNearestIdMunAreasToPhysicalStop(
                ExportConfigurator.GetConfig().CalcCloseAreasToStationMacro,
                new Coord { Latitude = ps.Latitude, Longitude = ps.Longitude },
                ExportConfigurator.GetConfig().MunLayer, 10000);
            
            if (!string.IsNullOrEmpty(munIds))
            {
                foreach (var mun in munIds.Split(";".ToCharArray()))
                {
                    if (!string.IsNullOrEmpty(mun))
                    {
                        string mun1 = mun;
                        var city = _cites.SingleOrDefault(c => c.MunId == Convert.ToInt32(mun1));
                        if (city != null)
                            citiesListforPs.Add(city);
                    }
                }
            }
            return citiesListforPs.OrderBy(c=>c.Name).ToList();
        }

       
        public string GetCityNameByPs(BaseTableEntity physicalStop)
        {
            var ps = GlobalData.RouteModel.PhysicalStopList.SingleOrDefault(p => p.ID == physicalStop.ID);
            if (ps == null) return string.Empty;

            var city = _cites.SingleOrDefault(c => c.Code == ps.CityCode);
            return city == null ? string.Empty : city.Name ;
        }

        public string StationCount
        {
            get { return _stationCount.ToString(); }
        }

        public void ShowStationOnMap(BaseTableEntity physicalStop)
        {
            var ps = GlobalData.RouteModel.PhysicalStopList.SingleOrDefault(p => p.ID == physicalStop.ID);
            if (ps == null) return;
            _transCadMunipulationDataDal.ZoomToLayerFeutureById(ExportConfigurator.GetConfig().PhisicalStopsLayerName, new List<int> { ps.ID }, ps.StationCatalog);
            var imageBus = GetImageBus();
            _transCadMunipulationDataDal.SetLabelOnMap(ExportConfigurator.GetConfig().PhisicalStopsLayerName, true, "ID_SEKER",true,imageBus);
        }

        private static string GetImageBus()
        {
            var imageBus = Path.Combine(ConfigurationExportHelper.GetAppSettings("imagePath"), "BUS3.BMP");
            if (!IoHelper.IsFileExists(imageBus))
                imageBus = string.Empty;
            return imageBus;
        }

        #endregion
    }
}
