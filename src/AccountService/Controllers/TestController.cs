using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static CustomLogs.SetupCustomLogs;

namespace AccountService.Controllers
{
    public class TestController : Controller
    {
        private readonly Random _rnd;

        public TestController()
        {
            _rnd = new Random();
        }

        [HttpPost]
        public IActionResult Test1(string request)
        {
            Logger().Debug("call Test1");
            return Ok(request+" *-*" + request);
        }

        [HttpPost]
        public async Task<IActionResult> TestDelay(string request)
        {
            var delay = _rnd.Next(100, 2900);
            Logger().Debug("call TestDelay for request {0} with delay {1}", request, delay);
            await Task.Delay(delay);
            Logger().Debug("finish TestDelay for request {0}", request);
            return Ok(request+" !-!" + delay);
        }
    }
}