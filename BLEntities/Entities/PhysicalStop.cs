using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using BLEntities.Model;

namespace BLEntities.Entities
{
    public class PhysicalStop : BaseClass
    {
        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Longitude_0")]
        public int Longitude { get; set; }
        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Latitude_0")]
        public int Latitude { get; set; }

        [DomainValidator("+", "-",MessageTemplateResourceType = typeof(Resources.ValidationResource)
           , MessageTemplateResourceName = "PhysicalStop_Direction_NOT_IN_Domain")]
        public string  Direction { get; set; }
        
        public string StationCatalog { get; set; }

        public int LinkId { get; set; }

        public int? FromPlatform { get; set; }
        public int? ToPlatform { get; set; }

        //public bool IsCityLinkedManual { get; set; }


        public short? Floor { get; set; }

        public int LinkUserId { get; set; }
        public string  Street { get; set; }
        public int House { get; set; }
        public byte Across { get; set; }
        public string  CityCode { get; set; }
        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Longitude_0_N")]
        public int LongitudeN { get; set; }
        [RangeValidator(1, RangeBoundaryType.Inclusive, 100000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
           , MessageTemplateResourceName = "Latitude_0_N")]
        public int LatitudeN { get; set; }

        public PriceArea PriceArea { get; set; }

        public PriceZoneArea PriceZoneArea { get; set; }
        
        public string StationShortName { get; set; }
        public bool IsTrainMainStation { get; set; }
        private int _idStationStatus;
        public String  StationStatus { get; set; }
        [RangeValidator(1, RangeBoundaryType.Inclusive, 5, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "StationStatus_0")]
        public int IdStationStatus
        {
            get
            {
                return _idStationStatus;
            }

            set
            {
                if (value != _idStationStatus)
                {
                    if (GlobalData.BaseTableEntityDictionary[GlobalConst.baseStationStatus] != null)
                    {
                        BaseTableEntity be = GlobalData.BaseTableEntityDictionary[GlobalConst.baseStationStatus].Find(it => it.ID == value);
                        StationStatus = be != null ? be.Name : string.Empty;
                    }
                }
                _idStationStatus = value;

            }
        }


        public enmLinkStationCity CityLinkCode { get; set; }

        public int? AreaOperatorId { get; set; }

        private int _idStationType;
        public String StationType { get; set; }
        [RangeValidator(1, RangeBoundaryType.Inclusive, 12, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "StationType_0")]
        public int IdStationType
        {
            get
            {
                return _idStationType;
            }

            set
            {
                if (value != _idStationType)
                {
                    if (GlobalData.BaseTableEntityDictionary[GlobalConst.baseStationTipus] != null)
                    {
                        BaseTableEntity be = GlobalData.BaseTableEntityDictionary[GlobalConst.baseStationTipus].Find(it => it.ID == value);
                        StationType = be != null ? be.Name : string.Empty;
                    }
                }
                _idStationType = value;

            }
        } 
    }
}
