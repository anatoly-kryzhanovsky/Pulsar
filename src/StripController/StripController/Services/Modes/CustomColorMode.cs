using StripController.Infrastructure.StripWrapper;

namespace StripController.Services.Modes
{
    class CustomColorMode : ICustomColorMode
    {
        public IStripper Stripper { get; }

        public CustomColorMode(IStripper stripper)
        {
            Stripper = stripper;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void SetColor(byte r, byte g, byte b)
        {
            Stripper.SetPixelsColor(0, Stripper.PixelCount, r, g, b);
            Stripper.Apply();
        }

        public void SetBrigntness(byte value)
        {
            Stripper.SetBrightness(value);
            Stripper.Apply();
        }
    }
}