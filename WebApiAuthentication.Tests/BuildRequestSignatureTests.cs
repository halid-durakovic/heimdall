using System.Net.Http;
using Moq;
using NUnit.Framework;

namespace WebApiAuthentication.Tests
{
    [TestFixture]
    public class BuildRequestSignatureTests
    {
        [Test]
        public void builds_request_represenentation_and_returns_signature()
        {
            var mockBuildMessageRepresentation = new Mock<IBuildMessageRepresentation>();
            var mockCalculateSignature = new Mock<ICalculateSignature>();

            mockCalculateSignature.Setup(x => x.Calculate("secret", It.IsAny<string>()))
                .Returns("signature");

            var buildSignature = new BuildRequestSignature(mockBuildMessageRepresentation.Object, mockCalculateSignature.Object);

            var result = buildSignature.Build("secret", new HttpRequestMessage());

            mockBuildMessageRepresentation.Verify(x => x.Build(It.IsAny<HttpRequestMessage>()), Times.Once);

            Assert.That(result, Is.EqualTo("signature"));
        }
    }
}