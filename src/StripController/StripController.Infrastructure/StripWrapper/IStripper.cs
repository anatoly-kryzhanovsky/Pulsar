using System.Drawing;

namespace StripController.Infrastructure.StripWrapper
{
    public interface IStripper
    {
        byte PixelCount { get; }

        void SetPixelColor(byte pixel, byte r, byte g, byte b);
        void SetPixelsColor(byte start, byte end, byte r, byte g, byte b);
        void SetBrightness(byte value);
        void SetPixelsColor(byte brightness, Color[] colors);
        void Apply();

        void Start();
        void Stop();
    }
}
