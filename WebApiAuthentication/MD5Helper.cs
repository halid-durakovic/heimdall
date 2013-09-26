using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebApiAuthentication
{
    public class MD5Helper
    {
        public static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (var md5 = MD5.Create())
                return md5.ComputeHash(await httpContent.ReadAsByteArrayAsync());
        }

        public static async Task<bool> IsMd5Valid(HttpRequestMessage request)
        {
            var hashHeader = request.Content.Headers.ContentMD5;

            if (request.Content == null)
                return hashHeader == null || hashHeader.Length == 0;

            var hash = await ComputeHash(request.Content);
            return hash.SequenceEqual(hashHeader);
        }
    }
}
