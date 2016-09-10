using System.Web.Mvc;

namespace SleepingBarber.Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}