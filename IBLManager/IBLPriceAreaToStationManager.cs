using System;
using System.Collections.Generic;
using BLEntities.Accessories;
using BLEntities.Entities;

namespace IBLManager
{
    public interface IBlPriceAreaToStationManager
    {
        string ErrorMessage { get;}
        string FilePath { get;}
        List<PriceZoneArea> PriceZoneAreas { get; }
        bool CheckDataValidity();
        bool ValidateInput();
        bool ImportInput();
        bool ExportToLicensingSystem();
        event EventHandler<MessageEventArgs> Changed;
    }

    

}
