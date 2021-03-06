﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using static CustomLogs.SetupCustomLogs;

namespace AccountService.Controllers
{
    /// <summary>
    /// Only for testing infrastructure
    /// </summary>
    public class TestController : Controller
    {
        private readonly ITracer _tracer;
        private readonly Random _rnd;

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
            Logger().Debug("call Test1");
            _tracer.ActiveSpan?
                .SetOperationName("TestController/Test1 " + request)
                .Log(new Dictionary<string, object> {
                    { "request", request },
                });
            return Ok(request+" *-*" + request);
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
            Logger().Debug("call TestDelay for request {0} with delay {1}", request, delay);
            _tracer.ActiveSpan?
                .SetOperationName("TestController/TestDelay")
                .Log(new Dictionary<string, object> {
                    { "request", request },
                    { "delay", delay },
                });
            await Task.Delay(delay);
            Logger().Debug("finish TestDelay for request {0}", request);
            _tracer.ActiveSpan?.Log($"finish TestDelay for request {request}");
            return Ok(request+" !-!" + delay);
        }
    }
}