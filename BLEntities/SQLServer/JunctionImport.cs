using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace BLEntities.SQLServer
{
    public class JunctionImport : SQLImportEntityBase
    {
        public long JunctionId { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string  JunctionDesc { get; set; }
        public int CityId { get; set; }
         
    }
}
