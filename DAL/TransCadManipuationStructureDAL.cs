using System;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using Logger;
using System.Xml.Linq;
using Utilities;
using System.IO;
using BLEntities.Entities;
using BLEntities.Accessories;
using ExportConfiguration;

namespace DAL
{
    public partial class TransCadMunipulationDataDAL
    {
        private readonly IInternalBaseDal _dalSqlServer = new InternalTvunaImplementationDal();

        //public List<ISSTR> GetISSTR(string mapLayerName)
        //{
        //    List<ISSTR> lstResults = new List<ISSTR>();
        //    DataTable resDataTable = new DataTable();
        //    try
        //    {
        //        int maxFetchCount = 20000;
        //        object nodes_count = Gisdk.DoFunction("GetRecordCount", mapLayerName, null);
        //        string[] filedNames = new string[] { "ID", "Seconds" };
        //        int initPart = Convert.ToInt32(nodes_count) / maxFetchCount;
        //        string sentance = "SELECT ID,Seconds  WHERE ID>={0} AND ID<{1}";
        //        for (int i = 0; i <= initPart; i++)
        //        {
        //            string sqlSentance = string.Format(sentance, i * maxFetchCount, (i + 1) * maxFetchCount);
        //            DataTable dt = FillData(filedNames, mapLayerName, sqlSentance);
        //            if (dt.IsDataTableFull())
        //            {
        //                resDataTable.Merge(dt, true);
        //            }
        //        }
        //        if (resDataTable.IsDataTableFull())
        //        {
        //            foreach (DataRow row in resDataTable.Rows)
        //            {
        //                lstResults.Add(new ISSTR
        //                {
        //                    LinkID = Convert.ToInt32(row["ID"]),
        //                    Seconds =row["Seconds"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Seconds"])
        //                });
        //            }
        //        }
        //        return lstResults;
        //    }
        //    catch (Exception exc)
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //        LoggerManager.WriteToLog(exc);
        //        return null;

        //    }
        //    finally
        //    {
        //        if (gisdk != null && gisdk.IsConnected())
        //            gisdk.Close();
        //    }
        //}
        /// <summary>
        /// Get IS_STR table
        /// </summary>
        /// <param name="mapLayerName"></param>
        /// <returns></returns>
        /// <summary>
        /// Get Link Attributes By Id
        /// </summary>
        /// <param name="mapLayer"></param>
        /// <param name="endPointLayer"></param>
        /// <param name="endPointLayer"></param>
        /// <param name="linkId"></param>
        /// <returns></returns>
        public RouteLink GetLinkAttributesById(string mapLayer,string endPointLayer, int linkId)
        {
            RouteLink routeLink =null;
            try
            {
                object data = Gisdk.DoFunction("GetRecordValues", mapLayer, linkId.ToString(), new object[] { "Length","Seconds" });
                object infoLength = Gisdk.DoFunction("ExtractArray", data, 1); // Length
                object[] lengthElementData = CaliperForm.Connection.ObjectToArray(infoLength, "string");
                int length = Convert.ToInt32(Convert.ToDouble(lengthElementData[1]) * ExportConfigurator.GetConfig().LengthConverter);
                object infoSeconds = Gisdk.DoFunction("ExtractArray", data, 2); // Seconds
                object[] secondsElementData = CaliperForm.Connection.ObjectToArray(infoSeconds, "string");
                float duration = (float)Convert.ToDecimal(secondsElementData[1]);
                Gisdk.DoFunction("SetLayer", mapLayer);
                object node_ids = Gisdk.DoFunction("GetEndpoints", linkId);
                object[] infoData = CaliperForm.Connection.ObjectToArray(node_ids, "string");
                return new RouteLink
                {
                    LinkId = linkId,
                    Length = length,
                    Duration = duration,
                    NodeIdFirst = Convert.ToInt32(infoData[0]),
                    NodeIdLast = Convert.ToInt32(infoData[1])
                };
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return routeLink;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }




            //string[] filedNames = null;
            //string sentance = null;
            //filedNames = new string[] { "ID", "Seconds" };
            //sentance = string.Format("SELECT ID,Seconds WHERE ID={0}", linkID);
            //DataTable dt = FillData(filedNames, mapLayer, sentance);
            //if (dt.IsDataTableFull() && dt.Rows.Count == 1)
            //{
            //    return dt.Rows[0]["Seconds"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["Seconds"]);
            //}
            //return 0;
        }

