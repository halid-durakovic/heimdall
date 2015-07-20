using System.Net.Http;

namespace Heimdall.Interfaces
{
    public interface IBuildMessageRepresentation
    {
        string Build(HttpRequestMessage request);
    }
}