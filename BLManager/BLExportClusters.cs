using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using BLEntities.Model;
using Utilities;

namespace BLManager
{
    public class BLExportClusters
    {
        public List<BaseTableEntity> SelectedClusters { get; set; }
        public List<BaseTableEntity> AllClusters { get; set; }
        public BLExportClusters()
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                var result = (from p in GlobalData.RouteModel.RouteLineList
                              where !string.IsNullOrEmpty(p.ClusterName)
                              orderby p.RouteNumber
                              select new BaseTableEntity { ID = p.IdCluster, Name = p.ClusterName});
                AllClusters = result.Distinct(new BaseTableEntityComparer()).ToList();
            }
        }

        internal class BaseTableEntityComparer : IEqualityComparer<BaseTableEntity>
        {
            public bool Equals(BaseTableEntity x, BaseTableEntity y)
            {
                if (x == null || y == null) return false; 
                if (Object.ReferenceEquals(x.ID, y.ID)) return true;
                return x.ID == y.ID;
            }


            public int GetHashCode(BaseTableEntity baseTableEntity)
            {
                if (Object.ReferenceEquals(baseTableEntity, null)) return 0;
                return baseTableEntity.ID.GetHashCode();
            }
        }


    }
}
