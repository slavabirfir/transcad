using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Report
{
    public class ErrorReportEntity
    {
        public string  ID { get; set; }
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string ErrorString { get; set; }
    }
}
