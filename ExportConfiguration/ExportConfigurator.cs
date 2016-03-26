using System.Configuration;

namespace ExportConfiguration
{
    public class ExportConfigurator : ConfigurationSection
    {

        /// <summary>
        /// Returns an ASPNET2Configuration instance
        /// </summary>
        public static ExportConfigurator GetConfig()
        {
            return ConfigurationManager.GetSection("TransportMinistery/ExportConfigurator") as ExportConfigurator;
        }

        [ConfigurationProperty("danLikeOperators", IsRequired = true)]
        public string DanLikeOperators
        {
            get
            {
                return this["danLikeOperators"].ToString();
            }
        }

        [ConfigurationProperty("convertIsStrShapeToTABMacro", IsRequired = true)]
        public string ConvertIsStrShapeToTabMacro
        {
            get
            {
                return this["convertIsStrShapeToTABMacro"].ToString();
            }
        }

        [ConfigurationProperty("convertIsStrShapeToTABMacroUIFile", IsRequired = true)]
        public string ConvertIsStrShapeToTabMacroUiFile
        {
            get
            {
                return this["convertIsStrShapeToTABMacroUIFile"].ToString();
            }
        }


        [ConfigurationProperty("convertIsStrShapeToTABogr2ogrPath", IsRequired = true)]
        public string ConvertIsStrShapeToTaBogr2OgrPath
        {
            get
            {
                return this["convertIsStrShapeToTABogr2ogrPath"].ToString();
            }
        }


        [ConfigurationProperty("convertIsStrShapeToTABFtpHost", IsRequired = true)]
        public string ConvertIsStrShapeToTabFtpHost
        {
            get
            {
                return this["convertIsStrShapeToTABFtpHost"].ToString();
            }
        }

        [ConfigurationProperty("convertIsStrShapeToTABFtpUser", IsRequired = true)]
        public string ConvertIsStrShapeToTabFtpUser
        {
            get
            {
                return this["convertIsStrShapeToTABFtpUser"].ToString();
            }
        }

        [ConfigurationProperty("convertIsStrShapeToTABFtpPassword", IsRequired = true)]
        public string ConvertIsStrShapeToTabFtpPassword
        {
            get
            {
                return this["convertIsStrShapeToTABFtpPassword"].ToString();
            }
        }






        [ConfigurationProperty("egedNispah2LoadMapMacroFile", IsRequired = true)]
        public string EgedNispah2LoadMapMacroFile
        {
            get
            {
                return this["egedNispah2LoadMapMacroFile"].ToString();
            }
        }
        [ConfigurationProperty("egedNispah2LoadMapMacroName", IsRequired = true)]
        public string EgedNispah2LoadMapMacroName
        {
            get
            {
                return this["egedNispah2LoadMapMacroName"].ToString();
            }
        }


        [ConfigurationProperty("egedRouteSystemLayerName", IsRequired = true)]
        public string EgedRouteSystemLayerName
        {
            get
            {
                return this["egedRouteSystemLayerName"].ToString();
            }
        }

        [ConfigurationProperty("egedFileSourceFolder", IsRequired = true)]
        public string EgedFileSourceFolder
        {
            get
            {
                return this["egedFileSourceFolder"].ToString();
            }
        }

        [ConfigurationProperty("egedFarAwayStationMacro", IsRequired = true)]
        public string EgedFarAwayStationMacro
        {
            get
            {
                return this["egedFarAwayStationMacro"].ToString();
            }
        }



        [ConfigurationProperty("operatorsConvertNotDB", IsRequired = true)]
        public string OperatorsConvertNotDB
        {
            get
            {
                return this["operatorsConvertNotDB"].ToString();
            }
        }

        [ConfigurationProperty("dataFolder", IsRequired = true)]
        public string DataFolder
        {
            get
            {
                return this["dataFolder"].ToString();
            }
        }


        [ConfigurationProperty("updateEndPointsMacroUIFile", IsRequired = true)]
        public string UpdateEndPointsMacroUIFile
        {
            get
            {
                return this["updateEndPointsMacroUIFile"].ToString();
            }
        }

