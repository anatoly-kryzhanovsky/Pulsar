using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StripController.Services.Modes
{
    public class SpectrumUpdatedEventArgs : EventArgs
    {
        public IReadOnlyCollection<Color> Colors { get; }
        public IReadOnlyCollection<double> Values { get; }
        public double Brightness { get; }

        public SpectrumUpdatedEventArgs(IEnumerable<double> value, IEnumerable<Color> colors, double brightness)
        {
            Values = value.ToArray();
            Colors = colors.ToArray();
            Brightness = brightness;
        }
    }
}