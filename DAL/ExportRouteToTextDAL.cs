using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using BLEntities.Entities;
using System.IO;
using Utilities;
using BLEntities.Accessories;
using Logger;
using BLEntities.Model;

namespace DAL
{
    public class ExportRouteToTextDal : IExportToTextDAL
    {
        private readonly string _delimiter = string.Empty;
        private readonly ITransCadMunipulationDataDAL _dal = new TransCadMunipulationDataDAL();
        private const string Windows1255 = "windows-1255";

        #region IExportToText Members
        /// <summary>
        /// Export Route Lines 
        /// </summary>
        /// <param name="list"></param>
        //"Operator",I,1,2,0,2,0,,"","",,"Blank",
        //"Cluster",I,3,3,0,3,0,,"","",,"Blank",
        //"MAKAT",I,6,7,0,7,0,,"","",,"Blank",
        //"Signpost",C,13,4,0,4,0,,"","",,"Blank",
        //"Dir",I,17,1,0,1,0,,"","",,"Blank",
        //"Var",I,18,2,0,2,0,,"","",,"Blank",
        //"Service_Type",I,20,2,0,2,0,,"","",,"Blank",
        //"Sug_Kav",I,22,2,0,2,0,,"","",,"Blank",
        //"Tipus_Kav",I,24,2,0,2,0,,"","",,"Blank",
        //"Hagdara",I,26,3,0,3,0,,"","",,"Blank",
        //"Main_District",I,29,4,0,4,0,,"","",,"Blank",
        //"Secondary_District",I,33,4,0,4,0,,"","",,"Blank",
        //"Road_Description",C,37,20,0,20,0,,"","",,"Blank",
        //"Unatiut",I,57,2,0,2,0,,"","",,"Blank",
        //"RouteLen",R,59,8,0,8,2,,"","",,"Blank",
        public void ExportRouteLines(List<RouteLine> list, string fileName, ref string message)
        {
           try
           {
            using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
            {
                if (list.IsListFull())
                    list.ForEach(it => sw.WriteLine(
                        String.Concat(
                         it.IdOperator.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                         it.IdCluster.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                         it.TextExportCatalog.ToExportFormatSpaceBefore(7), _delimiter,
                         it.Signpost.ToExportFormatSpaceBefore(4), _delimiter,
                         it.Dir.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                         it.VarNum.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                         it.IdServiceType.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                         
                         "1".ToExportFormatSpaceBefore(2), _delimiter, 
                         //it.Hagdara.ToString().ToExportFormat(3) , delimiter,
                         "0".ToExportFormatSpaceBefore(3), _delimiter,
                         it.IdZoneHead.ToString().ToExportFormatSpaceBefore(4), _delimiter, 
                         it.IdZoneSubHead.ToString().ToExportFormatSpaceBefore(4), _delimiter, 
                         //ExportRouteToSQLDAL.DEFAULTAREA.ToString().ToExportFormat(4), delimiter,
                         //ExportRouteToSQLDAL.DEFAULTAREA.ToString().ToExportFormat(4), delimiter,
                         it.RoadDescription.ToExportFormatSpaceBefore(20),_delimiter,
                         it.IdSeasonal.VerifyIntOnNullConst().ToExportFormatSpaceBefore(2), _delimiter, 
                         it.RouteLen.ToString().ToExportFormatSpaceBefore(8), _delimiter)  
                        )); 
                sw.Flush();
            }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }

        /// <summary>
        /// Export Route Stops
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        /// 100
        //"Operator",I,1,2,0,2,0,,"","",,"Blank",
        //"Cluster",I,3,3,0,3,0,,"","",,"Blank",
        //"Signpost",C,6,4,0,4,0,,"","",,"Blank",
        //"Dir",I,10,1,0,1,0,,"","",,"Blank",
        //"Var",I,11,2,0,2,0,,"","",,"Blank",
        //"StopID",I,13,7,0,7,0,,"","",,"Blank",
        //"StopSeq",I,20,3,0,3,0,,"","",,"Blank",
        //"StopKind",I,23,1,0,1,0,,"","",,"Blank",
        //"StopType",I,24,1,0,1,0,,"","",,"Blank",
        //"FAlight",I,25,7,0,7,0,,"","",,"Blank",
        //"DistPrev",I,32,7,0,7,0,,"","",,"Blank",
        //"DistStart",I,39,7,0,7,0,,"","",,"Blank",
        //"B-56",C,46,50,0,50,0,,"","",,"Blank",
        //"MAKAT",I,96,7,0,7,0,,"","",,"Blank",
        //MAKATSTATION,I,103,5,"","",,"Blank"
        public void ExportRouteStops(List<RouteLine> listRouteLines, List<RouteStop> listRouteStops,
            string fileName, ref string message)
        {
            try
            {
                if (!listRouteLines.IsListFull() || !listRouteStops.IsListFull())
                    return;
                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    listRouteLines.ForEach(delegate(RouteLine routeLine)
                    {
                        List<RouteStop> orderedList =
                        (from p in listRouteStops orderby p.Ordinal where p.RouteId == routeLine.ID select p).ToList<RouteStop>();
                        if (orderedList.IsListFull())
                        {
                            int distPrev = 0;
                            int distStart = 0;
                            for (int i = 0; i < orderedList.Count; i++)
                            {
                                RouteStop it = orderedList[i];
                                if (i > 0)
                                {
                                    distPrev = Convert.ToInt32((orderedList[i].Milepost - orderedList[i - 1].Milepost) * ExportConfiguration.ExportConfigurator.GetConfig().LengthConverter * GlobalConst.MileToKilometerCoeffitient);
                                    distStart += distPrev;
                                }
                                sw.WriteLine(String.Concat(
                                       it.RouteLine.IdOperator.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                       it.RouteLine.IdCluster.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                                       it.RouteLine.Signpost.ToExportFormatSpaceBefore(4), _delimiter,
                                       it.RouteLine.Dir.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                                       it.RouteLine.VarNum.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                       it.PhysicalStop.ID.ToString().ToExportFormatSpaceBefore(7), _delimiter,
                                       it.Ordinal.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                                       "2", _delimiter,
                                       it.IdStationType.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                                       it.Horada.ToString().ToExportFormatSpaceBefore(7), _delimiter,
                                       distPrev.ToString().ToExportFormatSpaceBefore(7), _delimiter,
                                       distStart.ToString().ToExportFormatSpaceBefore(7), _delimiter,
                                       string.Empty, _delimiter,
                                       it.RouteLine.TextExportCatalog.ToExportFormatSpaceBefore(7), _delimiter,
                                       it.PhysicalStop.StationCatalog.ToExportFormatSpaceBefore(5)));
                                //if (i == orderedList.Count - 1)
                                //{
                                //    sw.WriteLine(String.Concat(
                                //    it.RouteLine.IdOperator.ToString().ToExportFormat(2), delimiter,
                                //  it.RouteLine.IdCluster.ToString().ToExportFormat(3), delimiter,
                                //  it.RouteLine.Signpost.ToExportFormat(4), delimiter,
                                //  it.RouteLine.Dir.ToString().ToExportFormat(1), delimiter,
                                //  it.RouteLine.VarNum.ToExportFormat(2), delimiter,
                                //  it.PhysicalStop.ID.ToString().ToExportFormat(7), delimiter,
                                //  it.Ordinal.ToString().ToExportFormat(3), delimiter,
                                //  "2", delimiter,
                                //  it.IdStationType.ToString().ToExportFormat(7), delimiter,
                                //  it.Horada.ToString().ToExportFormat(7), delimiter,
                                //  distPrev.ToString().ToExportFormat(7), delimiter,
                                //  distStart.ToString().ToExportFormat(7), delimiter,
                                //  it.B_56.ToExportFormat(50), delimiter,
                                //  it.RouteLine.Catalog.ToString().ToExportFormat(5), delimiter));
                                //}
                            }
                        }
                    });
                    sw.Flush();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }
        
         
        /// <summary>
        /// Export Kv Cars
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        //* 17
        //"Operator",I,1,2,0,2,0,,"","",,"Blank",
        //"Cluster",I,3,3,0,3,0,,"","",,"Blank",
        //"MAKAT",C,6,7,0,7,0,,"","",,"Blank",
        //"Signpost",C,13,4,0,4,0,,"","",,"Blank",
        //"Dir",I,17,1,0,1,0,,"","",,"Blank",
        //"VarNum",I,18,2,0,2,0,,"","",,"Blank",
        //"VehicleTyp",I,20,2,0,2,0,,"","",,"Blank",
        //"VehicleSiz",I,22,2,0,2,0,,"","",,"Blank",

        public void ExportKvCars(List<RouteLine> list, string fileName, ref string message)
        {
            try
            {
              if (list.IsListFull())
                {
                  using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                   {
                       list.ForEach(it => sw.WriteLine(String.Concat(
                        it.IdOperator.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                        it.IdCluster.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                        it.TextExportCatalog.ToExportFormatSpaceBefore(7), _delimiter,
                        it.Signpost.ToExportFormatSpaceBefore(4), _delimiter,
                        it.Dir.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                        it.VarNum.ToString().ToExportFormatSpaceBefore(2), _delimiter ,
                        "2".ToExportFormatSpaceBefore(2),_delimiter,
                        "1".ToExportFormatSpaceBefore(2))));
                    sw.Flush();
                    sw.Close(); 
                  }
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        /// 42
        //    "Operator",I,1,2,0,2,0,,"","",,"Blank",
         //   "Cluster",I,3,3,0,3,0,,"","",,"Blank",
         //   "Signpost",C,6,4,0,4,0,,"","",,"Blank",
         //   "Dir",I,10,1,0,1,0,,"","",,"Blank",
         //   "Var",I,11,2,0,2,0,,"","",,"Blank",
         //   "NodeID",I,13,8,0,8,0,,"","",,"Blank",
         //   "NodeSeq",I,21,3,0,3,0,,"","",,"Blank",
         //  "DistPrev",I,24,7,0,7,0,,"","",,"Blank",
         //   "DistStart",I,31,7,0,7,0,,"","",,"Blank",
        ///    "MAKAT",C,38,7,0,7,0,,"","",,"Blank",
        public void ExportkvMaslul(List<RouteLine> list, string fileName, string routeLayerName,string linkLayer, ref string message)
        {
            try
            {
                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    list.ForEach(delegate(RouteLine it)
                    {
                        List<RouteLink> linkIds = _dal.GetRouteLinkList(it, routeLayerName, linkLayer);
                        if (linkIds.IsListFull())
                        {
                            int nodeSeq = 0;
                            int distPrev = 0;
                            int distStart = 0;
                            for (int i = 0; i < linkIds.Count; i++)
                            {
                                RouteLink routeLink = linkIds[i];
                                if (i > 0)
                                {
                                    distPrev = linkIds[i - 1].Length;
                                    distStart += linkIds[i - 1].Length;
                                }
                                sw.WriteLine(String.Concat(
                                it.IdOperator.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                it.IdCluster.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                                it.Signpost.ToExportFormatSpaceBefore(4), _delimiter,
                                it.Dir.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                                it.VarNum.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                routeLink.NodeIdFirst.ToString().ToExportFormatSpaceBefore(8), _delimiter,
                                (++nodeSeq).ToString().ToExportFormatSpaceBefore(3),
                                distPrev.ToString().ToExportFormatSpaceBefore(7),
                                distStart.ToString().ToExportFormatSpaceBefore(7),
                                it.TextExportCatalog.ToExportFormatSpaceBefore(7)));
                                if (i == linkIds.Count - 1)
                                {
                                    distPrev = linkIds[i].Length ;
                                    distStart += linkIds[i].Length;
                                    sw.WriteLine(String.Concat(
                                    it.IdOperator.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                    it.IdCluster.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                                    it.Signpost.ToExportFormatSpaceBefore(4), _delimiter,
                                    it.Dir.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                                    it.VarNum.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                    routeLink.NodeIdLast.ToString().ToExportFormatSpaceBefore(8), _delimiter,
                                    (++nodeSeq).ToString().ToExportFormatSpaceBefore(3),
                                    distPrev.ToString().ToExportFormatSpaceBefore(7),
                                    distStart.ToString().ToExportFormatSpaceBefore(7),
                                    it.TextExportCatalog.ToExportFormatSpaceBefore(7)));
                                }
                            }
                        }
                    });
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }
        
        //"Operator",I,1,2,0,2,0,,"","",,"Blank",
        //"Cluster",I,3,3,0,3,0,,"","",,"Blank",
        //"MAKAT",I,6,7,0,7,0,,"","",,"Blank",
        //"Dir",I,13,1,0,1,0,,"","",,"Blank",
        //"Var",I,14,2,0,2,0,,"","",,"Blank",
        //"MAKATSTATION",I,16,5,0,5,0,,"","",,"Blank",
        //"Duration",I,21,10,0,10,0,,"","",,"Blank",
        public void ExportRouteStopDuration(List<RouteLine> listRouteLines, List<RouteStop> listRouteStops, 
            string fileName, ref string message)
        {
            try
            {
                if (!listRouteLines.IsListFull() || !listRouteStops.IsListFull())
                    return;
                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    listRouteLines.ForEach(delegate(RouteLine routeLine)
                    {
                        List<RouteStop> orderedList =
                        (from p in listRouteStops orderby p.Ordinal where p.RouteId == routeLine.ID select p).ToList<RouteStop>();
                        if (orderedList.IsListFull())
                        {
                            int distPrev = 0;
                            int distStart = 0;
                            for (int i = 0; i < orderedList.Count; i++)
                            {
                                RouteStop it = orderedList[i];
                                if (i > 0)
                                {
                                    distPrev = Convert.ToInt32((orderedList[i].Milepost - orderedList[i - 1].Milepost) * ExportConfiguration.ExportConfigurator.GetConfig().LengthConverter * GlobalConst.MileToKilometerCoeffitient);
                                    distStart += distPrev;
                                }
                                sw.WriteLine(String.Concat(
                                       it.RouteLine.IdOperator.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                       it.RouteLine.IdCluster.ToString().ToExportFormatSpaceBefore(3), _delimiter,
                                       it.RouteLine.TextExportCatalog.ToExportFormatSpaceBefore(7), _delimiter,
                                       it.RouteLine.Dir.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                                       it.RouteLine.VarNum.ToString().ToExportFormatSpaceBefore(2), _delimiter,
                                       it.PhysicalStop.StationCatalog.ToExportFormatSpaceBefore(5), _delimiter,
                                       it.Duration.ToString().ToExportFormatSpaceBefore(10)
                                       //, delimiter,
                                       //it.LengthFromStart.ToString().ToExportFormatSpaceBefore(7), delimiter,
                                       //distStart.ToString().ToExportFormatSpaceBefore(7), delimiter,
                                       //it.Speed.ToString().ToExportFormatSpaceBefore(7)
                                       ));
                                }
                        }
                    });
                    sw.Flush();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }

        #endregion
    }
}
