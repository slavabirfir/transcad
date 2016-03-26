using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using BLEntities.Accessories;
using BLEntities.SQLServer;


namespace IDAL
{
    /// <summary>
    /// I Export To SQL DAL
    /// </summary>
    public interface IExportInfrastructureToSQLDAL
    {
        event EventHandler<ImportToSQLArgs> Changed;
        bool ExportInfrasructureData(List<CityImport> lstCities, List<JunctionImport> lstJunctions, List<StationImport> lstStations, ref string message);
    }
}
