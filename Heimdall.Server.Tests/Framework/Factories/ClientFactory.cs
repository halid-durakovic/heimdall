using System.Net.Http;
using Heimdall.Client;

namespace Heimdall.Server.Tests.Framework.Factories
{
    public class ClientFactory
    {
        public static HttpClient CreateSignedClient()
        {
            var client = HeimdallClientFactory.Create("username", "secret");
            return client;
        }

        public static HttpClient CreateUnsignedClient()
        {
            return HttpClientFactory.Create();
        }
    }
}