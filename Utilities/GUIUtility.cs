using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace Utilities
{
    public static class GUIUtility
    {
       
        /// <summary>
        /// Is Application Already Running 
        /// </summary>
        /// <returns></returns>
        public static  bool IsApplicationAlreadyRunning()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            if (proc.Contains("."))
            {
                proc = proc.Split(".".ToCharArray())[0]; 
            }
            Process[] processesAll = Process.GetProcesses();
            int sameApplicationCount = processesAll.Count(p=>p.ProcessName.StartsWith(proc));
            return sameApplicationCount > 1; 
        }

    }
}
