using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Heimdall.Tests;
using Moq;
using NUnit.Framework;

namespace Heimdall.Server.Tests
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

            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            authenticateRequest = new AuthenticateRequest(mockGetSecretFromUsername.Object);
        }

        [Test]
        public async Task returns_not_authorised_for_requests_without_username_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();

            var result = await authenticateRequest.IsAuthenticated(request);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task returns_not_authorised_for_requests_without_authentication_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = null;

            var result = await authenticateRequest.IsAuthenticated(request);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task returns_false_if_retrieved_secret_is_null()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns((string)null);

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");

            authenticateRequest = new AuthenticateRequest(mockGetSecretFromUsername.Object);

            var result = await authenticateRequest.IsAuthenticated(request);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task returns_false_if_signatures_dont_match()
        {
            var mockGetSecretFromUsername = new Mock<IGetSecretFromUsername>();
            mockGetSecretFromUsername.Setup(x => x.Secret(It.IsAny<string>()))
                .Returns("secret");

            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            authenticateRequest = new AuthenticateRequest(mockGetSecretFromUsername.Object);

            var result = await authenticateRequest.IsAuthenticated(request);
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

            authenticateRequest = new AuthenticateRequest(new HashCalculator(), mockBuildRequestSignature.Object, mockGetSecretFromUsername.Object);

            var result = authenticateRequest.IsAuthenticated(request);

            mockBuildRequestSignature.Verify(x => x.Build(It.IsAny<string>(), It.IsAny<HttpRequestMessage>()), Times.Never());
        }

        [Test]
        public async Task returns_true_if_signatures_match()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            var mockBuildRequestSignature = new Mock<IBuildRequestSignature>();
            mockBuildRequestSignature.Setup(x => x.Build("secret", request))
                .Returns("signature_hash");

            authenticateRequest = new AuthenticateRequest(new HashCalculator(), mockBuildRequestSignature.Object, mockGetSecretFromUsername.Object);

            var result = await authenticateRequest.IsAuthenticated(request);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task returns_false_if_request_md5_header_does_not_match_()
        {
            var wrongMd5 = Encoding.Default.GetBytes("wrong");

            var request = HttpRequestMessageBuilder.Instance()
                .WithContent(new StringContent("test"))
                .WithContentMD5(wrongMd5)
                .Build();

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, "signature_hash");
            request.Headers.Add(HeaderNames.UsernameHeader, "username");

            var result = await authenticateRequest.IsAuthenticated(request);

            Assert.That(result, Is.False);
        }

    }
}
