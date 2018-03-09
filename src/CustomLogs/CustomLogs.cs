#define ISSUE_NOT_SOLVED //todo [Serilog.Sinks.Fluentd] Connection exception Connection refused 127.0.0.1:24224
using System;
using App.Metrics.Internal;
using App.Metrics.Logging;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Shared;
using ILogger = Serilog.ILogger;

namespace CustomLogs
{
    /// <summary>
    /// https://marketplace.visualstudio.com/items?itemName=Suchiman.SerilogAnalyzer
    /// https://github.com/serilog/serilog/wiki/Writing-Log-Events
    /// </summary>
    public static class CustomLogs
    {
        public static void ConfigureStartup()
        {
            // ReSharper disable once ConvertClosureToMethodGroup
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            var fluentdHost = Environment.GetEnvironmentVariable("FluentD_Host") ?? "localhost";
            int.TryParse(Environment.GetEnvironmentVariable("FluentD_Port") ?? "24224", out var fluentdPort);
            Console.WriteLine("Write logs to fluentd {0}:{1}", fluentdHost, fluentdPort);
            Log.Information("Write logs to fluentd {FluentdHost}:{FluentdPort}", fluentdHost, fluentdPort);

            // or: services.AddSingleton<Serilog.ILogger>(logger);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override(typeof(DefaultMeterTickerScheduler).FullName, LogEventLevel.Information)
                .MinimumLevel.Override(typeof(DefaultMetricsRegistry).FullName, LogEventLevel.Information)
                .MinimumLevel.Override(typeof(DefaultReservoirRescaleScheduler).FullName, LogEventLevel.Information)
                .MinimumLevel.Override(typeof(DefaultForwardDecayingReservoir).FullName, LogEventLevel.Information)
                .Enrich.FromLogContext() //LogContext.PushProperty("A", 1)
#if ISSUE_NOT_SOLVED
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                //.WriteTo.RollingFile(new RenderedCompactJsonFormatter(), @"Logs/{Date}.json", retainedFileCountLimit: 10)
#else
                .WriteTo.Fluentd(
                    new FluentdSinkOptions(fluentdHost, fluentdPort) //see fluentd-config
                    {
                        Tag = "WebServer",
                    }
                )
#endif
                .CreateLogger();

#if DEBUG
            if (LogProvider.For<DefaultMeterTickerScheduler>().IsDebugEnabled())
                throw new Exception("Too many logs. Log configuration was failed");
#endif
        }


        public static void Configure(ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddSerilog();

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var isDebug = false;
#if DEBUG
                isDebug = true;
#endif

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                var info = new {Action = "ApplicationStarted", DebugMode = isDebug};
                Logger().Information("Application Started {@Info}", info);
                Log.Information("Application Started {@Info} {@Instance}", info, InstanceInfo);
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                // server is not going to shutdown
                // until the callback is done
                //Console.WriteLine("gracefull shutdown");
                var info = new {Action = "ApplicationStopping"};
                Log.Information("Application Stopping {@Info} {@Instance}", info, InstanceInfo);
            });
        }

        public static InstanceInfo InstanceInfo;


        /// <summary>
        /// using static CustomLogs.CustomLogs;
        /// </summary>
        /// <returns></returns>
        public static ILogger Logger()
        {
            //use RenderedCompactJsonFormatter to Save all context properties
            return Log.Logger.ForContext("InstanceId", InstanceInfo.Id);
        }
    }
}