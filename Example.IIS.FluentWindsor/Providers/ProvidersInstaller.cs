using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Heimdall;

namespace Example.IIS.Providers
{
    public class ProvidersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IGetSecretFromUsername>().ImplementedBy<GetSecretFromUsername>());
        }
    }
}