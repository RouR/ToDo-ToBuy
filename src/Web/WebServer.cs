using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Fluentd;

namespace Web
{
    public static class WebServer
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseHealthChecks("/healthz") //nginx-ingress require this path
                .UseUrls("http://0.0.0.0:5555") // Take that 0.0.0.0 instead of localhost, Docker port forwarding!!!
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //var env = hostingContext.HostingEnvironment;
                    //config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    //config.AddEnvironmentVariables();

                    var conf = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
#if DEBUG
                        .MinimumLevel.Debug()
                        //.WriteTo.Console()
#else
                        .MinimumLevel.Information()
#endif
                        .WriteTo.Fluentd(new FluentdSinkOptions("localhost", 24224)
                        {
                            Tag = "WebServer",
                        });
                    Log.Logger = conf.CreateLogger();
#if DEBUG
                    var info = new { Action = "Startup", Method = "Configure", Configuration = conf };
                    Log.Logger.Debug("Logger {@info}", info);
#endif
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddSerilog(dispose: true);
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureMetricsWithDefaults(builder =>
                {
                    builder
                        .Report.ToConsole(TimeSpan.FromSeconds(2))
                        .Report.ToInfluxDb(
                            options =>
                            {
                                //https://grafana.com/dashboards?search=app%20metrics
                                options.InfluxDb.BaseUri = new Uri("http://192.168.99.100:8083/");
                                //options.InfluxDb.BaseUri = new Uri("http://monitoring-influxdb.kube-system.svc:8086");
                                options.InfluxDb.Database = "appmetrics";
                                //options.InfluxDb.Consistenency = "consistency";
                                //options.InfluxDb.UserName = "appm";
                                //options.InfluxDb.Password = "appm";
                                //options.InfluxDb.RetensionPolicy = "rp";
                                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                                options.HttpPolicy.FailuresBeforeBackoff = 5;
                                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                                //options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                                //options.Filter = filter;
                                //options.FlushInterval = TimeSpan.FromSeconds(1);
                            })
                            ;
                })
                .UseMetrics()
                //.UseMetricsWebTracking()
                .Build();
    }
}