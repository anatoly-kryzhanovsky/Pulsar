using System;

namespace StripController.ViewInterfaces
{
    public class ColorChangedArgs: EventArgs
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public ColorChangedArgs(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}