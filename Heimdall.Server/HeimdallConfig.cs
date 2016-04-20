using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Heimdall.Server
{
    public class HeimdallConfig
    {
        private static readonly List<string> InsecureVerbs = new List<string>();
        private static readonly List<string> InsecurePaths = new List<string>();

        public static void AllowVerb(string verb)
        {
            if (!string.IsNullOrEmpty(verb))
                InsecureVerbs.Add(verb.ToUpper());
        }

        public static void AllowPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
                InsecurePaths.Add(path.ToLower());
        }

        internal static bool IgnoreVerb(HttpRequestMessage message)
        {
            var currentVerb = message.Method.ToString();
            return InsecureVerbs.Any(x => currentVerb.ToUpper().StartsWith(x));
        }

        internal static bool IgnorePath(HttpRequestMessage message)
        {
            var pathAndQuery = message.RequestUri.PathAndQuery;
            return InsecurePaths.Any(x => pathAndQuery.ToLower().StartsWith(x));
        }
    }
}