using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApiAuthentication.Tests;

namespace WebApiAuthentication.Server.Tests
{
    [TestFixture]
    public class AuthenticateRequestTests
    {
        private Mock<IGetSecretFromUsername> mockGetSecretFromUsername;
        private AuthenticateRequest authenticateRequest;

        [SetUp]
        public void SetUp()
        {
            mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();

            authenticateRequest = new AuthenticateRequest(mockGetSecretFromUsername.Object);
        }

        [Test]
        public void returns_not_authorised_for_requests_without_username_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();

            var result = authenticateRequest.IsAuthenticated(request);

            Assert.That(result, Is.False);
        }

        [Test]
        public void returns_not_authorised_for_requests_without_authentication_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = null;

            var result = authenticateRequest.IsAuthenticated(request);
            Assert.That(result, Is.False);
        }

        [Test]
        public void returns_false_if_retrieved_secret_is_null()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns((string)null);

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");

            authenticateRequest = new AuthenticateRequest(mockGetSecretFromUsername.Object);

            var result = authenticateRequest.IsAuthenticated(request);
            Assert.That(result, Is.False);
        }

        [Test]
        public void returns_false_if_signatures_dont_match()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            authenticateRequest = new AuthenticateRequest(mockGetSecretFromUsername.Object);

            var result = authenticateRequest.IsAuthenticated(request);
            Assert.That(result, Is.False);
        }

        [Test]
        public void if_secret_is_not_returned_does_not_build_request_signature()
        {
            var mockBuildRequestSignature = new Mock<IBuildRequestSignature>();

            mockBuildRequestSignature.Setup(x => x.Build(It.IsAny<string>(), It.IsAny<HttpRequestMessage>()));

            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns((string)null);

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            authenticateRequest = new AuthenticateRequest(mockBuildRequestSignature.Object, mockGetSecretFromUsername.Object);

            var result = authenticateRequest.IsAuthenticated(request);

            mockBuildRequestSignature.Verify(x => x.Build(It.IsAny<string>(), It.IsAny<HttpRequestMessage>()), Times.Never());
        }

        [Test]
        public void returns_true_if_signatures_match()
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

            authenticateRequest = new AuthenticateRequest(mockBuildRequestSignature.Object, mockGetSecretFromUsername.Object);

            var result = authenticateRequest.IsAuthenticated(request);

            Assert.That(result, Is.True);
        }
    }
}