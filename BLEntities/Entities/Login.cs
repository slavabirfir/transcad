using System;

namespace BLEntities.Entities
{
    public class TranscadLogin
    {
        public string   UserName { get; set; }
        public string   OperatorName { get; set; }
        public DateTime LoginDate { get; set; }
        public string   ClusterName { get; set; }
        public string   WorkspaceFile { get; set; }
    }
}
