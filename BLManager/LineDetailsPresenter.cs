using System;
using System.Collections.Generic;
using System.Linq;
using IBLManager;
using BLEntities.Entities;
using BLEntities.Model;
using Logger;
using Utilities;
using ExportConfiguration;
using DAL;
using IDAL;
namespace BLManager
{
    public class LineDetailsPresenter : ILineDetailsPresenter
    {
        readonly IDataSearchAndManipulateBLManager _dataSearchAndManipulateBlManager = new DataSearchAndManipulateBLManager();
        readonly ITransCadBLManager _transCadBlManager = new TransCadBlManager();
        readonly IInternalBaseDal _dalDb = new InternalTvunaImplementationDal();

        public LineDetailsPresenter()
        {
            if (_transCadBlManager.IsEgedOperator)
                _transCadBlManager = new EgedBlManager();
        }

        /// <summary>
        /// Get Last FilterdItem
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteLine GetLastFilterdItem(List<RouteLine> filteredList, RouteLine current)
        {
            if (filteredList == null) return null;
            return filteredList[0];
        }
        /// <summary>
        /// Get First Filterd Item
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteLine GetFirstFilterdItem(List<RouteLine> filteredList, RouteLine current)
        {
            if (filteredList == null) return null;
            return filteredList[filteredList.Count - 1];
        }
        /// <summary>
        /// Get Next Filterd Item
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteLine GetNextFilterdItem(List<RouteLine> filteredList, RouteLine current)
        {
            if (filteredList == null || current == null) return null;
            RouteLine retValue = current;

            int index = filteredList.FindIndex(p => p.ID == current.ID);
            if (index < filteredList.Count - 1)
            {
                retValue = filteredList[++index];
            }
            return retValue;
        }
        /// <summary>
        /// Get Prev Filterd Item
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteLine GetPrevFilterdItem(List<RouteLine> filteredList, RouteLine current)
        {
            if (filteredList == null || current == null) return null;
            RouteLine retValue = current;

            int index = filteredList.FindIndex(p => p.ID == current.ID);
            if (index > 0)
            {
                retValue = filteredList[--index];
            }
            return retValue;
        }

        #region ILineDetailsPresenter Members

        /// <summary>
        /// Get Clusters
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetClusters()
        {
            return _dataSearchAndManipulateBlManager.GetClusters();
        }
        /// <summary>
        /// Get Route Type
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetRouteType()
        {
            return _dataSearchAndManipulateBlManager.GetRouteType();
        }
        /// <summary>
        /// Get Service Type
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetServiceType()
        {
            return _dataSearchAndManipulateBlManager.GetServiceType();
        }
        /// <summary>
        /// GetCatalogInfo
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public CatalogInfo GetCatalogInfo(int? catalog)
        {
            if (GlobalData.CatalogInfolList.IsListFull())
            {
                return GlobalData.CatalogInfolList.Find(ci => ci.Catalog == catalog);
            }
            return null;
        }

        /// <summary>
        /// Add Catalog To Catalog List
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public CatalogInfo AddCatalogToCatalogList(int catalog, int idCluster, int accountingGroupID, int routeNumber)
        {
            if (catalog > 0)
            {
                if (GlobalData.CatalogInfolList.IsListFull())
                {
                    if (!GlobalData.CatalogInfolList.Exists(c => c.Catalog == catalog && c.IdCluster == idCluster))
                    {
                        CatalogInfo cinfo = new CatalogInfo
                        {
                            Catalog = catalog,
                            IdCluster = idCluster,
                            RouteNumber = routeNumber,
                            AccountingGroupID = accountingGroupID
                        };
                        GlobalData.CatalogInfolList.Add(cinfo);
                        return cinfo;
                    }
                }
            }
            return null;
        }



        /// <summary>
        /// Get Seasonal
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetSeasonal()
        {
            return _dataSearchAndManipulateBlManager.GetSeasonal();
        }
        /// <summary>
        /// ShowSelectStateForm
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool ShowSelectStateForm(RouteLine routeLine)
        {
            if (routeLine.IdCluster > 0)
            {
                if (!GlobalData.ClusterToZoneDictionary.ContainsKey(routeLine.IdCluster))
                    throw new ApplicationException(string.Format("Cluster ID {0} was not found in the Database. Please, contact your Admin", routeLine.IdCluster));
                var clusterToZone = GlobalData.ClusterToZoneDictionary[routeLine.IdCluster];
                var be = GlobalData.BaseTableEntityDictionary[GlobalConst.tblOperatorCluster].Find(it => it.ID == routeLine.IdCluster);
                if (be == null)
                    return false;
                if (clusterToZone != null && be.AdditonalInfo != null && be.AdditonalInfo.ContainsKey("IsManyDistricts") &&
                    Convert.ToBoolean(be.AdditonalInfo["IsManyDistricts"]) && clusterToZone.ClusterStateList.IsListFull())
                    return clusterToZone.ClusterStateList.Count > 1;
            }
            return false;
        }

