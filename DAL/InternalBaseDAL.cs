using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using BLEntities.Accessories;
using Utilities;
using System.Data.Linq;
using BLEntities.Model;
using BLEntities.Entities;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using BLEntities.SQLServer;

namespace DAL
{
    public class InternalBaseDAL : IInternalBaseDAL
    {
        readonly string connectionString = ConfigurationHelper.GetDBConnectionString("OperatorConnectionString");
        readonly string entities = "Entities";
        #region Constructor
        public InternalBaseDAL()
        {

        }
        #endregion
        #region private
        DataBaseDataContext dbContext = null;
        #endregion
        #region IInternalBaseDAL Members


        

        public List<BLEntities.Accessories.Operator> GetOperatorList()
        {
            using (dbContext = new DataBaseDataContext(connectionString))
            {
                return (from p in dbContext.procGetAlltblOperator()
                        select new BLEntities.Accessories.Operator
                       {
                           Email = p.Email,
                           EnglishName = p.EnglishOperatorName,
                           IdOperator = p.IdOperator,
                           OperatorName = p.OperatorName,
                           PathToRSTFile = p.PathToRSTFile,
                           PathToRSTRouteLineExportFolder = p.PathToRSTRouteLineExportFolder
                       }).ToList<BLEntities.Accessories.Operator>();
            }
        }
        /// <summary>
        /// Set Active Operator
        /// </summary>
        /// <param name="user"></param>
        public void SetActiveOperator(LoginUser user)
        {
            if (user!=null)
            {
                using (dbContext = new DataBaseDataContext(connectionString))
                {
                    dbContext.procUpdateinternalUser(
                        (int?)user.IdUser,
                        user.UserOperator!=null ?  (int?)user.UserOperator.IdOperator : 0,
                        user.UserName,
                        (bool?)user.IsSuperViser,
                        user.FirstName,
                        user.LastName,
                        user.FirmName,
                        user.Telephone);
                }
            }
        }


        public LoginUser GetLoginUserInfo(string userGroupName)
        {
            LoginUser loginUser = null;
            string currentUserLogin = CurrentSettingsHelper.GetLoginUserName();
            using (dbContext = new DataBaseDataContext(connectionString))
            {
                var userData = dbContext.internalUserGetByUserName(currentUserLogin).SingleOrDefault<internalUserGetByUserNameResult>();
                if (userData != null)
                {
                    loginUser = new LoginUser
                    {
                        UserName = currentUserLogin,
                        UserOperator = new Operator
                        {
                            IdOperator = userData.IdOperator
                        },
                        IdUser = userData.IdUser,
                        IsSuperViser = userData.IsSuperViser,
                        FirmName = userData.FirmName,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Telephone = userData.Telephone 
                    };
                    var operatorData = dbContext.tblGetOperatorIdOperator(loginUser.UserOperator.IdOperator).SingleOrDefault<tblGetOperatorIdOperatorResult>();
                    if (operatorData != null)
                    {
                        loginUser.UserOperator.OperatorName = operatorData.OperatorName;
                        loginUser.UserOperator.Email = operatorData.Email;
                        loginUser.UserOperator.PathToRSTRouteLineExportFolder = operatorData.PathToRSTRouteLineExportFolder;
                        loginUser.UserOperator.PathToRSTFile = operatorData.PathToRSTFile;
                        
                        loginUser.UserOperator.EnglishName= operatorData.EnglishOperatorName; 
                    }

                }
            }
            return loginUser;
        }

        private string ExtractTableName(string typeName)
        {
            typeName = typeName.Replace("System.Data.Linq.Table`1[DAL.", string.Empty);
            typeName = typeName.Replace("]", string.Empty);
            return typeName;
        }

