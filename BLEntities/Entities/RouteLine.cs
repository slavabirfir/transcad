using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using BLEntities.Model;
using Utilities;
using ExportConfiguration;

namespace BLEntities.Entities
{
    [HasSelfValidation]
    public class RouteLine : BaseClass
    {
        #region private
        private int idCluster;
        private int idOperator;
        private int idServiceType;
        private int idExclusivityLine;
        private int idZoneHead;
        private int idZoneSubHead;
        
        #endregion
        #region Properties

        public List<int> CityIds { get; set; }
        public List<long> JunctionIds { get; set; }

        public string OperatorName { get; set; }
        public byte[] ShapeFile { get; set; }
        
        public string ValidExportDate { get; set; }
        
        [RegexValidator(".+$",MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Signpost_Empty")]
        public string  Signpost { get; set; }
        
        [RangeValidator(1, RangeBoundaryType.Inclusive, 3, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
        , MessageTemplateResourceName = "Dir_Empty")]
        //[NotNullValidator( MessageTemplateResourceType = typeof(Resources.ValidationResource)
        //, MessageTemplateResourceName = "Dir_Empty")]
        public int? Dir { get; set; }

        [RegexValidator(".+$", MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Var_Empty")]
        public string Var { get; set; }
        
        public int  VarNum { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 999999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Operator_0")]
        public int IdOperator
        {
            get { return idOperator; }
            set
            {
                if (value != idOperator)
                {
                    var oper = GlobalData.OperatorList.FirstOrDefault(o => o.IdOperator == value);
                    OperatorName = oper != null ? oper.OperatorName : string.Empty;  
                }
                idOperator = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 999999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Cluster_0")]
        public int IdCluster
        {
            get
            {
                return idCluster; 
            }

            set
            {
                if (value != idCluster)
                {
                    if (GlobalData.BaseTableEntityDictionary[GlobalConst.tblOperatorCluster] != null)
                    {
                        var be = GlobalData.BaseTableEntityDictionary[GlobalConst.tblOperatorCluster].Find(it => it.ID == value);
                        ClusterName = be != null ? be.Name : string.Empty;
                    }
                }
                idCluster = value; 

            }
        } 
        /// <summary>
        /// Display only
        /// </summary>
        public string ClusterName { get; set; }
        /// <summary>
        /// AccountingGroupID
        /// </summary>
        [RangeValidator(0, RangeBoundaryType.Inclusive, 10000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "AccountingGroupID_Type_Empty")]
        public int AccountingGroupID { get; set; }
        
        /// <summary>
        /// IdExclusivityLine
        /// </summary>
        [RangeValidator(0, RangeBoundaryType.Inclusive, 999999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "ExclusivityLine_Type_Empty")]
        public int IdExclusivityLine
        {
            get { return idExclusivityLine; }
            set 
            {
                if (value != idExclusivityLine || value==0)
                {
                    if (GlobalData.BaseTableEntityDictionary[GlobalConst.baseExclusivityLine] != null)
                    {
                        var be = GlobalData.BaseTableEntityDictionary[GlobalConst.baseExclusivityLine].Find(it => it.ID == value);
                        ExclusivityLineTypeName = be != null ? be.Name : string.Empty;
                    }
                }
                idExclusivityLine = value; 
            }
        }

        /// <summary>
        /// IdServiceType
        /// </summary>
        [RangeValidator(1, RangeBoundaryType.Inclusive, 999999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Service_Type_Empty")]
        public int IdServiceType
        {
            get { return idServiceType; }
            set
            {
                if (value != idServiceType)
                {
                    if (GlobalData.BaseTableEntityDictionary[GlobalConst.baseServiceType] != null)
                    {
                        var be = GlobalData.BaseTableEntityDictionary[GlobalConst.baseServiceType].Find(it => it.ID == value);
                        ServiceTypeName = be != null ? be.Name : string.Empty;
                    }
                }
                idServiceType = value;
            }
        }



        public string ServiceTypeName { get; set; }
        public string ExclusivityLineTypeName { get; set; }
        public string Company { get; set; }

        public decimal  RouteLen { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 99999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "MAKAT_0")]
        [NotNullValidator(MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "MAKAT_Empty")]
        public int? Catalog { get; set; }
        

        public short Hagdara { get; set; }
        
        //[RangeValidator(1, RangeBoundaryType.Inclusive, 500, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
        //    , MessageTemplateResourceName = "Seasonal_Empty")]
        public int  IdSeasonal { get; set; }

        //[RegexValidator(".+$", MessageTemplateResourceType = typeof(Resources.ValidationResource)
        //    , MessageTemplateResourceName = "RoadDescription_Empty")]
        [StringLengthValidator(1,32,MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "RoadDescriptionLength")]
        public string RoadDescription { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 9999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "RouteNumber_Empty")]
        //[NotNullValidator(MessageTemplateResourceType = typeof(Resources.ValidationResource)
        //    , MessageTemplateResourceName = "RouteNumber_0")]
        public int? RouteNumber { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 99999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Mahoz_Roshi_Empty")]
        public int IdZoneHead { get { return idZoneHead; } 
            set 
            {
                if (value != idZoneHead)
                {
                    if (!GlobalData.ClusterToZoneDictionary.IsDictionaryFull())
                        ZoneHeadName = string.Empty;
                    else
                    {
                        var clusterToZone = GlobalData.ClusterToZoneDictionary.ContainsKey(IdCluster) ? GlobalData.ClusterToZoneDictionary[IdCluster] : null;
                        if (clusterToZone != null)
                        {
                            var state = clusterToZone.ClusterStateList.FirstOrDefault(s => s.MainZoneId == value);
                            ZoneHeadName = state != null ? state.SubZoneName : string.Empty;
                        }
                        else
                            ZoneHeadName = string.Empty;
                    }
                }
                idZoneHead = value; 
            }
        }

        public string ZoneHeadName { get; set; }

        [RangeValidator(0, RangeBoundaryType.Inclusive, 9999999, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Mahoz_Mishne_Empty")]
        public int IdZoneSubHead
        {
            get { return idZoneSubHead; }
            set
            {
                
                    if (!GlobalData.ClusterToZoneDictionary.IsDictionaryFull())
                        ZoneSubHeadName = string.Empty;
                    else
                    {
                        var clusterToZone =GlobalData.ClusterToZoneDictionary.ContainsKey(IdCluster) ? GlobalData.ClusterToZoneDictionary[IdCluster] : null;
                        if (clusterToZone != null)
                        {
                            var state =
                                clusterToZone.ClusterStateList.FirstOrDefault(
                                    s => s.MainZoneId == IdZoneHead && s.SubZoneId == value);
                            ZoneSubHeadName = state != null ? state.SubZoneName : string.Empty;
                        }
                        else
                            ZoneSubHeadName = string.Empty;
                    }
                    idZoneSubHead = value;
            }
        }
        public string ZoneSubHeadName { get; set; }

        public bool Accessibility { get; set; }


        public int CatalogBackUp { get; set; }
         
        private string SetStringName(string delimitedValue,string baseTableName)
        {
            string[] arrayValues = delimitedValue.Split(ExportConfigurator.GetConfig().LogicalDataSplitDevider.ToCharArray());
            var sb = new StringBuilder();
            if (arrayValues.IsListFull())
            {
                foreach (string item in arrayValues)
                {
                    if (GlobalData.BaseTableEntityDictionary[baseTableName] != null)
                    {
                        var be = GlobalData.BaseTableEntityDictionary[baseTableName].Find(it => it.ID.ToString() == item);
                        if (be != null)
                        {
                            sb.Append(be.Name);
                            sb.Append(" ");
                        }
                    }
                }
            }
            return sb.ToString(); 
        }

       
        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        private RouteLine _routeLineBackUp;
        public RouteLine RouteLineBackUp
        {
            get { return _routeLineBackUp ?? (_routeLineBackUp = new RouteLine()); }
            set 
            {
                _routeLineBackUp = value;  
            }
        }





        #region IEditableObject Members

        public override void ArchiveObject()
        {
            RouteLineBackUp = new RouteLine
                                  {
                                      Signpost = this.Signpost,
                                      Dir = this.Dir,
                                      Var = this.Var,
                                      IdCluster = this.IdCluster,
                                      IdServiceType = this.IdServiceType,
                                      Catalog = this.Catalog,
                                      IdSeasonal = this.IdSeasonal,
                                      RoadDescription = this.RoadDescription,
                                      RouteNumber = this.RouteNumber,
                                      IdZoneHead = this.IdZoneHead,
                                      IdZoneSubHead = this.IdZoneSubHead
                                  };
        }
        /// <summary>
        /// Begin Edit Implementation
        /// </summary>
        protected override void BeginEditImplementation()
        {
                
        }
        /// <summary>
        /// Cancel Edit Implementation
        /// </summary>
        protected override void CancelEditImplementation()
        {
            
                this.Signpost = RouteLineBackUp.Signpost;
                this.Dir = RouteLineBackUp.Dir;
                this.Var = RouteLineBackUp.Var;
                this.IdCluster = RouteLineBackUp.IdCluster;
                this.IdServiceType = RouteLineBackUp.IdServiceType;
                this.Catalog = RouteLineBackUp.Catalog;
                this.IdSeasonal = RouteLineBackUp.IdSeasonal;
                this.RoadDescription = RouteLineBackUp.RoadDescription;
                this.RouteNumber = RouteLineBackUp.RouteNumber;
                this.IdZoneHead = RouteLineBackUp.IdZoneHead;
                this.IdZoneSubHead = RouteLineBackUp.IdZoneSubHead;
            
        }
        /// <summary>
        /// EndEditImplementation 
        /// </summary>
        protected override void EndEditImplementation()
        {
            //RouteLineBackUp = new RouteLine();
            
        }

        public bool IsBase { get; set; }
        public string  TextExportCatalog { get; set; }

        
        public bool ConfirmedForSaturday { get { return false; } } // Temporary

        #endregion
        #region ToString
        public override string ToString()
        {
            return string.Format("{0} / {1} / {2} / {3} / {4}",this.idCluster ,this.Catalog, this.Signpost, this.Dir, this.Var)  ;
        }
        #endregion
        #region Aux Prop
        public string StopPresentList { get; set; }
        #endregion


    }
}