        /// <summary>
        /// Save Route Line
        /// </summary>
        /// <param name="routeLine"></param>

        /// <returns></returns>
        public bool SaveRouteLine(RouteLine routeLine)
        {
            if (routeLine.IsNewEntity) // Set Id IdZoneHead  Zone Code and AccountGroupId
            {
                if (routeLine.IdZoneHead == 0)
                {
                    var clusterToZone = GlobalData.ClusterToZoneDictionary[routeLine.IdCluster];
                    if (clusterToZone != null && clusterToZone.ClusterStateList.IsListFull())
                    {
                        routeLine.IdZoneHead = clusterToZone.ClusterStateList[0].MainZoneId;
                        routeLine.IdZoneSubHead = clusterToZone.ClusterStateList[0].SubZoneId;
                    }
                }
                routeLine.AccountingGroupID = 0;
            }
            if (!_dataSearchAndManipulateBlManager.IsRouteLineValid(routeLine))
                return false;
            if (routeLine.IsBase) // Update all Catalog , Direction, Var lines => IsBase = 0
            {
                GlobalData.RouteModel.RouteLineList.ForEach(rl =>
                {
                    if (rl.IdOperator == routeLine.IdOperator &&
                        rl.IdCluster == routeLine.IdCluster && rl.Catalog == routeLine.Catalog &&
                        rl.Dir == routeLine.Dir && rl.Var != routeLine.Var)
                    {
                        rl.IsBase = false;
                        _transCadBlManager.SaveRouteLine(rl);
                    }
                }
                    );
            }

            return _transCadBlManager.SaveRouteLine(routeLine);
        }
        /// <summary>
        /// Get Car Types
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetVehicleTypes()
        {
            return _dataSearchAndManipulateBlManager.GetVehicleTypes();
        }
        /// <summary>
        /// Get Car Size
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetVehicleSizes()
        {
            return _dataSearchAndManipulateBlManager.GetVehicleSizes();
        }
        /// <summary>
        /// IsRouteLineValid
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsRouteLineValid(RouteLine routeLine)
        {
            return _dataSearchAndManipulateBlManager.IsRouteLineValid(routeLine);
        }

        /// <summary>
        /// GetDirections
        /// </summary>
        /// <returns></returns>
        public List<string> GetDirections()
        {
            return _dataSearchAndManipulateBlManager.GetDirectionList();
        }


        /// <summary>
        /// Is Variant Exists In Variant Num Table
        /// </summary>
        /// <param name="variant"></param>
        /// <returns></returns>
        public bool IsVariantExistsInVariantNumTable(string variant)
        {
            return _dataSearchAndManipulateBlManager.IsVariantExistsInVariantNumTable(variant);
        }

