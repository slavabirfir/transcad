using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using BLEntities.Accessories;

namespace IBLManager
{
    public interface  IExportRouteBLManager
    {
        bool ExportData(ref string message);
        bool IsImportManagerFilled(ref string message);
        event EventHandler<ImportToSQLArgs> Changed;
        bool IsCanceledByUser { get; set; }
        //List<BaseTableEntity> exportClustersOfOperator { get; }
    }
}
