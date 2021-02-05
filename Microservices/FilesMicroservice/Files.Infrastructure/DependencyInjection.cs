using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Files.Application.Common.Interfaces;
using Files.Infrastructure.Services;

namespace Files.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUploadFileService, UploadFileService>();
            return services;
        }
    }
}
