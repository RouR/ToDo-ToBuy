using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.home.Controllers
{
    [Area("home")]
    public class AngularController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}