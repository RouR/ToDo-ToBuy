using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Serilog;
using Shared;
using static CustomLogs.SetupCustomLogs;

namespace Web.Areas.home.Controllers
{
    /// <summary>
    /// default route
    /// </summary>
    [Area("home")]
    public class HelloController : Controller
    {
        private readonly ITracer _tracer;
        private readonly AccountServiceClient _accountServiceClient;
        private readonly Random _rnd;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracer"></param>
        /// <param name="accountServiceClient"></param>
        public HelloController(ITracer tracer,
            AccountServiceClient accountServiceClient)
        {
            _tracer = tracer;
            _accountServiceClient = accountServiceClient;
            _rnd = new Random();
        }
        
        /// <summary>
        /// default page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Logger().Debug("get Index");
            Logger().Information("get Index rnd {R}", _rnd.Next(0, 10));

            _tracer.ActiveSpan?.Log("HelloController/Index");

            Log.Debug("get Index");
            return View();
        }

        /// <summary>
        /// Only for testing infrastructure
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IActionResult Fail()
        {
            Log.Debug("get Fail");

            var r = _rnd.Next(0, 10);
            Log.Debug("random fail {@R}", r);
            _tracer.ActiveSpan?
                .SetOperationName("HelloController/Fail")
                .Log(new Dictionary<string, object> {
                    { "random", r },
                    { "condition", "r <= 7" },
                    { "Exception", r <= 7},
                });
            if (r <= 7)
                throw new Exception("Test failture");

            return LocalRedirect("/");
        }

        /// <summary>
        /// Only for testing infrastructure
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Only for testing infrastructure
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Test2()
        {
            Logger().Debug("get Test2");
            var sb = new StringBuilder();
            var request = $"req{_rnd.Next(2000, 9000)}";
            sb.AppendLine("Request:");
            sb.AppendLine(request);
            _tracer.ActiveSpan?
                .Log(new Dictionary<string, object> {
                    { "accountServiceClient.TestDelay", request },
                });
            var response = await _accountServiceClient.TestDelay(request);
            sb.AppendLine("Response:");
            sb.AppendLine(response);

            return View("simple", sb.ToString());
        }
    }
}