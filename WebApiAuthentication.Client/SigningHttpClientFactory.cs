using System.Net.Http;
using WebApiAuthentication.Client.Handlers;
using WebApiAuthentication.Handlers;

namespace WebApiAuthentication.Client
{
    public class SigningHttpClientFactory
    {
        public static HttpClient Create(string username, IGetSecretFromUsername getSecretFromUsername)
        {
            return Create(username, getSecretFromUsername.Secret(username));
        }

        public static HttpClient Create(string username, string secret)
        {
            //handlers are applied in the order they are passed in the Create method
            return HttpClientFactory.Create(
                new UsernameHandler(username),
                new TimestampHandler(),
                new RequestMD5HeaderHandler(),
                new HmacSigningHandler(secret)
                );
        }
    }
}
