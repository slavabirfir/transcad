using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DAL;
using IDAL;
using Utilities;
using BLEntities.Entities;
using BLEntities.Model;

namespace BLManager
{
    public class BlClusterValidExportDateManage
    {
        #region vars
        private const string FormatConst = "dd/MM/yyyy";
        private readonly IInternalBaseDal _internalBaseDal;
        public List<string> ReportResults { get; set; }
        public List<OperatorValidateLineExportDate> OperatorValidateLineExportDateData { get; set; }
        public List<BaseTableEntity> Clusters { get; set; }
        #endregion
        /// <summary>
        /// ctr
        /// </summary>
        public BlClusterValidExportDateManage(bool allOperators)
        {
            _internalBaseDal = new TransportLicensingDal();
            Init(allOperators);
        }

        public BlClusterValidExportDateManage(): this(true){}

        /// <summary>
        /// Init
        /// </summary>
        private void Init(bool allOperators)
        {
            OperatorValidateLineExportDateData = _internalBaseDal.GetOperatorValidateLineExportDate().Where(op=>op.IdOperator!=3).ToList();
            InitClusters();
            if (!allOperators) return;
            SetOperatorNameValue();
            AddOperatorsYetNotDefined();
        }
        // MultiState
        private void InitClusters()
        {
            Clusters = new List<BaseTableEntity>(GlobalData.ClusterToZoneDictionary.Count);
            GlobalData.ClusterToZoneDictionary.Keys.ToList().ForEach(c =>
            {
                if (!Clusters.Exists(cId => cId.ID == c))
                    Clusters.Add(new BaseTableEntity
                                     {
                                         ID = c,
                                         Name = string.Format("{0} - {1}", GlobalData.ClusterToZoneDictionary[c].ClusterName, GlobalData.ClusterToZoneDictionary[c].ClusterID) 
                                     });
            });
            Clusters = Clusters.OrderBy(i => i.Name).ToList();
            Clusters.Insert(0, new BaseTableEntity
                                   {
                                       ID =0,
                                       Name = string.Empty
                                   });
            
        }

        /// <summary>
        /// SetOperatorNameValue
        /// </summary>
        private void SetOperatorNameValue()
        {
            
                OperatorValidateLineExportDateData.ForEach(opVed => {
                           var oper = GlobalData.OperatorList.SingleOrDefault(op=>op.IdOperator == opVed.IdOperator);
                           if (oper != null){
                               opVed.OperatorName = oper.OperatorName;
                           }});
            
        }


        private void AddOperatorsYetNotDefined()
        {
            if (GlobalData.OperatorList!=null && OperatorValidateLineExportDateData != null)
            {
                GlobalData.OperatorList.ForEach(oper =>{
                if (!OperatorValidateLineExportDateData.Exists(e=>e.IdOperator == oper.IdOperator))
                {
                    OperatorValidateLineExportDateData.Add(new OperatorValidateLineExportDate
                                                               {
                                                                   IdOperator = oper.IdOperator,
                                                                   OperatorName = oper.OperatorName,
                                                                   ClusterValidateLineExportDates  = new List<ClusterValidateLineExportDate>()
                                                               });
                }

                                                        }); //Exclude Eged
                    OperatorValidateLineExportDateData = OperatorValidateLineExportDateData.Where(op=>op.IdOperator!=3).OrderBy(el => el.OperatorName).ToList();
            }
        }
        
