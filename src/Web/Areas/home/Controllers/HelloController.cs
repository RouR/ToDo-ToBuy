using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.home.Controllers
{
    [Area("home")]
    public class HelloController : Controller
    {
        private readonly ILogger _logger;
        private readonly Random _rnd;

        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
            _rnd = new Random();
        }
        public IActionResult Index()
        {
            _logger.LogDebug("get Index");
            return View();
        }

        public IActionResult Fail()
        {
            _logger.LogDebug("get Fail");

            if (_rnd.Next(0, 10) <= 7)
                throw new Exception("Test failture");

            return LocalRedirect("/");
        }
    }
}