using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Heimdall.Client.Windsor
{
    public class WindsorInstaller: IWindsorInstaller 
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IHeimdallClientFactory>().ImplementedBy<HeimdallClientFactory>());
        }
    }
}
