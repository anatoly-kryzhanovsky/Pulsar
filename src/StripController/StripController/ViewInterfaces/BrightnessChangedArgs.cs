using System;

namespace StripController.ViewInterfaces
{
    public class BrightnessChangedArgs:EventArgs
    {
        public byte Value { get; }

        public BrightnessChangedArgs(byte value)
        {
            Value = value;
        }
    }
}