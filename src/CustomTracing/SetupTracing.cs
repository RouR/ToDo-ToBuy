using System;
using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Contrib.NetCore.CoreFx;
using OpenTracing.Util;
using Shared;
using static CustomLogs.SetupCustomLogs;

namespace CustomTracing
{
    public static class SetupTracing
    {
        public static void ConfigureServices(InstanceInfo instanceInfo, IServiceCollection services, bool isPublicWebService)
        {
            var jaegerAgentHost = Environment.GetEnvironmentVariable("TRACING_AGENT_HOST");// ?? "localhost";
            var samplingRateParam = Environment.GetEnvironmentVariable("TRACING_RATE") ?? "100";
            int.TryParse(samplingRateParam, out var samplingRate);
            if (samplingRate > 100) samplingRate = 100;
            if (samplingRate < 0) samplingRate = 0;

            if (string.IsNullOrEmpty(jaegerAgentHost) || samplingRate == 0)
            {
                Console.WriteLine("Tracing is disabled (environments TRACING_AGENT_HOST, TRACING_RATE)");
                services.AddSingleton<ITracer>(new NoTracer());
                return;
            }

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = Assembly.GetEntryAssembly().GetName().Name;

                //ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                //var loggingReporter = new LoggingReporter(loggerFactory);

                var reporter = new RemoteReporter.Builder()
                    .WithSender(new UdpSender(jaegerAgentHost, 6831, 0))
                    .Build();

                //https://www.jaegertracing.io/docs/sampling/
                ISampler sampler = samplingRate == 100 
                    ? new ConstSampler(sample: true) as ISampler
                    : new ProbabilisticSampler(samplingRate/100) as ISampler;

                Logger().Information("Tracer use sampler {Sampler}", sampler.GetType().Name);

                ITracer tracer = new Tracer.Builder(serviceName)
                    //.WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .WithReporter(reporter)
                    //.WithReporter(new CompositeReporter(loggingReporter, reporter))
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            // Prevent endless loops when OpenTracing is tracking HTTP requests to Jaeger.
            services.Configure<HttpHandlerDiagnosticOptions>(options =>
            {
                options.IgnorePatterns.Clear();
                options.IgnorePatterns.Add(request => request.RequestUri.Port == 8086 && request.RequestUri.PathAndQuery.Contains("write?db=appmetrics"));
            });
                    
            // Enables OpenTracing instrumentation for ASP.NET Core, CoreFx, EF Core
            services.AddOpenTracing(builder =>
                {
                    builder.ConfigureAspNetCore(options =>
                    {
                        options.Hosting.ExtractEnabled = context => !isPublicWebService; // never trust TraceId from user web-requests
                        options.Hosting.OnRequest = (span, context) =>
                        {
                            span.SetTag("InstanceId", instanceInfo.Id.ToString());
                        };
                    });
                }
            );
        }
    }
}
