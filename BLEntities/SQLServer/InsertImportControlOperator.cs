using System;

namespace BLEntities.SQLServer 
{
    public class InsertImportControlOperator
    {
        private DateTime _mImportStartDate;
        public DateTime ImportStartDate
        {
            get { return _mImportStartDate.ToLocalTime(); }
            set { _mImportStartDate = value; }
        }

        private DateTime _mImportFinishDate;
        public DateTime ImportFinishDate
        {
            get { return _mImportFinishDate.ToLocalTime(); }
            set { _mImportFinishDate = value; }
        }


        public int OperatorId { get; set; }

        public int DataSource { get; set; }
        
        public int ImportControlId { get; set; }
        public int? Status { get; set; }
    }
 
}
