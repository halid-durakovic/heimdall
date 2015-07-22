using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Heimdall.Tests
{
    [TestFixture]
    public class MD5HelperTests
    {
        private HashCalculator hashCalculator;

        [SetUp]
        public void SetUp()
        {
            hashCalculator = new HashCalculator();
        }

        [Test]
        public async Task returns_false_if_request_md5_header_does_not_match_content()
        {
            var wrongMd5 = Encoding.Default.GetBytes("wrong");

            var request = HttpRequestMessageBuilder.Instance()
                .WithContent(new StringContent("test"))
                .WithContentMD5(wrongMd5)
                .Build();

            var result = await hashCalculator.IsValidHash(request);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task returns_true_if_request_md5_header_matches_content_md5()
        {
            var request = HttpRequestMessageBuilder.Instance()
                .WithContent(new StringContent("test"))
                .Build();

            var result = await hashCalculator.IsValidHash(request);

            Assert.That(result, Is.True);
        }
    }
}
