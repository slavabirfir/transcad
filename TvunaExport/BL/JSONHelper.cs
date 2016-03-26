using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace TvunaExport.BL
{
    public class JSONHelper
    {
        /// <summary>
        /// Json Serialize To Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] JsonSerializeToArray<T>(T t)
        {

            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(stream, t);
            return stream.ToArray();

        }

        /// <summary>
        /// Json Serialize To String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonSerializeToString<T>(T t)
        {
            return Encoding.UTF8.GetString(JsonSerializeToArray<T>(t));
        }


        /// <summary>
        /// Json DeSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T JsonDeSerialize<T>(byte[] array)
        {

            MemoryStream stream = new MemoryStream(array, 0, array.Length);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            stream.Position = 0;
            return (T)ser.ReadObject(stream);

        }

        /// <summary>
        /// Json DeSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeSerialize<T>(string jsonString)
        {
            return JsonDeSerialize<T>(Encoding.UTF8.GetBytes(jsonString));
        }
    }
}
