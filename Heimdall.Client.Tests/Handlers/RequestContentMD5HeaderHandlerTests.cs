using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Heimdall.Client.Handlers;
using Heimdall.Tests.Framework;
using NUnit.Framework;

namespace Heimdall.Client.Tests.Handlers
{
    [TestFixture]
    public class RequestContentMD5HeaderHandlerTests
    {
        private HttpClient client;
        private HttpRequestMessage request;

        [SetUp]
        public void SetUp()
        {
            var handler = new RequestContentMd5HeaderHandler() { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);

            request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com") { Content = new StringContent("something") };
            request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);
        }

        [Test]
        public void generates_md5_header_and_saves_to_request()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com")
            {
                Content = new StringContent("something")
            };

            var expectedMD5 = new MD5CryptoServiceProvider()
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
