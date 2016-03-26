using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using BLEntities.Model;
using Utilities;

namespace BLEntities.Entities
{
    [HasSelfValidation]
    public class RouteStop : BaseClass
    {
        private int? _idStationType;


        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Longitude_0")]
        public int Longitude { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Latitude_0")]
        public int Latitude { get; set; }

        [NotNullValidator (MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "RouteLine_Foreign_Not_Found")]
        public RouteLine RouteLine { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Route_ID_0")]
        public int RouteId { get; set; }

        [RangeValidator(0d, RangeBoundaryType.Inclusive, 1000000d, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Milepost_0")]
        public double Milepost { get; set; }

        public int MilepostRounded { get; set; }
        public int MilepostFromOriginStation { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Physical_Stop_ID_0")]
        public int PhysicalStopId { get; set; }

        [NotNullValidator(MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Physical_Stop_ID_Foreign_Not_Found")]
        public PhysicalStop PhysicalStop { get; set; }


        [RangeValidator(1, RangeBoundaryType.Inclusive, 5, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "IdStationType_0")]
        public int? IdStationType
        {
            get
            {
                return _idStationType;
            }
            set
            {
                if (_idStationType != value && (value == 2 || value==4))
                {
                    StationNameHorada = string.Empty;
                    StationCatalogHorada = string.Empty;
                }
                _idStationType = value; 
            }
        }

        public string StationTypeName { get; set; }
        
        public int Ordinal{ get; set; }

        private int? _horada;
        public int?   Horada	 	{ 
            get
            {
                return _horada;
            }
            set 
            {
                if (_horada != value && _horada.HasValue && _horada>0)
                {
                    List<RouteStop> horadaList = GlobalData.RouteModel.RouteStopList.FindAll(rs => rs.RouteId == RouteId).OrderBy(rs => rs.Milepost).ToList<RouteStop>();
                    RouteStop horadaRouteStop = horadaList.FindLast(rsInner => rsInner.Ordinal == value);
                    if (horadaRouteStop != null)
                    {
                        StationCatalogHorada = horadaRouteStop.StationCatalog;
                        string name = string.Empty;
                        if (GlobalData.RouteModel.PhysicalStopList.IsListFull())
                        {
                            PhysicalStop ps = GlobalData.RouteModel.PhysicalStopList.FindLast(psInner => psInner.ID == PhysicalStopId);
                            if (ps != null)
                                name = ps.Name;
                        }
                        StationNameHorada = name;
                    }
                }
                _horada = value;
            }
        }
        
        public string StationCatalogHorada { get; set; }
        public string StationNameHorada { get; set; }

       [RegexValidator(".+$", MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "StationCatalog_Empty")]
        public string StationCatalog { get; set; }

        #region IEditableObject Members
        private RouteStop _routeStopBackUp;
        public RouteStop RouteStopBackUp
        {
            get { return _routeStopBackUp ?? (_routeStopBackUp = new RouteStop()); }
            set
            {
                _routeStopBackUp = value;
            }
        }

        /// <summary>
        /// Begin Edit Implementation
        /// </summary>
        protected override void BeginEditImplementation()
        {
            RouteStopBackUp.IdStationType = IdStationType;
            RouteStopBackUp.Horada = Horada;
            RouteStopBackUp.IsSelected = IsSelected;
        }

        /// <summary>
        /// Cancel Edit Implementation
        /// </summary>
        protected override void CancelEditImplementation()
        {
            IdStationType = RouteStopBackUp.IdStationType;
            Horada = RouteStopBackUp.Horada;
            IsSelected = RouteStopBackUp.IsSelected;
        }
        /// <summary>
        /// EndEditImplementation 
        /// </summary>
        protected override void EndEditImplementation()
        {
            RouteStopBackUp = new RouteStop();
        }


        /// <summary>
        /// For inner needs
        /// </summary>
        public bool IsSelected { get; set; }


        public float Duration { get; set; }  
        public int LinkId { get; set; }
        
        public override BaseClassStyle BaseClassStyle
        {
            get
            {
                if (GlobalData.LoginUser.UserOperator.OperatorGroup == EnmOperatorGroup.PlanningFirm)
                {
                    if (this.PhysicalStop != null && this.PhysicalStop.IdStationStatus == 2)
                        return new BaseClassStyle { ForeColor = "Blue" };
                }
                return new BaseClassStyle();
            }
        }
        
        public string Platform { get; set; }
        #endregion
        
    }
}
