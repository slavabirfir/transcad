using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using System.Data.OleDb;
using System.IO;
using System.Data;
using BLEntities.Accessories;
using Utilities;


namespace DAL
{
    public class CSVFielOperationIml : ICSVFielOperation
    {
        #region ICSVFielOperation Members

        public List<JunctionVersionImport> GetJunctionVersionList(string fileNameFullPath, int operatorId)
        {
            List<JunctionVersionImport> juncVersionList = new List<JunctionVersionImport>();
            using (CsvReader reader = new CsvReader(fileNameFullPath))
            {
                while (reader.ReadNextRecord())
                {
                    if (Convert.ToInt32(reader.Fields[0]) == operatorId)
                    {
                        juncVersionList.Add(new JunctionVersionImport
                        {
                            OperatorId = Convert.ToInt32(reader.Fields[0]),
                            OfficeLineId = Convert.ToInt32(reader.Fields[1]),
                            Direction = Convert.ToInt32(reader.Fields[2]),
                            LineAlternative = string.IsNullOrEmpty(reader.Fields[3]) ? string.Empty : StringHelper.ConvertToHebrewEncoding(reader.Fields[3]),
                            JunctionVersion = Convert.ToInt32(reader.Fields[4]),
                            JunctionOrder = Convert.ToInt32(reader.Fields[5]),
                            JunctionId = Convert.ToInt64(reader.Fields[6]),
                            DistanceFromPreviousJunction = Convert.ToInt32(reader.Fields[7]),
                            DistanceFromOriginJunction = Convert.ToInt32(reader.Fields[8])

                        });
                    }
                }
            }

            return juncVersionList;

        }

        #endregion

        #region ICSVFielOperation Members
        /// <summary>
        /// BuildDTfromJunctionVersionExport
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private DataTable BuildDTfromJunctionVersionExport(List<JunctionVersionExport> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OperatorId", typeof(int));
            dt.Columns.Add("OfficeLineId", typeof(int));
            dt.Columns.Add("Direction", typeof(int));
            dt.Columns.Add("LineAlternative", typeof(string));
            dt.Columns.Add("JunctionVersion", typeof(int));
            dt.Columns.Add("PathToZipFile", typeof(string));
            lst.ForEach(e =>
            {
                DataRow dr = dt.NewRow();
                dr["OperatorId"] = e.OperatorId;
                dr["OfficeLineId"] = e.OfficeLineId;
                dr["Direction"] = e.Direction;
                dr["LineAlternative"] = e.LineAlternative;
                dr["JunctionVersion"] = e.JunctionVersion;
                dr["PathToZipFile"] = e.PathToZipFile;
                dt.Rows.Add(dr);
            });
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// SetJunctionVersionList
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lst"></param>
        public void SetJunctionVersionList(string fileName, List<JunctionVersionExport> lst)
        {
            using (CsvWriter writer = new CsvWriter())
            {
                writer.WriteCsv(BuildDTfromJunctionVersionExport(lst), fileName, Encoding.UTF8);
            }
        }

        #endregion
    }
}
