using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BLEntities.Accessories;
using BLEntities.Entities;
using DAL;
using ExportConfiguration;
using IDAL;
using Utilities;
using BLEntities.Model;
using System.IO;

namespace BLManager
{
    public class BlComparePriceAreaOfStation
    {
        private List<StationAreaPriceCompareInputData> _stationAreaPriceCompareList;
        
        private readonly ITransCadMunipulationDataDAL _dalTranscad;

        public BlComparePriceAreaOfStation()
        {
            _dalTranscad = new TransCadMunipulationDataDAL();
        }

        public bool CheckDataValidity(string inputCsVfileName,ref string  errorMessage)
        {
            var hasErrors = false;
            var sb = new StringBuilder();
            _stationAreaPriceCompareList = new List<StationAreaPriceCompareInputData>();
            using (var reader = new CsvReader(inputCsVfileName,Encoding.UTF8))
            {
                while (reader.ReadNextRecord())
                {
                    var stationCatalogFromFile = Convert.ToString(reader.Fields[0]);
                    if (!string.IsNullOrEmpty(stationCatalogFromFile) && !Validators.IsNumeric(stationCatalogFromFile))
                    {
                        reader.ReadNextRecord();
                        stationCatalogFromFile = Convert.ToString(reader.Fields[0]);
                    }
                    
                    var unionStation = reader.FieldCount == 4 ? Convert.ToString(reader.Fields[3]) : string.Empty;
                    if (!string.IsNullOrEmpty(unionStation))
                    {
                        var physicalStopUnionStation = GlobalData.RouteModel.PhysicalStopList.SingleOrDefault(p => p.StationCatalog == unionStation);
                        if (physicalStopUnionStation == null)
                        {
                            sb.AppendFormat("{0} ", unionStation);
                            hasErrors = true;
                        }
                    }

                    var physicalStop = GlobalData.RouteModel.PhysicalStopList.SingleOrDefault(p => p.StationCatalog == stationCatalogFromFile);
                    if (physicalStop==null)
                    {
                        sb.AppendFormat("{0} ", stationCatalogFromFile);
                        hasErrors = true ; 
                    }
                    else
                    {
                        int longitude = (reader.Fields[1] == null || !Validators.IsNumeric(reader.Fields[1])) ? 0 : Convert.ToInt32(reader.Fields[1]);
                        int latitude = (reader.Fields[2] == null || !Validators.IsNumeric(reader.Fields[2])) ? 0 : Convert.ToInt32(reader.Fields[2]);
                        _stationAreaPriceCompareList.Add(new StationAreaPriceCompareInputData
                        {
                            StationCatalog = stationCatalogFromFile,
                            NewCoord = new Coord { Longitude = longitude, Latitude = latitude },
                            CurrentCoord = new Coord { Longitude = physicalStop.Longitude, Latitude = physicalStop.Latitude },
                            UnionStation = unionStation  
                        });
                    }
                }
            }
            if (hasErrors)
            {
                errorMessage = String.Concat(Resources.BLManagerResource.ListOfStationNotFoundInTranscad, sb.ToString()); 
                return false;
            }
            if (!_stationAreaPriceCompareList.IsListFull())
            {
                errorMessage = Resources.BLManagerResource.StationNotDefinedInImportFile;
                return false;
            }
            return true;
        }

        private int GetPriceAreaId(PhysicalStop physicalStop, IEnumerable<PriceArea> dataPriceArea,string layerName)
        {
            var prAreaId = _dalTranscad.GetNearestIdPriceAreaRecordToPhysicalStop(physicalStop, layerName, true, "ZoneCode");
            if (prAreaId > 0)
            {
                var pa = dataPriceArea.SingleOrDefault(p => p.ID == prAreaId);
                if (pa != null)
                    return pa.ID;
            }
            return 0;
        }

        private IEnumerable<StationAreaPriceCompareOutputData> GetStationAreaPriceCompareOutputData(string layerName)
        {
               var oper = GlobalData.OperatorList.SingleOrDefault(op => op.IdOperator == Convert.ToInt32(layerName));
               if (oper==null)
               {
                   throw new ApplicationException(string.Format("Layer name {0} is not valid operator id found into the OperatorList.xml file",layerName));
               }
               var  listOutPutData = new List<StationAreaPriceCompareOutputData>();
               var dataPriceArea = _dalTranscad.GetAllPriceListPolygon(layerName);
              
               _stationAreaPriceCompareList.ForEach(sc =>
               {
                    var temporaryCurrentPs = new PhysicalStop { Longitude = sc.CurrentCoord.Longitude,Latitude = sc.CurrentCoord.Latitude };
                    var temporaryNewPs = new PhysicalStop { Longitude = sc.NewCoord.Longitude, Latitude = sc.NewCoord.Latitude };
                    listOutPutData.Add(new StationAreaPriceCompareOutputData
                                           {
                                               CurrentIdPriceArea = GetPriceAreaId(temporaryCurrentPs, dataPriceArea, layerName),
                                               NewIdPriceArea = GetPriceAreaId(temporaryNewPs, dataPriceArea, layerName),
                                               StationCatalog = sc.StationCatalog,
                                               OperatorId = oper.IdOperator,
                                               OperatorName = oper.OperatorName,
                                               UnionStation = sc.UnionStation
                                           });

               });
               return listOutPutData; 
        }

