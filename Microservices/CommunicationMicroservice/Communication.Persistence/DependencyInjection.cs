using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Communication.Application.Interfaces;
using Communication.Persistence.DBContext;

namespace Communication.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CommunicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CommunicationDatabase"),
                b => b.MigrationsAssembly(typeof(CommunicationDbContext).Assembly.FullName)));

            services.AddScoped<ICommunicationDbContext>(provider => provider.GetService<CommunicationDbContext>());
            return services;
        }
    }
}
