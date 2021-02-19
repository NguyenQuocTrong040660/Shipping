using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Files.Application;
using Files.Infrastructure;
using Files.Persistence;
using Serilog;
using Files.Api.Configs;
using Files.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Files.Api.Services;

namespace Files.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "GREXSOLUTIONS",
                                  builder =>
                                  {
                                      builder.WithOrigins(
                                                "http://api-gatewayapi.spatronics.com.vn/",
                                                "https://api-gatewayapi.spatronics.com.vn/",
                                                "http://www.api-gatewayapi.spatronics.com.vn/",
                                                "https://www.api-gatewayapi.spatronics.com.vn/"
                                            )
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowAnyOrigin();
                                  });
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //Disabled in PROD
            //if (!Environment.IsProduction())
            //{
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Files API", Version = "v1" });
                    c.IgnoreObsoleteActions();
                    c.IgnoreObsoleteProperties();
                    c.MapType<FileContentResult>(() => new OpenApiSchema
                    {
                        Type = "file"
                    });
                });
            //}

            services.AddControllers();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddSingleton<IEnvironmentApplication, EnvironmentApplication>();
            services.AddPersistence(Configuration);
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            ConfigureAuthenticationServices(services);
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

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
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Files API V1");
                });
            //}

            loggerfactory.AddSerilog();
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseCors("GREXSOLUTIONS");

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
