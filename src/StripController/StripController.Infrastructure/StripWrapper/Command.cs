namespace StripController.Infrastructure.StripWrapper
{
    abstract class Command
    {
        public abstract byte[] GetBody();
        public abstract int GetCommandId();
    }
}
