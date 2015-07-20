using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using Heimdall.Interfaces;

namespace Heimdall
{
    public class HashCalculator : ICalculateHashes
    {
        private static readonly object ComputeHashSyncronise = new object();
        private static readonly object IsValidHashHashSyncronise = new object();

        public byte[] ComputeHash(HttpContent httpContent)
        {
            lock (ComputeHashSyncronise)
            {
                var contentPayload = httpContent.ReadAsByteArrayAsync();
                var bytePayload = contentPayload.Result;
                using (var md5 = MD5.Create())
                    return md5.ComputeHash(bytePayload);
            }
        }

        public bool IsValidHash(HttpRequestMessage request)
        {
            lock (IsValidHashHashSyncronise)
            {
                var hashHeader = request.Content.Headers.ContentMD5;
                if (request.Content == null)
                    return hashHeader == null || hashHeader.Length == 0;
                var hash = ComputeHash(request.Content);
                return hash.SequenceEqual(hashHeader);
            }
        }
    }
}
