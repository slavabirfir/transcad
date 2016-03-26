using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLManager;
using BLEntities.Entities;
using System.IO;
using ExportConfiguration;
using Utilities;
using System.Xml.Linq;

namespace BLManager
{
    public class XMLMetaDataLoader : IXMLMetaDataLoader
    {
        private string GetXMLMetaDataFile
        {
            get
            {
                string xmlFile = "MetaModelData.xml";
                string fileName = Path.Combine(ExportConfigurator.GetConfig().DataFolder, xmlFile);
                if (!IoHelper.IsFileExists(fileName))
                {
                    throw new ApplicationException(string.Format("{0} file is not exists in application Data folder {1}", fileName, ExportConfigurator.GetConfig().DataFolder));
                }
                return fileName;
            }
        } 

        #region IXMLMetaDataLoader Members

        public void FillXMLMetaData(ModelMetaData metaData)
        {
            XDocument doc = XDocument.Load(GetXMLMetaDataFile);
            if (doc != null)
            {
                //List<Operator> resData = new List<Operator>();
                var data = (from p in doc.Descendants("layer")
                            select new 
                            {
                                LayerName = (p.Attribute("name").Value),
                                Fields = p.Element("fields").Elements().ToDictionary(k => k.Attribute("name").Value, v => v.Attribute("type").Value)
                            }).ToList();
                foreach (var item in data)
                {
                    metaData.XMLMetaData.Add(item.LayerName, item.Fields); 
                }   
            }
            
        }
        /// <summary>
        /// CompareModelMetaDataXMLAndTranscad
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public bool CompareModelMetaDataXMLAndTranscad(ModelMetaData metaData,ref string errorMessage)
        {
            foreach (var item in metaData.XMLMetaData.Keys)
            {
                Dictionary<String, String> fieldsXML = metaData.XMLMetaData[item];
                if (!metaData.TranscadMetaData.ContainsKey(item))
                {
                    errorMessage = String.Concat(errorMessage,"The ", item," layer was not found");
                    return false;
                }
                else
                {
                    Dictionary<String, String> fieldsTranscad = metaData.TranscadMetaData[item];
                    foreach (var itemFieldName in fieldsXML.Keys)
                    {
                        if (!fieldsTranscad.ContainsKey(itemFieldName))
                        {
                            errorMessage = String.Concat(errorMessage, "The ", item, " layer doesn't have the ",itemFieldName," field");
                            return false;
                        }
                        else
                        {
                            String xmlFieldType = fieldsXML[itemFieldName];
                            String transcadFieldType = fieldsTranscad[itemFieldName];
                            if (!xmlFieldType.Equals(transcadFieldType))
                            {
                                errorMessage = String.Concat(errorMessage, "The ", item, " layer field ", itemFieldName, " has uncorrect format. It should to be a ", xmlFieldType);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
