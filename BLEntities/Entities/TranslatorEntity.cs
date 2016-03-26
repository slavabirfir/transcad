using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    public class TranslatorEntity
    {
        public string EnglishName { get; set; }
        public string HebrewName { get; set; }
        public Dictionary<string,List<string>>  RealPossibleValues { get; set; }
    }
}
