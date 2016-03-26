using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using DAL;
using Utilities;
using ExportConfiguration;
using BLEntities.Entities;
using BLEntities.Model;

namespace BLManager
{
    public class LayerManagerBL
    {
        private static int iconFormIndex = 4;
        private static int linearWidth = 1;
        private static string is_mun = "is_mun";
        

        /// <summary>
        /// dal
        /// </summary>
        private ITransCadMunipulationDataDAL dal = new TransCadMunipulationDataDAL();
        /// <summary>
        /// Constractor
        /// </summary>
        public LayerManagerBL()
        {
            SetUserLayers();
        }

        /// <summary>
        /// Get Internal Layer
        /// </summary>
        /// <returns></returns>
        private List<string> GetInternalLayer()
        {
            return new List<string> { 
                ExportConfigurator.GetConfig().RouteStopsLayerName, 
                ExportConfigurator.GetConfig().RouteSystemLayerName,
                ExportConfigurator.GetConfig().PhisicalStopsLayerName,
                ExportConfigurator.GetConfig().MapLayerName,
                ExportConfigurator.GetConfig().Endpoints
            };
        }
        /// <summary>
        /// Set User Layers
        /// </summary>
        private void SetUserLayers()
        {
            List<string> internalLayers = GetInternalLayer();
            List<string> allGISLayers = dal.GetLayersName();
            foreach (var item in internalLayers)
            {
                if (allGISLayers.Exists(l => l == item))
                {
                    allGISLayers.Remove(item); 
                }
            }
            List<Layer> userXMLLayers = dal.FillLayerList();
            userXMLLayers.ForEach(layer =>
                {
                    layer.IsSelectedByUser = (allGISLayers.Exists(l => l == layer.LayerName));
                    if (layer.IsSelectedByUser)
                    {
                        layer.FieldList = dal.GetLayerFields(layer.LayerName);
                    }
                }
            );
            GlobalData.LayerList = userXMLLayers;
        }
        /// <summary>
        /// Get User List
        /// </summary>
        public List<Layer> GetUserList {
            get
            {
                return GlobalData.LayerList;
            }
        }

        /// <summary>
        /// Get Layer Fields
        /// </summary>
        /// <param name="selectedLayer"></param>
        /// <returns></returns>
        public List<string> GetLayerFields(Layer selectedLayer)
        {
            if (GlobalData.LayerList.IsListFull())
            {
                Layer layer = GlobalData.LayerList.Find(l => l == selectedLayer);
                if (layer != null)
                    return layer.FieldList;
            }
            return null;
        }

        private void PaintLayerColor(Layer layer)
        {
            if (layer == null)
                return;

            if (layer.LayerTypeValue == LayerType.Line)
            {
                if (layer.TransCadColorValue != null)
                {
                    dal.SetLineWidth(layer.LayerName, linearWidth);
                    
                    TransCadColor conv = layer.TransCadColorValue.GetTranscadConvertedObject();
                    dal.SetLineColor(layer.LayerName, layer.TransCadColorValue.Red,
                        layer.TransCadColorValue.Grean,
                        layer.TransCadColorValue.Blue);
                    
                }
            }
            if (layer.LayerTypeValue == LayerType.Area)
            {
                if (layer.TransCadColorValue != null)
                {
                    dal.SetBorderWidth(layer.LayerName, linearWidth);

                    TransCadColor conv = layer.TransCadColorValue.GetTranscadConvertedObject();
                    dal.SetBorderColor(layer.LayerName, conv.Red,
                            conv.Grean,
                            conv.Blue);
                }
            }
            if (layer.LayerTypeValue == LayerType.Point)
            {
                if (layer.TransCadColorValue != null)
                {
                    dal.SetIconSize(layer.LayerName, linearWidth);
                    
                    TransCadColor conv = layer.TransCadColorValue.GetTranscadConvertedObject();
                    dal.SetIconColor(layer.LayerName, conv.Red,
                            conv.Grean,
                            conv.Blue);
                }
            }

            //if (!layer.IsShowLabel)
            //{
            //    layer.FieldToLabel = "ID";
            //}
            //dal.SetLabelOnMap(layer.LayerName, layer.IsShowLabel, layer.FieldToLabel, string.Empty);  
            
        }

        public void SetIsMunSelected()
        {
            bool isMunSelected = false;
            GlobalData.LayerList.ForEach(layer =>
            {

                if (layer.LayerName.ToLower() == is_mun.ToLower())
                {
                    isMunSelected = layer.IsSelectedByUser;
                }
            });
            GlobalData.TransCadCurrentEnvoromnetInfo.IsMunLayerExists = isMunSelected;
        }


        /// <summary>
        /// Update Map
        /// </summary>
        public void UpdateMap()
        {
            bool isProcessDone = false; 
            if (GlobalData.LayerList.IsListFull())
            {
                GlobalData.LayerList.ForEach(layer =>
                    {
                        List<string> allGisLayers = dal.GetLayersName();
                        if (!layer.IsSelectedByUser)
                        {
                            // Drop the layer if the requested layer exists
                            if (allGisLayers.Exists(l => l == layer.LayerName))
                            {
                                dal.DropLayer(ExportConfigurator.GetConfig().MapLayerName,layer.LayerName);
                                isProcessDone = true; 
                            }
                        }
                        else
                        {
                            // Add selected layer if the requested layer was not already appended to map
                            if (!allGisLayers.Exists(l => l == layer.LayerName))
                            {
                                isProcessDone = true; 
                                var array = new object[2];
                                array[0] = new object[2] {"Shared",layer.Shared.ToStringTranscadValue() };
                                array[1] = new object[2] { "ReadOnly", layer.ReadOnly.ToStringTranscadValue() };
                                dal.AddLayer(ExportConfigurator.GetConfig().MapLayerName,
                                layer.LayerName,layer.DBName,layer.DBLayerName,array);
                                if (layer.LayerTypeValue == LayerType.Point)
                                {
                                    dal.SetIcon(layer.LayerName, "Font Character", "Arial|Bold|10", iconFormIndex);
                                    iconFormIndex += 4;
                                }
                                if (layer.LayerTypeValue == LayerType.Line)
                                {
                                    dal.SetLineStyle(layer.LayerName);
                                }
                            }
                            PaintLayerColor(layer);
                            if (!string.IsNullOrEmpty(layer.Label))
                            {
                                dal.SetLabelOnMap(layer.LayerName, true, layer.Label,false); 
                            }
                        }
                    });
                if (isProcessDone)
                {
                    dal.RedrawMap(ExportConfigurator.GetConfig().MapLayerName);
                    SetUserLayers();
                }
            }
        }
    }
}
