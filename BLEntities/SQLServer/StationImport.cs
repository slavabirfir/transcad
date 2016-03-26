using System;

namespace BLEntities.SQLServer 
{
    [Serializable]
    public class StationImport : SQLImportEntityBase
    {
        public int StationId { get; set; }
        
        public string  StationName { get; set; }

        public string StationShortName { get; set; }
        

        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string  CityId { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string B56Heb { get; set; }
        public string B56Eng { get; set; }
        public int LinkId{ get; set; }
        public string StationCatalog { get; set; }

        public decimal  LatDifferrent { get; set; }
        public decimal LongDifferrent { get; set; }
        public int? AreaOperatorId { get; set; }

        public int StationStatusId { get; set; }
        public int StationTypeId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
