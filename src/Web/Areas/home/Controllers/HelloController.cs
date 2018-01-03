using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.home.Controllers
{
    [Area("home")]
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}