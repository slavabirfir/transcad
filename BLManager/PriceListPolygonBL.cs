using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using IDAL;
using DAL;
using Utilities;
using ExportConfiguration;
using System.IO;
using BLEntities.Model;
using System.Data;
using DAL.SQLServerDAL;
using System.Data.SqlClient;
using Logger;
using BLEntities.SQLServer;

namespace BLManager
{
    public class PriceListPolygonBl
    {
        private const string Physicalstationnonarea = "PhysicalStationNoneArea.csv";
        private const string Stationcatalog = "StationCatalog";
        public bool IsCanceledByUser { get; set; }
        public event EventHandler<EventArgs> Changed;
        public List<PriceArea> DataPriceArea { get; set; }

        readonly ITransCadMunipulationDataDAL _dalTranscad = new TransCadMunipulationDataDAL();
        /// <summary>
        /// .ctor
        /// </summary>
        public PriceListPolygonBl()
        {
            DataPriceArea = _dalTranscad.GetAllPriceListPolygon(ExportConfigurator.GetConfig().PriceAreaPolygonName);
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="term"></param>
        public List<PriceArea> Search(String term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return DataPriceArea;
            }
            else
            {
                return  (from p in DataPriceArea
                                       where !string.IsNullOrEmpty(p.Name) && p.Name.IndexOf(term) >= 0
                                       select p).ToList();
            }
        }



        /// <summary>
        /// GetChangedAreaList
        /// </summary>
        /// <returns></returns>
        public List<PriceArea> GetChangedAreaListAndUpdateZeroZoneCodes()
        {
                var changedPriceArea = new List<PriceArea>();
                string connectionString = DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false);
                var connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    var dalPriceArea = new PriceImportDAL();
                    DataPriceArea.ForEach(p =>
                    {
                        if (p.ID==0)
                        {
                            p.ID = dalPriceArea.GetMaxNumInUseByAutoNumKod(1, connection);
                            _dalTranscad.UpdatePriceListPolygon(p, ExportConfigurator.GetConfig().PriceAreaPolygonName);
                        }
                        if (dalPriceArea.IsAreaWasChanged(p, connection))
                        {
                            changedPriceArea.Add(p);
                        }
                    });
                    return changedPriceArea;
                }
                catch (Exception exp)
                {
                    LoggerManager.WriteToLog(exp);
                    throw (exp);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
        }

        private DataTable GetPriceListWithEmptyDescriptionsOrEmptyZoneCode(List<PriceArea> priceAreaList)
        {
            DataTable dt = new DataTable();
            const string PRICETRANSCADID = "PriceTranscadID";
            dt.Columns.Add(PRICETRANSCADID, typeof(string));
            if (priceAreaList.IsListFull())
            {
                List<PriceArea> emptyList = priceAreaList.FindAll(p => p.ID == 0 || string.IsNullOrEmpty(p.Name));
                if (emptyList.IsListFull())
                {
                    emptyList.ForEach(st =>
                    {
                        DataRow dr = dt.NewRow();
                        dr[PRICETRANSCADID] = st.TranscadId;
                        dt.Rows.Add(dr);
                    });
                }
            }
            return dt;
        }

        /// <summary>
        /// Get Phisical Stop Without Area
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable GetPhisicalStopWithoutArea(List<PhysicalStop> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(Stationcatalog, typeof(string));
            if (list.IsListFull())
            {
                list.ForEach(st =>
                {
                    if (GlobalData.RouteModel.RouteStopList.Exists(rs => rs.PhysicalStopId == st.ID ))
                    {
                        DataRow dr = dt.NewRow();
                        dr[Stationcatalog] = st.StationCatalog;
                        dt.Rows.Add(dr);
                    }
                });
            }
            return dt;
        }

        private void RunChanged()
        {
            if (Changed != null)
            {
                Changed(null, null);
                if (IsCanceledByUser)
                    throw new OperationCanceledException();
            }
        }


        /// <summary>
        /// ExportPriceList
        /// </summary>

