using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using BLEntities.SQLServer;
using DAL;
using DAL.SQLServerDAL;
using ExportConfiguration;
using IDAL;
using Logger;
using Utilities;
using BLEntities.Model;

namespace BLManager
{
    public class BlExportPriceAreaOfStationForAllOperators
    {
        private List<PhysicalStop> _stationList;
        private readonly ITransCadMunipulationDataDAL _dalTranscad;
        public BlExportPriceAreaOfStationForAllOperators()
        {
            _dalTranscad = new TransCadMunipulationDataDAL();
        }
        /// <summary>
        /// CheckDataValidity
        /// </summary>
        /// <param name="inputCsVfileName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool CheckDataValidity(string inputCsVfileName, ref string errorMessage)
        {
            var hasErrors = false;
            var sb = new StringBuilder();
            _stationList = new List<PhysicalStop>();
            using (var reader = new CsvReader(inputCsVfileName, Encoding.UTF8))
            {
                while (reader.ReadNextRecord())
                {

                    var stationCatalogFromFile = Convert.ToString(reader.Fields[0]);
                    if (!string.IsNullOrEmpty(stationCatalogFromFile) && !Validators.IsNumeric(stationCatalogFromFile))
                    {
                        reader.ReadNextRecord();
                        stationCatalogFromFile = Convert.ToString(reader.Fields[0]);
                    }
                    var physicalStop = GlobalData.RouteModel.PhysicalStopList.FirstOrDefault(p => p.StationCatalog == stationCatalogFromFile);
                    if (physicalStop == null)
                    {
                        sb.AppendFormat("{0} ", stationCatalogFromFile);
                        hasErrors = true;
                    }
                    else
                    {
                        _stationList.Add(physicalStop);
                    }
                }
            }
            if (hasErrors)
            {
                errorMessage = String.Concat(Resources.BLManagerResource.ListOfStationNotFoundInTranscad, sb.ToString());
                return false;
            }
            if (!_stationList.IsListFull())
            {
                errorMessage = Resources.BLManagerResource.StationNotDefinedInImportFile;
                return false;
            }
            return true;
        }
        /// <summary>
        /// GetPriceArea
        /// </summary>
        /// <param name="physicalStop"></param>
        /// <param name="dataPriceArea"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private PriceArea GetPriceArea(PhysicalStop physicalStop, IEnumerable<PriceArea> dataPriceArea, string layerName)
        {

            var prAreaId = _dalTranscad.GetNearestIdPriceAreaRecordToPhysicalStop(physicalStop, layerName, true, "ZoneCode");

            if (prAreaId > 0)
            {
                var pa = dataPriceArea.FirstOrDefault(p => p.ID == prAreaId);
                if (pa != null)
                    return pa;
            }
            return null;
        }

        /// <summary>
        /// GetZoneImportsOfOperator
        /// </summary>
        /// <param name="layerOperatorName"></param>
        /// <param name="allStation"></param>
        /// <returns></returns>
        private List<ZoneImport> GetZoneImportsOfOperator(string layerOperatorName, bool allStation)
        {
            var oper = GlobalData.OperatorList.FirstOrDefault(op => op.IdOperator == Convert.ToInt32(layerOperatorName));
            if (oper == null)
                throw new ApplicationException(string.Format("Layer name {0} is not valid operator id found into the OperatorList.xml file", layerOperatorName));
            var listOutPutData = new List<ZoneImport>();
            
            var dataPriceArea = _dalTranscad.GetAllPriceListPolygon(layerOperatorName);
            if (!allStation)
                _stationList.ForEach(phs => AddToListOutPutData(phs, dataPriceArea, layerOperatorName, listOutPutData));
            else
            {
                //_dalTranscad.SetMap(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName);
                GlobalData.RouteModel.PhysicalStopList.ForEach(phs => AddToListOutPutData(phs, dataPriceArea, layerOperatorName, listOutPutData));
            }
            return listOutPutData;
        }
        /// <summary>
        /// AddToListOutPutData
        /// </summary>
        /// <param name="phs"></param>
        /// <param name="dataPriceArea"></param>
        /// <param name="layerOperatorName"></param>
        /// <param name="listOutPutData"></param>
        private void AddToListOutPutData(PhysicalStop phs, IEnumerable<PriceArea> dataPriceArea, string layerOperatorName, ICollection<ZoneImport> listOutPutData)
        {
            var priceArea = GetPriceArea(phs, dataPriceArea, layerOperatorName);
            if (priceArea != null)
            {
               listOutPutData.Add(new ZoneImport
               {
                   StationId = int.Parse(phs.StationCatalog),
                   OperatorId = Convert.ToInt32(layerOperatorName),
                   ShapeFile = new byte[] {},
                   ShapeGeomteryCoords = new List<Coord>(),
                   ZoneCode = priceArea.ID,
                   ZoneDescription = priceArea.Name,
                   ImportZoneControlOperatorId = 0,
                   Area = priceArea.Area
               });
            }
        }
        /// <summary>
        /// GetStationIdsDataTable
        /// </summary>
        /// <returns></returns>
        private DataTable GetStationIdsDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("StationId", typeof(int));
            if (_stationList != null && _stationList.IsListFull())
            {
                _stationList.ForEach(s =>
                {
                    var dr = dt.NewRow();
                    dr["StationId"] = int.Parse(s.StationCatalog);
                    dt.Rows.Add(dr);
                });
                dt.AcceptChanges();

            }
            return dt;
        }

