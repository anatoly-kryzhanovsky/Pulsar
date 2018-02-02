using StripController.ViewInterfaces;

namespace StripController.Services
{
    public interface IViewFactory
    {
        IProgramModeView CreateProgramModeView();
        ICustomColorModeView CreateCustomCoroModeView();
        ICaptureModeView CreateCaptureColorModeView();
        IMainWindow CreateMainWindow();

        IOpenFileDialog CreateOpenFileDialog();
        ISaveFileDialog CreateSaveFileDialog();
    }
}