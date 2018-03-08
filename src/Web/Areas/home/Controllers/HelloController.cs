using System;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Web.Areas.home.Controllers
{
    [Area("home")]
    public class HelloController : Controller
    {
        private readonly Random _rnd;

        public HelloController()
        {
            _rnd = new Random();
        }
        public IActionResult Index()
        {
            Log.Debug("get Index");
            return View();
        }

        public IActionResult Fail()
        {
            Log.Debug("get Fail");

            var r = _rnd.Next(0, 10);
            Log.Debug("random fail {@r}", r);
            if (r <= 7)
                throw new Exception("Test failture");

            return LocalRedirect("/");
        }
    }
}