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
            //var jaegerUri = Environment.GetEnvironmentVariable("JaegerUri") ?? "http://localhost:14268/api/traces";

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                string serviceName = Assembly.GetEntryAssembly().GetName().Name;

                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var loggingReporter = new LoggingReporter(loggerFactory);

                var reporter = new RemoteReporter.Builder()
                    .WithSender(new UdpSender())
                    .Build();

                var sampler = new ConstSampler(sample: true);

                Logger().Information("Tracer use sampler {Sampler}", sampler.GetType().Name);

                ITracer tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    //.WithReporter(reporter)
                    .WithReporter(new CompositeReporter(loggingReporter, reporter))
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            // Prevent endless loops when OpenTracing is tracking HTTP requests to Jaeger.
            services.Configure<HttpHandlerDiagnosticOptions>(options =>
            {
                //options.IgnorePatterns.Add(request => jaegerUri.IsBaseOf(request.RequestUri));
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
