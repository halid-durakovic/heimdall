using System.Net.Http;
using WebApiAuthentication.Client.Handlers;

namespace WebApiAuthentication.Client
{
    internal class StaticSecretFromKey : IGetSecretFromKey
    {
        private readonly string secret;

        public StaticSecretFromKey(string secret)
        {
            this.secret = secret;
        }

        public string Secret(string key)
        {
            return secret;
        }
    }

    public class SigningHttpClientFactory
    {
        public static HttpClient Create(string username, IGetSecretFromKey getSecretFromKey)
        {
            //handlers are applied in the order they are passed in the Create method
            return HttpClientFactory.Create(
                new UsernameHandler(username),
                new TimestampHandler(),
                new ContentMD5HeaderHandler(),
                new HmacSigningHandler(username, getSecretFromKey)
                );
        }

        public static HttpClient Create(string username, string getSecretFromKey)
        {
            return Create(username, new StaticSecretFromKey(getSecretFromKey));
        }
    }
}
