namespace StripController.Services.Modes
{
    public interface ICustomColorMode : IMode
    {
        void SetColor(byte r, byte g, byte b);
        void SetBrigntness(byte value);
    }
}