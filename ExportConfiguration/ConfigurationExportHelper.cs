using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ExportConfiguration
{
    public static class ConfigurationExportHelper
    {
        public static string GetDBConnectionString(string connectionKeyString)
        {
          return  ConfigurationManager.ConnectionStrings[connectionKeyString].ConnectionString;
        }

        public static bool IsAppSettingExists(string appSettingsKey)
        {
            return ConfigurationManager.AppSettings[appSettingsKey] != null;
        }

        public static string GetAppSettings(string appSettingsKey)
        {
            if (ConfigurationManager.AppSettings[appSettingsKey] == null)
            {
                throw new ApplicationException(string.Format("appSettingsKey {} was not found ",appSettingsKey)); 
            }
            return ConfigurationManager.AppSettings[appSettingsKey]; ;
        }
    }
}
