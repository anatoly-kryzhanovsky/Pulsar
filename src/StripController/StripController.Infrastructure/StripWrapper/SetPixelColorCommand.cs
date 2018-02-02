namespace StripController.Infrastructure.StripWrapper
{
    class SetPixelColorCommand : Command
    {
        private readonly byte _pixel;
        private readonly byte _r;
        private readonly byte _g;
        private readonly byte _b;

        public SetPixelColorCommand(byte pixel, byte r, byte g, byte b)
        {
            _pixel = pixel;
            _r = r;
            _g = g;
            _b = b;
        }

        public override byte[] GetBody()
        {
            return new[] {_pixel, _r, _g, _b};
        }

        public override int GetCommandId()
        {
            return Commands.SetPixelColorCommandId;
        }
    }
}
