#region Usings

using Application;
using Application.Services;
using Domain.Settings;
using Hangfire;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Health;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Identities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presentation.Abstraction;
using Presentation.OpenApiTransformers;
using System.Text;
using System.Threading.RateLimiting;

#endregion

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddInfrastructureDependencies(configuration);
        services.AddApplicationDependencies();
        services.AddAuthConfig(configuration);
        services.AddHangfireConfig(configuration);
        services.AddHttpContextAccessor();
        services.AddCORSConfig(configuration);
        services.AddRateLimiterConfig();
        services.AddHealthCheckConfig(configuration);

        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IDeliveryManService, DeliveryManService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderManagementService, OrderManagementService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserManagementService, UserManagementService>();

        services
            .AddEndpointsApiExplorer()
            .AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        return services;
    }

    #region Auth

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        #region Jwt

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        #endregion

        #region Roles

        //services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        #endregion

        #region Add Identity

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        #endregion

        #region Validations

        var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings?.Key!)),
                    ValidIssuer = settings?.Issuer,
                    ValidAudience = settings?.Audience
                };
            });

        #endregion

        #region Identity Configurations

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(24);
        });

        #endregion

        #region Service Lifetime

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISignInService, SignInService>();

        #endregion

        return services;
    }

    #endregion

    #region Hangfire

    private static IServiceCollection AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJobManager, JobManager>();

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))
        );

        services.AddHangfireServer();

        return services;
    }

    #endregion

    #region CORS

    private static IServiceCollection AddCORSConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!);
            });
        });

        return services;
    }

    #endregion

    #region RateLimiter

    private static IServiceCollection AddRateLimiterConfig(this IServiceCollection services)
    {
        services.AddOptions<RateLimitingOptions>()
            .BindConfiguration(nameof(RateLimitingOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<IOptions<RateLimitingOptions>>().Value;

        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            #region IpLimit

            rateLimiterOptions.AddPolicy(RateLimitingOptions.PolicyNames.IpLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.IpPolicy.PermitLimit,
                        Window = TimeSpan.FromSeconds(settings.IpPolicy.WindowInSeconds)
                    }
                )
            );

            #endregion

            #region UserLimit

            rateLimiterOptions.AddPolicy(RateLimitingOptions.PolicyNames.UserLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.GetId(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.UserPolicy.PermitLimit,
                        Window = TimeSpan.FromSeconds(settings.UserPolicy.WindowInSeconds)
                    }
                )
            );

            #endregion

            #region FixedWindow

            rateLimiterOptions.AddFixedWindowLimiter(RateLimitingOptions.PolicyNames.Fixed, options =>
            {
                options.QueueLimit = settings.FixedWindow.QueueLimit;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

                options.PermitLimit = settings.FixedWindow.PermitLimit;
                options.Window = TimeSpan.FromSeconds(settings.FixedWindow.WindowInSeconds);
            });

            #endregion
        });

        return services;
    }

    #endregion

    #region HealthCheck

    private static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(name: "Database", connectionString: configuration.GetConnectionString("DefaultConnection")!)
            .AddHangfire(options => { options.MinimumAvailableServers = 1; })
            .AddCheck<MailProviderHealthCheck>(name: "Mail service");

        return services;
    } 

    #endregion
}