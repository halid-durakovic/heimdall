using System;
using System.Net.Http;
using Moq;
using NUnit.Framework;
using WebApiAuthentication.Client.Handlers;

namespace WebApiAuthentication.Client.Tests.Handlers
{
    [TestFixture]
    public class HmacSigningHandlerTests
    {
        private HmacSigningHandler handler;
        private HttpClient client;

        private Mock<IGetSecretFromKey> mockGetSecretFromKey;

        [SetUp]
        public void SetUp()
        {
            mockGetSecretFromKey = new Mock<IGetSecretFromKey>();

            handler = new HmacSigningHandler("mykey123", mockGetSecretFromKey.Object) { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);
        }

        [Test]
        public void takes_key_in_ctor_and_sets_to_property()
        {
            var handler = new HmacSigningHandler("key123", mockGetSecretFromKey.Object);

            Assert.That(handler.SigningKey, Is.EqualTo("key123"));
        }

        [Test]
        public void gets_secret_from_repository()
        {
            mockGetSecretFromKey.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com")
            {
                Content = new StringContent("something")
            };

            request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);

            var result = client.SendAsync(request).Result;

            mockGetSecretFromKey.Verify(x => x.Secret("mykey123"));
        }
    }
}