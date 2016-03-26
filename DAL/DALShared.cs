using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using BLEntities.Model;
using BLEntities.SQLServer;
using IDAL;
using Logger;
using Utilities;
using System.IO;
using ExportConfiguration;

namespace DAL
{
    public static class DalShared
    {

        public const string ApplicationIntentReadOnly = ";ApplicationIntent=ReadOnly";

        public const string Separator = ";";
        private static readonly ITransCadMunipulationDataDAL TransCadMunipulationDataDal = new TransCadMunipulationDataDAL();
        private static double GetDistanceBetweenPoints(Coord p, Coord q)
        {
            var a = p.Longitude - q.Longitude;
            var b = p.Latitude - q.Latitude;
            return Math.Sqrt(a * a + b * b);
        }

        /// <summary>
        /// IsTaxiOperator
        /// </summary>
        /// <returns></returns>
        public static bool IsTaxiOperator()
        {
            return GlobalData.LoginUser.UserOperator.IdOperator == ExportConfigurator.GetConfig().TaxiOperatorId;
        }
        public static bool IsTrainOperator()
        {
            return GlobalData.LoginUser.UserOperator.IdOperator == ExportConfigurator.GetConfig().TrainOperatorId;
        }
        public static bool IsOperatorsIDsNotCalcNispah2Distances()
        {
            string[] operatorsIDsNotCalcNispah2DistancesArray = ExportConfigurator.GetConfig().OperatorsIDsNotCalcNispah2Distances.Split(Separator.ToCharArray());
            if (operatorsIDsNotCalcNispah2DistancesArray.Any())
                return operatorsIDsNotCalcNispah2DistancesArray.FirstOrDefault(el => el == GlobalData.LoginUser.UserOperator.IdOperator.ToString()) != null;
            return false;
        }


        public static void UpdateCitiesAndJunctionList(RouteLine routeLine)
        {
            if (routeLine == null) return; 
            var data = TransCadMunipulationDataDal.RunTranscadMacro(ExportConfigurator.GetConfig().GetRlInfoMacro, ExportConfigurator.GetConfig().GetRlInfoUi, new List<object>() { routeLine.ID });
            if (data != null)
            {
                routeLine.JunctionIds = new List<long>();
                routeLine.CityIds = new List<int>();
                var lst = data.ToString().Split(";".ToCharArray());
                for (var i = 0; i < lst.Length - 1; i++)
                {
                    var junctionCity = lst[i].Split("_".ToCharArray());
                    if (!routeLine.JunctionIds.Contains(int.Parse(junctionCity[0])))
                        routeLine.JunctionIds.Add(int.Parse(junctionCity[0]));

                    if (!routeLine.CityIds.Contains(int.Parse(junctionCity[1])))
                        routeLine.CityIds.Add(int.Parse(junctionCity[1]));
                }
            }
        }

        public static List<Coord> GetShapeGeomteryCoords(int routeLineId)
        {
            var coorList = new List<Coord>();
            var data = TransCadMunipulationDataDal.RunTranscadMacro(ExportConfigurator.GetConfig().RouteSystemPointsMacro, ExportConfigurator.GetConfig().RouteSystemPointsUI, new List<object>() { routeLineId });
            if (data != null)
            {
                var coords = data.ToString().Split("|".ToCharArray());
                for (var i = 0; i < coords.Length - 1; i++)
                {
                    var lonLat = coords[i].Split(",".ToCharArray());
                    var coor = new Coord { Longitude = int.Parse(lonLat[0]), Latitude = int.Parse(lonLat[1])};
                    coorList.Add(coor);
                }
            }
            return coorList;
        }

