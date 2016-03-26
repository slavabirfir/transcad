using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLManager
{
    public interface IFastSearchBLManagerPresenter
    {
        List<string> SearchData { get; set; }
        List<string> GetFilteredList(string filter);
        string UserSelectedData { get; set; }
        bool IsItemExists(string newItem); 
    }
}
