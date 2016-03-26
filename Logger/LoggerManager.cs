using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSLogger = Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using System.Security.Principal;

namespace Logger
{
    public class LoggerManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public static void WriteToLog(Exception e)
        {
            WriteToLog(e.Message + Environment.NewLine + e.Source + " => " + e.StackTrace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToLog(string message)
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("Message", message);
            ManagedSecurityContextInformationProvider informationHelper = new ManagedSecurityContextInformationProvider();
            informationHelper.PopulateDictionary(dictionary);
            MSLogger.Logger.Write("Log entry with extra information", dictionary);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private string ErrorString(Dictionary<string, object> dictionary)
        {
            string res = string.Empty;
            dictionary.Values.ToList().ForEach(s => res += s + Environment.NewLine);
            return res;
        }

    }
}
