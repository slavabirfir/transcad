using System;
using System.Collections.Generic;

namespace BLEntities.Entities
{
    public class OperatorValidateLineExportDate
    {
        public int IdOperator { get; set; }
        public string OperatorName { get; set; }
        public List<ClusterValidateLineExportDate> ClusterValidateLineExportDates { get; set; }
        public override string ToString()
        {
            return OperatorName;
        }
    }
    public class ClusterValidateLineExportDate
    {
        public int IdCluster { get; set; }
        public string ClusterName { get; set; }
        public DateTime? ValidExportDate { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, {1}", ClusterName, ValidExportDate.HasValue ? ValidExportDate.Value.ToString("dd/MM/yyyy") : string.Empty);
        }
    }
}