        public static void BuildNispah2(string nispa2CoordsFolder, LineDetailImport lineDetailImport, IImportIDAL<ShapeOfJunctionDetailsImport> dalShapeOfJunctionDetailsImport, DbConnection connection)
        {
            const int delta = 5;
            if (lineDetailImport != null && lineDetailImport.ShapeGeomteryCoords != null && lineDetailImport.ShapeGeomteryCoords.Any())
            {
                var line = string.Format("{0} {1} {2}", lineDetailImport.Line, lineDetailImport.Direction, lineDetailImport.LineAlternative);
                var data = BuildNispah2Table(lineDetailImport.ShapeGeomteryCoords, delta, line);
                if (data.IsDataTableFull())
                {
                    foreach (DataRow row in data.Rows)
                        SaveShapeOfJunctionDetailsImport(row, lineDetailImport, connection, dalShapeOfJunctionDetailsImport);
                    if (!string.IsNullOrEmpty(nispa2CoordsFolder))
                    {
                        var halufa = lineDetailImport.LineAlternative;
                        if (halufa.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                            halufa = ((int)lineDetailImport.LineAlternative[0]).ToString();
                        var currentFile = Path.Combine(nispa2CoordsFolder, string.Format("{0}_{1}_{2}.csv", lineDetailImport.Line, lineDetailImport.Direction, halufa));
                        CsvUtils.WriteCsvFile(currentFile, data, Encoding.UTF8);
                    }
                }
            }
        }
        private static void SaveShapeOfJunctionDetailsImport(DataRow row, LineDetailImport lineDetailImport, DbConnection connection, IImportIDAL<ShapeOfJunctionDetailsImport> dalShapeOfJunctionDetailsImport)
        {
            dalShapeOfJunctionDetailsImport.Save(new ShapeOfJunctionDetailsImport
            {
                ImportControlId = lineDetailImport.ImportControlId,
                OperatorId = lineDetailImport.OperatorId,
                OfficeLineId = lineDetailImport.OfficeLineId,
                Direction = lineDetailImport.Direction,
                LineAlternative = lineDetailImport.LineAlternative,
                Order = Convert.ToInt32(row["OrderId"]),
                Lat = Convert.ToDecimal(row["Latitude"]),
                Long = Convert.ToDecimal(row["Longitude"])
            }, connection);
        }

        private static DataTable BuildNispah2Table(List<Coord> coords, int delta, string line)
        {
            var dt = new DataTable();
            dt.Columns.Add("OrderId", typeof(int));
            dt.Columns.Add("Longitude", typeof(decimal));
            dt.Columns.Add("Latitude", typeof(decimal));
            if (coords != null && coords.IsListFull())
            {
                AddNispah2Row(coords[0], 1, dt);
                for (int i = 1; i < coords.Count; i++)
                {
                    var distance = GetDistanceBetweenPoints(coords[i - 1], coords[i]);
                    if (distance > delta)
                        AddNispah2Row(coords[i], i + 1, dt);
                    else
                        LoggerManager.WriteToLog(string.Format("Line {0}. Distance between point x1 : {1}, y1 : {2} and x2 {3}, y2 {4} is {5}. The second point was not applied",
                        line, coords[i - 1].Longitude, coords[i - 1].Latitude, coords[i].Longitude, coords[i].Latitude, distance));
                }
                dt.AcceptChanges();
            }
            return dt;
        }

        private static void AddNispah2Row(Coord coord, int orderId, DataTable dt)
        {
            var dr = dt.NewRow();
            dr["OrderId"] = orderId;
            dr["Longitude"] = coord.Longitude.ConvertFromIntToTvunaDoubleFormat();
            dr["Latitude"] = coord.Latitude.ConvertFromIntToTvunaDoubleFormat();
            dt.Rows.Add(dr);
        }

        public static string GetNispah2CoordsFolder()
        {
            const string nispah2Coords = "Nispah2Coords";
            string folder = Path.Combine(ExportConfigurator.GetConfig().DataFolder, nispah2Coords);
            if (!IoHelper.IsFolderExists(folder))
            {
                IoHelper.CreateFolder(folder);
            }
            folder = Path.Combine(folder, GlobalData.LoginUser.UserOperator.IdOperator.ToString());
            if (!IoHelper.IsFolderExists(folder))
            {
                IoHelper.CreateFolder(folder);
            }
            folder = Path.Combine(folder, GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.ClusterId.ToString());
            if (IoHelper.IsFolderExists(folder))
            {
                IoHelper.DeleteFolder(folder, true);
            }

            IoHelper.CreateFolder(folder);
            return folder;
        }

        /// <summary>
        /// DALShared
        /// </summary>
        /// <param name="operatorGroup"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        public static string GetConnectionString(EnmOperatorGroup operatorGroup, bool isReadOnly)
        {
            var connectionString = operatorGroup.Equals(EnmOperatorGroup.Operator) ? ConfigurationHelper.GetDbConnectionString("TransportLisencingString") : ConfigurationHelper.GetDbConnectionString("TrafficLisencingFirm");
            if (isReadOnly)
            {
                return string.Format("{0}{1}", connectionString, ApplicationIntentReadOnly);
            }
            return connectionString;
        }


        /// <summary>
        /// Fill Cluster To Zone List
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, ClusterToZone> FillClusterToZoneDictionary()
        {
            IInternalBaseDal internalBaseDal = new InternalTvunaImplementationDal();
            return internalBaseDal.GetClusterToZoneDictionary();
        }

        //public static Dictionary<int, ClusterToZone> FillClusterToZoneDictionary()
        //{

        //    const string xmlFile = "ClusterToZone.xml";
        //    string fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, xmlFile);
            
        //    if (!IOHelper.IsFileExists(fileName))
        //    {
        //        throw new ApplicationException(string.Format("{0} file is not exists in Data folder {1}", xmlFile, ExportConfigurator.GetConfig().DataFolder));
        //    }

        //    var doc = XDocument.Load(fileName);
        //    var clusterToZones = new Dictionary<int, ClusterToZone>(); 
                
        //    var result =(doc.Descendants("Cluster").Select(p => new
        //    {
        //        ClusterID = int.Parse(p.Attribute("id").Value),
        //        ClusterName = p.Attribute("name").Value,
        //        MainZoneID =
        //    int.Parse(p.Element("MainZone").Attribute("id").Value),
        //        SubZoneID = int.Parse(p.Element("SubZone").Attribute("id").Value),
        //        MainZoneName = p.Element("MainZone").Attribute("name").Value,
        //        SubZoneName = p.Element("SubZone").Attribute("name").Value})).ToList();
        //    if (result.Any())
        //    {
        //        foreach (var res in result)
        //        {
        //           if (!clusterToZones.ContainsKey(res.ClusterID))
        //           {
        //                clusterToZones.Add(res.ClusterID,new ClusterToZone
        //                             {
        //                                 ClusterID = res.ClusterID,
        //                                 ClusterName = res.ClusterName,
        //                                 ClusterStateList = new List<ClusterState>
        //                                                        {
        //                                                            new ClusterState
        //                                                                {
        //                                                                    MainZoneId = res.MainZoneID,
        //                                                                    MainZoneName = res.MainZoneName,
        //                                                                    SubZoneId = res.SubZoneID,
        //                                                                    SubZoneName = res.SubZoneName
        //                                                                }
        //                                                        } ,
        //                             } );
                       
        //           }
        //           else
        //           {
        //                clusterToZones[res.ClusterID].ClusterStateList.Add(new ClusterState
        //                                               {
        //                                                   MainZoneId = res.MainZoneID,
        //                                                   MainZoneName = res.MainZoneName,
        //                                                   SubZoneId = res.SubZoneID,
        //                                                   SubZoneName = res.SubZoneName
        //                                               });
        //           }
        //        }
        //    }
        //    return clusterToZones;
        //}
    }
}
