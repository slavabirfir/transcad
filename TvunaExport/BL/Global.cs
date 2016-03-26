using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TvunaExport.BL;

namespace TvunaExport.BL
{
    public static class Global
    {
        const int PeriodId = 1;
        public static InsertImportControlOperator ImportControlOperator { get; set; }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="IdOperator"></param>
        public static void Initialize(int IdOperator)
        {
            DateTime now = DateTime.Now;
            ImportControlOperator = new InsertImportControlOperator 
            {
                OperatorId = IdOperator,
                ImportFinishDate = now,
                ImportStartDate = now,
                FromDate = now.AddDays(-1),
                ToDate = now.AddDays(1),
                PeriodId = PeriodId
            };
        }
        /// <summary>
        /// Get Json Of Import ControlOperator
        /// </summary>
        /// <returns></returns>
        public static string GetJsonOfImportControlOperator()
        {
            return JSONHelper.JsonSerializeToString<InsertImportControlOperator>(ImportControlOperator);
        }
    }
}
