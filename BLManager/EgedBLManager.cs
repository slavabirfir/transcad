using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Model;
using IDAL;
using DAL;
using Utilities;
using BLEntities.Entities;
using BLEntities.Accessories;
using ExportConfiguration;
using System.IO;

namespace BLManager
{
    public class EgedBlManager : ITransCadBLManager
    {
        #region private
        private IInternalBaseDal internalBaseDAL = new InternalTvunaImplementationDal();
        private EgedSQLServerDAL egedSQLServerDAL = new EgedSQLServerDAL();
        private ITransCadMunipulationDataDAL transCadFetchData = new TransCadMunipulationDataDAL();
        #endregion

        #region ITransCadBLManager Members
        /// <summary>
        /// SetLoginUserInfoAndVerifyIsSuperViser
        /// </summary>
        /// <returns></returns>
        public bool SetLoginUserInfoAndVerifyToShowSelectOpersList()
        {
            return false;
        }
        /// <summary>
        /// BuildModelData
        /// </summary>
        public void BuildModelData()
        {
            var custerToZoneManagerBl = new ClusterToZoneManagerBL();
            custerToZoneManagerBl.SetCusterToZoneManagerBL();
            GlobalData.Directions = internalBaseDAL.GetDirectionList();
            GlobalData.BaseTableEntityDictionary = internalBaseDAL.GetBaseTableEntityList(GlobalData.LoginUser.UserOperator.IdOperator);
            GlobalData.BaseTableTranslatorList = internalBaseDAL.FillBaseTableTranslatorList();

            //List<string> lLayers = transCadFetchData.GetLayersName();

            Dictionary<int, PhysicalStop> dicPhysicalStop = egedSQLServerDAL.GetPhysicalStopsRawDataDictionary();
            if (!dicPhysicalStop.IsDictionaryFull())
            {
                throw new ApplicationException(Resources.BLManagerResource.EgedPhysicalDBListEmpty);
            }
            GlobalData.RouteModel.PhysicalStopList = dicPhysicalStop.Values.ToList();

            Dictionary<int, List<RouteStop>> dicrouteStop = egedSQLServerDAL.GetRouteStopsRawDataDictionary();
            if (!dicrouteStop.IsDictionaryFull())
            {
                throw new ApplicationException(Resources.BLManagerResource.EgedNispah3DBListEmpty);
            }

            GlobalData.RouteModel.RouteStopList = new List<RouteStop>();
            foreach (int routeId in dicrouteStop.Keys)
            {
                GlobalData.RouteModel.RouteStopList.AddRange(dicrouteStop[routeId]);
            }

            Dictionary<int, RouteLine> dicRouteLineList = egedSQLServerDAL.GetRouteLinesRawDataDictionary();
            if (!dicRouteLineList.IsDictionaryFull())
            {
                throw new ApplicationException(Resources.BLManagerResource.TranscadLinelDBListEmpty);
            }
            GlobalData.RouteModel.RouteLineList = dicRouteLineList.Values.ToList();

            GlobalData.TransCadCurrentEnvoromnetInfo = transCadFetchData.EgedGetTransCadCurrentEnvoromnetInfo();
            BLSharedUtils.BuildCatalogInfoList();
            // Set Route and Phisical Stop Objects of RouteStopList
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                GlobalData.RouteModel.RouteStopList.ForEach(
                    delegate(RouteStop routeStop)
                    {
                        if (!dicPhysicalStop.ContainsKey(routeStop.PhysicalStopId))
                        {
                            routeStop.PhysicalStop = null;
                        }
                        else
                        {
                            routeStop.PhysicalStop = dicPhysicalStop[routeStop.PhysicalStopId];
                        }
                        if (routeStop.PhysicalStop != null)
                        {
                                routeStop.StationCatalog = routeStop.PhysicalStop.StationCatalog;
                                routeStop.Name = routeStop.StationCatalog;
                                routeStop.Longitude = routeStop.PhysicalStop.Longitude;
                                routeStop.Latitude = routeStop.PhysicalStop.Latitude;
                        }
                        BLSharedUtils.SetRouteStopStationType(routeStop);
                        routeStop.RouteLine = dicRouteLineList[routeStop.RouteId];
                    });
            }

            GlobalData.RouteModel.RouteLineList.ForEach(it => BLSharedUtils.UpdateDropFirstStationData(dicrouteStop[it.ID]));
            if (!IsStartUpDone)
                IsStartUpDone = true;

        }

