using System.Collections.Generic;

namespace BLEntities.Entities
{
    public class BaseTableEntity 
    {
        public Dictionary<string, object> AdditonalInfo { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is BaseTableEntity)
            {
                var bte = (BaseTableEntity)obj;
                return bte.ID.Equals(ID); 
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
