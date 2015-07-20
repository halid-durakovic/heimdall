using System;
using System.Net.Http;
using Heimdall.Client.Handlers;
using Heimdall.Tests.Framework;
using NUnit.Framework;

namespace Heimdall.Client.Tests.Handlers
{
    [TestFixture]
    public class RequestTimestampHandlerTests
    {
        private TimestampHandler handler;
        private HttpClient client;

        [SetUp]
        public void SetUp()
        {
            handler = new TimestampHandler() { InnerHandler = new TestHandler() };
            client = new HttpClient(handler);
        }

        [Test]
        public void sets_date_header()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com");

            var result = client.SendAsync(request)
                .Result;

            Assert.That(request.Headers.Date, Is.Not.Null);
            Assert.That(request.Headers.Date, Is.Not.EqualTo(DateTime.MinValue));
        }
    }
}