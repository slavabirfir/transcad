using System;

namespace BLEntities.Accessories
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
        public int ProgressBarValue { get; set; }
        public int MaxProgressBarValue { get; set; }
    }
}
