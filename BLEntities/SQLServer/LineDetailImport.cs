using System.Collections.Generic;
using BLEntities.Entities;

namespace BLEntities.SQLServer 
{
    public class LineDetailImport : SQLImportEntityBase
    {
        public int ImportControlId { get; set; }
        public int OperatorId { get; set; }
        public int ClusterId { get; set; }
        public string LineSign { get; set; }
        public int OfficeLineId { get; set; }
        public int Line { get; set; }
        public int Direction { get; set; }
        public string LineAlternative { get; set; }
        public int ServiceType { get; set; }
        public int LineDetailType { get; set; }
        public int DistrictId { get; set; }
        public int DistrictSecId { get; set; }
        public bool IsBase { get; set; }
        public int AccountingGroupId { get; set; }
        public bool Accessibility { get; set; }

        public int IdExclusivityLine{ get; set; }
        public bool ConfirmedForSaturday { get; set; }
        public byte[] Shape { get; set; }
        public List<Coord> ShapeGeomteryCoords { get; set; }
         
     }
}