        [ConfigurationProperty("updatePhysicalStopsMacroUIFile", IsRequired = true)]
        public string UpdatePhysicalStopsMacroUIFile
        {
            get
            {
                return this["updatePhysicalStopsMacroUIFile"].ToString();
            }
        }

        [ConfigurationProperty("junctionVersionUIFile", IsRequired = true)]
        public string JunctionVersionUIFile
        {
            get
            {
                return this["junctionVersionUIFile"].ToString();
            }
        }

        [ConfigurationProperty("transactionLevel", IsRequired = true)]
        public int TransactionLevel
        {
            get
            {
                return (int)this["transactionLevel"];
            }
        }

        [ConfigurationProperty("pathToExportConstraintViolationFolder", IsRequired = true, DefaultValue = "")]
        public string PathToExportConstraintViolationFolder
        {
            get
            {
                return this["pathToExportConstraintViolationFolder"].ToString();
            }
        }


        [ConfigurationProperty("eggedExportSationFolder", IsRequired = true)]
        public string EggedExportSationFolder
        {
            get
            {
                return this["eggedExportSationFolder"].ToString();
            }
        }

        [ConfigurationProperty("showLineInStationToolBox", IsRequired = true)]
        public string ShowLineInStationToolBox
        {
            get
            {
                return this["showLineInStationToolBox"].ToString();
            }
        }

        [ConfigurationProperty("priceAreaPolygonMacroName", IsRequired = true)]
        public string PriceAreaPolygonMacroName
        {
            get
            {
                return this["priceAreaPolygonMacroName"].ToString();
            }
        }
        [ConfigurationProperty("priceAreaPolygonMacroUIFile", IsRequired = true)]
        public string PriceAreaPolygonMacroUIFile
        {
            get
            {
                return this["priceAreaPolygonMacroUIFile"].ToString();
            }
        }

        [ConfigurationProperty("polygonCalcAreaUI", IsRequired = true)]
        public string PolygonCalcAreaUI
        {
            get
            {
                return this["polygonCalcAreaUI"].ToString();
            }
        }

        [ConfigurationProperty("polygonCalcAreaMacroName", IsRequired = true)]
        public string PolygonCalcAreaMacroName
        {
            get
            {
                return this["polygonCalcAreaMacroName"].ToString();
            }
        }


        [ConfigurationProperty("priceAreaPolygonFileFolder", IsRequired = true)]
        public string PriceAreaPolygonFileFolder
        {
            get
            {
                return this["priceAreaPolygonFileFolder"].ToString();
            }
        }
        [ConfigurationProperty("priceAreaPolygonName", IsRequired = true)]
        public string PriceAreaPolygonName
        {
            get
            {
                return this["priceAreaPolygonName"].ToString();
            }
        }


        [ConfigurationProperty("priceAreaPolygonToolBox", IsRequired = true)]
        public string PriceAreaPolygonToolBox
        {
            get
            {
                return this["priceAreaPolygonToolBox"].ToString();
            }
        }


        [ConfigurationProperty("routeSystemPointsUI", IsRequired = true)]
        public string RouteSystemPointsUI
        {
            get
            {
                return this["routeSystemPointsUI"].ToString();
            }
        }

        [ConfigurationProperty("routeSystemPointsMacro", IsRequired = true)]
        public string RouteSystemPointsMacro
        {
            get
            {
                return this["routeSystemPointsMacro"].ToString();
            }
        }

        [ConfigurationProperty("getRLInfoMacro", IsRequired = true)]
        public string GetRlInfoMacro
        {
            get
            {
                return this["getRLInfoMacro"].ToString();
            }
        }
        [ConfigurationProperty("getRLInfoUI", IsRequired = true)]
        public string GetRlInfoUi
        {
            get
            {
                return this["getRLInfoUI"].ToString();
            }
        }

        [ConfigurationProperty("calcCloseAreasToStationMacro", IsRequired = true)]
        public string CalcCloseAreasToStationMacro
        {
            get
            {
                return this["calcCloseAreasToStationMacro"].ToString();
            }
        }

        [ConfigurationProperty("trafficLicensingURL", IsRequired = true)]
        public string TrafficLicensingUrl
        {
            get
            {
                return (string)this["trafficLicensingURL"];
            }
        }

