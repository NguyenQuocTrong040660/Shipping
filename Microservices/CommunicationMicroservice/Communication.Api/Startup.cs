using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Communication.Api.Filters;
using Communication.Application;
using Communication.Application.Interfaces;
using Communication.Infrastructure;
using Communication.Infrastructure.Services;
using Communication.Migration;
using Communication.Persistence;
using System;
using System.Linq;
using System.Text;
using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;

namespace Communication.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistence(Configuration);
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddControllers(options =>
              options.Filters.Add(new ApiExceptionFilterAttribute())).AddFluentValidation();

            //Should be disabled in PROD
            services.AddMigrationServices();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHealthChecks();
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Communication Microservice API";

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
                                      builder.WithOrigins()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowAnyOrigin();
                                  });
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        private void ConfigureAuthenticationServices(IServiceCollection services)
        {
            string secret = Configuration["JWTTokenConfig:Secret"];
            string issuer = Configuration["JWTTokenConfig:Issuer"];
            string audience = Configuration["JWTTokenConfig:Audience"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidAudience = audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerfactory)
        {
            //if (!env.IsProduction())
            //{
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Communication Microservice API V1");
                });
            //}

            loggerfactory.AddSerilog();
            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("GREXSOLUTIONS");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
