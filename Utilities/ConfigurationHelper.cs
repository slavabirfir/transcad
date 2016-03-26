using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Utilities
{
    public static class ConfigurationHelper
    {
        public static string GetDbConnectionString(string connectionKeyString)
        {
          return  ConfigurationManager.ConnectionStrings[connectionKeyString].ConnectionString;
        }
    }
}
