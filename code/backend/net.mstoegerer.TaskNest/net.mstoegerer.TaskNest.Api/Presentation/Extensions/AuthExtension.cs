using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using net.mstoegerer.TaskNest.Api.Domain.Configs;

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
}