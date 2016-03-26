using System.Collections.Generic;
using System.Globalization;

namespace BLEntities.Entities
{
    public class ClusterToZone
    {
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }

        //public int MainZoneID { get; set; }
        //public int SubZoneID { get; set; }

        //public string  MainZoneName { get; set; }
        //public string SubZoneName { get; set; }

        public List<ClusterState> ClusterStateList { get; set; }

        
    }
}
