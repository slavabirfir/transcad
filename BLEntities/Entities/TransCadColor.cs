using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Entities
{
    public class TransCadColor
    {
        public TransCadColor GetTranscadConvertedObject()
        {
            TransCadColor tc = new TransCadColor();
            tc.Blue = coeffitient * this.Blue;
            tc.Grean = coeffitient * this.Grean;
            tc.Red = coeffitient * this.Red;
            return tc;
        }
        private int coeffitient = 256;
        public int Red { get; set; }
        public int Grean { get; set; }
        public int Blue { get; set; }
    }
}
