using System;
using System.Collections.Generic;
using BLEntities.Entities;

namespace BLEntities.SQLServer
{
    public class ZoneImport
    {
        public int OperatorId { get; set; }
        public int ZoneCode { get; set; }
        public string ZoneDescription { get; set; }
        public int   StationId  { get; set; }
        public List<Coord> ShapeGeomteryCoords  { get; set; }
        public Byte[] ShapeFile { get; set; }
        public DateTime FromDate { get; set; }
        public int ImportZoneControlOperatorId { get; set; }
        public double Area { get; set; }
    }
}
