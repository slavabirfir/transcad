using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Model;
using BLEntities.Entities;
using DAL;
using IDAL;
using Utilities;
using BLEntities.Accessories;
using ExportConfiguration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Diagnostics;
using BLManager.Resources;

namespace BLManager
{
    public class TransCadBlManager : ITransCadBLManager
    {
        #region Const
        public const string RecreatedRouteLineTextFileName = "RecreatedRouteLineTextFileName.txt";
        public const string notepad = "notepad.exe";
        public const string SEPARATOR = ";";
        public const string Clusters = "Clusters"; 
        #endregion
        #region Private
        /// <summary>
        /// ITransCadFetchDataDAL
        /// </summary>
        private readonly ITransCadMunipulationDataDAL _transCadFetchData;
        /// <summary>
        /// IInternalBaseDAL
        /// </summary>
        private readonly IInternalBaseDal _internalBaseDal;
        private readonly IInternalBaseDal _internalSqlBaseDal;


        #endregion
        /// <summary>
        /// TransCadLoadBLManager
        /// </summary>
        public TransCadBlManager()
        {
            _transCadFetchData = new TransCadMunipulationDataDAL();
            _internalBaseDal = new InternalTvunaImplementationDal();
            _internalSqlBaseDal = new TransportLicensingDal();
        }

        #region ITransCadLoad Members
        /// <summary>
        /// Init Map Resize
        /// </summary>
        public void InitMapResize()
        {
            _transCadFetchData.InitMapResize(ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName);
        }

       /// <summary>
        /// VerifyRouteSystem
       /// </summary>
       /// <returns></returns>
        public string VerifyRouteSystem()
        {
            var fileName = GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PathToRSTFile;
            const string verificationLevel = "Connected";
           return _transCadFetchData.VerifyRouteSystem(fileName, verificationLevel);

        }

        /// <summary>
        /// Set Map Units
        /// </summary>
        public void SetCurrentMapUnits()
        {
            if (GlobalData.LoginUser != null && GlobalData.LoginUser.UserOperator != null)
            {
                string fileName = GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PathToRSTFile;
                fileName = fileName.Replace(" ", string.Empty);
                GlobalData.MapUnits = _transCadFetchData.GetMapUnits(fileName);
            }
        }

        /// <summary>
        /// Is Valid TransCad Environment
        /// </summary>
        /// <param name="errorString"></param>
        /// <returns></returns>
        public bool IsValidTransCadEnvironment(ref string errorString)
        {
            var sb = new StringBuilder();
            GlobalData.TransCadCurrentEnvoromnetInfo = _transCadFetchData.GetTransCadCurrentEnvoromnetInfo();

            if (GlobalData.TransCadCurrentEnvoromnetInfo.RouteSystemFile.IndexOf(GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PathToRSTFile) == -1)
                sb.AppendLine(string.Format(Resources.BLManagerResource.RouteSystemFileWrong, GlobalData.TransCadCurrentEnvoromnetInfo.RouteSystemFile));
            // Layres RouteStopsLayerName PhisicalStopsLayerName RouteSystemLayerName
            if (!GlobalData.TransCadCurrentEnvoromnetInfo.LayersName.Contains(ExportConfigurator.GetConfig().RouteStopsLayerName)
                ||
                !GlobalData.TransCadCurrentEnvoromnetInfo.LayersName.Contains(ExportConfigurator.GetConfig().PhisicalStopsLayerName)
                ||
                !GlobalData.TransCadCurrentEnvoromnetInfo.LayersName.Contains(ExportConfigurator.GetConfig().RouteSystemLayerName))
                sb.AppendLine(Resources.BLManagerResource.NotAllLayersExists);
            if (GlobalData.LoginUser.IsSuperViser)
                return true;
            errorString = sb.ToString();
            return string.IsNullOrEmpty(errorString);
        }
        /// <summary>
        /// CheckRouteStopIntegrity
        /// </summary>
        /// <param name="results"></param>
        /// <param name="baseClass"></param>
        public void CheckRouteStopIntegrity(ValidationResults results, BaseClass baseClass)
        {
            var rs = baseClass as RouteStop;
            if (rs == null) return;
            if (BLSharedUtils.IsCentralBusStation(rs) && (string.IsNullOrEmpty(rs.Platform)))
                results.AddResult(new ValidationResult(BLManagerResource.CentralStationFloorAndPlatformNotDefined, rs, "Platform Not Defined", null, null));

            if (BLSharedUtils.IsCentralBusStation(rs) && (!string.IsNullOrEmpty(rs.Platform)) && rs.Platform.IsNumber())
            {
                var platform = Convert.ToInt32(rs.Platform);
                if ((rs.PhysicalStop != null && rs.PhysicalStop.FromPlatform.HasValue &&
                    platform < rs.PhysicalStop.FromPlatform.Value) || 
                    (rs.PhysicalStop != null && rs.PhysicalStop.ToPlatform.HasValue &&
                    platform > rs.PhysicalStop.ToPlatform.Value))
                results.AddResult(new ValidationResult(BLManagerResource.CentralStationPlatformNotDefined, rs, "Platform is not defined for the station", null, null));
            }

            if (!BLSharedUtils.IsLoweringStation(rs) && (rs.Horada == null || rs.Horada == 0))
                results.AddResult(new ValidationResult(BLManagerResource.Horada_0, rs, "Lowering Station was not defined", null, null));
        }

