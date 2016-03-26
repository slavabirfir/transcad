using System.Collections.Generic;
using BLEntities.Accessories;
using BLEntities.Entities;
using BLEntities.SQLServer;

namespace IDAL
{
    public interface IInternalBaseDal
    {
        //LoginUser GetLoginUserInfo(string groupUserName, enmOperatorGroup operatorGroup);
        Dictionary<string, List<BaseTableEntity>> GetBaseTableEntityList(int idOperator);
        List<Operator> GetOperatorList();
      
        List<OperatorValidateLineExportDate> GetOperatorValidateLineExportDate();
        void SaveOperatorValidateLineExportDate(List<OperatorValidateLineExportDate> operatorValidateLineExportDates);

        List<City> GetCityListFromExternalSource();
        List<TranslatorEntity> FillBaseTableTranslatorList();
        List<string> GetDirectionList();
        List<BaseLineInfo> GetBaseLine(Operator operatorEntity);
        bool UpdateOperator(Operator operatorEntity);
        int? GetNewCatalog(int idOperator, int idCluster);
         
        bool IsCatalogExists(RouteLine routeLine);
        List<AccountingGroup> GetAccountingGroupByOperatorIdAndClusterId(int idOperator, int idCluster);
        bool IsLuzOfLineExists(RouteLine routeLine);

        List<BaseTableEntity> GetClustersByOpearatorId(int opearatorId);

        bool TranscadLoginInsert(TranscadLogin transcadLogin);
        bool TranscadLoginDelete(string userName);
        string TranscadLoginIsFileOpened(string workspaceFile);
        List<TranscadLogin> GetAllTranscadLogins();

        Dictionary<string, object> GetFreeOfficeLineId(RouteLine routeLine, int newRouteNumber, bool? isArrivePlanning);

        Dictionary<int, ClusterToZone> GetClusterToZoneDictionary();

        bool InsertPhysicalLayerTestJurnal(int idOperator, string userName, bool isPassedTest); 
    }
}
