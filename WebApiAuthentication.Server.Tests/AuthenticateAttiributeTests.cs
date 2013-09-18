using System.Net;
using System.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using WebApiAuthentication.Tests;

namespace WebApiAuthentication.Server.Tests
{
    [TestFixture]
    public class AuthenticateAttiributeTests
    {
        private AuthenticateAttiribute attribute;

        [SetUp]
        public void SetUp()
        {
            attribute = new AuthenticateAttiribute();
        }

        [Test]
        public void returns_not_authorised_for_requests_without_username_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void returns_not_authorised_for_requests_without_authentication_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = null;

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void returns_not_authorised_if_retrieved_secret_is_null()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns((string)null);

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            attribute.GetSecretFromUsername = mockGetSecretFromUsername.Object;
            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void returns_not_authorised_if_signatures_dont_match()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            attribute.GetSecretFromUsername = mockGetSecretFromUsername.Object;

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void passes_through_if_signatures_match()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            var mockBuildRequestSignature = new Mock<IBuildRequestSignature>();
            mockBuildRequestSignature.Setup(x => x.Build("secret", request))
                .Returns("signature_hash");

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            attribute.GetSecretFromUsername = mockGetSecretFromUsername.Object;
            attribute.BuildRequestSignature = mockBuildRequestSignature.Object;

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Null); //for purposes of this test, for now
        }
    }
}
