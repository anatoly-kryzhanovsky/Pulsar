using System.Collections.Generic;
using StripController.PresentationEntities;

namespace StripController.Services.Modes
{
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs args);

    public interface IProgramMode : IMode
    {
        event TimeChangedEventHandler TimeChanged;
        
        void SetProgramItems(IEnumerable<ProgramItemPe> items);
        void SetInterpolate(bool interpolate);
    }
}