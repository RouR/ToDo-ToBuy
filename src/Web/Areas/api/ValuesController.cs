﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Serilog;
using static CustomLogs.SetupCustomLogs;

namespace Web.Areas.api
{
    [Area("api")]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ITracer _tracer;

        public ValuesController(ITracer tracer)
        {
            _tracer = tracer;
        }

        /// <summary>
        /// Some text from comments
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            Logger().Information("api/values {$id}", id);

            _tracer.ActiveSpan?
                .SetOperationName("api-values")
                .Log(new Dictionary<string, object> {
                    { "test234234", id }
                });


            return "value " + id + "  *:" + _tracer.ActiveSpan?.Context.SpanId;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
