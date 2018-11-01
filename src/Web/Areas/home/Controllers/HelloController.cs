using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Serilog;
using Shared;
using Utils;
using static CustomLogs.SetupCustomLogs;

namespace Web.Areas.home.Controllers
{
    /// <summary>
    /// default route
    /// </summary>
    [Area("home")]
    public class HelloController : Controller
    {
        private readonly Random _rnd;
        private readonly ITracer _tracer;
        // ReSharper disable NotAccessedField.Local
        private readonly AccountServiceClient _accountServiceClient;
        private readonly ToDoServiceClient _todoServiceClient;
        private readonly ToBuyServiceClient _tobuyServiceClient;
        // ReSharper restore NotAccessedField.Local

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracer"></param>
        /// <param name="accountServiceClient"></param>
        /// <param name="todoServiceClient"></param>
        /// <param name="tobuyServiceClient"></param>
        public HelloController(ITracer tracer,
            AccountServiceClient accountServiceClient,
            ToDoServiceClient todoServiceClient,
            ToBuyServiceClient tobuyServiceClient
            )
        {
            _tracer = tracer;
            _accountServiceClient = accountServiceClient;
            _todoServiceClient = todoServiceClient;
            _tobuyServiceClient = tobuyServiceClient;
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
        
        
        /// <summary>
        /// Only for testing infrastructure
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Test3()
        {
            var sb = new StringBuilder();
            var request = $"req{_rnd.Next(2000, 9000)}";
            sb.AppendLine("Request:");
            sb.AppendLine(request);

            _tracer.ActiveSpan?.SetOperationName("HelloController/Test3 - 1+2");
            
            var responseAcc = await _accountServiceClient.TestDelay(request);

            var (toToResult, toBuyResult) = await TaskEx.WhenAll(
                _todoServiceClient.TestDelay(request),
                _tobuyServiceClient.TestDelay(request)
                );

            sb.AppendLine("Response AccountService:");
            sb.AppendLine(responseAcc);
            
            sb.AppendLine("Response ToDoService:");
            sb.AppendLine(toToResult);
            
            sb.AppendLine("Response ToBuyService:");
            sb.AppendLine(toBuyResult);

            return View("simple", sb.ToString());
        }
        
        /// <summary>
        /// Only for testing infrastructure
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Test4()
        {
            var sb = new StringBuilder();
            var request = $"req{_rnd.Next(2000, 9000)}";
            sb.AppendLine("Request:");
            sb.AppendLine(request);
            
            _tracer.ActiveSpan?.SetOperationName("HelloController/Test4 - Fail");
            
            var responseAcc = await _accountServiceClient.TestDelay(request);

            var (toToResult, toBuyResult) = await TaskEx.WhenAll(
                _todoServiceClient.TestFail(request),
                _tobuyServiceClient.TestFail(request)
            );

            sb.AppendLine("Response AccountService:");
            sb.AppendLine(responseAcc);
            
            sb.AppendLine("Response ToDoService:");
            sb.AppendLine(toToResult);
            
            sb.AppendLine("Response ToBuyService:");
            sb.AppendLine(toBuyResult);

            return View("simple", sb.ToString());
        }
        
        /// <summary>
        /// Only for testing infrastructure
        /// </summary>
        /// <returns></returns>
        public IActionResult Headers()
        {
            var sb = new StringBuilder();

            foreach (var header in this.HttpContext.Request.Headers)
            {
                sb.AppendLine($"\t{header.Key} = {header.Value}");
            }
            
            return View("simple", sb.ToString());
        }
    }
}