        /// <summary>
        /// ExportPriceList
        /// </summary>
        public bool ExportPriceListDb(ref string message)
        {
            LoggerManager.WriteToLog(" Export Price List DB Starting ");
            var psListFiltered = new List<PhysicalStop>();
            try
            {
                RunChanged();
                DataPriceArea = _dalTranscad.GetAllPriceListPolygon(ExportConfigurator.GetConfig().PriceAreaPolygonName);
                var dtEmptyPrices = GetPriceListWithEmptyDescriptionsOrEmptyZoneCode(DataPriceArea);
                RunChanged();
                if (dtEmptyPrices.IsDataTableFull())
                {
                    message = Resources.BLManagerResource.PriceListWithEmptyCodeOrDesc;
                    return false;
                }
                if (!DataPriceArea.IsListFull())
                {
                    message = "Price List is empty";
                    return false;
                }
                if (!GlobalData.RouteModel.PhysicalStopList.IsListFull())
                {
                    message = "PhysicalStopList List is empty";
                    return false;
                }
                var macroName = ExportConfigurator.GetConfig().PriceAreaPolygonMacroName;

                var exportShapeFolder = Path.Combine(ExportConfigurator.GetConfig().PriceAreaPolygonFileFolder,
                                                        GlobalData.LoginUser.UserOperator.IdOperator.ToString());
                if (IoHelper.IsFolderExists(exportShapeFolder))
                {
                    IoHelper.DeleteDirectoryFolders(new DirectoryInfo(exportShapeFolder));
                    IoHelper.DeleteFiles(exportShapeFolder, "*.*", false);
                }
                else
                    IoHelper.CreateFolder(exportShapeFolder);

                // Only station that are not train stations
                GlobalData.RouteModel.PhysicalStopList.Where(p=>p.IdStationType!=7).ToList().ForEach(ps =>
                {
                    var prAreaId = _dalTranscad.GetNearestIdPriceAreaRecordToPhysicalStop(ps, ExportConfigurator.GetConfig().PriceAreaPolygonName, true, "ZoneCode");
                       if (prAreaId > 0)
                       {
                           var pa = DataPriceArea.FindLast(p => p.ID == prAreaId);
                           RunChanged();
                           if (pa != null && ps.IdStationStatus == 1)
                           {
                               ps.PriceArea = pa;
                               psListFiltered.Add(ps);
                           }
                       }
                });

                _dalTranscad.RunExportMacro(macroName, ExportConfigurator.GetConfig().PriceAreaPolygonMacroUIFile, new List<object>() { exportShapeFolder, 
                    ExportConfigurator.GetConfig().PriceAreaPolygonName });
               DataPriceArea.ForEach(p =>{
                      RunChanged();
                      string pattern = string.Format("{0}\\{1}", exportShapeFolder, p.TranscadId);
                      p.ShapeFilePath = string.Concat(pattern, Zipper.ZIPEXTENTION);
                      Zipper.ZIPFoder(pattern, p.ShapeFilePath);

                      using (var fs = new FileStream(p.ShapeFilePath, FileMode.Open,FileAccess.Read))
                      {
                          var b = new Byte[fs.Length];
                          fs.Read(b, 0, b.Length);
                          p.ShapeFile = b;
                      }
                      Object data = _dalTranscad.RunTranscadMacro(ExportConfigurator.GetConfig().PolygonCalcAreaMacroName,
                              ExportConfigurator.GetConfig().PolygonCalcAreaUI,
                              new List<object>() { p.TranscadId, ExportConfigurator.GetConfig().PriceAreaPolygonName });
                      if (data != null)
                      {
                          p.Coords = new List<Coord>();
                          string[] coords = data.ToString().Split("|".ToCharArray());
                          for (int i = 0; i < coords.Length - 1; i++)
                          {
                              var lonLat = coords[i].Split(",".ToCharArray());
                              var coor = new Coord
                                               {
                                                   Longitude = int.Parse(lonLat[0]),
                                                   Latitude = int.Parse(lonLat[1])
                                               };
                              p.Coords.Add(coor);
                          }
                      }
                 });
            }
            catch(Exception exp)
            {
                if (exp is OperationCanceledException)
                {
                    message = Resources.BLManagerResource.OperationCanceledByUser;
                    _dalTranscad.CloseConnection();
                    return false;
                }
                throw;
            }
            if (psListFiltered.IsListFull())
            {
                var connectionString = DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false);
                var connection = new SqlConnection(connectionString);
                var controllerEntity = new ExternalImport_InsertImportZoneControlOperator { OperatorId = GlobalData.LoginUser.UserOperator.IdOperator, FromDate = DateTime.MinValue };
                var dalController = new ExternalImport_InsertImportZoneControlOperatorDAL();
                try
                {
                    connection.Open();

                    dalController.Add(controllerEntity, connection);
                    var dalPerRow = new ZoneImportDAL();
                    psListFiltered.ForEach(phs =>
                    {
                        RunChanged();
                        var priceArea = DataPriceArea.FindLast(d => d.ID == phs.PriceArea.ID);
                        if (priceArea != null)
                        {
                            var zoneImport = new ZoneImport
                            {
                                StationId = int.Parse(phs.StationCatalog),
                                OperatorId = GlobalData.LoginUser.UserOperator.IdOperator,

                                ShapeFile = priceArea.ShapeFile,
                                ShapeGeomteryCoords = priceArea.Coords,
                                ZoneCode = priceArea.ID,
                                ZoneDescription = priceArea.Name,
                                ImportZoneControlOperatorId = controllerEntity.ImportZoneControlOperatorId,
                                Area = priceArea.Area
                            };

                            dalPerRow.Save(zoneImport, connection);
                        }
                    });
                    controllerEntity.Status = 0;
                    dalController.Save(controllerEntity, connection);
                    if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm)
                       ProcessLauncher.OpenExplorerWithURL(string.Format(ExportConfigurator.GetConfig().TrafficLicensingPlanningZoneImportURL, controllerEntity.ImportZoneControlOperatorId));
                    else
                       ProcessLauncher.OpenExplorerWithURL(string.Format(ExportConfigurator.GetConfig().TrafficLicensingZoneImportURL, controllerEntity.ImportZoneControlOperatorId));
                    return true;
                }
                catch (Exception exp)
                {
                    controllerEntity.Status = 3;
                    dalController.Delete(controllerEntity, connection);
                    LoggerManager.WriteToLog(exp);
                    if (exp is OperationCanceledException)
                    {
                        message = Resources.BLManagerResource.OperationCanceledByUser;
                        _dalTranscad.CloseConnection();
                        return false;
                    }
                    throw;
                }
                finally
                {

                    connection.Close();
                    connection.Dispose();
                }


            }
            message = Resources.BLManagerResource.NonStationToExportToZonePriceArea;
            return false;
        }



        private static DataTable BuildDTfromPhysicalStops(List<PhysicalStop> lst)
        {
            var dt = new DataTable();
            dt.Columns.Add("OperatorId", typeof(int));
            dt.Columns.Add("ZoneCode", typeof(int));
            dt.Columns.Add("ZoneDescription", typeof(string));
            dt.Columns.Add("StationId", typeof(int));
            dt.Columns.Add("Shape", typeof(string));
            dt.Columns.Add("ShapeGeomtery", typeof(string));
            

            //POLYGON ((34781420 32040200, 34780550 32040810, 34779630 32039940, 34778660 32039570, 34777580 32039480, 34777200 32039540, 34776770 32038960, 34776730 32038640, 34776250 32038110, 34775780 32037300, 34776430 32036200, 34774890 32033370, 34772370 32035290, 34770790 32035450, 34770130 32035290, 34769030 32035390, 34765690 32036490, 34763410 32036870, 34761370 32037110, 34760590 32037150, 34759890 32036910, 34759610 32036650, 34759370 32036230, 34759110 32035570, 34759070 32032930, 34755890 32033290, 34755770 32032490, 34755190 32030830, 34753550 32030570, 34751370 32030890, 34750990 32030230, 34750150 32029070, 34742240 32033240, 34742350 32033330, 34743080 32034950, 34744030 32035950, 34744240 32036400, 34744130 32036850, 34744230 32037210, 34744760 32037390, 34745180 32038300, 34745070 32039200, 34744640 32039920, 34744320 32040190, 34744320 32040910, 34744000 32041540, 34744000 32041990, 34745160 32042800, 34745460 32044700, 34744620 32044970, 34745240 32046500, 34745140 32046770, 34745550 32047950, 34746720 32048040, 34747030 32049030, 34747980 32050300, 34748080 32050750, 34748720 32050750, 34748710 32051840, 34749350 32052020, 34749550 32052560, 34749760 32053730, 34750290 32054190, 34750710 32055000, 34751450 32055630, 34752820 32055820, 34754620 32056000, 34756840 32057730, 34759570 32062150, 34759350 32062600, 34761030 32066220, 34761230 32068290, 34760590 32069460, 34760700 32069640, 34761650 32068650, 34762290 32068660, 34762820 32069200, 34763020 32070010, 34763020 32070730, 34763340 32071190, 34763860 32071910, 34764800 32074350, 34764800 32075340, 34765430 32075790, 34765640 32076510, 34765430 32077320, 34765640 32077600, 34766790 32080850, 34767520 32082380, 34767510 32083820, 34767830 32084370, 34767830 32084820, 34767290 32085090, 34767810 32087340, 34768870 32087440, 34769180 32089060, 34768750 32090140, 34767900 32090500, 34768010 32090680, 34769070 32090590, 34769700 32090960, 34770540 32092220, 34770220 32093030, 34768950 32093930, 34770850 32094120, 34771490 32094930, 34771480 32095290, 34772010 32096830, 34772750 32096740, 34771620 32100250, 34777000 32098510, 34777500 32097170, 34777940 32096580, 34778660 32096210, 34781040 32096230, 34781370 32097080, 34786520 32097240, 34797100 32099700, 34802080 32100530, 34803670 32100450, 34802830 32097380, 34802080 32094260, 34801130 32089510, 34800450 32087670, 34799390 32085430, 34799170 32084290, 34799410 32082650, 34797950 32080350, 34799050 32079530, 34801470 32078190, 34801670 32077230, 34801550 32076590, 34799970 32073270, 34801650 32070970, 34801550 32069890, 34801890 32069510, 34802270 32065570, 34802470 32064870, 34803110 32063290, 34803590 32062430, 34804690 32061050, 34805590 32060350, 34806810 32059520, 34804700 32056980, 34805760 32056360, 34807880 32055640, 34810010 32054300, 34811280 32053490, 34812030 32052410, 34812890 32049890, 34812890 32048080, 34811420 32047810, 34811010 32046790, 34809300 32046380, 34808420 32043990, 34806660 32040450, 34804060 32038630, 34803680 32039470, 34800820 32039490, 34800320 32040110, 34797920 32040030, 34794340 32042630, 34793840 32041830, 34790960 32041410, 34787600 32042970, 34787100 32045030, 34786540 32045030, 34786200 32044410, 34783770 32043690, 34783250 32043050, 34781420 32040200))

            lst.ForEach(e =>
            {
                StringBuilder sb = new StringBuilder(); 
                sb.Append("POLYGON ((");
                e.PriceArea.Coords.ForEach(c => sb.AppendFormat("{0} {1}, ",c.Longitude,c.Latitude));
                string dataShapeCoords = sb.ToString().Substring(0,sb.ToString().Length - 2);
                dataShapeCoords = String.Concat(dataShapeCoords,"))");
                DataRow dr = dt.NewRow();
                dr["OperatorId"] = GlobalData.LoginUser.UserOperator.IdOperator;
                dr["ZoneCode"] = e.PriceArea.ID;
                dr["ZoneDescription"] = e.PriceArea.Name;
                dr["StationId"] =int.Parse(e.StationCatalog);
                dr["Shape"] = e.PriceArea.ShapeFilePath;
                dr["ShapeGeomtery"] = dataShapeCoords;
                dt.Rows.Add(dr);
            });
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// SetJunctionVersionList
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lst"></param>
        private void SetPhysicalStopsPriceArea(string fileName, List<PhysicalStop> lst)
        {
            using (CsvWriter writer = new CsvWriter())
            {
                writer.WriteCsv(BuildDTfromPhysicalStops(lst), fileName, Encoding.UTF8);
            }
        }


        public void ShowPriceAreaPolygonToolBox()
        {
            const string macroName = "pl";
            //ITransCadMunipulationDataDAL dalTranscad = new TransCadMunipulationDataDAL();

            _dalTranscad.RunExportMacro(macroName, ExportConfigurator.GetConfig().PriceAreaPolygonToolBox,
                new List<object> { GlobalData.LoginUser.UserOperator.IdOperator,(int) GlobalData.LoginUser.UserOperator.OperatorGroup });
        }
    }
}