        public void BuildReport(string outputFolder)
        {
            if (_stationAreaPriceCompareList.IsListFull())
            {
                _dalTranscad.OpenWorkspace(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsFileName);
                var priceListLayers = _dalTranscad.GetMapLayers(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName, "Area");
                if (priceListLayers.IsListFull())
                {
                    var stationAreaPriceCompareOutputDataResult = new List<StationAreaPriceCompareOutputData>();
                    priceListLayers.ForEach(layerName => stationAreaPriceCompareOutputDataResult.AddRange(GetStationAreaPriceCompareOutputData(layerName)));
                    
                    using (var writer = new CsvWriter())
                    {
                        var outputCsvFileName = Path.Combine(outputFolder,string.Format("{0}_{1}.csv", "StationsReport",DateTime.Now.ToString("ddMMyyyy_hh_mm_ss")));
                        writer.WriteCsv(BuildDTfromStationAreaPriceCompareList(stationAreaPriceCompareOutputDataResult), outputCsvFileName, Encoding.UTF8);
                        ProcessLauncher.RunProcess("notepad.exe", "\"" + outputCsvFileName + "\"");
                    }
                    _dalTranscad.CloseMap(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName);
                }
                else
                {
                    throw new ApplicationException("The Price Area Operator List doesn't have any layer");
                }
            }
            
        }

        private static DataTable BuildDTfromStationAreaPriceCompareList(List<StationAreaPriceCompareOutputData> stationAreaPriceCompareOutputDataResult)
        {
            var dt = new DataTable();
            dt.Columns.Add("OperatorId", typeof(int));
            dt.Columns.Add("OperatorName", typeof(string));
            dt.Columns.Add("StationCatalog", typeof(string));
            dt.Columns.Add("IdAreaCurrent", typeof(int));
            dt.Columns.Add("IdAreaNew", typeof(int));
            dt.Columns.Add("Remarks", typeof(string));
            stationAreaPriceCompareOutputDataResult.ForEach(sc =>
            {
                var dr = dt.NewRow();
                dr["OperatorId"] = sc.OperatorId;
                dr["OperatorName"] = sc.OperatorName;
                dr["StationCatalog"] = sc.StationCatalog;
                dr["IdAreaCurrent"] = sc.CurrentIdPriceArea;
                dr["IdAreaNew"] = sc.NewIdPriceArea;
                if (sc.CurrentIdPriceArea != sc.NewIdPriceArea)
                {
                    var sb = new StringBuilder();
                    
                    if (sc.CurrentIdPriceArea > 0 && sc.NewIdPriceArea>0 && string.IsNullOrEmpty(sc.UnionStation))
                        sb.AppendFormat("{0} ", Resources.BLManagerResource.StationAreaProceWillBeChanged);

                    if (sc.CurrentIdPriceArea > 0 && sc.NewIdPriceArea > 0 && !string.IsNullOrEmpty(sc.UnionStation))
                        sb.AppendFormat("{0} ",string.Format(Resources.BLManagerResource.StationCanceledWillMovedToNewArea,sc.UnionStation, sc.NewIdPriceArea.ToString()));

                    
                    if (sc.CurrentIdPriceArea > 0 && sc.NewIdPriceArea == 0 && !string.IsNullOrEmpty(sc.UnionStation))
                        sb.AppendFormat("{0} ", string.Format(Resources.BLManagerResource.StationCanceledWillMovedToNonArea, sc.UnionStation));


                    dr["Remarks"] = sb.ToString();
                    dt.Rows.Add(dr);
                }
                else if (sc.CurrentIdPriceArea == sc.NewIdPriceArea && sc.CurrentIdPriceArea == 0)
                {
                    dr["Remarks"] = Resources.BLManagerResource.StationDoesnotHasNewAreaPriceAndOldPriceArea;
                    dt.Rows.Add(dr);
                }
            });
            dt.AcceptChanges();
            return dt;
        }

    }
}
