using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using BLEntities.Accessories;

namespace IDAL
{
    public interface ICSVFielOperation
    {
        List<JunctionVersionImport> GetJunctionVersionList(string fileName, int operartorId);
        void SetJunctionVersionList(string fileName, List<JunctionVersionExport> lst);
    }
}
