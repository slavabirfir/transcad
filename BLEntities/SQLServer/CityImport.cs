using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace BLEntities.SQLServer 
{
    public class CityImport : SQLImportEntityBase
    {
        public int  CityId { get; set; }
        public string CityName { get; set; }
        public int? AuthorityId { get; set; }
        
    }
}
