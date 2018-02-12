using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Web.Utils;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Web
{
    /*
     * Install-Package Microsoft.AspNetCore.All     
     */
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMetrics();

            services.AddMvc();

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

            //var metrics = AppMetrics.CreateDefaultBuilder()
            //    //.Configuration.Configure(
            //    //    options =>
            //    //    {
            //    //        //options.AddServerTag();
            //    //        options.GlobalTags.Add("myTagKey", "myTagValue");
            //    //    })
            //    .Report.ToConsole(TimeSpan.FromSeconds(6))
            //    .Report.ToInfluxDb(
            //        options => {
            //            //https://grafana.com/dashboards?search=app%20metrics
            //            options.InfluxDb.BaseUri = new Uri("http://192.168.99.100:8083/");
            //            //options.InfluxDb.BaseUri = new Uri("http://monitoring-influxdb.kube-system.svc:8086");
            //            options.InfluxDb.Database = "app-metrics";
            //            options.InfluxDb.Consistenency = "consistency";
            //            options.InfluxDb.UserName = "appm";
            //            options.InfluxDb.Password = "appm";
            //            options.InfluxDb.RetensionPolicy = "rp";
            //            options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
            //            options.HttpPolicy.FailuresBeforeBackoff = 5;
            //            options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
            //            options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
            //            //options.Filter = filter;
            //            options.FlushInterval = TimeSpan.FromSeconds(1);
            //        })
            //    .Build();

            //services.AddMetrics(metrics);
            //services.AddMetricsTrackingMiddleware();
            

            services.AddSingleton<InstanceInfo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            //var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            //configuration.DisableTelemetry = true;

            loggerFactory.AddSerilog();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                // server is not going to shutdown
                // until the callback is done
                Console.WriteLine("gracefull shutdown");
                Log.Logger.Information("Gracefull shutdown.");
            });

            //app.UseMetricsAllMiddleware();

            // Or to cherry-pick the tracking of interest
            // app.UseMetricsActiveRequestMiddleware();
            // app.UseMetricsErrorTrackingMiddleware();
            // app.UseMetricsPostAndPutSizeTrackingMiddleware();
            // app.UseMetricsRequestTrackingMiddleware();
            // app.UseMetricsOAuth2TrackingMiddleware();
            // app.UseMetricsApdexTrackingMiddleware();

            app.UseMiddleware<LogMiddleware>();
            
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