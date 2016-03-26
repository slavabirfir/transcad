using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using Utilities;
using BLEntities.Entities; 
namespace BLManager
{
    public class FastSearchBLManagerPresenterCatalog 
    {
        public string UserSelectedData { get; set; }
        public List<CatalogInfo> InitialData { get; set; }
        public FastSearchBLManagerPresenterCatalog(List<CatalogInfo> initialData)
        {
            UserSelectedData = null; 
            if (initialData==null) 
                initialData = new List<CatalogInfo>(); 
            
            List<string> strData = new List<string>();
            initialData.ForEach(c => strData.Add(c.Catalog.ToString()));
            SearchData = initialData;
           
        }

        /// <summary>
        /// Search Data
        /// </summary>
        public List<CatalogInfo> SearchData { get; set; }
        /// <summary>
        /// Get Filtered List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<CatalogInfo> GetFilteredList(string filter)
        {
            if (SearchData.IsListFull())
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return SearchData;
                }
                else
                   return (from p in SearchData
                        where p.Catalog.ToString().StartsWith(filter)
                        select p).ToList<CatalogInfo>();
            }
            else
                return null;
        }

        
        /// <summary>
        /// isItemExists
        /// </summary>
        /// <returns></returns>
        public bool IsItemExists(string newItem)
        {
            List<string>   splitData = UserSelectedData.Split("|".ToCharArray()).ToList<string>() ;
            if (splitData != null)
            {
                return splitData.Exists(it => it == newItem); 
            }
            return false; 
        }
    }
}
