using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    public class CatalogInfo
    {
        public int? Catalog { get; set; }
        public int IdCluster { get; set; }
        public bool IsNew { get; set; }
        public int IdServiceType { get; set; }
        public int IdExclusivityLineType { get; set; }
        public string FromPathCityName { get; set; }
        public int? RouteNumber { get; set; }
        public int IdZoneHead { get; set; }
        public int IdZoneSubHead { get; set; }
        public int RouteLineBelongCounter { get; set; }
        public int AccountingGroupID { get; set; }


        public override string ToString()
        {
            return string.Format("{0} {1}", Catalog, FromPathCityName);
        } 
    }
}
