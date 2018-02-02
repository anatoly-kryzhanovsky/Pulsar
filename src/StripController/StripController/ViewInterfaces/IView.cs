using System;

namespace StripController.ViewInterfaces
{
    public interface IView
    {
        event EventHandler Activated;
        event EventHandler Deactivated;
        event EventHandler LoadStateRequested;
        event EventHandler SaveStateRequested;

        void Activate();
        void Deactivate();

        void LoadState();
        void SaveState();
    }
}