using Presentation.OpenApiTransformations;
using Infrastructure;
using Application;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddInfrastructureDependencies(configuration);
        services.AddApplicationDependencies();

        services
            .AddEndpointsApiExplorer()
            .AddOpenApiConfig();

        return services;
    }

    #region OpenApi

    private static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        return services;
    }

    #endregion
}
