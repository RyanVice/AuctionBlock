using System.Web.Mvc;

namespace AuctionBlock.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HelloWorld()
        {
            return View();
        }
    }
}
