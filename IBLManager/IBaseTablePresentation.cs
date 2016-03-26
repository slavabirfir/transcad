using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace IBLManager
{
    public interface  IBaseTablePresentation
    {
        List<TranslatorEntity> GetBaseTableNames { get; }
        List<BaseTableEntity> GetBaseTableEntities(string baseTableName);
        
    }
}
