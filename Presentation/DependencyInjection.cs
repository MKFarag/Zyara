using Presentation.OpenApiTransformations;
using Infrastructure;
using Application;
using Presentation.Abstraction;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddInfrastructureDependencies(configuration);
        services.AddApplicationDependencies();
        services.AddExceptionHandlerConfig();

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

    #region



    #endregion

    #region OpenApi

    private static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        return services;
    }

    #endregion
}
