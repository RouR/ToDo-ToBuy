using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using App.Metrics;
using Serilog.Context;
using Web.Utils;

namespace Web
{
    public sealed class LogMiddleware
    {
        private readonly InstanceInfo _instanceInfo;
        private readonly IMetrics _metrics;

        const string MessageTemplate =
            "HTTP {RequestId} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        static readonly ILogger Log = Serilog.Log.ForContext<LogMiddleware>();

        readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next, InstanceInfo instanceInfo, IMetrics metrics)
        {
            _instanceInfo = instanceInfo;
            _metrics = metrics;
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var reqId = Guid.NewGuid().ToString();
            httpContext.Request.Headers.Add(Settings.Header, reqId);

            using (LogContext.PushProperty("InstanceInfoId", _instanceInfo.Id))
            using (LogContext.PushProperty("InstanceInfoCodeVer", _instanceInfo.CodeVer))
            using (_metrics.Measure.Timer.Time(MetricsRegistry.Sample1Timer))
            {
                _metrics.Measure.Counter.Increment(MetricsRegistry.Sample1Counter);
                _metrics.Measure.Counter.Increment(MetricsRegistry.Sample2Counter);
                var sw = Stopwatch.StartNew();
                try
                {
                    await _next(httpContext);
                    sw.Stop();
                    _metrics.Measure.Counter.Decrement(MetricsRegistry.Sample1Counter);
                    _metrics.Measure.Counter.Decrement(MetricsRegistry.Sample2Counter);

                    var statusCode = httpContext.Response?.StatusCode;

                    _metrics.Measure.Meter.Mark(MetricsRegistry.Sample1Meter,
                        new MetricTags(new[] { "Code", "CodeX" },
                            new[] { statusCode.ToString(), statusCode.ToString()[0] + "xx" }));

                    var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                    var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;
                    log.Write(level, MessageTemplate, GetReqestId(httpContext), httpContext.Request.Method,
                        httpContext.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);
                }
                // Never caught, because `LogException()` returns false.
                catch (Exception ex) when (LogException(httpContext, sw, ex))
                {
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetReqestId(HttpContext httpContext)
        {
            return httpContext.Request.Headers[Settings.Header].SingleOrDefault();
        }

        static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Headers[Settings.Header].SingleOrDefault(),
                    httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds);

            return false;
        }

        static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm",
                    request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }
    }
}