        /// <summary>
        /// Check Route Line Integrity
        /// </summary>
        /// <param name="results"></param>
        /// <param name="baseClass"></param>
        public void CheckRouteLineIntegrity(ValidationResults results, BaseClass baseClass)
        {
            var rl = baseClass as RouteLine;
            // Is Base
            if (rl == null) return;
            if (!rl.IsBase)
            {
                var lst = GlobalData.RouteModel.RouteLineList.FindAll(routeLine =>
                                                                      routeLine.IdCluster == rl.IdCluster &&
                                                                      routeLine.IsBase && rl.IdCluster == routeLine.IdCluster
                                                                      && rl.Catalog == routeLine.Catalog && rl.Dir == routeLine.Dir);
                if (!(lst.IsListFull() && lst.Count == 1))
                {
                    var result = new ValidationResult(BLManagerResource.IsBaseWasNotDefined, rl, "IsBaseWasNotDefined", null, null);
                    results.AddResult(result);
                }
            }
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                //IdCluster + Catalog + SignPost +  Var + Dir 
                int sameLinesCount = (from p in GlobalData.RouteModel.RouteLineList
                                      where p.IdCluster == rl.IdCluster &&
                                            p.Catalog == rl.Catalog &&
                                            //p.Signpost == rl.Signpost &&
                                            p.Var == rl.Var &&
                                            p.Dir == rl.Dir
                                      select p).Count();
                if (sameLinesCount > 1)
                {
                    var result = new ValidationResult
                        (BLManagerResource.CombinationofIdCluster_Catalog_SignPost_Var_Dir_MustBeUnique, rl, "Catalog_SignPost_Var_Dir_MustBeUnique", null, null);
                    results.AddResult(result);
                }

                var rsList = (from p in GlobalData.RouteModel.RouteStopList
                              where p.RouteId == rl.ID
                              orderby p.Milepost
                              select p).ToList();
                if (!rl.IsNewEntity)
                {
                    if (rsList.Count == 0)
                    {
                        var result = new ValidationResult
                            (BLManagerResource.RouteStopsLessThen2, rl, "RouteStopsLessThen2", null, null);
                        results.AddResult(result);
                    }
                    else
                    {
                        if (rsList.Count > 0)
                        {
                            if (rsList[0].IdStationType != 1) // Embarkation  
                            {
                                var result = new ValidationResult
                                    (BLManagerResource.RouteStopsFirstStationNotEmbarkation, rl, "RouteStopsFirstStationNotEmbarkation", null, null);
                                results.AddResult(result);
                            }
                            if (rsList[rsList.Count - 1].IdStationType != 2) // Landing  
                            {
                                var result = new ValidationResult
                                    (BLManagerResource.RouteStopsLastStationNotLanding, rl, "RouteStopsLastStationNotLanding", null, null);
                                results.AddResult(result);
                            }
                            for (int i = 0; i < rsList.Count; i++)
                            {
                                if (i <= 0) continue;
                                if (rsList[i - 1].Milepost == rsList[i].Milepost)
                                {
                                    var result = new ValidationResult
                                        (string.Format(BLManagerResource.RouteStopsLastTwoStationEqualDistance, rsList[i - 1].Ordinal, rsList[i].Ordinal), rl, "RouteStopsLastTwoStationEqualDistance", null, null);
                                    results.AddResult(result);
                                }
                            }
                            string resultString = string.Empty;
                            int limit = ExportConfigurator.GetConfig().MinLimitDistanceBetweanStations;
                            if (limit > 0 && IsDistanceIsLessThenLimit(rsList, limit, ref resultString))
                            {
                                var result = new ValidationResult
                                    (string.Format(BLManagerResource.DistanceBetweenTwoStationLessThen, resultString, limit), rl, "DistanceBetweenTwoStationLessThen", null, null);
                                results.AddResult(result);

                            }
                            // finding same station into the line for non Taxi operators
                            if (rl.IdExclusivityLine != 6 && !BLSharedUtils.IsTaxiOperator()) // Season => not for calling in service
                            {
                                var duplicatedStationInPath =
                                    (from line in GlobalData.RouteModel.RouteLineList
                                     where line.ID != rl.ID && line.StopPresentList == rl.StopPresentList && rl.StopPresentList != null
                                     select line).ToList();

                                if (duplicatedStationInPath.IsListFull())
                                {
                                    var rlInnerName = duplicatedStationInPath[0].Name;
                                    var result = new ValidationResult
                                        (string.Format(BLManagerResource.DuplicatedStationInPath, rl.Name, rlInnerName), rl, "DuplicatedStationInPath", null, null);
                                    results.AddResult(result);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <returns></returns>
        private static String GetRouteStopIDs(List<RouteStop> rsList)
        {
            var strBuilder = new StringBuilder();
            StringBuilder strBuilderErros = null;
            if (rsList.IsListFull())
            {
                var orderedList = rsList.OrderBy(o => o.Milepost).ToList();
                orderedList.ForEach(rs =>
                {
                    if (rs.PhysicalStop == null)
                    {
                        if (strBuilderErros == null)
                            strBuilderErros = new StringBuilder();
                        strBuilderErros.Append(string.Format("The route stop {0} doesn't have it PhysicalStop for Operator {1} and RouteLine ID {2}", rs.ID, GlobalData.LoginUser.UserOperator.IdOperator, rs.RouteId));
                        strBuilderErros.Append(Environment.NewLine);
                    }
                    else
                    {
                        strBuilder.Append(rs.PhysicalStop.StationCatalog);
                        strBuilder.Append(ExportConfigurator.GetConfig().LogicalDataSplitDevider);
                    }
                }
                    );
            }
            if (strBuilderErros != null)
                throw new ApplicationException(strBuilderErros.ToString());
            return strBuilder.ToString();
        }


        /// <summary>
        /// Is Distance Is Less Then Limit. If yes returns list of station
        /// </summary>
        /// <param name="rsList"></param>
        /// <returns></returns>
        private bool IsDistanceIsLessThenLimit(List<RouteStop> rsList, int limit, ref string errrorString)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false;
            if (rsList.IsListFull())
            {
                for (int i = 0; i < rsList.Count; i++)
                {
                    if (i > 0)
                    {
                        double multi = 1;
                        if (GlobalData.MapUnits == GlobalConst.Miles)
                        {
                            multi = GlobalConst.MileToKilometerCoeffitient;
                        }
                        if ((rsList[i].Milepost - rsList[i - 1].Milepost) * multi * 1000 < limit)
                        {
                            flag = true;
                            sb.Append(rsList[i].Ordinal);
                            sb.Append(ExportConfigurator.GetConfig().LogicalDataSplitDevider);
                            sb.Append(rsList[i - 1].Ordinal);
                            sb.Append(" ");
                        }
                    }
                }
            }
            errrorString = sb.ToString();
            return flag;
        }

        /// <summary>
        /// Set Route Line Additional Data
        /// </summary>
        /// <param name="routeLine"></param>
        private void SetRouteLineAdditionalData(RouteLine routeLine)
        {
            if (routeLine != null)
            {
                routeLine.ValidationFunction = new ValidationFunctionDelegate(CheckRouteLineIntegrity);
                // TODO should be discussed
                if (routeLine.IdOperator == 0)
                    routeLine.IdOperator = GlobalData.LoginUser.UserOperator.IdOperator;

                routeLine.RouteNumber = routeLine.RouteNumber == 0 ? null : routeLine.RouteNumber;
                routeLine.Catalog = routeLine.Catalog == 0 ? null : routeLine.Catalog;
                if (routeLine.Dir < 1 || routeLine.Dir > 3)
                {
                    routeLine.Dir = null;
                }
                routeLine.Company = GlobalData.LoginUser.UserOperator.OperatorName;
                //SetHoradaInSpecificRouteLine(routeLine);
            }
        }

        /// <summary>
        /// Is Start Up Done
        /// </summary>
        public bool IsStartUpDone { get; set; }



        /// <summary>
        /// Replace Empty Horada Route Stops
        /// </summary>
        /// <param name="routeId"></param>
        private void ReplaceEmptyHoradaRouteStops(int routeId)
        {
            var rsOfRouteId = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == routeId);
            if (rsOfRouteId.IsListFull())
            {
                var routeLine = GlobalData.RouteModel.RouteLineList.FindLast(rl => rl.ID == routeId);
                BLSharedUtils.GetListRouteStopsByIdRouteLine(routeLine, this.IsEgedOperator);
                var horadaEmptyList = GlobalData.RouteModel.RouteStopList.FindAll(rs =>
                      rs.RouteId == routeId &&
                      (!rs.Horada.HasValue || rs.Horada.Value == 0 || rs.Horada.Value > rsOfRouteId.Count));
                if (horadaEmptyList.IsListFull())
                {
                    horadaEmptyList.ForEach(rsEmpty =>
                    {
                        var innerRsList =
                            GlobalData.RouteModel.RouteStopList.FindAll(
                            rsIterator => rsIterator.Milepost > rsEmpty.Milepost &&
                            rsIterator.RouteId == routeId &&
                            (rsIterator.IdStationType >= 2)).OrderBy(s => s.Milepost).ToList<RouteStop>();
                        if (innerRsList.IsListFull())
                        {
                            rsEmpty.Horada = innerRsList[0].Ordinal;
                            SaveRouteStop(rsEmpty);
                        }
                    });
                }
            }
        }


        public List<RouteLine> LinesNotBelongToOperator(List<RouteLine> lines, Operator oper)
        {
            return BLSharedUtils.LinesNotBelongToOperator(lines, oper);
        }

        /// <summary>
        /// FillTranscadMetaData
        /// </summary>
        /// <param name="metaData"></param>
        public void FillTranscadMetaData(ModelMetaData metaData)
        {
            Dictionary<string, string> dataDic = _transCadFetchData.GetTableStructure((ExportConfigurator.GetConfig().RouteSystemLayerName));
            metaData.TranscadMetaData.Add(ExportConfigurator.GetConfig().RouteSystemLayerName, dataDic);
            dataDic = _transCadFetchData.GetTableStructure((ExportConfigurator.GetConfig().RouteStopsLayerName));
            metaData.TranscadMetaData.Add(ExportConfigurator.GetConfig().RouteStopsLayerName, dataDic);
            dataDic = _transCadFetchData.GetTableStructure((ExportConfigurator.GetConfig().PhisicalStopsLayerName));
            metaData.TranscadMetaData.Add(ExportConfigurator.GetConfig().PhisicalStopsLayerName, dataDic);
        }

        public string GetSelectedClusterName()
        {
            if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig==null)
                return BLManagerResource.All;
            if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.ClusterId ==-1)
                return BLManagerResource.All;
            var clustres = _internalBaseDal.GetClustersByOpearatorId(GlobalData.LoginUser.UserOperator.IdOperator);
            var cluster = clustres.SingleOrDefault(e => e.ID == GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.ClusterId);
            if (cluster != null)
                return cluster.Name;
            return string.Empty; 

        }

        /// <summary>
        /// Build Model Data
        /// </summary>
        public void BuildModelData(List<RouteLine> selectedList)
        {
            SetCurrentMapUnits();
            var custerToZoneManagerBl = new ClusterToZoneManagerBL();
            custerToZoneManagerBl.SetCusterToZoneManagerBL();
            GlobalData.Directions = _internalBaseDal.GetDirectionList();
            if (IsDanLikeOperators)
            {
                GlobalData.InitMetaData();
                FillTranscadMetaData(GlobalData.MetaData);
                IXMLMetaDataLoader xmlMetaDataLoader = new XMLMetaDataLoader();
                xmlMetaDataLoader.FillXMLMetaData(GlobalData.MetaData);
                string errorMessage = Environment.NewLine;
                if (!xmlMetaDataLoader.CompareModelMetaDataXMLAndTranscad(GlobalData.MetaData, ref errorMessage))
                {
                    throw new ApplicationException(String.Concat(Resources.BLManagerResource.DanOperMetaDataNotConfirmed, errorMessage));
                }
                _transCadFetchData.ReloadRouteSystem(GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PathToRSTFile);
            }
           
            GlobalData.BaseTableEntityDictionary = _internalBaseDal.GetBaseTableEntityList(GlobalData.LoginUser.UserOperator.IdOperator);
            // FillBaseTableTranslatorList
            GlobalData.BaseTableTranslatorList = _internalBaseDal.FillBaseTableTranslatorList();
            // Set TransCad Data
            Dictionary<int, PhysicalStop> dicPhysicalStop = _transCadFetchData.GetPhysicalStopsRawDataDictionary();
            if (!dicPhysicalStop.IsDictionaryFull())
            {
                throw new ApplicationException(BLManagerResource.TranscadPhysicalDBListEmpty);
            }
            GlobalData.RouteModel.PhysicalStopList = dicPhysicalStop.Values.ToList();
            
            Dictionary<int, List<RouteStop>> dicrouteStop = _transCadFetchData.GetRouteStopsRawDataDictionary();
            if (!dicrouteStop.IsDictionaryFull())
            {
                throw new ApplicationException(BLManagerResource.TranscadNispah3DBListEmpty);
            }

            GlobalData.RouteModel.RouteStopList = new List<RouteStop>();
            foreach (int routeId in dicrouteStop.Keys)
            {
                GlobalData.RouteModel.RouteStopList.AddRange(dicrouteStop[routeId]);
            }
           
            Dictionary<int, RouteLine> dicRouteLineList = _transCadFetchData.GetRouteLinesRawDataDictionary();
            if (!dicRouteLineList.IsDictionaryFull())
            {
                throw new ApplicationException(BLManagerResource.EgedLinelDBListEmpty);
            }
            GlobalData.RouteModel.RouteLineList = dicRouteLineList.Values.ToList();
            //// Set read only PriceArea
            //transCadFetchData.SetReadOnlyFieldSet(ExportConfiguration.ExportConfigurator.GetConfig().PriceAreaPolygonName, transCadFetchData.PriceAreaReadOnlyFields, true);
            BLSharedUtils.BuildCatalogInfoList();

            // Set Route and Phisical Stop Objects of RouteStopList
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                //GlobalData.RouteModel.RouteLineList.ForEach(it => it.Route_Len = transCadFetchData.CalculateRouteLine(it,ExportConfigurator.GetConfig().RouteSystemLayerName));  
                GlobalData.RouteModel.RouteLineList.ForEach(SetRouteLineAdditionalData);
                GlobalData.RouteModel.RouteStopList.ForEach(
                    delegate(RouteStop routeStop)
                    {
                        //routeStop.Milepost = routeStop.Milepost * GlobalConst.DistanceConverter; 
                        routeStop.ValidationFunction = new ValidationFunctionDelegate(CheckRouteStopIntegrity);
                        routeStop.PhysicalStop = dicPhysicalStop.ContainsKey(routeStop.PhysicalStopId) ? dicPhysicalStop[routeStop.PhysicalStopId] : null;
                        if (routeStop.PhysicalStop != null)
                        {
                            routeStop.StationCatalog = routeStop.PhysicalStop.StationCatalog;
                            routeStop.Name = routeStop.StationCatalog;
                            routeStop.MilepostRounded = Convert.ToInt32(Math.Round(routeStop.Milepost * ExportConfigurator.GetConfig().LengthConverter * GlobalConst.MileToKilometerCoeffitient, 2)); //Convert.ToInt32(Math.Round(routeStop.Milepost, 2))  ;
                        }
                        BLSharedUtils.SetRouteStopStationType(routeStop);
                        routeStop.RouteLine = dicRouteLineList[routeStop.RouteId];
                    });

                SetNewRouteLines();
                //SetDuplicatedLines();
                SetRouteStopsOrdinalNumber();
                // GlobalData.RouteModel.RouteLineList.ForEach(it => SetRouteLineAdditionalData(it));
                GlobalData.RouteModel.RouteLineList.ForEach(it =>
                {
                    if (dicrouteStop.ContainsKey(it.ID))
                    {
                        var horadaList = dicrouteStop[it.ID].OrderBy(rs => rs.Milepost).ToList<RouteStop>(); //GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.Route_ID == it.ID).OrderBy(rs => rs.Milepost).ToList<RouteStop>();
                        BLSharedUtils.UpdateDropFirstStationData(horadaList);
                    }
                });
            }

            if (ExportConfigurator.GetConfig().RunValidationCheckOnStartUp || IsStartUpDone)
            {
                ValidateData(selectedList);
            }

            if (!IsStartUpDone)
            {
                IsStartUpDone = true;
            }
            //SetCurrentMapUnits();
            // Set Map offset
            SetOffSet(ExportConfigurator.GetConfig().RouteStopsLayerName, false, ExportConfigurator.GetConfig().StopsOffsetPoints);
            //SetOffSet(ExportConfigurator.GetConfig().RouteSystemLayerName, true , null);
            SetOffSet(ExportConfigurator.GetConfig().RouteSystemLayerName, true, 1);
            // sort the RouteStopList list
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                List<RouteStop> routeStopOrderedList = (from p in GlobalData.RouteModel.RouteStopList
                                                        orderby p.RouteId, p.Ordinal
                                                        select p).ToList<RouteStop>();
                GlobalData.RouteModel.RouteStopList = routeStopOrderedList;
            }
        }



