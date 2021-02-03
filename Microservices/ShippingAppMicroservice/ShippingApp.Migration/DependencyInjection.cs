using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Migration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMigrationServices(this IServiceCollection services)
        {
            services.AddScoped<ISeedShippingApp, SeedShippingApp>();
            return services;
        }
    }
}
