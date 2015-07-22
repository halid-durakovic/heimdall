using System.Net.Http;
using Heimdall.Client.Handlers;

namespace Heimdall.Client
{
    public interface IHeimdallClientFactory
    {
        HttpClient Create(string username, IGetSecretFromUsername getSecretFromUsername);
        HttpClient Create(string username, string secret);
    }

    public class HeimdallClientFactory : IHeimdallClientFactory
    {
        public static HttpClient Create(string username, IGetSecretFromUsername getSecretFromUsername)
        {
            return Create(username, getSecretFromUsername.Secret(username));
        }

        HttpClient IHeimdallClientFactory.Create(string username, string secret)
        {
            return Create(username, secret);
        }

        HttpClient IHeimdallClientFactory.Create(string username, IGetSecretFromUsername getSecretFromUsername)
        {
            return Create(username, getSecretFromUsername);
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