        [ConfigurationProperty("transcadConnectivityTimerPeriod", IsRequired = false)]
        public int TranscadConnectivityTimerPeriod
        {
            get
            {
                return (int)this["transcadConnectivityTimerPeriod"];
            }
        }


        [ConfigurationProperty("workspacePriceListAllOperstorsFileName", IsRequired = true)]
        public string WorkspacePriceListAllOperstorsFileName
        {
            get
            {
                return (string)this["workspacePriceListAllOperstorsFileName"];
            }
        }
        [ConfigurationProperty("workspacePriceListAllOperstorsMapName", IsRequired = true)]
        public string WorkspacePriceListAllOperstorsMapName
        {
            get
            {
                return (string)this["workspacePriceListAllOperstorsMapName"];
            }
        }


        [ConfigurationProperty("trafficLicensingZoneImportURL", IsRequired = true)]
        public string TrafficLicensingZoneImportURL
        {
            get
            {
                return (string)this["trafficLicensingZoneImportURL"];
            }
        }


        [ConfigurationProperty("trafficLicensingPlanningURL", IsRequired = true)]
        public string TrafficLicensingPlanningURL
        {
            get
            {
                return (string)this["trafficLicensingPlanningURL"];
            }
        }
        [ConfigurationProperty("trafficLicensingPlanningZoneImportURL", IsRequired = true)]
        public string TrafficLicensingPlanningZoneImportURL
        {
            get
            {
                return (string)this["trafficLicensingPlanningZoneImportURL"];
            }
        }

        [ConfigurationProperty("monitorTimerInterval", IsRequired = true)]
        public int MonitorTimerInterval
        {
            get
            {
                return (int)this["monitorTimerInterval"];
            }
        }

        [ConfigurationProperty("isVerifyRS", IsRequired = true)]
        public bool IsVerifyRS
        {
            get
            {
                return bool.Parse(this["isVerifyRS"].ToString());
            }
        }


        [ConfigurationProperty("minLimitDistanceBetweanStations", IsRequired = true)]
        public int MinLimitDistanceBetweanStations
        {
            get
            {
                return int.Parse(this["minLimitDistanceBetweanStations"].ToString());
            }
        }


        [ConfigurationProperty("phisicalStopsLayerName", DefaultValue = "Phisical Stops", IsRequired = true)]
        public string PhisicalStopsLayerName
        {
            get
            {
                return this["phisicalStopsLayerName"].ToString();
            }
        }
        [ConfigurationProperty("routeStopsLayerName", DefaultValue = "Stops", IsRequired = true)]
        public string RouteStopsLayerName
        {
            get
            {

                return this["routeStopsLayerName"].ToString();
            }

        }
        [ConfigurationProperty("mappingServer", DefaultValue = "TransCAD", IsRequired = true)]
        public string MappingServer
        {
            get
            {
                return this["mappingServer"].ToString();
            }
        }


        [ConfigurationProperty("endpoints", DefaultValue = "Endpoints", IsRequired = true)]
        public string Endpoints
        {
            get
            {
                return this["endpoints"].ToString();
            }
        }

        [ConfigurationProperty("routeSystemLayerName", DefaultValue = "Route System", IsRequired = true)]
        public string RouteSystemLayerName
        {
            get
            {
                return this["routeSystemLayerName"].ToString();
            }
        }
        [ConfigurationProperty("mapLayerName", DefaultValue = "is_str", IsRequired = true)]
        public string MapLayerName
        {
            get
            {
                return this["mapLayerName"].ToString();
            }
        }
        [ConfigurationProperty("mapLayerFile", IsRequired = true)]
        public string MapLayerFile
        {
            get
            {
                return this["mapLayerFile"].ToString();
            }
        }




        [ConfigurationProperty("munLayer", IsRequired = true)]
        public string MunLayer
        {
            get
            {
                return this["munLayer"].ToString();
            }
        }


        [ConfigurationProperty("isAuthenticationserviceActive", DefaultValue = true, IsRequired = true)]
        public bool IsAuthenticationserviceActive
        {
            get
            {
                return (bool)this["isAuthenticationserviceActive"];
            }
        }


