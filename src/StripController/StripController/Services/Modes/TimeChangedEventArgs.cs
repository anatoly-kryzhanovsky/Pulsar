using System;

namespace StripController.Services.Modes
{
    public class TimeChangedEventArgs : EventArgs
    {
        public TimeSpan Time { get; }

        public TimeChangedEventArgs(TimeSpan time)
        {
            Time = time;
        }
    }
}