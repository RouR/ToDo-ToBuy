using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Web.Utils;
using Microsoft.Extensions.HealthChecks;

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

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                // server is not going to shutdown
                // until the callback is done
                Console.WriteLine("gracefull shutdown");
            });
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHealthChecks(checks =>
            {
                //However, the MVC web application has multiple dependencies on the rest of the microservices. Therefore, it calls one AddUrlCheck method for each microservice
                //checks.AddSqlCheck("CatalogDb", Configuration["ConnectionString"]);
                //checks.AddUrlCheck(Configuration["CatalogUrl"]);
                //checks.AddUrlCheck(Configuration["OrderingUrl"]);

                //If the microservice does not have a dependency on a service or on SQL Server, you should just add a Healthy("Ok") check.
                checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });


            services.AddSingleton<InstanceInfo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            //configuration.DisableTelemetry = true;


            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {area="home", controller = "Hello", action = "Index" });
            });
        }
    }
}
