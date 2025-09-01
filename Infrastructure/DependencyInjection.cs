﻿#region Usings

using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

#endregion

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("DefaultConnection is not found in appsettings.json");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddMailConfig(configuration);

        return services;
    }

    private static IServiceCollection AddMailConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailTemplateOptions>().Bind(configuration.GetSection(nameof(EmailTemplateOptions)));

        services.AddOptions<MailSettings>()
            .Bind(configuration.GetSection(nameof(MailSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();

        return services;
    }
}
