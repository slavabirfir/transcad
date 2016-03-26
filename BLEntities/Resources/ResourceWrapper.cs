using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Resources
{
    public static class ResourceWrapper
    {
        public static String GetEmbarkOnly()
        {
            return Resources.ResourceEntities.Embarkation;
        }
    }
}
