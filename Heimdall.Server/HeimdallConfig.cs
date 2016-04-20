using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Heimdall.Server
{
    public class HeimdallConfig
    {
        private static List<string> InsecureVerbs = new List<string>();
        private static List<string> InsecurePaths = new List<string>();

        public static void AllowVerb(string verb)
        {
            if (!string.IsNullOrEmpty(verb))
                InsecureVerbs.Add(verb.ToUpper());
        }

        public static void AllowPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
                InsecurePaths.Add(path);
        }

        internal static bool IgnoreVerb(HttpRequestMessage message)
        {
            return InsecureVerbs.Any(x => message.Method.ToString().ToUpper().ToLower().StartsWith(x));
        }

        internal static bool IgnorePath(HttpRequestMessage message)
        {
            return InsecurePaths.Any(x => message.RequestUri.PathAndQuery.ToLower().StartsWith(x));
        }
    }
}