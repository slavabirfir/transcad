using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Accessories
{
    public class ImportToSQLArgs : EventArgs
    {
        public int ProgressBarValue { get; set; }
        public int MaxProgressBarValue { get; set; }
        public string Message { get; set; }
    }
}
