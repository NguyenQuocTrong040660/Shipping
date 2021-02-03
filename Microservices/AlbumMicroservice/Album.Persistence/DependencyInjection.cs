using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Album.Persistence.DBContext;
using Album.Persistence.Mapping;
using Album.Application.Common.Interfaces;

namespace Album.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AlbumDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AlbumDbContext).Assembly.FullName)));

            services.AddScoped<IAlbumDbContext>(provider => provider.GetService<AlbumDbContext>());

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new NewMappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}

