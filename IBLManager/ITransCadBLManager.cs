using System;
using System.Collections.Generic;
using BLEntities.Entities;
using BLEntities.Accessories;
namespace IBLManager
{
    public interface ITransCadBLManager
    {
        bool SetLoginUserInfoAndVerifyToShowSelectOpersList();
        void BuildModelData(List<RouteLine> routeLines);
        void BuildModelData();
        bool IsStartUpDone { get; set; }
        #region Map Manupulation
        void ZoomToRouteLinesLayerFeutureById( List<RouteLine> entityList);
        void ZoomToRouteStopsLayerFeutureById(List<RouteStop > entityList);
        void ZoomToPhysicalStopsLayerFeutureById(List<PhysicalStop> entityList);
        void SetLabelRouteLines(bool isVisible, string fieldName);
        void SetLabelRouteStops(bool isVisible,bool isbyName, int selectedImageStopLabelIndex);
        void ShowStreetName( bool isVisible);
        void ShowStreetFlow(bool isVisible);
        
        void InitMapResize();
        string GetMapTitle();
        void SaveMapToImage(string fileName);
        #endregion
        #region Validation
        string VerifyRouteSystem();
        bool IsValidTransCadEnvironment(ref string errorString);
        void ValidateData(List<RouteLine> selectedLines);
        void ValidateRouteLineData(List<RouteLine> selectedLines);
        void ValidatePhysicalStopsData();
        void ValidateRouteStopData(List<RouteLine> selectedLines);
        bool IsValidList<T>(List<T> lst) where T : BaseClass;
        bool IsValidDataModel { get; }
        bool IsExportBaseEnabled { get; }
        bool IsAdminFormEnabled { get; }
        bool IsDanLikeOperators { get; }

        List<RouteLine> LinesNotBelongToOperator(List<RouteLine> lines, Operator oper);

        void FillTranscadMetaData(ModelMetaData data);
        string GetSelectedClusterName();
        int InValidListObjectCount<T>(List<T> lst) where T : BaseClass;
        void SetUserOperator(LoginUser user, string opearatorName);
        int NewListObjectCount<T>(List<T> lst) where T : BaseClass;
        void SetRouteStopsOrdinalNumber();
        void SetHoradaInSpecificRouteLine(RouteLine routeLine);

        bool IsExistsHoradaStationInLine(RouteLine routeLine);

        void SetRouteStopsDataInSpecificRouteLine(RouteLine routeLine, StationOrderConst stationOrderConstofChangedRouteStop);
        int ValidListObjectCount<T>(List<T> lst) where T : BaseClass;
        List<T> GetInValidEntities<T>(List<T> lst) where T : BaseClass;
        void ConvertCatalogsTo7Positions();
        #endregion

        #region Data Manipulation
        bool DeleteRouteLine(RouteLine routeLine);
        bool DeleteRouteStop(RouteStop routeStop);
        #endregion
        #region TableStructure
        List<string> GetRouteLineLayerFields();
        List<string> GetRouteStopsLayerFields();
        #endregion
        #region ReCreateTableStructure
        void ReCreateTableStructure( );
        void ShowRecreatedRouteLineReport();
        event EventHandler<RecreatedArgs> RouteLineWasRecreated;
        bool SaveRouteLine(RouteLine routeLine);
        bool SaveRouteStop(RouteStop routeStop);
        void SetOffSet(string lyr_set_name, bool isRouteLineLayer, double? offset);
        bool IsLayerConnectedToRS(string layerName);
        #endregion
        #region Set SQL Data
        void SetBaseRouteLines();
        #endregion
        #region Catalog Converting
        string BuildExportCatalog(RouteLine rl);
        string GetStopCity(RouteStop routeStop);
        #endregion
        #region Time Duration in Seconds
        void CalculateRouteLineStopsDuration(RouteLine routeLine);
        void RunExportMacro(string whereClass, List<RouteLine> routeLineList);
        void RePopulateRouteStopList();
        string GetShapeFileFolder { get; }
        bool IsEgedOperator { get; }
        List<RouteLine> GetRouteLineListWithNotValidExportDate(List<RouteLine> sourse);
        void CloseConnection();

        #endregion
    }
}
