using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace BLEntities.SQLServer 
{
    public class LineDetailJunctionImport : SQLImportEntityBase
    {
        public int ImportControlId { get; set; }
        public int OperatorId { get; set; }
        public int OfficeLineId { get; set; } 
        public int Direction { get; set; }
        public string LineAlternative { get; set; } 
        public int JunctionOrder { get; set; }
        public long JunctionId { get; set; } 
        public int DistanceFromPreviousJunction { get; set; }
        public int DistanceFromOriginJunction { get; set; }
         
    }
}
