using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Heimdall.Server.Windsor
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new DeferredResolutionHmacAuthenticationHandler());
        }
    }
}
