using Microsoft.Extensions.DependencyInjection;
using ToBuyService.Interfaces;

namespace ToBuyService.Services
{
    public static class _ServiceRegisterInjections
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<ITobuyService, TobuyService>();
        }
    }
}
