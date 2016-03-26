using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace BLEntities.Accessories
{
    public class JunctionVersionSQLServerExport
    {
        public int JunctionVersion { get; set; }
        public byte[] Shape { get; set; }
        public List<Coord> ShapeGeomteryCoords { get; set; }
        public DateTime CreateDate  { get; set; }
        public string PathToZipFile { get; set; }
    }
}
