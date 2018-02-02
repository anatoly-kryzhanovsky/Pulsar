namespace StripController.Infrastructure.StripWrapper
{
    class ExecuteStripCommand : Command
    {
        public override byte[] GetBody()
        {
            return new byte[0];
        }

        public override int GetCommandId()
        {
            return 0;
        }
    }
}
