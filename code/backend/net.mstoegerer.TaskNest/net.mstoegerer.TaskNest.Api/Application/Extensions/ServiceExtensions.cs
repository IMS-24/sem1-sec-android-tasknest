using net.mstoegerer.TaskNest.Api.Application.Services;

namespace net.mstoegerer.TaskNest.Api.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<TodoService>();
        services.AddScoped<AuthService>();
        services.AddScoped<UserService>();
        services.AddScoped<AttachmentService>();
        services.AddScoped<EvilService>();
        return services;
    }
}