using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Album.Application.Common.Interfaces;
using Album.Infrastructure.Services;

namespace Album.Infrastructure
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
