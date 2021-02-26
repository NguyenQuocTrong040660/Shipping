using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Communication.Domain.AppSetting;

namespace Communication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfiguration = configuration.GetSection("EmailConfiguration");
            services.Configure<EmailConfiguration>(emailConfiguration);

            return services;
        }
    }
}
