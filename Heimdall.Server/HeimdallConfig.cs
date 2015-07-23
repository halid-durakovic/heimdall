using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;

namespace Heimdall.Server
{
    public class HeimdallConfig
    {
        private static List<string> InsecurePaths = new List<string>();
         
        public static void AllowPath(string path)
        {
            InsecurePaths.Add(path);
        }

        internal static bool IgnorePath(HttpRequestMessage message)
        {
            return InsecurePaths.Any(x => message.RequestUri.PathAndQuery.ToLower().StartsWith(x));
        }
    }
}