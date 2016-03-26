using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace BLEntities.SQLServer 
{
    public class CarImport : SQLImportEntityBase
    {
        public int IdRouteCarImportRow { get; set; }
        public int ImportControlId { get; set; }
        public int IdOperator { get; set; }
        public int IdCluster { get; set; }
        public string Catalog { get; set; }
        public string Signpost { get; set; }
        public byte Direction { get; set; }
        public int VarNum { get; set; }
        public int IdVehicleType { get; set; }
        public int IdVehicleSize { get; set; }
        
    }
}
