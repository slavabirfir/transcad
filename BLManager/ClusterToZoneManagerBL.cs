using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using DAL;
using Utilities;
using BLEntities.Model;
using BLEntities.Entities;

namespace BLManager
{
    public class ClusterToZoneManagerBL
    {

        /// <summary>
        /// dal
        /// </summary>
        private ITransCadMunipulationDataDAL dal = new TransCadMunipulationDataDAL();
        /// <summary>
        /// Set User Layers
        /// </summary>
        private static void ClusterToZoneData()
        {
            if (!GlobalData.ClusterToZoneDictionary.IsDictionaryFull())
                GlobalData.ClusterToZoneDictionary = DalShared.FillClusterToZoneDictionary();
        }
        /// <summary>
        /// Custer To Zone Manager BL
        /// </summary>
        public void SetCusterToZoneManagerBL()
        {
            ClusterToZoneData();
        }
        /// <summary>
        /// Get Cluster To Zone By Cluster ID
        /// </summary>
        /// <param name="clusterID"></param>
        /// <returns></returns>
        public ClusterToZone GetClusterToZoneByClusterID(int clusterID)
        {
            if (GlobalData.ClusterToZoneDictionary.IsDictionaryFull())
                return GlobalData.ClusterToZoneDictionary[clusterID];
            return null;
        }
    }
}
