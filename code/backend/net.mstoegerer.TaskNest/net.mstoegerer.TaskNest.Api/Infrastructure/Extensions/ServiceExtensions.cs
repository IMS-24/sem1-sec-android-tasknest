using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using Serilog;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration, bool isDevelopment)
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
                    });
                if (isDevelopment)
                    options.EnableSensitiveDataLogging()
                        .LogTo(Log.Information, LogLevel.Information);
            }
        );
        return services;
    }
}