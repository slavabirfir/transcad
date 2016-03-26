using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace IDAL
{
    public interface IExportBaseDAL
    {
        #region Base
        void Exporttv_yeshuv(List<City> lst,string fileName, ref string message);
        void Exportzg_ktaim(List<Link> lst,string fileName, ref string message);
        void Exportzg_mokdim(List<Node> lst,string fileName, ref string message);
        void Exportzg_tachana(List<Stop> lst,string fileName, ref string message);
        #endregion
    }
}
