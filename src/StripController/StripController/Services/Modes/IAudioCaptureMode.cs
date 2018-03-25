using System.Collections.Generic;
using StripController.PresentationEntities;

namespace StripController.Services.Modes
{
    public delegate void SpectrumUpdatedEventHandler(object sender, SpectrumUpdatedEventArgs args);

    public interface IAudioCaptureMode : IMode
    {
        event SpectrumUpdatedEventHandler SpectrumUpdated;
        
        void SetGradient(IEnumerable<GradientPointPe> gradient);
        void SetSensivite(double value);
    }
}