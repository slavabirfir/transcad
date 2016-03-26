using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace BLEntities.SQLServer 
{
    public class LineDetailStationImport : SQLImportEntityBase
    {
        public int ImportControlId { get; set; }
        public int OperatorId { get; set; }
        public int OfficeLineId { get; set; }
        public int Direction { get; set; }
        public string  LineAlternative { get; set; }
        public long StationId { get; set; }
        public int StationOrder { get; set; }
        public int StationType { get; set; }
        public int StationActivityType { get; set; }
        public int FirstDropStationId { get; set; }
        public int DistanceFromPreviousStation { get; set; }
        public int DistanceFromOriginStation { get; set; }
        public float Duration { get; set; }
        public short?  StationFloor { get; set; }
        public string StationPlatform { get; set; }
	}
}
