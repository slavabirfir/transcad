using System.Collections.Generic;
using BLEntities.Accessories;

namespace IBLManager
{
    public interface IPresentationRouteMainForm
    {
        bool IsVisibleManagerPart { get; }

        List<TransCadField> GetRouteLinesFields();
        List<TransCadField> GetRouteStopsFields();

        void ExitFormApplication();
    }
}
