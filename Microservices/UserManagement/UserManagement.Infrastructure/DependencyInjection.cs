using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Common;
using UserManagement.Infrastructure.Identity;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Services;

namespace UserManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("UserManagementDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.SignIn.RequireConfirmedAccount = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(36500);
                options.Lockout.MaxFailedAccessAttempts = 10;

            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddHostedService<JwtRefreshTokenCache>();
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

            return services;
        }
    }
}
