using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.SQLServer

{   
    //public enum ExternalImport_InsertImportZoneControlOperator_Status
    //{
    //    COMPLITED = 0,
    //    FAILED = 3,
    //    START = null
    //}

    public class ExternalImport_InsertImportZoneControlOperator
    {
        public int OperatorId { get; set; }
        public DateTime FromDate { get; set; }
        public int ImportZoneControlOperatorId { get; set; }
        public int?  Status { get; set; }
    }
}
