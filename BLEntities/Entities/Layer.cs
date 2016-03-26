using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    /// <summary>
    /// Layer infrastructure 
    /// </summary>
    public class Layer
    {
      public int ID {get;set;}
      public String MapName { get; set; }
      public String LayerName { get; set; }
      public String LayerHebrew { get; set; }
      public String DBName { get; set; }   // path
      public String DBLayerName { get; set; }
      public String Label { get; set; }
      public bool ReadOnly { get; set; }
      public bool Shared   { get; set; }
      public override string ToString()
      {
          return LayerHebrew;
      }
      public bool IsSelectedByUser { get; set; } 

      public List<string> FieldList { get; set; }
      public override bool Equals(object obj)
      {
          if (obj == null || (!(obj is Layer))) return false; 
          Layer layer = (Layer)obj;
          return this.LayerName.Equals(layer.LayerName);
      }

      public override int GetHashCode()
      {
          return base.GetHashCode();
      }

      public LayerType LayerTypeValue { get; set; }

      public TransCadColor TransCadColorValue { get; set; }
      
    }
}
