using Ninject;
using StripController.Infrastructure.ModuleConfig;

namespace StripController.Services.ModuleConfig
{
    class ApplicationKernel: StandardKernel
    {
        public ApplicationKernel()
            : base(
                new ApplicationServiceModules(),
                new InfrastructureModule())
        {
            
        }
    }
}
