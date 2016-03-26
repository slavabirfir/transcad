using System;
using System.Collections.Generic;
using BLEntities.Entities;
using BLEntities.Accessories;

namespace IDAL
{
    public interface  ITransCadMunipulationDataDAL
    {
       #region Raw Data


       Dictionary<int, RouteLine> GetRouteLinesRawDataDictionary();
       Dictionary<int, PhysicalStop> GetPhysicalStopsRawDataDictionary();
       Dictionary<int, List<RouteStop>> GetRouteStopsRawDataDictionary();

       TransCadCurrentEnvoromnetInfo GetTransCadCurrentEnvoromnetInfo();
       TransCadCurrentEnvoromnetInfo EgedGetTransCadCurrentEnvoromnetInfo();
       #endregion

       #region Map Manupulation
       void SetMap(string mapName); 


       void ExportEndPointsAsCsvFile(string fileName);

       string GetMapTitle(string map_name);
       void SetMapLabelOptions(); 
       void ShowStreetName(string fieldName, bool isVisible);
       void ShowStreetFlow(bool isVisible);
       string VerifyRouteSystem(string rs_path, string verification_level);
       void ZoomToLayerFeutureById(string layerName, List<int> IDs, string title);
       //void EgedZoomToLayerFeutureById(string layerName, List<int> IDs, string title, string whereClass);
       //void EgedZoomToStopLayerFeutureById(string layerName, List<Coord> coordinates, string title, string whereClass); 

       void SetReadOnlyFieldSet(string layer, List<string> fieldSet, bool isReadOnly);
       void SetLabelOnMap(string layerName,bool isVisible, string fieldName,bool isSelectAdded, string imagePath);
       void SetLabelOnMap(string layerName, bool isVisible, string fieldName, bool isSelectAdded);
       void InitMapResize(string layername);
       string GetMapUnits(string layerFile);
       void SetMapUnits(string units, string routeSystem);
       void SaveMapToImage(string fileName,string fileFormat);
       bool SaveRouteZipFileInfo(int idRoute, string zipFile, string layerName);
       List<string> GetLayersName();
       List<Layer> FillLayerList();

       List<string> GetMapLayers(string mapName, string layerType);
       bool IsLayerExists(string layerName); 
       void DropLayer(string mapName, string layerName);
       bool AddLayer(string mapName, string layerName, string dbName, string dbLayerName, object options);
       void RenameLayer(string maplayer, string newLayerName, object options);

       void RedrawMap(string mapName);
       void SetLineColor(string layerName, int red, int green, int blue);
       void SetIconColor(string layerName, int red, int green, int blue);
       void SetBorderColor(string layerName, int red, int green, int blue);
       void SetLineWidth(string layerName, decimal linewidth);
       void SetLineStyle(string layerName);
       void SetIconSize(string layerName, decimal pointsize);
       void SetBorderWidth(string layerName, decimal borderwidth);
       void SetIcon(string layerName, string icon_type, string name, decimal sizeorindex);
       string GetStopCity(int longitude, int latitude, string areaLayer);
       void SetArrowTopology(string layer);
       void ReloadRouteSystem(String routeSystem);
       bool IsEgedWorkSpaceInTranscad();
       bool IsRouteSystemInTranscad();
       void OpenWorkspace(string wsFileName);

       List<string> GetMapNames();
       void CloseMap(string mapName);

        bool IsTranscadActive(); 

       #endregion

       #region Data Manipulation

       void CloseConnection();

       bool DeleteRouteLine(RouteLine routeLine, string layer);
       bool DeleteRouteStop(RouteStop routeStop, string layer);
       List<string> GetLayerFields(string layerName);
       void ReCreateTableStructure(string layerRouteLineName, string layerRouteStopName, string phisicalStopsLayerName);
       event EventHandler<RecreatedArgs> RouteLineWasRecreated;
       List<PriceZoneArea> GetPriceZoneAreas(string nodeLayerName); 
       bool SaveRouteLine(RouteLine routeLine, string layerName);
       bool SaveRouteStop(RouteStop routeStop, string layerName);
       bool SavePhisicalStopStop(PhysicalStop physicalStop, string layerName);
       #endregion

       #region Route Line
       decimal CalculateRouteLine(RouteLine rl,string layerName,string linkLayer);
       List<RouteLink> GetRouteLinkList(RouteLine routeLine, string layerName, string linkLayer);
       void ExportFile(string layerName, string fileName);
       #endregion

       #region Base Data
       List<City> GetCities(string nodeLayerName);
       List<Link> GetLinks(string nodeLayerName,string endPoints,string mapLayer);
       List<Node> GetNodes(string nodeLayerName,string cityNameLayer);
       List<Stop> GetStops(string routeSystemLayer,string cityNameLayer, string linkLayer);
       //void Exportzg_ktaim(string fileName, ref string message);
       //void Exportzg_mokdim(string fileName, ref string message);
       //void Exportzg_tachana(string fileName, ref string message);
       void SetOffSet(string lyr_set_name, string offset_type, double? offset);
       //List<ISSTR> GetISSTR(string mapLayerName);
       int GetDistance(Coord c1, Coord c2);
       Coord GetCoordinate(int idPoint,string layerName);
       List<LinkStop> GetStationLinkID(string rs_layer, RouteLine routeLine);
       List<RouteLink> GetRouteLinkListForDuration(RouteLine routeLine, string layerName, string linkLayer, string endPointLayer);

       void RunExportMacro(string macroName, string uiFile, List<object> parameters);
       
       Object RunTranscadMacro(string macroName, string uiFile, List<object> parameters);  
       bool IsEndPointHasEmptyCityOrTypeDavid(string endPoints_layer);

       #endregion

       #region PriceListPolygon
        List<PriceArea> GetAllPriceListPolygon(string priceListPolygonName);
        bool UpdatePriceListPolygon(PriceArea priceArea, string priceListPolygonName);
        int GetNearestIdPriceAreaRecordToPhysicalStop(PhysicalStop stop, string areaLayer,bool realCoordinate,string returnField);
        string GetNearestIdMunAreasToPhysicalStop(string uiFile, Coord coord, string areaLayer, int threshhold);
        List<string> PriceAreaReadOnlyFields { get; }
        void AddRouteSystemLayer(string mapName, string routeSystemName, string pathToRouteSystemFile,object options);
        #endregion
       #region Structure
        Dictionary<String, String> GetTableStructure(string layerName);
        #endregion


        
    }
}