        public bool IsStartUpDone { get; set; }
        /// <summary>
        /// Zoom To Route Line sLayer Feuture B yId
        /// </summary>
        /// <param name="entityList"></param>
        public void ZoomToRouteLinesLayerFeutureById(List<BLEntities.Entities.RouteLine> entityList)
        {
            if (!entityList.IsListFull())
                return;
            List<int> ids = new List<int>();
            StringBuilder title = new StringBuilder();
            entityList.ForEach(delegate(RouteLine e)
            {
                title.Append(e.Name);
                title.Append("|");
                ids.Add(e.ID);
            });
           
            int[] idArray = ids.ToArray();
            string whereClass = "Makat8={0} OR ";
            StringBuilder sb = new StringBuilder(string.Concat("Select * from ", ExportConfigurator.GetConfig().EgedRouteSystemLayerName, " where "));
            foreach (int item in idArray)
            {
                sb.AppendFormat(whereClass, item);
            }
            String strQuery = sb.ToString().Substring(0, sb.ToString().Length - 4);
            transCadFetchData.RunExportMacro("eZ", ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroFile, new List<object> { ExportConfigurator.GetConfig().EgedRouteSystemLayerName, strQuery });


        }
        /// <summary>
        /// Zoom To Route Stops Layer Feuture ById
        /// </summary>
        /// <param name="entityList"></param>
        public void ZoomToRouteStopsLayerFeutureById(List<BLEntities.Entities.RouteStop> entityList)
        {
            StringBuilder title = new StringBuilder();
            List<Coord> coordinates = new List<Coord>();
            entityList.ForEach(delegate(RouteStop e)
            {
                PhysicalStop ps = GlobalData.RouteModel.PhysicalStopList.Find(psInner => psInner.ID == e.PhysicalStopId);
                if (ps != null)
                {
                    title.Append(ps.Name);
                    title.Append("|");
                    coordinates.Add(new Coord { Longitude = ps.Longitude, Latitude = ps.Latitude });
                }
            });
           
            string whereClass = "(Longitude={0} AND Latitude={1}) OR ";
            StringBuilder sb = new StringBuilder("Select * where ");
            foreach (Coord item in coordinates)
            {
                sb.AppendFormat(whereClass, item.Longitude, item.Latitude);
            }
            String strQuery = sb.ToString().Substring(0, sb.ToString().Length - 4);
            transCadFetchData.RunExportMacro("eZ", ExportConfigurator.GetConfig().EgedNispah2LoadMapMacroFile, new List<object> { ExportConfigurator.GetConfig().PhisicalStopsLayerName, strQuery });
        }

        public void ZoomToPhysicalStopsLayerFeutureById(List<BLEntities.Entities.PhysicalStop> entityList)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// SetLabelRouteLines
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="fieldName"></param>
        public void SetLabelRouteLines(bool isVisible, string fieldName)
        {
            transCadFetchData.SetLabelOnMap(ExportConfigurator.GetConfig().EgedRouteSystemLayerName,
                isVisible, fieldName, true, string.Empty);
        }
        /// <summary>
        /// SetLabelRouteStops
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="isByName"></param>
        /// <param name="selectedImageStopLabelIndex"></param>
        public void SetLabelRouteStops(bool isVisible, bool isByName, int selectedImageStopLabelIndex)
        {
            string busImage = string.Format("BUS{0}.BMP", selectedImageStopLabelIndex);
            string imageBus = Path.Combine(ConfigurationExportHelper.GetAppSettings("imagePath"), busImage);
            if (!IoHelper.IsFileExists(imageBus))
                imageBus = string.Empty;
            transCadFetchData.SetLabelOnMap(ExportConfigurator.GetConfig().PhisicalStopsLayerName,
                isVisible, isByName ?
                "CorrectPhStopName" : "ID_SEKER", true, imageBus);
        }
        /// <summary>
        /// ShowStreetName
        /// </summary>
        /// <param name="isVisible"></param>
        public void ShowStreetName(bool isVisible)
        {
            transCadFetchData.ShowStreetName("STREET", isVisible);
        }
        /// <summary>
        /// ShowStreetFlow
        /// </summary>
        /// <param name="isVisible"></param>
        public void ShowStreetFlow(bool isVisible)
        {
            transCadFetchData.ShowStreetFlow(isVisible);
        }
        /// <summary>
        /// InitMapResize
        /// </summary>
        public void InitMapResize()
        {
            transCadFetchData.InitMapResize(ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName);
        }
        /// <summary>
        /// GetMapTitle
        /// </summary>
        /// <returns></returns>
        public string GetMapTitle()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Map To Image
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveMapToImage(string fileName)
        {
            transCadFetchData.SaveMapToImage(fileName, "JPEG");
        }
        /// <summary>
        /// GetListRouteStopsByIDRouteLine
        /// </summary>
        /// <param name="rl"></param>
        /// <returns></returns>
        public List<RouteStop> GetListRouteStopsByIDRouteLine(RouteLine rl)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// VerifyRouteSystem
        /// </summary>
        /// <returns></returns>
        public string VerifyRouteSystem()
        {
            throw new NotImplementedException();
        }

       

