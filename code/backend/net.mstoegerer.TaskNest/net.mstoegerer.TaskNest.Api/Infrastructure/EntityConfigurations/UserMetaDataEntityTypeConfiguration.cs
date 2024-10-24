using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class UserMetaDataEntityTypeConfiguration : IEntityTypeConfiguration<UserMetaData>
{
    public void Configure(EntityTypeBuilder<UserMetaData> builder)
    {
        builder.ToTable("user_metadata");
        builder.HasKey(x => x.Id);

        builder.Property(entity => entity.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .HasDefaultValueSql("uuid_generate_v4()")
            .IsRequired();

        builder.Property(entity => entity.UserId)
            .IsRequired();

        builder.Property(md => md.Location)
            .HasColumnType("geometry")
            .IsRequired(false);

        builder.HasIndex(e => e.Location)
            .HasMethod("GIST");


        builder
            .HasMany(md => md.MetaData)
            .WithOne(kv => kv.UserMetaData)
            .HasForeignKey(kv => kv.UserMetaDataId)
            ;
        builder.Property(x => x.Password).IsRequired(false);
    }
}