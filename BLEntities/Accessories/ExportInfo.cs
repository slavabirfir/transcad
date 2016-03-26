using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Model;

namespace BLEntities.Accessories
{
    public class ExportInfo
    {
        public bool IsSuccess { get; set; }
        public int IdOperator { get; set; }
        public DateTime  ExportDate { get; set; }
        public RouteModel RouteModel { get; set; }
    }
}
