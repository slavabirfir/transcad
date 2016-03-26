using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ExportConfiguration;
using Utilities;
using System.IO;
using IBLManager;
using BLEntities.Model;
using BLEntities.Entities;
using DAL;
using IDAL;
using BLEntities.Accessories;
using System.Data;
using Logger;
using BLEntities.SQLServer;
using DAL.SQLServerDAL;
using System.Data.SqlClient;
namespace BLManager
{
    /// <summary>
    /// Administration BL
    /// </summary>
    public class AdministrationBl
    {
        private const string Comma = ",";
        /// <summary>
        /// privates
        /// </summary>
        private readonly ITransCadBLManager _transCadBlManager = new TransCadBlManager();
        /// <summary>
        /// Decrypt Config File
        /// </summary>
        public void DecryptConfigFile()
        {
            const string configFile = "RouteExportProcess.EXE.config";
            AppConfigProtector.DecryptConfig();
            ProcessLauncher.RunProcess("explorer",
               Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), configFile));
        }
        /// <summary>
        /// Set Base Route Lines
        /// </summary>
        public void SetBaseRouteLines()
        {
            _transCadBlManager.SetBaseRouteLines();   
        }


        /// <summary>
        /// Create Converted File For Import
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="routeLineDistinct"></param>
        private static void CreateConvertedFileForImport(string fileName, List<RouteLine> routeLineDistinct)
        {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}",
                            "IdOperator", Comma,
                            "IdCluster", Comma,
                            "OLDCatalog", Comma,
                            "NEWCatalog"));
                routeLineDistinct.ForEach(rl => sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                                                            rl.IdOperator, Comma,
                                                                            rl.IdCluster, Comma,
                                                                            rl.CatalogBackUp, Comma,
                                                                            rl.Catalog)));
                IoHelper.WriteToFile(fileName, sb.ToString());
        }

        /// <summary>
        /// Create Converted File For Operator
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="routeLineDistinct"></param>
        private void CreateConvertedFileForOperator(string fileName, List<RouteLine> routeLineDistinct)
        {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                            "שם מפעיל", Comma,
                            "שם אשכול", Comma,
                            "מס' קו", Comma,
                            "מקט ישן", Comma,
                            "מקט חדש", Comma,
                            "עיר מוצא", Comma,
                            "עיר יעד"
                            ));
                routeLineDistinct.ForEach(rl =>
                {
                    var rsList = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == rl.ID);

                    var  sourceStation = rsList.Find(rs => rs.Ordinal==1);
                    var  destinationStation = rsList.Find(rs => rs.Ordinal == rsList.Count);

                    var sourceCity = _transCadBlManager.GetStopCity(sourceStation);
                    var destinationCity = _transCadBlManager.GetStopCity(destinationStation);
                    
                    sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                        GlobalData.LoginUser.UserOperator.OperatorName, Comma,
                        rl.ClusterName, Comma,
                        rl.RouteNumber, Comma,
                        rl.CatalogBackUp, Comma,
                        rl.Catalog, Comma,
                        sourceCity, Comma,
                        destinationCity
                       ));
                });
                IoHelper.WriteToFile(fileName, sb.ToString());
        }

        /// <summary>
        /// Convert Catalogs To 7 Positions
        /// </summary>
        /// <param name="folderName"></param>
        public void ConvertCatalogsTo7Positions(string folderName,ref string message)
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                if (!GlobalData.RouteModel.RouteLineList[0].Catalog.IsLengthLess7())
                {
                    message = Resources.BLManagerResource.CatalogIsUpdated;
                    return;
                }
                if (!_transCadBlManager.IsLayerConnectedToRS(ExportConfiguration.ExportConfigurator.GetConfig().MunLayer))
                {
                    message = Resources.BLManagerResource.IsMunNotConnected;
                    return;
                }
                
                _transCadBlManager.ConvertCatalogsTo7Positions();
                var routeLineDistinct = new List<RouteLine>();
                GlobalData.RouteModel.RouteLineList.ForEach(rl =>
                {
                    if (!routeLineDistinct.Exists(rlGroupBy => rlGroupBy.IdOperator == rl.IdOperator &&
                                                              rlGroupBy.IdCluster == rl.IdCluster &&
                                                              rlGroupBy.CatalogBackUp == rl.CatalogBackUp &&
                                                              rlGroupBy.Catalog == rl.Catalog))
                    {
                        routeLineDistinct.Add(rl);
                    }
                });
                string fileName = string.Format("{0}{1}{2}", "CatalogConvertedInfoForImport_", GlobalData.LoginUser.UserOperator.IdOperator, ".csv");
                CreateConvertedFileForImport(Path.Combine(folderName, fileName), routeLineDistinct);
                fileName = string.Format("{0}{1}{2}", "CatalogConvertedInfoForOperator_", GlobalData.LoginUser.UserOperator.IdOperator, ".csv");
                CreateConvertedFileForOperator(Path.Combine(folderName, fileName), routeLineDistinct);
            }
            else
                message = Resources.BLManagerResource.NonUpdatedList;
        }
        /// <summary>
        /// Calc Station Horada
        /// </summary>
        public void CalcStationHorada()
        {
            // Set Route and Phisical Stop Objects of RouteStopList
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                GlobalData.RouteModel.RouteLineList.ForEach(rl => _transCadBlManager.SetHoradaInSpecificRouteLine(rl));
            }
        }



        /// <summary>
        /// Update Physical Stops
        /// </summary>
        public void UpdateEndPoints()
        {
            ITransCadMunipulationDataDAL transCadMunipulationDataDal = new TransCadMunipulationDataDAL();
            transCadMunipulationDataDal.RunExportMacro("e",ExportConfigurator.GetConfig().UpdateEndPointsMacroUIFile, null);
        }
        /// <summary>
        /// Update EndPoints
        /// </summary>
        public void UpdatePhysicalStops()
        {
            var transCadMunipulationDataDal = new TransCadMunipulationDataDAL();
            transCadMunipulationDataDal.RunExportMacro("updPhysicalStops", ExportConfigurator.GetConfig().UpdatePhysicalStopsMacroUIFile, null);
        }
        private static List<JunctionVersionImport> GetlineJunctionfromCsv(RouteLine routeLine, IEnumerable<JunctionVersionImport> juncVersionList)
        {
            return ( 
            from p in juncVersionList 
            where p.Direction ==  routeLine.Dir.Value &&
                  p.OfficeLineId == routeLine.Catalog.Value  &&
                  p.LineAlternative == routeLine.Var
            select p).ToList<JunctionVersionImport>();

        }
        /// <summary>
        /// Is Lists Equal
        /// </summary>
        /// <param name="lineJunctionTranscad"></param>
        /// <param name="lineJunctionfromCSV"></param>
        /// <returns></returns>
        private static bool IsListsEqual(List<RouteLink> lineJunctionTranscad, List<JunctionVersionImport> lineJunctionfromCSV)
        {
            if (!lineJunctionTranscad.IsListFull() || !lineJunctionfromCSV.IsListFull())
                return false;
            else
            {
                if (lineJunctionTranscad.Count +1 != lineJunctionfromCSV.Count) return false;
                for(int i=0;i<lineJunctionTranscad.Count;i++)
                {
                    
                    if (i == lineJunctionTranscad.Count - 1)
                    {
                        if (lineJunctionfromCSV[i].JunctionId != lineJunctionTranscad[i].NodeIdFirst ||
                            lineJunctionfromCSV[i+1].JunctionId!= lineJunctionTranscad[i].NodeIdLast)
                            return false;

                    }
                    else
                    {
                        if (lineJunctionfromCSV[i].JunctionId != lineJunctionTranscad[i].NodeIdFirst)
                            return false;
                    }
                }
                return true;     
            }
        }

        /// <summary>
        /// GetJunctionVersionList
        /// </summary>
        /// <param name="fileNameFullPath"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        private static IEnumerable<JunctionVersionImport> GetJunctionVersionList(string fileNameFullPath, int operatorId)
        {
            var juncVersionList = new List<JunctionVersionImport>();
            using (var reader = new CsvReader(fileNameFullPath))
            {
                while (reader.ReadNextRecord())
                {
                    if (Convert.ToInt32(reader.Fields[0]) == operatorId)
                    {
                        juncVersionList.Add(new JunctionVersionImport
                        {
                            OperatorId = Convert.ToInt32(reader.Fields[0]),
                            OfficeLineId = Convert.ToInt32(reader.Fields[1]),
                            Direction = Convert.ToInt32(reader.Fields[2]),
                            LineAlternative = string.IsNullOrEmpty(reader.Fields[3]) ? string.Empty : StringHelper.ConvertToHebrewEncoding(reader.Fields[3]),
                            JunctionVersion = Convert.ToInt32(reader.Fields[4]),
                            JunctionOrder = Convert.ToInt32(reader.Fields[5]),
                            JunctionId = Convert.ToInt64(reader.Fields[6]),
                            DistanceFromPreviousJunction = Convert.ToInt32(reader.Fields[7]),
                            DistanceFromOriginJunction = Convert.ToInt32(reader.Fields[8])

                        });
                    }
                }
            }

            return juncVersionList;

        }
        

        /// <summary>
        /// Build Junction Version File
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="folder"></param>
        public void BuildJunctionVersionFileAndWriteInText(string filename, string folder)
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                string pathToExport = Path.Combine(folder, GlobalData.LoginUser.UserOperator.IdOperator.ToString());
                if (IoHelper.IsFolderExists(pathToExport)) Directory.Delete(pathToExport, true);
                IoHelper.CreateFolder(pathToExport);
                ITransCadMunipulationDataDAL transCadFetchData = new TransCadMunipulationDataDAL();
                ICSVFielOperation csvVFielOperation = new CSVFielOperationIml();
                var exportList = new List<JunctionVersionExport>();
                var juncVersionList = csvVFielOperation.GetJunctionVersionList(filename, GlobalData.LoginUser.UserOperator.IdOperator);
                GlobalData.RouteModel.RouteLineList.ForEach(routeLine =>
                {
                    List<RouteLink> lineJunctionTranscad = transCadFetchData.GetRouteLinkList(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MapLayerName);
                    List<JunctionVersionImport> lineJunctionfromCsv = GetlineJunctionfromCsv(routeLine, juncVersionList);
                    if (IsListsEqual(lineJunctionTranscad, lineJunctionfromCsv))
                    {
                        string exportFileName = Path.Combine(pathToExport, routeLine.ID.ToString());
                        exportList.Add(new JunctionVersionExport
                        {
                            Direction = lineJunctionfromCsv[0].Direction,
                            OperatorId = GlobalData.LoginUser.UserOperator.IdOperator,
                            JunctionVersion = lineJunctionfromCsv[0].JunctionVersion,
                            LineAlternative = lineJunctionfromCsv[0].LineAlternative,
                            IDRoute = routeLine.ID,
                            OfficeLineId = lineJunctionfromCsv[0].OfficeLineId,
                            PathToZipFile = exportFileName
                        });
                        IoHelper.CreateFolder(exportFileName);
                        transCadFetchData.SaveRouteZipFileInfo(routeLine.ID, StringHelper.ConvertToDefaultFrom1255(exportFileName), ExportConfigurator.GetConfig().RouteSystemLayerName);
                    }
                });
                transCadFetchData.RunExportMacro("j", ExportConfigurator.GetConfig().JunctionVersionUIFile, null);
                exportList.ForEach(e =>
                {
                    string zipDestinationFile = string.Concat(e.PathToZipFile, Zipper.ZIPEXTENTION);
                    Zipper.ZIPFoder(e.PathToZipFile, zipDestinationFile);
                });
                string fileNameExport = Path.Combine(folder, string.Format("JunctionVersion_{0}.csv", GlobalData.LoginUser.UserOperator.IdOperator));
                csvVFielOperation.SetJunctionVersionList(fileNameExport, exportList);
            }
        }
      
        public void BuildJunctionVersionFileAndWriteToSqlServer(string filename, string folder)
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                string pathToExport = Path.Combine(folder, GlobalData.LoginUser.UserOperator.IdOperator.ToString());
                if (IoHelper.IsFolderExists(pathToExport)) Directory.Delete(pathToExport, true);
                IoHelper.CreateFolder(pathToExport);
                var exportListSql = new List<JunctionVersionSQLServerExport>();
                var juncVersionList =GetJunctionVersionList(filename, GlobalData.LoginUser.UserOperator.IdOperator);
                DateTime now = DateTime.Now;
                ITransCadMunipulationDataDAL dalTransCad = new TransCadMunipulationDataDAL();
                GlobalData.RouteModel.RouteLineList.ForEach(routeLine =>
                    {
                        List<RouteLink> lineJunctionTranscad = dalTransCad.GetRouteLinkList(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MapLayerName);
                        List<JunctionVersionImport> lineJunctionfromCsv = GetlineJunctionfromCsv(routeLine, juncVersionList);
                        if (IsListsEqual(lineJunctionTranscad, lineJunctionfromCsv))
                        {
                            string exportFileName = Path.Combine(pathToExport, routeLine.ID.ToString());
                            var coorList = new List<Coord>();
                            Object data = dalTransCad.RunTranscadMacro(ExportConfigurator.GetConfig().RouteSystemPointsMacro, ExportConfigurator.GetConfig().RouteSystemPointsUI, new List<object>() { routeLine.ID });
                            if (data != null)
                            {
                                var coords = data.ToString().Split("|".ToCharArray());
                                for (int i = 0; i < coords.Length - 1; i++)
                                {
                                    var lonLat = coords[i].Split(",".ToCharArray());
                                    coorList.Add(new Coord { Longitude = int.Parse(lonLat[0]), Latitude = int.Parse(lonLat[1]) });
                                }
                            }
                            exportListSql.Add(new JunctionVersionSQLServerExport
                            {
                                JunctionVersion = lineJunctionfromCsv[0].JunctionVersion,
                                CreateDate = now,
                                ShapeGeomteryCoords = coorList,
                                Shape = null,
                                PathToZipFile = exportFileName
                            });
                            IoHelper.CreateFolder(exportFileName);
                            dalTransCad.SaveRouteZipFileInfo(routeLine.ID, StringHelper.ConvertToDefaultFrom1255(exportFileName), ExportConfigurator.GetConfig().RouteSystemLayerName);
                        }
                        else
                        {
                            dalTransCad.SaveRouteZipFileInfo(routeLine.ID, String.Empty, ExportConfigurator.GetConfig().RouteSystemLayerName);
                        }
                    });
                if (exportListSql.IsListFull())
                {
                    dalTransCad.RunExportMacro("j", ExportConfigurator.GetConfig().JunctionVersionUIFile, null);
                    exportListSql.ForEach(e =>
                    {
                        string zipDestinationFile = string.Concat(e.PathToZipFile, Zipper.ZIPEXTENTION);
                        Zipper.ZIPFoder(e.PathToZipFile, zipDestinationFile);
                        using (var fs = new FileStream(zipDestinationFile, FileMode.Open, FileAccess.Read))
                        {
                            var b = new Byte[fs.Length];
                            fs.Read(b, 0, b.Length);
                            e.Shape = b;
                        }
                    });
                    var connection = new SqlConnection(DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false));
                    SqlTransaction transaction = null;
                    try
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction((IsolationLevel)ExportConfigurator.GetConfig().TransactionLevel);
                        var dal = new JunctionVersionSQLServerExportDAL();
                        exportListSql.ForEach(e => dal.Save(e, connection, transaction));
                    }
                    catch (Exception exp)
                    {
                        if (transaction != null) transaction.Rollback();
                        LoggerManager.WriteToLog(exp);
                        throw;
                    }
                    finally
                    {
                        if (transaction != null) transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// ExportEggedStations
        /// </summary>
        /// <param name="filename"></param>
        public void ExportEggedStations(string filename)
        {
            if (_transCadBlManager.IsEgedOperator)
                GlobalData.RouteModel.PhysicalStopList = (new TransCadMunipulationDataDAL()).GetPhysicalStopsRawData();
            if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
            {
                try
                {
                    const string delimiter = ";";
                    const decimal  coefficient = 1000000;
                    var eggedFileName =Path.Combine(filename, string.Format("StationsEgged_{0}.txt", DateTime.Now.ToString("dd_MM_yyyy_hh_mm")));
                    using (var sw = new StreamWriter(eggedFileName, false,Encoding.GetEncoding(1255)))
                    {
                        // Get only approved stations and not trains
                        var onlyApprovedStations = GlobalData.RouteModel.PhysicalStopList.FindAll(st => st.IdStationStatus == 1 && st.IdStationType != 7 && st.IdStationType != 10);
                        if (onlyApprovedStations.IsListFull())
                        {
                            onlyApprovedStations.ForEach(it =>
                                sw.WriteLine(
                                String.Concat(
                                 it.StationCatalog.ToString(), delimiter,
                                 Convert.ToDecimal(it.Longitude / coefficient).ToString(), delimiter,
                                 Convert.ToDecimal(it.Latitude / coefficient).ToString(), delimiter,
                                 Convert.ToDecimal(it.LongitudeN / coefficient).ToString(), delimiter,
                                 Convert.ToDecimal(it.LatitudeN / coefficient).ToString(), delimiter,
                                 (it.Name.Replace("\"", string.Empty).Replace("'", string.Empty)), delimiter,
                                 (it.Street.Replace("\"", string.Empty).Replace("'", string.Empty)), delimiter,
                                 it.House.ToString(), delimiter,
                                 it.CityCode, delimiter,
                                 it.Across.ToString(), delimiter,
                                 it.LinkUserId.ToString(), delimiter,
                                 it.Direction, delimiter,
                                 it.IdStationType, delimiter,
                                 string.IsNullOrEmpty(it.StationShortName)? string.Empty :  it.StationShortName.Replace("\"", string.Empty).Replace("'", string.Empty)), delimiter
                                ));
                            sw.Flush();
                        }
                    }
                    ProcessLauncher.RunProcess("notepad", eggedFileName);
                }
                catch (Exception exp)
                {
                    LoggerManager.WriteToLog(exp);
                    throw;
                }
            }
        }
        /// <summary>
        /// ExportMapForCompare
        /// </summary>
        public void ExportMapForCompare()
        {
            var dal = new AdminOperationSqldal();
            var insertImportControlOperator = new InsertImportControlOperator
            {
                OperatorId = GlobalData.LoginUser.UserOperator.IdOperator,
                ImportStartDate = DateTime.Now
            };
            
            List<StationImport> stationImportList = null;
            var transCadMunipulationDataDal = new TransCadMunipulationDataDAL();
            var stopList = transCadMunipulationDataDal.GetStops(ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MunLayer, ExportConfigurator.GetConfig().MapLayerName);
            if (stopList.IsListFull())
            {
                DateTime now = DateTime.Now;  
                stationImportList = new List<StationImport>();
                stopList.ForEach(stop =>
                {
                    var staitonImport = new StationImport
                    {
                        CityId = stop.CityCode,
                        HouseNumber = stop.House.ToString(),
                        StreetName = stop.Street,
                        StationName = stop.Name,
                        Lat = stop.Lat,
                        Long = stop.Long,
                        B56Heb = string.Empty,
                        B56Eng = string.Empty,
                        StationId = stop.ID_SEKER,
                        LinkId = stop.LinkID,
                        StationCatalog = stop.ID_SEKER.ToString("00000"),
                        LatDifferrent = stop.LatDifferrent,
                        LongDifferrent = stop.LongDifferrent,
                        UpdateDate = now
                    };
                    stationImportList.Add(staitonImport);
                });
            }


            var routeJunctionImports = new List<LineDetailJunctionImport>() ;
            var routeStopImports = new List<LineDetailStationImport>(); 
            var routeImports = new List<LineDetailImport>();
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                GlobalData.RouteModel.RouteLineList.ForEach(routeLine =>
                {
                    routeImports.Add(new LineDetailImport
                    {
                        OperatorId = routeLine.IdOperator,
                        ClusterId = routeLine.IdCluster,
                        OfficeLineId = int.Parse(routeLine.Catalog.ToString()),
                        LineSign = routeLine.Signpost,
                        Direction = (byte)routeLine.Dir,
                        LineAlternative = routeLine.Var,
                        ServiceType = routeLine.IdServiceType,
                        LineDetailType = 1, // Urban Line
                        Line = (int)routeLine.RouteNumber,
                        DistrictId = routeLine.IdZoneHead,
                        DistrictSecId = routeLine.IdZoneSubHead,
                        IsBase = routeLine.IsBase
                    });
                    List<RouteStop> orderedList = (from p in GlobalData.RouteModel.RouteStopList orderby p.Ordinal where p.RouteId == routeLine.ID select p).ToList<RouteStop>();
                    if (orderedList.IsListFull())
                    {
                        int distPrev = 0, distStart = 0;

                        for (int i = 0; i < orderedList.Count; i++)
                        {
                            RouteStop it = orderedList[i];
                            if (i > 0)
                            {
                                distPrev = Convert.ToInt32((orderedList[i].Milepost - orderedList[i - 1].Milepost) * ExportConfiguration.ExportConfigurator.GetConfig().LengthConverter * GlobalConst.MileToKilometerCoeffitient);
                                distStart += distPrev;
                            }
                            routeStopImports.Add(new LineDetailStationImport
                            {
                                
                                OperatorId = routeLine.IdOperator,
                                OfficeLineId = int.Parse(routeLine.Catalog.ToString()),
                                Direction = (byte)routeLine.Dir,
                                LineAlternative = routeLine.Var,
                                StationId = Convert.ToInt64(it.PhysicalStop.StationCatalog),// it.PhysicalStop.ID,   it.PhysicalStop.StationCatalog
                                StationOrder = it.Ordinal,
                                StationType = 2,
                                StationActivityType = it.IdStationType ?? 0,
                                FirstDropStationId = it.Horada ?? 0,
                                DistanceFromPreviousStation = distPrev,
                                DistanceFromOriginStation = distStart,
                                Duration = it.Duration / 60, // convert to min
                                StationFloor = it.PhysicalStop.Floor,
                                StationPlatform = it.Platform 
                            });
                        }
                    }
                    List<RouteLink> linkIds = transCadMunipulationDataDal.GetRouteLinkList(routeLine, ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MapLayerName);
                    if (linkIds.IsListFull())
                    {
                        int nodeSeq = 0;
                        int distPrev = 0, distStart = 0;
                        for (int i = 0; i < linkIds.Count; i++)
                        {
                            RouteLink routeLink = linkIds[i];
                            if (i > 0)
                            {
                                distPrev = linkIds[i - 1].Length;
                                distStart += linkIds[i - 1].Length;
                            }
                            routeJunctionImports.Add(
                                new LineDetailJunctionImport
                                {
                                    OperatorId = routeLine.IdOperator,
                                    OfficeLineId = int.Parse(routeLine.Catalog.ToString()),
                                    Direction = (byte)routeLine.Dir,
                                    LineAlternative = routeLine.Var,
                                    JunctionId = routeLink.NodeIdFirst,
                                    JunctionOrder = (++nodeSeq),
                                    DistanceFromPreviousJunction = distPrev,
                                    DistanceFromOriginJunction = distStart
                                }
                            );
                            if (i == linkIds.Count - 1)
                            {
                                distPrev = linkIds[i].Length;
                                distStart += linkIds[i].Length;
                                routeJunctionImports.Add(
                                new LineDetailJunctionImport
                                {
                                    OperatorId = routeLine.IdOperator,
                                    OfficeLineId = int.Parse(routeLine.Catalog.ToString()),
                                    Direction = (byte)routeLine.Dir,
                                    LineAlternative = routeLine.Var,
                                    JunctionId = routeLink.NodeIdLast,
                                    JunctionOrder = (++nodeSeq),
                                    DistanceFromPreviousJunction = distPrev,
                                    DistanceFromOriginJunction = distStart
                                }
                            );
                            }
                        }
                    }
                });

            }
            dal.ExportToTranscadOperator(insertImportControlOperator, routeImports, routeJunctionImports, routeStopImports, stationImportList);
        }

        /// <summary>
        /// ExportEgedCSV
        /// </summary>
        
        /// <param name="folder"></param>
        public void ExportEgedCsv(string folder)
        {
            if (folder == null) throw new ArgumentNullException("folder");
            var egedDal = new EgedSQLServerDAL();
            var fullPathName = Path.Combine(folder, String.Concat("Eged_", DateTime.Now.ToStringddMMyyyyhhmmssNONSeparator()));
            IoHelper.CreateFolder(fullPathName);
            var nispah2FileName = Path.Combine(fullPathName,"egedNispah2.csv");
            var nispah1And3RepresentFileName = Path.Combine(fullPathName,"egedNispah1And3Reperesnt.csv");  
            // DAL
            // Get Nispah 2
            var dt2 = egedDal.GetNispah2();
            // Get 1 and 3 represent
            var dt1And3 = egedDal.GetNispah1And3Represent();
            BLSharedUtils.WriteCsvFile(nispah2FileName, dt2, Encoding.UTF8);
            BLSharedUtils.WriteCsvFile(nispah1And3RepresentFileName, dt1And3, Encoding.UTF8);
            ProcessLauncher.RunProcess("explorer", fullPathName);
        }
        /// <summary>
        /// Append Operator Route Systems
        /// </summary>
        public void AppendOperatorRouteSystems()
        {
            GlobalData.OperatorList = new TransportLicensingDal().GetOperatorList();

            if (GlobalData.OperatorList == null || !GlobalData.OperatorList.Any()) 
                return;

            var workOperList = GlobalData.OperatorList.Where(oper => oper.TransCadStatus == 1 && oper.OperatorGroup == EnmOperatorGroup.Operator &&
                               !string.IsNullOrEmpty(oper.SelectedTranscadClusterConfig.PathToRSTFile) && File.Exists(oper.SelectedTranscadClusterConfig.PathToRSTFile)).ToList();

            if (!workOperList.Any()) 
                return;
            
            var options = new object[2] { new object[2] { "ReadOnly", "True" }, new object[2] { "Shared", "False" } };

            workOperList.ForEach(o => new TransCadMunipulationDataDAL().AddRouteSystemLayer(ExportConfigurator.GetConfig().MapLayerName,
                         string.Format("RS Operator {0}", o.IdOperator), o.SelectedTranscadClusterConfig.PathToRSTFile, options));
        }



        private static void ExecuteCommandSync(string fileName, string args)
        {
            var thisProcess = new Process { StartInfo = { CreateNoWindow = false, WindowStyle = ProcessWindowStyle.Normal, FileName = fileName, Arguments = args }};
            thisProcess.StartInfo.UseShellExecute = false; 
            thisProcess.StartInfo.CreateNoWindow = true ; 
            thisProcess.Start();
            thisProcess.WaitForExit();
        }

        private static void ExportIsstrAsShape(DirectoryInfo shapeFileDestination, ITransCadMunipulationDataDAL dalTransCad, string shapeFileDestinationFileName)
        {
            dalTransCad.RunExportMacro(ExportConfigurator.GetConfig().ConvertIsStrShapeToTabMacro, ExportConfigurator.GetConfig().ConvertIsStrShapeToTabMacroUiFile, new List<object>() { shapeFileDestinationFileName, ExportConfigurator.GetConfig().MapLayerName });
            var isstrPrj = Path.Combine(ExportConfigurator.GetConfig().DataFolder, "isstr.prj");
            if (File.Exists(isstrPrj))
            {
                IoHelper.CopyFile(isstrPrj, Path.Combine(shapeFileDestination.FullName, "isstr.prj"));
                return; 
            }
            throw new ApplicationException(string.Format("The isstr.prj file was not found into the Data folder {0}",isstrPrj));
        }

        private static string ConvertToTabAndReturnZipFile(FileSystemInfo mainDir, FileSystemInfo targetTab, ITransCadMunipulationDataDAL dalTransCad, string targetTabFileName, string shapeFileDestinationFileName)
        {
            var isstrFiels = dalTransCad.GetLayerFields(ExportConfigurator.GetConfig().MapLayerName);
            var args = string.Format("\"{0}\" \"{1}\"", targetTabFileName, shapeFileDestinationFileName);
            ExecuteCommandSync(ExportConfigurator.GetConfig().ConvertIsStrShapeToTaBogr2OgrPath, args);
            var tabFile = Path.Combine(targetTab.FullName, "isstr.tab");
            const string strSteet = "STREET Char (60)";
            const string egedSteet = "STREET Char (40)";
            if (File.Exists(tabFile) && isstrFiels.Any())
            {
                var textAll = File.ReadAllText(tabFile);
                var isstrFielsMoreTen = isstrFiels.Where(i => i.Length >= 10).Select(i => i);
                if (isstrFielsMoreTen.Any())
                {
                    var all = textAll;
                    isstrFielsMoreTen.ToList().ForEach(el =>
                    {
                        all = all.Replace(string.Format("{0}1", el.Substring(0, 9)), el);
                        all = all.Replace(string.Format("{0}", el.Substring(0, 10)), el);
                    });
                    textAll = textAll.Replace(strSteet, egedSteet);
                    File.WriteAllText(tabFile, textAll);
                }
                var zipFile = string.Format("isstrtab_{0}.zip", DateTime.Now.ToString("ddMMyyyy hhmmss"));
                var zipFileDestinationFolder = Path.Combine(mainDir.FullName, zipFile);
                Zipper.ZIPFoder(targetTab.FullName, zipFileDestinationFolder);
                return zipFileDestinationFolder;
            }
            return string.Empty;  
        }

        public string ConvertIstrShapeToTabAndGetZipFile()
        {
            var folder = Path.Combine(ExportConfigurator.GetConfig().DataFolder, string.Format("SHAPE_TO_TAB\\{0}", DateTime.Now.ToString("ddMMyyyy hhmmss")));
            var di = Directory.CreateDirectory(folder);
            var shapeFileDestination = Directory.CreateDirectory(Path.Combine(di.FullName, "SHP"));
            var shapeFileDestinationFileName = Path.Combine(shapeFileDestination.FullName, "isstr.shp");
            var targetTab = Directory.CreateDirectory(Path.Combine(di.FullName, "TAB"));
            var targetTabFileName = Path.Combine(targetTab.FullName, "isstr.tab");
            var dalTransCad = new TransCadMunipulationDataDAL();
            ExportIsstrAsShape(shapeFileDestination, dalTransCad, shapeFileDestinationFileName);
            var zipFile = ConvertToTabAndReturnZipFile(di, targetTab, dalTransCad, targetTabFileName, shapeFileDestinationFileName);
            if (!string.IsNullOrEmpty(ExportConfigurator.GetConfig().ConvertIsStrShapeToTabFtpHost))
            {
                SendZipByFtp(zipFile);
                Directory.Delete(di.FullName, true);
            }
            return Path.GetFileName(zipFile);
        }


        private static void SendZipByFtp(string zipFile)
        {
            var ftpClient = new Ftp(ExportConfigurator.GetConfig().ConvertIsStrShapeToTabFtpHost,
                                    ExportConfigurator.GetConfig().ConvertIsStrShapeToTabFtpUser,
                                    ExportConfigurator.GetConfig().ConvertIsStrShapeToTabFtpPassword);

            ftpClient.UploadBigFile(zipFile);//ConfigurationManager.AppSettings["localfile"]);
        }

        public bool VerifyBasedMap()
        {
            DataTable errStationDt = CreateErrStationDataTable();
            SetStationErrors(errStationDt);
            var status = true;
            if (errStationDt.IsDataTableFull())
            {
                status = false;
                BuildErrorStationReport(errStationDt);
            }
            SaveErrorStationStatusToTable(status);
            return status;
        }

        private static void SaveErrorStationStatusToTable(bool status)
        {
            IInternalBaseDal internalBaseDal = new InternalTvunaImplementationDal();
            internalBaseDal.InsertPhysicalLayerTestJurnal(GlobalData.LoginUser.UserOperator.IdOperator,
                                                          Environment.UserName, status);
        }

        private static void BuildErrorStationReport(DataTable errStationDt)
        {
            using (var writer = new CsvWriter())
            {
                var outputCsvFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), string.Format("StationErrorReport_{0}.csv", DateTime.Now.ToString("ddMMyyyy_hh_mm_ss")));
                writer.WriteCsv(errStationDt, outputCsvFileName, Encoding.UTF8);
                ProcessLauncher.RunProcess("notepad.exe", "\"" + outputCsvFileName + "\"");
            }
        }

        private static DataTable CreateErrStationDataTable()
        {
            var errStationDt = new DataTable();
            errStationDt.Columns.Add("סוג מפעיל", typeof (string));
            errStationDt.Columns.Add("מקט תחנה", typeof(string));
            errStationDt.Columns.Add("שם תחנה", typeof(string));
            errStationDt.Columns.Add("שדה שגוי", typeof(string));
            errStationDt.Columns.Add("שגיאה", typeof(string));
            return errStationDt;
        }

        private static void SetStationErrors(DataTable errStationDt)
        {
           var errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps=>string.IsNullOrEmpty(ps.Name));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "שם תחנה", "שם תחנה ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => !string.IsNullOrEmpty(ps.Name) && ps.Name.Length > 16 && string.IsNullOrEmpty(ps.StationShortName));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "שם תחנה", "אורך של שדה שם תחנה יותר גדול מי 16 תוים"));
          
           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => string.IsNullOrEmpty(ps.StationCatalog));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "מקט תחנה", "מקט תחנה ריק"));
           
           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => string.IsNullOrEmpty(ps.CityCode));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "קוד ישוב", "קוד ישוב ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.LinkId==0);
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "קוד לינק מפה בו שייכת תחנה", "קוד ישובקוד לינק מפה בו שייכת תחנה ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.LongitudeN == 0 || ps.LatitudeN == 0);
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "קואורדינטות בהיסט", "קקואורדינטות בהיסט ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.IdStationType == 0);
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "סוג תחנה", "סוג תחנה ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.IdStationStatus == 0);
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "סטאטוס תחנה", "סטאטוס תחנה ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => string.IsNullOrEmpty(ps.StationShortName));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "שם תחנה קצר", "שם תחנה קצר ריק"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.CityLinkCode==0);
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "FromPlatform & ToPlatform", "שדות FromPlatform ו ToPlatform חייבים להיות עם ערכים"));

           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.IdStationType == 8 && (!ps.FromPlatform.HasValue || !ps.ToPlatform.HasValue));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "האם תחנה עברה טיוב ידני בשיוך לאזור מוניציפאל", "האם תחנה עברה טיוב ידני בשיוך לאזור מוניציפאל ריק"));
           
           errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => (ps.IdStationType == 7 || ps.IdStationType == 4) && ps.Street != "מסילת ברזל");
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "רחוב", "חייב להיות עם ערך - מסילת ברזל"));

            errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.IdStationStatus > 2 && GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm);
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "סטאטוס תחנה של חברת תכנון", "תחנות מאושרות ומתוכננות רק לקבוצת חברות תכנון"));

            errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => (ps.IdStationStatus == 2 && GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.Operator));
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "סטאטוס תחנה של מפעיל", "תחנות מאושרות למפעילים,זו תחנה בתכנון "));

            errStations = GlobalData.RouteModel.PhysicalStopList.Where(ps => ps.IdStationType == 8 && !BLSharedUtils.IsTaxiOperator());
           if (errStations.Any())
               errStations.ToList().ForEach(ps => AddRowToErrStationDataTable(errStationDt, ps, "סוג תחנה", "תחנת מוניות שירות רק לקבוצת חברות מוניות "));
 
        }

        private static string GetOperatorType()
        {
            const string operators = "מפעילים";
            const string planningFirm = "חברת תכנון";
            const string taxi = "מוניות";
            if(GlobalData.LoginUser.UserOperator.OperatorGroup==EnmOperatorGroup.PlanningFirm)
            {
                return BLSharedUtils.IsTaxiOperator() ? taxi : planningFirm;
            }
            return operators;
        }

        private static void AddRowToErrStationDataTable(DataTable errStationDt,PhysicalStop physicalStop,string field,string errMessage)
        {
            var errData = new object[]
             {
                 GetOperatorType(),
                 physicalStop.StationCatalog,
                 physicalStop.Name,
                 field,
                 errMessage
             };
            errStationDt.Rows.Add(errData);
        }
    }
}
