using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using IDAL;
using BLEntities.SQLServer ;
using BLEntities.Entities;
using DAL;
using ExportConfiguration;
using Utilities;
using System.IO;
using System.Reflection;
using BLEntities.Model;
using BLEntities.Accessories;
using System.Data;
using System.Collections;

namespace BLManager
{
    public class ExportBaseToSQLBL : IExportBaseBlManager //IExportBLManager
    {
        #region const
        public const string CITYDAT = "City.dat";
        public const string NODEDAT = "Node.dat";
        public const string STOPDAT = "Stop.dat";
        private const string PHYSICALSTATIONCATALOGSTARTWITH6ANDMORETHEN65535 = "PhysicalStationCatalogStartWith6AndMoreThen65535.csv";
        private const int ZERRO = 0;
        private const string  SIX = "6";
        private const string STATIONCATALOG = "StationCatalog";
        private const int CatalogStationUpperLimit = 65535;
        #endregion

        ITransCadMunipulationDataDAL dalTranCad = new TransCadMunipulationDataDAL();
        IExportInfrastructureToSQLDAL dalSQLExport = new ExportBaseToSqldal();
        /// <summary>
        /// Get File Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFileName(string fileName)
        {
            return IoHelper.GetFileName(fileName, ExportConfigurator.GetConfig().DataFolder);
        }

        
        /// <summary>
        /// Get List Data From File
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileNameFullpath"></param>
        /// <returns></returns>
        private List<T> GetListDataFromFile<T>(string fileNameFullpath)
        {
            string json = IoHelper.ReadFromFile(GetFileName(fileNameFullpath));
            if (!string.IsNullOrEmpty(json))
                return JSONHelper.JsonDeSerialize<List<T>>(json);
            else
                return null;
        }

        /// <summary>
        /// Write Infractructure
        /// </summary>
        private void WriteInfractructureToLocalStorage()
        {
            List<City> cityList = dalTranCad.GetCities(ExportConfigurator.GetConfig().MunLayer);
            string json = JSONHelper.JsonSerializeToString<List<City>>(cityList);
            IoHelper.WriteToFile(GetFileName(ExportBaseToSQLBL.CITYDAT), json);

            List<Node> nodeList = dalTranCad.GetNodes(ExportConfigurator.GetConfig().Endpoints, ExportConfigurator.GetConfig().MunLayer);
            json = JSONHelper.JsonSerializeToString<List<Node>>(nodeList);
            IoHelper.WriteToFile(GetFileName(ExportBaseToSQLBL.NODEDAT), json);

            List<Stop> stopList = dalTranCad.GetStops(ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MunLayer, ExportConfigurator.GetConfig().MapLayerName);
            json = JSONHelper.JsonSerializeToString<List<Stop>>(stopList);
            IoHelper.WriteToFile(GetFileName(ExportBaseToSQLBL.STOPDAT), json);
        }

        /// <summary>
        /// Verify End Points Layer Is Full
        /// </summary>
        /// <returns></returns>
        private bool VerifyEndPointsLayerIsFull()
        {
            var res = !dalTranCad.IsEndPointHasEmptyCityOrTypeDavid(ExportConfigurator.GetConfig().Endpoints);
            if (!res)
            {
                Logger.LoggerManager.WriteToLog("End Point Has Empty City Or TypeDavid");
            }
            return res;
        }

        /// <summary>
        /// Export Data 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportBaseData(ref string message)
        {
            var dtStationStartWith6AndMoreThen65535 = GetPhisicalStopCatalogStartWith6AndValueMoreThen65535();
            if (dtStationStartWith6AndMoreThen65535.IsDataTableFull())
            {
                message = Resources.BLManagerResource.PhisicalStopsCatalogMore65535;
                var fileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), PHYSICALSTATIONCATALOGSTARTWITH6ANDMORETHEN65535);
                BLSharedUtils.WriteCsvFile(fileFullPath, dtStationStartWith6AndMoreThen65535);
                ProcessLauncher.RunProcess("notepad.exe", "\"" + fileFullPath + "\"");
                return false;
            }
            var linkStationToCityBl = new BlLinkStationToCity();
            if (linkStationToCityBl.WriteUnlinkedStations())
            {
                message = Resources.BLManagerResource.PhisicalStopsCatalogNoHaveIsMun;
                return false;
            }
            if (!VerifyEndPointsLayerIsFull())
            {
                message = Resources.BLManagerResource.EndPointsMacroShouldBeRun;
                return false;
            }
            OnChanged(new ImportToSQLArgs { MaxProgressBarValue = 10, ProgressBarValue = 2, Message = Resources.BLManagerResource.EgedDBUpdate });
            var loadEgedMapBl = new LoadEgedMapBL();
            loadEgedMapBl.LoadEndPointsFromTranscadAndPhysicalStopsForEgedLoadMapProcess();
            OnChanged(new ImportToSQLArgs { MaxProgressBarValue = 10, ProgressBarValue = 1, Message = Resources.BLManagerResource.LoadTranscadCity });
            var cityList = dalTranCad.GetCities(ExportConfigurator.GetConfig().MunLayer);
            OnChanged(new ImportToSQLArgs { MaxProgressBarValue = 10, ProgressBarValue = 2, Message = Resources.BLManagerResource.LoadTranscadJunction });
            var nodeList = dalTranCad.GetNodes(ExportConfigurator.GetConfig().Endpoints, ExportConfigurator.GetConfig().MunLayer);
            OnChanged(new ImportToSQLArgs { MaxProgressBarValue = 10, ProgressBarValue = 3, Message = Resources.BLManagerResource.LoadTranscadStation });
            var stopList = dalTranCad.GetStops(ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MunLayer, ExportConfigurator.GetConfig().MapLayerName);
            stopList = BLSharedUtils.FilterTrainStops(stopList);

