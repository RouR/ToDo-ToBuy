using System;
using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var JaegerAgentHost = Environment.GetEnvironmentVariable("TRACING_AGENT_HOST") ?? "localhost";

                string serviceName = Assembly.GetEntryAssembly().GetName().Name;

                //ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                //var loggingReporter = new LoggingReporter(loggerFactory);

                var reporter = new RemoteReporter.Builder()
                    .WithSender(new UdpSender(JaegerAgentHost, 6831, 0))
                    .Build();

                var sampler = new ConstSampler(sample: true);

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
                options.IgnorePatterns.Add(request => request.RequestUri.Port == 8086 && request.RequestUri.PathAndQuery.Contains("write?db=appmetrics"));
                options.IgnorePatterns.Add(request =>
                {
                    Logger().Information("request.RequestUri {@RequestUri}", request.RequestUri.PathAndQuery);
                    Logger().Information("request.RequestUri2 {@RequestUri2}", request.RequestUri);
                    Console.WriteLine($"request.RequestUri {request.RequestUri}");
                    return request.RequestUri.PathAndQuery.Contains("healthz");
                });
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
