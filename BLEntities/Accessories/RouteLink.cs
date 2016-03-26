using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Accessories
{
    public class RouteLink
    {
        public int LinkId { get; set; }
        public int TraverseDirection  { get; set; }
        public int NodeIdFirst { get; set; }
        public int NodeIdLast { get; set; }
        public int Length { get; set; }
        public float Duration { get; set; }
    }
}
