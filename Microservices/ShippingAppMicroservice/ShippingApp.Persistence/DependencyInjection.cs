using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Persistence.DBContext;
using ShippingApp.Persistence.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

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

            var mapperConfig = new MapperConfiguration(mc =>
            {
                //mc.AddProfile(new MappingProfile());
                mc.AddProfile(new ProductTypeProfile());
                mc.AddProfile(new CountryProfile());
                mc.AddProfile(new BrandProfile());
                mc.AddProfile(new ProductOverviewProfile());
                mc.AddProfile(new ReservationProfile());
                mc.AddProfile(new MemberShipProfile());
                mc.AddProfile(new PromotionProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
