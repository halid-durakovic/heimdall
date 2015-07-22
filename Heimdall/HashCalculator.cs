using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Heimdall
{
    public interface ICalculateHashes
    {
        Task<byte[]> ComputeHash(HttpRequestMessage request);
        Task<bool> IsValidHash(HttpRequestMessage request);
    }

    public class HashCalculator : ICalculateHashes
    {
        public async Task<byte[]> ComputeHash(HttpRequestMessage request)
        {
            var content = await request.Content.ReadAsStringAsync();
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(string.Empty)))
                return hmacsha256.ComputeHash(Encoding.Default.GetBytes(content));
        }

        public async Task<bool> IsValidHash(HttpRequestMessage request)
        {
            var hashHeader = request.Content.Headers.ContentMD5;

            if (request.Content == null)
                return hashHeader == null || hashHeader.Length == 0;

            var content = await request.Content.ReadAsStringAsync();

            byte[] hash;
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(string.Empty)))
                hash = hmacsha256.ComputeHash(Encoding.Default.GetBytes(content));

            return hash.SequenceEqual(hashHeader);
        }
    }
}