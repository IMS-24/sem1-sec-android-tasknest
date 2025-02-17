using Microsoft.EntityFrameworkCore;
using net.mstoegerer.TaskNest.Api.Domain.DTOs;
using net.mstoegerer.TaskNest.Api.Domain.Entities;
using net.mstoegerer.TaskNest.Api.Infrastructure.Context;
using net.mstoegerer.TaskNest.Api.Presentation.Middlewares;

namespace net.mstoegerer.TaskNest.Api.Application.Services;

public class AttachmentService(ApplicationDbContext dbContext, ILogger<AttachmentService> logger)
{
    public async Task<AttachmentDto> CreateAttachmentAsync(CreateAttachmentDto attachmentDto)
    {
        logger.LogInformation("Create attachment {@Attachment}", attachmentDto);
        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            Name = attachmentDto.Name,
            FileName = attachmentDto.FileName,
            ContentType = attachmentDto.ContentType,
            Data = attachmentDto.Data,
            Size = attachmentDto.Data.Length,
            TodoId = attachmentDto.TodoId,
            UploadedById = CurrentUser.UserId,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow
        };
        dbContext.Attachments.Add(attachment);
        await dbContext.SaveChangesAsync();
        return new AttachmentDto
        {
            Id = attachment.Id,
            Name = attachment.Name,
            FileName = attachment.FileName,
            ContentType = attachment.ContentType,
            Data = attachment.Data,
            Size = attachment.Size,
            TodoId = attachment.TodoId,
            UploadedById = attachment.UploadedById,
            CreatedUtc = attachment.CreatedUtc,
            UpdatedUtc = attachment.UpdatedUtc
        };
    }

    public async Task<AttachmentDto> GetAttachmentAsync(Guid id)
    {
        logger.LogInformation("Get attachment {Id}", id);
        var attachment = await dbContext.Attachments.FirstOrDefaultAsync(x => x.Id == id);
        if (attachment == null) throw new Exception("Attachment not found");
        return new AttachmentDto
        {
            Id = attachment.Id,
            Name = attachment.Name,
            FileName = attachment.FileName,
            ContentType = attachment.ContentType,
            Data = attachment.Data,
            Size = attachment.Size,
            TodoId = attachment.TodoId,
            UploadedById = attachment.UploadedById,
            CreatedUtc = attachment.CreatedUtc,
            UpdatedUtc = attachment.UpdatedUtc
        };
    }
}