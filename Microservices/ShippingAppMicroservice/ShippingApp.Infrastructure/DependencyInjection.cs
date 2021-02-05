using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingApp.Domain.AppSetting;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Infrastructure.Repositories;
using ShippingApp.Persistence.DBContext;

namespace ShippingApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShippingAppDbContext, ShippingAppDbContext>();
            services.AddScoped<IShippingAppRepository, ShippingAppRepository>();

            var emailConfiguration = configuration.GetSection("EmailConfiguration");
            services.Configure<EmailConfiguration>(emailConfiguration);

            return services;
        }
    }
}
