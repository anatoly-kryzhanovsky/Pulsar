using System.Drawing;

namespace StripController.Infrastructure.StripWrapper
{
    class SetStripStateCommand : Command
    {
        private readonly Color[] _colors;
        private readonly byte _brightness;
        
        public SetStripStateCommand(byte brightness, Color[] colors)
        {
            _brightness = brightness;
            _colors = colors;
        }

        public override byte[] GetBody()
        {
            var body = new byte[2 + _colors.Length * 3];
            body[0] = _brightness;
            body[1] = (byte)_colors.Length;

            for (int i = 0; i < _colors.Length; i++)
            {
                body[i * 3 + 2] = _colors[i].R;
                body[i * 3 + 2 + 1] = _colors[i].G;
                body[i * 3 + 2 + 2] = _colors[i].B;
            }
            
            return body;
        }

        public override int GetCommandId()
        {
            return Commands.SetStripStateCommandId;
        }
    }
}