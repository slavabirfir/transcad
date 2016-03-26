using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace BLEntities.Accessories
{
    public class ExportToSQL
    {
        public bool TakeLastExportedInfrastructure { get; set; }
        public bool SaveLastExportedInfrastructure { get; set; }
        public ExportToSQLType ExportToSQLType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Year { get; set; }
        public int PeriodName { get; set; }
        public int TransactionID { get; set; }
    }
}
