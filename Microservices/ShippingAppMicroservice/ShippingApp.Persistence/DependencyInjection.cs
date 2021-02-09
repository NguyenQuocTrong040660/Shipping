using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingApp.Application.Interfaces;
using ShippingApp.Persistence.DBContext;

namespace ShippingApp.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShippingAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ShippingAppDatabase"),
                b => b.MigrationsAssembly(typeof(ShippingAppDbContext).Assembly.FullName)));

            services.AddScoped<IShippingAppDbContext>(provider => provider.GetService<ShippingAppDbContext>());
            return services;
        }
    }
}
