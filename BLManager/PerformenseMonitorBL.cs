using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ExportConfiguration;
using Utilities;
using System.IO;
using Logger;


namespace BLManager
{
    public class PerformenseMonitorBl
    {
        private const string FileConstName = "OperatorPerformenceCounters_";
        private const string DirConstName = "OperatorTranscad";
        private const string Txt = ".txt";
        private const string Delimiter = " ; ";
        private static string _fileName = null;
        private static PerformenceCounters _perfCounters = null;
        public static void RunTimer()
        {
            if (ExportConfigurator.GetConfig().MonitorTimerInterval > 0)
            {
                _perfCounters = new PerformenceCounters();
                string fileInnerName = String.Concat(FileConstName, Environment.UserName,DateTime.Now.ToString("_dd-MM-yyyy hh-mm-ss"), Txt);
                //fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, fileInnerName);
                string dirName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                , DirConstName);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
                _fileName =Path.Combine( Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    dirName), fileInnerName);


                using (var file = new System.IO.StreamWriter(_fileName,false, Encoding.UTF8))
                {
                    file.WriteLine(string.Concat("AvailableRAM",Delimiter,"CurrentCpuUsage"));
                }

                var aTimer = new Timer();
                aTimer.Elapsed += OnTimedEvent;
                // Set the Interval to x seconds.
                aTimer.Interval =(double) ExportConfigurator.GetConfig().MonitorTimerInterval;
                aTimer.Enabled = true;
            }
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                using (var file = new StreamWriter(_fileName, true, Encoding.UTF8))
                {
                    file.WriteLine(string.Concat(_perfCounters.getAvailableRAM(), Delimiter, _perfCounters.getCurrentCpuUsage()));
                }
            }
            catch(Exception exp)
            {
                LoggerManager.WriteToLog(exp);
            }
        }

    }
}
