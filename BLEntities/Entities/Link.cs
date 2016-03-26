using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    public class Link
    {
        public int LinkID { get; set; }
        public int NA { get; set; }
        public int NB { get; set; }
        public int Length { get; set; }
        public string Name { get; set; }
    }
    
}
