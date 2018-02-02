using System;

namespace StripController.Configuration.Models
{
    public class ProgramItem
    {
        public TimeSpan Time { get; set; }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public byte StartPixel { get; set; }
        public byte EndPixel { get; set; }

        public byte Brightness { get; set; }

        public EProgramItemType Type { get; set; }
    }
}