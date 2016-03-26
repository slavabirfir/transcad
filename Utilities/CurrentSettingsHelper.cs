using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Utilities
{
    public static class CurrentSettingsHelper
    {
        /// <summary>
        /// Get Login User Name
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserName()
        {
            char[] delimiters = new char[] { '\\'};
            string userName = WindowsIdentity.GetCurrent().Name;
            string[] userNameStructure = userName.Split(delimiters);
            if (userNameStructure.Length > 1)
            {
                return userNameStructure[userNameStructure.Length - 1];
            }
            return userName; 
        }
    }
}
