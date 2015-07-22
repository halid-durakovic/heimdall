using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Heimdall.Tests;
using Moq;
using NUnit.Framework;

namespace Heimdall.Server.Tests
{
    [TestFixture]
    public class AuthenticateAttiributeTests
    {
        private Mock<IAuthenticateRequest> mockAuthenticateRequest;

        private AuthenticateAttiribute attribute;

        [SetUp]
        public void SetUp()
        {
            mockAuthenticateRequest = new Mock<IAuthenticateRequest>();

            attribute = new AuthenticateAttiribute
                        {
                            AuthenticateRequest = mockAuthenticateRequest.Object
                        };
        }

        [Test]
        public void returns_unauthorized_if_authenticaterequest_is_false()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = null;

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            mockAuthenticateRequest.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>()))
                .Returns(Task.FromResult(false));

            attribute.AuthenticateRequest = mockAuthenticateRequest.Object;

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
