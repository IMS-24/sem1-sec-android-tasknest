using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using net.mstoegerer.TaskNest.Api.Domain.Entities;

namespace net.mstoegerer.TaskNest.Api.Infrastructure.EntityConfigurations;

public class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder
            .ToTable("attachment");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(entity => entity.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .HasDefaultValueSql("uuid_generate_v4()")
            .IsRequired();

        builder
            .Property(x => x.FileName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.ContentType)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Size)
            .IsRequired();

        builder
            .Property(x => x.Data)
            .IsRequired();

        builder
            .Property(x => x.CreatedUtc)
            .IsRequired();

        builder
            .Property(x => x.UpdatedUtc)
            .IsRequired();
        builder
            .Property(x => x.UploadedById)
            .IsRequired();
        builder
            .HasOne<User>(x => x.UploadedBy)
            .WithMany(user => user.UploadedAttachments)
            .HasForeignKey(x => x.UploadedById);

        builder
            .HasOne(x => x.Todo)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.TodoId);

        builder
            .HasData(new List<Attachment>
            {
                new()
                {
                    Name = "Pepe",
                    Id = Guid.Parse("d5bfdef9-331d-4162-bf50-f3e43f699499"),
                    FileName = "test.txt",
                    ContentType = "text/plain",
                    Size = 100,
                    Data = [0x00, 0x01, 0x02],
                    CreatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    UploadedById = Guid.Parse("23d8d722-4037-466c-a68f-98e90e9ba66b"),
                    TodoId = Guid.Parse("e832db47-e640-4539-825b-b3940ff882d9")
                }
            });
    }
}