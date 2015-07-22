using System.Linq;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace Heimdall.Server.Windsor.Tests
{
    [TestFixture]
    public class HeimdallServerTests
    {
        [Test]
        public void Should_install_deferred_delegating_handler()
        {
            var container = new WindsorContainer();
            container.Install(new WindsorInstaller());

            Assert.That(GlobalConfiguration.Configuration.MessageHandlers.OfType<DeferredResolutionHmacAuthenticationHandler>().Count(), Is.GreaterThan(0));
        }
    }
}
