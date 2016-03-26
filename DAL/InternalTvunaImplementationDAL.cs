using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using IDAL;
using BLEntities.Entities;
using BLEntities.Accessories;
using Utilities;
using System.Data;
using System.Xml.Linq;
using System.IO;
using ExportConfiguration;
using BLEntities.Model;
using BLEntities.SQLServer;

namespace DAL
{
    public class InternalTvunaImplementationDal : IInternalBaseDal
    {
        #region Private / Const
        private const string Transcadoperatorsconnectionstringkey = "TranscadOperators";
        public string ConnectionString { 
            get
            {
                return DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false);
            } 
        }

        public string ConnectionStringReadOnly
        {
            get
            {
                return DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup, true);
            }
        }


        public string ConnectionStringTranscadOperator
        {
            get
            {
                return ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey);
            }
        }

        public string ConnectionStringTranscadOperatorReadOnly
        {
            get
            {
                return string.Format("{0}{1}",ConfigurationHelper.GetDbConnectionString(Transcadoperatorsconnectionstringkey),DalShared.ApplicationIntentReadOnly);
            }
        }


        #endregion
        
        #region IInternalBaseDAL Members
        public bool InsertPhysicalLayerTestJurnal(int idOperator, string userName, bool isPassedTest)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"IdOperator", idOperator},
                              {"UserName", userName},
                              {"IsPassedTest", isPassedTest}
                          };
            return SQLServerHelper.ExecuteQuery("dbo.InsertPhysicalLayerTestJurnal", dis, ConnectionStringTranscadOperator);
        }

        /// <summary>
        /// Get Base Table Entity List
        /// </summary>
        /// <param name="idOperator"></param>
        /// <returns></returns>
        public Dictionary<string, List<BaseTableEntity>> GetBaseTableEntityList(int idOperator)
        {
            var data = new Dictionary<string, List<BaseTableEntity>>();
            var spTcCarTypeList = (from p in SQLServerHelper.GetData("[dbo].[TcGetVehicleType]", null, ConnectionStringReadOnly).AsEnumerable()
                                      select new BaseTableEntity
                                      {
                                          ID = p.Field<int>("VehicleTypeId"),
                                          Name = p.Field<string>("VehicleTypeDesc")
                                      }).ToList();
            data.Add("TcGetVehicleType", spTcCarTypeList);

            var spTcCarSizeList = (from p in SQLServerHelper.GetData("[dbo].[TcGetVehicleSize]", null, ConnectionStringReadOnly).AsEnumerable()
                                   select new BaseTableEntity
                                   {
                                       ID = p.Field<int>("VehicleSizeId"),
                                       Name = p.Field<string>("VehicleSizeDesc")
                                   }).ToList();
            data.Add("TcGetVehicleSize", spTcCarSizeList);

            //var spTcMahozList = (from p in SQLServerHelper.GetData("[dbo].[SpTcMahoz]", null, connectionString).AsEnumerable()
            //                       select new BaseTableEntity
            //                       {
            //                           ID = p.Field<int>("IdZone"),
            //                           Name = p.Field<string>("ZoneName")
            //                       }).ToList();
            //data.Add("SpTcMahoz", spTcMahozList);

            var spTcSugKavList = (from p in SQLServerHelper.GetData("[dbo].[TcGetLineType]", null, ConnectionStringReadOnly).AsEnumerable()
                                   select new BaseTableEntity
                                   {
                                       ID = Convert.ToInt32(p.Field<int>("LineType")),
                                       Name = p.Field<string>("LineTypeDesc")
                                   }).ToList();
            data.Add("TcGetLineType", spTcSugKavList);

            var spTcSugSherutList = (from p in SQLServerHelper.GetData("[dbo].[TcGetServiceType]", null, ConnectionStringReadOnly).AsEnumerable()
                                   select new BaseTableEntity
                                   {
                                       ID = Convert.ToInt32(p.Field<int>("ServiceTypeId")),
                                       Name = p.Field<string>("ServiceTypeDesc")
                                   }).ToList();
            data.Add("TcGetServiceType", spTcSugSherutList);

            var spTcTipusTachanaList = (from p in SQLServerHelper.GetData("[dbo].[TcGetStationActivityType]", null, ConnectionStringReadOnly).AsEnumerable()
                                   select new BaseTableEntity
                                   {
                                       ID = Convert.ToInt32(p.Field<int>("StationActivityType")),
                                       Name = p.Field<string>("StationActivityTypeDesc")
                                   }).ToList();
            data.Add("TcGetStationActivityType", spTcTipusTachanaList);

            var spTcVarConvertList = (from p in SQLServerHelper.GetData("[dbo].[TcGetVarConvert]", null, ConnectionStringReadOnly).AsEnumerable()
                                        select new BaseTableEntity
                                        {
                                            ID =Convert.ToInt32((p.Field<int>("VarNum"))),
                                            Name = p.Field<string>("VarChar")
                                        }).ToList();
            data.Add("TcGetVarConvert", spTcVarConvertList);



            var spExclusivityLine = (from p in SQLServerHelper.GetData("[dbo].[TcGetExclusivityLine]", null, ConnectionStringReadOnly).AsEnumerable()
                                      select new BaseTableEntity
                                      {
                                          ID = Convert.ToInt32((p.Field<int>("ExclusivityLineId"))),
                                          Name = p.Field<string>("ExclusivityLineDesc")
                                      }).ToList();
            data.Add("TcGetExclusivityLine", spExclusivityLine);


            var spTcGetStationStatus = (from p in SQLServerHelper.GetData("[dbo].[TcGetStationStatus]", null, ConnectionStringReadOnly).AsEnumerable()
                                     select new BaseTableEntity
                                     {
                                         ID = Convert.ToInt32((p.Field<int>("StationStatusId"))),
                                         Name = p.Field<string>("StationStatusDesc")
                                     }).ToList();
            data.Add("TcGetStationStatus", spTcGetStationStatus);

            var spTcGetStationType = (from p in SQLServerHelper.GetData("[dbo].[TcGetStationType]", null, ConnectionStringReadOnly).AsEnumerable()
                                        select new BaseTableEntity
                                        {
                                            ID = Convert.ToInt32((p.Field<int>("StationTypeId"))),
                                            Name = p.Field<string>("StationTypeDesc")
                                        }).ToList();
            data.Add("TcGetStationType", spTcGetStationType);


            var paramSp = new Dictionary<string,object> {{"Operator", idOperator}};
            var spTcFetchClusterByIdMofilList = (from p in SQLServerHelper.GetData("[dbo].[TcGetClusterByOperatorId]", paramSp, ConnectionStringReadOnly).AsEnumerable()
                                        select new BaseTableEntity()
                                        {
                                            ID = p.Field<int>("ClusterId"),
                                            AdditonalInfo = new Dictionary<string, object>
                                                                {
                                                                    {"IsManyDistricts",p.Field<bool>("IsManyDistricts")} 
                                                                } ,
                                            Name = p.Field<string>("Description")
                                        }).ToList();

            data.Add("TcGetClusterByOperatorId", spTcFetchClusterByIdMofilList);

            return data;
        }

        /// <summary>
        /// GetClustersByOpearatorId
        /// </summary>
        /// <param name="opearatorId"></param>
        /// <returns></returns>
        public List<BaseTableEntity> GetClustersByOpearatorId(int opearatorId)
        {
            var oper = GlobalData.OperatorList.SingleOrDefault(op => op.IdOperator == opearatorId);
            if (oper == null) return new List<BaseTableEntity>(); 

            var paramSp = new Dictionary<string, object> { { "Operator", opearatorId } };
            return (from p in SQLServerHelper.GetData("[dbo].[TcGetClusterByOperatorId]", paramSp, DalShared.GetConnectionString(oper.OperatorGroup,true)).AsEnumerable()
                                                 select new BaseTableEntity
                                                 {
                                                     ID = p.Field<int>("ClusterId"),
                                                     Name = p.Field<string>("Description")
                                                 }).ToList();
           
        }

        public List<TranscadLogin> GetAllTranscadLogins()
        {
            return (from p in SQLServerHelper.GetData("[dbo].[TranscadLoginGetAll]", null, ConnectionStringTranscadOperatorReadOnly).AsEnumerable()
                    select new TranscadLogin
                    {
                        OperatorName = p.Field<string>("OperatorName"),
                        ClusterName = p.Field<string>("ClusterName"),
                        LoginDate = p.Field<DateTime>("LoginDate"),
                        WorkspaceFile = p.Field<string>("WorkspaceFile"),
                        UserName = p.Field<string>("UserName")
                    }).ToList();
        }


        public bool TranscadLoginInsert(TranscadLogin transcadLogin)
        {
            var dis = new Dictionary<string,object>
                          {
                              {"UserName", transcadLogin.UserName},
                              {"OperatorName", transcadLogin.OperatorName},
                              {"ClusterName", transcadLogin.ClusterName},
                              {"LoginDate", transcadLogin.LoginDate},
                              {"WorkspaceFile", transcadLogin.WorkspaceFile}
                          };
            return SQLServerHelper.ExecuteQuery("[dbo].[TranscadLoginInsert]", dis, ConnectionStringTranscadOperator);
        }

        public bool TranscadLoginDelete(string userName)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"UserName", userName}
                          };
            return SQLServerHelper.ExecuteQuery("[dbo].[TranscadLoginDelete]", dis, ConnectionStringTranscadOperator);
        }

        public string TranscadLoginIsFileOpened(string workspaceFile)
        {
            var dis = new Dictionary<string, object>
                          {
                              {"WorkspaceFile", workspaceFile}
                          };
            return SQLServerHelper.GetScalar<string>("[dbo].[TranscadLoginIsFileOpened]",dis,ConnectionStringTranscadOperatorReadOnly);
        }

        /// <summary>
        /// Get Operator List
        /// </summary>
        /// <returns></returns>
        public List<Operator> GetOperatorList()
        {
            return (from p in SQLServerHelper.GetData("[dbo].[SpTcGetAllMafil]", null,ConnectionStringReadOnly).AsEnumerable()
                    select new Operator
                  {
                      IdOperator =p.Field<int>("IdMofil"),
                      OperatorName = p.Field<string>("MofilName"),
                      TransCadStatus = (p.IsNull("TransCadStatus") ? default(byte)  : p.Field<byte>("TransCadStatus"))
                  }).ToList();
        }
        /// <summary>
        /// Get Direction List
        /// </summary>
        /// <returns></returns>
        public List<string> GetDirectionList()
        {
            return (from p in SQLServerHelper.GetData("[dbo].[TcGetDirections]", null, ConnectionStringReadOnly).AsEnumerable()
                    select p.Field<int>("Direction").ToString()                    
                    ).ToList();
        }


        /// <summary>
        /// Fill Base Table Translator List
        /// </summary>
        /// <returns></returns>
        public List<TranslatorEntity> FillBaseTableTranslatorList()
        {
            const string xmlFile = "BaseTableTranslator.xml";
            var fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, xmlFile);
            if (!IoHelper.IsFileExists(fileName))
            {
                throw new ApplicationException(string.Format("{0} file is not exists in application Data folder {1}", fileName, ExportConfigurator.GetConfig().DataFolder));
            }
            var doc = XDocument.Load(fileName);
            if (doc != null)
            {
                var resData = new List<TranslatorEntity>();
                var data = (from p in doc.Descendants("table")
                            select new
                            {
                                EnglishName = p.Attribute("englishname").Value,
                                HebrewName = p.Attribute("hebrewname").Value,
                                RealValueName = p.Element("RealData") == null ? null : p.Element("RealData").Attribute("value").Value,
                                PossibleValueList = p.Element("RealData") == null ? null : p.Element("RealData").Elements()
                            }).ToList();
                foreach (var item in data)
                {
                    var element = new TranslatorEntity {EnglishName = item.EnglishName, HebrewName = item.HebrewName};
                    if (!string.IsNullOrEmpty(item.RealValueName))
                    {
                        var possibleValues = new List<string>();
                        element.RealPossibleValues = new Dictionary<string, List<string>>();
                        element.RealPossibleValues.Add(item.RealValueName, possibleValues);
                        possibleValues.AddRange(item.PossibleValueList.Select(it => it.Attribute("value").Value));
                    }
                    resData.Add(element);
                }
                return resData;
            }
            return null;
        }

        /// <summary>
        /// Update Operator
        /// </summary>
        /// <param name="operatorEntity"></param>
        public bool UpdateOperator(Operator operatorEntity)
        {
            var dis = new Dictionary<string,object>
                          {
                              {"IdMofil", operatorEntity.IdOperator},
                              {"TransCadStatus", operatorEntity.TransCadStatus}
                          };
            return SQLServerHelper.ExecuteQuery("[dbo].[SpTcUpdateMafil]", dis, ConnectionString);
        }

        /// <summary>
        /// Get Base Line
        /// </summary>
        /// <param name="operatorEntity"></param>
        /// <returns></returns>
        public List<BaseLineInfo> GetBaseLine(Operator operatorEntity)
        {
            //ClusterId   OfficeLineId Direction   LineAlternative IsBase


            var inputParameter = new Dictionary<string, object> {{"operatorId", operatorEntity.IdOperator}};
            var data = SQLServerHelper.GetData("[dbo].[TcGetBaseLineDetail]", inputParameter, ConnectionStringReadOnly);
            if (!data.IsDataTableFull()) return null;

            var result = (from p in data.AsEnumerable()
                          select new BaseLineInfo 
                    {

                        Catalog = Convert.ToInt32(p.Field<int>("OfficeLineId")),
                        Direction = Convert.ToInt16(p.Field<int>("Direction")),
                        Variant = Convert.ToString(p.Field<string>("LineAlternative")),
                        IdCluster = p.Field<int>("ClusterId"),
                        IsBased = p.Field<bool>("IsBase")
                        
                    }
                   ).ToList();
            return result; 
        }
        /// <summary>
        /// Get New Catalog
        /// </summary>
        /// <param name="idOperator"></param>
        /// <param name="idCluster"></param>
        /// <returns></returns>
        public int? GetNewCatalog(int idOperator, int idCluster)
        {
            var inputParameter = new Dictionary<string, object> {{"IdOperator", idOperator}, {"IdCluster", idCluster}};
            return SQLServerHelper.GetOutPutParameter<int?>("dbo.SpTcGetNewCatalog", inputParameter, "CatalogNew",0, ConnectionString);
        }

         
        /// <summary>
        /// Is Catalog Exists
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsCatalogExists(RouteLine routeLine)
        {
            var inputParameter = new Dictionary<string, object>
                                     {
                                         {"IdOperator", routeLine.IdOperator},
                                         {"IdCluster", routeLine.IdCluster},
                                         {"OfficeLIneId", routeLine.Catalog}
                                     };
            var value = SQLServerHelper.GetOutPutParameter<int?>("[dbo].[IsOfficeLineIdExist]", inputParameter, "RetVal", 0, ConnectionStringReadOnly);
            return value.HasValue && value > 0; 
        }

        #endregion
        /// <summary>
        /// Is Luz Of Line Exists
        /// </summary>
        /// <param name="routeLine"></param>
        /// <returns></returns>
        public bool IsLuzOfLineExists(RouteLine routeLine)
        {
             
            var inputParameter = new Dictionary<string, object>
                                     {
                                         {"OfficeLineId", routeLine.Catalog},
                                         {"Direction", routeLine.Dir ?? -1},
                                         {"Alternative", routeLine.Var},
                                         {"OperatorId", routeLine.IdOperator}
                                     };
            var value = SQLServerHelper.GetScalar<int?>("[external].[CheckIfLineDetailHasTimetable]", inputParameter, ConnectionStringReadOnly);
            return value.HasValue && value > 0; 
        }



        #region IInternalBaseDAL Members

        /// <summary>
        /// GetAccountingGroupByOperatorIdAndClusterId
        /// </summary>
        /// <param name="idOperator"></param>
        /// <param name="idCluster"></param>
        /// <returns></returns>
        public List<AccountingGroup> GetAccountingGroupByOperatorIdAndClusterId(int idOperator, int idCluster)
        {
            var inputParameter = new Dictionary<string, object> {{"OperatorId", idOperator}, {"ClusterId", idCluster}};
            DataTable data = SQLServerHelper.GetData("[dbo].[GetAccountingGroupByOperator]", inputParameter, ConnectionStringReadOnly);

            var defaultAccountGroup = new AccountingGroup { OperatorId = idOperator, ClusterId = idCluster, AccountingGroupID = 0, AccountingGroupDesc = Resources.ResourceDAL.OperatorGroup };

            if (!data.IsDataTableFull()) return new List<AccountingGroup>{defaultAccountGroup};

            var result = (from p in data.AsEnumerable()
                          select new AccountingGroup
                          {
                              ClusterId = idCluster,
                              OperatorId = idOperator,
                              AccountingGroupID = Convert.ToInt32(p.Field<int>("AccountingGroupID")),
                              AccountingGroupDesc = p.Field<String>("AccountingGroupDesc").ToString()
                          }
                   ).ToList();
            result.Insert(0, defaultAccountGroup);
            return result; 
        }

       

        public List<City> GetCityListFromExternalSource()
        {
            return null;  
        }


        /// <summary>
        /// GetFreeOfficeLineId
        /// </summary>
        /// <param name="routeLine"></param>
        /// <param name="newRouteNumber"></param>
        /// <param name="isArrivePlanning"> </param>
        /// <returns></returns>
        public Dictionary<string,object>  GetFreeOfficeLineId(RouteLine routeLine, int newRouteNumber, bool? isArrivePlanning)
        {
            var stationIdTable = GetStationIdTable(routeLine);
            var dtJunctionIds = GetDtJunctionIds(routeLine);
            var dtCityId = GetCitiIdsTable(routeLine);

            var inputParameter = new Dictionary<string, object>
                                     {
                                         {"OperatorLineId", newRouteNumber},
                                         {"OperatorId", GlobalData.LoginUser.UserOperator.IdOperator},
                                         {"DistrictId", routeLine.IdZoneHead},
                                         {"DistrictSecId", routeLine.IdZoneHead},
                                         {"CityIds", dtCityId},
                                         {"StationIds", stationIdTable},
                                         {"JunctionIds", dtJunctionIds}
                                     };

            //Logger.LoggerManager.WriteToLog(string.Format("OperatorLineId {0}", newRouteNumber));
            //Logger.LoggerManager.WriteToLog(string.Format("OperatorId {0}", GlobalData.LoginUser.UserOperator.IdOperator));
            //Logger.LoggerManager.WriteToLog(string.Format("DistrictId {0}", routeLine.IdZoneHead));
            //Logger.LoggerManager.WriteToLog(string.Format("DistrictSecId {0}", routeLine.IdZoneHead));
            //Logger.LoggerManager.WriteToLog(string.Format("CityIds rows count {0}", dtCityId.Rows.Count));
            //Logger.LoggerManager.WriteToLog(string.Format("StationIds rows count {0}", stationIdTable.Rows.Count));
            //Logger.LoggerManager.WriteToLog(string.Format("JunctionIds rows  count {0}", dtJunctionIds.Rows.Count));

            if (isArrivePlanning.HasValue && isArrivePlanning.Value)
            {
                inputParameter.Add("IsArrivePlanning", true);
            }
            
            var outputParameter = new Dictionary<string, SqlParameter>
                                     {
                                         {"OfficeLineId", new SqlParameter
                                            {
                                                ParameterName = "OfficeLineId",
                                                Value = -1,
                                                SqlDbType = SqlDbType.Int,
                                                Direction = ParameterDirection.Output
                                            }
                                         },
                                         {"ErrorDesc", new SqlParameter
                                            {
                                                ParameterName = "ErrorDesc",
                                                Value = string.Empty,
                                                SqlDbType = SqlDbType.NVarChar,
                                                Size = -1,
                                                Direction = ParameterDirection.Output
                                            }},
                                     };
            return SQLServerHelper.GetOutPutParametersDictionary("[import].[ExternalImport_GetFreeOfficeLineId]", inputParameter, outputParameter, ConnectionString);

        }

        private static DataTable GetCitiIdsTable(RouteLine routeLine)
        {
            var dtCityId = new DataTable();
            dtCityId.Columns.Add(new DataColumn("CityId", typeof(int)));
            if (routeLine.CityIds.IsListFull())
            {
                foreach (var cityId in routeLine.CityIds)
                {
                    var row = dtCityId.NewRow();
                    row["CityId"] = cityId;
                    dtCityId.Rows.Add(row);
                }
            }
            dtCityId.AcceptChanges();
            return dtCityId;
        }

        private static DataTable GetDtJunctionIds(RouteLine routeLine)
        {
            var dtJunctionIds = new DataTable();
            dtJunctionIds.Columns.Add(new DataColumn("JunctionId", typeof(long)));
            if (routeLine.JunctionIds.IsListFull())
            {
                foreach (var junctionId in routeLine.JunctionIds)
                {
                    var row = dtJunctionIds.NewRow();
                    row["JunctionId"] = junctionId;
                    dtJunctionIds.Rows.Add(row);
                }
            }
            dtJunctionIds.AcceptChanges();
            return dtJunctionIds;
        }

        private static DataTable GetStationIdTable(RouteLine routeLine)
        {
            var dtStationId = new DataTable();
            dtStationId.Columns.Add(new DataColumn("StationId", typeof (int)));
            var rsList = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == routeLine.ID);
            if (rsList.IsListFull())
            {
                var orderedList = rsList.OrderBy(o => o.Milepost).Where(o=>o.PhysicalStop!=null).Select(o=> Convert.ToInt32(o.PhysicalStop.StationCatalog)).Distinct().ToList();
                orderedList.ForEach(stationId => 
                {
                    var row = dtStationId.NewRow();
                    row["StationId"] = stationId;
                    dtStationId.Rows.Add(row);
                });
            }
            dtStationId.AcceptChanges();
            return dtStationId;
        }

        public List<OperatorValidateLineExportDate> GetOperatorValidateLineExportDate()
        {
            throw new NotImplementedException();
        }

        public void SaveOperatorValidateLineExportDate(List<OperatorValidateLineExportDate> operatorValidateLineExportDates)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInternalBaseDal Members


        public Dictionary<int, ClusterToZone> GetClusterToZoneDictionary()
        {
            var result = (from p in SQLServerHelper.GetData("[dbo].[TcClusterId]", null, ConnectionString).AsEnumerable()
                        select new{
                            ClusterID = p.Field<int>("ClusterId"),
                            ClusterName = p.Field<string>("Description"),
                            MainZoneId = p.Field<int>("MainDistrictId"),
                            MainZoneName = p.Field<string>("MainDistrictName"),
                            SubZoneId = p.Field<int>("MainDistrictId"),
                            SubZoneName = p.Field<string>("MainDistrictName")}).ToList().OrderBy(p=>p.ClusterID);
            var clusterToZones = new Dictionary<int, ClusterToZone>(); 
            if (result.Any())
            {
                foreach (var res in result)
                {
                    if (!clusterToZones.ContainsKey(res.ClusterID))
                    {
                        clusterToZones.Add(res.ClusterID, new ClusterToZone
                                     {
                                         ClusterID = res.ClusterID,
                                         ClusterName = res.ClusterName,
                                         ClusterStateList = new List<ClusterState>
                                                                {
                                                                    new ClusterState
                                                                        {
                                                                            MainZoneId = res.MainZoneId,
                                                                            MainZoneName = res.MainZoneName,
                                                                            SubZoneId = res.SubZoneId,
                                                                            SubZoneName = res.SubZoneName
                                                                        }
                                                                },
                                     });

                    }
                    else
                    {
                        var res1 = res;
                        if (!clusterToZones[res.ClusterID].ClusterStateList.Exists(p=>p.MainZoneId == res1.MainZoneId))
                              clusterToZones[res.ClusterID].ClusterStateList.Add(new ClusterState
                                                       {
                                                           MainZoneId = res.MainZoneId,
                                                           MainZoneName = res.MainZoneName,
                                                           SubZoneId = res.SubZoneId,
                                                           SubZoneName = res.SubZoneName
                                                       });
                    }
                }
            }
            return clusterToZones;
        }

        #endregion
    }
}
