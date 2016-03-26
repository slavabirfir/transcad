using System.Collections.Generic;
using System.Linq;
using IDAL;
using DAL;
using ExportConfiguration;
using BLEntities.Model;
using System.Threading;
using Utilities;

namespace BLManager
{
    public class EnablePriceListPolygonBl
    {
        public bool IsAreaLayerAppended()
        {
            ITransCadMunipulationDataDAL dalTranscad = new TransCadMunipulationDataDAL();
            var layers = dalTranscad.GetLayersName();
            if (layers.Any() && layers.Exists(l=>l.Equals(ExportConfigurator.GetConfig().PriceAreaPolygonName)))
            {
                return true;
            }

            if (string.IsNullOrEmpty(GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PriceListLayer))
            {
                return false; 
            }

            var array = new object[2];
            array[0] = new object[2] {"Shared", "True"};
            array[1] = new object[2] {"ReadOnly", "False"};

            var lstPossibleLayerNames = new List<string>
                                            {
                                                ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName,
                                                ExportConfigurator.GetConfig().PriceAreaPolygonName
                                            };
            foreach (string t in lstPossibleLayerNames)
            {
                if (dalTranscad.AddLayer(ExportConfigurator.GetConfig().MapLayerName,
                                         ExportConfigurator.GetConfig().PriceAreaPolygonName,
                                         GlobalData.LoginUser.UserOperator.SelectedTranscadClusterConfig.PriceListLayer,
                                         t, array))
                    break;
            }

            // Check if Input layer exists and change it name to priceList
            var priceListLayers = dalTranscad.GetMapLayers(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName, "Area");
            if (priceListLayers.IsListFull())
            {
                var arrayInner = new object[] {new object[2] {"Permanent", "True"}};
                dalTranscad.RenameLayer(ExportConfigurator.GetConfig().WorkspacePriceListAllOperstorsMapName,
                                        ExportConfigurator.GetConfig().PriceAreaPolygonName, arrayInner);
            }
            // Set read only PriceArea
            
            
           
            
            dalTranscad.SetReadOnlyFieldSet(ExportConfigurator.GetConfig().PriceAreaPolygonName,dalTranscad.PriceAreaReadOnlyFields, true);
            
            return true; 
        }
    }
}
