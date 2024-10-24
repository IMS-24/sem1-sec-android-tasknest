using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(connectionString, o =>
                    {
                        o.UseNetTopologySuite();
                        //o.EnableDynamicJson();
                    })
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
        );
        return services;
    }
}