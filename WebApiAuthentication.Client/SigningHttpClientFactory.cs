using System.Net.Http;
using WebApiAuthentication.Client.Handlers;

namespace WebApiAuthentication.Client
{
    public class SigningHttpClientFactory
    {
        public static HttpClient Create(string username)
        {
            //handlers are applied in the order they are passed in the Create method
            return HttpClientFactory.Create(
                new UsernameHandler(username),
                new TimestampHandler(),
                new ContentMD5HeaderHandler());
        }
    }
}
