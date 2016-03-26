using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using IBLManager;
using BLEntities.Model;
using BLEntities.Accessories;
using Logger;

namespace BLManager
{
    public class PresentationRouteMainForm : IPresentationRouteMainForm
    {
        readonly ITransCadBLManager _transCadManager;
        
        public PresentationRouteMainForm()
        {
            _transCadManager = new TransCadBlManager();
        }


        static uint GetTranscadOpenedByProcessOwner(string processName, string username)
        {
            string query = @"SELECT * FROM Win32_Process where Name LIKE '" + processName + "%'";

            var moSearcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection moCollection = moSearcher.Get();

            foreach (ManagementObject mo in moCollection)
            {
                var args = new[] { string.Empty };
                int returnVal = Convert.ToInt32(mo.InvokeMethod("GetOwner", args));
                if (returnVal == 0)
                {
                    if (args[0] != null && args[0].Contains(username) || username.Contains(args[0]))
                    {
                        return (uint)mo["PROCESSID"]; 
                    }
                }
            }

            return 0;
        }




        public void ExitFormApplication()
        {
            try
            {
                var procId = GetTranscadOpenedByProcessOwner("tcw", Environment.UserName);
                if (procId > 0)
                {
                    var proc = Process.GetProcessById(Convert.ToInt32(procId));
                    {
                        proc.CloseMainWindow();
                        //proc.WaitForExit();
                        LoggerManager.WriteToLog("ExitFormApplication");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.WriteToLog(ex);
            }
        }



        /// <summary>
        /// Is Visible ManagerPart 
        /// </summary>
        public bool IsVisibleManagerPart
        {
            get
            {
                return GlobalData.LoginUser != null && GlobalData.LoginUser.IsSuperViser;
            }
        }
        /// <summary>
        /// Get RouteLines Fields
        /// </summary>
        /// <returns></returns>
        public List<TransCadField> GetRouteLinesFields()
        {
            if (_transCadManager.IsEgedOperator)
            {
                return new List<TransCadField> { new TransCadField { EnglishName = "Makat8", HebrewName = Resources.BLManagerResource.Makat8 } };
            }
            else
            {
                string Catalog = "Catalog";
                string Signpost = "Signpost";
                //string RoadDescription = "RoadDescription";
                string RouteNumber = "RouteNumber";
                string Route_Name = "Route_Name";
                List<TransCadField> retValue = new List<TransCadField>();
                List<string> lst = _transCadManager.GetRouteLineLayerFields();

                if (lst.Contains(Catalog))
                    retValue.Add(new TransCadField { EnglishName = Catalog, HebrewName = Resources.BLManagerResource.Catalog });
                if (lst.Contains(Signpost))
                    retValue.Add(new TransCadField { EnglishName = Signpost, HebrewName = Resources.BLManagerResource.Signpost });
                //if (lst.Contains(RoadDescription))
                //    retValue.Add(new TransCadField { EnglishName = RoadDescription, HebrewName = Resources.BLManagerResource.RoadDescription });
                if (lst.Contains(RouteNumber))
                    retValue.Add(new TransCadField { EnglishName = RouteNumber, HebrewName = Resources.BLManagerResource.RouteNumber });
                if (lst.Contains(Route_Name))
                    retValue.Add(new TransCadField { EnglishName = Route_Name, HebrewName = Resources.BLManagerResource.Route_Name });
                return retValue;
            }
        }
        /// <summary>
        /// Get Route Stops Fields
        /// </summary>
        /// <returns></returns>
        public List<TransCadField> GetRouteStopsFields()
        { 
                return null ;
        }

       

         
    }
}
