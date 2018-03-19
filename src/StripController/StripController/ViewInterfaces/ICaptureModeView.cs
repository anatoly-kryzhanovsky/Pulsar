using System;
using StripController.PresentationEntities;
using StripController.Views;

namespace StripController.ViewInterfaces
{
    public delegate void SensivityChangedDelegate(object sender, SensivityChangedArgs args);
    public delegate void CaptureStatusChangedDelegate(object sender, CaptureStatusChangedArgs args);
    public delegate void SpectrumImageSizeChangedDelegate(object sender, SpectrumImageSizeChangedArgs args);

    public interface IAudioCaptureModeView : IView
    {
        CaptureColorModePe DisplayObject { get; set; }
        
        event SensivityChangedDelegate SensivityChanged;
        event CaptureStatusChangedDelegate CaptureStatusChanged;
        event GradientChangedDelegate GradientChanged;
        event SpectrumImageSizeChangedDelegate SpectrumImageSizeChanged;
        event EventHandler SaveGradientRequested;
        event EventHandler LoadGradientRequested;

        void UpdateVisual();
        void SetGradient(GradientPointPe[] gradient);
    }

  
}