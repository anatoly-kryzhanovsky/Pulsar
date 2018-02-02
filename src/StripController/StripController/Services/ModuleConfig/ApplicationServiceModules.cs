using Ninject;
using Ninject.Modules;
using StripController.Configuration.Interfaces;

namespace StripController.Services.ModuleConfig
{
    class ApplicationServiceModules : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewFactory>()
                .To<ViewFactory>()
                .InSingletonScope();

            Bind<Settings>()
                .ToSelf()
                .InSingletonScope();

            Bind<IStripperSettings>()
                .ToMethod(context => context.Kernel.Get<Settings>().StripperSettings)
                .InSingletonScope();

            Bind<ICaptureModeSettings>()
                .ToMethod(context => context.Kernel.Get<Settings>().CaptrureModeSettings)
                .InSingletonScope();

            Bind<ICustomColorModeSettings>()
                .ToMethod(context => context.Kernel.Get<Settings>().CustomColorModeSettings)
                .InSingletonScope();

            Bind<IProgramModeSettings>()
                .ToMethod(context => context.Kernel.Get<Settings>().ProgramModeSettings)
                .InSingletonScope();
        }
    }
}