﻿namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();



        services
            .AddEndpointsApiExplorer()
            .AddOpenApi();

        return services;
    }
}
