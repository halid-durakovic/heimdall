using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Heimdall.Server
{
    public class HeimdallConfig
    {
        private static bool byPassWebApiCorsAndImplementOptions = false;

        private static readonly List<string> InsecureVerbs = new List<string>();
        private static readonly List<string> InsecurePaths = new List<string>();
        private static readonly List<string> InsecureVerbsAndPaths = new List<string>();

        internal static bool ByPassWebApiCorsAndImplementOptions
        {
            get { return byPassWebApiCorsAndImplementOptions; }
        }

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

        public static void AllowVerbAndPath(string verb, string path)
        {
            if (!string.IsNullOrEmpty(verb) && !string.IsNullOrEmpty(path))
                InsecureVerbsAndPaths.Add(GetVerbAndPath(verb, path));
        }

        public static void EnableByPassWebApiCorsAndImplementOptions(bool yesOrNo)
        {
            byPassWebApiCorsAndImplementOptions = yesOrNo;
        }

        private static string GetVerbAndPath(string verb, string path)
        {
            return $"{verb.ToUpper()} {path.ToLower()}";
        }

        internal static bool IgnoreVerb(HttpRequestMessage message)
        {
            var currentVerb = message.Method.ToString();
            return InsecureVerbs.Any(x => currentVerb.ToUpper().StartsWith(x));
        }

        internal static bool IgnorePath(HttpRequestMessage message)
        {
            var currentPathAndQuery = message.RequestUri.PathAndQuery;
            return InsecurePaths.Any(x => currentPathAndQuery.ToLower().StartsWith(x));
        }

        internal static bool IgnoreVerbAndPath(HttpRequestMessage message)
        {
            var currentVerb = message.Method.ToString();
            var currentPathAndQuery = message.RequestUri.PathAndQuery;
            return InsecureVerbsAndPaths.Any(x => GetVerbAndPath(currentVerb, currentPathAndQuery).StartsWith(x));
        }

        internal static bool HandleByPassWebApiCorsAndImplementOptions(HttpRequestMessage message)
        {
            if (byPassWebApiCorsAndImplementOptions)
            {
                var currentVerb = message.Method.ToString().ToUpper();
                if (currentVerb == "OPTIONS")
                {
                    message.Headers.Add("Access-Control-Allow-Origin", "*");
                    return true;
                }
            }
            return false;
        }
    }
}