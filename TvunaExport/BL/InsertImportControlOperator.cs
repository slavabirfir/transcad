using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvunaExport.BL
{
    public class InsertImportControlOperator
    {
        public int OperatorId { get; set; }
        public DateTime ImportStartDate { get; set; }
        public DateTime ImportFinishDate { get; set; }
        public int PeriodId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int ImportControlId { get; set; }
    }
}
