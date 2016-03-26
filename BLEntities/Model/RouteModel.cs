using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using System.ComponentModel;
namespace BLEntities.Model
{
    public class RouteModel
    {
        public List<RouteStop> RouteStopList { get; set; }
        public List<RouteLine> RouteLineList { get; set; }
        public List<PhysicalStop> PhysicalStopList { get; set; }
        //public List<ISSTR> ISSTRList { get; set; }
    }
}
