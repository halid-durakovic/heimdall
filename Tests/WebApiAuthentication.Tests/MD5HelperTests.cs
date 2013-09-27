using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebApiAuthentication.Tests
{
    [TestFixture]
    public class MD5HelperTests
    {
        [Test]
        public void returns_false_if_request_md5_header_does_not_match_content()
        {
            var wrongMd5 = Encoding.Default.GetBytes("wrong");

            var request = HttpRequestMessageBuilder.Instance()
                .WithContent(new StringContent("test"))
                .WithContentMD5(wrongMd5)
                .Build();

            var result = MD5Helper.IsMd5Valid(request).Result;

            Assert.That(result, Is.False);
        }

        [Test]
        public void returns_true_if_request_md5_header_matches_content_md5()
        {
            var request = HttpRequestMessageBuilder.Instance()
                .WithContent(new StringContent("test"))
                .Build();

            var result = MD5Helper.IsMd5Valid(request).Result;

            Assert.That(result, Is.True);
        }
    }
}
