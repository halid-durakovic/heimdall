using System.Net.Http;

namespace Heimdall.Server.Interfaces
{
    public interface IAuthenticateRequest
    {
        bool IsAuthenticated(HttpRequestMessage request);
    }
}