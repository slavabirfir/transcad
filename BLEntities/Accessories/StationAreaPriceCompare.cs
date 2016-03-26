using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace BLEntities.Accessories
{
    public class StationAreaPriceCompareInputData
    {
        public string StationCatalog { get; set; }
        public Coord CurrentCoord { get; set; }
        public Coord NewCoord { get; set; }
        public string UnionStation { get; set; }
    }

    public class StationAreaPriceCompareOutputData
    {
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string StationCatalog { get; set; }
        public int CurrentIdPriceArea { get; set; }
        public int NewIdPriceArea { get; set; }
        public string UnionStation { get; set; }
        
    }

}
