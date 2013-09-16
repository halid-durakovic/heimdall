using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WebApiAuthentication.Client.Handlers;

namespace WebApiAuthentication.Client.Tests.Handlers
{
    [TestFixture]
    public class ContentMD5HeaderHandlerTests
    {
        private ContentMD5HeaderHandler handler;
        private HttpClient client;

        [SetUp]
        public void SetUp()
        {
            handler = new ContentMD5HeaderHandler { InnerHandler = new TestHandler() };

            client = new HttpClient(handler);
        }

        [Test]
        public void generates_md5_header_and_saves_to_request()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com")
                                     {
                                         Content = new StringContent("something")
                                     };

            var expectedMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider()
                .ComputeHash(Encoding.UTF8.GetBytes("something"));

            var result = client.SendAsync(request)
                .Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var resultMD5 = request.Content
                .Headers
                .ContentMD5;

            Assert.IsTrue(resultMD5.SequenceEqual(expectedMD5));
        }

        [Test]
        public void does_not_set_header_if_no_content()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com")
            {
                Content = null
            };

            var result = client.SendAsync(request)
                .Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(request.Content, Is.Null);
        }
    }
}
