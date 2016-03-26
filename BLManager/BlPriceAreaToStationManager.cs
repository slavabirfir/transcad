using System;
using System.Collections.Generic;
using System.Linq;
using BLEntities.Accessories;
using BLEntities.Entities;
using BLEntities.Model;
using BLManager.Resources;
using DAL;
using ExportConfiguration;
using IBLManager;
using IDAL;
using Utilities;
using System.IO;
using System.Text;

namespace BLManager
{
    public class BlPriceAreaToStationManager : IBlPriceAreaToStationManager
    {
        private const int MaxProgressBarValueConst = 10;
        private const string PriceAreaZip = "PRICEAREA.ZIP";
        private const string PriceArea = "PriceArea";
        private const string Zonesdbd = "zones.dbd";
        private static IEnumerable<string> FileArray { get { return new List<string> { "ZONES.DBD", "ZONES.BIN", "ZONES.DSC", "ZONES.BDR", "ZONES.GRP" }.AsReadOnly(); } }
        private static IEnumerable<string> FieldZoneAreaPriceArray { get { return new List<string> { "PName", "ZName", "TatFull", "ID" }.AsReadOnly(); } }

        public string ErrorMessage { get; private set; }public string FilePath { get; private set; }
        public List<PriceZoneArea> PriceZoneAreas { get; private set; }
        
        private static void DeleteAllFile(IEnumerable<string> arrFiles)
        {
            if (arrFiles != null && arrFiles.Any())
            {
                foreach (string file in arrFiles)
                {
                    IoHelper.DeleteFile(file);
                }
            }
        }
        private static void EmptyFolder(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                EmptyFolder(subfolder);
            }
        }
        public static bool FileLocked(string fileName)
        {
            FileStream fs = null;
            try
            {
                // NOTE: This doesn't handle situations where file is opened for writing by another process but put into write shared mode, it will not throw an exception and won't show it as write locked
                fs = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None); // If we can't open file for reading and writing then it's locked by another process for writing
            }
            catch (UnauthorizedAccessException) // https://msdn.microsoft.com/en-us/library/y973b725(v=vs.110).aspx
            {
                // This is because the file is Read-Only and we tried to open in ReadWrite mode, now try to open in Read only mode
                try
                {
                    fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception)
                {
                    return true; // This file has been locked, we can't even open it to read
                }
            }
            catch (Exception)
            {
                return true; // This file has been locked
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return false;
        }

        private void AppendGeneralInfoToErrorString(string message)
        {
            ErrorMessage = string.Concat(message, Environment.NewLine, BLManagerResource.AreaPriceAskNewSet); 
        }

        private static void MoveFile(string fileFrom)
        {
            var fileName = Path.GetFileName(fileFrom);
            var fileTo = Path.Combine(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder, fileName);
            IoHelper.CopyFile(fileFrom,fileTo);
        }
        
        public bool ValidateInput()
        {
            OnChanged(new MessageEventArgs() { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 2, Message = BLManagerResource.AreaPriceValidFileNames });
            var activeFile = Path.Combine(ExportConfigurator.GetConfig().PriceAreaPriceZoneAreaFolder, Zonesdbd);
            if (!IoHelper.IsFileExists(activeFile))
            {
                AppendGeneralInfoToErrorString(string.Format(BLManagerResource.AreaPriceActiveFileNotExists, PriceAreaZip)); 
                return false;
            }
            if (FileLocked(activeFile))
            {
                AppendGeneralInfoToErrorString(string.Format(BLManagerResource.AreaPriceFileInUse, PriceAreaZip)); 
                return false;
            }
            if (_transCadMunipulationDataDal.IsLayerExists(ExportConfigurator.GetConfig().PriceAreaZonesAreaName))
                DropLayer();

            if (!FilePath.ToUpper().EndsWith(PriceAreaZip))
            {
                AppendGeneralInfoToErrorString(string.Format(BLManagerResource.AreaPriceZipNameWrong, PriceAreaZip)); 
                return false;
            }
            FileInputOperationsBulk();
            if (!ValidateFieldNames())
                return false; 
            return true;
        }

        private void FileInputOperationsBulk()
        {
            EmptyFolder(new DirectoryInfo(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder));
            //IoHelper.DeleteFolder(_folderTempAndPriceAreaName, true);
            MoveFile(FilePath);
            ZipunzipHelper.ExtractZipFile(FilePath, string.Empty, ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder);
        }

        private bool ValidateFieldNames()
        {
            var fileEntries = Directory.GetFiles(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder);

            var sb = new StringBuilder();
            bool flagFileNotExists = false;
            FileArray.ToList().ForEach(s =>
            {
                if (!fileEntries.Any(f => f.ToUpper().EndsWith(s)))
                {
                    if (!flagFileNotExists)
                        sb.AppendLine(BLManagerResource.AreaPriceFileNotExists);
                    sb.AppendLine(s);
                    flagFileNotExists = true;
                }
            });
            if (flagFileNotExists)
            {
                AppendGeneralInfoToErrorString(sb.ToString());
                DeleteAllFile(fileEntries);
                return false;
            }
            return true;
        }

