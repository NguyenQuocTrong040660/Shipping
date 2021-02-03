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
            //services.AddDbContext<ProductDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("ShippingAppDatabase")));
            services.AddScoped<IShippingAppDbContext, ShippingAppDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();

            var emailConfiguration = configuration.GetSection("EmailConfiguration");
            services.Configure<EmailConfiguration>(emailConfiguration);

            return services;
        }
    }
}
