using System.Globalization;

namespace BLEntities.Entities
{
    public class ClusterState
    {
        public int MainZoneId { get; set; }
        public int SubZoneId { get; set; }

        public string MainZoneName { get; set; }
        public string SubZoneName { get; set; }

        public string[] ToListViewItem()
        {
            return new[]
                       {
                           MainZoneId.ToString(CultureInfo.InvariantCulture),
                           MainZoneName,
                           SubZoneId.ToString(CultureInfo.InvariantCulture),
                           SubZoneName
                       };
        }
    }
}
