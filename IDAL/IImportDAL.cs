using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using System.Data.Common;

namespace IDAL
{

    public interface IImportIDAL<T> where T : class
    {
        bool Save(T entity, DbConnection connection);
    }
}
