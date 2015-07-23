using System.Collections.Generic;
using System.Web.Http;

namespace Heimdall.Server.Tests.Framework.Controllers
{
    public class AnyController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}