using System.Web.Mvc;

namespace WebApiAuthentication.Examples.IISServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Waiting for request....");
        }
    }
}
