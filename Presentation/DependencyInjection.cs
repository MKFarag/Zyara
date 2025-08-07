using Application;
using Application.Interfaces.Infrastructure;
using Domain.Settings;
using Hangfire;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Presentation.Abstraction;
using Presentation.OpenApiTransformations;

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
        services.AddMailConfig(configuration);

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

    #region OpenApi

    private static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        return services;
    }

    #endregion
}
