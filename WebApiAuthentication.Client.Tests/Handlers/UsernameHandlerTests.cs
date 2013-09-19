using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using WebApiAuthentication.Client.Handlers;
using WebApiAuthentication.Tests;
using WebApiAuthentication.Tests.Framework;

namespace WebApiAuthentication.Client.Tests.Handlers
{
    [TestFixture]
    public class UsernameHandlerTests
    {
        private UsernameHandler handler;
        private HttpClient client;

        [SetUp]
        public void SetUp()
        {
            handler = new UsernameHandler("mykey123") { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);
        }

        [Test]
        public void sets_username_header()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com");

            var result = client.SendAsync(request)
                .Result;

            Assert.That(request.Headers.GetValues(HeaderNames.UsernameHeader).FirstOrDefault(), Is.Not.Null);
            Assert.That(request.Headers.GetValues(HeaderNames.UsernameHeader).First(), Is.EqualTo("mykey123"));
        }

        [Test]
        public void does_not_set_username_header_if_already_set()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com");
            request.Headers.Add(HeaderNames.UsernameHeader, "otherkey123");

            var result = client.SendAsync(request)
                .Result;

            Assert.That(request.Headers.GetValues(HeaderNames.UsernameHeader).First(), Is.EqualTo("otherkey123"));
        }
    }
}
