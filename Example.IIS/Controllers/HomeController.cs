using System.Web.Mvc;

namespace Example.IIS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Waiting for request....");
        }
    }
}
