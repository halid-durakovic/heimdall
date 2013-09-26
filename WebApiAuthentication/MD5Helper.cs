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
    }
}