        /// <summary>
        /// Is Service Type Enable After Changing Route Type
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public void UpdateCatalogOfRouteLines(CatalogInfo catalogInfo)
        {
            _dataSearchAndManipulateBlManager.UpdateCatalogOfRouteLines(catalogInfo);
        }
        /// <summary>
        /// Is Service Type Disable
        /// </summary>
        /// <param name="routeType"></param>
        /// <returns></returns>
        public bool IsServiceTypeDisable(int routeType)
        {
            return routeType == 5 || routeType == 1;
        }
        /// <summary>
        /// Collect Service Type
        /// </summary>
        public int CollectServiceType
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Set Initiate Catalog Data
        /// </summary>
        /// <param name="routeLine"></param>
        public void SetInitiateCatalogData(RouteLine routeLine)
        {
            if (routeLine != null)
            {
                routeLine.IdCluster = 0;
                routeLine.RouteNumber = null;
                routeLine.IdServiceType = 0;
                routeLine.IdZoneHead = 0;
                routeLine.IdZoneSubHead = 0;
            }
        }
        /// <summary>
        /// Set Cataog Data By Value Changing
        /// </summary>
        /// <param name="routeLine"></param>
        public void SetCatalogDataByValueChanging(int? catalog, RouteLine routeLine)
        {
            _dataSearchAndManipulateBlManager.SetCatalogDataByValueChanging(catalog, routeLine);
        }
        /// <summary>
        /// Remove Catalog FromList
        /// </summary>
        /// <param name="catalogInfo"></param>
        public void RemoveCatalogFromList(CatalogInfo catalogInfo)
        {
            if (catalogInfo != null)
            {
                if (GlobalData.CatalogInfolList.IsListFull())
                {
                    if (GlobalData.CatalogInfolList.Exists(c => c.Catalog == catalogInfo.Catalog
                        && c.IdCluster == catalogInfo.IdCluster))
                    {
                        GlobalData.CatalogInfolList.Remove(catalogInfo);
                    }
                }
            }
        }
        /// <summary>
        /// Get Selected List
        /// </summary>
        /// <param name="delimitedValues"></param>
        /// <param name="baseTableName"></param>
        /// <returns></returns>
        private List<BaseTableEntity> GetSelectedList(string delimitedValues, string baseTableName)
        {
            List<BaseTableEntity> lst = new List<BaseTableEntity>();
            if (!string.IsNullOrEmpty(delimitedValues))
            {
                string[] arrayList = delimitedValues.Split(ExportConfigurator.GetConfig().LogicalDataSplitDevider.ToCharArray());
                if (arrayList.IsListFull())
                {
                    foreach (string item in arrayList)
                    {
                        if (!item.Equals("0"))
                        {
                            if (GlobalData.BaseTableEntityDictionary[baseTableName] != null)
                            {
                                BaseTableEntity be = GlobalData.BaseTableEntityDictionary[baseTableName].Find(it => it.ID.ToString() == item);
                                if (be != null)
                                {
                                    lst.Add(be);
                                }
                            }
                        }
                    }
                }
            }
            return lst;
        }
        /// <summary>
        /// Get Selected Vehicle Types
        /// </summary>
        /// <param name="delimitedValues"></param>
        /// <returns></returns>
        public List<BaseTableEntity> GetSelectedVehicleTypes(string delimitedValues)
        {
            return GetSelectedList(delimitedValues, GlobalConst.baseVehicleType);
        }
        /// <summary>
        /// Get Selected Vehicle Sizes
        /// </summary>
        /// <param name="delimitedValues"></param>
        /// <returns></returns>
        public List<BaseTableEntity> GetSelectedVehicleSizes(string delimitedValues)
        {
            return GetSelectedList(delimitedValues, GlobalConst.baseVehicleSize);
        }
        /// <summary>
        /// Is Luz Of Line Exists
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsLuzOfLineExists(RouteLine routeLine)
        {
            if (routeLine == null || routeLine.IsNewEntity)
                return false;
            bool result = false;
            try
            {
                result = _dalDb.IsLuzOfLineExists(routeLine);
                return result;
            }
            catch (Exception exp)
            {
                Logger.LoggerManager.WriteToLog(exp);
                return false;
            }
        }

        /// <summary>
        /// GetExclusivityLineType
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetExclusivityLineType()
        {
            return _dataSearchAndManipulateBlManager.GetExclusivityLineType();
        }


        public bool IsSuperViser
        {
            get { return _transCadBlManager.IsAdminFormEnabled; }
        }

        public bool IsPlanningFirm
        {
            get { return GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm; }
        }

        /// <summary>
        /// IsEgedOperator
        /// </summary>

        public bool IsEgedOperator
        {
            get { return _transCadBlManager.IsEgedOperator; }
        }

        /// <summary>
        /// Get Accounting Group By Operator And Cluster
        /// </summary>
        /// <param name="idCluster"></param>
        /// <returns></returns>
        public List<AccountingGroup> GetAccountingGroupByOperatorAndCluster(BaseTableEntity cluster, int currentAccountGroupByOperatorId)
        {

            IInternalBaseDal dal = new InternalTvunaImplementationDal();
            List<AccountingGroup> lst = new List<AccountingGroup>();
            if (cluster.ID == 0)
            {
                lst.Insert(0, new AccountingGroup { OperatorId = GlobalData.LoginUser.UserOperator.IdOperator, ClusterId = cluster.ID, AccountingGroupID = -1, AccountingGroupDesc = string.Empty });
            }
            else
            {
                lst = dal.GetAccountingGroupByOperatorIdAndClusterId(GlobalData.LoginUser.UserOperator.IdOperator, cluster.ID);
                if (currentAccountGroupByOperatorId == -1)
                {
                    lst.Insert(0, new AccountingGroup { OperatorId = GlobalData.LoginUser.UserOperator.IdOperator, ClusterId = cluster.ID, AccountingGroupID = -1, AccountingGroupDesc = string.Empty });
                }
            }
            return lst;
        }

