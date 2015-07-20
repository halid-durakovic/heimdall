using System.Net.Http;

namespace Heimdall.Interfaces
{
    public interface ICalculateHashes
    {
        byte[] ComputeHash(HttpContent httpContent);
        bool IsValidHash(HttpRequestMessage request);
    }
}