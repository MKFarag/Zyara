#region Usings

using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

#endregion

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMapsterConfig();

        return services;
    }

    #region Mapster

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var MappingConfig = TypeAdapterConfig.GlobalSettings;
        MappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(MappingConfig));

        return services;
    }

    #endregion
}