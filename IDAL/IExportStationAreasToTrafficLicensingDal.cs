using System.Collections.Generic;
using System.Data.Common;
using BLEntities.Entities;

namespace IDAL
{
    public interface IExportStationAreasToTrafficLicensingDal
    {
        bool ExportPriceArea(List<PhysicalStop> physicalStop, ref string errorMessage);
        void Save(PhysicalStop psStop, DbConnection connection);
        int InsertImportControl(ImporControl imporControl, DbConnection connection);
        void UpdateImportControl(ImporControl imporControl, DbConnection connection);
        void DeleteImporControl(ImporControl imporControl, DbConnection connection);
    }
}
