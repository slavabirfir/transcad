using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Accessories;
using BLEntities.Entities;

namespace BLEntities.Model
{
    public static class GlobalData
    {
        static GlobalData()
        {
            RouteModel = new RouteModel();
        }
        public static List<TranslatorEntity> BaseTableTranslatorList { get; set; }   
        public static List<CatalogInfo> CatalogInfolList { get; set; }

        public static List<Operator> OperatorList { get; set; }

        public static RouteModel RouteModel { get; set; }
        public static LoginUser LoginUser { get; set; }
        public static TransCadCurrentEnvoromnetInfo TransCadCurrentEnvoromnetInfo { get; set; }
        public static Dictionary<string,List<BaseTableEntity>> BaseTableEntityDictionary;
        public static List<string> Directions = new List<string>();  
        public static string  MapUnits { get; set; }

        public static List<Layer> LayerList { get; set; }
        public static Dictionary<int, ClusterToZone> ClusterToZoneDictionary { get; set; }


        public static ModelMetaData MetaData { get; set; }
        public static void InitMetaData()
        {
            MetaData = new ModelMetaData
                           {
                               TranscadMetaData = new Dictionary<string, Dictionary<string, string>>(),
                               XMLMetaData = new Dictionary<string, Dictionary<string, string>>()
                           };
        }
    }
}
