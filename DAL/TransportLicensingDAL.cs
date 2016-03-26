using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using IDAL;
using BLEntities.Accessories;
using BLEntities.Entities;
using System.Xml.Linq;
using System.IO;
using Utilities;
using BLEntities.SQLServer;
using ExportConfiguration;

namespace DAL
{
    public class TransportLicensingDal : IInternalBaseDal
    {
        #region IInternalBaseDAL Members

        private const string ClusterValidDateLineExport = "ClusterValidDateLineExport.xml";
        private const string Format = "dd/MM/yyyy";
        public LoginUser GetLoginUserInfo(string groupUserName,EnmOperatorGroup operGroup)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetBaseTableEntityList
        /// </summary>
        /// <param name="idOperator"></param>
        /// <returns></returns>
        public Dictionary<string, List<BaseTableEntity>> GetBaseTableEntityList(int idOperator)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetOperatorValidateLineExportDate
        /// </summary>
        /// <returns></returns>
        public List<OperatorValidateLineExportDate> GetOperatorValidateLineExportDate()
        {
            var lineExportDates = new List<OperatorValidateLineExportDate>();
            var fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, ClusterValidDateLineExport);
            if (!File.Exists(fileName))
            {
                return lineExportDates;
            }

            var doc = XDocument.Load(fileName);
            var result = (from p in doc.Descendants("Operator")
                          select new OperatorValidateLineExportDate
                          {
                              IdOperator = int.Parse(p.Element("IdOperator").Value),
                              ClusterValidateLineExportDates = (p.Element("Clusters").Descendants("Cluster").Select
                              (cluster => new ClusterValidateLineExportDate
                              {
                                  IdCluster = int.Parse(cluster.Element("IdCluster").Value),
                                  ValidExportDate = string.IsNullOrEmpty(cluster.Element("ValidExportDate").Value) ? (DateTime?)  null :  DateTime.ParseExact(cluster.Element("ValidExportDate").Value, Format, CultureInfo.InvariantCulture)
                              }).ToList())
                          }).ToList();

