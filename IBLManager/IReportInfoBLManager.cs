using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using BLEntities.Report;

namespace IBLManager
{
    public interface IReportInfoBLManager 
    {
        List<ErrorReportEntity> GetRouteStopsErrorData(List<RouteStop> lstData);
        List<ErrorReportEntity> GetRouteLinesErrorData(List<RouteLine> lstData);
        List<ErrorReportEntity> GetPhysicalStopsErrorData(List<PhysicalStop> lstData); 
    }
}