        /// <summary>
        /// IsExportsNeeded
        /// </summary>
        /// <param name="idOperator"></param>
        /// <param name="soPlst"></param>
        /// <param name="zLst"></param>
        /// <returns></returns>
        private bool IsExportsNeeded(int idOperator, List<StationOperatorPriceArea> soPlst, List<ZoneImport> zLst)
        {
            if (soPlst == null || !soPlst.IsListFull()) return true;
            var operDbReturnedList = soPlst.Where(s => s.OperatorId == idOperator).ToList();
            if (!operDbReturnedList.IsListFull()) return true;
            var isExportsNeeded = _stationList.Any(st =>
               !operDbReturnedList.Exists(sop => sop.StationId.ToString() == st.StationCatalog) ||
               !zLst.Exists(z => z.StationId.ToString() == st.StationCatalog));

            if (isExportsNeeded) return true;
            var differentZones = (from st in operDbReturnedList
                                  join z in zLst
                                      on st.StationId equals z.StationId
                                  where st.ZoneCode != z.ZoneCode
                                  select new { StatiomId = st.StationId }).ToList();
            return differentZones.Any();
        }

        private static void PrintSationOfOperator(List<ZoneImport> zoneImports, string layerName)
        {
            if (!zoneImports.IsListFull())
                LoggerManager.WriteToLog(string.Format("**** BlExportPriceAreaOfStationForAllOperators.PrintSationOfOperator zoneImports for {0} Operator is Empty",layerName));
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("**** BlExportPriceAreaOfStationForAllOperators.PrintSationOfOperator zoneImports for {0} Operator. Station count list is {1}", layerName, zoneImports.Count));
                zoneImports.OrderBy(z=>z.ZoneCode).ThenBy(z=>z.StationId).ToList().ForEach(zi=> sb.AppendLine(string.Format("ZoneCode : {0}, StationId {1}",zi.ZoneCode,zi.StationId)));
                LoggerManager.WriteToLog(sb.ToString());
            }
        }

        /// <summary>
        /// ExportPriceListDb
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportPriceListDb(ref string message)
        {
            _dalTranscad.OpenWorkspace(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsFileName);
            var priceListLayers = _dalTranscad.GetMapLayers(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName, "Area");
            if (priceListLayers.IsListFull())
            {
                var connection = new SqlConnection(DalShared.GetConnectionString(EnmOperatorGroup.Operator,false));
                var listExternalImportInsertImportZoneControlOperator = new List<ExternalImport_InsertImportZoneControlOperator>();
                var dalController = new ExternalImport_InsertImportZoneControlOperatorDAL();
                try
                {
                    var dalPerRow = new ZoneImportDAL();
                    connection.Open();
                    var dt = GetStationIdsDataTable();
                    var controlSationList = dalController.GetStationOperatorPriceAreaList(dt, connection);
                    priceListLayers.OrderBy(l=>Convert.ToInt32(l)).ToList().ForEach(layerName =>
                    {
                        var zoneImportList = GetZoneImportsOfOperator(layerName, false);
                        LoggerManager.WriteToLog(string.Format("********** Start  External Import ZoneControlOperator Operator : {0}", layerName));
                        if (zoneImportList.IsListFull() && (IsExportsNeeded(int.Parse(layerName), controlSationList, zoneImportList)))
                        {
                            zoneImportList = GetZoneImportsOfOperator(layerName, true);
                            PrintSationOfOperator(zoneImportList, layerName);
                            var controllerEntity = new ExternalImport_InsertImportZoneControlOperator { OperatorId = int.Parse(layerName), FromDate = DateTime.Now.AddDays(1) };
                            dalController.Add(controllerEntity, connection);
                            listExternalImportInsertImportZoneControlOperator.Add(controllerEntity);
                            zoneImportList.ForEach(zi =>
                            {
                                zi.ImportZoneControlOperatorId = controllerEntity.ImportZoneControlOperatorId;
                                try
                                {
                                    var ps = GlobalData.RouteModel.PhysicalStopList.FirstOrDefault(pstop => Convert.ToInt32(pstop.StationCatalog) == zi.StationId);
                                    // LoggerManager.WriteToLog(string.Format("Before Saving station {0} in DB for Operator {1} , IdStationStatus : {2}, ZoneCode {3} , ImportZoneControlOperatorId {4}", ps.StationCatalog, layerName, ps.IdStationStatus, zi.ZoneCode,zi.ImportZoneControlOperatorId));
                                    if (ps == null)
                                        LoggerManager.WriteToLog(string.Format("Station {0} was not found in DB for Operator {1}", zi.StationId, layerName));
                                    if (ps != null && ps.IdStationStatus == 1 && ps.IdStationType != 7) // Active station only for Operator group only
                                    {
                                        if (!dalPerRow.Save(zi, connection))
                                            LoggerManager.WriteToLog(string.Format("The station {0} was not saved in DB for Operator {1}. ZoneCode : {2}, ImportZoneControlOperatorId {3}", zi.StationId, layerName, zi.ZoneCode, zi.ImportZoneControlOperatorId ));
                                        //else
                                        //    LoggerManager.WriteToLog(string.Format("The station {0} was successfully saved in DB for Operator : {1} . ZoneCode : {2}  ImportZoneControlOperatorId {3}", zi.StationId, layerName, zi.ZoneCode, zi.ImportZoneControlOperatorId));
                                    }

                                }
                                catch(Exception exp)
                                {
                                    LoggerManager.WriteToLog(string.Format("The station {0} was not saved in DB for Operator {1}. ZoneCode {2}  ImportZoneControlOperatorId {3}. \n Exception is {4}", zi.StationId, layerName, zi.ZoneCode, zi.ImportZoneControlOperatorId, exp.Message));
                                    throw;
                                }
                            });
                            controllerEntity.Status = 1;
                            dalController.Save(controllerEntity, connection);
                            LoggerManager.WriteToLog(string.Format("********** End  External Import ZoneControlOperator Operator : {0}, Controller : {1}, Status {2}",layerName, controllerEntity.ImportZoneControlOperatorId,controllerEntity.Status));
                        }
                    }
                    );
                    return true;
                }
                catch (Exception exp)
                {
                    if (listExternalImportInsertImportZoneControlOperator.IsListFull())
                    {
                        listExternalImportInsertImportZoneControlOperator.ForEach(c =>
                        {
                            c.Status = 3;
                            dalController.Delete(c, connection);
                            LoggerManager.WriteToLog(exp);
                        });
                    }

                    LoggerManager.WriteToLog(exp);
                    throw;
                }
                finally
                {
                    _dalTranscad.CloseMap(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName);
                    //_dalTranscad.SetMap(ExportConfigurator.GetConfig().MapLayerName);
                    connection.Close();
                    connection.Dispose();
                }
            }
            message = "The Price Area Operator List doesn't have any layer";
            return false;
        }
    }
}
