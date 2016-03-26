using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Utilities
{   
    public class RegisterHelper
    {
        public static string GetRegisterValue(string keyName,string valueName,string defValue)
        {
            return (string)Registry.GetValue(keyName, valueName, defValue);
        }

    }
}
