using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Album.Migration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMigrationServices(this IServiceCollection services)
        {
            services.AddScoped<ISeedSampleData, SeedSampleData>();
            return services;
        }
    }
}
