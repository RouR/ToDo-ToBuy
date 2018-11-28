using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using static CustomLogs.SetupCustomLogs;

namespace Web.Areas.api
{
    /// <summary>
    /// Only for testing infrastructure
    /// </summary>
    [Area("api")]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ITracer _tracer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracer"></param>
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
        public IEnumerable<string> Index()
        {
            return new[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public string Get(int id)
        {
            Logger().Information("api/values {$id}", id);

            _tracer.ActiveSpan?
                .SetOperationName("api-values")
                .Log(new Dictionary<string, object> {
                    { "test234234", id }
                });


            return "value " + id + "  *:" + _tracer.ActiveSpan?.Context.TraceId;
        }
        
        /// <summary>
        /// test auth
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("private/{id}")]
        public string GetPrivate(int id)
        {
            Logger().Information("api/GetPrivate {$id}", id);

            _tracer.ActiveSpan?
                .SetOperationName("api-GetPrivate")
                .Log(new Dictionary<string, object> {
                    { "aaaa id", id }
                });


            return "value " + id + "  *:" + _tracer.ActiveSpan?.Context.TraceId;
        }

        
    }
}