        /// <summary>
        /// GetClusterDataByOperatorValidateLineExportDateSelected
        /// </summary>
        /// <param name="operatorValidateLineExportDateSelected"></param>
        /// <returns></returns>
        public List<ClusterValidateLineExportDate> GetClusterDataByOperatorValidateLineExportDateSelected(OperatorValidateLineExportDate operatorValidateLineExportDateSelected)
        {
            if (OperatorValidateLineExportDateData.IsListFull())
            {
                var results = OperatorValidateLineExportDateData.FindLast(opr => opr.IdOperator == operatorValidateLineExportDateSelected.IdOperator).ClusterValidateLineExportDates;
                if (results.IsListFull())
                    results.ForEach(c=>c.ClusterName = GetClusterNameById(c.IdCluster));
                return results;
            }
            return null;
        }
        /// <summary>
        /// DeleteClusterValidateLineExportDate
        /// </summary>
        /// <param name="operatorValidateLineExportDate"></param>
        /// <param name="clusterValidateLineExportDate"></param>
        public void DeleteClusterValidateLineExportDate(OperatorValidateLineExportDate operatorValidateLineExportDate, ClusterValidateLineExportDate clusterValidateLineExportDate)
        {
            if (operatorValidateLineExportDate == null || clusterValidateLineExportDate == null) return;
            operatorValidateLineExportDate.ClusterValidateLineExportDates.Remove(clusterValidateLineExportDate);
            Save();
        }
        /// <summary>
        /// Save
        /// </summary>
        private void Save()
        {
            _internalBaseDal.SaveOperatorValidateLineExportDate(OperatorValidateLineExportDateData);
        }
        /// <summary>
        /// SaveRow
        /// </summary>
        /// <param name="operatorValidateLineExportDate"></param>
        /// <param name="clusterId"></param>
        /// <param name="date"></param>
        public void SaveRow(OperatorValidateLineExportDate operatorValidateLineExportDate, int clusterId, string date)
        {
            if (operatorValidateLineExportDate == null) return;
            var clusterValidateLineExportDate = operatorValidateLineExportDate.ClusterValidateLineExportDates.SingleOrDefault(el => el.IdCluster == clusterId);
            if (clusterValidateLineExportDate!=null)
                clusterValidateLineExportDate.ValidExportDate = GetDateTimeFromString(date);
            else
                operatorValidateLineExportDate.ClusterValidateLineExportDates.Add(new ClusterValidateLineExportDate
                                                                                      {
                                                                                          IdCluster = clusterId,
                                                                                          ValidExportDate = GetDateTimeFromString(date)
                                                                                      });
                
            Save();
        }
        /// <summary>
        /// ParseToDateTime
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateTime? GetDateTimeFromString(string dateString)
        {
            string[] format = { FormatConst };
            DateTime date;

            if (!DateTime.TryParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
            {
                return null;
            }
            return date;
        }
        /// <summary>
        /// GetRestrictedByValidateExportDateList
        /// </summary>
        /// <returns></returns>
        private static List<CatalogInfo> GetRestrictedByValidateExportDateList ()
        {
            var restrictedCiList = new List<CatalogInfo>();
            var restrictedRlList = GlobalData.RouteModel.RouteLineList.Where(rl => 
                    {
                        var dt = GetDateTimeFromString(rl.ValidExportDate);
                        return dt.HasValue && DateTime.Today < dt.Value;
                    }).ToList();
            if (restrictedRlList.IsListFull())
            {
                 restrictedCiList = GlobalData.CatalogInfolList.Where(c => restrictedRlList.Exists(rl=>rl.Catalog ==c.Catalog && rl.IdCluster == c.IdCluster)).ToList();
            }
            return restrictedCiList;
        }
        
        /// <summary>
        /// GetClusterNameById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetClusterNameById(int id)
        {
            if (Clusters!=null)
            {
                var cluster = Clusters.SingleOrDefault(c => c.ID == id);
                if (cluster != null)
                    return cluster.Name; 
            }
            return string.Empty;  
        }


        /// <summary>
        /// GetClustersByOperatorId
        /// </summary>
        /// <param name="operatorValidateLineExportDateSelected"></param>
        /// <returns></returns>
        public List<BaseTableEntity> GetClustersByOperatorId(OperatorValidateLineExportDate operatorValidateLineExportDateSelected)
        {
            IInternalBaseDal internalBaseDal = new InternalTvunaImplementationDal();
            var clusters = new List<BaseTableEntity>(internalBaseDal.GetClustersByOpearatorId(operatorValidateLineExportDateSelected.IdOperator));
            var result = Clusters.Where(c => clusters.Exists(cInner => cInner.ID == c.ID)).OrderBy(c => c.Name).ToList();
            result.Insert(0, new BaseTableEntity
            {
                ID = 0,
                Name = string.Empty
            });
            return result;
        }
        /// <summary>
        /// GetGeneralOperatorReport
        /// </summary>
        /// <param name="sourse"></param>
        /// <returns></returns>
        public List<string> GetExportValidationDateReport(List<RouteLine> sourse)
        {
            var resultList = new List<string>();
            if (OperatorValidateLineExportDateData.IsListFull()) 
            {
                var operatorValidateLineExportDate = OperatorValidateLineExportDateData.SingleOrDefault(el => el.IdOperator == GlobalData.LoginUser.UserOperator.IdOperator && el.ClusterValidateLineExportDates.IsListFull());
                if (operatorValidateLineExportDate != null)
                {
                    var lstOperatorValidateLineExportDateNotValid = operatorValidateLineExportDate.ClusterValidateLineExportDates.Where(clusterValidateLineExportDate => sourse.Exists(rl => rl.IdCluster == clusterValidateLineExportDate.IdCluster && clusterValidateLineExportDate.ValidExportDate > DateTime.Now)).ToList();
                    if (lstOperatorValidateLineExportDateNotValid.IsListFull())
                        lstOperatorValidateLineExportDateNotValid.ForEach(c => resultList.Add(string.Format(Resources.BLManagerResource.ClusterOneNotValidExportDate, GetClusterNameById(c.IdCluster))));
                }
            }
            var restrictedByValidateExportDateList = GetRestrictedByValidateExportDateList();
            if (sourse.IsListFull() && restrictedByValidateExportDateList.IsListFull())
            {
                var errorList = restrictedByValidateExportDateList.Where(c => sourse.Exists(rl => c.Catalog == rl.Catalog && rl.IdCluster == c.IdCluster)).ToList();
                if (errorList.IsListFull())
                    errorList.ForEach(l => resultList.Add(string.Format(Resources.BLManagerResource.RouteLineOneNotValidExportDate, l.Catalog,Clusters.Single(c=>c.ID==l.IdCluster).Name)));
            }
            return resultList;          
        }
        /// <summary>
        /// Show Report
        /// </summary>
        /// <param name="source"></param>
        public void ShowReport(List<string> source)
        {
            if (source == null || !source.IsListFull()) return;
            using (var writer = new CsvWriter())
            {
                var outputCsvFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), string.Format("Report_{0}.csv", DateTime.Now.ToString("ddMMyyyy_hh_mm_ss")));
                writer.WriteCsv(BuildErrorList(source), outputCsvFileName, Encoding.UTF8);
                ProcessLauncher.RunProcess("notepad.exe", "\"" + outputCsvFileName + "\"");
            }
        }

