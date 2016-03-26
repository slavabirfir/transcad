using System;
using BLEntities.Accessories;

namespace IBLManager
{
    public interface IExportBaseBlManager
    {
        bool ExportBaseData(ref string fileName);
        event EventHandler<ImportToSQLArgs> Changed;
    }
}
