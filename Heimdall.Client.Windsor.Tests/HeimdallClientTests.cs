using Castle.Windsor;
using NUnit.Framework;

namespace Heimdall.Client.Windsor.Tests
{
    [TestFixture]
    public class HeimdallClientTests
    {
        [Test]
        public void Should_be_able_to_resolve_client_factory()
        {
            var container = new WindsorContainer();
            container.Install(new WindsorInstaller());

            Assert.That(container.Resolve<IHeimdallClientFactory>(), Is.Not.Null);
        }
    }
}
