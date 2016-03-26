namespace BLEntities.SQLServer 
{
    public class ShapeOfJunctionDetailsImport : SQLImportEntityBase
    {
        public int ImportControlId { get; set; }
        public int OperatorId { get; set; }
        public int OfficeLineId { get; set; }
        public int Direction { get; set; }
        public string LineAlternative { get; set; }
        public int Order { get; set; }
        public decimal  Lat { get; set; }
        public decimal Long { get; set; }
        
    }
}
