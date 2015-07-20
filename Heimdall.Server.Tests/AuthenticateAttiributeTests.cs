using System;
using System.Net;
using System.Net.Http;
using Heimdall.Server.Interfaces;
using Heimdall.Tests.Framework;
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
        public void throws_exception_if_authenticaterequest_not_set()
        {
            var actionContext = HttpActionContextBuilder
                .Instance()
                .Build();

            attribute.AuthenticateRequest = null;
            Assert.Throws<NullReferenceException>(() => attribute.OnActionExecuting(actionContext));
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
                .Returns(false);

            attribute.AuthenticateRequest = mockAuthenticateRequest.Object;

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
