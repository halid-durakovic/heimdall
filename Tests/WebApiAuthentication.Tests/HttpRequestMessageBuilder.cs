using System;
using System.Net.Http;
using System.Text;

namespace WebApiAuthentication.Tests
{
    public class HttpRequestMessageBuilder
    {
        private HttpMethod httpMethod;
        private string url;
        private HttpContent content;
        private byte[] contentMD5;

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
            contentMD5 = MD5Helper.ComputeHash(content).Result;

            return this;
        }

        public HttpRequestMessageBuilder WithContentMD5(byte[] bytes)
        {
            contentMD5 = bytes;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var result = new HttpRequestMessage(httpMethod, url) { Content = content };

            if (content != null)
                result.Content.Headers.ContentMD5 = contentMD5;

            return result;
        }
    }
}