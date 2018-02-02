using System;

namespace StripController.ViewInterfaces
{
    public class SpectrumImageSizeChangedArgs : EventArgs
    {
        public int Width { get; }
        public int Height { get; }

        public SpectrumImageSizeChangedArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}