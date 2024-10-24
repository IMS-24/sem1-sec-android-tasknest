using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("user");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(entity => entity.Id)
            .IsRequired();

        builder
            .Property(entity => entity.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(entity => entity.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(entity => entity.Password)
            .HasMaxLength(200)
            .IsRequired();


        builder
            .Property(entity => entity.CreatedUtc)
            .IsRequired();

        builder
            .Property(entity => entity.UpdatedUtc)
            .IsRequired();
        /*builder
            .Property(entity => entity.Token)
            .HasMaxLength(200)
            .IsRequired(false);

        builder
            .Property(entity => entity.TokenValidUntilUtc)
            .IsRequired(false);*/
        builder
            .HasMany(x => x.Todos)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder
            .HasMany(x => x.UploadedAttachments)
            .WithOne(x => x.UploadedBy)
            .HasForeignKey(x => x.UploadedById);

        builder
            .HasMany(x => x.ProvidedShares)
            .WithOne(x => x.SharedBy)
            .HasForeignKey(x => x.SharedById);

        builder
            .HasMany(x => x.ReceivedShares)
            .WithOne(x => x.SharedWith)
            .HasForeignKey(x => x.SharedWithId);

        builder
            .HasMany(x => x.MetaDataAssociation)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder
            .HasMany(u => u.MetaDataAssociation)
            .WithOne(md => md.User)
            .HasForeignKey(md => md.UserId);

        builder
            .HasData(new User
            {
                Email = "admin@tasknest.com",
                Name = "Admin",
                Password = "admin",
                CreatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                Id = Guid.Parse("23d8d722-4037-466c-a68f-98e90e9ba66b")
            });
    }
}