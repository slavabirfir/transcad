using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Globalization;
using BLEntities.Model;
using Logger;
using System.Threading;
using Utilities;
using GUIBase;
using BLManager;
using GUIBase.Resources;

namespace RouteExportProcess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += AppThreadException;
            Application.EnableVisualStyles();
            //if (ExportConfiguration.ExportConfigurator.GetConfig().MonitorTimerInterval > 0)
            //   PerformenseMonitorBL.RunTimer(); 
            Application.ApplicationExit += Application_ApplicationExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            var routeExportProccess = new frmRouteExportProccess();
            Application.Run(routeExportProccess);
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            var manager = new BlLoginsManager();
            manager.DeleteLogin();
        }


        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ExceptionCatch((Exception)e.ExceptionObject);
            }
            catch (Exception exc)
            {
                ExceptionCatch(exc);
            }
        }


        /// <summary>
        /// Displays dialog with information about exceptions that occur in the application. 
        /// </summary>
        private static void AppThreadException(object source, ThreadExceptionEventArgs e)
        {
            ExceptionCatch(e.Exception);
        }

        private static void ExceptionCatch(Exception exception)
        {
            LoggerManager.WriteToLog(exception);
            var errorMessage = new StringBuilder();
            errorMessage.AppendFormat(new CultureInfo("en-us", true), "{0}", exception.Message);
            MessageBox.Show(errorMessage.ToString(), ResourceGUIBase.Caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            try
            {
                Application_ApplicationExit(null, null);
            }
            catch (Exception)
            {
                LoggerManager.WriteToLog(exception);
            }
            Application.Exit();
        }

    }
}
