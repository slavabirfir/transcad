using System;
using System.Collections.Generic;
using BLEntities.Entities;
using Utilities;
using DAL.SQLServerDAL;
using DAL;
using System.Data.SqlClient;
using BLEntities.Model;
using Logger;
using IDAL;

namespace BLManager
{
    public class BLZoneImportAreaChanged
    {
        private PriceImportDAL _dal = new PriceImportDAL();
        private readonly ITransCadMunipulationDataDAL dalTranscad = new TransCadMunipulationDataDAL();
        public BLZoneImportAreaChanged(List<PriceArea> lst)
        {
            ChangedListBLPriceArea = new List<BLPriceArea>(); 
            if (lst.IsListFull())
              lst.ForEach(e=>ChangedListBLPriceArea.Add(new BLPriceArea { ID = e.ID,Name = e.Name,ReplaceZoneCode=false,TranscadId = e.TranscadId}));
            
        }
        public List<BLPriceArea> ChangedListBLPriceArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool IsSelectablePriceArea(PriceArea pa)
        {
            return true;
        }

        /// <summary>
        /// UpdateList
        /// </summary>
        public void UpdateList()
        {
            string connectionString = DalShared.GetConnectionString(GlobalData.LoginUser.UserOperator.OperatorGroup,false);
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var dalPriceArea = new PriceImportDAL();
                if (ChangedListBLPriceArea.IsListFull())
                 {
                     ChangedListBLPriceArea.ForEach(pl=>
                         {
                             if (pl.ReplaceZoneCode)
                             {
                                 pl.ID = dalPriceArea.GetMaxNumInUseByAutoNumKod(1, connection);
                                 dalTranscad.UpdatePriceListPolygon(pl, ExportConfiguration.ExportConfigurator.GetConfig().PriceAreaPolygonName);
                             }
                         });
                 }
            }
            catch (Exception exp)
            {
                LoggerManager.WriteToLog(exp);
                throw;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }


    }

    public class BLPriceArea : PriceArea
    {
        public bool ReplaceZoneCode { get; set; }
    }
}
