using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingApp.Domain.AppSetting;
using ShippingApp.Application.Interfaces;
using ShippingApp.Infrastructure.Repositories;

namespace ShippingApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IShippingAppRepository<>), typeof(ShippingAppRepository<>));

            var emailConfiguration = configuration.GetSection("EmailConfiguration");
            services.Configure<EmailConfiguration>(emailConfiguration);

            return services;
        }
    }
}
