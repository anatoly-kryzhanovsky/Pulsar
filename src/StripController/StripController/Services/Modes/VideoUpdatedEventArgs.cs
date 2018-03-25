using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace StripController.Services.Modes
{
    public class VideoUpdatedEventArgs : EventArgs
    {
        public IReadOnlyCollection<Color> Colors { get; }
        
        public VideoUpdatedEventArgs(IEnumerable<Color> colors)
        {            
            Colors = colors.ToArray();        
        }
    }
}
