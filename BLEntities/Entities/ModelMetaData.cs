using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    public class ModelMetaData
    {
        public Dictionary<String, Dictionary<String,String>>   TranscadMetaData { get; set; }
        public Dictionary<String, Dictionary<String, String>>  XMLMetaData { get; set; }
    }
}
