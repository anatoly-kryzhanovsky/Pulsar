using Ninject;
using Ninject.Modules;
using StripController.Configuration.Interfaces;
using StripController.Infrastructure.StripWrapper;

namespace StripController.Infrastructure.ModuleConfig
{
    public class InfrastructureModule: NinjectModule
    {
        public override void Load()
        {
            Bind<IStripper>()
                .To<Stripper>()
                .InSingletonScope()
                .WithConstructorArgument("address", context => context.Kernel.Get<IStripperSettings>().Address)
                .WithConstructorArgument("port", context => context.Kernel.Get<IStripperSettings>().Port)
                .WithConstructorArgument("pixelCount", context => context.Kernel.Get<IStripperSettings>().PixelCount)
                .OnActivation(stripper => stripper.Start())
                .OnDeactivation(stripper => stripper.Stop());
        }
    }
}
