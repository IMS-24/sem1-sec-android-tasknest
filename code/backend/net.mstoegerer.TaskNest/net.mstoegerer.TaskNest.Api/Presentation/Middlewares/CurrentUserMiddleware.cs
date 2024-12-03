using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace net.mstoegerer.TaskNest.Api.Presentation.Middlewares;

public static class CurrentUser
{
    public static string UserId { get; private set; }
    public static string UserName { get; private set; }
    public static string Email { get; private set; }

    public static void SetUser(string userId, string userName, string email)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
    }

    public static void Clear()
    {
        UserId = null;
        UserName = null;
        Email = null;
    }
}

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Clear the static user data to avoid stale information
        CurrentUser.Clear();

        var authorizationHeader = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (!string.IsNullOrEmpty(userId)) CurrentUser.SetUser(userId, userName, email);
            }
            catch
            {
                // Log token parsing errors if necessary
                // Optionally handle invalid token scenarios
            }
        }

        await _next(context);
    }
}

public static class CurrentUserMiddlewareExtensions
{
    public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CurrentUserMiddleware>();
    }
}