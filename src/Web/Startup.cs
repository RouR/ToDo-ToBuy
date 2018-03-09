using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Utils;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Fluentd;

namespace Web
{
    /*
     * Install-Package Microsoft.AspNetCore.All     
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;

            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine("SeriLog" + msg));

            var fluentdHost = Environment.GetEnvironmentVariable("FluentD_Host") ?? "localhost";
            int.TryParse(Environment.GetEnvironmentVariable("FluentD_Port") ?? "24224", out var fluentdPort);
            Console.WriteLine("Write logs to fluentd {0}:{1}", fluentdHost, fluentdPort);
            Log.Information("Write logs to fluentd {@fluentdHost}:{@fluentdPort}", fluentdHost, fluentdPort);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
#if false //DEBUG
                .WriteTo.Console()
#endif
                .WriteTo.Fluentd(
                    new FluentdSinkOptions(fluentdHost, fluentdPort) //see fluentd-config
                    {
                        Tag = "WebServer",
                    }
                )
                .CreateLogger();

            Log.Information("Starting...");
           
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var influxDb = Environment.GetEnvironmentVariable("InfluxDB") ?? "http://monitoring-influxdb.kube-system:8086";
            Console.WriteLine("Write metrics to InfluxDB {0}", influxDb);
            Log.Information("Write metrics to InfluxDB {@influxDB}", influxDb);

            var metrics = AppMetrics.CreateDefaultBuilder()
                .Report.ToInfluxDb(
                    options =>
                    {
                        options.InfluxDb.BaseUri = new Uri(influxDb);
                        options.InfluxDb.Database = "appmetrics";
                        //options.InfluxDb.Consistenency = "consistency";
                        options.InfluxDb.UserName = "appmetrics";
                        options.InfluxDb.Password = "appmetrics";
                        options.InfluxDb.RetensionPolicy = "default";
                        options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                        options.HttpPolicy.FailuresBeforeBackoff = 5;
                        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                        options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                        //options.Filter = null;
                        options.FlushInterval = TimeSpan.FromSeconds(5);
                    })
                .Build();

            services.AddMetrics(metrics);
            services.AddMetricsReportScheduler();

            services.AddMvc( /*options => options.AddMetricsResourceFilter()*/);

            services.AddHealthChecks(checks =>
            {
                //However, the MVC web application has multiple dependencies on the rest of the microservices. Therefore, it calls one AddUrlCheck method for each microservice
                //checks.AddSqlCheck("CatalogDb", Configuration["ConnectionString"]);
                //checks.AddUrlCheck(Configuration["CatalogUrl"]);
                //checks.AddUrlCheck(Configuration["OrderingUrl"]);

                //If the microservice does not have a dependency on a service or on SQL Server, you should just add a Healthy("Ok") check.
                checks.AddValueTaskCheck("HTTP Endpoint",
                    () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });

            services.AddSingleton<InstanceInfo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory, InstanceInfo instanceInfo)
        {
            //var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            //configuration.DisableTelemetry = true;


            loggerFactory.AddSerilog();

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                var info = new { Action = "ApplicationStarted", InstanceInfo = instanceInfo, DebugMode = isDebug };
                Log.Information("Application Started {@info}", info);
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                // server is not going to shutdown
                // until the callback is done
                //Console.WriteLine("gracefull shutdown");
                var info = new { Action = "ApplicationStopping", InstanceInfo = instanceInfo };
                Log.Information("Gracefull shutdown {@info}", info);
            });


            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {area = "home", controller = "Hello", action = "Index"});
            });
        }
    }
}