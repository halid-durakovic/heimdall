using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WebApiHmac.Client.Tests
{
    [TestFixture]
    public class RequestContendMd5HeaderGeneratingHandlerTests
    {
        private RequestContendMd5HeaderGeneratingHandler handler;
        private HttpClient client;

        [SetUp]
        public void SetUp()
        {
            handler = new RequestContendMd5HeaderGeneratingHandler { InnerHandler = new TestHandler() };

            client = new HttpClient(handler);
        }

        [Test]
        public void generates_md5_header_and_saves_to_request()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com")
                                     {
                                         Content = new StringContent("something")
                                     };

            var expectedMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider()
                .ComputeHash(Encoding.UTF8.GetBytes("something"));

            var result = client.SendAsync(request)
                .Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var resultMd5 = request.Content
                .Headers
                .ContentMD5;

            Assert.IsTrue(resultMd5.SequenceEqual(expectedMd5));
        }
    }
}
