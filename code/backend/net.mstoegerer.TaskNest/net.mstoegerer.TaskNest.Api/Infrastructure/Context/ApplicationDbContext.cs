using System.Globalization;
using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoShare> TodoShares { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<UserMetaData> UserMetaData { get; set; }
    public DbSet<MetaData> MetaData { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    public DbSet<UserPort> UserPorts { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
#if DEBUG
        optionsBuilder.EnableDetailedErrors();
#endif
        optionsBuilder.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}