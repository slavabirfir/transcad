using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Utilities;
using System.Data.Common;

namespace Utilities
{
    public class SQLServerHelper
    {
         /// <summary>
         /// Get Data By Connection
         /// </summary>
         /// <param name="spName"></param>
         /// <param name="inputParameter"></param>
         /// <param name="connection"></param>
         /// <returns></returns>
         public static DataTable GetDataByConnection(string spName, Dictionary<string, object> inputParameter, SqlConnection connection)
         {
             return GetDataByConnection(spName,inputParameter,connection,null);
         }

        /// <summary>
        /// Get Data
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable GetDataByConnection(string spName, Dictionary<string, object> inputParameter, SqlConnection connection,SqlTransaction transaction)
        {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.CommandText = spName;
                if (transaction != null)
                    command.Transaction = transaction;
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);
                return dt;
        }



        /// <summary>
        /// Get Data
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable GetData(string spName, Dictionary<string, object> inputParameter, String connectionString)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = sqlCon;
                command.CommandText = spName;
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                var sqlAdapter = new SqlDataAdapter(command);
                var dt = new DataTable();
                sqlAdapter.Fill(dt);
                return dt;
            }
        }
        /// <summary>
        /// Execute Query
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool ExecuteQuery(string spName, Dictionary<string, object> inputParameter, String connectionString)
        {
            using (var sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                using (var command = new SqlCommand
                                  {
                                      CommandType = CommandType.StoredProcedure,
                                      Connection = sqlCon,
                                      CommandTimeout = 999999,
                                      CommandText = spName
                                  })
                {
                    if (inputParameter != null)
                    {
                        foreach (string paramName in inputParameter.Keys)
                        {
                            command.Parameters.Add(new SqlParameter
                                                       {
                                                           Direction = ParameterDirection.Input,
                                                           ParameterName = "@" + paramName,
                                                           Value = inputParameter[paramName]
                                                       });
                        }
                    }
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Get Out Put Parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outPutParamName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static T GetOutPutParameter<T>(string spName, Dictionary<string, object> inputParameter,string outPutParamName,T defaultVaue, String connectionString)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = sqlCon;
                command.CommandText = spName;
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                command.Parameters.Add(new SqlParameter
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "@" + outPutParamName,
                    Value = defaultVaue
                });
                command.ExecuteNonQuery();
                foreach (SqlParameter sqlParam in command.Parameters)
                {
                    string paramKey = sqlParam.ParameterName.Substring(1);
                    if (paramKey.Equals(outPutParamName))
                        return (T) sqlParam.Value;
                }
                return default(T);
            }
        }


        public static Dictionary<string,object> GetOutPutParametersDictionary(string spName, Dictionary<string, object> inputParameter,Dictionary<string, SqlParameter> outputParameter,  String connectionString)
        {
            using (var sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                var command = new SqlCommand
                                  {
                                      CommandType = CommandType.StoredProcedure,
                                      Connection = sqlCon,
                                      CommandText = spName
                                  };
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                if (outputParameter != null)
                {
                    foreach (string paramName in outputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = outputParameter[paramName].Direction,
                            ParameterName = "@" + paramName,
                            Value = outputParameter[paramName].Value,
                            Size = outputParameter[paramName].Size,
                            SqlDbType = outputParameter[paramName].SqlDbType
                        });
                    }
                }
                command.ExecuteNonQuery();
                var result = new Dictionary<string, object>();
                if (outputParameter != null)
                {
                    foreach (SqlParameter sqlParam in command.Parameters)
                    {
                        if (outputParameter.ContainsKey(sqlParam.ParameterName.Substring(1)))
                        {
                            var paramKey = sqlParam.ParameterName.Substring(1);
                            result.Add(paramKey, sqlParam.Value);
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Get Scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static T GetScalar<T>(string spName, Dictionary<string, object> inputParameter,   String connectionString)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = sqlCon;
                command.CommandText = spName;
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                 
                return (T) command.ExecuteScalar ();
                
            }
        }


        //private static String UDFFormatString = string.Format("select {}") 
        //        cmd.CommandText = "select datbasename.dbo.functionname(@param1) as functionresult"
// cmd.CommandType = CommandType.text
// cmd.Parameters.AddWithValue("param1", "param1val")
//String result;
//conn.Open()
//dr = cmd.ExecuteReader
// dr.HasRows Then
// dr.Read()
//result = dr("functionResult")

        /// <summary>
        /// run udf function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="UDFName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static T GetUDFReturnValue<T>(string UDFName, Dictionary<string, object> inputParameter, String connectionString)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string commandText =string.Concat("select ",UDFName);
                StringBuilder sb = new StringBuilder();
                if (inputParameter.IsDictionaryFull())
                {
                    
                    sb.Append("(");
                    int counter = inputParameter.Keys.Count-1,i=0;
                    foreach (string key in inputParameter.Keys)
                    {
                        if (i< counter)
                            sb.Append(String.Concat("@", key, ","));    
                        else
                            sb.Append(String.Concat("@", key));
                        i++;
                    }
                    sb.Append(")");
                    sb.Append(" as functionresult");
                }
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = sqlCon;
                command.CommandText =string.Concat(commandText,sb.ToString());
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows)
                {
                    dr.Read();
                    return (T)dr["functionResult"];
                }
                else
                    throw new ApplicationException(string.Format("The function {0} didn't return any value",UDFName));
                
            }
        }


        public static bool ExecuteQueryWithOutputParameters(string spName, Dictionary<string, object> inputParameter, Dictionary<string, object> outputParameter, SqlConnection connection)
        {
            return ExecuteQueryWithOutputParameters(spName, inputParameter, outputParameter, connection, null);
        }
        /// <summary>
        /// Execute Query With Output Parameters
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ExecuteQueryWithOutputParameters(string spName, Dictionary<string, object> inputParameter, Dictionary<string, object> outputParameter, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;
            command.Connection = connection;
            if (transaction!=null)
               command.Transaction = transaction;
            command.CommandTimeout = 999999;
            if (inputParameter != null)
            {
                foreach (string paramName in inputParameter.Keys)
                {
                    command.Parameters.Add(new SqlParameter
                    {
                        Direction = ParameterDirection.Input,
                        ParameterName = "@" + paramName,
                        Value = inputParameter[paramName]
                    });
                }
            }
            
            if (outputParameter != null)
            {
                foreach (string paramName in outputParameter.Keys)
                {
                    command.Parameters.Add(new SqlParameter
                    {
                        Direction = ParameterDirection.Output,
                        ParameterName = "@" + paramName,
                        Value = outputParameter[paramName]
                    });
                }
            }
            bool result = command.ExecuteNonQuery() > 0;
            if (outputParameter.IsDictionaryFull<string, object>() && command.Parameters != null && command.Parameters.Count>0)
            {
                foreach (SqlParameter sqlParam in command.Parameters)
                {
                    string paramKey = sqlParam.ParameterName.Substring(1);
                    if (sqlParam.Direction == ParameterDirection.Output && outputParameter.ContainsKey(paramKey))
                        outputParameter[paramKey] = sqlParam.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// Execute Query
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool ExecuteQuery(string spName, Dictionary<string, object> inputParameter, SqlConnection connection)
        {
            return ExecuteQuery(spName, inputParameter, connection, null);
        }
        /// <summary>
        /// Execute Query
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameter"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ExecuteQuery(string spName, Dictionary<string, object> inputParameter, SqlConnection connection, SqlTransaction transaction)
        {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                command.Connection = connection; 
                if (transaction !=null)
                    command.Transaction = transaction;
                command.CommandTimeout = 999999;
                if (inputParameter != null)
                {
                    foreach (string paramName in inputParameter.Keys)
                    {
                        command.Parameters.Add(new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + paramName,
                            Value = inputParameter[paramName]
                        });
                    }
                }
                return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// ExecuteQueryBySqlParameter
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameters"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ExecuteQueryBySqlParameter(string spName, SqlParameter[] inputParameters, SqlConnection connection, SqlTransaction transaction)
        {
            var command = new SqlCommand
                              {
                                  CommandType = CommandType.StoredProcedure,
                                  CommandText = spName,
                                  Connection = connection,
                                  CommandTimeout = 0
                              };

            if (transaction != null)
                command.Transaction = transaction;
            if (inputParameters != null)
            {
                foreach (var parameter in inputParameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// ExecuteQueryBySqlParameter
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputParameters"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool ExecuteQueryBySqlParameter(string spName, SqlParameter[] inputParameters, SqlConnection connection)
        {
            return ExecuteQueryBySqlParameter(spName, inputParameters, connection, null);
        }
        

    }
}
