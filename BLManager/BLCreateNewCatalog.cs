using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Entities;
using BLEntities.Model;
using Utilities;
using IDAL;
using DAL;
using System.Diagnostics;
namespace BLManager
{
    public class BLCreateNewCatalog
    {
        private ILineDetailsPresenter lineDetailsPresenter = new LineDetailsPresenter() ;
        public List<BaseTableEntity> Clusters { get { return lineDetailsPresenter.GetClusters(); } }

        public CatalogInfo CatalogInfoNew { get; set; }
        /// <summary>
        /// Get Catalog Y Value
        /// </summary>
        /// <param name="activeRouteLine"></param>
        /// <returns></returns>
        private int GetCatalogYValue(RouteLine activeRouteLine)
        {
            int zerro = 0;
            if (activeRouteLine == null || !activeRouteLine.Catalog.HasValue || activeRouteLine.Catalog.ToString().Length < 7)
                return zerro;
            else
            {
                int val = int.Parse(activeRouteLine.Catalog.ToString().Substring(3, 1));
                return val;
            }
        }
        /// <summary>
        /// Set Catalog Null
        /// </summary>
        public void SetCatalogNull()
        {
            CatalogInfoNew = null;
        }

        /// <summary>
        /// Is Catalog Already Exists
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="routeNumber"></param>
        /// <returns></returns>
        public bool IsCatalogAlreadyExists(int catalog, int idCluster)
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                List<RouteLine> routeLineMatchList = (from p in GlobalData.RouteModel.RouteLineList
                                                      where p.Catalog == catalog && p.IdCluster == idCluster
                                                      select p).ToList<RouteLine>();


                return routeLineMatchList.IsListFull();
            }

            /// TODO :
            /// Add code to check if the Transport Licensing database has the Catalog SLAVA

            return false; 
        }
        


        /// <summary>
        /// Get Catalog
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="routeNumber"></param>
        /// <returns></returns>
        public void  SetCatalog(BaseTableEntity cluster, AccountingGroup accountingGroup,  int routeNumber, int catalog)
        {
            CatalogInfoNew = new CatalogInfo
            {
                Catalog = catalog,
                AccountingGroupID = accountingGroup.AccountingGroupID,
                IdCluster = cluster.ID,
                RouteNumber = routeNumber
            };
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

    }
}
