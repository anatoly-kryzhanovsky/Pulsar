using System.Windows;
using Ninject;
using StripController.Infrastructure.StripWrapper;
using StripController.Services;
using StripController.Services.ModuleConfig;

namespace StripController
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var kernel = new ApplicationKernel();
            var viewFactory = kernel.Get<IViewFactory>();
            var stripper = kernel.Get<IStripper>();
            var settings = kernel.Get<Settings>();

            var mainWindow = viewFactory.CreateMainWindow();
            mainWindow.ShowDialog();
            stripper.Stop();

            settings.Save();
        }
    }
}
