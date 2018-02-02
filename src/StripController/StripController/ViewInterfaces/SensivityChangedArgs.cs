using System;

namespace StripController.ViewInterfaces
{
    public class SensivityChangedArgs : EventArgs
    {
        public double Value { get; set; }

        public SensivityChangedArgs(double value)
        {
            Value = value;
        }
    }
}