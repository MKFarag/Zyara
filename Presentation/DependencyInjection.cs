#region Usings

using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Identities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

#endregion

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddInfrastructureDependencies(configuration);
        services.AddAuthConfig(configuration);

        services
            .AddEndpointsApiExplorer()
            .AddOpenApi();

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

        //services.AddScoped<IAuthService, AuthService>();
        //services.AddScoped<IRoleService, RoleService>();
        //services.AddScoped<ISignInService, SignInService>();

        #endregion

        return services;
    }

    #endregion
}
