using System.Net.Http;

namespace WebApiAuthentication.Client
{
    public class SigningHttpClientFactory
    {
        public static HttpClient Create()
        {
            //handlers are applied in the order they are passed in the Create method
            return HttpClientFactory.Create(
                new TimestampHandler(),
                new RequestContentMD5HeaderGeneratingHandler());
        }
    }
}
