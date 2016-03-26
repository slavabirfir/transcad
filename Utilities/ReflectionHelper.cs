using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Utilities
{
    public static class ReflectionHelper
    {
        public static string ToString(object t)
        {
            PropertyInfo[] props = t.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Concat("Class ", t.GetType().Name, Environment.NewLine));
            foreach (PropertyInfo item in props)
            {
                sb.Append(string.Format("{0} = {1}", item.Name, item.GetValue(t,null )));
                sb.Append(Environment.NewLine); 
            }
            return sb.ToString();    
        }
    }
}
