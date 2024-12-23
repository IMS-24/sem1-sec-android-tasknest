using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using NetTopologySuite.Geometries;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class TodoEntityTypeConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder
            .ToTable("todo");
        builder
            .HasKey(x => x.Id);

        builder
            .Property(entity => entity.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .HasDefaultValueSql("uuid_generate_v4()")
            .IsRequired();

        builder
            .Property(entity => entity.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(entity => entity.Content)
            .HasMaxLength(2000)
            .IsUnicode();

        builder
            .Property(entity => entity.CreatedUtc)
            .IsRequired();

        builder
            .Property(entity => entity.UpdatedUtc)
            .IsRequired();

        builder
            .Property(entity => entity.Status)
            .HasMaxLength(20)
            .HasDefaultValue("new")
            .IsRequired();

        builder
            .Property(entity => entity.DueUtc)
            .IsRequired(false);

        builder.Property(md => md.Location)
            .HasColumnType("geometry(Point, 4326)");

        builder
            .Property(entity => entity.UserId)
            .IsRequired();

        builder
            .HasOne<User>(entity => entity.User)
            .WithMany(user => user.Todos)
            .HasForeignKey(entity => entity.UserId)
            ;

        builder
            .HasOne<User>(entity => entity.AssignedTo)
            .WithMany(user => user.AssignedTodos)
            .HasForeignKey(entity => entity.AssignedToId)
            .IsRequired(false);

        builder
            .HasMany<TodoShare>(entity => entity.Shares)
            .WithOne(share => share.Todo)
            .HasForeignKey(share => share.TodoId);

        builder
            .HasData(new List<Todo>
            {
                new()
                {
                    Content = "<ol><li>First</li><li>Second</li></ol>",
                    CreatedUtc = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedUtc = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Title = "First Todo",
                    Id = Guid.Parse("e832db47-e640-4539-825b-b3940ff882d9"),
                    UserId = Guid.Parse("23d8d722-4037-466c-a68f-98e90e9ba66b"),
                    AssignedToId = Guid.Parse("23d8d722-4037-466c-a68f-98e90e9ba66b"),
                    Location = new Point(15.4395, 47.0707) { SRID = 4326 }
                }
            });
    }
}