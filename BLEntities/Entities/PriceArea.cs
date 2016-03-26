using System;
using System.Collections.Generic;

namespace BLEntities.Entities
{
    public class PriceArea : BaseClass
    {
         
        public Byte[] ShapeFile { get; set; }
        public string ShapeFilePath { get; set; }
        public List<Coord> Coords { get; set; }
        public int TranscadId { get; set; }
        public double  Area { get; set; }
    }
}
