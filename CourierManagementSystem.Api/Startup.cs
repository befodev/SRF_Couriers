using CourierManagementSystem.Api.Configuration;
using CourierManagementSystem.Api.Data;
using CourierManagementSystem.Api.Middleware;
using CourierManagementSystem.Api.Repositories;
using CourierManagementSystem.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CourierManagementSystem.Api
{
    public class Startup
    {
        private readonly WebApplicationBuilder _builder;


        public Startup(WebApplicationBuilder builder)
        {
            _builder = builder;
        }


        public WebApplication Initialize()
        {
            IServiceCollection services = _builder.Services;

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            ConfigureSwagger(services);
            ConfigureDatabase(services);
            ConfigureJwt(services);
            ConfigureCors(services);

            RegisterRepositories(services);
            RegisterServices(services);

            WebApplication app = InitializeWebApplication();
            return app;
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Courier Management System API",
                    Version = "v1",
                    Description = "API для управления курьерской доставкой"
                });

                // Add JWT authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string? connectionString = _builder.Configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                    options.UseInMemoryDatabase("CourierManagementDb");
                else
                {
                    if (connectionString.Contains("Host=") || connectionString.Contains("Server=") && connectionString.Contains("Port="))
                        options.UseNpgsql(connectionString);
                    else
                        options.UseSqlServer(connectionString);
                }
            });
        }

        private void ConfigureJwt(IServiceCollection services)
        {
            IConfigurationSection? jwtSettings = _builder.Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettings);

            string? jwtSecret = jwtSettings.Get<JwtSettings>()?.Secret ?? "your-super-secret-key-with-at-least-32-characters-for-security";
            string? jwtIssuer = jwtSettings.Get<JwtSettings>()?.Issuer ?? "CourierManagementSystem";
            string? jwtAudience = jwtSettings.Get<JwtSettings>()?.Audience ?? "CourierManagementSystemUsers";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IDeliveryPointRepository, DeliveryPointRepository>();
            services.AddScoped<IDeliveryPointProductRepository, DeliveryPointProductRepository>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<ICourierService, CourierService>();
            services.AddScoped<IRouteService, RouteService>();
        }

        private WebApplication InitializeWebApplication()
        {
            _builder.Services.AddHttpClient<IOpenStreetMapService, OpenStreetMapService>();
            WebApplication app = _builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Courier Management System API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
