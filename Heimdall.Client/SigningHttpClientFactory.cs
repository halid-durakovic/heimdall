using System.Net.Http;
using Heimdall.Client.Handlers;

namespace Heimdall.Client
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
                new RequestContentMd5HeaderHandler(),
                new HmacSigningHandler(secret)
                );
        }
    }
}
