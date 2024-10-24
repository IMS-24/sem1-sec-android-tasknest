using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class MetaDataEntityTypeConfiguration : IEntityTypeConfiguration<MetaData>
{
    public void Configure(EntityTypeBuilder<MetaData> builder)
    {
        builder
            .ToTable("meta_data");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(entity => entity.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .HasDefaultValueSql("uuid_generate_v4()")
            .IsRequired();

        builder
            .Property(kv => kv.Key)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(kv => kv.Value)
            .IsRequired()
            .HasMaxLength(255);
        //autoincrement order
        builder.Property(entity => entity.Order)
            .ValueGeneratedOnAdd();
    }
}