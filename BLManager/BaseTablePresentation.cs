using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Entities;
using BLEntities.Model;

namespace BLManager
{
    public class BaseTablePresentation : IBaseTablePresentation
    {
        #region IBaseTablePresentation Members
        /// <summary>
        /// Get Base Table Names
        /// </summary>
        /// <returns></returns>
        public List<TranslatorEntity> GetBaseTableNames
        {
            get { return GlobalData.BaseTableTranslatorList; } 
        }
        /// <summary>
        /// Get Base Table Entities
        /// </summary>
        /// <param name="baseTableName"></param>
        /// <returns></returns>
        public List<BaseTableEntity> GetBaseTableEntities(string baseTableName)
        {
            if (GlobalData.BaseTableEntityDictionary != null && GlobalData.BaseTableEntityDictionary.Keys.Contains(baseTableName))
            {
                if (baseTableName.ToUpper().Equals("TCGETLINETYPE"))
                {
                    return (from p in GlobalData.BaseTableEntityDictionary[baseTableName]
                            where p.ID != 0
                            select p).ToList<BaseTableEntity>();
                }
                else
                  return GlobalData.BaseTableEntityDictionary[baseTableName];
            }
            else
                return null; 
        }

        #endregion
    }
}