        /// <summary>
        /// Set Horada In Specific Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// 


        public void SetHoradaInSpecificRouteLine(RouteLine routeLine)
        {
            if (routeLine != null)
            {
                List<RouteStop> list = (from p in GlobalData.RouteModel.RouteStopList
                                        where p.RouteId == routeLine.ID
                                        orderby p.Milepost
                                        select p).ToList<RouteStop>();
                list.ForEach(delegate(RouteStop rs)
                {
                    if (BLSharedUtils.IsLoweringStation(rs))
                    {
                        rs.Horada = 0;
                        SaveRouteStop(rs);
                    }
                    else
                    {
                        List<RouteStop> innerRSList =
                            list.FindAll(rsIterator => rsIterator.Milepost > rs.Milepost &&
                            (rsIterator.IdStationType == 2 || rsIterator.IdStationType == 3)).OrderBy(s => s.Milepost).ToList<RouteStop>();
                        if (innerRSList.IsListFull())
                        {
                            rs.Horada = innerRSList[0].Ordinal;
                            SaveRouteStop(rs);
                        }
                    }
                }
                );


            }
        }

        public bool IsExistsHoradaStationInLine(RouteLine routeLine)
        {
            if (routeLine != null)
            {
                var horadaStations = (from p in GlobalData.RouteModel.RouteStopList
                                      where p.RouteId == routeLine.ID && p.Horada != 0
                                      orderby p.Milepost
                                      select p);
                return horadaStations.Any();
                
            }
            return false;              
        }

