using System;
using System.Threading;
using System.Threading.Tasks;
using StripController.Infrastructure.StripWrapper;
using StripController.Services;
using StripController.Services.Modes;
using StripController.ViewInterfaces;

namespace StripController.Presenters
{
    class VideoCaptureModePresenter
    {
        private readonly IVideoCaptureModeView _view;
        private readonly IViewFactory _viewFactory;
        private readonly TaskScheduler _uiScheduler;
        
        private readonly IVideoCaptureMode _mode;
        
        public VideoCaptureModePresenter(
            IVideoCaptureModeView view,
            IViewFactory viewFactory,        
            IStripper stripper)
        {
            _view = view;
            _viewFactory = viewFactory;
                                    
            _view.Activated += ViewOnActivated;
            _view.Deactivated += ViewOnDeactivated;
            
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _mode = new VideoCaptureMode(stripper);
            _mode.VideoUpdated += ModeOnVideoUpdated;
        }

        private void ModeOnVideoUpdated(object sender, VideoUpdatedEventArgs args)
        {
            Task.Factory.StartNew(() =>
                {
                    _view.UpdateVisual(args.Colors);
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                _uiScheduler);
        }

        private void ViewOnDeactivated(object sender, EventArgs eventArgs)
        {
            _mode.Stop();
        }

        private void ViewOnActivated(object sender, EventArgs eventArgs)
        {
            _mode.Start();
        }        
    }
}