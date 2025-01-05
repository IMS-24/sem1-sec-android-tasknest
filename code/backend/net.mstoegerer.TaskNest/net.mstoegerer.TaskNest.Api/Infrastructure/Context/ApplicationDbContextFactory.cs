using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Context;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        try
        {
            IConfiguration configuration = new ConfigurationManager()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Presentation/appsettings.Development.json", true)
                //.AddJsonFile("Presentation/appsettings.Production.json", true)
                .AddEnvironmentVariables()
                .Build();
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");
            optionsBuilder.UseNpgsql(connectionString, o => o.UseNetTopologySuite());
            return new ApplicationDbContext(optionsBuilder.Options);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Could not load Configuration");
            Console.Error.WriteLine(e);
            throw;
        }
    }
}