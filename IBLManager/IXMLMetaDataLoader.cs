using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Entities;

namespace IBLManager
{
    public interface IXMLMetaDataLoader
    {
        void FillXMLMetaData(ModelMetaData metaData);
        bool CompareModelMetaDataXMLAndTranscad(ModelMetaData metaData,ref string errorMessage);
    }
}
