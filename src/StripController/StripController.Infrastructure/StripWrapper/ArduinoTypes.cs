namespace StripController.Infrastructure.StripWrapper
{
    static class ArduinoTypes
    {
        public const int IntSize = 2;
        public const int ByteSize = 1;

        public static byte[] GetBytes(int value)
        {
            var result = new byte[IntSize];
            result[0] = (byte)((value >> 8) & 0xFF);
            result[1] = (byte)(value & 0xFF);

            return result;
        }

        public static byte[] GetBytes(byte value)
        {
            var result = new byte[ByteSize];
            result[0] = value;

            return result;
        }
    }
}
