using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Model
{
    /// <summary>
    /// Const
    /// </summary>
    public abstract class GlobalConst
    {
        public const string baseRouteType = "TcGetLineType";
        public const string baseSeasonal ="";//   "baseSeasonal";
        public const string baseServiceType = "TcGetServiceType";
        public const string baseExclusivityLine = "TcGetExclusivityLine";
        public const string baseVarConverter = "TcGetVarConvert";
        public const string baseVehicleSize = "TcGetVehicleSize";
        public const string baseVehicleType = "TcGetVehicleType";

        public const string baseStationStatus = "TcGetStationStatus";
        public const string baseStationTipus = "TcGetStationType";

        public const string tblOperatorCluster = "TcGetClusterByOperatorId";
        //public const string baseZone = "SpTcMahoz";
        public const string baseStationType = "TcGetStationActivityType";
        public const string ShapeExtentionSHP = ".shp";
        public const string ShapeExtentionSHX =".shx";
        public const string Miles = "Miles";
        public const string Kilometeres = "Kilometeres";
        public const double MileToKilometerCoeffitient = 1.609344;
        public const string ENTITIES = "entities";
        public const string Infrastructure = "Infrastructure";
    }
}
