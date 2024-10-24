using net.mstoegerer.TaskNest.Api.Application.Services;

namespace net.mstoegerer.TaskNest.Api.Presentation.Middlewares;

public class TokenValidationMiddleware(RequestDelegate next, UserService userService, JwtService jwtService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        /*
        // Fetch the token from the Authorization header
        if (!context.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization header missing.");
            return;
        }

        // Extract the token (assuming Bearer scheme)
        var token = tokenHeader.ToString().Replace("PepeToken ", "", StringComparison.OrdinalIgnoreCase).Trim();

        // Validate the token
        var user = await userService.GetUserByTokenAsync(token); // Fetch the user associated with the token
        if (user == null || !await jwtService.VerifyTokenAsync(user))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or expired token.");
            return;
        }
        */

        // Continue to the next middleware if token is valid
        await next(context);
    }
}