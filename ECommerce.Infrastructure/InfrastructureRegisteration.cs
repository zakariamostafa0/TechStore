using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection AddInfrastructureConfigration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}
