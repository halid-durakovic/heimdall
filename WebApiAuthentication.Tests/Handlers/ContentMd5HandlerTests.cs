using NUnit.Framework;
using System;
using System.Net.Http;
using WebApiAuthentication.Handlers;
using WebApiAuthentication.Tests.Framework;

namespace WebApiAuthentication.Tests.Handlers
{
    [TestFixture]
    public class ContentMd5HandlerTests
    {
        private HttpClient client;
        private HttpRequestMessage request;

        [SetUp]
        public void SetUp()
        {
            var handler = new ContentMd5Handler() { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);

            request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com") { Content = new StringContent("something") };
            request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);
        }

        [Test]
        public void adds_md5_header()
        {
            var result = client.SendAsync(request).Result;

            Assert.That(request.Content.Headers.ContentMD5, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void does_not_add_md5_header_if_content_emtpy()
        {
            request.Content = null;

            var result = client.SendAsync(request).Result;

            Assert.That(request.Content, Is.Null.Or.Empty);
        }
    }
}