        /// <summary>
        /// IsValidTransCadEnvironment
        /// </summary>
        /// <param name="errorString"></param>
        /// <returns></returns>
        public bool IsValidTransCadEnvironment(ref string errorString)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ValidateData
        /// </summary>
        public void ValidateData(List<RouteLine> selectedLines)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ValidateRouteLineData
        /// </summary>
        public void ValidateRouteLineData(List<RouteLine> selectedLines)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ValidatePhysicalStopsData
        /// </summary>
        public void ValidatePhysicalStopsData()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ValidateRouteStopData
        /// </summary>
        public void ValidateRouteStopData(List<RouteLine> selectedLines)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// IsValidList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public bool IsValidList<T>(List<T> lst) where T : BLEntities.Entities.BaseClass
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// IsValidDataModel
        /// </summary>
        public bool IsValidDataModel
        {
            get { return true; }
        }
        /// <summary>
        /// IsExportBaseEnabled
        /// </summary>
        public bool IsExportBaseEnabled
        {
            get { return false; }
        }
        /// <summary>
        /// IsAdminFormEnabled
        /// </summary>
        public bool IsAdminFormEnabled
        {
            get { return true; }
        }
        /// <summary>
        /// IsDanLikeOperators
        /// </summary>
        public bool IsDanLikeOperators
        {
            get { return false; }
        }

        public List<RouteLine> LinesNotBelongToOperator(List<RouteLine> lines, Operator oper)
        {
            return BLSharedUtils.LinesNotBelongToOperator(lines, oper);
        }

