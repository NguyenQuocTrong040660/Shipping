using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
                                        //LOCAL
                                        "http://localhost:4200/",
                                        "https://localhost:4200/",

                                        //MAIN PAGE
                                        "http://shippingapp.spartronics.com/",
                                        "https://shippingapp.spartronics.com/",
                                        "http://www.spartronics.com.vn/",
                                        "https://www.spartronics.com.vn/",

                                        //USER
                                        "http://api-user.spartronics.com.vn/",
                                        "https://api-user.spartronics.com.vn/",
                                        "http://www.api-user.spartronics.com.vn/",
                                        "https://www.api-user.spartronics.com.vn/",

                                        //Shipping Application
                                        "http://api-shippingapp.spartronics.com.vn/",
                                        "https://api-shippingapp.spartronics.com.vn/",
                                        "http://www.api-shippingapp.spartronics.com.vn/",
                                        "https://www.api-shippingapp.spartronics.com.vn/"

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

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

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


            ConfigureAuthenticationServices(services);
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
            //if (!env.IsProduction())
            //{
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseOpenApi();
                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "GatewayApi API V1");
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                });
            //}

            loggerfactory.AddSerilog();
            app.UseSerilogRequestLogging();
            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("GREXSOLUTIONS");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOcelot().Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
