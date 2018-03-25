namespace StripController.Services.Modes
{
    public delegate void VideoDataUpdatedEventHandler(object sender, VideoUpdatedEventArgs args);

    public interface IVideoCaptureMode : IMode
    {
        event VideoDataUpdatedEventHandler VideoUpdated;     
    }
}
