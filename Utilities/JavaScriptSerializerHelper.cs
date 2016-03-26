using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Utilities
{
    /// <summary>
    /// Java Script Serializer Helper
    /// </summary>
    public class JavaScriptSerializerHelper
    {
        private static JavaScriptSerializer sr = new JavaScriptSerializer();
        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return sr.Serialize(obj);
        }
        /// <summary>
        /// DeSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string json)
        {
            return (T)sr.Deserialize<T>(json);
        }
    }
}