        private static bool CheckEmptyField(IEnumerable<PriceZoneArea> data,string message,StringBuilder stringBuilder)
        {
            var sb = new StringBuilder();
            if (data.Any())
            {
                data.ToList().ForEach(a => sb.AppendFormat("{0} ", a.ID));
                stringBuilder.AppendLine(string.Format(message, sb));
                return false;
            }
            return true;
        }


        private static bool CheckDuplicated(IEnumerable<string> list, string message,StringBuilder stringBuilder)
        {
            var duplicates = list.GroupBy(x => x)               // group matching items
                             .Where(g => g.Skip(1).Any())   // where the group contains more than one item
                             .SelectMany(g => g);   

            var sb = new StringBuilder();
            if (duplicates.Any())
            {
                duplicates.Distinct().ToList().ForEach(a => sb.AppendFormat("{0}|", a));
                stringBuilder.AppendLine(string.Format(message, sb));
                return false;
            }
            return true;
        }

        private void DropLayer()
        {
            _transCadMunipulationDataDal.DropLayer(ExportConfigurator.GetConfig().MapLayerName, ExportConfigurator.GetConfig().PriceAreaZonesAreaName);
        }

        public bool CheckDataValidity()
        {
            OnChanged(new MessageEventArgs() { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 6, Message = BLManagerResource.AreaPriceValidData });
            if (!PriceZoneAreas.IsListFull())
            {
                AppendGeneralInfoToErrorString(BLManagerResource.AreaPriceLayerEmpty);
                return false;
            }
            var stBuilder = new StringBuilder();
            bool notValid = false;
            if (!CheckEmptyField(PriceZoneAreas.Where(a => string.IsNullOrEmpty(a.PName)),BLManagerResource.AreaPriceIDWithEmptyName,stBuilder))
                notValid = true;
            if (!CheckEmptyField(PriceZoneAreas.Where(a => string.IsNullOrEmpty(a.TatFull)), BLManagerResource.AreaPriceIDIsEmpty,stBuilder))
                notValid = true;
            if (!CheckEmptyField(PriceZoneAreas.Where(a => !a.Area.HasValue), BLManagerResource.AreaPriceAreaIsEmpty, stBuilder))
                notValid = true;

            if (!CheckDuplicated(PriceZoneAreas.Select(a=>a.TatFull), BLManagerResource.AreaPriceIDDuplicated,stBuilder))
                notValid = true;
            if (!CheckDuplicated(PriceZoneAreas.Select(a=>a.PName), BLManagerResource.AreaPriceNameDuplicated,stBuilder))
                notValid = true;
            if (notValid)
            {
                AppendGeneralInfoToErrorString(stBuilder.ToString());
                return false;
            }
            return true;
        }
        public bool ImportInput()
        {
            OnChanged(new MessageEventArgs() { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 3, Message = BLManagerResource.AreaPriceValidFileFields });
            var array = new object[2];
            array[0] = new object[2] {"Shared", "True"};
            array[1] = new object[2] {"ReadOnly", "False"};
            var dbName = Path.Combine(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder, "zones.dbd");
            _transCadMunipulationDataDal.AddLayer(ExportConfigurator.GetConfig().MapLayerName, ExportConfigurator.GetConfig().PriceAreaZonesAreaName, dbName, ExportConfigurator.GetConfig().PriceAreaZonesAreaName, array);
            var fieldZoneAreaPriceArray = _transCadMunipulationDataDal.GetLayerFields(ExportConfigurator.GetConfig().PriceAreaZonesAreaName);
            var sb = new StringBuilder();
            bool flagFieldNotExists = false;
            FieldZoneAreaPriceArray.ToList().ForEach(s =>
            {
                if (!fieldZoneAreaPriceArray.Any(f => f.ToUpper().EndsWith(s.ToUpper())))
                {
                    if (!flagFieldNotExists)
                        sb.AppendLine(BLManagerResource.AreaPriceFieldsNotExists);
                    sb.AppendLine(s);
                    flagFieldNotExists = true;
                }
            });
            if (flagFieldNotExists)
            {
                AppendGeneralInfoToErrorString(sb.ToString());
                EmptyFolder(new DirectoryInfo(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder));
                return false;
            }
            OnChanged(new MessageEventArgs() { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 4, Message = BLManagerResource.AreaPriceLoadData });
            PriceZoneAreas = _transCadMunipulationDataDal.GetPriceZoneAreas(ExportConfigurator.GetConfig().PriceAreaZonesAreaName);
            return true;
        }
        
        public bool ExportToLicensingSystem()
        {
            OnChanged(new MessageEventArgs { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 7, Message = BLManagerResource.AreaPriceExportGISOperations });
            UpdatePriceListZoneValues();
            DropLayer();
            OnChanged(new MessageEventArgs { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 8, Message = BLManagerResource.AreaPriceExportProcess });
            if (ExportAreaToDb())
            {
                OnChanged(new MessageEventArgs { MaxProgressBarValue = MaxProgressBarValueConst, ProgressBarValue = 9, Message = BLManagerResource.AreaPriceExportMovingFiles });
                CopyActiveToHistory();
                CopyTempToActiveAndEmptyTempFolder();
                return true;
            }
            return false; 
        }

