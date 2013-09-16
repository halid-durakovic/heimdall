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
        private HttpRequestMessage request;

        private Mock<IGetSecretFromKey> mockGetSecretFromKey;
        private Mock<IBuildMessageRepresentation> mockBuildMessageRepresentation;
        private Mock<ICalculateSignature> mockCalculateSignature;

        [SetUp]
        public void SetUp()
        {
            mockGetSecretFromKey = new Mock<IGetSecretFromKey>();
            mockBuildMessageRepresentation = new Mock<IBuildMessageRepresentation>();
            mockCalculateSignature = new Mock<ICalculateSignature>();

            mockGetSecretFromKey.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            mockBuildMessageRepresentation.Setup(x => x.Build(It.IsAny<HttpRequestMessage>()))
                .Returns("message");

            handler = new HmacSigningHandler("mykey123", mockGetSecretFromKey.Object, mockBuildMessageRepresentation.Object, mockCalculateSignature.Object) { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);

            request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com") { Content = new StringContent("something") };
            request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);
        }

        [Test]
        public void gets_secret_from_repository_based_on_passed_in_key()
        {
            var result = client.SendAsync(request).Result;

            mockGetSecretFromKey.Verify(x => x.Secret("mykey123"));
        }

        [Test]
        public void builds_message_representation()
        {
            var result = client.SendAsync(request).Result;

            mockBuildMessageRepresentation.Verify(x => x.Build(request));
        }

        [Test]
        public void adds_calculated_signature_to_headers()
        {
            mockCalculateSignature.Setup(x => x.Generate("secret", "message"))
                .Returns("signature");

            var result = client.SendAsync(request).Result;

            mockCalculateSignature.Verify(x => x.Generate("secret", "message"));

            var authHeader = request.Headers.Authorization;

            Assert.That(authHeader, Is.Not.Null);
            Assert.That(authHeader.Scheme, Is.EqualTo(HeaderNames.AuthenticationScheme));
            Assert.That(authHeader.Parameter, Is.EqualTo("signature"));
        }

        //[Test]
        //public void generates_signature_with_key()
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com")
        //    {
        //        Content = new StringContent("something")
        //    };

        //    request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);

        //    var result = client.SendAsync(request).Result;

        //    mockCa
        //    mockGetSecretFromKey.Verify(x => x.Secret("mykey123"));
        //}

    }
}