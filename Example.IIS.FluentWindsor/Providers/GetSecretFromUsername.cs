using Heimdall;

namespace Example.IIS.Providers
{
    public class GetSecretFromUsername : IGetSecretFromUsername
    {
        public string Secret(string username)
        {
            return "secret";
        }
    }
}