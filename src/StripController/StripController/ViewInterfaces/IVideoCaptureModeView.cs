using System.Collections.Generic;
using System.Windows.Media;

namespace StripController.ViewInterfaces
{
    public interface IVideoCaptureModeView : IView
    {
        void UpdateVisual(IReadOnlyCollection<Color> colors);
    }
}
