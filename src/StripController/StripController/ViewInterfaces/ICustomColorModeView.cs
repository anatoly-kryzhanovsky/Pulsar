using System;
using StripController.PresentationEntities;

namespace StripController.ViewInterfaces
{
    public delegate void ColorChangedDelegate(object sender, ColorChangedArgs args);
    public delegate void ApplyColorDelegate(object sender, EventArgs args);
    public delegate void BrightnessChangedDeletage(object sender, BrightnessChangedArgs args);

    public interface ICustomColorModeView: IView
    {
        CustomColorModePe DisplayObject { get; set; }

        event ColorChangedDelegate ColorChanged;
        event ApplyColorDelegate ApplyColor;
        event BrightnessChangedDeletage BrightnessChanged;
    }
}