        /// <summary>
        /// Get Catalogs Of Same Route Number
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="newRouteNumber"></param>
        /// <returns></returns>
        public List<CatalogInfo> GetCatalogsOfSameRouteNumber(RouteLine routeLine, string newRouteNumber)
        {
            var resultCatalogList = new List<CatalogInfo>();
            if (string.IsNullOrEmpty(newRouteNumber)) return resultCatalogList;
            int newRouteNumberValue = int.Parse(newRouteNumber);
            if (routeLine.RouteNumber == newRouteNumberValue && !routeLine.IsNewEntity) return resultCatalogList;

            BLSharedUtils.BuildCatalogInfoList(); // Build Catalog List   
            List<CatalogInfo> foundCatalogInfoList = GlobalData.CatalogInfolList.FindAll(ci => ci.RouteNumber == newRouteNumberValue);
            if (foundCatalogInfoList.IsListFull())
                foundCatalogInfoList.ForEach(el => resultCatalogList.Add(new CatalogInfo
                {
                    Catalog = el.Catalog,
                    FromPathCityName = el.FromPathCityName,
                    IdCluster = el.IdCluster
                }));

            return resultCatalogList.OrderBy(el => el.Catalog).ToList();
        }


        public void UpdateClusterZone(RouteLine routeLine, ClusterState clusterState)
        {
            if (clusterState != null && routeLine != null)
            {
                routeLine.IdZoneHead = clusterState.MainZoneId;
                routeLine.IdZoneSubHead = clusterState.SubZoneId;
                if (routeLine.IdZoneHead != routeLine.IdZoneSubHead)
                {
                    LoggerManager.WriteToLog(string.Format("The {0} MainZoneId and {1} SubZoneId of the {} cluster Id are not the same.", clusterState.MainZoneId, clusterState.SubZoneId));
                    routeLine.IdZoneHead = routeLine.IdZoneSubHead;
                }
            }
        }

