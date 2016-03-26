using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Accessories
{
    public class TransCadCurrentEnvoromnetInfo
    {
        public string RouteSystemFile { get; set; }
        public string MapLayerName { get; set; }
        public bool IsMunLayerExists { get; set; }
        public string MapLayerFile { get; set; }
        public bool IsNetworkDefined { get; set; }
        public List<string> LayersName { get; set; }

    }
}