        /// <summary>
        /// Set Route Stops Ordinal Number In Specific Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="stationOrderConstofChangedRouteStop"></param>
        public void SetRouteStopsDataInSpecificRouteLine(RouteLine routeLine, StationOrderConst stationOrderConstofChangedRouteStop)
        {
            if (routeLine != null)
            {

                List<RouteStop> list = (from p in GlobalData.RouteModel.RouteStopList
                                        where p.RouteId == routeLine.ID
                                        orderby p.Milepost
                                        select p).ToList<RouteStop>();
                int number = 1;
                list.ForEach(delegate(RouteStop rs)
                {
                    if (number == 1)
                    {
                        rs.IdStationType = 1;
                        if (stationOrderConstofChangedRouteStop == StationOrderConst.First)
                        {
                            SaveRouteStop(rs);
                        }

                    }
                    else if (number == list.Count)
                    {
                        rs.IdStationType = 2;
                        if (stationOrderConstofChangedRouteStop == StationOrderConst.Last)
                        {
                            SaveRouteStop(rs);
                        }
                    }
                    rs.Ordinal = number;
                    BLSharedUtils.SetRouteStopStationType(rs);
                    number++;
                });
                //SetHoradaInSpecificRouteLine(routeLine);
            }
        }

        /// <summary>
        /// Set Route Stops OrdinalNumber
        /// </summary>
        public void SetRouteStopsOrdinalNumber()
        {
            if (GlobalData.RouteModel.RouteStopList.IsListFull())
            {
                var groups = from p in GlobalData.RouteModel.RouteStopList
                             group p by p.RouteId into g
                             select g.Key;

                foreach (var item in groups)
                {
                    List<RouteStop> list = (from p in GlobalData.RouteModel.RouteStopList
                                            where p.RouteId == item
                                            orderby p.Milepost
                                            select p).ToList<RouteStop>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        RouteStop rs = list[i];
                        rs.Ordinal = i + 1;
                        if (i == 0 && rs.IdStationType != 1)
                        {
                            rs.IdStationType = 1;
                            SaveRouteStop(rs);
                        }
                        if (i == list.Count - 1 && rs.IdStationType != 2)
                        {
                            rs.IdStationType = 2;
                            SaveRouteStop(rs);
                        }

                    };


                }
            }
        }


        /// <summary>
        /// Set User Operator
        /// </summary>
        /// <param name="user"></param>
        /// <param name="opearatorName"></param>
        public void SetUserOperator(LoginUser user, string opearatorName)
        {
            if (user == null)
                return;
            Operator oper = GlobalData.OperatorList.Find(op => op.OperatorName == opearatorName);
            if (oper != null)
                user.UserOperator = oper;
        }
        /// <summary>
        /// Get List Operators By Active Directory Groups
        /// </summary>
        /// <param name="activeDirectories"></param>
        /// <returns></returns>
        private static List<Operator> GetListOperatorsByActiveDirectoryGroups(List<string> activeDirectories)
        {
            var opers = new List<Operator>();
            if (GlobalData.OperatorList.IsListFull() && activeDirectories.IsListFull ()) 
            {
                GlobalData.OperatorList.ForEach(oper =>
                    {
                        if (activeDirectories.Exists (op=>op.Equals(oper.OperatorName)))
                            opers.Add(oper);
                    });
            }
            return opers;
        }

        /// <summary>
        /// Get Login User Info
        /// </summary>
        /// <returns></returns>
        public bool SetLoginUserInfoAndVerifyToShowSelectOpersList()
        {
            GlobalData.OperatorList = _internalSqlBaseDal.GetOperatorList();
            //var dbOpers = new StringBuilder ();
            //dbOpers.AppendLine("--------------DB START----------------");
            //GlobalData.OperatorList.ForEach(g => dbOpers.AppendFormat("{0},", g.OperatorName));
            //dbOpers.AppendLine("--------------DB FINISH----------------");


            List<Operator> userActiveDirectoryOperatorsOprators;
            if (!ExportConfigurator.GetConfig().IsAuthenticationserviceActive)
            {
                userActiveDirectoryOperatorsOprators = GlobalData.OperatorList;
            }
            else
            {
                List<string> userActiveDirectoryGroups;
                if (Convert.ToBoolean(ExportConfigurator.GetConfig().DevelopmentEnv))
                {
                    var dh = new DomainUserHelper();
                    userActiveDirectoryGroups = dh.GetUserGroups();
                }
                else
                {
                    userActiveDirectoryGroups = new List<string>();
                    var ws = new RishuyAD.RishuyAD();
                    string[] groups = ws.GetGroupNameByUser(CurrentSettingsHelper.GetLoginUserName());
                    if (groups != null)
                        userActiveDirectoryGroups = groups.ToList();

                }
                userActiveDirectoryOperatorsOprators = GetListOperatorsByActiveDirectoryGroups(userActiveDirectoryGroups);
            }
            if (!userActiveDirectoryOperatorsOprators.IsListFull())
            {
                throw new ApplicationException(BLManagerResource.ActiveDirectoryGroupsNotDefinedInOperatorListXML);
            }
            //var groupsAd = new StringBuilder ();
            //groupsAd.AppendLine("--------------AD START----------------");
            //userActiveDirectoryOperatorsOprators.ForEach (g=>groupsAd.AppendFormat("{0},",g.OperatorName));
            //groupsAd.AppendLine("--------------AD FINISH----------------");

            var isShowOperatorChooseForm = userActiveDirectoryOperatorsOprators.Count > 1;

            if (userActiveDirectoryOperatorsOprators.Count==1)
            {
                var operatorSelectBlManager = new OperatorSelectBlManager(userActiveDirectoryOperatorsOprators[0]);
                isShowOperatorChooseForm = operatorSelectBlManager.SetSelectedTranscadClusterConfigAndTestClustersByOperatorIsListFull(userActiveDirectoryOperatorsOprators[0]);
            }

            var oper = !isShowOperatorChooseForm ? userActiveDirectoryOperatorsOprators[0] : new Operator();
            
            GlobalData.LoginUser = new LoginUser
            {
                UserName = CurrentSettingsHelper.GetLoginUserName(),
                UserOperator = oper,
                IsSuperViser = !ExportConfigurator.GetConfig().IsAuthenticationserviceActive,
                ActiveDirectotyOperatorList = userActiveDirectoryOperatorsOprators
            };
            // Encrypt Config file
            AppConfigProtector.EncryptConfig();
            return isShowOperatorChooseForm;
        }
        /// <summary>
        /// GetCurrentUserGroup
        /// </summary>
        /// <returns></returns>
        private EnmOperatorGroup GetCurrentUserGroup(string userGroupName)
        {
          EnmOperatorGroup currentUserGroup = EnmOperatorGroup.Operator;
          if ( GlobalData.OperatorList.IsListFull() && ExportConfigurator.GetConfig().IsAuthenticationserviceActive)
          {
              Operator oper = GlobalData.OperatorList.FindLast(op => op.OperatorName.Equals(userGroupName));
              if (oper!=null)
              {
                  currentUserGroup = oper.OperatorGroup;
              }

          }
          return currentUserGroup;    
        }

