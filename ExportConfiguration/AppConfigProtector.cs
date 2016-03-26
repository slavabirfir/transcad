using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ExportConfiguration
{
    public class AppConfigProtector
    {
        /// <summary>
        /// Protect Connection String
        /// </summary>
        public static void EncryptConfig()
        {
            ToggleConnectionStringProtection(System.Windows.Forms.Application.ExecutablePath, true);
        }
        /// <summary>
        /// Un protect Connection String
        /// </summary>
        public static void DecryptConfig()
        {
            ToggleConnectionStringProtection(System.Windows.Forms.Application.ExecutablePath, false);
        }

        private static void PerformProtection(Configuration oConfiguration, bool protect,  string  sectionName , bool isConnectionString)
        {
            // Define the Dpapi provider name.
            string strProvider = "DataProtectionConfigurationProvider";
            // string strProvider = "RSAProtectedConfigurationProvider";
            //ConfigurationSection oSectionTransportMinistery = null;
            bool blnChanged = false;
            ConfigurationSection oSectionConfig = null;
            if (isConnectionString)
            {
                oSectionConfig = oConfiguration.GetSection(sectionName) as ConfigurationSection;//  as System.Configuration.ConnectionStringsSection;
            }
            else
            {
                ConfigurationSectionGroupCollection secGroups = oConfiguration.SectionGroups;//.GetSection("TransportMinistery");
                ConfigurationSectionGroup configurationSectionGroup = secGroups["TransportMinistery"];
                oSectionConfig = configurationSectionGroup.Sections[sectionName] as ConfigurationSection;
            }
  
            if (oSectionConfig != null)
            {
                if ((!(oSectionConfig.ElementInformation.IsLocked)) && (!(oSectionConfig.SectionInformation.IsLocked)))
                {
                    if (protect)
                    {
                        if (!(oSectionConfig.SectionInformation.IsProtected))
                        {
                            blnChanged = true;
                            oSectionConfig.SectionInformation.ProtectSection(strProvider);
                        }
                    }
                    else
                    {
                        if (oSectionConfig.SectionInformation.IsProtected)
                        {
                            blnChanged = true;
                            oSectionConfig.SectionInformation.UnprotectSection();
                        }
                    }
                }
                if (blnChanged)
                {
                    oSectionConfig.SectionInformation.ForceSave = true;
                    // Save the current configuration.
                    oConfiguration.Save();
                }
            }
        }

        /// <summary>
        /// Toggle Connection String Protection
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="protect"></param>
        private static void ToggleConnectionStringProtection
                (string pathName, bool protect)
        {
            Configuration oConfiguration = null;
            try
            {
                oConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(pathName);
                if (oConfiguration != null)
                {
                    PerformProtection(oConfiguration, protect, "ExportConfigurator",false );
                    PerformProtection(oConfiguration, protect, "connectionStrings",true);
                    
                }
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
            finally
            {
            }
        }
    }
}
