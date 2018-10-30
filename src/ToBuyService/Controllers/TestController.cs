using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomLogs;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace ToBuyService.Controllers
{
    /// <summary>
    /// Only for testing infrastructure
    /// </summary>
    public class TestController : Controller
    {
        private readonly ITracer _tracer;
        private readonly Random _rnd;
        private static int counter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracer"></param>
        public TestController(ITracer tracer)
        {
            _tracer = tracer;
            _rnd = new Random();
        }

        /// <summary>
        /// No delay
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Test1(string request)
        {
            SetupCustomLogs.Logger().Debug("call Test1");
            _tracer.ActiveSpan?
                .SetOperationName("TestController/Test1 " + request)
                .Log(new Dictionary<string, object>
                {
                    {"request", request},
                });
            return Ok(request + " *-*" + request);
        }

        /// <summary>
        /// Random delay
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TestDelay(string request)
        {
            var delay = _rnd.Next(300, 1100);
            SetupCustomLogs.Logger().Debug("call TestDelay for request {0} with delay {1}", request, delay);
            _tracer.ActiveSpan?
                .SetOperationName("TestController/TestDelay")
                .Log(new Dictionary<string, object>
                {
                    {"request", request},
                    {"delay", delay},
                });
            await Task.Delay(delay);
            SetupCustomLogs.Logger().Debug("finish TestDelay for request {0}", request);
            _tracer.ActiveSpan?.Log($"finish TestDelay for request {request}");
            return Ok(request + " !-!" + delay);
        }

        /// <summary>
        /// Random fail
        /// fail =  (counter +1 ) % 3 != 0
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TestFail(string request)
        {
            if (counter++ > 1000)
                counter = 0;

            var fail = (counter +1 ) % 3 != 0;
            
            var delay = fail ? _rnd.Next(600, 1600) :  _rnd.Next(300, 700);
            
            _tracer.ActiveSpan?
                .SetOperationName("TestController/TestFail")
                .Log(new Dictionary<string, object>
                {
                    {"request", request},
                    {"counter", counter},
                    {"fail", fail},
                    {"delay", delay},
                });
            await Task.Delay(delay);
            
            if(fail)
                throw new Exception("Random fail, retry request");
            
            return Ok(request + " !-!" + delay);
        }
    }
}