        private bool ExportAreaToDb()
        {
            return true;

            string message = string.Empty; 
            IExportStationAreasToTrafficLicensingDal dal = new ExportStationAreasToTrafficLicensingDal();
            if (!dal.ExportPriceArea(GlobalData.RouteModel.PhysicalStopList, ref message))
            {
                ErrorMessage = message;
                return false;
            }
            return true;
        }

        private void UpdatePriceListZoneValues()
        {
            var macroName = ExportConfigurator.GetConfig().PriceAreaPolygonMacroName;
            var exportShapeFolder = ExportConfigurator.GetConfig().PriceAreaZonesFileFolder;
            if (IoHelper.IsFolderExists(exportShapeFolder))
            {
                IoHelper.DeleteDirectoryFolders(new DirectoryInfo(exportShapeFolder));
                IoHelper.DeleteFiles(exportShapeFolder, "*.*", false);
            }
            else
                IoHelper.CreateFolder(exportShapeFolder);
            GlobalData.RouteModel.PhysicalStopList.Where(p => p.IdStationType != 7).ToList().ForEach(ps =>
            {
                var prAreaId = _transCadMunipulationDataDal.GetNearestIdPriceAreaRecordToPhysicalStop(ps, ExportConfigurator.GetConfig().PriceAreaZonesAreaName, true, "ID");
                if (prAreaId > 0)
                {
                    var pa = PriceZoneAreas.FindLast(p => p.ID == prAreaId);
                    if (pa != null && ps.IdStationStatus == 1)
                    {
                        ps.PriceZoneArea = pa;
                    }
                }
            });
            _transCadMunipulationDataDal.RunExportMacro(macroName, ExportConfigurator.GetConfig().PriceAreaPolygonMacroUIFile, 
            new List<object>() { exportShapeFolder, ExportConfigurator.GetConfig().PriceAreaZonesAreaName });

            PriceZoneAreas.ForEach(p=> {
                      string pattern = string.Format("{0}\\{1}", exportShapeFolder, p.ID);
                      p.ShapeFilePath = string.Concat(pattern, Zipper.ZIPEXTENTION);
                      Zipper.ZIPFoder(pattern, p.ShapeFilePath);

                      using (var fs = new FileStream(p.ShapeFilePath, FileMode.Open, FileAccess.Read))
                      {
                          var b = new Byte[fs.Length];
                          fs.Read(b, 0, b.Length);
                          p.ShapeFile = b;
                      }

                       var data = _transCadMunipulationDataDal.RunTranscadMacro(ExportConfigurator.GetConfig().PolygonCalcAreaMacroName,
                       ExportConfigurator.GetConfig().PolygonCalcAreaUI,
                       new List<object>() { p.ID, ExportConfigurator.GetConfig().PriceAreaZonesAreaName });
                       if (data != null)
                       {
                           p.ShapeGeomteryCoords = new List<Coord>();
                           var coords = data.ToString().Split("|".ToCharArray());
                           for (int i = 0; i < coords.Length - 1; i++)
                           {
                               var lonLat = coords[i].Split(",".ToCharArray());
                               var coor = new Coord
                               {
                                   Longitude = int.Parse(lonLat[0]),
                                   Latitude = int.Parse(lonLat[1])
                               };
                               p.ShapeGeomteryCoords.Add(coor);
                           }
                       }
                        });
            
        }

        private static void CopyTempToActiveAndEmptyTempFolder()
        {

            var dirInfo = new DirectoryInfo(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder);
            foreach (var file in dirInfo.GetFiles())
            {

                if (!file.Name.ToUpper().EndsWith(PriceAreaZip))
                {
                    IoHelper.CopyFile(file.FullName,Path.Combine(ExportConfigurator.GetConfig().PriceAreaPriceZoneAreaFolder,file.Name));
                }
            }
            EmptyFolder(new DirectoryInfo(ExportConfigurator.GetConfig().PriceAreaTemporaryPriceZoneAreaFolder));
            //IoHelper.DeleteFolder(_folderTempAndPriceAreaName, true);
        }

        private static void CopyActiveToHistory()
        {
            var historyFileName = string.Format(@"{0}\{1}_{2}.zip", ExportConfigurator.GetConfig().PriceAreaHistoryPriceZoneAreaFolder, PriceArea, DateTime.Now.ToStringddMMyyyyhhmmssNONSeparator());
            ZipunzipHelper.ZipFolder(historyFileName, string.Empty, ExportConfigurator.GetConfig().PriceAreaPriceZoneAreaFolder); 
        }

        private readonly ITransCadMunipulationDataDAL _transCadMunipulationDataDal;
        public BlPriceAreaToStationManager(string filePath)
        {
            FilePath = filePath;
            _transCadMunipulationDataDal = new TransCadMunipulationDataDAL();
        }

        public event EventHandler<MessageEventArgs> Changed;

        /// <summary>
        /// On Changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(MessageEventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
    }
}
