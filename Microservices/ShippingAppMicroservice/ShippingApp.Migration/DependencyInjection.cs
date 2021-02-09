using Microsoft.Extensions.DependencyInjection;

namespace ShippingApp.Migration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMigrationServices(this IServiceCollection services)
        {
            services.AddScoped<ISeedShippingApp, SeedShippingApp>();
            return services;
        }
    }
}
