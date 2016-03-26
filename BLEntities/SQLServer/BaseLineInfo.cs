using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.SQLServer
{
    public class BaseLineInfo
    {
        public int? Catalog { get; set; }
        public string Variant { get; set; }
        public short? Direction { get; set; }
        public int? IdCluster { get; set; }
        public bool IsBased { get; set; }
    }
}
