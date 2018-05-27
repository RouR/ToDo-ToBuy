using System.Threading.Tasks;
using CustomMetrics;
using CustomTracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Shared;
using Web.Utils;

namespace Web
{
    public class Startup
    {
        //public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;

            CustomLogs.SetupCustomLogs.ConfigureStartup();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var instanceInfo = new InstanceInfo();
            services.AddSingleton(instanceInfo);
            CustomLogs.SetupCustomLogs.ConfigureServices(instanceInfo);
            SetupDefaultWebMetrics.ConfigureServices(instanceInfo, services);
            SetupTracing.ConfigureServices(instanceInfo, services, true);

            CustomLogs.SetupCustomLogs.PrintAllEnv();

            services.AddMvc();


            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddMvcCore().AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddApiVersioning(options =>
                {
                    //https://github.com/Microsoft/aspnet-api-versioning/wiki/New-Services-Quick-Start#aspnet-core
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(0, 1);
                    options.ReportApiVersions = true; 
                }
            );
            
            services.AddSwaggerGen(
                options =>
                {
                    // resolve the IApiVersionDescriptionProvider service
                    // note: that we have to build a temporary service provider here because one has not been created yet
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, SwaggerUtils.CreateInfoForApiVersion(description));
                    }

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(SwaggerUtils.XmlCommentsFilePath); //check project properties - add xml docs to bin\Debug\netcoreapp2.0\Web.xml
                });

            services.AddHealthChecks(checks =>
            {
                //However, the MVC web application has multiple dependencies on the rest of the microservices. Therefore, it calls one AddUrlCheck method for each microservice
                //checks.AddSqlCheck("CatalogDb", Configuration["ConnectionString"]);
                //checks.AddUrlCheck(Configuration["CatalogUrl"]);

                //If the microservice does not have a dependency on a service or on SQL Server, you should just add a Healthy("Ok") check.
                checks.AddValueTaskCheck("HTTP Endpoint",
                    () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });
        }


        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider
            )
        {
            //var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            //configuration.DisableTelemetry = true;

            CustomLogs.SetupCustomLogs.Configure(loggerFactory, applicationLifetime);
            SetupDefaultWebMetrics.Configure(app);

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

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
        }


       
    }
}