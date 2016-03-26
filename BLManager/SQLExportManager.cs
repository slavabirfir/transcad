using System;
using System.Collections.Generic;
using BLEntities.Entities;
using BLEntities.Accessories;
using IBLManager;
using IDAL;
using DAL;
using ExportConfiguration;
using Utilities;
 
namespace BLManager
{
    public class SQLExportManager 
    {
        public List<int> YearsList { get; set; }
        readonly ITransCadMunipulationDataDAL _dalTranCad = new TransCadMunipulationDataDAL();
        readonly IExportBaseBlManager _exportBlManager = new ExportBaseToSQLBL();
        public ExportToSQL ExportToSqlInfo { get; set; }
        public SQLExportManager() 
        { 
            ExportToSqlInfo = new ExportToSQL(); 
            int currentYear = DateTime.Today.Year; 
            const int yearBack = 5;
            const int yearForward = 10;
            YearsList = new List<int>();
            for (int i = currentYear - yearBack; i < currentYear + yearForward; i++)
            {
                YearsList.Add(i);
            }
            ExportToSqlInfo.Year = currentYear;
        }
        /// <summary>
        /// Export Data
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ExportData(ref string message)
        {
            //exportBLManager.TakeLastExportedInfrastructure = exportToSQLInfo.TakeLastExportedInfrastructure;
            return _exportBlManager.ExportBaseData(ref message);
        }

            /// <summary>
        /// Write Infractructure
        /// </summary>
        public void WriteInfractructure()
        {
            List<City> cityList  = _dalTranCad.GetCities(ExportConfigurator.GetConfig().MunLayer);
            string json = JSONHelper.JsonSerializeToString<List<City>>(cityList);
            IoHelper.WriteToFile(GetFileName(ExportBaseToSQLBL.CITYDAT), json);
            
            List<Node> nodeList = _dalTranCad.GetNodes(ExportConfigurator.GetConfig().Endpoints, ExportConfigurator.GetConfig().MunLayer);
            json = JSONHelper.JsonSerializeToString<List<Node>>(nodeList);
            IoHelper.WriteToFile(GetFileName(ExportBaseToSQLBL.NODEDAT), json);

            List<Stop> stopList = _dalTranCad.GetStops(ExportConfigurator.GetConfig().RouteSystemLayerName, ExportConfigurator.GetConfig().MunLayer, ExportConfigurator.GetConfig().MapLayerName);
            json = JSONHelper.JsonSerializeToString<List<Stop>>(stopList);
            IoHelper.WriteToFile(GetFileName(ExportBaseToSQLBL.STOPDAT), json);
        }
        
        /// <summary>
        /// Get File Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFileName(string fileName)
        {
            return IoHelper.GetFileName(fileName, ExportConfigurator.GetConfig().DataFolder);
        }

    }
}