        /// <summary>
        /// Verify Route System
        /// </summary>
        /// <param name="rs_path"></param>
        /// <param name="verification_level"></param>
        /// <returns></returns>
        public string VerifyRouteSystem(string rs_path, string verification_level)
        {
            try
            {
                object data = Gisdk.DoFunction("VerifyRouteSystem", rs_path, verification_level);
                if (data != null && !String.IsNullOrEmpty(data.ToString()))
                {
                    return data.ToString();
                }
                else
                {
                    return String.Empty;  
                }
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return exc.Message;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        public List<string> GetMapLayers(string mapName,string layerType)
        {
            var lst = new List<string>();
            try
            {
                object layers = Gisdk.DoFunction("GetMapLayers", mapName, layerType);
                object info = Gisdk.DoFunction("ExtractArray", layers, 1);
                object[] arrayData = CaliperForm.Connection.ObjectToArray(info, "string");
                foreach (var item in arrayData)
                {
                    lst.Add(item.ToString());
                }
                return lst;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Get Layers Name
        /// </summary>
        /// <returns></returns>
        public List<string> GetLayersName()
        {
            var lst = new List<string>();
            try
            {
                object layers = Gisdk.DoFunction("GetLayers");
                object info = Gisdk.DoFunction("ExtractArray", layers, 1);
                object[] arrayData = CaliperForm.Connection.ObjectToArray(info, "string");
                foreach (var item in arrayData)
                {
                    lst.Add(item.ToString());
                }
                return lst;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }

        }

        public List<string> GetMapNames()
        {
            var lst = new List<string>();
            try
            {
                object layers = Gisdk.DoFunction("GetMapNames");
                object info = Gisdk.DoFunction("ExtractArray", layers, 1);
                object[] arrayData = CaliperForm.Connection.ObjectToArray(info, "string");
                lst.AddRange(arrayData.Select(item => item.ToString()));
                return lst;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return null;
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        public void CloseMap(string mapName)
        {
            try
            {
                Gisdk.DoFunction("CloseMap", mapName);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);

            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }


        /// <summary>
        /// Fill Layer List
        /// </summary>
        /// <returns></returns>
        public List<Layer> FillLayerList()
        {

            const string xmlFile = "Layer.xml";

            string fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, xmlFile);
            if (!IoHelper.IsFileExists(fileName))
            {
                throw new ApplicationException(string.Format("{0} file is not exists in application Data folder {1}", fileName, ExportConfigurator.GetConfig().DataFolder));
            }
            XDocument doc = XDocument.Load(fileName);
            if (doc != null)
            {
                return (from p in doc.Descendants("layer")
                        select new Layer
                        {
                            MapName = p.Element("mapname").Value,
                            LayerName = p.Element("layername").Value,
                            LayerHebrew = p.Element("layerhebrew").Value,
                            DBName = p.Element("dbname").Value,
                            DBLayerName = p.Element("dblayername").Value,
                            Label = p.Element("label").Value,
                            ReadOnly = Boolean.Parse(p.Element("readonly").Value),
                            Shared = Boolean.Parse(p.Element("shared").Value),
                            LayerTypeValue = (LayerType)Enum.Parse(typeof(LayerType), p.Element("layertype").Value)
                        }).ToList<Layer>();
            }
            return null;
        }

        /// <summary>
        /// Drop Layer
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="layerName"></param>
        public void DropLayer(string mapName, string layerName)
        {
            try
            {
                Gisdk.DoFunction("DropLayer",mapName,layerName);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
 
        }
        /// <summary>
        /// Add Layer
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="layerName"></param>
        /// <param name="dbName"></param>
        /// <param name="dbLayerName"></param>
        /// <param name="options"></param>
        public bool AddLayer(string mapName, string layerName, string dbName, string dbLayerName, object options)
        {
            try
            {
                Gisdk.DoFunction("AddLayer", mapName, layerName, dbName, dbLayerName, options);
                return true;
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
                return false; 
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Redraw Map
        /// </summary>
        /// <param name="mapname"></param>
        public void RedrawMap(string mapname)
        {
            try
            {
                Gisdk.DoFunction("RedrawMap", mapname); 
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Set Line Color
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public void SetLineColor(string layerName, int red, int green, int blue)
        {
            try
            {
                //65535, 65535, 0
                var colorRgb = Gisdk.DoFunction("ColorRGB", red, green, blue);
                Gisdk.DoFunction("SetLineColor", string.Concat(layerName,"|"), colorRgb);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        public void SetMap(string mapName)
        {
            try
            {
                Gisdk.DoFunction("SetMap", mapName);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }




        /// <summary>
        /// Set Icon Color
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public void SetIconColor(string layerName, int red, int green, int blue)
        {
            try
            {
                var colorRgb = Gisdk.DoFunction("ColorRGB", red, green, blue);
                Gisdk.DoFunction("SetIconColor", string.Concat(layerName, "|"), colorRgb);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Set Border Color
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public void SetBorderColor(string layerName, int red, int green, int blue)
        {
            try
            {
                var colorRgb = Gisdk.DoFunction("ColorRGB", red, green, blue);
                Gisdk.DoFunction("SetBorderColor", string.Concat(layerName, "|"), colorRgb);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Set Line Style
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="linewidth"></param>
        public void SetLineWidth(string layerName, decimal linewidth)
        {
            try
            {
                Gisdk.DoFunction("SetLineWidth", string.Concat(layerName, "|"), linewidth);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Set Icon Size
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="pointsize"></param>
        public void SetIconSize(string layerName, decimal pointsize)
        {
            try
            {

                Gisdk.DoFunction("SetIconSize", string.Concat(layerName, "|"), pointsize);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }
        /// <summary>
        /// Set Border Width
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="borderwidth"></param>
        public void SetBorderWidth(string layerName, decimal borderwidth)
        {
            try
            {

                Gisdk.DoFunction("SetBorderWidth", string.Concat(layerName, "|"), borderwidth);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Set Icon
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="iconType"></param>
        /// <param name="name"></param>
        /// <param name="sizeorindex"></param>
        public void SetIcon(string layerName, string iconType, string name, decimal sizeorindex)
        {
            try
            {

                Gisdk.DoFunction("SetIcon", string.Concat(layerName, "|"), iconType, name, sizeorindex);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Set Line Style
        /// </summary>
        /// <param name="layerName"></param>
        public void SetLineStyle(string layerName)
        {

            try
            {
                //string data = "{{{1, -1, 0}}}";
                object[] array1 =new object[1]; 
                object[] array2 =new object[1];
                object[] array3 = new object[1];
                array3[0]= new object[3] {1,-1,0};
                array2[0] = array3;
                array1[0] = array2;
                var lineStyle = Gisdk.DoFunction("LineStyle", array1);
                Gisdk.DoFunction("SetLineStyle", string.Concat(layerName, "|"), lineStyle);
            }
            catch (Exception exc)
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
                LoggerManager.WriteToLog(exc);
            }
            finally
            {
                if (_gisdk != null && _gisdk.IsConnected())
                    _gisdk.Close();
            }
        }

        /// <summary>
        /// Get Route Line Area
        /// </summary>
        /// <param name="areaLayer"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public string GetStopCity(int longitude, int latitude, string areaLayer)
        {
            Gisdk.DoFunction("SetLayer", areaLayer);
            var coord = Gisdk.DoFunction("Coord", longitude, latitude);
            object nearestRecord = Gisdk.DoFunction("LocateNearestRecord", coord, 0.005);
            if (nearestRecord != null)
            {
                var areaArray = new object[] { "MUNICIPALN" };
                object oAreaNumber = Gisdk.DoFunction("GetRecordValues", areaLayer, nearestRecord, areaArray);
                object arrayElements = Gisdk.DoFunction("ExtractArray", oAreaNumber, 1);
                object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(arrayElements, "string");
                return fieldsArrayData[1] == null ? string.Empty : Convert.ToString(fieldsArrayData[1]);
            }
            return string.Empty;
        }


        public int GetNearestIdPriceAreaRecordToPhysicalStop(PhysicalStop stop, string areaLayer, bool realCoordinate,string returnField)
        {
            Gisdk.DoFunction("SetLayer", areaLayer);
            var coord = Gisdk.DoFunction("Coord", realCoordinate ? stop.Longitude : stop.LongitudeN,
                                                  realCoordinate ? stop.Latitude : stop.LatitudeN);
            object nearestRecord = Gisdk.DoFunction("LocateNearestRecord", coord, 0.005);
            if (nearestRecord != null)
            {
                //var areaArray = new object[] { "ZoneCode" };
                var areaArray = new object[] { returnField };
                object oAreaNumber = Gisdk.DoFunction("GetRecordValues", areaLayer, nearestRecord, areaArray);
                object arrayElements = Gisdk.DoFunction("ExtractArray", oAreaNumber, 1);
                object[] fieldsArrayData = CaliperForm.Connection.ObjectToArray(arrayElements, "string");
                return fieldsArrayData[1] == null ? 0 : Convert.ToInt32(fieldsArrayData[1]);
            }
            return 0;
        }

        public string GetNearestIdMunAreasToPhysicalStop(string uiFile, Coord coord, string areaLayer, int threshhold)
        {
            var res = RunTranscadMacro("CloseAreasToStation", uiFile,
                new List<object>() { coord.Longitude, coord.Latitude, ExportConfigurator.GetConfig().MunLayer, threshhold });
            return res == null ? string.Empty : res.ToString();
        }

    }
}