        public Dictionary<string, List<BaseTableEntity>> GetBaseTableEntityList(int idOperator)
        {
            Dictionary<string, List<BaseTableEntity>> data = new Dictionary<string, List<BaseTableEntity>>();
            using (dbContext = new DataBaseDataContext(connectionString))
            {
                
                var baseRouteTypesList = (from p in dbContext.baseRouteTypes
                                          select new BaseTableEntity { ID = p.IdRouteType, Name = p.RouteTypeName }).ToList();
                data.Add(ExtractTableName(dbContext.baseRouteTypes.GetType().ToString()), baseRouteTypesList);

                var baseSeasonalList = (from p in dbContext.baseSeasonals
                                          select new BaseTableEntity { ID = p.IdSeasonal,Name = p.SeasonalName }).ToList();
                data.Add(ExtractTableName(dbContext.baseSeasonals.GetType().ToString()), baseSeasonalList);

                var baseZoneList = (from p in dbContext.baseZones
                                    select new BaseTableEntity { ID = p.IdZone, Name = p.ZoneName }).ToList();
                data.Add(ExtractTableName(dbContext.baseZones.GetType().ToString()), baseZoneList);

                var baseServiceTypeList = (from p in dbContext.baseServiceTypes
                                        select new BaseTableEntity { ID = p.IdServiceType, Name = p.ServiceTypeName }).ToList();
                data.Add(ExtractTableName(dbContext.baseServiceTypes.GetType().ToString()), baseServiceTypeList);

                var baseVehicleSizeList = (from p in dbContext.baseVehicleSizes
                                           select new BaseTableEntity { ID = p.IdVehicleSize, Name = p.VehicleSizeName }).ToList();
                data.Add(ExtractTableName(dbContext.baseVehicleSizes.GetType().ToString()), baseVehicleSizeList);

                var baseVehicleTypeList = (from p in dbContext.baseVehicleTypes
                                           select new BaseTableEntity { ID = p.IdVehicleType, Name = p.VehicleTypeName }).ToList();
                data.Add(ExtractTableName(dbContext.baseVehicleTypes.GetType().ToString()), baseVehicleTypeList);

                var baseVarConverterList = (from p in dbContext.baseVarConverters
                                           select new BaseTableEntity { ID = p.VarNum, Name = p.VarChar }).ToList();
                data.Add(ExtractTableName(dbContext.baseVarConverters.GetType().ToString()), baseVarConverterList);


                var tblClustersList = (from p in dbContext.tblOperatorClusterGetClustersByIdOperator(idOperator)
                                           select new BaseTableEntity { ID = p.IdCluster, Name = p.ClusterName }).ToList();
                data.Add(GlobalConst.tblOperatorCluster, tblClustersList);

                var baseStationTypeList = (from p in dbContext.baseStationTypes
                                           select new BaseTableEntity { ID = p.IdStationType, Name = p.StationTypeName }).ToList();
                data.Add(ExtractTableName(dbContext.baseStationTypes.GetType().ToString()), baseStationTypeList);

                
            }
            
            return data;

        }
        #endregion

        #region XML
        public List<TranslatorEntity> FillBaseTableTranslatorList()
        {
            string entities = this.entities;
            string xmlFile ="BaseTableTranslator.xml";
            string executeFolder =Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location);
            executeFolder = Path.Combine(executeFolder, entities);
            if (!IOHelper.IsFolderExists(executeFolder))
            {
                throw new ApplicationException(string.Format("Application Data folder {0} is not exist", executeFolder)); 
            }
            string fileName = Path.Combine(executeFolder, xmlFile);
            if (!IOHelper.IsFileExists(fileName))
            {
                throw new ApplicationException(string.Format("{0} file is not exists in application Data folder {1}",fileName, executeFolder));
            }
            var f = new { d =4};
            XDocument doc = XDocument.Load(fileName);
            if (doc!=null)
            {
                List<TranslatorEntity> resData = new List<TranslatorEntity>();
                var data = (from p in doc.Descendants("table")
                          select new {
                              EnglishName = p.Attribute("englishname").Value,
                              HebrewName = p.Attribute("hebrewname").Value,
                              RealValueName = p.Element("RealData")==null ? null : p.Element("RealData").Attribute("value").Value ,
                              PossibleValueList =p.Element("RealData") == null ? null : p.Element("RealData").Elements()
                          }).ToList();
                foreach (var item in data)
                {
                    TranslatorEntity element = new TranslatorEntity();
                    element.EnglishName = item.EnglishName;
                    element.HebrewName = item.HebrewName;
                    if (item.PossibleValueList!=null)
                    {
                        List<string> possibleValues = new List<string>(); 
                        element.RealPossibleValues.Add(item.RealValueName,possibleValues);
                        foreach (var it in item.PossibleValueList)
	                    {
                            possibleValues.Add(it.Value); 
	                    }
                    }
                    resData.Add(element);
                }
                return resData; 
            }
            return null;
        }
        

        public List<string> GetDirectionList()
        {
            throw new NotImplementedException();
        }
       
        public bool UpdateOperator(Operator operatorEntity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInternalBaseDAL Members


        public List<BaseLineInfo> GetBaseLine(Operator operatorEntity)
        {
            throw new NotImplementedException();
        }
        public int? GetNewCatalog(int idOperator, int idCluster)
        {
            throw new NotImplementedException();
        }

        

        

        public bool IsCatalogExists(RouteLine routeLine)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
