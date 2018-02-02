using System;

namespace StripController.ViewInterfaces
{
    public class CaptureStatusChangedArgs : EventArgs
    {
        public bool IsEnabled { get; set; }

        public CaptureStatusChangedArgs(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}