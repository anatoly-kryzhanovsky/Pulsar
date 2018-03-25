using StripController.ViewInterfaces;

namespace StripController.Services
{
    public interface IViewFactory
    {
        IProgramModeView CreateProgramModeView();
        ICustomColorModeView CreateCustomCoroModeView();
        IVideoCaptureModeView CreateVideoCaptureColorModeView();
        IAudioCaptureModeView CreateCaptureColorModeView();
        IMainWindow CreateMainWindow();

        IOpenFileDialog CreateOpenFileDialog();
        ISaveFileDialog CreateSaveFileDialog();        
    }
}