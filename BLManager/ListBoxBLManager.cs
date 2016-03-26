using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;
using Utilities;
using ExportConfiguration;

namespace BLManager
{
    public class ListBoxBLManager
    {

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="allData"></param>
        /// <param name="selectedEntities"></param>
        public ListBoxBLManager(List<BaseTableEntity> allData, List<BaseTableEntity> selectedEntities)
        {
            this.SelectedEntities = selectedEntities;
            this.AllData = allData.FindAll(en => en.ID > 0);
        }

        public List<BaseTableEntity> SelectedEntities { get; set; }
        public List<BaseTableEntity> AllData { get; set; }
        /// <summary>
        /// Add To Selected
        /// </summary>
        /// <param name="entity"></param>
        public void AddToSelected(BaseTableEntity entity)
        {
            if (SelectedEntities!=null)
            {
                if (!SelectedEntities.Exists(en => en.Equals(entity)))
                {
                    SelectedEntities.Add(entity);
                }
            }
        }
        /// <summary>
        /// Remove From Selected
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveFromSelected(BaseTableEntity entity)
        {
            if (SelectedEntities.IsListFull())
            {
                SelectedEntities.Remove(entity); 
            }
        }
        /// <summary>
        /// Build Selected Delimeted String
        /// </summary>
        /// <returns></returns>
        public string BuildSelectedDelimetedString()
        {
            StringBuilder sb = new StringBuilder(); 
            if (SelectedEntities != null)
            {
                SelectedEntities.ForEach(be => 
                    {
                        sb.Append(be.ID.ToString());
                        sb.Append(ExportConfigurator.GetConfig().LogicalDataSplitDevider);
                    });
            }
            return sb.ToString(); 
        }

    }
}

