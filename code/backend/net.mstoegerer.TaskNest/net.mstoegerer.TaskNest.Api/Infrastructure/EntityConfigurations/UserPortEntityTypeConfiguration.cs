using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class UserPortEntityTypeConfiguration : IEntityTypeConfiguration<UserPort>
{
    public void Configure(EntityTypeBuilder<UserPort> builder)
    {
        builder
            .ToTable("user_port");

        builder.HasKey(x => x.Id);

        builder
            .Property(entity => entity.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .HasDefaultValueSql("uuid_generate_v4()")
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .IsRequired();
    }
}