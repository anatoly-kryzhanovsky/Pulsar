namespace StripController.Infrastructure.StripWrapper
{
    class SetBrighnessCommand : Command
    {
        private readonly byte _value;

        public SetBrighnessCommand(byte value)
        {
            _value = value;
        }

        public override byte[] GetBody()
        {
            return new[] { _value };
        }

        public override int GetCommandId()
        {
            return Commands.SetBrighnessCommandId;
        }
    }
}
