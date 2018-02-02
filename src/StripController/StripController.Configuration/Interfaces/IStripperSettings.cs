namespace StripController.Configuration.Interfaces
{
    public interface IStripperSettings
    {
        string Address { get; }
        int Port { get; }
        byte PixelCount { get; }
    }
}
