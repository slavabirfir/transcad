using System;

namespace BLEntities.Entities
{
    public class AccountingGroup
    {
        public int AccountingGroupID { get; set; }
        public int ClusterId { get; set; }
        public int OperatorId { get; set; }
        public String AccountingGroupDesc { get; set; }
        public override string ToString()
        {
            return AccountingGroupDesc;
        }
        

    }
}