        /// <summary>
        /// Validate Route Line Data
        /// </summary>
        public void ValidateRouteLineData(List<RouteLine> selectedRoutes)
        {
            SetStopPresentList();
            if (GlobalData.RouteModel == null) return;
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                if (selectedRoutes == null)
                {
                    GlobalData.RouteModel.RouteLineList.ForEach(rl => rl.Validate());
                }
                else
                {
                    GlobalData.RouteModel.RouteLineList.ForEach(rl =>
                                                                    {
                                                                        if (selectedRoutes.Exists(re => re.ID == rl.ID))
                                                                            rl.Validate();
                                                                    });
                }
            }
        }
        /// <summary>
        /// SetStopPresentList
        /// </summary>
        private static void SetStopPresentList()
        {
            if (GlobalData.RouteModel != null)
            {
                if (GlobalData.RouteModel.RouteLineList.IsListFull() && GlobalData.RouteModel.RouteStopList.IsListFull())
                {
                    GlobalData.RouteModel.RouteLineList.ForEach(rl => rl.StopPresentList =
                        GetRouteStopIDs(GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == rl.ID)));
                }
            }

        }


        /// <summary>
        /// Validate Route Stop Data
        /// </summary>
        public void ValidateRouteStopData(List<RouteLine> selectedRoutes)
        {
            if (GlobalData.RouteModel != null)
            {
                if (GlobalData.RouteModel.RouteStopList.IsListFull())
                {
                    if (selectedRoutes == null)
                    {
                        GlobalData.RouteModel.RouteStopList.ForEach(rs => rs.Validate());
                    }
                    else
                    {
                       GlobalData.RouteModel.RouteStopList.ForEach(rs=>
                        {
                            if (selectedRoutes.Exists(re => re.ID == rs.RouteId))
                                rs.Validate();
                        });
                    }
                }
            }

        }


        /// <summary>
        /// Validate Data
        /// </summary>
        public void ValidateData(List<RouteLine> selectedRoutes)
        {
            if (ExportConfigurator.GetConfig().IsVerifyRS)
            {
                var errorRouteSystem = VerifyRouteSystem();
                if (!String.IsNullOrEmpty(errorRouteSystem))
                {
                    throw new ApplicationException(string.Format(Resources.BLManagerResource.MapsWithErrors, errorRouteSystem));
                }
            }
            ValidateRouteLineData(selectedRoutes);
            ValidateRouteStopData(selectedRoutes);

        }

        /// <summary>
        /// Is Valid List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public bool IsValidList<T>(List<T> lst) where T : BaseClass
        {
            return lst.IsListFull() && lst.All(baseEntity => baseEntity.ValidationErrors.IsListFull() == false);
        }
        /// <summary>
        /// InValid List Object Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public int InValidListObjectCount<T>(List<T> lst) where T : BaseClass
        {
            return lst.Count(baseEntity => baseEntity.ValidationErrors != null);
        }

        /// <summary>
        /// Is Valid DataModel
        /// </summary>
        public bool IsValidDataModel
        {
            get
            {
                return IsValidList<RouteLine>(GlobalData.RouteModel.RouteLineList) &&
                       IsValidList<RouteStop>(GlobalData.RouteModel.RouteStopList) &&
                       IsValidList<PhysicalStop>(GlobalData.RouteModel.PhysicalStopList);
            }

        }
        /// <summary>
        /// Is Export Base Enabled
        /// </summary>
        public bool IsExportBaseEnabled
        {
            get
            {
                return BLSharedUtils.IsTaxiOperator() && GlobalData.TransCadCurrentEnvoromnetInfo.IsMunLayerExists; //&& GlobalData.LoginUser.IsSuperViser;
            }
        }




        /// <summary>
        /// Get InValid Entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public List<T> GetInValidEntities<T>(List<T> lst) where T : BaseClass
        {
            return lst.FindAll(be => be.ValidationErrors.IsListFull());
        }


        /// <summary>
        /// Valid List Object Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public int ValidListObjectCount<T>(List<T> lst) where T : BaseClass
        {
            return lst.Count(baseEntity => baseEntity.ValidationErrors == null);
        }

        /// <summary>
        /// Zoom To Route Lines Layer Feuture By Id
        /// </summary>
        /// <param name="entityList"></param>
        public void ZoomToRouteLinesLayerFeutureById(List<RouteLine> entityList)
        {
            List<int> ids = new List<int>();
            StringBuilder title = new StringBuilder();
            entityList.ForEach(delegate(RouteLine e)
            {
                title.Append(e.Name);
                title.Append("|");
                ids.Add(e.ID);
            });
            _transCadFetchData.ZoomToLayerFeutureById(ExportConfigurator.GetConfig().RouteSystemLayerName, ids, title.ToString());
            string routeSystemLayerName = ExportConfiguration.ExportConfigurator.GetConfig().RouteSystemLayerName;
            _transCadFetchData.SetArrowTopology(routeSystemLayerName);
        }

        /// <summary>
        /// Zoom To Route Stops Layer Feuture By Id 
        /// </summary>
        /// <param name="entityList"></param>
        public void ZoomToRouteStopsLayerFeutureById(List<RouteStop> entityList)
        {
            List<int> ids = new List<int>();
            StringBuilder title = new StringBuilder();
            entityList.ForEach(delegate(RouteStop e)
            {
                PhysicalStop ps = GlobalData.RouteModel.PhysicalStopList.Find(psInner => psInner.ID == e.PhysicalStopId);
                title.Append(ps.Name);
                title.Append("|");
                ids.Add(ps.ID);
            });
            _transCadFetchData.ZoomToLayerFeutureById(ExportConfigurator.GetConfig().PhisicalStopsLayerName, ids, title.ToString());
        }
        /// <summary>
        /// Zoom To Physical Stops Layer Feuture By Id
        /// </summary>
        /// <param name="entityList"></param>
        public void ZoomToPhysicalStopsLayerFeutureById(List<PhysicalStop> entityList)
        {

        }
        /// <summary>
        /// Show Street Name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="isVisible"></param>
        public void ShowStreetName(bool isVisible)
        {
            _transCadFetchData.ShowStreetName("STREET", isVisible);
        }

        public void SaveMapToImage(string fileName)
        {
            _transCadFetchData.SaveMapToImage(fileName, "JPEG");
        }

        /// <summary>
        /// Show Street Flow
        /// </summary>
        /// <param name="isVisible"></param>
        public void ShowStreetFlow(bool isVisible)
        {
            _transCadFetchData.ShowStreetFlow(isVisible);
        }



        /// <summary>
        /// Set Init Value RouteLine
        /// </summary>
        /// <param name="rl"></param>
        public void SetInitValueRouteLine(RouteLine routeLine)
        {
            if (routeLine != null)
            {
                routeLine.Signpost = string.Empty;
                routeLine.Dir = null;
                routeLine.Var = string.Empty;
                routeLine.IdCluster = 0;
                routeLine.IdServiceType = 0;
                routeLine.Catalog = null;
                routeLine.IdSeasonal = 0;
                routeLine.AccountingGroupID = 0;
                routeLine.RoadDescription = string.Empty;
                routeLine.RouteNumber = null;
                routeLine.IdZoneHead = 0;
                routeLine.IdZoneSubHead = 0;
                routeLine.ServiceTypeName = string.Empty;
                routeLine.ClusterName = string.Empty;
            }
        }
        /// <summary>
        /// SetNewRouteLines
        /// </summary>
        private void SetNewRouteLines()
        {
            // Set Route and Phisical Stop Objects of RouteStopList
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                // New Lines
                GlobalData.RouteModel.RouteLineList.ForEach(delegate(RouteLine rl)
                {
                    if ((rl.Catalog == null || rl.Catalog == 0) &&
                                         (rl.RouteNumber == null || rl.RouteNumber == 0) &&
                                         (rl.IdCluster == 0) &&
                                         (string.IsNullOrEmpty(rl.Signpost)) && string.IsNullOrEmpty(rl.Var) &&
                                          (rl.Dir == null || rl.Dir == 0))
                    {
                        rl.IsNewEntity = true;
                        //SetRouteStopsDataInSpecificRouteLine(rl, StationOrderConst.First);
                        //SetRouteStopsDataInSpecificRouteLine(rl, StationOrderConst.Last);
                    }

                });

                // Duplicated Lines
                GlobalData.RouteModel.RouteLineList.ForEach(
                    delegate(RouteLine rl)
                    {
                        if (!rl.IsNewEntity)
                        {
                            rl.DisticntValue =
                            String.Concat(rl.IdCluster, rl.Catalog.ToString(), rl.Signpost, rl.Var,
                             rl.Dir.ToString(), rl.IdServiceType);
                        }
                    });

                var result = from rl in GlobalData.RouteModel.RouteLineList

                             group rl by rl.DisticntValue into g
                             where g.Count() > 1 && !string.IsNullOrEmpty(g.Key)
                             select new { DisticntValue = g.Key, Data = g };

                foreach (var item in result)
                {
                    List<RouteLine> duplicated = GlobalData.RouteModel.RouteLineList.FindAll(rl => rl.DisticntValue == item.DisticntValue);
                    if (duplicated != null)
                    {
                        duplicated = (from p in duplicated
                                      orderby p.ID
                                      select p).ToList<RouteLine>();
                        for (int i = 1; i < duplicated.Count; i++)
                        {
                            //SetInitValueRouteLine(duplicated[i]);
                            try
                            {
                                if (duplicated[i].Dir == null)
                                    duplicated[i].Dir = 0;
                                if (duplicated[i].RouteNumber == null)
                                    duplicated[i].RouteNumber = 0;
                                if (duplicated[i].Catalog == null)
                                    duplicated[i].Catalog = 0;
                                SaveRouteLine(duplicated[i]);
                                duplicated[i].IsNewEntity = true;
                                duplicated[i].IdDuplicatedSourceID = duplicated[0].ID;
                            }
                            catch
                            { }
                        }
                    }
                }
            }
        }





        /// <summary>
        /// Delete Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool DeleteRouteLine(RouteLine routeLine)
        {
            if (routeLine != null)
            {
                List<RouteStop> routeStopsList = null;
                if (GlobalData.RouteModel.RouteStopList.IsListFull())
                {
                    routeStopsList = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == routeLine.ID);
                    _transCadFetchData.DeleteRouteLine(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName);
                    foreach (RouteStop item in routeStopsList)
                    {
                        GlobalData.RouteModel.RouteStopList.Remove(item);
                    }
                }
                GlobalData.RouteModel.RouteLineList.Remove(routeLine);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Get Last Order Number In Route Stop In Route Line
        /// </summary>
        /// <param name="rl"></param>
        /// <returns></returns>
        private int GetLastOrderNumberInRouteStopInRouteLine(RouteLine rl)
        {
            if (rl == null)
            {
                return 0;
            }
            else
            {
                int last = (from p in GlobalData.RouteModel.RouteStopList
                            where p.RouteId == rl.ID
                            select p.Ordinal).ToList<int>().Max();
                return last;
            }
        }


        /// <summary>
        /// Delete Route Stop
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public bool DeleteRouteStop(RouteStop routeStop)
        {
            if (routeStop != null)
            {
                // Find station with deleted Horada and save the rs Horada as 0
                if (routeStop.Horada > 0)
                {
                    if (GlobalData.RouteModel.RouteStopList.IsListFull())
                    {
                        List<RouteStop> horadaList = GlobalData.RouteModel.RouteStopList.FindAll(rs =>
                             rs.RouteId == routeStop.RouteId && rs.Horada == routeStop.Ordinal);
                        if (horadaList.IsListFull())
                        {
                            horadaList.ForEach(rs =>
                            {
                                List<RouteStop> innerRSList =
                                    GlobalData.RouteModel.RouteStopList.FindAll(
                                    rsIterator => rsIterator.Milepost > rs.Milepost &&
                                    rsIterator.RouteId == routeStop.ID &&
                                    rsIterator.Ordinal >= routeStop.Horada &&
                                    (rsIterator.IdStationType >= 2)).OrderBy(s => s.Milepost).ToList<RouteStop>();
                                if (innerRSList.IsListFull())
                                {
                                    rs.Horada = innerRSList[0].Ordinal;
                                    SaveRouteStop(rs);
                                }
                            });
                        }
                    }
                }
                _transCadFetchData.DeleteRouteStop(routeStop, ExportConfigurator.GetConfig().RouteStopsLayerName);
                int lastOrdinal = GetLastOrderNumberInRouteStopInRouteLine(routeStop.RouteLine);
                StationOrderConst stationOrderConst = StationOrderConst.Regular;
                if (routeStop.Ordinal == 1)
                {
                    stationOrderConst = StationOrderConst.First;
                }
                if (routeStop.Ordinal == lastOrdinal)
                {
                    stationOrderConst = StationOrderConst.Last;
                }
                GlobalData.RouteModel.RouteStopList.Remove(routeStop);
                SetRouteStopsDataInSpecificRouteLine(routeStop.RouteLine, stationOrderConst);

                ReplaceEmptyHoradaRouteStops(routeStop.RouteId);

                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// New List Object Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public int NewListObjectCount<T>(List<T> lst) where T : BaseClass
        {
            if (lst.IsListFull())
                return lst.Count(en => en.IsNewEntity);
            else
                return 0;
        }

        /// <summary>
        /// Is Config Decripted Enabled
        /// </summary>
        public bool IsAdminFormEnabled
        {
            get
            {
                return GlobalData.LoginUser.IsSuperViser;
            }
        }

        #endregion
        #region TableStructure
        /// <summary>
        /// Get Route Line Layer Fields
        /// </summary>
        /// <returns></returns>
        public List<string> GetRouteLineLayerFields()
        {
            return _transCadFetchData.GetLayerFields(ExportConfigurator.GetConfig().RouteSystemLayerName);
        }
        /// <summary>
        /// Get Route Stops Layer Fields
        /// </summary>
        /// <returns></returns>
        public List<string> GetRouteStopsLayerFields()
        {
            return _transCadFetchData.GetLayerFields(ExportConfigurator.GetConfig().RouteStopsLayerName);
        }
        /// <summary>
        /// ReCreateTableStructure
        /// </summary>
        public void ReCreateTableStructure()
        {
            _transCadFetchData.RouteLineWasRecreated += TransCadFetchDataRouteLineWasRecreated;
            IoHelper.DeleteFile(GetRouteLineRecreationReportFileName());
            GlobalData.BaseTableTranslatorList = _internalBaseDal.FillBaseTableTranslatorList();
            if (!GlobalData.ClusterToZoneDictionary.IsDictionaryFull())
                GlobalData.ClusterToZoneDictionary = DalShared.FillClusterToZoneDictionary();
            _transCadFetchData.ReCreateTableStructure(ExportConfigurator.GetConfig().RouteSystemLayerName,
                ExportConfigurator.GetConfig().RouteStopsLayerName, ExportConfigurator.GetConfig().PhisicalStopsLayerName);
        }
        /// <summary>
        /// GetRouteLineRecreationReportFileName
        /// </summary>
        /// <returns></returns>
        private static string GetRouteLineRecreationReportFileName()
        {
            var fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(fileName, RecreatedRouteLineTextFileName);
        }
        /// <summary>
        /// Show Recreated Route Line Report
        /// </summary>
        public void ShowRecreatedRouteLineReport()
        {
            if (IoHelper.IsFileExists(GetRouteLineRecreationReportFileName()))
                ProcessLauncher.RunProcess("notepad.exe", GetRouteLineRecreationReportFileName());
        }

        /// <summary>
        /// transCad Fetch Data Route Line Was Recreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TransCadFetchDataRouteLineWasRecreated(object sender, RecreatedArgs e)
        {
            // Write to Text file
            IoHelper.AddLineToTextFile(GetRouteLineRecreationReportFileName(), e.ErrorString);
            // Wire Up event
            if (RouteLineWasRecreated != null)
            {
                RouteLineWasRecreated(null, e);
            }
        }
        /// <summary>
        /// Route Line Was Recreated event
        /// </summary>
        public event EventHandler<RecreatedArgs> RouteLineWasRecreated;
        /// <summary>
        /// Set Label Route Lines
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="fieldName"></param>
        public void SetLabelRouteLines(bool isVisible, string fieldName)
        {
            _transCadFetchData.SetLabelOnMap(ExportConfigurator.GetConfig().RouteSystemLayerName,
                isVisible, fieldName, true, string.Empty);
        }
        /// <summary>
        /// Set Label Route Stops
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
            _transCadFetchData.SetLabelOnMap(ExportConfigurator.GetConfig().PhisicalStopsLayerName,
                isVisible, isByName ?
                "CorrectPhStopName" : "ID_SEKER", true, imageBus);
        }

        /// <summary>
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool SaveRouteStop(RouteStop routeStop)
        {
            if (routeStop == null || routeStop.RouteLine == null)
                return false;
            //SetHoradaInSpecificRouteLine(routeStop.RouteLine); 
            if (BLSharedUtils.IsLoweringStation(routeStop))
                routeStop.Horada = 0;
            if (_transCadFetchData.SaveRouteStop(routeStop, ExportConfigurator.GetConfig().RouteStopsLayerName))
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
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool SaveRouteLine(RouteLine routeLine)
        {

            if (_transCadFetchData.SaveRouteLine(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName))
            {
                SetRouteLineAdditionalData(routeLine);
                routeLine.IsNewEntity = false;
                routeLine.IdDuplicatedSourceID = 0;
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Set Off Set
        /// </summary>
        /// <param name="lyr_set_name"></param>
        /// <param name="isRouteLineLayer"></param>
        /// <param name="offset"></param>
        public void SetOffSet(string lyr_set_name, bool isRouteLineLayer, double? offset)
        {
            string offset_type = null;
            if (offset > 0)
            {
                offset_type = isRouteLineLayer ? "Channel" : "Offset";
                offset = offset_type.Equals("Channel") ? null : offset;
            }
            _transCadFetchData.SetOffSet(String.Concat(lyr_set_name, "|"), offset_type, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetBaseRouteLines()
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                GlobalData.RouteModel.RouteLineList.ForEach
                    (
                      rl =>
                      {
                          if (!GlobalData.RouteModel.RouteLineList.Exists(en =>
                              en.IdCluster == rl.IdCluster &&
                              en.Catalog == rl.Catalog &&
                              en.Dir == rl.Dir && en.IsBase
                              ))
                          {
                              List<RouteLine> groupList = (from p in GlobalData.RouteModel.RouteLineList

                                                           where p.IdCluster == rl.IdCluster &&
                                                                 p.Catalog == rl.Catalog &&
                                                                 p.Dir == rl.Dir
                                                           orderby p.Var
                                                           select p).ToList<RouteLine>();
                              groupList[0].IsBase = true;
                              SaveRouteLine(groupList[0]);
                          }
                      }
                    );

            }
        }


        /// <summary>
        /// ConvertCatalogsTo7Positions
        /// </summary>
        public void ConvertCatalogsTo7Positions()
        {
            string[] operatorsConvertNotDBArray = ExportConfiguration.ExportConfigurator.GetConfig().OperatorsConvertNotDB.Split(SEPARATOR.ToCharArray());
            bool isCurrentOpreratorInoperatorsConvertNotDBArray = operatorsConvertNotDBArray.FirstOrDefault<string>(el => el == GlobalData.LoginUser.UserOperator.IdOperator.ToString()) != null;
            var clusters = (from p in GlobalData.RouteModel.RouteLineList
                            group p by p.IdCluster into g
                            select g.Key).ToList<int>();
            if (clusters.IsListFull())
            {
                foreach (int idCluster in clusters)
                {
                    List<RouteLine> byClustersList = (from p in GlobalData.RouteModel.RouteLineList
                                                      where idCluster == p.IdCluster
                                                      orderby p.IdCluster
                                                      select p).ToList<RouteLine>();
                    byClustersList.ForEach(rl =>
                    {
                        if (rl.Catalog.IsLengthLess7())
                        {
                            var sameCatalogList = (from p in byClustersList
                                                   where p.RouteNumber == rl.RouteNumber && p.Catalog.IsLengthLess7()
                                                   group p by p.Catalog into g
                                                   select g.Key).ToList<int?>();
                            int counter = -1;
                            foreach (int? catalog in sameCatalogList)
                            {
                                counter++;
                                List<RouteLine> lstSameCatalogs = (from p in byClustersList
                                                                   where p.RouteNumber == rl.RouteNumber && p.Catalog.IsLengthLess7() && p.Catalog == catalog
                                                                   select p).ToList<RouteLine>();
                                if (lstSameCatalogs.IsListFull())
                                    lstSameCatalogs.ForEach(rlSame => UpdateRouteLineCatalog(rlSame, counter, isCurrentOpreratorInoperatorsConvertNotDBArray));
                            }
                        }
                    });
                }
            }
        }


        private void UpdateRouteLineCatalog(RouteLine rl, int counter, bool isCurrentOpreratorInoperatorsConvertNotDBArray)
        {
            rl.CatalogBackUp = rl.Catalog.Value;
            rl.Catalog = int.Parse(string.Format("{0}{1}{2}", rl.IdCluster.ToString(), counter.ToString(), rl.RouteNumber.Value.AddZerroInStart(3).ToString()));

            if (isCurrentOpreratorInoperatorsConvertNotDBArray || !_internalBaseDal.IsCatalogExists(rl))
                SaveRouteLine(rl);
            else
                throw new ApplicationException(string.Format(Resources.BLManagerResource.ConvertNewCatalogProblem, rl.RouteNumber, rl.ClusterName));
        }



        /// <summary>
        /// Build Export Catalog
        /// </summary>
        /// <param name="idCluster"></param>
        /// <param name="rl"></param>
        /// <param name="counter"></param>
        public string BuildExportCatalog(RouteLine rl)
        {
            string returnVaue = rl.Catalog.Value.AddZerroInStart(7);
            return returnVaue;
        }

        /// <summary>
        /// Is Layer Connected To RS
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public bool IsLayerConnectedToRS(string layerName)
        {
            List<string> layers = _transCadFetchData.GetLayersName();
            return layers.IsListFull() && layers.Exists(l => l == layerName);

        }

        /// <summary>
        /// Get Map Title
        /// </summary>
        /// <returns></returns>
        public string GetMapTitle()
        {
            string mapTitle = _transCadFetchData.GetMapTitle(ExportConfiguration.ExportConfigurator.GetConfig().MapLayerName);
            mapTitle = mapTitle.Replace("|", " ");
            if (mapTitle.EndsWith(" "))
            {
                mapTitle = mapTitle.Substring(0, mapTitle.Length - 1);
            }
            return mapTitle;
        }
        /// <summary>
        /// GetS top City
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public string GetStopCity(RouteStop routeStop)
        {
            return StringHelper.ConvertToHebrewEncoding(_transCadFetchData.GetStopCity(routeStop.Longitude, routeStop.Latitude, ExportConfiguration.ExportConfigurator.GetConfig().MunLayer));
        }
        /// <summary>
        /// Calculate Route Line Stops Duration
        /// </summary>
        /// <param name="routeLine"></param>
        public void CalculateRouteLineStopsDuration(RouteLine routeLine)
        {
            if (routeLine == null || !GlobalData.RouteModel.RouteStopList.IsListFull()) return;
            List<RouteStop> rsList = (from p in GlobalData.RouteModel.RouteStopList
                                      where p.RouteId == routeLine.ID
                                      orderby p.Milepost
                                      select p).ToList();
            if (!rsList.IsListFull()) return;
            List<RouteLink> linkIds = _transCadFetchData.GetRouteLinkListForDuration(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MapLayerName, ExportConfigurator.GetConfig().Endpoints);
            if (!linkIds.IsListFull()) return;
            int counter = 0;
            List<LinkStop> rsLinkStops = _transCadFetchData.GetStationLinkID(ExportConfigurator.GetConfig().RouteSystemLayerName, routeLine);
            rsList.ForEach(rs =>
            {
                float duration = 0;
                double lengthFromStart = 0;
                counter++;
                rs.LinkId = rsLinkStops.FindLast(rsInner => rsInner.StopId == rs.ID).LinkId;
                int c = 0;
                foreach (var linkId in linkIds)
                {
                    c++;
                    if (linkId.LinkId == rs.LinkId)
                    {
                        Coord coord = null;
                        double distance = 0;
                        if (linkId.TraverseDirection == 1)
                            coord = _transCadFetchData.GetCoordinate(linkId.NodeIdFirst, ExportConfigurator.GetConfig().Endpoints);
                        else
                            coord = _transCadFetchData.GetCoordinate(linkId.NodeIdLast, ExportConfigurator.GetConfig().Endpoints);

                        distance = Math.Abs(_transCadFetchData.GetDistance(coord, new Coord { Longitude = rs.Longitude, Latitude = rs.Latitude }));


                        float part = ((float)distance / linkId.Length) * linkId.Duration;
                        duration += part;
                        lengthFromStart += distance;
                        break;
                    }
                    duration += linkId.Duration;
                    lengthFromStart += linkId.Length;
                }
                rs.Duration = duration;
            });
        }


        public string GetShapeFileFolder
        {
            get
            {
                var path = Path.Combine(ExportConfigurator.GetConfig().ShapeFileFolder, GlobalData.LoginUser.UserOperator.IdOperator.ToString()); 
                if (GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig==null)
                    return path;
                path = Path.Combine(path, Clusters);
                return Path.Combine(path, GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.ClusterId.ToString());
            }
        }
        /// <summary>
        /// Calculate All Route Line Stops Duration
        /// </summary>
        public void RunExportMacro(string whereClass, List<RouteLine> routeLineList)
        {
            if (!routeLineList.IsListFull())
                throw new ApplicationException(BLManagerResource.NonUpdatedList);
            string macroName = ExportConfigurator.GetConfig().ExportMacroName;

            var lst = new List<string> { "Duration" };
            _transCadFetchData.SetReadOnlyFieldSet(ExportConfigurator.GetConfig().RouteStopsLayerName, lst, false);
            string uiFile = ExportConfigurator.GetConfig().ExportMacroUIFile;

            if (IoHelper.IsFolderExists(GetShapeFileFolder))
                Directory.Delete(GetShapeFileFolder, true);

            IoHelper.CreateFolder(GetShapeFileFolder);

            _transCadFetchData.RunExportMacro(macroName, uiFile, new List<object>() { string.Concat(GetShapeFileFolder, "\\"), whereClass });
            // Read Only approve
            _transCadFetchData.SetReadOnlyFieldSet(ExportConfigurator.GetConfig().RouteStopsLayerName, lst, true);
            routeLineList.ForEach(rl => rl.ShapeFile = BLSharedUtils.GetShapeFileContent(GetShapeFileFolder, rl.ID));

        }



        /// <summary>
        /// Re Populate Route Stop List
        /// </summary>
        /// <returns></returns>
        public void RePopulateRouteStopList()
        {

            var dicrouteStop = _transCadFetchData.GetRouteStopsRawDataDictionary();
            if (!dicrouteStop.IsDictionaryFull())
            {
                throw new ApplicationException(BLManagerResource.TranscadNispah3DBListEmpty);
            }

            var updatedList = new List<RouteStop>();
            foreach (int routeId in dicrouteStop.Keys)
            {
                updatedList.AddRange(dicrouteStop[routeId]);
            }

            if (updatedList.IsListFull())
            {

                GlobalData.RouteModel.RouteStopList.ForEach(rs =>
                {
                    RouteStop updRouteStop = updatedList.FindLast(uptRS => uptRS.ID == rs.ID);
                    if (updRouteStop != null)
                    {
                        rs.Duration = updRouteStop.Duration;
                    }
                });
            }
        }
        /// <summary>
        /// ValidatePhysicalStopsData
        /// </summary>

        public void ValidatePhysicalStopsData()
        {
            if (GlobalData.RouteModel != null)
            {
                if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
                {
                    GlobalData.RouteModel.PhysicalStopList.ForEach(rs => rs.Validate());
                }
            }
        }
        /// <summary>
        /// IsOperatorsEnableCalcFirstStation
        /// </summary>
        public bool IsDanLikeOperators
        {
            get
            {
                var operatorsEnableCalcFirstStation = ExportConfigurator.GetConfig().DanLikeOperators.Split(SEPARATOR.ToCharArray());
                return operatorsEnableCalcFirstStation.FirstOrDefault(el => el == GlobalData.LoginUser.UserOperator.IdOperator.ToString()) != null;
            }
        }

        /// <summary>
        /// IsEgedOperator
        /// </summary>

        public bool IsEgedOperator
        {
            get { return GlobalData.LoginUser.UserOperator.IdOperator == 3; }
        }

        /// <summary>
        /// GetDateTimeFromString
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static DateTime? GetDateTimeFromString(string dateTime)
        {
            if (!string.IsNullOrEmpty(dateTime) && dateTime != @"  /  /")
            {
                string[] format = { "dd/MM/yyyy" };
                DateTime date;

                if (DateTime.TryParseExact(dateTime, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }
            }

            return null;
        }
        /// <summary>
        /// Get Route Line List With Not Valid Export Date
        /// </summary>
        /// <param name="sourse"></param>
        /// <returns></returns>
        public List<RouteLine> GetRouteLineListWithNotValidExportDate(List<RouteLine> sourse)
        {
            var errorList = new List<RouteLine>();
            if (sourse.IsListFull())
            {
                    sourse.ForEach(rl =>
                   {
                       var dt = GetDateTimeFromString(rl.ValidExportDate);
                       if (dt.HasValue)
                       {
                           if (DateTime.Today<dt.Value)
                           {
                                errorList.Add(rl);
                           }
                       }

                   });
            }
            return errorList;
        }
        /// <summary>
        /// CloseConnection
        /// </summary>
        public void CloseConnection()
        {
            _transCadFetchData.CloseConnection();
        }

        /// <summary>
        /// BuildModelData
        /// </summary>
        public void BuildModelData()
        {
            BuildModelData(null);
        }

        #endregion
    }
}