        /// <summary>
        /// Update Catalog With Selected Catalog
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="catalog"></param>
        /// <param name="newRouteNumber"></param>
        public bool UpdateCatalogWithSelectedCatalog(RouteLine routeLine, int catalog, string newRouteNumber, ref string error)
        {
            if (string.IsNullOrEmpty(newRouteNumber)) return false;

            int newRouteNumberValue = int.Parse(newRouteNumber);

            if ((routeLine.RouteNumber != newRouteNumberValue && catalog == 0) || (routeLine.RouteNumber == newRouteNumberValue && catalog == 0 && routeLine.IsNewEntity))
            {
                IInternalBaseDal dal = new InternalTvunaImplementationDal();

                if (routeLine.IdZoneHead == 0)
                {
                    var cluster = GlobalData.ClusterToZoneDictionary[routeLine.IdCluster];
                    if (cluster != null && cluster.ClusterStateList.IsListFull())
                    {
                        routeLine.IdZoneHead = cluster.ClusterStateList[0].MainZoneId;
                        routeLine.IdZoneSubHead = routeLine.IdZoneHead;
                    }
                }
                if (routeLine.IsNewEntity && routeLine.IdCluster > 0 && routeLine.AccountingGroupID == -1)
                {
                    routeLine.AccountingGroupID = 0;
                }

                bool? isArrivePlanning = null;
                if (IsPlanningFirm)
                    isArrivePlanning = true;
                LoggerManager.WriteToLog(string.Format("***** New Line is Appended to RS => The routeLine {0} in Operator {1} has IdZoneHead={2} IsPlanning Firm {3}", routeLine, GlobalData.LoginUser.UserOperator.IdOperator, routeLine.IdZoneHead, isArrivePlanning ?? false));
                DalShared.UpdateCitiesAndJunctionList(routeLine);
                var resultFreeOfficeLineId = dal.GetFreeOfficeLineId(routeLine, newRouteNumberValue, isArrivePlanning);
                if (resultFreeOfficeLineId != null && resultFreeOfficeLineId.ContainsKey("ErrorDesc") && resultFreeOfficeLineId.ContainsKey("OfficeLineId"))
                {
                    if (string.IsNullOrEmpty(resultFreeOfficeLineId["ErrorDesc"].ToString()))
                    {
                        routeLine.Catalog = Convert.ToInt32(resultFreeOfficeLineId["OfficeLineId"]);
                        routeLine.RouteNumber = newRouteNumberValue;
                        return true;
                    }
                    error = resultFreeOfficeLineId["ErrorDesc"].ToString();
                }
                return false;
            }
            if ((routeLine.RouteNumber != newRouteNumberValue && catalog > 0) || (routeLine.RouteNumber == newRouteNumberValue && catalog > 0 && routeLine.IsNewEntity))
            {
                if (GlobalData.CatalogInfolList.IsListFull())
                {
                    var foundCatalogInfo = GlobalData.CatalogInfolList.FindLast(ci => ci.Catalog == catalog);
                    if (foundCatalogInfo != null)
                    {
                        routeLine.Catalog = foundCatalogInfo.Catalog;
                        routeLine.IdCluster = foundCatalogInfo.IdCluster;
                        routeLine.RouteNumber = foundCatalogInfo.RouteNumber;
                        routeLine.IdServiceType = foundCatalogInfo.IdServiceType;
                        routeLine.IdZoneHead = foundCatalogInfo.IdZoneHead;
                        routeLine.IdZoneSubHead = foundCatalogInfo.IdZoneSubHead;
                        routeLine.IdExclusivityLine = foundCatalogInfo.IdExclusivityLineType;
                        routeLine.AccountingGroupID = foundCatalogInfo.AccountingGroupID;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// UpdateCatalogAttributes
        /// </summary>
        /// <param name="routeLine"></param>
        //public bool UpdateCatalogAttributes(RouteLine routeLine, string newRouteNumber)
        //{
        //    int newRouteNumberValue =int.Parse(newRouteNumber);
        //    IInternalBaseDAL dal = new InternalTvunaImplementationDAL();

        //    if (routeLine.RouteNumber != newRouteNumberValue || routeLine.IsNewEntity)
        //    {
        //        if (GlobalData.CatalogInfolList.IsListFull())
        //        {
        //            CatalogInfo foundCatalogInfo = GlobalData.CatalogInfolList.FindLast(ci => ci.RouteNumber == newRouteNumberValue);
        //            if (foundCatalogInfo == null || routeLine.IsNewEntity) // not found into catalog list we will create a new one
        //            {
        //                routeLine.Catalog = dal.GetFreeOfficeLineId(routeLine, newRouteNumberValue);
        //                routeLine.RouteNumber = newRouteNumberValue;
        //            }
        //            else // found into the current Catalog list. We'll change the Route Line Attributes according the found Catalog
        //            {
        //                routeLine.Catalog = foundCatalogInfo.Catalog;
        //                routeLine.IdCluster = foundCatalogInfo.IdCluster;
        //                routeLine.RouteNumber = foundCatalogInfo.RouteNumber;
        //                routeLine.IdServiceType = foundCatalogInfo.IdServiceType;
        //                routeLine.IdZoneHead = foundCatalogInfo.IdZoneHead;
        //                routeLine.IdZoneSubHead = foundCatalogInfo.IdZoneSubHead;
        //                routeLine.IdExclusivityLine = foundCatalogInfo.IdExclusivityLineType;
        //                routeLine.AccountingGroupID = foundCatalogInfo.AccountingGroupID;
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// Update Cluster For Same Route Lines
        /// </summary>
        /// <param name="rouleLine"></param>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public bool UpdateClusterForSameRouteLines(RouteLine rouleLine, BaseTableEntity cluster)
        {
            if (rouleLine != null && cluster != null)
            {
                List<RouteLine> listSameCatalogLines = GlobalData.RouteModel.RouteLineList.FindAll(rl => rl.Catalog == rouleLine.Catalog);// find lines with the same catalog 
                if (listSameCatalogLines.IsListFull())
                {
                    listSameCatalogLines.ForEach(rl =>
                    {
                        rl.IdCluster = cluster.ID;
                        rl.AccountingGroupID = 0;
                        _transCadBlManager.SaveRouteLine(rl);
                    });
                    return true;
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// Update Accounting Group For Same Route Lines
        /// </summary>
        /// <param name="rouleLine"></param>
        /// <param name="accountiongGroup"></param>
        /// <returns></returns>
        public bool UpdateAccountingGroupForSameRouteLines(RouteLine rouleLine, AccountingGroup accountiongGroup)
        {
            if (rouleLine != null && accountiongGroup != null)
            {
                List<RouteLine> listSameCatalogLines = GlobalData.RouteModel.RouteLineList.FindAll(rl => rl.Catalog == rouleLine.Catalog);// find lines with the same catalog 
                if (listSameCatalogLines.IsListFull())
                {
                    listSameCatalogLines.ForEach(rl =>
                    {
                        rl.AccountingGroupID = accountiongGroup.AccountingGroupID;
                        _transCadBlManager.SaveRouteLine(rl);
                    });
                    return true;
                }
                return false;
            }
            return false;
        }


        #endregion
    }
}
