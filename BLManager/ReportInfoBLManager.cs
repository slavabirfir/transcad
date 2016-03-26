using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Report;
using Utilities;
using BLEntities.Entities;
using BLEntities.Accessories;
using BLEntities.Model;

namespace BLManager
{
    /// <summary>
    /// Report Info BL Manager
    /// </summary>
    public class ReportInfoBLManager : IReportInfoBLManager
    {
        /// <summary>
        /// Get Route Stops Error Data
        /// </summary>
        /// <param name="lstData"></param>
        /// <returns></returns>
        public List<ErrorReportEntity> GetRouteStopsErrorData(List<RouteStop> lstData)
        {
            var repData = new List<ErrorReportEntity>();
            if (lstData.IsListFull())
            {
                foreach (var t in lstData)
                {
                    if (t.IsNewEntity)
                    {
                        repData.Add(new ErrorReportEntity
                        {
                            ID = string.IsNullOrEmpty(t.StationCatalog) ? string.Empty : t.StationCatalog,
                            Name = string.IsNullOrEmpty(t.Name) ? string.Empty : t.Name,
                            ErrorString = Resources.BLManagerResource.NewStation,
                            PropertyName = Resources.BLManagerResource.NewStation
                        });
                    }
                    else if (t.ValidationErrors.IsListFull())
                    {
                        var t1 = t;
                        var ps = GlobalData.RouteModel.PhysicalStopList.Find(p => p.ID == t1.PhysicalStopId);
                        if (ps != null)
                        {
                            foreach (var errorResult in t.ValidationErrors)
                            {
                                var result = errorResult;
                                if (!repData.Exists(p => p.ID == ps.StationCatalog &&
                                    p.Name == ps.Name &&
                                    p.ErrorString == result.ErrorMessage &&
                                    p.PropertyName == result.PropertyName
                                    ))
                                {
                                    repData.Add(new ErrorReportEntity
                                    {
                                        ID = ps.StationCatalog,
                                        Name = ps.Name,
                                        ErrorString = errorResult.ErrorMessage,
                                        PropertyName = errorResult.PropertyName
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return repData;
        }

        /// <summary>
        /// Get Route Lines Error Data
        /// </summary>
        /// <param name="lstData"></param>
        /// <returns></returns>
        public List<ErrorReportEntity> GetRouteLinesErrorData(List<RouteLine> lstData)
        {
            List<ErrorReportEntity> repData = new List<ErrorReportEntity>();
            if (lstData.IsListFull())
            {
                foreach (RouteLine t in lstData)
                {
                    if (t.ValidationErrors.IsListFull())
                    {
                        foreach (ValidationErrorResult errorResult in t.ValidationErrors)
                        {
                            repData.Add(new ErrorReportEntity
                            {
                                ID = t.Catalog.ToString(),
                                Name = t.Name,
                                ErrorString = errorResult.ErrorMessage,
                                PropertyName = errorResult.PropertyName
                            });
                        }
                    }
                }
            }
            return repData;
        }



        /// <summary>
        /// GetPhysicalStopsErrorData
        /// </summary>
        /// <param name="lstData"></param>
        /// <returns></returns>
        public List<ErrorReportEntity> GetPhysicalStopsErrorData(List<PhysicalStop> lstData)
        {
            List<ErrorReportEntity> repData = new List<ErrorReportEntity>();
            if (lstData.IsListFull())
            {
                foreach (PhysicalStop t in lstData)
                {
                    if (t.ValidationErrors.IsListFull())
                    {
                        foreach (ValidationErrorResult errorResult in t.ValidationErrors)
                        {
                            repData.Add(new ErrorReportEntity
                            {
                                ID = t.StationCatalog.ToString(),
                                Name = t.Name,
                                ErrorString = errorResult.ErrorMessage,
                                PropertyName = errorResult.PropertyName
                            });
                        }
                    }
                }
            }
            return repData;
        }

       
    }
}
