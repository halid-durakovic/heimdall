using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
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
            InsecureVerbs.Add(verb?.ToUpper());
        }

        public static void AllowPath(string path)
        {
            InsecurePaths.Add(path);
        }

        internal static bool IgnoreVerb(HttpRequestMessage message)
        {
            return InsecureVerbs.Any(x => HttpContext.Current.Request.HttpMethod.ToUpper().ToLower().StartsWith(x));
        }

        internal static bool IgnorePath(HttpRequestMessage message)
        {
            return InsecurePaths.Any(x => message.RequestUri.PathAndQuery.ToLower().StartsWith(x));
        }
    }
}