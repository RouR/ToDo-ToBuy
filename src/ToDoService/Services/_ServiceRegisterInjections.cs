using Microsoft.Extensions.DependencyInjection;
using ToDoService.Interfaces;

namespace ToDoService.Services
{
    public static class _ServiceRegisterInjections
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<ITodoService, TodoService>();
        }
    }
}
