using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Communication.Persistence;
using Communication.Persistence.DBContext;

namespace Communication.Api
{
    public class Program
    {
        //public async static Task Main(string[] args)
        //{
        //    var host = CreateHostBuilder(args).Build();

        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;
        //        var hostingEnvironment = services.GetService<IWebHostEnvironment>();
        //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        //        Log.Logger = new LoggerConfiguration()
        //            .MinimumLevel.Debug()
        //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //            .Enrich.FromLogContext()
        //            .WriteTo.Console()
        //            .WriteTo.RollingFile(Path.Combine($"{hostingEnvironment.ContentRootPath}\\logs", "log-{Date}.txt"))
        //            .CreateLogger();

        //        try
        //        {
        //            var context = services.GetRequiredService<CommunicationDbContext>();

        //            if (context.Database.IsSqlServer())
        //            {
        //                context.Database.Migrate();
        //            }

        //            await CommunicationDbContextSeed.SeedConfiguration(context);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        //            throw;
        //        }
        //    }

        //    await host.RunAsync();
        //}

        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    //var context = services.GetRequiredService<CommunicationDbContext>();

                    //if (context.Database.IsSqlServer())
                    //{
                    //    context.Database.Migrate();
                    //}

                    //await CommunicationDbContextSeed.SeedConfiguration(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                }
            }

            await host.RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((context, options) =>
                {
                    options
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(
                    webBuilder => webBuilder.UseIISIntegration()
                    .UseWebRoot("wwwroot")
                    .UseStartup<Startup>()
                );
        }
    }
}
