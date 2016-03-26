using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Accessories
{
    public class JunctionVersionExport
    {
        public int OperatorId { get; set; }
        public int OfficeLineId { get; set; }
        public int Direction { get; set; }
        public string LineAlternative { get; set; }
        public int JunctionVersion { get; set; }
        public string PathToZipFile { get; set; }
        public int IDRoute { get; set; }
    }
}

