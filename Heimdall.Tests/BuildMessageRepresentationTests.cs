using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;

namespace Heimdall.Tests
{
    [TestFixture]
    public class BuildMessageRepresentationTests
    {
        private HttpRequestMessage request;
        private BuildMessageRepresentation buildMessageString;

        [SetUp]
        public void SetUp()
        {
            request = HttpRequestMessageBuilder.Instance().Build();
            buildMessageString = new BuildMessageRepresentation();
        }

        [Test]
        public void returns_string_containing_path()
        {
            var result = buildMessageString.Build(request);
            Assert.That(result, Contains.Substring("/api"));
        }

        [Test]
        public void returns_string_containing_path_without_double_slashes()
        {
            request.RequestUri = new Uri("http://www.me.com//api");

            var result = buildMessageString.Build(request);
            Assert.That(result, Contains.Substring("/api"));
            Assert.That(result, Is.Not.ContainsSubstring("//api"));
        }

        [Test]
        public void returns_string_containing_verb()
        {
            request.Method = new HttpMethod("GET");

            var result = buildMessageString.Build(request);
            Assert.That(result, Contains.Substring("GET"));
        }

        [Test]
        public void returns_string_containing_request_content_md5()
        {
            request.Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("someKey", "someValue"), });

            var md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(
                    request.Content.ReadAsByteArrayAsync().Result);

            request.Content.Headers.ContentMD5 = md5;

            var result = buildMessageString.Build(request);

            Assert.That(result, Contains.Substring(Convert.ToBase64String(md5)));
        }

        [Test]
        public void returns_string_containing_timestamp()
        {
            request.Headers.Date = DateTime.UtcNow;

            var result = buildMessageString.Build(request);

            var date = request.Headers.Date.Value.UtcDateTime.ToString(CultureInfo.InvariantCulture);

            Assert.That(result, Contains.Substring(date));
        }

        [Test]
        public void returns_string_containing_timestamp_of_custom_date_header_if_present()
        {
            var date = DateTime.Now.ToString();
            request.Headers.Add(HeaderNames.CustomDateHeader, date);

            var result = buildMessageString.Build(request);

            Assert.That(result, Contains.Substring(date));
        }

        [Test]
        public void returns_string_containing_timestamp_of_custom_date_header_if_present_even_if_request_date_header_is_present()
        {
            var date = DateTime.Now.AddDays(-12).AddMonths(-34).ToString();
            request.Headers.Add(HeaderNames.CustomDateHeader, date);
            request.Headers.Date = DateTime.Now;

            var result = buildMessageString.Build(request);

            Assert.That(result, Contains.Substring(date));
        }

        [Test]
        public void returns_string_containing_contenttype()
        {
            var buildMessageString = new BuildMessageRepresentation();

            request.Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("a", "b"), });
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            var result = buildMessageString.Build(request);

            Assert.That(result, Contains.Substring("text/html"));
        }
    }
}