        /// <summary>
        /// BuildClustersDataTableErrorList
        /// </summary>
        /// <param name="errorList"></param>
        /// <returns></returns>
        private static DataTable BuildErrorList(List<string> errorList)
        {
            var dt = new DataTable();
            dt.Columns.Add("תיאור", typeof(string));
            errorList.ForEach(sc =>
            {
                var dr = dt.NewRow();
                dr["תיאור"] = sc;
                dt.Rows.Add(dr);
            });
            dt.AcceptChanges();
            return dt;
        }


        /// <summary>
        /// Remove Not Allowed Routes Based On Export Validatio Date
        /// </summary>
        /// <param name="sourse"></param>
        public List<RouteLine> GetAllowedRoutesBasedOnExportValidatioDate(List<RouteLine> sourse)
        {
            var returnedRoutes = new List<RouteLine>();
            if (OperatorValidateLineExportDateData.IsListFull())
            {
                var operatorValidateLineExportDate = OperatorValidateLineExportDateData.SingleOrDefault(el => el.IdOperator == GlobalData.LoginUser.UserOperator.IdOperator && el.ClusterValidateLineExportDates.IsListFull());
                if (operatorValidateLineExportDate != null)
                {
                    var lstOperatorValidateLineExportDateNotValid = operatorValidateLineExportDate.ClusterValidateLineExportDates.Where(clusterValidateLineExportDate => sourse.Exists(rl => rl.IdCluster == clusterValidateLineExportDate.IdCluster && clusterValidateLineExportDate.ValidExportDate > DateTime.Now)).ToList();
                    returnedRoutes = sourse.Where(rl => !lstOperatorValidateLineExportDateNotValid.Exists(c => c.IdCluster == rl.IdCluster)).ToList();
                }
            }

            var restrictedByValidateExportDateList = GetRestrictedByValidateExportDateList();
            var routes = !returnedRoutes.IsListFull() ? sourse : returnedRoutes;

            var lstCatalogNotAllowedToExport = restrictedByValidateExportDateList.Where(c => routes.Exists(rl => c.Catalog == rl.Catalog && rl.IdCluster == c.IdCluster)).ToList();
            if (lstCatalogNotAllowedToExport.IsListFull())
                returnedRoutes = routes.Where(rl => !lstCatalogNotAllowedToExport.Exists(c => c.IdCluster == rl.IdCluster && c.Catalog == rl.Catalog)).ToList();
            return returnedRoutes;
        }
    }
}
