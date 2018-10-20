using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared;
using static CustomLogs.SetupCustomLogs;

namespace Web.Areas.home.Controllers
{
    [Area("home")]
    public class HelloController : Controller
    {
        private readonly AccountServiceClient _accountServiceClient;
        private readonly Random _rnd;

        public HelloController(AccountServiceClient accountServiceClient)
        {
            _accountServiceClient = accountServiceClient;
            _rnd = new Random();
        }
        public IActionResult Index()
        {
            Logger().Debug("get Index");
            Logger().Information("get Index rnd {R}", _rnd.Next(0, 10));

            Log.Debug("get Index");
            return View();
        }

        public IActionResult Fail()
        {
            Log.Debug("get Fail");

            var r = _rnd.Next(0, 10);
            Log.Debug("random fail {@R}", r);
            if (r <= 7)
                throw new Exception("Test failture");

            return LocalRedirect("/");
        }

        public async Task<IActionResult> Test1()
        {
            Logger().Debug("get Test1");
            var sb = new StringBuilder();
            var request = $"req{_rnd.Next(20, 90)}";
            sb.AppendLine("Request:");
            sb.AppendLine(request);
            var response = await _accountServiceClient.TestCall(request);
            sb.AppendLine("Response:");
            sb.AppendLine(response);

            return View("simple", sb.ToString());
        }

        public async Task<IActionResult> Test2()
        {
            Logger().Debug("get Test2");
            var sb = new StringBuilder();
            var request = $"req{_rnd.Next(2000, 9000)}";
            sb.AppendLine("Request:");
            sb.AppendLine(request);
            var response = await _accountServiceClient.TestDelay(request);
            sb.AppendLine("Response:");
            sb.AppendLine(response);

            return View("simple", sb.ToString());
        }
    }
}