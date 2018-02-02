using System;
using StripController.PresentationEntities;

namespace StripController.ViewInterfaces
{
    public delegate void DeleteItemRequestedDelegate(object sender, DeleteItemRequestedAgrs args);
    
    public interface IProgramModeView : IView
    {
        ProgramModePe DisplayObject { get; set; }

        event DeleteItemRequestedDelegate DeleteItemRequested;
        event EventHandler CreateItemRequested;
        event EventHandler PlayRequested;
        event EventHandler StopRequested;
        event EventHandler SaveProgramRequested;
        event EventHandler LoadProgramRequested;
        event EventHandler ResetProgramRequested;
    }
}