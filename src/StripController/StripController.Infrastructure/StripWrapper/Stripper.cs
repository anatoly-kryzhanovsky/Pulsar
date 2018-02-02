using System.Drawing;

namespace StripController.Infrastructure.StripWrapper
{
    class Stripper : IStripper
    {
        private readonly NetworkManager _networkManager;
        private readonly string _address;
        private readonly int _port;

        public byte PixelCount { get; }

        public Stripper(string address, int port, byte pixelCount)
        {
            PixelCount = pixelCount;

            _address = address;
            _port = port;
            _networkManager = new NetworkManager();
        }

        public void Start()
        {
            _networkManager.Start(_address, _port);
        }

        public void Stop()
        {
            _networkManager.Stop();
        }

        public void SetPixelColor(byte pixel, byte r, byte g, byte b)
        {
            _networkManager.EnqueueCommand(new SetPixelColorCommand(pixel, r, g, b));
        }

        public void SetPixelsColor(byte start, byte end, byte r, byte g, byte b)
        {
            _networkManager.EnqueueCommand(new SetPixelColorBulkCommand(start, end, r, g, b));
        }

        public void SetBrightness(byte value)
        {
            _networkManager.EnqueueCommand(new SetBrighnessCommand(value));
        }

        public void SetPixelsColor(byte brightness, Color[] colors)
        {
            _networkManager.EnqueueCommand(new SetStripStateCommand(brightness, colors));
        }

        public void Apply()
        {
            _networkManager.EnqueueCommand(new ExecuteStripCommand());
        }
    }
}
