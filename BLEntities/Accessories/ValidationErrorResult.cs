using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Accessories 
{
    public class ValidationErrorResult   
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public override string ToString()
        {
            return string.Format(Resources.ValidationResource.ValidationResultString, PropertyName,ErrorMessage);
        }
    }
}
