using NUnit.Framework;

namespace Heimdall.Tests
{
    [TestFixture]
    public class CalculateSignatureTests
    {
        private CalculateSignature calculateSignature;

        [SetUp]
        public void SetUp()
        {
            calculateSignature = new CalculateSignature();
        }

        [Test]
        public void calculates_signature_with_key()
        {
            var result = calculateSignature.Calculate("mykey", "this_is_a_test");

            Assert.That(result, Is.EqualTo("yrSldFqLV03RdqzEkmYQuqE8ZqHyxNSMOckMqPRNMuo="));
        }
    }
}