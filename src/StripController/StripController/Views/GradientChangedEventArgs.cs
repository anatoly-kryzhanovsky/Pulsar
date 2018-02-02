using System;
using System.Collections.Generic;
using StripController.PresentationEntities;

namespace StripController.Views
{
    public class GradientChangedEventArgs : EventArgs
    {
        public IEnumerable<GradientPointPe> Gradient { get; }

        public GradientChangedEventArgs(IEnumerable<GradientPointPe> gradient)
        {
            Gradient = gradient;
        }
    }
}