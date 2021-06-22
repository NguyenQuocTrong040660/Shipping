using Microsoft.Extensions.DependencyInjection;

namespace Communication.Migration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMigrationServices(this IServiceCollection services)
        {
            services.AddScoped<ISeedCommunication, SeedCommunication>();
            return services;
        }
    }
}
