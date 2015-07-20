using System.Net.Http;

namespace Heimdall.Interfaces
{
    public interface IBuildRequestSignature
    {
        string Build(string secret, HttpRequestMessage request);
    }
}