namespace StripController.Infrastructure.StripWrapper
{
    class SetPixelColorBulkCommand : Command
    {
        private readonly byte _start;
        private readonly byte _end;
        private readonly byte _r;
        private readonly byte _g;
        private readonly byte _b;
        
        public SetPixelColorBulkCommand(byte start, byte end, byte r, byte g, byte b)
        {
            _start = start;
            _end = end;
            _r = r;
            _g = g;
            _b = b;
        }

        public override byte[] GetBody()
        {
            return new[] { _start, _end, _r, _g, _b };
        }

        public override int GetCommandId()
        {
            return Commands.SetPixelColorBulkCommandId;
        }
    }
}