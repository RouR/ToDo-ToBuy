using System;
using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace CustomMetrics
{
    public static class SetupDefaultWebMetrics
    {
        private static bool isEnabled = false;
        public static void ConfigureServices(InstanceInfo instanceInfo, IServiceCollection services)
        {
            var influxDb = Environment.GetEnvironmentVariable("InfluxDB");// ?? "http://monitoring-influxdb.kube-system.svc:8086";

            if (string.IsNullOrEmpty(influxDb))
            {
                Console.WriteLine("WebMetrics is disabled (enviroment InfluxDB)");
                return;
            }

            isEnabled = true;

            Console.WriteLine("Write metrics to InfluxDB {0}", influxDb);
            var metricsBuilder = AppMetrics.CreateDefaultBuilder();
            var metrics = metricsBuilder
                .Report.ToInfluxDb(
                    options =>
                    {
                        options.InfluxDb.BaseUri = new Uri(influxDb);
                        options.InfluxDb.Database = "appmetrics";
                        //options.InfluxDb.Consistenency = "";
                        options.InfluxDb.UserName = "appmetrics";
                        options.InfluxDb.Password = "appmetrics";
                        //options.InfluxDb.RetensionPolicy = "default";
                        options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                        options.HttpPolicy.FailuresBeforeBackoff = 5;
                        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                        //options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                        //options.Filter = null;
                        options.FlushInterval = TimeSpan.FromSeconds(10);
                    })
                .Build();
            metricsBuilder.Configuration.Configure(
                options =>
                {
                    options.DefaultContextLabel = "WebFrontApp";
                    options.GlobalTags.Add("Instance", instanceInfo.Id.ToString());
                    options.GlobalTags.Add("CodeVer", instanceInfo.CodeVer);
                    options.Enabled = true;
                    options.ReportingEnabled = true;
                });

            services.AddMetrics(metricsBuilder);
            services.AddMetricsReportScheduler();
        }

        public static void Configure(IApplicationBuilder app)
        {
            if(isEnabled)
                app.UseMetricsAllMiddleware();
        }
    }
}
