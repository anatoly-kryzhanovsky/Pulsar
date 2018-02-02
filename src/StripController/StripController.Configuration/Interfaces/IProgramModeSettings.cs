using System.Collections.Generic;
using StripController.Configuration.Models;

namespace StripController.Configuration.Interfaces
{
    public interface IProgramModeSettings : IConfigSection
    {
        bool Interpolate { get; set; }
        IReadOnlyCollection<ProgramItem> ProgramItems { get; set; }
    }
}