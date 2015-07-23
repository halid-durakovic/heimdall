using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using Heimdall.Server.Tests.Framework.Factories;
using NUnit.Framework;

namespace Heimdall.Server.Tests
{
    [TestFixture]
    public class HmacAuthenticationHandlerTests
    {
        private HttpSelfHostServer server;

        [TestFixtureSetUp]
        public void SetUp()
        {
            this.server = ServerFactory.CreateServer("http://localhost:8080");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            this.server.Dispose();
        }

        [Test]
        public async Task should_return_401_for_unsigned_client()
        {
            var unsignedClient = ClientFactory.CreateUnsignedClient();
            var result = await unsignedClient.GetAsync("http://localhost:8080/api/values");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task should_return_200_for_signed_client()
        {
            var signedClient = ClientFactory.CreateSignedClient();
            var result = await signedClient.GetAsync("http://localhost:8080/api/values");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task should_return_200_for_unsigned_client_with_allowany_attribute()
        {
            HeimdallConfig.AllowPath("/api/any");
            var unsignedClient = ClientFactory.CreateUnsignedClient();
            var result = await unsignedClient.GetAsync("http://localhost:8080/api/any");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
