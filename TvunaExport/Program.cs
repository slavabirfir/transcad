using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TvunaExport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args != null && args.Length==2)
            {
                string data = args[1];
                if (!string.IsNullOrEmpty(data))
                {
                    int idOperator = int.Parse(data);
                    BL.Global.Initialize(idOperator);
                }
                else
                {
                    throw new ApplicationException("Illegal caller");
                }
            }
            else
            {
                throw new ApplicationException("Illegal caller");
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
