using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using Utilities; 
namespace BLManager
{
    public class FastSearchBLManagerPresenter : IFastSearchBLManagerPresenter
    {
        public FastSearchBLManagerPresenter()
        {
            UserSelectedData = string.Empty;  
        }

        /// <summary>
        /// Search Data
        /// </summary>
        public List<string> SearchData { get; set; }
        /// <summary>
        /// Get Filtered List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<string> GetFilteredList(string filter)
        {
            if (SearchData.IsListFull())
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return SearchData;
                }
                else
                   return (from p in SearchData
                        where p.StartsWith(filter)
                        select p).ToList<string>();
            }
            else
                return null;
        }

        /// <summary>
        /// UserSelectedData
        /// </summary>
        public string UserSelectedData { get; set; }
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
