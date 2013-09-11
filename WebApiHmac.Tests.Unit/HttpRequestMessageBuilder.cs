using System.Net.Http;

namespace WebApiHmac.Tests.Unit
{
    public class HttpRequestMessageBuilder
    {
        private HttpMethod httpMethod;
        private string url;
        private HttpContent content;
        private string contentMd5;

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

            if (!string.IsNullOrEmpty(contentMd5))
                result.Content.Headers.ContentMD5 = GetBytes(contentMd5);

            return result;
        }

        public HttpRequestMessageBuilder WithContentMd5(string md5)
        {
            this.contentMd5 = md5;
            return this;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

    }
}