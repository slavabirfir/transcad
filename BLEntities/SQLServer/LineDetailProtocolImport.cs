using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace BLEntities.SQLServer 
{
    public class LineDetailProtocolImport : SQLImportEntityBase
    {
        public int ImportControlId { get; set; }
        public int RowId { get; set; }
        public int ClusterId { get; set; }
        public int OperatorId { get; set; }
        public int OffilceLineId { get; set; }
        public int Direction { get; set; }
        public string  LineAlternative { get; set; }
        public int ProtocolOfficeId { get; set; }
        public int ParagraphOfficeId { get; set; }
        public int ProtocolId { get; set; }
         
    }
}
