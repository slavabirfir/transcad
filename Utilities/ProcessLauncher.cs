using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace Utilities
{
    /// <summary>
    /// Process Launcher
    /// </summary>
    public static class ProcessLauncher
    {
        /// <summary>
        /// Run Process
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="args"></param>
        public static void RunProcess(string processName, string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(processName);
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.Arguments = args;
            Process.Start(startInfo);
        }

        /// <summary>
        /// OpenExplorerWithURL
        /// </summary>
        /// <param name="url"></param>
        public static void OpenExplorerWithURL(string url)
        {
            // open URL in separate instance of default browser
            Process p = new Process();
            p.StartInfo.FileName = GetDefaultBrowserPath();
            p.StartInfo.Arguments = url;// "http://windowsclient.net/learn/video.aspx?v=324332";
            p.Start();
        }
        /// <summary>
        /// Get Default Browser Path
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultBrowserPath()
        {
            string key = @"htmlfile\shell\open\command";
            RegistryKey registryKey =
            Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];
        }
    }
}
