using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using BLEntities.Entities;
using System.IO;
using Utilities;
using Logger;
using BLEntities.Model;

namespace DAL
{
    public class ExportBaseToTextDal : IExportBaseDAL
    {
        #region IExportBaseDAL Members
        private readonly string _delimiter = string.Empty;
        private const string Zerro = "0";
        private const string Windows1255 = "windows-1255";

        //"LinkID",I,1,9,0,9,0,,,"",,Blank,
        //"NA",I,10,8,0,8,0,,,"",,Blank,
        //"NB",I,18,8,0,8,0,,,"",,Blank,
        //"Length",I,26,10,0,10,0,,,"",,Blank,
        //"A",I,36,1,0,1,0,,,"",,Blank,
        //"B",I,37,1,0,1,0,,,"",,Blank,
        //"C",I,38,1,0,1,0,,,"",,Blank,
        //"D",I,39,1,0,1,0,,,"",,Blank,
        //"E",I,40,1,0,1,0,,,"",,Blank,
        //"F",I,41,1,0,1,0,,,"",,Blank,
        //"G",I,42,1,0,1,0,,,"",,Blank,
        //"H",I,43,1,0,1,0,,,"",,Blank,
        //"Name",C,44,50,0,50,0,,,"",,Blank,
        public void Exportzg_ktaim(List<Link> lst,string fileName, ref string message)
        {
            try
            {
                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    if (lst.IsListFull())
                        lst.ForEach(it => sw.WriteLine(
                            String.Concat(
                             it.LinkID.ToString().ToExportFormatSpaceBefore(9), _delimiter,
                             it.NA.ToString().ToExportFormatSpaceBefore(8),_delimiter ,
                             it.NB.ToString().ToExportFormatSpaceBefore(8),_delimiter ,
                             it.Length.ToString().ToExportFormatSpaceBefore(10),_delimiter ,
                             " ",_delimiter,
                             " ",_delimiter,
                             " ",_delimiter,
                             " ",_delimiter,
                             " ",_delimiter,
                             " ",_delimiter,
                             " ",_delimiter,
                             " ",_delimiter,
                             it.Name.ToExportFormatSpaceBefore(50),_delimiter 
                            )));
                    sw.Flush();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }
        //"NodeID","Name","CityCode","Lat","Long","TypeDavid"
        //83
        //"NodeID",I,1,8,0,8,0,,,"",,Blank,
        //"A",I,9,1,0,1,0,,,"",,Blank,
        //"Name",C,10,50,0,50,0,,,"",,Blank,
        //"CityCode",C,60,4,0,4,0,,,"",,Blank,
        //"B",I,64,1,0,1,0,,,"",,Blank,
        //"Lat",R,65,9,0,9,6,,,"",,Blank,
        //"Long",R,74,9,0,9,6,,,"",,Blank,
        //"TypeDavid",C,83,1,0,1,0,,,"",,Blank,
        public void Exportzg_mokdim(List<Node> lst,string fileName, ref string message)
        {
            try
            {
                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    if (lst.IsListFull())
                        lst.ForEach(it => sw.WriteLine(
                            String.Concat(
                             it.NodeID.ToString().ToExportFormatSpaceBefore(8), _delimiter,
                             " ",
                             it.Name.ToExportFormatSpaceBefore(50), _delimiter,
                             it.CityCode.ToExportFormatSpaceBefore(4), _delimiter,
                             " ",
                             it.Lat.ToString().ToExportFormatSpaceBefore(9), _delimiter,
                             it.Long.ToString().ToExportFormatSpaceBefore(9), _delimiter,
                             it.TypeDavid.ToExportFormatSpaceBefore(1), _delimiter
                            )));
                    sw.Flush();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }
        //212
        //----"StopID",I,1,7,0,7,0,,,"",,Blank,-
        //----"Name",C,8,50,0,50,0,,,"",,Blank,-
        //"LinkID",I,58,9,0,9,0,,,"",,Blank,-
        //"DistNA",I,67,10,0,10,0,,,"",,Blank,-
        //"A",I,77,1,0,1,0,,,"",,Blank,-
        //"B",I,78,1,0,1,0,,,"",,Blank,-
        //"C",I,79,1,0,1,0,,,"",,Blank,-
        //-----"Lat",R,80,9,0,9,6,,,"",,Blank,-
        //----"Long",R,89,9,0,9,6,,,"",,Blank,-
        //"Street",C,98,50,0,50,0,,,"",,Blank,
        //"House",I,148,4,0,4,0,,,"",,Blank,
        //"Across",I,152,1,0,1,0,,,"",,Blank,
        //"D",I,153,1,0,1,0,,,"",,Blank,
        //"Neighborhood",C,154,50,0,50,0,,,"",,Blank,
        //"CityCode",C,204,4,0,4,0,,,"",,Blank,
        //"ID_SEKER",I,208,5,0,5,0,,,"",,Blank,
        //"OperatorId",I,213,3,0,3,0,,,"",,Blank,
        public void Exportzg_tachana(List<Stop> lst, string fileName, ref string message)
        {
            try
            {
                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    if (lst.IsListFull())
                        lst.ForEach(it => sw.WriteLine(
                            String.Concat(
                             it.StopID.ToString().ToExportFormatSpaceBefore(7), _delimiter,
                             it.Name.ToExportFormatSpaceAfter(50), _delimiter,
                             it.LinkID.ToString().ToExportFormatSpaceBefore(9), _delimiter,
                             Zerro.ToExportFormatSpaceAfter(10), _delimiter,
                             Zerro, _delimiter,
                             Zerro, _delimiter,
                             Zerro, _delimiter,
                             it.Lat.ToString().ToExportFormatSpaceAfter(9), _delimiter,
                             it.Long.ToString().ToExportFormatSpaceAfter(9), _delimiter,
                             it.Street.ToExportFormatSpaceBefore(50), _delimiter,
                             it.House.ToString().ToExportFormatSpaceBefore(4), _delimiter,
                             it.Across.ToString().ToExportFormatSpaceBefore(1), _delimiter,
                             Zerro, _delimiter,
                             Zerro.ToExportFormatSpaceAfter(50), _delimiter,
                             it.CityCode.ToExportFormatSpaceBefore(4), _delimiter,
                             it.ID_SEKER.ToString().ToExportFormatSpaceBefore(5), _delimiter,
                             GlobalData.LoginUser.UserOperator.IdOperator.ToString().ToExportFormatSpaceBefore(2),_delimiter
                            )));
                    sw.Flush();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }
        //54
        //"CityCode",C,1,4,0,4,0,,,"",,Blank,
        //"Name",C,5,50,0,50,0,,,"",,Blank,
        public void Exporttv_yeshuv(List<BLEntities.Entities.City> lst, string fileName, ref string message)
        {
            try
            {

                using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(Windows1255)))
                {
                    if (lst.IsListFull())
                        lst.ForEach(it => sw.WriteLine(
                            String.Concat(
                             it.Code.ToExportFormatSpaceBefore(4), _delimiter,
                             it.Name.ToExportFormatSpaceBefore(50), _delimiter
                            )));
                    sw.Flush();
                }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                message = exp.Message;
            }
        }

        #endregion

      
    }
}
