using System;
using System.Collections.Generic;
using System.Text;
using CustomCache;
using CustomCache.Utils;
using CustomMetrics;
using CustomTracing;
using DTO;
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
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

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
            ServiceClients.ConfigureServices(services, CustomLogs.SetupCustomLogs.Logger());
            SetupCustomCache.ConfigureServices(services, out var redisCacheOptions);

            CustomLogs.SetupCustomLogs.PrintAllEnv();

            //https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = false;
                options.StackBlockedRequests = false;
                //The RealIpHeader is used to extract the client IP when your Kestrel server is behind a reverse proxy, if your proxy uses a different header then X-Real-IP use this option to set it up.
                options.RealIpHeader = "X-Real-IP";
                //The ClientIdHeader is used to extract the client id for white listing, if a client id is present in this header and matches a value specified in ClientWhitelist then no rate limits are applied.
                options.ClientIdHeader = "X-ClientId";
                options.HttpStatusCode = 429;
                options.IpWhitelist = new List<string>()
                {
                    /*"127.0.0.1", "::1/10", "192.168.0.0/24" */
                };
                options.EndpointWhitelist = new List<string>()
                {
                    /*"get:/api/license", "*:/api/status" */
                };
                options.ClientWhitelist = new List<string>()
                {
                    /*"dev-id-1", "dev-id-2" */
                };
                options.GeneralRules = new List<RateLimitRule>()
                {
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Period = "10s",
                        //runtime exception PeriodTimespan = TimeSpan.FromSeconds(10),
                        Limit = 2
                    }
                };
                options.DisableRateLimitHeaders = true;
                options.RateLimitCounterPrefix = "web_throttle_";
            });
            services.Configure<IpRateLimitPolicies>(options =>
            {
                options.IpRules = new List<IpRateLimitPolicy>()
                {
                    /*
                    new IpRateLimitPolicy()
                    {
                        //like "192.168.0.0/24", "fe80::/10" or "192.168.0.0-192.168.0.255".
                        Ip =  "192.168.3.22/25",
                        Rules = new List<RateLimitRule>()
                        {
                            new RateLimitRule()
                            {
                                Endpoint = "*",
                                PeriodTimespan = TimeSpan.FromSeconds(2),
                                Limit = 2
                            }
                        }
                    }
                    */
                };
            });
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = !string.IsNullOrWhiteSpace(Settings.JwtIssuer),
                        ValidateAudience = !string.IsNullOrWhiteSpace(Settings.JwtAudience),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Settings.JwtIssuer,
                        ValidAudience = Settings.JwtAudience,
                        IssuerSigningKey = Settings.JwtSigningKey
                    };
                });
            
            // By default, ASP.NET Core application will reject any request coming from the cross-origin clients. 
            services.AddCors();

            services.AddMvc(options => {
                options.Filters.Add(typeof(GlobalValidatorAttribute));
                options.MaxModelValidationErrors = 10;
            });

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
                    options.IncludeXmlComments(SwaggerUtils
                        .XmlCommentsFilePath); //check project properties - add xml docs to bin\Debug\netcoreapp2.0\Web.xml
                });

            services.AddHealthChecks(checks =>
            {
                //However, the MVC web application has multiple dependencies on the rest of the microservices. Therefore, it calls one AddUrlCheck method for each microservice
                //checks.AddSqlCheck("CatalogDb", Configuration["ConnectionString"]);

                checks.AddUrlCheck(ServiceClients.HealthUrl(Service.Account), TimeSpan.FromSeconds(1));
                checks.AddUrlCheck(ServiceClients.HealthUrl(Service.ToDo), TimeSpan.FromSeconds(1));
                checks.AddUrlCheck(ServiceClients.HealthUrl(Service.ToBuy), TimeSpan.FromSeconds(1));

                checks.AddRedisCheck(redisCacheOptions, TimeSpan.FromSeconds(1));

                //If the microservice does not have a dependency on a service or on SQL Server, you should just add a Healthy("Ok") check.
                //checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });
        }


        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider,
            IHostingEnvironment env
        )
        {
            //var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            //configuration.DisableTelemetry = true;

            CustomLogs.SetupCustomLogs.Configure(loggerFactory, applicationLifetime);
            SetupDefaultWebMetrics.Configure(app);

            var cachePeriod = env.IsDevelopment() ? 0 : 10 /*d*/ * 24 /*h*/ * 60 /*m*/ * 60 /*s*/;
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    if (cachePeriod > 0)
                        ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                    else
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", $" no-cache, no-store, must-revalidate ");
                    }
                }
            });

            //todo: check headers not in minikube, with real ingress
            //app.UseIpRateLimiting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
            app.UseAuthentication();
            
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
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}