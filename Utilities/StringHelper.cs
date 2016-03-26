using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class StringHelper
    {
        /// <summary>
        /// Convert To Hebrew Encoding
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToHebrewEncoding(string source)
        {
            byte[] unicodeBytes = Encoding.Default.GetBytes(source);
            return Encoding.GetEncoding(1255).GetString(unicodeBytes);
        }
        /// <summary>
        /// Convert To Default From 1255
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToDefaultFrom1255(string source)
        {
            byte[] unicodeBytes = Encoding.GetEncoding(1255).GetBytes(source);
            return Encoding.Default.GetString(unicodeBytes);
        }
        /// <summary>
        /// Convert To ASCII Encoding
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToASCIIEncoding(string source)
        {
            byte[] unicodeBytes = Encoding.Default.GetBytes(source);
            return Encoding.ASCII.GetString(unicodeBytes);
        }
        /// <summary>
        /// Convert To Unicode Encoding
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToUnicodeEncoding(string source)
        {
            byte[] unicodeBytes = Encoding.Default.GetBytes(source);
            return Encoding.Unicode.GetString(unicodeBytes);
        }
        /// <summary>
        /// ConvertToISOEncoding
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToISOEncoding(string source)
        {
            byte[] unicodeBytes = Encoding.Default.GetBytes(source);
            return Encoding.GetEncoding("ISO-8859-1").GetString(unicodeBytes);
        }
        
    }
}