            return result;
        }
        /// <summary>
        /// SaveOperatorValidateLineExportDate
        /// </summary>
        /// <param name="operatorValidateLineExportDates"></param>
        /// <returns></returns>
        public void SaveOperatorValidateLineExportDate(List<OperatorValidateLineExportDate> operatorValidateLineExportDates)
        {
            if (operatorValidateLineExportDates != null)
            {
                var operatorValidateLineExportDatesToOperate =
                    operatorValidateLineExportDates.Where(el => el.ClusterValidateLineExportDates.IsListFull()).ToList();
                if (operatorValidateLineExportDatesToOperate.IsListFull())
                {
                   
                    var fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, ClusterValidDateLineExport);

                    var doc = new XDocument(
                                           new XElement("Operators",operatorValidateLineExportDatesToOperate.Select(oper => 
                                                    new XElement("Operator", 
                                                              new XElement("IdOperator", oper.IdOperator),
                                                              new XElement("Clusters",   oper.ClusterValidateLineExportDates.Select(clus => 
                                                                  new XElement("Cluster",
                                                                                new XElement("IdCluster", clus.IdCluster),
                                                                                 new XElement("ValidExportDate",clus.ValidExportDate.HasValue ? clus.ValidExportDate.Value.ToString(Format) : string.Empty)))
                                                               
                                                )
                                            ))));
                    using (TextWriter tw = new StreamWriter(fileName))
                    {
                        doc.Save(tw);    
                    }
               }
            }
        }

        private static List<TranscadClusterConfig> ExtarctListTranscadClusterConfig(XElement clusters, string operatorName)
        {
            if (clusters==null)
                throw new ApplicationException(string.Format(Resources.ResourceDAL.ClusterDefWasNotFountForOperator, operatorName));
            var data = (clusters.Elements("Cluster").Select(p => new TranscadClusterConfig
             {
                 ClusterId = int.Parse(p.Element("ClusterId").Value),
                 PathToRSTFile = p.Element("PathToRSTFile").Value,
                 PathToRSTWorkSpace = p.Element("PathToRSTWorkSpace").Value,
                 PriceListLayer = p.Element("PriceListLayer").Value,
             })).ToList();
            if (!data.IsListFull())
                throw new ApplicationException(string.Format(Resources.ResourceDAL.ClusterDefWasNotFountForOperator, operatorName));
            return data;
        }
        private static List<TranscadClusterConfig> ExtarctListTranscadClusterConfigFromDataTable(DataTable dtTable, int operatorId)
        {
            if (dtTable == null || dtTable.Rows == null || dtTable.Rows.Count==0)
                throw new ApplicationException(string.Format(Resources.ResourceDAL.ClusterDefWasNotFountForOperator, operatorId));
            var data = dtTable.Select(string.Format("OperatorId = {0}" , operatorId)).AsEnumerable().Select(p => new TranscadClusterConfig
            {
                ClusterId = p.Field<bool>("IsTranscadAllCluster") ? -1 : p.Field<int>("ClusterId"),
                PathToRSTFile =p.Field<string>("PathToRSTFile"),
                PathToRSTWorkSpace = p.Field<string>("PathToRSTWorkSpace"),
                PriceListLayer = p.Field<string>("PriceListLayer")
            }).ToList();
            if (!data.IsListFull())
                throw new ApplicationException(string.Format(Resources.ResourceDAL.ClusterDefWasNotFountForOperator, operatorId));
            return data;
        }
        public List<Operator> GetOperatorList()
        {
            var td = SQLServerHelper.GetData("dbo.TcClusterToOperator", null,ConfigurationHelper.GetDbConnectionString("TransportLisencingString"));
            return (from p in td.AsEnumerable()
                 select new Operator
{
        TransCadStatus = 1,
        IdOperator = p.Field<int>("OperatorId"),
        OperatorName = p.Field<string>("Name"),
        OperatorGroup = p.Field<int>("Databasesource") == 1 ? EnmOperatorGroup.Operator : EnmOperatorGroup.PlanningFirm,
        ListOfTranscadClusterConfig =
        ExtarctListTranscadClusterConfigFromDataTable(td, p.Field<int>("OperatorId"))
}).GroupBy(p => p.IdOperator)
        .Select(g => g.First()).OrderBy(o => o.OperatorName).ToList();
            //var doc = XDocument.Load(GetOperatorFileName);
            
            //var data = (from p in doc.Descendants("Operator")
            //            let clustes = p.Element("Clusters")
            //                select new Operator
            //                {
            //                    IdOperator =int.Parse(p.Element("IdMofil").Value),
            //                    OperatorName = p.Element("MofilName").Value,
            //                    TransCadStatus = p.Element("TransCadStatus") == null ? default(byte) : byte.Parse(p.Element("TransCadStatus").Value),
            //                    OperatorGroup = p.Element("Group") == null ? enmOperatorGroup.Operator : (enmOperatorGroup)Enum.Parse(typeof(enmOperatorGroup), p.Element("Group").Value),
            //                    ListOfTranscadClusterConfig = ExtarctListTranscadClusterConfig(clustes, p.Element("MofilName").Value)
            //                }).ToList().OrderBy(p=> p.OperatorName).ToList();

            
            
        }

        public List<City> GetCityListFromExternalSource()
        {
            var doc = XDocument.Load(GetExternalCityFileName);
            var data = (from p in doc.Descendants("City")
                        select new City
                        {
                            Code =         p.Element("Id").Value,
                            Name  =        p.Element("Desc").Value,
                        }).ToList().OrderBy(p => p.Name).ToList();
            return data;
            
        }



        public List<TranslatorEntity> FillBaseTableTranslatorList()
        {
            throw new NotImplementedException();
        }

        public List<string> GetDirectionList()
        {
            throw new NotImplementedException();
        }

        public List<BaseLineInfo> GetBaseLine(BLEntities.Accessories.Operator operatorEntity)
        {
            throw new NotImplementedException();
        }

        private string GetExternalCityFileName
        {
            get
            {
                string xmlFile = "Cities.xml";
                string fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, xmlFile);
                if (!IoHelper.IsFileExists(fileName))
                {
                    throw new ApplicationException(string.Format("{0} file is not exists in application Data folder {1}", fileName, ExportConfigurator.GetConfig().DataFolder));
                }
                return fileName;
            }
        }


        private static string GetOperatorFileName
        {
            get
            {
                const string xmlFile = "OperatorList.xml";
                string fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, xmlFile);
                if (!IoHelper.IsFileExists(fileName))
                {
                    throw new ApplicationException(string.Format("{0} file is not exists in application Data folder {1}", fileName, ExportConfigurator.GetConfig().DataFolder));
                }
                return fileName;
            }
        }

        /// <summary>
        /// UpdateOperator
        /// </summary>
        /// <param name="operatorEntity"></param>
        /// <returns></returns>
        public bool UpdateOperator(Operator operatorEntity)
        {
            
            //string executeFolder =  Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //executeFolder = Path.Combine(executeFolder, GlobalConst.ENTITIES);
            //if (!IOHelper.IsFolderExists(executeFolder))
            //{
            //    throw new ApplicationException(string.Format("Application Data folder {0} is not exist", executeFolder));
            //}

            if (GetOperatorFileName != null)
            {
                var doc = XDocument.Load(GetOperatorFileName);
                var elem = (from p in doc.Descendants("Operator")
                                 where p.Element("IdMofil").Value == operatorEntity.IdOperator.ToString()
                                 select p).FirstOrDefault();
                if (elem!= null)
                {
                    elem.SetElementValue("TransCadStatus", operatorEntity.TransCadStatus.ToString());
                    doc.Save(GetOperatorFileName);
                }
            }

            return true;
        }
        /// <summary>
        /// Get New Catalog
        /// </summary>
        /// <param name="idOperator"></param>
        /// <param name="idCluster"></param>
        /// <returns></returns>
        public int? GetNewCatalog(int idOperator, int idCluster)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Is Catalog Exists 
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsCatalogExists(BLEntities.Entities.RouteLine routeLine)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Is Luz Of Line Exists
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsLuzOfLineExists(RouteLine routeLine)
        {
            throw new NotImplementedException();
        }

        public List<BaseTableEntity> GetClustersByOpearatorId(int opearatorId)
        {
            throw new NotImplementedException();
        }

        public bool TranscadLoginInsert(TranscadLogin transcadLogin)
        {
            throw new NotImplementedException();
        }

        public bool TranscadLoginDelete(string userName)
        {
            throw new NotImplementedException();
        }

        public string TranscadLoginIsFileOpened(string workspaceFile)
        {
            throw new NotImplementedException();
        }

        public List<TranscadLogin> GetAllTranscadLogins()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetAccountingGroupByOperatorIdAndClusterId
        /// </summary>
        /// <param name="idOperator"></param>
        /// <param name="idCluster"></param>
        /// <returns></returns>
        public List<AccountingGroup> GetAccountingGroupByOperatorIdAndClusterId(int idOperator, int idCluster)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInternalBaseDAL Members


        public Dictionary<string,object> GetFreeOfficeLineId(RouteLine routeLine, int newRouteNumber, bool? isArrivePlanning)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ClusterToZone> GetClusterToZoneDictionary()
        {
            throw new NotImplementedException();
        }

        public bool InsertPhysicalLayerTestJurnal(int idOperator, string userName, bool isPassedTest)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
