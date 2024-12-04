using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using net.mstoegerer.TaskNest.Api.Domain.Configs;
using net.mstoegerer.TaskNest.Api.Presentation.AuthorizationFilter;

namespace net.mstoegerer.TaskNest.Api.Presentation.Extensions;

public static class AuthExtension
{
    public static IServiceCollection AddAuth0(this IServiceCollection services, Auth0Config config)
    {
        var domain = $"https://{config.Domain}/";
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = config.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddApiKey(this IServiceCollection services, Auth0Config config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config), "Auth0Config cannot be null.");

        if (string.IsNullOrWhiteSpace(config.ApiKey))
            throw new ArgumentException("The API key in Auth0Config cannot be null or empty.", nameof(config.ApiKey));

        services.AddSingleton<ApiKeyAuthorizationFilter>();

        services.AddSingleton<IApiKeyValidator, ApiKeyValidator>(_ => new ApiKeyValidator(config.ApiKey));

        return services;
    }
}