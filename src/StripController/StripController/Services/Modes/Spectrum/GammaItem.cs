using System.Drawing;

namespace StripController.Services.Modes.Spectrum
{
    class GammaItem
    {
        public Color Color { get; }
        public double Value { get; }

        public GammaItem(Color color, double value)
        {
            Color = color;
            Value = value;
        }
    }
}