using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace IDAL
{
    /// <summary>
    /// I Export To Text
    /// </summary>
    public interface IExportToTextDAL
    {
        
        #region Lines
        void ExportRouteLines(List<RouteLine> list, string fileName, ref string message);
        void ExportKvCars(List<RouteLine> list, string fileName, ref string message);
        void ExportRouteStops(List<RouteLine> listRouteLines, List<RouteStop> listRouteStops, string fileName, ref string message);
        void ExportkvMaslul(List<RouteLine> list, string fileName, string routeLayerName, string linkLayer, ref string message);
        void ExportRouteStopDuration(List<RouteLine> listRouteLines, List<RouteStop> listRouteStops, string fileName, ref string message);
        #endregion
    }
}