        [ConfigurationProperty("runValidationCheckOnStartUp", DefaultValue = true, IsRequired = true)]
        public bool RunValidationCheckOnStartUp
        {
            get
            {
                return (bool)this["runValidationCheckOnStartUp"];
            }
        }

        [ConfigurationProperty("lengthConverter", IsRequired = true)]
        public double LengthConverter
        {
            get
            {
                return double.Parse(this["lengthConverter"].ToString());
            }
        }

        [ConfigurationProperty("logicalDataSplitDevider", IsRequired = true, DefaultValue = "|")]
        public string LogicalDataSplitDevider
        {
            get
            {
                return this["logicalDataSplitDevider"].ToString();
            }
        }

        [ConfigurationProperty("stopsOffsetPoints", IsRequired = true)]
        public double StopsOffsetPoints
        {
            get
            {
                return double.Parse(this["stopsOffsetPoints"].ToString());
            }
        }


        [ConfigurationProperty("labelFont", IsRequired = true)]
        public string LabelFont
        {
            get
            {
                return this["labelFont"].ToString();
            }
        }


        [ConfigurationProperty("exportMacroName", IsRequired = true)]
        public string ExportMacroName
        {
            get
            {
                return this["exportMacroName"].ToString();
            }
        }

        [ConfigurationProperty("exportMacroUIFile", IsRequired = true)]
        public string ExportMacroUIFile
        {
            get
            {
                return this["exportMacroUIFile"].ToString();
            }
        }


        [ConfigurationProperty("shapeFileFolder", IsRequired = true)]
        public string ShapeFileFolder
        {
            get
            {
                return this["shapeFileFolder"].ToString();
            }
        }
        [ConfigurationProperty("developmentEnv", IsRequired = true)]
        public string DevelopmentEnv
        {
            get
            {
                return this["developmentEnv"].ToString();
            }
        }

        [ConfigurationProperty("transcadexe", IsRequired = true)]
        public string Transcadexe
        {
            get
            {
                return this["transcadexe"].ToString();
            }
        }

        [ConfigurationProperty("taxiOperatorId", IsRequired = true)]
        public int TaxiOperatorId
        {
            get
            {
                return int.Parse(this["taxiOperatorId"].ToString());
            }
        }

        [ConfigurationProperty("trainOperatorId", IsRequired = true)]
        public int TrainOperatorId
        {
            get
            {
                return int.Parse(this["trainOperatorId"].ToString());
            }
        }

        [ConfigurationProperty("tempPathForAllUsers", IsRequired = true)]
        public string TempPathForAllUsers
        {
            get
            {
                return this["tempPathForAllUsers"].ToString();
            }
        }

        [ConfigurationProperty("operatorsIDsNotCalcNispah2Distances", IsRequired = false)]
        public string OperatorsIDsNotCalcNispah2Distances
        {
            get
            {
                return this["operatorsIDsNotCalcNispah2Distances"].ToString();
            }
        }

        [ConfigurationProperty("priceAreaHistoryPriceZoneAreaFolder", IsRequired = true)]
        public string PriceAreaHistoryPriceZoneAreaFolder
        {
            get
            {
                return this["priceAreaHistoryPriceZoneAreaFolder"].ToString();
            }
        }

        [ConfigurationProperty("priceAreaPriceZoneAreaFolder", IsRequired = true)]
        public string PriceAreaPriceZoneAreaFolder
        {
            get
            {
                return this["priceAreaPriceZoneAreaFolder"].ToString();
            }
        }

        [ConfigurationProperty("priceAreaTemporaryPriceZoneAreaFolder", IsRequired = true)]
        public string PriceAreaTemporaryPriceZoneAreaFolder
        {
            get
            {
                return this["priceAreaTemporaryPriceZoneAreaFolder"].ToString();
            }
        }
        
        [ConfigurationProperty("priceAreaZonesFileFolder", IsRequired = true)]
        public string PriceAreaZonesFileFolder
        {
            get
            {
                return this["priceAreaZonesFileFolder"].ToString();
            }
        }

        [ConfigurationProperty("priceAreaZonesAreaName", IsRequired = true)]
        public string PriceAreaZonesAreaName
        {
            get
            {
                return this["priceAreaZonesAreaName"].ToString();
            }
        }
        

    }
}
