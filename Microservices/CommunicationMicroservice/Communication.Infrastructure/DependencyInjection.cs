using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Communication.Domain.AppSetting;
using Communication.Application.Interfaces;
using Communication.Infrastructure.Repositories;

namespace Communication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(ICommunicationRepository<>), typeof(CommunicationRepository<>));

            var emailConfiguration = configuration.GetSection("EmailConfiguration");
            services.Configure<EmailConfiguration>(emailConfiguration);

            return services;
        }
    }
}
