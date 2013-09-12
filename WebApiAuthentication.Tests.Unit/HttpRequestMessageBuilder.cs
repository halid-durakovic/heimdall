using System;
using System.Net.Http;

namespace WebApiAuthentication.Tests.Unit
{
    public class HttpRequestMessageBuilder
    {
        private HttpMethod httpMethod;
        private string url;
        private HttpContent content;
        private string contentMD5;

        public HttpRequestMessageBuilder()
        {
            httpMethod = HttpMethod.Post;
            url = "http://www.me.com/api";
        }

        public static HttpRequestMessageBuilder Instance()
        {
            return new HttpRequestMessageBuilder();
        }

        public HttpRequestMessageBuilder WithUrl(string url)
        {
            this.url = url;
            return this;
        }

        public HttpRequestMessageBuilder WithContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var result = new HttpRequestMessage(httpMethod, url) { };

            if (!string.IsNullOrEmpty(contentMD5))
                result.Content.Headers.ContentMD5 = Convert.FromBase64String(contentMD5);

            return result;
        }

        public HttpRequestMessageBuilder WithContentMD5(string md5)
        {
            this.contentMD5 = md5;
            return this;
        }
    }
}