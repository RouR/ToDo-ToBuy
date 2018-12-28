using AccountService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Services
{
    public static class _ServiceRegisterInjections
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
