using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMLib.Ocelot.Provider.AppConfiguration;
using NSwag;
using NSwag.Generation.Processors.Security;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System.Linq;

namespace ApiGatewayManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddOcelot().AddAppConfiguration();
            services.AddSwaggerForOcelot(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "GREXSOLUTIONS",
                builder =>
                {
                    builder.WithOrigins(
                                //MAIN PAGE
                                "http://shippingapp.spartronics.com",
                                "https://shippingapp.spartronics.com",
                                "http://www.shippingapp.spartronics.com",
                                "https://www.shippingapp.spartronics.com",
                                "http://shippingapp.spartronics.com:80",
                                "https://shippingapp.spartronics.com:80",
                                "http://www.shippingapp.spartronics.com:80",
                                "https://www.shippingapp.spartronics.com:80",
                                "http://vtnportal.spartronics.com:8008",
                                "https://vtnportal.spartronics.com:8008",
                                "http://www.vtnportal.spartronics.com:8008",
                                "https://www.vtnportal.spartronics.com:8008"
                                )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                });
            });

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "GatewayApi API";

                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}.",
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerfactory)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "GatewayApi API V1");
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            loggerfactory.AddSerilog();
            app.UseSerilogRequestLogging();
            app.UseHealthChecks("/health");

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("GREXSOLUTIONS");

            app.UseOcelot().Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
