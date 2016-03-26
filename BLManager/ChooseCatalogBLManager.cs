using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using Utilities;
using BLEntities.Model;

namespace BLManager
{
    public class CatalogPresenter
    {
        public int Catalog { get; set; }
        public string  FromPathCityName { get; set; }
        public int IdCluster { get; set; }
        public string ClusterName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}",Catalog,FromPathCityName,ClusterName) ;
        }
    }

    public class ChooseCatalogBLManager
    {
        public List<CatalogInfo> CatalogInfoList { get; set; }
        public ChooseCatalogBLManager(List<CatalogInfo> catalogInfoList)
        {
            this.CatalogInfoList = catalogInfoList;
        }
        public List<CatalogPresenter> GetList()
        {
            var result = new List<CatalogPresenter>();
            if (!result.IsListFull())
            {
                CatalogInfoList.ForEach(c => 
                  {
                      string clusterName = "";
                      if (GlobalData.BaseTableEntityDictionary[GlobalConst.tblOperatorCluster] != null)
                      {

                          BaseTableEntity be = GlobalData.BaseTableEntityDictionary[GlobalConst.tblOperatorCluster].Find(it => it.ID == c.IdCluster);
                          if (be != null)
                              clusterName = be.Name;
                          else
                              clusterName = string.Empty;

                      }
                      result.Add(new CatalogPresenter {
                                                        Catalog = c.Catalog.HasValue ? c.Catalog.Value : 0 ,
                                                        FromPathCityName = c.FromPathCityName,
                                                        ClusterName = clusterName
                                                    }
                                 );
                  });
 
            }
            return result;
        }
    }
}
