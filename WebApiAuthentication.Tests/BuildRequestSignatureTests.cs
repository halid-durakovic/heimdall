using Moq;
using NUnit.Framework;

namespace WebApiAuthentication.Tests
{
    [TestFixture]
    public class BuildRequestSignatureTests
    {
        [Test]
        public void returns_signature()
        {
            var mockBuildMessageRepresentation = new Mock<IBuildMessageRepresentation>();
            var mockCalculateSignature = new Mock<ICalculateSignature>();

            mockCalculateSignature.Setup(x => x.Calculate("secret", It.IsAny<string>()))
                .Returns("signature");

            var build = new BuildRequestSignature(mockBuildMessageRepresentation.Object, mockCalculateSignature.Object);

            Assert.That(build, Is.EqualTo("signature"));
        }
    }
}