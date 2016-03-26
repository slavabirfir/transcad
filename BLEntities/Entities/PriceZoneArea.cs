using System;
using System.Collections.Generic;

namespace BLEntities.Entities
{
    public class PriceZoneArea : BaseClass
    {
        public string TatFull { get; set; }
        public string ZName { get; set; }
        public string PName { get; set; }
        public int StationId { get; set; }
        public List<Coord> ShapeGeomteryCoords { get; set; }
        public Byte[] ShapeFile { get; set; }
        public String ShapeFilePath { get; set; }
        public DateTime FromDate { get; set; }
        public int ImportControlId { get; set; }
        public double? Area { get; set; }
        public int? Ring { get; set; }
    }
}
