using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace WebApiAuthentication.Tests
{
    [TestFixture]
    public class BuildRequestSignatureTests
    {
        private Mock<IBuildMessageRepresentation> mockBuildMessageRepresentation;
        private Mock<ICalculateSignature> mockCalculateSignature;

        [SetUp]
        public void SetUp()
        {
            mockBuildMessageRepresentation = new Mock<IBuildMessageRepresentation>();
            mockCalculateSignature = new Mock<ICalculateSignature>();
        }
        [Test]
        public void throws_exception_if_secret_null()
        {
            var buildSignature = new BuildRequestSignature(mockBuildMessageRepresentation.Object, mockCalculateSignature.Object);
            Assert.Throws<ArgumentNullException>(() => buildSignature.Build(null, new HttpRequestMessage()));
        }

        [Test]
        public void builds_request_represenentation_and_returns_signature()
        {
            mockCalculateSignature.Setup(x => x.Calculate("secret", It.IsAny<string>()))
                .Returns("signature");

            var buildSignature = new BuildRequestSignature(mockBuildMessageRepresentation.Object, mockCalculateSignature.Object);

            var result = buildSignature.Build("secret", new HttpRequestMessage());

            mockBuildMessageRepresentation.Verify(x => x.Build(It.IsAny<HttpRequestMessage>()), Times.Once);

            Assert.That(result, Is.EqualTo("signature"));
        }
    }
}