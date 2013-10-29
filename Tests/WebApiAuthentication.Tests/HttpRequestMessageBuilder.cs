using System.Net.Http;

namespace WebApiAuthentication.Tests
{
    public class HttpRequestMessageBuilder
    {
        private string url;
        private byte[] contentMd5;
        private HttpContent content;
        private readonly HttpMethod httpMethod;
        private readonly HashCalculator hashCalculator;

        public HttpRequestMessageBuilder()
        {
            httpMethod = HttpMethod.Post;
            url = "http://www.me.com/api";
            hashCalculator = new HashCalculator();
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
            contentMd5 = hashCalculator.ComputeHash(content);

            return this;
        }

        public HttpRequestMessageBuilder WithContentMD5(byte[] bytes)
        {
            contentMd5 = bytes;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var result = new HttpRequestMessage(httpMethod, url) { Content = content };

            if (content != null)
                result.Content.Headers.ContentMD5 = contentMd5;

            return result;
        }
    }
}