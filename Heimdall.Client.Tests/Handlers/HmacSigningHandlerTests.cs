using System;
using System.Net.Http;
using Cachely.Tests.Framework;
using Heimdall.Client.Handlers;
using Heimdall.Interfaces;
using Moq;
using NUnit.Framework;

namespace Heimdall.Client.Tests.Handlers
{
    [TestFixture]
    public class HmacSigningHandlerTests
    {
        private HttpClient client;
        private HttpRequestMessage request;

        private Mock<IBuildRequestSignature> mockBuildRequestSignature;

        [SetUp]
        public void SetUp()
        {
            mockBuildRequestSignature = new Mock<IBuildRequestSignature>();

            mockBuildRequestSignature.Setup(x => x.Build("secret", It.IsAny<HttpRequestMessage>()))
                .Returns("signature");

            var handler = new HmacSigningHandler("secret", mockBuildRequestSignature.Object) { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);

            request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com") { Content = new StringContent("something") };
            request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);
        }

        [Test]
        public void builds_message_representation()
        {
            var result = client.SendAsync(request).Result;

            mockBuildRequestSignature.Verify(x => x.Build("secret", request));
        }

        [Test]
        public void adds_calculated_signature_to_headers()
        {
            var result = client.SendAsync(request).Result;

            var authHeader = request.Headers.Authorization;

            Assert.That(authHeader, Is.Not.Null);
            Assert.That(authHeader.Scheme, Is.EqualTo(HeaderNames.AuthenticationScheme));
            Assert.That(authHeader.Parameter, Is.EqualTo("signature"));
        }
    }
}