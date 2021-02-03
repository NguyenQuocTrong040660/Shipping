using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MMLib.Ocelot.Provider.AppConfiguration;
using NSwag;
using NSwag.Generation.Processors.Security;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System.Linq;
using System.Text;

namespace APIGateway
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
            services.AddOcelot().AddAppConfiguration();
            services.AddSwaggerForOcelot(Configuration);

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "gatewayapi API";

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
            ConfigureAuthenticationServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "GREXSOLUTIONS",
                builder =>
                {
                    builder.WithOrigins(
                                        //MAIN PAGE
                                        "http://shippingapp.spatronics.com/",
                                        "https://shippingapp.spatronics.com/",
                                        "http://www.shippingapp.spatronics.com/",
                                        "https://www.shippingapp.spatronics.com/",

                                        //USER
                                        "http://api-user.shippingapp.spatronics.com/",
                                        "https://api-user.shippingapp.spatronics.com/",
                                        "http://www.api-user.shippingapp.spatronics.com/",
                                        "https://www.api-user.shippingapp.spatronics.com/",

                                         //Shipping Application
                                        "http://api-shippingapp.shippingapp.spatronics.com/",
                                        "https://api-shippingapp.shippingapp.spatronics.com/",
                                        "http://www.api-shippingapp.shippingapp.spatronics.com/",
                                        "https://www.api-shippingapp.shippingapp.spatronics.com/"

                                        )
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin();
                });
            });
        }

        private void ConfigureAuthenticationServices(IServiceCollection services)
        {
            string secret = Configuration["JWTTokenConfig:Secret"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerfactory)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseOpenApi();
                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "gatewayapi API V1");
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            loggerfactory.AddSerilog();
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("GREXSOLUTIONS");

            app.UseAuthentication();
            app.UseOcelot().Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
