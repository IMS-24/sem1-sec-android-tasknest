using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;

namespace net.mstoegerer.TaskNest.Api.Presentation.Middlewares;

public static class CurrentUser
{
    public static string ExternalUserId { get; set; } = null!;
    public static Guid UserId { get; private set; } = Guid.Empty;
    public static bool IsAdmin { get; private set; }


    public static void SetUser(Guid userId, string externalUserId, bool isAdmin = false)
    {
        UserId = userId;
        ExternalUserId = externalUserId;
        IsAdmin = isAdmin;
    }

    public static void Clear()
    {
        UserId = Guid.Empty;
        ExternalUserId = null!;
        IsAdmin = false;
    }
}

public class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext,
        ILogger<CurrentUserMiddleware> logger)
    {
        // Clear the static user data to avoid stale information
        CurrentUser.Clear();

        var authorizationHeader = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            logger.LogDebug("Token: {Token}", token);
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                logger.LogDebug("JwtToken: {Token}", jwtToken);
                var externalUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(externalUserId))
                {
                    logger.LogDebug("Retry getting sub");
                    externalUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                }

                if (string.IsNullOrEmpty(externalUserId)) return;
                logger.LogDebug("ExternalUserId: {ExternalUserId}", externalUserId);
                var user = dbContext.Users.FirstOrDefault(x => x.ExternalId == externalUserId);
                logger.LogDebug("User: {User}", user?.Id);
                if (user == null) return;

                var isAdmin = user.Email.Contains("pepe")
                              || user.Email.Contains("alois.vollmaier")
                              || user.Email.Contains("alois.vollm@gmail.com")
                              || user.Email.Contains("markus.stoegerer")
                    ;
                if (!string.IsNullOrEmpty(externalUserId)) CurrentUser.SetUser(user.Id, externalUserId, isAdmin);
            }
            catch
            {
                logger.LogInformation("Failed to parse token");
                // Log token parsing errors if necessary
                // Optionally handle invalid token scenarios
            }
        }

        await next(context);
    }
}

public static class CurrentUserMiddlewareExtensions
{
    public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CurrentUserMiddleware>();
    }
}