using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class TodoShareEntityTypeConfiguration : IEntityTypeConfiguration<TodoShare>
{
    public void Configure(EntityTypeBuilder<TodoShare> builder)
    {
        builder
            .ToTable("todo_share");
        builder
            .HasKey(x => x.Id);

        builder
            .Property(entity => entity.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .HasDefaultValueSql("uuid_generate_v4()")
            .IsRequired();

        builder
            .Property(entity => entity.TodoId)
            .IsRequired();

        builder
            .HasOne<Todo>(x => x.Todo)
            .WithMany(x => x.Shares)
            .HasForeignKey(x => x.TodoId);

        builder
            .Property(entity => entity.SharedById)
            .IsRequired();

        builder
            .HasOne<User>(x => x.SharedBy)
            .WithMany(x => x.ProvidedShares)
            .HasForeignKey(x => x.SharedById);


        builder
            .Property(entity => entity.SharedWithId)
            .IsRequired();

        builder
            .HasOne<User>(x => x.SharedWith)
            .WithMany(x => x.ReceivedShares)
            .HasForeignKey(x => x.SharedWithId);
    }
}