            if (!cityList.IsListFull())
            {
                message = Resources.BLManagerResource.CityListIsNull;
                return false;
            }
            if (!nodeList.IsListFull())
            {
                message = Resources.BLManagerResource.NodeListIsNull;
                return false;
            }
            if (!stopList.IsListFull())
            {
                message = Resources.BLManagerResource.StationListIsNull;
                return false;
            }
            if (!GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                message = Resources.BLManagerResource.RouteLineIsEmpty;
                return false;
            }
            List<CityImport> citySqlList = null;
            if (cityList.IsListFull())
            {
                citySqlList = new List<CityImport>();
                var counter = 0;
                cityList.ForEach(c =>
                {
                    var cityImport = new CityImport { CityId = (String.IsNullOrEmpty(c.Code) || !Validators.IsNumeric(c.Code)) ? 0 : int.Parse(c.Code), CityName = c.Name , AuthorityId = c.AuthorityId};
                    if (!citySqlList.Exists(cd => cd.CityId == cityImport.CityId))
                    {
                        citySqlList.Add(cityImport);
                        OnChanged(new ImportToSQLArgs  { MaxProgressBarValue = cityList.Count, ProgressBarValue = counter++, Message = Resources.BLManagerResource.BuildingDataOfTranscadCity }); 
                    }});
            }
            List<JunctionImport> junctionSqlList = null;
            if (nodeList.IsListFull())
            {
                junctionSqlList = new List<JunctionImport>();
                int counter = 0;
                nodeList.ForEach(junction =>
                    {
                        if (cityList.Exists(c => c.Code == junction.CityCode))
                        {
                            var junctionImport = new JunctionImport
                            {
                                CityId = (String.IsNullOrEmpty(junction.CityCode) || !Validators.IsNumeric(junction.CityCode)) ? 0 : int.Parse(junction.CityCode),
                                JunctionDesc = junction.Name,
                                JunctionId = int.Parse(junction.NodeID),
                                Long = junction.Long,
                                Lat = junction.Lat
                            };
                            junctionSqlList.Add(junctionImport);
                            OnChanged(new ImportToSQLArgs
                            {
                                MaxProgressBarValue = nodeList.Count,
                                ProgressBarValue = counter++,
                                Message = Resources.BLManagerResource.BuildingDataOfTranscadJunction
                            });
                        }
                    });
            }
            List<StationImport> stationImportList = null;
            if (stopList.IsListFull())
            {
                stationImportList = new List<StationImport>();
                int counter = 0;
                DateTime now = DateTime.Now; 
                stopList.ForEach(stop =>
                {
                    var staitonImport = new StationImport
                    {
                        CityId = stop.CityCode,
                        HouseNumber = stop.House == ZERRO ? null : stop.House.ToString(),
                        StreetName = stop.Street,
                        StationName = stop.Name,
                        StationShortName = stop.StationShortName,
                        Lat = stop.Lat,
                        Long = stop.Long,
                        B56Heb = string.Empty,
                        B56Eng = string.Empty,
                        StationId = stop.ID_SEKER,
                        LinkId = stop.LinkID,
                        StationCatalog = stop.ID_SEKER.ToString("00000"),
                        LatDifferrent = stop.LatDifferrent,
                        LongDifferrent = stop.LongDifferrent,
                        StationStatusId = stop.StationStatusId,
                        StationTypeId = stop.StationTypeId,
                        UpdateDate = now ,
                        AreaOperatorId = stop.AreaOperatorId 
                    };
                    stationImportList.Add(staitonImport);

                    OnChanged(new ImportToSQLArgs
                    {
                        MaxProgressBarValue = stopList.Count,
                        ProgressBarValue = counter++,
                        Message = Resources.BLManagerResource.BuildingDataOfTranscadStation
                    });

                });
            }
           
            dalSQLExport.Changed+=dalSQLExport_Changed;
            dalSQLExport.ExportInfrasructureData(citySqlList, junctionSqlList, stationImportList, ref message);
            return true;
        }

        


        private static DataTable GetPhisicalStopCatalogStartWith6AndValueMoreThen65535()
        {
            var dt = new DataTable();
            dt.Columns.Add(STATIONCATALOG, typeof(string)); 
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {
                var list = (from st in GlobalData.RouteModel.PhysicalStopList
                            where st.StationCatalog.StartsWith(SIX) && Convert.ToInt32(st.StationCatalog) >= CatalogStationUpperLimit
                    orderby st.StationCatalog
                    select st).ToList<PhysicalStop>();
                if (list.IsListFull())
                {
                    list.ForEach(st =>
                    {
                        DataRow dr = dt.NewRow();
                        dr[STATIONCATALOG] = st.StationCatalog;
                        dt.Rows.Add(dr);
                    });
                } 
            }
            return dt;
        }
        /// <summary>
        /// dal SQLExport Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dalSQLExport_Changed(object sender, ImportToSQLArgs e)
        {
            this.OnChanged(e);
        }
        
        /// <summary>
        /// Changed
        /// </summary>
        public event EventHandler<ImportToSQLArgs> Changed;

        /// <summary>
        /// On Changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(ImportToSQLArgs e)
        {
            if (this.Changed != null)
                Changed(this, e);
        }
    }
}
