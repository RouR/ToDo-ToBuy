using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Shared
{
    public static class KeyHeaderChecker
    {
        private static string _key;
        
        internal const string Header = "X-Api-Key";

        public static void SetApiKey(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            
            _key = key.Trim();
        }
        
        public static void ApiKeyMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                //Allow TestController
                if (!context.Request.Path.StartsWithSegments(new PathString("/test")))
                {
                    var headerKey = context.Request.Headers[Header].FirstOrDefault();
                    await ValidateApiKey(context, next, headerKey);
                }
                else
                {
                    await next();
                }
            });
        }

        private static async Task ValidateApiKey(HttpContext context, Func<Task> next, string key)
        {
            var valid = _key != null && key?.Trim() == _key;
            
            if (!valid)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
            }
            else
            {
                await next();
            }
        }
    }
}