using StripController.Infrastructure.StripWrapper;

namespace StripController.Services.Modes
{
    public interface IMode
    {
        IStripper Stripper { get; }

        void Start();
        void Stop();
    }
}

