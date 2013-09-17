using System.Web.Http.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiAuthentication.Tests
{
    [TestFixture]
    public class BuildMessageRepresentationTests
    {
        private HttpActionContext actionContext;

        [SetUp]
        public void SetUp()
        {
            actionContext = HttpActionContextBuilder.Instance().Build();
        }

        [Test]
        public void returns_string_containing_uri()
        {
            var buildMessageString = new BuildMessageRepresentation();

            var result = buildMessageString.Build(actionContext.Request);
            Assert.That(result, Contains.Substring("/api"));
        }

        [Test]
        public void returns_string_containing_verb()
        {
            var buildMessageString = new BuildMessageRepresentation();

            actionContext.Request.Method = new HttpMethod("GET");

            var result = buildMessageString.Build(actionContext.Request);
            Assert.That(result, Contains.Substring("GET"));
        }

        [Test]
        public void returns_string_containing_request_content_md5()
        {
            var buildMessageString = new BuildMessageRepresentation();

            actionContext.Request.Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("someKey", "someValue"), });

            var md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(
                    actionContext.Request.Content.ReadAsByteArrayAsync().Result);

            actionContext.Request.Content.Headers.ContentMD5 = md5;

            var result = buildMessageString.Build(actionContext.Request);

            Assert.That(result, Contains.Substring(Convert.ToBase64String(md5)));
        }

        [Test]
        public void returns_string_containing_timestamp()
        {
            var buildMessageString = new BuildMessageRepresentation();

            var result = buildMessageString.Build(actionContext.Request);

            var date = actionContext.Request.Headers.Date.Value.UtcDateTime.ToString(CultureInfo.InvariantCulture);

            Assert.That(result, Contains.Substring(date));
        }

        [Test]
        public void returns_string_containing_contenttype()
        {
            var buildMessageString = new BuildMessageRepresentation();

            actionContext.Request.Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("a", "b"), });
            actionContext.Request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            var result = buildMessageString.Build(actionContext.Request);

            var date = actionContext.Request.Headers.Date.Value.UtcDateTime.ToString(CultureInfo.InvariantCulture);

            Assert.That(result, Contains.Substring("text/html"));
        }
    }
}
