using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddDatabaseConfig(configuration);

        return services;
    }

    #region Database

    private static IServiceCollection AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("DefaultConnection is not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
        );

        return services;
    }

    #endregion
}