        /// <summary>
        /// FillTranscadMetaData
        /// </summary>
        /// <param name="data"></param>
        public void FillTranscadMetaData(BLEntities.Entities.ModelMetaData data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// InValidListObjectCount
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public int InValidListObjectCount<T>(List<T> lst) where T : BLEntities.Entities.BaseClass
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// SetUserOperator
        /// </summary>
        /// <param name="user"></param>
        /// <param name="opearatorName"></param>
        public void SetUserOperator(LoginUser user, string opearatorName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NewListObjectCount
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public int NewListObjectCount<T>(List<T> lst) where T : BLEntities.Entities.BaseClass
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// SetRouteStopsOrdinalNumber
        /// </summary>
        public void SetRouteStopsOrdinalNumber()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///SetHoradaInSpecificRouteLine 
        /// </summary>
        /// <param name="routeLine"></param>
        public void SetHoradaInSpecificRouteLine(BLEntities.Entities.RouteLine routeLine)
        {
            throw new NotImplementedException();
        }

        public bool IsExistsHoradaStationInLine(RouteLine routeLine)
        {
            return false;
        }

        /// <summary>
        /// SetRouteStopsDataInSpecificRouteLine
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="stationOrderConstofChangedRouteStop"></param>
        public void SetRouteStopsDataInSpecificRouteLine(RouteLine routeLine, StationOrderConst stationOrderConstofChangedRouteStop)
        {
            //if (routeLine != null)
            //{

            //    List<RouteStop> list = (from p in GlobalData.RouteModel.RouteStopList
            //                            where p.Route_ID == routeLine.ID
            //                            orderby p.Ordinal
            //                            select p).ToList<RouteStop>();
            //    int number = 1;
            //    list.ForEach(delegate(RouteStop rs)
            //    {
            //        if (number == 1)
            //        {
            //            rs.IdStationType = 1;
            //            if (stationOrderConstofChangedRouteStop == StationOrderConst.First)
            //            {
            //                SaveRouteStop(rs);
            //            }

            //        }
            //        else if (number == list.Count)
            //        {
            //            rs.IdStationType = 2;
            //            if (stationOrderConstofChangedRouteStop == StationOrderConst.Last)
            //            {
            //                SaveRouteStop(rs);
            //            }
            //        }
            //        BLSharedUtils.SetRouteStopStationType(rs);
            //        number++;
            //    });
            //}
            
        }
        /// <summary>
        /// ValidListObjectCount
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public int ValidListObjectCount<T>(List<T> lst) where T : BLEntities.Entities.BaseClass
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetInValidEntities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public List<T> GetInValidEntities<T>(List<T> lst) where T : BLEntities.Entities.BaseClass
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ConvertCatalogsTo7Positions
        /// </summary>
        public void ConvertCatalogsTo7Positions()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// UpdateStationCatalogHorada
        /// </summary>
        /// <param name="routeID"></param>
        public void UpdateStationCatalogHorada(int routeID)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// DeleteRouteLine
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool DeleteRouteLine(BLEntities.Entities.RouteLine routeLine)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// DeleteRouteStop
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public bool DeleteRouteStop(BLEntities.Entities.RouteStop routeStop)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetRouteLineLayerFields
        /// </summary>
        /// <returns></returns>
        public List<string> GetRouteLineLayerFields()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetRouteStopsLayerFields
        /// </summary>
        /// <returns></returns>
        public List<string> GetRouteStopsLayerFields()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ReCreateTableStructure
        /// </summary>
        public void ReCreateTableStructure()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ShowRecreatedRouteLineReport
        /// </summary>
        public void ShowRecreatedRouteLineReport()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// RouteLineWasRecreated
        /// </summary>
        public event EventHandler<RecreatedArgs> RouteLineWasRecreated;
        /// <summary>
        /// SaveRouteLine
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool SaveRouteLine(RouteLine routeLine)
        {
            return (egedSQLServerDAL.SaveRouteLine(routeLine));
        }
        /// <summary>
        /// SaveRouteStop
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public bool SaveRouteStop(RouteStop routeStop)
        {
            if (routeStop == null || routeStop.RouteLine == null)
                return false;

            if (BLSharedUtils.IsLoweringStation(routeStop))
                routeStop.Horada = 0;
            if (egedSQLServerDAL.SaveRouteStop(routeStop))
            {
                BLSharedUtils.SetRouteStopStationType(routeStop);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// SetOffSet
        /// </summary>
        /// <param name="lyr_set_name"></param>
        /// <param name="isRouteLineLayer"></param>
        /// <param name="offset"></param>
        public void SetOffSet(string lyr_set_name, bool isRouteLineLayer, double? offset)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// IsLayerConnectedToRS
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public bool IsLayerConnectedToRS(string layerName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// SetBaseRouteLines
        /// </summary>
        public void SetBaseRouteLines()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// BuildExportCatalog
        /// </summary>
        /// <param name="rl"></param>
        /// <returns></returns>
        public string BuildExportCatalog(BLEntities.Entities.RouteLine rl)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetStopCity
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public string GetStopCity(BLEntities.Entities.RouteStop routeStop)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// CalculateRouteLineStopsDuration
        /// </summary>
        /// <param name="routeLine"></param>
        public void CalculateRouteLineStopsDuration(RouteLine routeLine)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// RunExportMacro
        /// </summary>
        /// <param name="whereClass"></param>
        /// <param name="routeLineList"></param>
        public void RunExportMacro(string whereClass, List<RouteLine> routeLineList)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// RePopulateRouteStopList
        /// </summary>
        public void RePopulateRouteStopList()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetShapeFileFolder
        /// </summary>
        public string GetShapeFileFolder
        {
            get
            {

                return Path.Combine(ExportConfigurator.GetConfig().ShapeFileFolder, GlobalData.LoginUser.UserOperator.IdOperator.ToString()); ;
            }
        }

        /// <summary>
        /// IsEgedOperator
        /// </summary>
        public bool IsEgedOperator
        {
            get { return true; }
        }
        // TODO:
        public List<RouteLine> GetRouteLineListWithNotValidExportDate(List<RouteLine> sourse)
        {
            return new List<RouteLine>();
        }

        public void CloseConnection()
        {
            
        }

        #endregion

        #region ITransCadBLManager Members


        public void BuildModelData(List<RouteLine> routeLines)
        {
            BuildModelData();
        }

        #endregion
        public string GetSelectedClusterName()
        {
            return Resources.BLManagerResource.All;
        }
    }
}
