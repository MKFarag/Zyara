#region Usings

using Infrastructure; 

#endregion

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddInfrastructureDependencies(configuration);


        services
            .AddEndpointsApiExplorer()
            .AddOpenApi();

        return services;
    }
}
