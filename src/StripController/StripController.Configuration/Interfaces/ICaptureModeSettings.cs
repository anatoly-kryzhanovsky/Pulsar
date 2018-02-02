using System.Collections.Generic;
using StripController.Configuration.Models;

namespace StripController.Configuration.Interfaces
{
    public interface ICaptureModeSettings: IConfigSection
    {
        double Sensivity { get; set; }

        IReadOnlyCollection<GradientItem> GradientItems { get; set; }
    }
}