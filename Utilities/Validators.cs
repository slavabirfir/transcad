using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic; 

namespace Utilities
{
    public  static class Validators
    {

        /// <summary>
        /// Is Numeric
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNumeric(object data)
        {
            return Information.IsNumeric(data);
        }

        /// <summary>
        /// Is Date
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsDate(object data)
        {
            return Information.IsDate(data);
        }


    }
}
