using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using WebApiAuthentication.Tests;
using WebApiAuthentication.Tests.Framework;

namespace WebApiAuthentication.Server.Tests
{
    [TestFixture]
    public class HmacAuthenticationHandlerTests
    {
        private HttpClient client;
        private HmacAuthenticationHandler handler;
        private Mock<IAuthenticateRequest> mockAuthenticateRequest;

        [SetUp]
        public void SetUp()
        {
            mockAuthenticateRequest = new Mock<IAuthenticateRequest>();
            handler = new HmacAuthenticationHandler(mockAuthenticateRequest.Object)
                      {
                          InnerHandler = new TestHandler()
                      };
            client = new HttpClient(handler);
        }

        [Test]
        public void returns_not_authorised_if_request_is_not_authorised()
        {
            mockAuthenticateRequest.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>()))
                .Returns(false);

            var request = HttpRequestMessageBuilder.Instance().Build();

            var result = client.SendAsync(request).Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
