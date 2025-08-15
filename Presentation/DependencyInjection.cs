#region Usings

using Application;
using Application.Interfaces.Infrastructure;
using Application.Services;
using Domain.Entities;
using Domain.Settings;
using Hangfire;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using Presentation.Abstraction;
using Presentation.OpenApiTransformations;
using System.Text; 

#endregion

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddInfrastructureDependencies(configuration);
        services.AddApplicationDependencies();
        services.AddExceptionHandlerConfig();
        services.AddHangfireConfig(configuration);
        services.AddHttpContextAccessor();
        services.AddCORSConfig(configuration);
        services.AddMailConfig(configuration);
        services.AddAuthConfig(configuration);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddEndpointsApiExplorer()
            .AddOpenApiConfig();

        return services;
    }

    #region ExceptionHandler

    private static IServiceCollection AddExceptionHandlerConfig(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

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

    #region Mail

    private static IServiceCollection AddMailConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailTemplateOptions>()
            .Bind(configuration.GetSection(nameof(EmailTemplateOptions)));

        services.AddOptions<MailSettings>()
            .Bind(configuration.GetSection(nameof(MailSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();

        return services;
    }

    #endregion

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
        //services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ISignInService, SignInService>();

        #endregion

        return services;
    }

    #endregion

    #region OpenApi

    private static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        return services;
    }

    #endregion
}
