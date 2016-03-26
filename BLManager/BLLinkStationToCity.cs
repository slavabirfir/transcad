using System;
using System.Collections.Generic;
using System.Linq;
using BLEntities.Entities;
using BLEntities.Model;
using Utilities;
using IDAL;
using DAL;
using System.Data;
using System.IO;

namespace BLManager
{
    public class StationToCityLink
    {
        public int ID { get; set; }
        public string Desc { get; set; }
        public string Catalog { get; set; }
        public string CityCode { get; set; }
        public enmLinkStationCity LinkStationCityEnum { get; set; }
        public override string ToString()
        {
            return string.Format("{0} ({1})", Desc, Catalog);
        }

    }

    public class BlLinkStationToCity
    {
        #region
        private const string Stationcatalog = "מקט תחנה";
        private const string Stationname = "שם תחנה";
        public List<StationToCityLink> SelectedStationList { get; set; }
        public List<City> SelectedCityList { get; set; }
        private List<City> _cities;
        private readonly ITransCadMunipulationDataDAL _trabscadDal = new TransCadMunipulationDataDAL();
        private const string Physicalstationcatalogsnohaveismun = "PhysicalStationCatalogNoHaveIsMun.csv";
        #endregion
        /// <summary>
        /// GetCity
        /// </summary>
        /// <param name="stationLink"></param>
        /// <returns></returns>
        public City GetCity(StationToCityLink stationLink)
        {
            City city = null;
            if (stationLink != null && stationLink.LinkStationCityEnum != enmLinkStationCity.NonLinked && _cities.IsListFull())
            {
                city = _cities.FirstOrDefault(elem => elem.Code == stationLink.CityCode);
            }
            return city; 
        }
        /// <summary>
        /// Link Station To City
        /// </summary>
        /// <param name="city"></param>
        /// <param name="stationToLink"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool LinkStationToCity(City city, StationToCityLink stationToLink, ref string  message)
        {
            PhysicalStop stopPhysical = GlobalData.RouteModel.PhysicalStopList.Single(ps => ps.ID == stationToLink.ID);
            if (stopPhysical.CityLinkCode != enmLinkStationCity.NonLinked) 
            {
                City cityInner = _cities.SingleOrDefault(c=>c.Code==stopPhysical.CityCode);
                if (city!=null)
                    message = string.Format(Resources.BLManagerResource.CantLinkAlreadyLinkedItem, cityInner.Name);
                else
                    message = string.Format(Resources.BLManagerResource.CantLinkAlreadyLinkedItem, stopPhysical.CityCode);
                return false; 
            }
            stopPhysical.CityCode = city.Code;
            stopPhysical.CityLinkCode = enmLinkStationCity.Manual;
            
            if (_trabscadDal.SavePhisicalStopStop(stopPhysical, ExportConfiguration.ExportConfigurator.GetConfig().PhisicalStopsLayerName))
            {
                UpdateStationToCityLink(stopPhysical, stationToLink);
            }
            return true;
        }
        /// <summary>
        /// UpdateStationToCityLink
        /// </summary>
        /// <param name="stopPhysical"></param>
        /// <param name="stationToCityLink"></param>
        private static void UpdateStationToCityLink(PhysicalStop stopPhysical, StationToCityLink stationToCityLink)
        {
            if (stationToCityLink!=null && stopPhysical != null)
            {
                 stationToCityLink.LinkStationCityEnum = stopPhysical.CityLinkCode;
                 stationToCityLink.CityCode = stopPhysical.CityCode;
            }
        }

