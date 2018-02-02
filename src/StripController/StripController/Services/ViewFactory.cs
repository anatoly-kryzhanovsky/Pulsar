using Ninject;
using StripController.Views;
using Ninject.Parameters;
using StripController.Presenters;
using StripController.ViewInterfaces;

namespace StripController.Services
{
    class ViewFactory: IViewFactory
    {
        private readonly IKernel _kernel;

        public ViewFactory(IKernel kernel)
        {
            _kernel = kernel;
        }
        
        public ICustomColorModeView CreateCustomCoroModeView()
        {
            var view = new CustomColorModeView();
            _kernel.Get<CustomColorModePresenter>(new ConstructorArgument("view", view));
            
            return view;
        }

        public ICaptureModeView CreateCaptureColorModeView()
        {
            var view = new CaptureModeView();
            _kernel.Get<CaptureModePresenter>(new ConstructorArgument("view", view));

            return view;
        }

        public IProgramModeView CreateProgramModeView()
        {
            var view = new ProgramModeView();
            _kernel.Get<ProgramModePresenter>(new ConstructorArgument("view", view));

            return view;
        }

        public IMainWindow CreateMainWindow()
        {
            var wnd = new MainWindow();
            wnd.SetViewFactory(this);

            return wnd;
        }

        public IOpenFileDialog CreateOpenFileDialog()
        {
            return new OpenFileDialog();
        }

        public ISaveFileDialog CreateSaveFileDialog()
        {
            return new SaveFileDialog();
        }
    }
}
