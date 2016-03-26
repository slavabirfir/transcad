using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Linq;
using Microsoft.VisualBasic;

namespace Utilities
{
    public static class Extenders
    {
        private const string Dot = ".";

        /// <summary>
        /// IsNumber
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            return !string.IsNullOrEmpty(str) && str.All(Char.IsNumber);
        }

        /// <summary>
        /// Convert From Int To Tvuna DoubleFormat
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ConvertFromIntToTvunaDoubleFormat(this int value)
        {
            string valString = value.ToString();
            if (valString.Length >= 3)
            {
                valString = String.Concat(valString.Substring(0, 2), Dot, valString.Substring(2, valString.Length - 2));
            }
            return Convert.ToDouble(valString);
        }


        /// <summary>
        /// Extract Numbers Only
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ExtractNumbersOnly(this string str)
        {
            string number = "0";
            foreach (char item in str)
            {
                if (Information.IsNumeric(item))
                {
                    number += item.ToString();
                }
            }
             
            return int.Parse(number);
        }

        //public static bool IsNumber(this string text)
        //{
        //    Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
        //    return regex.IsMatch(text);
        //}

        /// <summary>
        /// IsLengthLess7
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsLengthLess7(this int? val)
        {
            return val.HasValue && val.ToString().Length < 7;
        }

        /// <summary>
        /// ToStringddMMyyyy
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToStringddMMyyyy(this DateTime val)
        {
            return val.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// ToStringddMMyyyyNONSeparator
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToStringddMMyyyyhhmmssNONSeparator(this DateTime val)
        {
            return val.ToString("ddMMyyyy hhmmss");
        }

        /// <summary>
        /// To Fix Length
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string ToFixLength(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (str.Length <= maxLength)
                return str;
            return str.Substring(0, maxLength); 
        }


        /// <summary>
        /// Add Zerro In End
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static int AddZerroInEnd(this int val, int maxLength)
        {
            if (val.ToString().Length >= maxLength )
                return val;
            int delta = maxLength - val.ToString().Length;
            for(int i=0;i<delta;i++)
                val = val * 10;
            return val;
        }


        /// <summary>
        /// Add Zerro In Start
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string AddZerroInStart(this int val, int maxLength)
        {
            if (val.ToString().Length >= maxLength)
                return val.ToString();
            string value = string.Empty;  
            int delta = maxLength - val.ToString().Length;
            for (int i = 0; i < delta; i++)
                value = value + "0";
            return string.Format("{0}{1}",value,val) ;
        }


        /// <summary>
        /// Space
        /// </summary>
        /// <param name="str"></param>
        /// <param name="spacesCount"></param>
        /// <returns></returns>
        public static string Space(this string str, int spacesCount)
        {
            return new String(' ', spacesCount);
        }
        /// <summary>
        /// To Export Format 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charCounter"></param>
        /// <returns></returns>
        public static string ToExportFormatSpaceBefore(this string str, int charCounter)
        {
            
            string formattedString = string.Empty;

            if (string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < charCounter; i++)
                {
                    formattedString = " " + formattedString;
                }
            }
            else
            {
                if (str.Length > charCounter)
                {
                    //throw new ApplicationException(string.Format("The string {0} length more then permitted {1}", str.Length, charCounter));
                    formattedString = str.Substring(0, charCounter); 
                }
                else
                {
                    int diff = charCounter - str.Length;
                    for (int i = 0; i < diff; i++)
                    {
                        formattedString = " " + formattedString; 
                    }
                    formattedString += str;
                }

            }
            
            return formattedString;
        }

        /// <summary>
        /// To Export Format Space After
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charCounter"></param>
        /// <returns></returns>
        public static string ToExportFormatSpaceAfter(this string str, int charCounter)
        {
  
            string formattedString = string.Empty;

            if (string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < charCounter; i++)
                {
                    formattedString += " ";
                }
            }
            else
            {
                if (str.Length > charCounter)
                {
                    //throw new ApplicationException(string.Format("The string {0} length more then permitted {1}", str.Length, charCounter));
                    formattedString = str.Substring(0, charCounter);
                }
                else
                {
                    int diff = charCounter - str.Length;
                    for (int i = 0; i < diff; i++)
                    {
                        formattedString += " ";
                    }
                    str += formattedString;
                }

            }

            return str;
        }


        /// <summary>
        /// Verify On Null Const
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string VerifyIntOnNullConst(this int data)
        {
            //return data == 0 ? "null" : data.ToString();
            return data == 0 ? string.Empty : data.ToString(); 
        }


        /// <summary>
        /// Extract Numbers Only
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string  GetLastChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            else
            {
                return str.Substring(str.Length - 1); 
            }
        }

        ///// <summary>
        ///// Distinct
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="source"></param>
        ///// <returns></returns>
        //public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source)
        //{ 
        //    Dictionary<T, object> dict = new Dictionary<T, object>(); 
        //    foreach (T element in source) 
        //    { 
        //        if (!dict.ContainsKey(element)) 
        //            { dict.Add(element, null); 
        //                yield return element; 
        //            } 
        //    } 
        //}

        /// <summary>
        /// Is Data Set Has Table Full
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool  IsDataSetHasTableFull(this DataSet ds)
        {
            return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
        }
        /// <summary>
        /// Is Data Table Full
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsDataTableFull(this DataTable dt)
        {
            return dt!=null && dt.Rows.Count > 0;
        }

        /// <summary>
        /// IsD ictionary Full
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static bool IsDictionaryFull<T,V>(this Dictionary<T,V> dic)
        {
            return dic != null && dic.Keys.Count > 0;
        }

        /// <summary>
        /// Is List Full
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsListFull(this IList list)
        {
            
            return list != null && list.Count > 0;
        }

        /// <summary>
        /// To String Transcad Value
        /// </summary>
        /// <param name="boolValue"></param>
        /// <returns></returns>
        public static string  ToStringTranscadValue(this bool boolValue)
        {
            return boolValue ? "True" : "False";
        }
    }
}
