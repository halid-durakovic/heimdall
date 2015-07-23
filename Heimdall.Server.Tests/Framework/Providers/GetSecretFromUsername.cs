namespace Heimdall.Server.Tests.Framework.Providers
{
    public class GetSecretFromUsername : IGetSecretFromUsername
    {
        public string Secret(string username)
        {
            return "secret";
        }
    }
}