        /// <summary>
        /// Un Link Station To City
        /// </summary>
        /// <param name="city"></param>
        /// <param name="stationToLink"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool UnLinkStationToCity(City city, StationToCityLink stationToLink, ref string message)
        {
            
            PhysicalStop stopPhysical = GlobalData.RouteModel.PhysicalStopList.Single(ps => ps.ID == stationToLink.ID);
            if (stopPhysical.CityLinkCode == enmLinkStationCity.NonLinked)
            {
                City cityInner = _cities.SingleOrDefault(c => c.Code == stopPhysical.CityCode);
                if (city != null)
                    message = string.Format(Resources.BLManagerResource.CantUnLinkAlreadyUnLinkedItem);
                else
                    message = string.Format(Resources.BLManagerResource.CantUnLinkAlreadyUnLinkedItem);
                return false;
            }

            stopPhysical.CityCode = string.Empty ;
            stopPhysical.CityLinkCode = enmLinkStationCity.NonLinked;

            if (_trabscadDal.SavePhisicalStopStop(stopPhysical, ExportConfiguration.ExportConfigurator.GetConfig().PhisicalStopsLayerName))
            {
                UpdateStationToCityLink(stopPhysical, stationToLink);
            }
            return true;
            
        }
        /// <summary>
        /// GetPhisicalStopCatalogNoHaveIsMun
        /// </summary>
        /// <returns></returns>
        private static DataTable GetPhisicalStopCatalogNoHaveIsMun()
        {
            
            var dt = new DataTable();
            dt.Columns.Add(Stationcatalog, typeof(string));
            dt.Columns.Add(Stationname, typeof(string));
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {
                var list = (from st in GlobalData.RouteModel.PhysicalStopList
                            where st.CityLinkCode == enmLinkStationCity.NonLinked
                            orderby st.StationCatalog
                            select st).ToList();
                if (list.IsListFull())
                {
                    list.ForEach(st =>
                    {
                        DataRow dr = dt.NewRow();
                        dr[Stationcatalog] = st.StationCatalog;
                        dr[Stationname] = st.Name;
                        dt.Rows.Add(dr);
                    });
                }
            }
            return dt;
        }
        /// <summary>
        /// WriteUnlinkedStations
        /// </summary>
        /// <returns></returns>
        public bool WriteUnlinkedStations()
        {
            DataTable dtStationNoHaveIsMun = GetPhisicalStopCatalogNoHaveIsMun();
            if (dtStationNoHaveIsMun.IsDataTableFull())
            {
                string fileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Physicalstationcatalogsnohaveismun);
                BLSharedUtils.WriteCsvFile(fileFullPath, dtStationNoHaveIsMun);
                ProcessLauncher.RunProcess("notepad.exe", "\"" + fileFullPath + "\"");
                return true;
            }
            return false;
        }
        /// <summary>
        /// SetFilteredStationList
        /// </summary>
        /// <param name="token"></param>
        public void SetFilteredStationList( string token)
        {
            
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {
                if  (string.IsNullOrEmpty(token))
                     SelectedStationList = (from p in GlobalData.RouteModel.PhysicalStopList
                                       where p.CityLinkCode == enmLinkStationCity.NonLinked 
                                       orderby p.Name
                                       select new StationToCityLink { CityCode = p.CityCode, Catalog = p.StationCatalog, Desc = p.Name, ID = p.ID,LinkStationCityEnum = p.CityLinkCode }).ToList();
                else
                    SelectedStationList = (from p in GlobalData.RouteModel.PhysicalStopList
                                           where p.StationCatalog.IndexOf(token) >= 0
                                           orderby p.Name
                                           select new StationToCityLink { CityCode = p.CityCode, Catalog = p.StationCatalog, Desc = p.Name, ID = p.ID, LinkStationCityEnum = p.CityLinkCode }).ToList();

            }
            else
            {
                SelectedStationList = new List<StationToCityLink>(); 
            }
        }
        /// <summary>
        /// SetFilteredCityList
        /// </summary>
        /// <param name="token"></param>
        public void SetFilteredCityList(string token)
        {
            
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {

                SelectedCityList = (from p in _cities
                                       where (string.IsNullOrEmpty(token) || p.Name.IndexOf(token) >= 0)
                                       select p).ToList();

            }
            else
            {
                SelectedCityList = new List<City>(); 
            }
        }
        
        /// <summary>
        /// InitData
        /// </summary>
        public void InitData()
        {
            IInternalBaseDal dal = new TransportLicensingDal();
            _cities = dal.GetCityListFromExternalSource();
            _cities.Insert(0, new 
                            City { 
                                    Name = Resources.BLManagerResource.MunicipalityArea, 
                                    Code = Resources.BLManagerResource.Code0000 
            });
        }
       
    }
}
