using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    public class Stop
    {
        public int StopID { get; set; }
        public string Name { get; set; }
        public string StationShortName { get; set; }

        public int LinkID { get; set; }
        public int DistNA { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int Across { get; set; }
        public string Neighborhood { get; set; }
        public string CityCode { get; set; }
        public int ID_SEKER { get; set; }
        public decimal LatDifferrent { get; set; }
        public decimal LongDifferrent { get; set; }
        public int StationStatusId { get; set; }
        public int StationTypeId { get; set; }

        public bool IsTrainMainStation { get; set; }
        //public bool IsCityLinkedManual { get; set; }
        public int? AreaOperatorId { get; set; }
    }
}
